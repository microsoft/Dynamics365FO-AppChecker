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

CREATE INDEX ON :Method(Name);
CREATE INDEX ON :Method(Package, Artifact, Name);

CREATE INDEX ON :ManagedType(Name);
CREATE INDEX ON :ManagedType(Artifact);
CREATE INDEX ON :ManagedMethod(Name);
CREATE INDEX ON :ManagedMethod(Package, Artifact, Name);

// Create the classes.
return "Creating Classes";
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
return "Creating Class extension";
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractclassinheritanceCSV.csv"  AS exts
MATCH(base:Class {Artifact: exts.Artifact})
MATCH(super:Class {Name: exts.Extends})
MERGE (base) -[:EXTENDS] -> (super);
MATCH(c:Class) -[r:EXTENDS] -> (s:Class) RETURN COUNT(r) + " class extensions";

// Class fields
return "Creating Class fields";
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractclassfieldsCSV.csv" AS exts
CREATE (f:ClassMember { Name: exts.Member, Type: exts.Type, Visibility: exts.Visibility})
MERGE (c:Class{Artifact: exts.Artifact})
MERGE (c) -[:FIELD]-> (f);
MATCH(c:Class)-[:FIELD]-(f:ClassMember) RETURN COUNT(f) + " class members";

// Class methods
return "Creating Class methods";
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractClassmethodsCSV.csv" AS exts
MATCH (c:Class { Artifact: exts.Artifact })
CREATE (m:Method {Name: exts.Method, Package: exts.Package, Artifact: exts.Artifact,
          IsStatic: toBoolean(exts.IsStatic), IsFinal: toBoolean(exts.IsFinal), IsAbstract: toBoolean(exts.IsAbstract),
          Visibility: exts.Visibility, CMP: toInteger(exts.CMP), LOC: toInteger(exts.LOC), NOS: toInteger(exts.NOS) })
MERGE (c)-[:DECLARES]-> (m);
MATCH(c:Class)-[:DECLARES]-(m:Method) RETURN COUNT(m) + " class methods";

// Attributes on class methods.
return "Creating Class method attributes";
USING PERIODIC COMMIT 2000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractClassMethodAttributesCSV.csv" AS exts
MATCH (m:Method { Name: exts.Method, Artifact: "class" + exts.Artifact})
MATCH (a:Class) WHERE ((a.Name = exts.Attribute) or (a.Name = exts.Attribute + "attribute"))
MERGE (m)-[:HASATTRIBUTE]-> (a);
MATCH (c:Class) -[:DECLARES]-> (m:Method) -[r:HASATTRIBUTE]-(a:Class) RETURN COUNT(a) + " method attributes";

// Class delegates
return "Creating Class delegates";
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractClassDelegatesCSV.csv" AS exts
MATCH (c:Class { Artifact: exts.Artifact })
CREATE (m:Delegate {Name: exts.Name })
MERGE (c)-[:DECLARES]-> (m);
MATCH (c:Class) -[:DECLARES]-(m:Delegate) RETURN COUNT(m) + " class delegates";

// Class attributes
return "Creating Class attributes";
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractClassAttributesCSV.csv" AS exts
MATCH (c:Class { Name: exts.Artifact })
MATCH (a:Class) WHERE ((a.Name = exts.Name) or (a.Name = exts.Name + "attribute"))
MERGE (c)-[:HASATTRIBUTE]-> (a);
MATCH(c:Class) -[r:HASATTRIBUTE]-> (a:Class) RETURN COUNT(r) + " class attributes";

// Interfaces
return "Creating interfaces";
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractinterfacesCSV.csv" AS exts
MERGE(p:Package {Name: exts.Package})
CREATE (m:Interface {Artifact: exts.Artifact, Name: exts.Name, Package: exts.Package })
MERGE (p) -[:CONTAINS]-> (m);
MATCH (m:Interface) RETURN COUNT(m) + " interfaces";

