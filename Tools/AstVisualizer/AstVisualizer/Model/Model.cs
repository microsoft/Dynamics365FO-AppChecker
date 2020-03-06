// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace AstVisualizer
{
    using LanguageExtractorInterfaces;
    using System.IO;

    public class Model
    {
        [ImportMany]
        public IEnumerable<Lazy<ILanguageExtractor, ILanguageExtractorData>> Extractors { get; set; }

        public Model()
        {
            var catalog = new AggregateCatalog();

            // Locating the extensions (or plugins) that provide extraction.
            // We use a local directory designated by the environment variable called ASTEXTRACTORS.
            // It a value is provided, it is interpreted as a ; separated list of directories that are
            // searched for assemblies.

            var extractorsPath = Environment.GetEnvironmentVariable("ASTEXTRACTORS");
            if (extractorsPath != null)
            {
                var parts = extractorsPath.Split(';');
                foreach (var part in parts)
                {
                    if (Directory.Exists(part))
                    {
                        catalog.Catalogs.Add(new DirectoryCatalog(part));
                    }
                }
            }

            // The build script for the extractors are such that the assemblies end in the same
            // directory as this executable. We do this by having a reference from this app to the
            // extractors, which is not very clean, but it causes the build system to place the
            // assemblies in the same directory as the app.
            // Alternatively, individual assemblies can be added with .Add(new AssemblyCatalog(myAssembly)) 
            catalog.Catalogs.Add(new DirectoryCatalog("."));

            var container = new CompositionContainer(catalog);
            container.SatisfyImportsOnce(this);
        }
    }
}
