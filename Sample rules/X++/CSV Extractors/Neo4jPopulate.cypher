// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

// You may want to run neo4j locally or in a container. If you run it with docker locally, you can
// use the following command to start the container
//docker run  --publish=7474:7474 --publish=7687:7687 --memory=3G -ti ^
//   -v "c:/neoproblem/data:/data" ^
//   -v "c:/neoproblem/logs:/logs" ^
//   -v "c:/neoproblem/conf:/var/lib/neo4j/conf" ^
//   -v "c:/neoproblem/import:/var/lib/neo4j/import" ^
//   --env NEO4J_AUTH=neo4j/test ^
//   neo4j:latest
// If you are running the container in the azure cloud you will need to create fileshares to files to import.
// You can start the container with this Azure command:
// az group deployment create --resource-group <resourceGroup> --template-file <template>. You can find details
// on this template file in https://docs.microsoft.com/en-us/azure/container-instances/container-instances-reference-yaml
// (for YAML) and here https://docs.microsoft.com/en-us/azure/container-instances/container-instances-multi-container-group (for JSON).

// You will want to increment the Heap size by setting the
// dbms.memory.heap.max_size=4G
// in the configuration file (neo4j.conf) or in the environment variables
// dbms_memory_heap_max__size = 4G (note the double underscores)

// Note that Neo4j implements an annoying feature where it restricts the directories from which
// it can load CSV files to be directories under the neo4j root. To get rid of this, you have
// to change this in the settings:
// # dbms.directories.import=import
// dbms.security.allow_csv_import_from_file_urls=true
// or in the environment variable dbms_security_allow__csv__import__from__file__urls = true

// This script can be executed interactively in the browser, or using the cypher shell (https://neo4j.com/docs/operations-manual/current/tools/cypher-shell/).
// If the browser is used, you will want to set the option that allows multiple semicolon separated
// queries to be executed. Otherwise, you can only run one at the time. The cypher-shell tool can be
// invoked like this (on windows)
// bin\cypher-shell -p neo4j -u neo4j  <Populate.cypher
//
// If you want to run this with the Neo4j running in a container, you will need to attach to the container
// Azure:
//     az container exec  --resource-group <resourcegroup>  --container-name <containername> --name <groupname> --exec-command "bin\cypher-shell -p neo4j -u neo4j <Populate.cypher"
// Locally:
//    docker exec -it <containernumber> bin\cypher-shell -p neo4j -u neo4j  <import\Populate.cypher

// Every class construct (i.e. that contains methods and or fields etc) is
// represented as a Class node. Therefore, classes and tabular objects (that share the
// same namespace) are all called Class with a given unique name.
// There may be an artifact field as well, or other indication of the type of class.
// Forms and queries have nodes that basically serve to hold metadata (number of
// datasources etc.) and have a pointer to the class implementing the behavior. Note
// that this works only because the class names are disambiguated with $Form_ and $Query_
// Therefore: All Names on Classes are unique.

// The directory containing the CSV files is designated by the $EXPORTDIRECTORY parameter. Use
// the :PARAM in the browser or on the commandline for cypher-shell to set the correct directory.
// :PARAM EXPORTDIRECTORY  => "file:///C:/users/johndoe/desktop/"
:PARAM EXPORTDIRECTORY  => "file:///";

CREATE CONSTRAINT ON (a:Package) ASSERT a.Name IS UNIQUE;

CREATE INDEX ON :Class(Name);
CREATE INDEX ON :Class(Artifact);

CREATE INDEX ON :Interface(Name);
CREATE INDEX ON :Interface(Artifact);

CREATE INDEX ON :Table(Name);
CREATE INDEX ON :Table(Artifact);

CREATE INDEX ON :Form(Name);
CREATE INDEX ON :Form(Artifact);

CREATE INDEX ON :Query(Name);
CREATE INDEX ON :Query(Artifact);

CREATE INDEX ON :View(Name);
CREATE INDEX ON :View(Artifact);

// Create the packages.
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM $EXPORTDIRECTORY + "ExtractPackagesCSV.csv" AS exts
CREATE (p:Package {Name: exts.Package, Description: exts.Description, ModelId: exts.ModelId});
MATCH(p:Package) RETURN COUNT(p) + " packages";

// Create the reference relationships between the packages
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractPackageDependenciesCSV.csv" AS exts
MERGE (p:Package{Name: exts.Package})
MERGE (ref:Package {Name: exts.References})
CREATE (p)-[:REFERENCES]->(ref);
MATCH(p:Package)-[r:REFERENCES]->(ref:Package) RETURN COUNT(r) + " package references";

