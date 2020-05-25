namespace BulkQuery
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using WorkflowLibrary;

    class Program
    {
        /// <summary>
        /// This is the number of concurrent sessions that query the server.
        /// </summary>
        private const int noOfSessions = 4;

        /// <summary>
        /// Downloads a set of files by executing queries against BaseX. The BaseX server is identified with 
        /// the server URL, the username and password and optionally the port number. The queries to run are 
        /// given as arguments to this command. The downloaded files will have the same names as the queries,
        /// with the extension provided.
        /// </summary>
        /// <param name="server">The name of the BaseX server.</param>
        /// <param name="database">The name of the database to be queries on the BaseX server.</param>
        /// <param name="username">The user name. The default is "admin".</param>
        /// <param name="password">The password used to log into the BaseX server.</param>
        /// <param name="port">The port number where the BaseX server listens for connections. The default value is 1984</param>
        /// <param name="extension">The file extension of the extracted files. The default is XML.</param>
        /// <param name="outputDirectory">The target directory. If this parameter is not provided, the current working directory is used.</param>
        /// <param name="verbose">Print extra progress information to the console.</param>
        /// <param name="args">The files containing the queries to execute. A list of query files can be provided, including wildcards.</param>
        public static void Main(string server, string password, string database, 
            string username="admin", DirectoryInfo outputDirectory=null, string extension="xml", int port=1984, bool verbose=false, params string[] args)
        {
            if (args == null || !args.Any())
            {
                Console.WriteLine("No scripts provided");
                return;
            }

            var s = new BaseXInterface.BaseXServer(server, port, username, password);

            var files = new List<string>();

            foreach (var arg in args)
            {
                var p = Path.GetDirectoryName(arg);
                var l = Directory.EnumerateFiles(p, Path.GetFileName(arg));
                files.AddRange(l);
            }

            if (outputDirectory == null)
                outputDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

            if (verbose)
            {
                Console.WriteLine("Server      {0}", server);
                Console.WriteLine("Database    {0}", database);
                Console.WriteLine("Username    {0}", username);
                Console.WriteLine("Port        {0}", port);
                Console.WriteLine("extension   {0}", extension);
                Console.WriteLine("Target dir  {0}", outputDirectory.ToString());
                Console.WriteLine();
                Console.WriteLine("Files");
                foreach (var file in files)
                {
                    Console.WriteLine("    {0}", file);
                }
            }

            Extract(s, database, files, outputDirectory, extension, verbose);
        }

        private static void Extract(BaseXInterface.BaseXServer server, string database, IEnumerable<string> scripts, DirectoryInfo outputDirectory, string extension, bool verbose)
        {
            Parallel.ForEach(scripts,
                new ParallelOptions { MaxDegreeOfParallelism = 4 },
                fn =>
                {
                    using (var session = server.GetSession(database))
                    {
                        var outputFileName = Path.Combine(outputDirectory.FullName, Path.ChangeExtension(Path.GetFileName(fn), extension));

                        try
                        {
                            if (verbose)
                            {
                                Console.WriteLine("Extracting {0} into {1}", fn, outputFileName);
                            }
                            var result = Rule.GetRuleFromFile(fn)
                                .RunAsString(session)
                                .SaveAsFile(outputFileName);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(fn);
                            Console.WriteLine(e.Message);
                        }
                    }
                });
        
        }
    }
}
