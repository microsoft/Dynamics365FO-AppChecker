// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace LanguageExtractorInterfaces
{
    /// <summary>
    /// This defines the interface that all language extractors must implement. It
    /// is used in the MEF framework to define Imports and Exports of such extractors.
    /// </summary>
    public interface IDiagnosticItem
    {
        /// <summary>
        /// The start line where the diagnostic message applies, numbered from 1
        /// </summary>
        int Line { get; }

        /// <summary>
        /// The start column where the diagnostic message applies, numbered from 1
        /// </summary>
        int Column { get; }

        /// <summary>
        /// The end line where the diagnostic message applies, numbered fron 1
        /// </summary>
        int EndLine { get; }

        /// <summary>
        /// The end column where the diagnostic message applies, numbered from 1
        /// </summary>
        int EndColumn { get; }

        /// <summary>
        /// The unique identification of the diagnostic
        /// </summary>
        string Id { get; }

        /// <summary>
        /// The human readable message text
        /// </summary>
        string Message { get; }

        /// <summary>
        /// The diagnostic severity, e.g. warning, error, informational
        /// </summary>
        string Severity { get; }
    }
}
