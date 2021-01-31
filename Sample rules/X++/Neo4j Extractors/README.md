# Introduction #

This directory contains files that can be used to generate files that can be quickly imported into the neo4j database. These files are comma separated files, that conceptually can be used for other purposes as well.

## Naming ##
The import format of Neo4j requires that the input files are split into data files (that contain the nodes) and relationship files (that contain the edges between the nodes). To make it clear which is which, a naming convention is used:

| File name | Meaning |
| -- | -- |
*Name*Nodes.xq | XQuery extractor script for nodes designated by the given name. For instance, the file called ClassNodes.xq will extract a comma separated file called ClassNodes.csv that contains one line for each class (and Neo4j will import each line as a node in the graph) |
*Name*\_*RELATION*\_*Name* .xq| XQuery script that extracts relationships between nodes. For instance, the file called Class\_EXTENDS\_Class.xq contains an XQuery script that creates a comma separated file where each line descripes that one class extends another. Neo4j will create an edge between the classes based on this information. | 


The xq files are XQuery source files that can be executed against a BaseX backend that contains a database containing X++ ASTs. The BulkQuery tool that is part of this repo can be used to execute all the scripts in a particular directory (i.e. the .xq files), storing the results (i.e. the .csv files) into a target directory.

There may be other files that can be used for other extraction purposes, if the information used to generate the nodes and edges are not based in a BaseX database. 

Once the data is created in CSV files, it can be imported into Neo4j by using the nwo4j-admin tool with the import argument. You will how to do this by perusing the import.bat file in this directory.

