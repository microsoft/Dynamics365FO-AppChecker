// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace XppReasoningWpf
{
    class SampleQueries
    {
        public static List<string> AddQueries(string rootDirectory)
        {
            var result = new List<string>();
            var assembly = Assembly.GetExecutingAssembly();

            // Get all the xq streams??
            var names = assembly.GetManifestResourceNames().Where(n => n.StartsWith("XppReasoningWpf.Queries", StringComparison.OrdinalIgnoreCase));

            foreach (var queryName in names)
            {
                using (Stream s = assembly.GetManifestResourceStream(queryName))
                {
                    var r = new StreamReader(s);
                    {
                        var c = r.ReadToEnd();

                        var parts = queryName.Split('.').ToList();
                        parts.RemoveAt(0); // Skip namespace

                        var extension = parts.Last();
                        parts.RemoveAt(parts.Count() - 1);

                        var name = parts.Last();
                        parts.RemoveAt(parts.Count() - 1);

                        string directoryPart = Path.Combine(parts.ToArray());
                        var directoryName = Path.Combine(rootDirectory, directoryPart);
                        Directory.CreateDirectory(directoryName);

                        var fullName = Path.Combine(directoryName, name + '.' + extension);
                        result.Add(fullName);
                        File.WriteAllText(fullName, c);
                    }
                }
            }

            return result;
        }
    }
}