// Create the classes.
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractClassmetricsCSV.csv" AS exts
CREATE (c:Class {Artifact: exts.Artifact, Name: exts.Name, Package: exts.Package,
                 IsAbstract: toBoolean(exts.IsAbstract), IsFinal: toBoolean(exts.IsFinal), IsStatic: toBoolean(exts.IsStatic),
                 NOAM: toInteger(exts.NOAM), LOC: toInteger(exts.LOC), NOM: toInteger(exts.NOM), NOA: toInteger(exts.NOA),
                 WMC: toInteger(exts.WMC), NOPM: toInteger(exts.NOPM), NOPA: toInteger(exts.NOPA), NOS: toInteger(exts.NOS) })
MERGE (p:Package{Name: exts.Package})
CREATE (p)-[:CONTAINS]->(c);
MATCH(c:Class) RETURN COUNT(c) + " classes";

// Class inheritance information.
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractclassinheritanceCSV.csv"  AS exts
MATCH(base:Class {Artifact: exts.Artifact})
MATCH(super:Class {Name: exts.Extends})
MERGE (base) -[:EXTENDS] -> (super);
MATCH(c:Class) -[r:EXTENDS] -> (s:Class) RETURN COUNT(r) + " class extensions";

// Class fields
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractclassfieldsCSV.csv" AS exts
CREATE (f:ClassMember { Name: exts.Member, Type: exts.Type, Visibility: exts.Visibility})
MERGE (c:Class{Artifact: exts.Artifact})
MERGE (c) -[:FIELD]-> (f);
MATCH(c:Class)-[:FIELD]-(f:ClassMember) RETURN COUNT(f) + " class members";

// Class methods
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractClassmethodsCSV.csv" AS exts
MATCH (c:Class { Artifact: exts.Artifact })
CREATE (m:Method {Name: exts.Method,
          IsStatic: toBoolean(exts.IsStatic), IsFinal: toBoolean(exts.IsFinal), IsAbstract: toBoolean(exts.IsAbstract),
          Visibility: exts.Visibility, CMP: toInteger(exts.CMP), LOC: toInteger(exts.LOC), NOS: toInteger(exts.NOS) })
MERGE (c)-[:DECLARES]-> (m);
MATCH(c:Class)-[:DECLARES]-(m:Method) RETURN COUNT(m) + " class methods";

// attributes on class methods.
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractClassMethodAttributesCSV.csv" AS exts
MATCH (c:Class {Name: exts.Class}) -[:DECLARES]->(m:Method { Name: exts.Method})
MATCH (a:Class) WHERE ((a.Name = exts.Attribute) or (a.Name = exts.Attribute + "attribute"))
MERGE (m)-[:HASATTRIBUTE]-> (a);
MATCH (c:Class) -[:DECLARES]-> (m:Method) -[r:HASATTRIBUTE]-(a:Class) RETURN COUNT(a) + " method attributes";

// Class delegates
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractClassDelegatesCSV.csv" AS exts
MATCH (c:Class { Artifact: exts.Artifact })
CREATE (m:Delegate {Name: exts.Name })
MERGE (c)-[:DECLARES]-> (m);
MATCH (c:Class) -[:DECLARES]-(m:Delegate) RETURN COUNT(m) + " class delegates";

// Class attributes
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractClassAttributesCSV.csv" AS exts
MATCH (c:Class { Name: exts.Artifact })
MATCH (a:Class) WHERE ((a.Name = exts.Name) or (a.Name = exts.Name + "attribute"))
MERGE (c)-[:HASATTRIBUTE]-> (a);
MATCH(c:Class) -[r:HASATTRIBUTE]-> (a:Class) RETURN COUNT(r) + " class attributes";

// Interfaces
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractinterfacesCSV.csv" AS exts
MATCH(p:Package {Name: exts.Package})
CREATE (m:Interface {Artifact: exts.Artifact, Name: exts.Name })
MERGE (p) -[:CONTAINS]-> (m);
MATCH (m:Interface) RETURN COUNT(m) + " interfaces";

// Interface methods
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractInterfaceMethodsCSV.csv" AS exts
MATCH (c:Interface { Artifact: exts.Artifact })
CREATE (m:Method {Name: exts.Method })
MERGE (c)-[:DECLARES]-> (m);
MATCH (c:Interface) -[:DECLARES]-> (m:Method) RETURN COUNT(m) + " interface methods";

