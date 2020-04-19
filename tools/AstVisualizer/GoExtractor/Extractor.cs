
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoExtractor
{
    using System.ComponentModel.Composition;
    using System.Xml.Linq;
    using LanguageExtractorInterfaces;

    [Export(typeof(ILanguageExtractor)),
        ExportMetadata("Name", "Go"),
        ExportMetadata("Extension", "go"),
        ExportMetadata("Version", "1.0.0"),
        ExportMetadata("Sample", @"package main

import (
   ""fmt"" 
)

func Main(arg int) int {
    return arg + 1
}")]
    public class Extractor : ILanguageExtractor
    {
        public (XDocument, IEnumerable<IDiagnosticItem>) Extract(string source)
        {
            return (null, null);
        }
    }
}
