using System;
using System.Collections.Generic;

namespace CSharpAssemblyExtractor
{
    using ICSharpCode.Decompiler;
    using ICSharpCode.Decompiler.CSharp;
    using ICSharpCode.Decompiler.CSharp.OutputVisitor;
    using ICSharpCode.Decompiler.CSharp.Syntax;
    using ICSharpCode.Decompiler.DebugInfo;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;
    using System.CommandLine;

    internal class Program
    {
        static int Main(string[] args)
        {
            var assemblyFilenameOption = new Option<FileInfo>(
                new[] { "--assembly", "-a" },
                description: "The assembly to decompile into XML.");

            var verboseOption = new Option<bool>(
                new[] { "--verbose", "-v" },
                description: "Provide output as code is running.");

            var outputOption = new Option<DirectoryInfo>(
                new[] { "--output", "-o" },
                description: "The root directory where the XML files will be written. The directory must exist.");

            var rootCommand = new RootCommand
            {
                assemblyFilenameOption,
                verboseOption,
                outputOption,
            };

            rootCommand.Description = "Command to extract C# code into XML suitable for reasoning.";

            rootCommand.SetHandler((FileInfo assemblyFile, bool verbose, DirectoryInfo outputDirectory) =>
            {
                if (assemblyFile == null)
                {
                    Console.Error.WriteLine("No assembly provided to extract");
                    return;
                }

                if (outputDirectory == null)
                {
                    Console.Error.WriteLine("No target directory provided");
                    return;
                }

                // Set up the preferences for the decompilation of the IL into source.
                var settings = new DecompilerSettings() 
                { 
                    AlwaysUseBraces = true, 
                    ShowXmlDocumentation = true 
                };
                settings.CSharpFormattingOptions.IndentationString = "    ";

                var decompiler = new CSharpDecompiler(assemblyFile.FullName, settings);

                // Traverse all the types in the assembly
                foreach (var typeDefinition in decompiler.TypeSystem.MainModule.TopLevelTypeDefinitions)
                {
                    if (typeDefinition.Name.StartsWith("<"))
                        continue;

                    if (verbose)
                    {
                        Console.WriteLine($"Extracting {typeDefinition.FullName}.");
                    }
                    var syntaxTree = decompiler.DecompileType(typeDefinition.FullTypeName);

                    // This is needed to get the locations correctly set in the AST.
                    StringWriter w = new StringWriter();

                    var q = new TextWriterTokenWriter(w);
                    q.IndentationString = "    ";

                    TokenWriter tokenWriter = q;

                    tokenWriter = TokenWriter.WrapInWriterThatSetsLocationsInAST(tokenWriter);
                    syntaxTree.AcceptVisitor(new CSharpOutputVisitor(tokenWriter, settings.CSharpFormattingOptions));
                    var source = w.ToString();

                    var generator = new XmlGeneratorVisitor(assemblyFile.FullName, source);
                    syntaxTree.AcceptVisitor(generator);

                    File.WriteAllText(Path.Combine(outputDirectory.FullName, typeDefinition.FullTypeName.Name) + ".xml", generator.Document.ToString());
                }
            },
            assemblyFilenameOption, verboseOption, outputOption);

            return rootCommand.Invoke(args);
        }
    }
}
