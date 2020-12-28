
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoExtractor
{
    using System.ComponentModel.Composition;
    using System.Diagnostics;
    using System.IO;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using LanguageExtractorInterfaces;

    [Export(typeof(ILanguageExtractor)),
        ExportMetadata("Name", "Go"),
        ExportMetadata("Extension", "go"),
        ExportMetadata("Version", "1.0.0"),
        ExportMetadata("Sample", @"package main

import (
   ""fmt"" 
)

func Main(arg int) int {
    return arg + 1
}")]
    public class Extractor : ILanguageExtractor
    {
        class DiagnosticItem : IDiagnosticItem
        {
            public int Line {get; set; }

            public int Column { get; set; }

            public string Message { get; set; }

            public int EndLine => this.Line;

            public int EndColumn => this.Column;

            public string Id => "Error"; // There is no recorded moniker for errors.

            public string Severity => "Error";
        }

        /// <summary>
        /// Get a temporary directory.
        /// </summary>
        /// <returns>The name of a temporary directory.</returns>
        private static string GetTempDirectory()
        {
            string path;
            do
            {
                path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            } while (Directory.Exists(path));

            Directory.CreateDirectory(path);
            return path;
        }

        /// <summary>
        /// Extracts the source code passed the this method.
        /// </summary>
        /// <param name="source">The source code to extract</param>
        /// <returns>The tuple of extracted code and list of diagnostics.
        /// In this implementation there system will not generate a document 
        /// when there are parse errors. In this case the returned document 
        /// will be nil and there will be one or more entries in the diagnostics
        /// list.
        /// </returns>
        public (XDocument, IEnumerable<IDiagnosticItem>) Extract(string source)
        {
            // Store the source in a file in a temporary directory
            string tempDirectory = GetTempDirectory();

            File.WriteAllText(Path.Combine(tempDirectory, "test.go"), source);

            // Calculate the path to the go extractor. We do this by traversing the
            // known structure of the source tree for now.
            var localDllPath = typeof(Extractor).Assembly.Location;
            var parts = localDllPath.Split(Path.DirectorySeparatorChar);

            // Get rid of all the directories leading to the ast extractor (i.e. this executable).
            var root = string.Join(Path.DirectorySeparatorChar.ToString(), parts.Take<string>(parts.Length - 6));

            // Add the directory path to the extractor .go file.
            string GoExtractorSource = Path.Combine (root, "Extractors", "Go");

            string GoExecutable;
            var goRoot = Environment.GetEnvironmentVariable("GOROOT");
            if  (goRoot != null)
            {
                GoExecutable = Path.Combine(goRoot, "bin", "go");
            }
            else
            {
                // No GOROOT environment variable is defined, so assume that 
                // the user's path contains the directory.
                GoExecutable = "go";
            }

            try
            {
                // Call the extractor using the process interface. It is assumed 
                // that go is installed beforehand and is recorded in the path.
                var process = new Process();

                // Configure the process using the StartInfo properties. Use the 
                // same directory for the source and the results.
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.WorkingDirectory = GoExtractorSource;
                process.StartInfo.FileName = GoExecutable;
                process.StartInfo.Arguments = string.Format("run extractor.go walker.go -Source={0} -Target={0}", tempDirectory);
                // process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();

                // Check the resulting files. Either there is ane error file,
                // which is read and parsed and presented as a DiagnosticItem,
                var extractedFile = Path.Combine(tempDirectory, "main", "main.xml");
                if (File.Exists(extractedFile))
                {
                    var xml = File.ReadAllText(extractedFile);
                    return (XDocument.Parse(xml), null);
                }
                else
                {
                    var errorFile = Path.Combine(tempDirectory, "errors.xml");
                    if (File.Exists(errorFile))
                    {
                        var xml = File.ReadAllText(errorFile);
                        var doc = XDocument.Parse(xml);
                        var diagnostics = new List<IDiagnosticItem>();

                        foreach (var diag in doc.XPathSelectElements("//Diagnostic"))
                        {
                            diagnostics.Add(new DiagnosticItem
                            {
                                Message = diag.Attribute("Message").Value,
                                Line = int.Parse(diag.Attribute("StartLine").Value),
                                Column = int.Parse(diag.Attribute("StartColumn").Value)
                            });
                        }

                        return (XDocument.Parse("<Result>The Go source contains one or more errors</Result>"), diagnostics);
                    }
                }

                // or there is an XML file with the results passed as the document.
            }
            finally
            {
                // Clean up the temporary files
                Directory.Delete(tempDirectory, recursive: true);
            }

            return (null, null);
        }
    }
}
