// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.Collections.Generic;
using System.Linq;

namespace TSqlExtractor
{
    using LanguageExtractorInterfaces;
    using Microsoft.SqlServer.TransactSql.ScriptDom;
    using System.ComponentModel.Composition;
    using System.IO;
    using System.Xml.Linq;

    [Export(typeof(ILanguageExtractor)),
        ExportMetadata("Name", "TSQL"),
        ExportMetadata("Extension", "sql"),
        ExportMetadata("Version", "1.0.0"),
        ExportMetadata("Sample", @"CREATE TABLE T1
(
    Id INT NOT NULL,
    Name VARCHAR(20),
    PRIMARY KEY(Id)
)
")]
    public class Extractor : ILanguageExtractor
    {
        public (XDocument, IEnumerable<IDiagnosticItem>) Extract(string source)
        {
            IList<IDiagnosticItem> diagnostics = new List<IDiagnosticItem>();
            XDocument document = null;

            TSql150Parser p = new TSql150Parser(false);
            TextReader r = new StringReader(source);
            TSqlFragment fragment = p.Parse(r, out IList<ParseError> l);

            if (l.Any())
            {
                foreach (var e in l)
                {
                    diagnostics.Add(new DiagnosticItem(e));
                }
            }
            else
            {
                document = TSQLVisitor.Generate(fragment);
            }

            return (document, diagnostics);
        }
    }
}
