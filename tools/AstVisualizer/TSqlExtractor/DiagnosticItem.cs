// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace TSqlExtractor
{
    using LanguageExtractorInterfaces;
    using Microsoft.SqlServer.TransactSql.ScriptDom;

    public class DiagnosticItem : IDiagnosticItem
    {
        private ParseError diagnostic;

        public int Line { get { return this.diagnostic.Line; } }
        public int Column { get { return this.diagnostic.Column; } }

        public int EndLine { get { return this.diagnostic.Line; } }
        public int EndColumn { get { return this.diagnostic.Column+1; } }

        public string Id { get { return this.diagnostic.Number.ToString(); } }

        public string Message { get { return this.diagnostic.Message; } }

        public string Severity { get { return "Error"; } }

        public DiagnosticItem(ParseError diagnostic)
        {
            this.diagnostic = diagnostic;
        }

        public override string ToString()
        {
            return Id + ": " + Message;
        }
    }
}