// Interface methods
return "Creating interface methods";
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractInterfaceMethodsCSV.csv" AS exts
MATCH (c:Interface { Artifact: exts.Artifact })
CREATE (m:Method {Name: exts.Method, Package: exts.Package, Artifact: exts.Artifact })
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
return "Creating interface attributes";
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractInterfaceAttributesCSV.csv" AS exts
MATCH (c:Interface { Name: exts.Interface})
MATCH (a:Class) WHERE ((a.Name = exts.Attribute) or (a.Name = exts.Attribute + "attribute"))
MERGE (c)-[:HASATTRIBUTE]-> (a);
MATCH (c:Interface)-[r:HASATTRIBUTE]->(a:Class) RETURN count(r) + " interface attributes";

// Class interface implementation
return "Creating interface implenentation";
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractClassInterfaceImplementationsCSV.csv" AS exts
MATCH(c:Class {Artifact: exts.Artifact})
MATCH(i:Interface {Name: exts.Implements})
MERGE (c) -[:IMPLEMENTS]-> (i);
MATCH (c:Class)-[r:IMPLEMENTS]->(i:Interface) RETURN count(r) + " interface implementations";

// Create the tables. First create the metadata artifact and then the class
// that implements the functionality. The names are unique across classes and tables.

// Create tables (metadata part)
return "Creating tables";
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractTablesCSV.csv" AS exts
CREATE (t:Table {Artifact: exts.Artifact, Name: exts.Name, Package: exts.Package,
                 Label: exts.Label, SystemTable: exts.SystemTable, SaveDataPerPartition: exts.SaveDataPerPartition,
                 ClusteredIndex: exts.ClusteredIndex, TableGroup: exts.TableGroup})
MERGE (p:Package{Name: exts.Package})
MERGE (p)-[:CONTAINS]->(t);
MATCH(t:Table) RETURN COUNT(t) + " tables";

// Create the table fields
return "Creating table fields";
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractTableFieldsCSV.csv" AS exts
MATCH (t:Table{Name: exts.TableName})
CREATE (f:TableField{Name: exts.FieldName, Type: exts.Type, Label: exts.Label,
                     Mandatory: toBoolean(exts.Mandatory), Visible: toBoolean(exts.Visible), EDT: exts.ExtendedDataType})
MERGE(t)-[:HASFIELD]->(f);
MATCH(t:Table)-[:HASFIELD]->(f:TableField) RETURN COUNT(f) + " table fields";

// Create table classes linking to tables created above.
return "Creating table behaviors";
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractTableMetricsCSV.csv" AS exts
CREATE (c:Class {Artifact: exts.Artifact, Name: exts.Name, Package: exts.Package,
                 LOC: toInteger(exts.LOC), NOM: toInteger(exts.NOM), WMC: toInteger(exts.WMC), NOPM: toInteger(exts.NOPM), NOS: toInteger(exts.NOS) })
WITH c, exts
MERGE (p:Package {Name: exts.Package})
WITH c, exts
MATCH (p) -[:CONTAINS]-> (t:Table {Artifact: exts.Artifact})
MERGE (t) -[:BEHAVIOR]-> (c);
MATCH(t:Table)-[r:BEHAVIOR]->(c:Class) RETURN COUNT(r) + " table behaviors";

// Extract the methods on the tables
return "Creating table methods";
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractTableMethodsCSV.csv" AS exts
MATCH (t:Table{Name: exts.Name}) -[:BEHAVIOR]-> (c:Class)
CREATE (m:Method {Name: exts.Method, Package: exts.Package, Artifact: exts.Artifact,
                  IsAbstract: toBoolean(exts.IsAbstract), IsFinal: toBoolean(exts.IsFinal), IsStatic: toBoolean(exts.IsStatic),
                  Visibility: exts.Visibility, CMP: toInteger(exts.CMP), LOC: toInteger(exts.LOC), NOS: toInteger(exts.NOS) })
