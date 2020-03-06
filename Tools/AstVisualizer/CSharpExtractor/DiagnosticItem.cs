// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace CSharpExtractor
{
    using LanguageExtractorInterfaces;
    using Microsoft.CodeAnalysis;

    public class DiagnosticItem : IDiagnosticItem
    {
        private Diagnostic diagnostic;

        public int Line { get { return this.diagnostic.Location.GetLineSpan().StartLinePosition.Line + 1; } }
        public int Column { get { return this.diagnostic.Location.GetLineSpan().StartLinePosition.Character + 1; } }

        public int EndLine { get { return this.diagnostic.Location.GetLineSpan().EndLinePosition.Line + 1; } }
        public int EndColumn { get { return this.diagnostic.Location.GetLineSpan().EndLinePosition.Character +1 ; } }

        public string Id { get { return this.diagnostic.Id; } }

        public string Message { get { return this.diagnostic.GetMessage(); } }

        public string Severity { get { return this.diagnostic.Severity.ToString(); } }

        public DiagnosticItem(Diagnostic diagnostic)
        {
            this.diagnostic = diagnostic;
        }

        public override string ToString()
        {
            return Id + ": " + Message;
        }
    }
}
