// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BaseXInterface
{
    /// <summary>
    /// These are mainly useful extension methods on sessions
    /// </summary>
    public static class Utilities
    {
        ///// <summary>
        ///// Set the bindings from the bindings dialog. 
        ///// </summary>
        ///// <param name="commaSeparatedList">A string constructed as a comma separated list of name=value, like so:
        ///// name=val, name=val, ..., name=val</param>
        ///// <returns>The value returned from the server</returns>
        //private static string SetBindings(this Session session, string commaSeparatedList)
        //{
        //    return session.Execute(@"set bindings " + commaSeparatedList);
        //}

        /// <summary>
        /// Set the bindings from the bindings dialog. 
        /// </summary>
        /// <param name="commaSeparatedList">A string constructed as a comma separated list of name=value, like so:
        /// name=val, name=val, ..., name=val</param>
        /// <returns>The value returned from the server</returns>
        private async static Task<string> SetBindingsAsync(this Session session, string commaSeparatedList)
        {
            return await session.ExecuteAsync(@"set bindings " + commaSeparatedList);
        }

        /// <summary>
        /// The spec describes that variable names must satisfy the same criteria
        /// as names of tags. Instead of duplicating the requirements in code, we
        /// simply try to instantiate a document consisting of a node with the 
        /// given name.
        /// </summary>
        /// <param name="name">The name to check.</param>
        /// <returns>true if the name is a valid variable name, and false otherwise.</returns>
        private static bool ValidateVariableName(string name)
        {
            try
            {
                var d = XDocument.Parse(string.Format("<{0}/>", name));
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private static string CalculateBindingsString(params Tuple<string, string>[] vars)
        {
            string result = string.Empty;

            if (vars != null)
            {
                foreach (var v in vars)
                {
                    var name = v.Item1;
                    var value = v.Item2;

                    if (ValidateVariableName(name))
                    {
                        if (result.Any())
                            result += ",";
                        result += name + "=" + value;
                    }
                }
            }

            return result;
        }

        private static string CalculateExternalDeclarations(params string[] vars)
        {
            string result = string.Empty;

            foreach (var v in vars)
            {
                if (ValidateVariableName(v))
                {
                    result += "declare variable $" + v + " external;\n";
                }
            }

            return result;
        }

        public static void OpenDatabase(this Session session, string database)
        {
            session.Execute("open " + database);
        }

        public async static Task OpenDatabaseAsync(this Session session, string database)
        {
            await session.ExecuteAsync("open " + database);
        }

        /// <summary>
        /// Execute the query against the database in the current session
        /// </summary>
        /// <param name="query">The query to execute.</parthis Session sessionam>
        /// <param name="externalVars">The external variables to be referenced in the query.</param>
        /// <returns>The result of the query</returns>
        public static async Task<string> DoQueryAsync(this Session session, string query, params Tuple<string, string>[] externalVars)
        {
            string result;

            if (externalVars != null)
            {
                string bindingsString = CalculateBindingsString(externalVars);
                result = await session.SetBindingsAsync(bindingsString);

                string externalDeclarations = ""; // CalculateExternalDeclarations(externalVars.Select(t => t.Item1).ToArray());
                result = await session.ExecuteAsync(@"xquery " + externalDeclarations + query);
            }
            else
            {
                result = await session.ExecuteAsync(@"xquery " + query);
            }
            return result;
        }

        public async static Task<string> CheckQueryAsync(this Session session, string query)
        {
            // Deal with any quotes that may appear in the string
            query = query.Replace("'", "&#34;").Replace("\"", "&#39;");
            return await session.ExecuteAsync(string.Format("xquery xquery:parse('{0}')", query));
        }

        public static async Task<string> SubmitQueryAsync(this Session session, string query, params Tuple<string, string>[] externalVars)
        {
            string result;

            string bindingsString = CalculateBindingsString(externalVars);
            result = await session.SetBindingsAsync(bindingsString);

            query = query.Replace("'", "&#34;").Replace("\"", "&#39;");
            var queryString = $"jobs:eval('{query}', (), map {{ 'cache': true() }})";

            result = await session.ExecuteAsync(@"xquery " + queryString);

            return result;
        }
    }
}
