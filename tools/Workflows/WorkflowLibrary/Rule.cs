// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WorkflowLibrary
{
    using BaseXInterface;
    using System.Text.RegularExpressions;

    public class Rule
    {
        private string Source { get; set; }

        public XDocument Run(Session session)
        {
            var result = session.DoQueryAsync(this.Source).Result;
            try
            {
                return XDocument.Parse(result);
            }
            catch
            {
                XDocument res = new XDocument();
                res.Document.Add(new XElement("Error", result));
                return res;
            }
        }

        public string RunAsString(Session session)
        {
            var result = session.DoQueryAsync(this.Source).Result;
            return result;
        }

        public async Task<string> RunAsStringAsync(Session session)
        {
            var result = await session.DoQueryAsync(this.Source);
            return result;
        }

        private static IEnumerable<string> ExtractCategoriesFromSource(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException(nameof(source));
            }

            // Find a (: comment containg @Category: C1, C2, ... Cn.
            string pattern = @"\(:[\s\t]*@Category[\s\t]+(l?.*):\)";

            Match res = Regex.Match(source, pattern);
            if (res.Success)
            {
                var listOfCategories = res.Groups[1].Captures[0].Value.Trim(' ');
                return listOfCategories.Split(',');
            }

            return new List<string>();
        }

        public static XDocument GetQueriesAsXml(IDictionary<string, string> queriesByName)
        {
            if (queriesByName == null)
                throw new ArgumentNullException(nameof(queriesByName));

            XDocument result = new XDocument();
            XElement root = new XElement("PredefinedQueries");
            result.Document.Add(root);

            foreach (var kvp in queriesByName)
            {
                string name = kvp.Key;
                string source = kvp.Value;
                IEnumerable<string> categories = ExtractCategoriesFromSource(source);

                IEnumerable<XElement> categoryElements = categories.Select(c => new XElement("Category", new XAttribute("Name", c)));
                XElement query = new XElement("Query", new XAttribute("Name", name), categoryElements);
                query.Add(new XText(source));

                root.Add(query);
            }

            return result;
        }

        /// <summary>
        /// Get the designated rule from a file on the local file system. The
        /// method will throw an ArgumentException if the file is not found.
        /// </summary>
        /// <param name="filename">The path of the file.</param>
        /// <returns>The Rule instance with the given source code.</returns>
        public static Rule GetRuleFromFile(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException(nameof(filename));
            }

            if (!File.Exists(filename))
            {
                throw new ArgumentException("No such file: " + filename, nameof(filename));
            }

            return new Rule()
            {
                Source = File.ReadAllText(filename)
            };
        }

        public static Rule GetRuleFromSource(string source)
        {
            return new Rule() { Source = source };
        }
    }
}
