// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.Collections.Generic;
using System.Xml.Linq;

namespace LanguageExtractorInterfaces
{
    /// <summary>
    /// This defines the interface that all language extractors must implement. It
    /// is used in the MEF framework to define Imports and Exports of such extractors.
    /// </summary>
    public interface ILanguageExtractor
    {
        (XDocument, IEnumerable<IDiagnosticItem>) Extract(string source);
    }
}