// Interface inheritance
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractInterfaceinheritanceCSV.csv" AS exts
MATCH(super:Interface {Artifact: exts.Artifact})
MATCH(base:Interface {Name: exts.Extends})
MERGE (super) -[:EXTENDS]-> (base);
MATCH (s:Interface)-[r:EXTENDS]->(b:Interface) RETURN count(r) + " interface extensions";

// Attributes on interfaces
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractInterfaceAttributesCSV.csv" AS exts
MATCH (c:Interface { Name: exts.Interface})
MATCH (a:Class) WHERE ((a.Name = exts.Attribute) or (a.Name = exts.Attribute + "attribute"))
MERGE (c)-[:HASATTRIBUTE]-> (a);
MATCH (c:Interface)-[r:HASATTRIBUTE]->(a:Class) RETURN count(r) + " interface attributes";

// Class interface implementation
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractClassInterfaceImplementationsCSV.csv" AS exts
MATCH(c:Class {Artifact: exts.Artifact})
MATCH(i:Interface {Name: exts.Implements})
MERGE (c) -[:IMPLEMENTS]-> (i);
MATCH (c:Class)-[r:IMPLEMENTS]->(i:Interface) RETURN count(r) + " interface implementations";

// Create the tables. First create the metadata artifact and then the class
// that implements the functionality. The names are unique across classes and tables.

// Create tables (metadata part)
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractTablesCSV.csv" AS exts
CREATE (t:Table {Artifact: exts.Artifact, Name: exts.Name, Package: exts.Package,
                 Label: exts.Label, SystemTable: exts.SystemTable, SaveDataPerPartition: exts.SaveDataPerPartition,
                 ClusteredIndex: exts.ClusteredIndex, TableGroup: exts.TableGroup})
MERGE (p:Package{Name: exts.Package})
MERGE (p)-[:CONTAINS]->(t);
MATCH(t:Table) RETURN COUNT(t) + " tables";

// Create the table fields
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractTableFieldsCSV.csv" AS exts
MATCH (t:Table{Name: exts.TableName})
CREATE (f:TableField{Name: exts.FieldName, Type: exts.Type, Label: exts.Label,
                     Mandatory: toBoolean(exts.Mandatory), Visible: toBoolean(exts.Visible), EDT: exts.ExtendedDataType})
MERGE(t)-[:HASFIELD]->(f);
MATCH(t:Table)-[:HASFIELD]->(f:TableField) RETURN COUNT(f) + " table fields";

// Create table classes linking to tables created above.
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractTableMetricsCSV.csv" AS exts
CREATE (c:Class {Artifact: exts.Artifact, Name: exts.Name,
                 LOC: toInteger(exts.LOC), NOM: toInteger(exts.NOM), WMC: toInteger(exts.WMC), NOPM: toInteger(exts.NOPM), NOS: toInteger(exts.NOS) })
WITH c, exts
MATCH (t:Table {Artifact: exts.Artifact})
MERGE (t) -[:BEHAVIOR]-> (c);
MATCH(t:Table)-[r:BEHAVIOR]->(c:Class) RETURN COUNT(r) + " table behaviors";

// Extract the methods on the tables
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractTableMethodsCSV.csv" AS exts
MATCH (t:Table{Name: exts.Name}) -[:BEHAVIOR]-> (c:Class)
CREATE (m:Method {Name: exts.Method,
                  IsAbstract: toBoolean(exts.IsAbstract), IsFinal: toBoolean(exts.IsFinal), IsStatic: toBoolean(exts.IsStatic),
                  Visibility: exts.Visibility, CMP: toInteger(exts.CMP), LOC: toInteger(exts.LOC), NOS: toInteger(exts.NOS) })
MERGE (c)-[:DECLARES]->(m);
MATCH (t:Table) -[:BEHAVIOR]-> (c:Class) -[r:DECLARES]-> (m:Method) RETURN COUNT(m) + " table methods";

// attributes on table methods.
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractTableMethodAttributesCSV.csv" AS exts
MATCH (t:Table {Name: exts.Table}) -[:BEHAVIOR]-> (c:Class) -[:DECLARES]->(m:Method { Name: exts.Method})
MATCH (a:Class) WHERE ((a.Name = exts.Attribute) or (a.Name = exts.Attribute + "attribute"))
MERGE (m) -[:HASATTRIBUTE]-> (a);
MATCH (t:Table) -[:BEHAVIOR]-> (c:Class) -[:DECLARES]-> (m:Method) -[r:HASATTRIBUTE]-> (a:Class) RETURN COUNT(r) + " table method attributes";

// TODO: Views. ExtractViews written.

