// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using ICSharpCode.AvalonEdit.Snippets;
using System;
using System.Collections.Generic;

namespace XppReasoningWpf
{
    public class Snippets
    {
        private IDictionary<string, Snippet> snippets;

        public IDictionary<string, Snippet> NamedSnippets => this.snippets;

        public Snippets()
        {
            this.snippets = new Dictionary<string, Snippet>(StringComparer.OrdinalIgnoreCase);

            var artifact = new SnippetReplaceableTextElement { Text = "$a" };
            var method = new SnippetReplaceableTextElement { Text = "$method" };
            var topLevelNodeName = new SnippetReplaceableTextElement { Text = "Results" };
            var resultNodeName = new SnippetReplaceableTextElement { Text = "Result" };

            Snippet basicSnippet = new Snippet
            {
                Elements = {
                    new SnippetTextElement { Text = "(: Put description here. It will appear in the tooltip :)\n" },
                    new SnippetTextElement { Text = "<" }, new SnippetBoundElement { TargetElement = topLevelNodeName }, new SnippetTextElement {Text = ">\n"},
                    new SnippetTextElement { Text = "{\n" },
                    new SnippetTextElement { Text = "  for " }, new SnippetBoundElement { TargetElement = artifact }, new SnippetTextElement { Text = " in /(Class | Table | Form | Query)\n" },
                    new SnippetReplaceableTextElement { Text = "  where true()\n" },
                    new SnippetTextElement { Text = "  order by "}, new SnippetBoundElement { TargetElement = artifact }, new SnippetReplaceableTextElement { Text = "/@Name\n" },
                    new SnippetCaretElement { },
                    new SnippetTextElement { Text =     "  return " }, new SnippetTextElement { Text = "<" }, new SnippetBoundElement { TargetElement = resultNodeName },
                        new SnippetTextElement { Text = " Artifact='{" }, new SnippetBoundElement { TargetElement = artifact }, new SnippetTextElement { Text = "/@Artifact}'\n"},
                        new SnippetTextElement { Text = "    StartLine='{" }, new SnippetBoundElement { TargetElement = artifact }, new SnippetTextElement { Text = "/@StartLine}'"},
                        new SnippetTextElement { Text = " StartCol='{" }, new SnippetBoundElement { TargetElement = artifact }, new SnippetTextElement { Text = "/@StartCol}'"},
                        new SnippetTextElement { Text = " EndLine='{" }, new SnippetBoundElement { TargetElement = artifact }, new SnippetTextElement { Text = "/@EndLine}'"},
                        new SnippetTextElement { Text = " EndCol='{" }, new SnippetBoundElement { TargetElement = artifact }, new SnippetTextElement { Text = "/@EndCol}'"},
                    new SnippetTextElement {Text = "/>\n"},
                    new SnippetTextElement { Text = "}\n" },
                    new SnippetTextElement { Text = "</" }, new SnippetBoundElement { TargetElement = topLevelNodeName }, new SnippetTextElement {Text = ">\n"},
                }
            };

            Snippet ruleSnippet = new Snippet
            {
                Elements = {
                    new SnippetTextElement { Text = "(: Put description here. It will appear in the tooltip :)\n" },
                    new SnippetTextElement { Text = "<Diagnostics>\n" },

                    new SnippetTextElement { Text = "{\n" },
                    new SnippetTextElement { Text = "  for " }, new SnippetBoundElement { TargetElement = artifact }, new SnippetTextElement { Text = " in /(Class | Table | Form | Query)\n" },
                    new SnippetTextElement { Text = "  for " }, new SnippetBoundElement { TargetElement = method }, new SnippetTextElement { Text = " in " }, new SnippetBoundElement { TargetElement=artifact }, new SnippetTextElement { Text = "//Method\n" },
                    new SnippetReplaceableTextElement { Text = "  where true() \n" },
                    new SnippetTextElement { Text = "  order by "}, new SnippetBoundElement { TargetElement = artifact }, new SnippetReplaceableTextElement { Text = "/@Name\n" },
                    new SnippetTextElement { Text = "  let $typeNamePair := fn:tokenize(" }, new SnippetBoundElement { TargetElement = artifact }, new SnippetTextElement { Text= "/@Artifact, ':')\n" }, 
                    new SnippetCaretElement { },
                    new SnippetTextElement { Text =     "  return <Diagnostic>\n" },
                        new SnippetTextElement { Text = "     <Moniker>" }, new SnippetReplaceableTextElement {Text="Name" }, new SnippetTextElement { Text = "</Moniker>\n"},
                        new SnippetTextElement { Text = "     <Severity>" }, new SnippetReplaceableTextElement { Text = "Error, Warning" }, new SnippetTextElement { Text = "</Severity>\n"},
                        new SnippetTextElement { Text = "     <Path>" }, new SnippetTextElement { Text = "dynamics://{$typeNamePair[1]}/{$typeNamePair[2]}/Method/{string(" }, new SnippetBoundElement { TargetElement = method }, new SnippetTextElement { Text = "/@Name)}" }, new SnippetTextElement { Text = "</Path>\n"},
                        new SnippetTextElement { Text = "     <Message>" }, new SnippetReplaceableTextElement { Text = "This is the rule message" }, new SnippetTextElement { Text = "</Message>\n"},
                        new SnippetTextElement { Text = "     <DiagnosticType>AppChecker</DiagnosticType>\n" },

                        new SnippetTextElement { Text = "     <Line>{string(" }, new SnippetBoundElement { TargetElement = artifact }, new SnippetTextElement { Text = "/@StartLine)}</Line>\n"},
                        new SnippetTextElement { Text = "     <Column>{string(" }, new SnippetBoundElement { TargetElement = artifact }, new SnippetTextElement { Text = "/@StartCol)}</Column>\n"},
                        new SnippetTextElement { Text = "     <EndLine>{string(" }, new SnippetBoundElement { TargetElement = artifact }, new SnippetTextElement { Text = "/@EndLine)}</EndLine>\n"},
                        new SnippetTextElement { Text = "     <EndColumn>{string(" }, new SnippetBoundElement { TargetElement = artifact }, new SnippetTextElement { Text = "/@EndCol)}</EndColumn>\n"},
                    new SnippetTextElement { Text =     "  </Diagnostic>\n" },
                    new SnippetTextElement { Text = "}\n" },
                    new SnippetTextElement { Text = "</Diagnostics>\n"},
                }
            };

            Snippet dbInfoSnippet = new Snippet
            {
                Elements = {
                    new SnippetTextElement { Text = "(: Show the statistics of the database given its name  :)\n" },
                    new SnippetTextElement { Text = "(: Note: This will consume resources for big databases :)\n" },
                    new SnippetTextElement { Text = "(: if the database is not already open.                :)\n" },
                    new SnippetTextElement { Text = "db:info($database)" }
                }
            };

            Snippet dbListSnippet = new Snippet
            {
                Elements = {
                    new SnippetTextElement { Text = "(: Show the databases  :)\n" },
                    new SnippetTextElement { Text = "db:list()" }
                }
            };

            this.snippets.Add("Basic", basicSnippet);
            this.snippets.Add("Appchecker rule", ruleSnippet);
            this.snippets.Add("Database info", dbInfoSnippet);
            this.snippets.Add("Database list", dbListSnippet);
        }
    }
}
