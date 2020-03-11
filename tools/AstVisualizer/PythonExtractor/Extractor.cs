// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.Collections.Generic;
using LanguageExtractorInterfaces;
using System.ComponentModel.Composition;
using System.IO;
using System.Xml.Linq;
using System.Diagnostics;
using System;
using System.Linq;

namespace PythonExtractor
{
    [Export(typeof(ILanguageExtractor)),
        ExportMetadata("Name", "Python"),
        ExportMetadata("Extension", "py"),
        ExportMetadata("Version", "1.0.0"),
        ExportMetadata("Sample", @"# Function to reverse arr[] from index start to end 
def rverseArray(arr, start, end): 
    while (start < end): 
        temp = arr[start] 
        arr[start] = arr[end] 
        arr[end] = temp 
        start += 1
        end = end-1
")]
    public class Extractor : ILanguageExtractor
    {
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

        public (XDocument, IEnumerable<IDiagnosticItem>) Extract(string source)
        {
            // Create a temporary directory and place the incoming source in there.
            string sourceDirectory = GetTempDirectory();
            string pythonFilePath = Path.Combine(sourceDirectory, "source.py");
            string resultDirectory = GetTempDirectory();
            
            try
            {
                File.WriteAllText(pythonFilePath, source);

                // Calculate the path to the python extractor. We do this by traversing the
                // known structure of the source tree for now.
                var localDllPath = typeof(Extractor).Assembly.Location;
                var parts = localDllPath.Split(Path.DirectorySeparatorChar);

                // Get rid of all the directories leading to the ast extractor (i.e. this executable).
                var root = string.Join(Path.DirectorySeparatorChar.ToString(), parts.Take<string>(parts.Length - 6));

                // Add the directory path to the extractor .py file.
                string PythonExtractorSource = Path.Combine(Path.Combine(Path.Combine(Path.Combine(root, "Extractors"), "Python"), "extractor"), "pythonextractor.py");

                // Start the python extractor.
                using (var process = new Process())
                {
                    process.StartInfo.FileName = "python";
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.UseShellExecute= false;
                    process.EnableRaisingEvents = true;
                    process.StartInfo.Arguments = $"\"{PythonExtractorSource}\" --formatxml=False \"{sourceDirectory}\" \"{resultDirectory}\"";
                           
                    process.Start();
                    process.WaitForExit();
                }

                return (XDocument.Load(Path.Combine(resultDirectory, "source.xml")), null);
            }
            finally
            {
                // Clean up the temporary directories and their content.
                Directory.Delete(sourceDirectory, recursive: true);
                Directory.Delete(resultDirectory, recursive: true);
            }
        }
    }
}