// Create the forms metadata artifact
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractFormsCSV.csv" AS exts
CREATE (t:Form {Artifact: exts.Artifact, Name: exts.Name })
MERGE (p:Package{Name: exts.Package})
MERGE (p)-[:CONTAINS]->(t);
MATCH (f:Form) RETURN COUNT(f) + " forms";

// Create forms classes, linking to the form artifact
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractFormMetricsCSV.csv" AS exts
CREATE (c:Class {Artifact: exts.Artifact, Name: exts.Name,
        LOC: toInteger(exts.LOC), NOM: toInteger(exts.NOM), WMC: toInteger(exts.WMC), NOPM: toInteger(exts.NOPM), NOS: toInteger(exts.NOS) })
MERGE (f:Form{Artifact: exts.Artifact})
MERGE (f)-[:BEHAVIOR]->(c);
MATCH (f:Form) -[r:BEHAVIOR]-> (c:Class) RETURN count(r) + " form behaviors";

// Create forms methods. These are wired up to the classes implementing form behavior
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractFormsMethodsCSV.csv" AS exts
MATCH (f:Form{ Artifact: exts.Artifact }) -[:BEHAVIOR] ->(c:Class)
CREATE (m:Method {Name: exts.Name, IsAbstract: toBoolean(exts.IsAbstract), IsStatic: toBoolean(exts.IsStatic), IsFinal: toBoolean(exts.IsFinal),
                  Visibility: exts.Visibility, CMP: toInteger(exts.CMP), LOC: toInteger(exts.LOC), NOS: toInteger(exts.NOS) })
MERGE (c)-[:DECLARES]-> (m);
MATCH (f:Form) -[:BEHAVIOR]-> (c:Class) -[r:DECLARES]-> (m:Method) RETURN COUNT(m) + " form methods";

// attributes on forms methods.
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractFormMethodAttributesCSV.csv" AS exts
MATCH (f:Form {Name: exts.Form}) -[:BEHAVIOR]-> (c:Class)-[:DECLARES]->(m:Method { Name: exts.Method})
MATCH (a:Class) WHERE ((a.Name = exts.Attribute) or (a.Name = exts.Attribute + "attribute"))
MERGE (m)-[:HASATTRIBUTE]-> (a);
MATCH (f:Form) -[:BEHAVIOR]-> (c:Class) -[:DECLARES]-> (m:Method) -[r:HASATTRIBUTE] -> (a:Class) RETURN COUNT(r) + " form method attributes";

// Create the query metadata artifact
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractQueriesCSV.csv" AS exts
CREATE (t:Query {Artifact: exts.Artifact, Name: exts.Name, Title: exts.Title })
MERGE (p:Package{Name: exts.Package})
CREATE (p)-[:CONTAINS]->(t);
MATCH (t:Query) RETURN COUNT(t) + " queries";

// Create the query classes, linking to the query artifact.
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractQueryMetricsCSV.csv" AS exts
CREATE (c:Class {Artifact: exts.Artifact, Name: exts.Name,
                 LOC: toInteger(exts.LOC), NOM: toInteger(exts.NOM), WMC: toInteger(exts.WMC), NOPM: toInteger(exts.NOPM), NOS: toInteger(exts.NOS) })
MERGE (f:Query{Artifact: exts.Artifact})
CREATE (f)-[:BEHAVIOR]->(c);
MATCH (t:Query) -[r:BEHAVIOR]-> (c:Class) RETURN COUNT(r) + " query behaviors";

// Create query methods.
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractQueryMethodsCSV.csv" AS exts
MATCH (q:Query { Artifact: exts.Artifact }) -[:BEHAVIOR] ->(c:Class)
CREATE (m:Method {Name: exts.Name,
                  IsAbstract: toBoolean(exts.IsAbstract), IsStatic: toBoolean(exts.IsStatic), IsFinal: toBoolean(exts.IsFinal),
                  Visibility: exts.Visibility, CMP: toInteger(exts.CMP), LOC: toInteger(exts.LOC), NOS: toInteger(exts.NOS) })
CREATE (c)-[:DECLARES]-> (m);
MATCH (t:Query) -[:BEHAVIOR]-> (c:Class) -[:DECLARES]-> (m:Method) RETURN COUNT(m) + " query methods";

// Wire up the data sources in the query to their tables.
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractQueryDataSourcesCSV.csv" AS exts
MATCH (q:Query { Artifact: exts.Artifact })
MERGE (t:Table { Name: exts.Table})
CREATE (q)-[:CONSUMES]-> (t);
MATCH (q:Query) -[r:CONSUMES]-> (t:Table) RETURN COUNT(r) + " query table references";