MERGE (c)-[:DECLARES]->(m);
MATCH (t:Table) -[:BEHAVIOR]-> (c:Class) -[r:DECLARES]-> (m:Method) RETURN COUNT(m) + " table methods";

// attributes on table methods.
return "Creating table method attributes";
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractTableMethodAttributesCSV.csv" AS exts
MATCH (t:Table {Name: exts.Table}) -[:BEHAVIOR]-> (c:Class) -[:DECLARES]->(m:Method { Name: exts.Method})
MATCH (a:Class) WHERE ((a.Name = exts.Attribute) or (a.Name = exts.Attribute + "attribute"))
MERGE (m) -[:HASATTRIBUTE]-> (a);
MATCH (t:Table) -[:BEHAVIOR]-> (c:Class) -[:DECLARES]-> (m:Method) -[r:HASATTRIBUTE]-> (a:Class) RETURN COUNT(r) + " table method attributes";

// Views (metadata part).
return "Creating views";
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractViewsCSV.csv" AS exts
CREATE (t:View {Artifact: exts.Artifact, Name: exts.Name, Package: exts.Package,
                 Label: exts.Label, TableGroup: exts.TableGroup})
MERGE (p:Package{Name: exts.Package})
MERGE (p)-[:CONTAINS]->(t);
MATCH(t:View) RETURN COUNT(t) + " views";

// View fields
return "Creating view fields";
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractViewFieldsCSV.csv" AS exts
MATCH (t:View{Name: exts.ViewName})
CREATE (f:ViewField{Name: exts.FieldName, Type: exts.Type, Label: exts.Label,
                     Mandatory: toBoolean(exts.Mandatory), Visible: toBoolean(exts.Visible), EDT: exts.ExtendedDataType})
MERGE(t)-[:HASFIELD]->(f);
MATCH(t:View)-[:HASFIELD]->(f:ViewField) RETURN COUNT(f) + " view fields";

// View behaviors
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractViewMetricsCSV.csv" AS exts
CREATE (c:Class {Artifact: exts.Artifact, Name: exts.Name,
                 LOC: toInteger(exts.LOC), NOM: toInteger(exts.NOM), WMC: toInteger(exts.WMC), NOPM: toInteger(exts.NOPM), NOS: toInteger(exts.NOS) })
WITH c, exts
MATCH (p:Package {Name:exts.Package}) -[:CONTAINS]-> (t:View {Artifact: exts.Artifact})
MERGE (t) -[:BEHAVIOR]-> (c);
MATCH(t:View) -[r:BEHAVIOR]-> (c:Class) RETURN COUNT(r) + " view behaviors";

// Extract the methods on the views
return "Creating view methods";
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractViewMethodsCSV.csv" AS exts
MATCH (t:View{Name: exts.Name}) -[:BEHAVIOR]-> (c:Class)
CREATE (m:Method {Name: exts.Method, Package: exts.Package, Artifact: exts.Artifact,
                  IsAbstract: toBoolean(exts.IsAbstract), IsFinal: toBoolean(exts.IsFinal), IsStatic: toBoolean(exts.IsStatic),
                  Visibility: exts.Visibility, CMP: toInteger(exts.CMP), LOC: toInteger(exts.LOC), NOS: toInteger(exts.NOS) })
MERGE (c)-[:DECLARES]->(m);
MATCH (t:View) -[:BEHAVIOR]-> (c:Class) -[r:DECLARES]-> (m:Method) RETURN COUNT(m) + " view methods";

// attributes on view methods.
return "Creating view method attributes";
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractViewMethodAttributesCSV.csv" AS exts
MATCH (p:Package {Name: exts.Package}) -[:CONTAINS]-> (t:View {Name: exts.View}) -[:BEHAVIOR]-> (c:Class) -[:DECLARES]->(m:Method { Name: exts.Method})
MATCH (a:Class) WHERE ((a.Name = exts.Attribute) or (a.Name = exts.Attribute + "attribute"))
MERGE (m) -[:HASATTRIBUTE]-> (a);
MATCH (t:View) -[:BEHAVIOR]-> (c:Class) -[:DECLARES]-> (m:Method) -[r:HASATTRIBUTE]-> (a:Class) RETURN COUNT(r) + " view method attributes";

