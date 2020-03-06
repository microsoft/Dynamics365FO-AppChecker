// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpExtractor
{
    using LanguageExtractorInterfaces;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using System.ComponentModel.Composition;
    using System.IO.Pipes;
    using System.Xml.Linq;

    [Export(typeof(ILanguageExtractor)),
        ExportMetadata("Name", "C#"),
        ExportMetadata("Extension", "cs"),
        ExportMetadata("Version", "1.0.0"),
        ExportMetadata("Sample", @"using System;
namespace NS1.NS2
{
    public class C
    {
        private string banana;
        int foo(int parm=1) 
        { 
            int a,b,c = 9;
            return a + 2; 
        }
    }
}")]
    public class Extractor : ILanguageExtractor
    {
        public (XDocument, IEnumerable<IDiagnosticItem>) Extract(string source)
        {
            var options = new CSharpParseOptions();
            var tree = CSharpSyntaxTree.ParseText(source, options);

            var trees = new[] { tree };

            var referenceList = new MetadataReference[]
            {
                // Add mscorlib
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                // Add System
                MetadataReference.CreateFromFile(typeof(Uri).Assembly.Location),
                // Add System.Core
                MetadataReference.CreateFromFile(typeof(PipeDirection).Assembly.Location),
            };

            var compilation = CSharpCompilation.Create("MyAssembly.dll", trees,
                references: referenceList,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            IList<IDiagnosticItem> diagnostics = new List<IDiagnosticItem>();
            foreach (var diagnostic in compilation.GetDiagnostics())
            {
                var item = new DiagnosticItem(diagnostic);
                diagnostics.Add(item);
            }

            var document = new XDocument();
            var compilationElement = new XElement("Compilation",
                new XAttribute("Version", "0.1"),
                new XAttribute("Language", "C#"),
                new XAttribute("Assembly", compilation.AssemblyName));

            document.Add(compilationElement);
            var diagnosticsElement = new XElement("Diagnostics");
            compilationElement.Add(diagnosticsElement);

            // Add nodes with the diagnostics
            foreach (var diagnostic in compilation.GetDiagnostics())
            {
                var item = new DiagnosticItem(diagnostic);
                diagnosticsElement.Add(new XElement("Diagnostic",
                    new XAttribute("Message", item.Message),
                    new XAttribute("Id", item.Id),
                    new XAttribute("Severity", item.Severity),
                    new XAttribute("StartLine", item.Line),
                    new XAttribute("StartCol", item.Column),
                    new XAttribute("EndLine", item.EndLine),
                    new XAttribute("EndCol", item.EndColumn)));
            }

            // Add Options element. Individual options are attributes.
            var optionsElement = new XElement("Options");
            compilationElement.Add(optionsElement);
            CSharpCompilationOptions compOptions = compilation.Options;
            string[,] optionData =
            {
                {"AllowUnsafe", compOptions.AllowUnsafe.ToString()},
                {"AssemblyIdentityComparer", compOptions.AssemblyIdentityComparer.ToString()},
                {"CheckOverflow", compOptions.CheckOverflow.ToString()},
                {"ConcurrentBuild", compOptions.ConcurrentBuild.ToString()},
                {"CryptoKeyContainer", compOptions.CryptoKeyContainer == null ? "null" : compOptions.CryptoKeyContainer},
                {"CryptoKeyFile", compOptions.CryptoKeyFile == null ? "null" : compOptions.CryptoKeyFile},
                {"CryptoPublicKeyLength", compOptions.CryptoPublicKey.Length.ToString()},
                {"DelaySign", compOptions.DelaySign.ToString()},
                {"Deterministic", compOptions.Deterministic.ToString()},
                {"Errors", compOptions.Errors.Count().ToString()},
                {"GeneralDiagnosticOption", compOptions.GeneralDiagnosticOption.ToString()},
                {"Language", compOptions.Language},
                {"MainTypeName", compOptions.MainTypeName == null ? "null" : compOptions.MainTypeName},
                {"MetadataImportOptions", compOptions.MetadataImportOptions.ToString()},
                {"MetadataReferenceResolver", compOptions.MetadataReferenceResolver == null ? "null" : compOptions.MetadataReferenceResolver.ToString()},
                {"ModuleName", compOptions.ModuleName == null ? "null" : compOptions.ModuleName},
                {"NullableContextOptions", compOptions.NullableContextOptions.ToString()},
                {"OptimizationLevel", compOptions.OptimizationLevel.ToString()},
                {"OutputKind", compOptions.OutputKind.ToString()},
                {"Platform", compOptions.Platform.ToString()},
                {"PublicSign", compOptions.PublicSign.ToString()},
                {"ReportSuppressedDiagnostics", compOptions.ReportSuppressedDiagnostics.ToString()},
                {"SourceReferenceResolver", compOptions.SourceReferenceResolver == null ? "null" : compOptions.SourceReferenceResolver.ToString()},
                {"SpecificDiagnosticOptionsCount", compOptions.SpecificDiagnosticOptions.Count().ToString()},
                {"StrongNameProvider", compOptions.StrongNameProvider == null ? "null" : compOptions.StrongNameProvider.ToString()},
                {"UsingsCount", compOptions.Usings.Count().ToString()},
                {"WarningLevel", compOptions.WarningLevel.ToString()}
            };

            for (int item = 0; item < optionData.GetLength(0); item++)
            {
                optionsElement.Add(new XAttribute(optionData[item, 0], optionData[item, 1]));
            }

            // Add the SpecificDiagnosticOptions
            if (compOptions.SpecificDiagnosticOptions.Count() > 0)
            {
                var specificElement = new XElement("SpecificDiagnosticOptions");
                optionsElement.Add(specificElement);
                foreach (var item in compOptions.SpecificDiagnosticOptions)
                {
                    specificElement.Add(new XElement("SpecificDiagnosticOption",
                        new XAttribute("name", item.Key),
                        new XAttribute("value", item.Value.ToString())));
                }
            }

            // Add the Usings
            if (compOptions.Usings.Count() > 0)
            {
                var usingElement = new XElement("Usings");
                optionsElement.Add(usingElement);
                foreach (var item in compOptions.Usings)
                {
                    usingElement.Add(new XElement("Using", new XAttribute("value", item)));
                }
            }

            // Add the references to the compilation.
            var referencesElement = new XElement("References");
            compilationElement.Add(referencesElement);
            foreach (var reference in compilation.ReferencedAssemblyNames)
            {
                referencesElement.Add(new XElement("Reference", new XAttribute("DisplayName", reference.GetDisplayName())));
            }

            // Add each of the trees (i.e. compilation units) to the compilation node.
            foreach (var atree in trees)
            {
                var walker = new CSharpConceptWalker(compilation, atree, document);
                walker.Visit(atree.GetRoot());
            }

            return (document, diagnostics);
        }
    }
}
