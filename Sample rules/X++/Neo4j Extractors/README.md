# Introduction #

This directory contains files that can be used to generate files that can be quickly imported into the neo4j database. These files are comma separated files, that conceptually can be used for other purposes as well.

The queries in the .xq files are evaluated in a BaseX instance. You may use the BulkQuery tool that is part of this github repo to do this work. It accepts the specification of a directory containing *.xq files and the name of a directory that will contain the .csv files.

## Naming ##
The import format of Neo4j requires that the input files are split into data files (that contain the nodes) and relationship files (that contain the edges between the nodes). To make it clear which is which, a naming convention is used:

| File name | Meaning |
| -- | -- |
*Name*Nodes.xq | XQuery extractor script for nodes designated by the given name. For instance, the file called ClassNodes.xq will extract a comma separated file called ClassNodes.csv that contains one line for each class (and Neo4j will import each line as a node in the graph) |
*Name*\_*RELATION*\_*Name* .xq| XQuery script that extracts relationships between nodes. For instance, the file called Class\_EXTENDS\_Class.xq contains an XQuery script that creates a comma separated file where each line descripes that one class extends another. Neo4j will create an edge between the classes based on this information. | 

The xq files are XQuery source files that can be executed against a BaseX backend that contains a database containing X++ ASTs. The BulkQuery tool that is part of this repo can be used to execute all the scripts in a particular directory (i.e. the .xq files), storing the results (i.e. the .csv files) into a target directory. The Bulkquery tool can be run like this:

    $ bulkquery --server localhost --password mypassword --verbose --database myxbasedatabase --extension csv --output-directory csvfiles queryfiles\*.xq

The command above will run all the queries in the queryfiles directory, producing a set of results in the csvfiles directory. The files will have the name of the query files with the .csv extension.

There may be other files that can be used for other extraction purposes, if the information used to generate the nodes and edges are not based in a BaseX database. One example of this is the cross reference database that is maintained by the X++ compiler. It is easier to extract the call information from this database than from the XML database. The source includes a query called Method_CALLS_Method.sql where you will find the details about how to get this information.

Once the data is created in CSV files, it can be imported into Neo4j by using the nwo4j-admin tool with the import argument. You will how to do this by perusing the import.bat file in this directory.