// Create the forms metadata artifact
return "Creating forms";
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractFormsCSV.csv" AS exts
CREATE (t:Form {Artifact: exts.Artifact, Name: exts.Name })
MERGE (p:Package{Name: exts.Package})
MERGE (p)-[:CONTAINS]->(t);
MATCH (f:Form) RETURN COUNT(f) + " forms";

// Create forms classes, linking to the form artifact
return "Creating forms behaviors";
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractFormMetricsCSV.csv" AS exts
CREATE (c:Class {Artifact: exts.Artifact, Name: exts.Name,
        LOC: toInteger(exts.LOC), NOM: toInteger(exts.NOM), WMC: toInteger(exts.WMC), NOPM: toInteger(exts.NOPM), NOS: toInteger(exts.NOS) })
MERGE (f:Form{Artifact: exts.Artifact})
MERGE (f)-[:BEHAVIOR]->(c);
MATCH (f:Form) -[r:BEHAVIOR]-> (c:Class) RETURN count(r) + " form behaviors";

// Create forms methods. These are wired up to the classes implementing form behavior
return "Creating forms methods";
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractFormsMethodsCSV.csv" AS exts
MATCH (f:Form{ Artifact: exts.Artifact }) -[:BEHAVIOR] ->(c:Class)
CREATE (m:Method {Name: exts.Name, Package: exts.Package, Artifact: exts.Artifact,
                  IsAbstract: toBoolean(exts.IsAbstract), IsStatic: toBoolean(exts.IsStatic), IsFinal: toBoolean(exts.IsFinal),
                  Visibility: exts.Visibility, CMP: toInteger(exts.CMP), LOC: toInteger(exts.LOC), NOS: toInteger(exts.NOS) })
MERGE (c)-[:DECLARES]-> (m);
MATCH (f:Form) -[:BEHAVIOR]-> (c:Class) -[r:DECLARES]-> (m:Method) RETURN COUNT(m) + " form methods";

// attributes on forms methods.
return "Creating forms method attributes";
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractFormMethodAttributesCSV.csv" AS exts
MATCH (f:Form {Name: exts.Form}) -[:BEHAVIOR]-> (c:Class)-[:DECLARES]->(m:Method { Name: exts.Method})
MATCH (a:Class) WHERE ((a.Name = exts.Attribute) or (a.Name = exts.Attribute + "attribute"))
MERGE (m)-[:HASATTRIBUTE]-> (a);
MATCH (f:Form) -[:BEHAVIOR]-> (c:Class) -[:DECLARES]-> (m:Method) -[r:HASATTRIBUTE] -> (a:Class) RETURN COUNT(r) + " form method attributes";

// Create the query metadata artifact
return "Creating queries";
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractQueriesCSV.csv" AS exts
CREATE (t:Query {Artifact: exts.Artifact, Name: exts.Name, Title: exts.Title })
MERGE (p:Package{Name: exts.Package})
CREATE (p)-[:CONTAINS]->(t);
MATCH (t:Query) RETURN COUNT(t) + " queries";

// Create the query classes, linking to the query artifact
return "Creating query behaviors";
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractQueryMetricsCSV.csv" AS exts
CREATE (c:Class {Artifact: exts.Artifact, Name: exts.Name,
                 LOC: toInteger(exts.LOC), NOM: toInteger(exts.NOM), WMC: toInteger(exts.WMC), NOPM: toInteger(exts.NOPM), NOS: toInteger(exts.NOS) })
MERGE (f:Query{Artifact: exts.Artifact})
CREATE (f)-[:BEHAVIOR]->(c);
MATCH (t:Query) -[r:BEHAVIOR]-> (c:Class) RETURN COUNT(r) + " query behaviors";

