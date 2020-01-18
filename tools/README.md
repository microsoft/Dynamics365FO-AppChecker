# Socratex: A tool for reasoning over source code.
This tool allows you to reason over source code as if it were data stored in a database.
It works by submitting queries in the XQuery language towards a server where a dedicated XML
database handles the queries against a repository of XML documents that describe the source code
corpus is a specific language.

The tool offers a rich editing experience for the XQuery queries, and is able to show the code
that is identified by the rule by interpreting the resulting XML results.

The requirements for the artifacts represented as XML are:

The queries must generate a single XML document, whose root level tag is immaterial. In addition there
are certain attributes that are required for the correct functioning of the tool's ability to show
the results mapped to the source code; for this to work correctly, the source code in the source language
must be stored in a property called Source.

Most nodes that represent an AST node should have the following attributes, designating the textual position
of the artifact within the source code:
	StartLine
	StartCol
	EndLine
	EndCol
which are all integer values starting with 1 as the first line and 1 as the first character on a line.

Data Collection. By default the system is configured to not send any data to any Applications Insight endpoint. You nmay set the sendTelemetry variable according to your needs.