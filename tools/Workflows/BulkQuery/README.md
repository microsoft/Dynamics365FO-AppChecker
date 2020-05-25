# BulkQuery
This tool is a command line tool built in .NET core that allows the user to execute one or more queries against a BaseX server in bulk.

The command line parameters are:

        Usage
            BulkQuery [options] [<args>...]

        Arguments:
            <args>    The files containing the queries to execute. A list of query files can be provided, including wildcards.

        Options:
        --server <server>                        The name of the BaseX server.
        --password <password>                    The password used to log into the BaseX server.
        --database <database>                    The name of the database to be queries on the BaseX server.
        --username <username>                    The user name. The default is "admin".
        --output-directory <output-directory>    The target directory.
        --extension <extension>                  The file extension of the extracted files.
        --port <port>                            The port number where the BaseX server listens for connections.
        --verbose                                Print extra information to the console.
        --version                                Show version information
        -?, -h, --help                           Show help and usage information

The name of the BaseX server is a URL that designates a server running a BaseX server. The port switch can be used to override the default port number, if necesarry. The user name and password should be provided. If no user name is provided, the "admin" user is used. The queries are executed against the database provided by the database switch.

The verbose flag causes the tool to list the supplied parameters and logs the query execution to the standard output.

The results of the query are written into files with the same name as the query, but with the extension provided by the extension switch. The default extension is XML. The result files are written in the directory specified by the -output-directory switch. If this switch is not used, the resulting files will be written in the current working directory.

## Examples

    $ BulkQuery --server localhost --password secret --database mycodebase  *.xq

Executes the queries contained in files with the xq extension in the current library against the BaseX server running locally. Resulting files (with the default extension .XML) are stored in the current directory.