// Create query methods.
return "Creating query methods";
USING PERIODIC COMMIT 1000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "ExtractQueryMethodsCSV.csv" AS exts
MATCH (q:Query { Artifact: exts.Artifact }) -[:BEHAVIOR] ->(c:Class)
CREATE (m:Method {Name: exts.Name, Package: exts.Package, Artifact: exts.Artifact,
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

// Now wire in the cross reference information. This information is provided by the cross reference
// database, not the XML database. Issue the command:
//
// $ sqlcmd -S . -d DYNAMICSXREFDB -E -s, -W -I script.sql -o xref.csv
//
// where script.sql contains the following select statement
//  select  source.[Path] as sourcePath, sourceModule.Module as 'SourceModule',
//         target.[Path] as targetPath, targetModule.Module as targetModule
//        ,count(target.Path) as cnt
//  from dbo.[References] as refs
//  join dbo.[Names] as source on refs.SourceId = source.Id
//  join dbo.Modules as sourceModule on source.ModuleId = sourceModule.Id
//  join dbo.[Names] as target on refs.TargetId = target.Id
//  join dbo.Modules as targetModule on target.ModuleId = targetModule.Id
//  where refs.kind=1 -- Call. Even references to fields are marked as calls, prompting filtering below.
//  and (target.Path like '/Classes/%/Methods/%'
//    or target.Path like '/Tables/%/Methods/%'
//    or target.Path like '/Queries/%/Methods/%'
//    or target.Path like '/Forms/%/Methods/%'
//    or target.Path like '/Views/%/Methods/%')
//  group by source.Path, sourceModule.Module, target.Path, targetModule.Module
//
// This will leave a csv file called xref.csv in the current directory
return "Creating XREF";
USING PERIODIC COMMIT 5000
LOAD CSV WITH HEADERS FROM  $EXPORTDIRECTORY + "xref.csv" AS exts
WITH split(toLower(exts.sourcePath), "/") as sourceParts,
     split(toLower(exts.targetPath), "/") AS targetParts,
     toInteger(exts.cnt) AS cnt,
     toLower(exts.SourceModule) AS sourceModule,
     toLower(exts.targetModule) AS targetModule
WITH CASE sourceParts[1]
    WHEN "classes" THEN ["class", sourceParts[2], sourceParts[4]]
    WHEN "tables" THEN ["table", sourceParts[2], sourceParts[4]]
    WHEN "queries" THEN ["query", sourceParts[2], "query$" + sourceParts[4]]
    WHEN "forms" THEN ["form", sourceParts[2], "form$" + sourceParts[4]]
    WHEN "views" THEN ["view", sourceParts[2], sourceParts[4]]
    ELSE [sourceParts[1],  sourceParts[4]]
END AS sourceTriplet,
CASE targetParts[1]
    WHEN "classes" THEN ["class", (targetParts[2]), (targetParts[4])]
    WHEN "tables" THEN ["table", (targetParts[2]), toLower(targetParts[4])]
    WHEN "queries" THEN ["query", (targetParts[2]), toLower("query$" + targetParts[4])]
    WHEN "forms" THEN ["form", (targetParts[2]), toLower("form$" + targetParts[4])]
    WHEN "views" THEN ["view", (targetParts[2]), toLower(targetParts[4])]
    ELSE [targetParts[1], targetParts[4]]
END AS targetTriplet, sourceModule, targetModule, cnt
WITH sourceModule, sourceTriplet[0] as sourceKind, sourceTriplet[1] as sourceArtifactName, sourceTriplet[2] as sourceMethod,
     targetModule, targetTriplet[0] as targetKind, targetTriplet[1] as targetArtifactName, targetTriplet[2] as targetMethod,
     cnt
//return sourceModule, sourceKind, sourceArtifactName, targetModule, targetKind, targetArtifactName;
MATCH (sm:Method) WHERE sm.Package = sourceModule and sm.Artifact = sourceKind + ":" + sourceArtifactName and sm.Name = sourceMethod
MATCH (tm:Method) WHERE tm.Package = targetModule and tm.Artifact = targetKind + ":" + targetArtifactName and tm.Name = targetMethod
MERGE (sm) -[:CALLS {Weight: cnt}]->(tm);
MATCH (p1) -[r:CALLS]->(p2) RETURN count(r) +" calls";

// Extract the information that creates the managed types and methods from the cross reference
// select source.[Path] as sourcePath, sourceModule.Module as sourceModule,
//        target.[Path] as targetPath, targetModule.Module as targetModule
//     ,count(target.Path) as cnt
// from dbo.[References] as refs
// join dbo.[Names] as source on refs.SourceId = source.Id
// join dbo.Modules as sourceModule on source.ModuleId = sourceModule.Id
// join dbo.[Names] as target on refs.TargetId = target.Id
// join dbo.Modules as targetModule on target.ModuleId = targetModule.Id
// where refs.kind=1 -- Call. Even references to fields are marked as calls, prompting filtering below.
// and (target.Path like '/ClrType/%/Methods/%')
// group by source.Path, sourceModule.Module, target.Path, targetModule.Module
//
// Into a file called clrtypes.csv.
//
USING PERIODIC COMMIT 5000
LOAD CSV WITH HEADERS FROM $EXPORTDIRECTORY + "clrtypes.csv" AS exts
WITH split(toLower(exts.sourcePath), "/") as sourceParts,
     split(toLower(exts.targetPath), "/") AS targetParts,
     toInteger(exts.cnt) AS cnt,
     toLower(exts.sourceModule) AS sourceModule,
     toLower(exts.targetModule) AS targetModule
WITH CASE sourceParts[1]
    WHEN "classes" THEN ["class", sourceParts[2], sourceParts[4]]
    WHEN "tables" THEN ["table", sourceParts[2], sourceParts[4]]
    WHEN "queries" THEN ["query", sourceParts[2], "query$" + sourceParts[4]]
    WHEN "forms" THEN ["form", sourceParts[2], "form$" + sourceParts[4]]
    WHEN "views" THEN ["view", sourceParts[2], sourceParts[4]]
    ELSE [sourceParts[1],  sourceParts[4]]
END AS sourceTriplet,
CASE targetParts[1]
    WHEN "clrtype" THEN ["clrtype", (targetParts[2]), (targetParts[4])]
    ELSE [targetParts[1], targetParts[4]]
END AS targetTriplet, sourceModule, targetModule, cnt
WITH sourceModule, sourceTriplet[0] as sourceKind, sourceTriplet[1] as sourceArtifactName, sourceTriplet[2] as sourceMethod,
     targetModule, targetTriplet[0] as targetKind, targetTriplet[1] as targetArtifactName, targetTriplet[2] as targetMethod,
     cnt
// return sourceModule, sourceKind, sourceArtifactName, targetModule, targetKind, targetArtifactName;
MATCH (sm:Method) WHERE sm.Package = sourceModule and sm.Artifact = sourceKind + ":" + sourceArtifactName and sm.Name = sourceMethod
MERGE (p:Package {Name: targetModule})
MERGE (mt:ManagedType {Artifact: targetKind + ":" + targetArtifactName, Name: targetArtifactName})
MERGE (p) -[:CONTAINS]-> (mt)
MERGE (mm:ManagedMethod {Artifact: targetArtifactName, Name: targetMethod, Package: p.Name})
MERGE (mt) -[:DECLARES]-> (mm)
MERGE (sm) -[:CALLS {Weight: cnt}]-> (mm);
MATCH (m:ManagedType) RETURN count(m) +" Managed types";
MATCH (m:ManagedMethod) RETURN count(m) +" Managed methods";
MATCH (p1:Method) -[r:CALLS]->(p2:ManagedMethod) RETURN count(r) +" calls to managed methods";
