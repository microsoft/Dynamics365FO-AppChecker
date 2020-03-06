// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace LanguageExtractorInterfaces
{
    /// <summary>
    /// This interface describes metadata that is assigned to each extractor.
    /// </summary>
    public interface ILanguageExtractorData
    {
        /// <summary>
        /// The name of the language
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The extension of files containing this language
        /// </summary>
        string Extension { get; }

        /// <summary>
        /// The version of the extractor
        /// </summary>
        string Version { get; }

        /// <summary>
        /// A sample in this language
        /// </summary>
        string Sample { get; }
    }
}
