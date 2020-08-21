// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;

namespace AstVisualizer
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Xml;
    using System.Xml.Linq;
    using ICSharpCode.AvalonEdit.Highlighting;
    using ICSharpCode.SharpDevelop.Editor;
    using LanguageExtractorInterfaces;
    using Microsoft.Win32;
    using Saxon.Api;

    public class ViewModel : INotifyPropertyChanged
    {
        private readonly ICommand executeQueryCommand;
        public ICommand ExecuteQueryCommand => this.executeQueryCommand;

        public static ICommand ExecuteExtractionCommand { get; private set; }
        public static ICommand ExitCommand { get; private set; }
        public static ICommand OpenSourceCommand { get; private set; }
        public static ICommand SaveSourceCommand { get; private set; }

        public static ICommand OpenAstCommand { get; private set; }
        public static ICommand SaveAstCommand { get; private set; }

        public static ICommand OpenQueryCommand { get; private set; }
        public static ICommand SaveQueryCommand { get; private set; }
        public static ICommand SaveQueryResultsCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private string caretPositionString = string.Empty;

        private readonly MainWindow view;
        private readonly Model model;

        /// <summary>
        /// Loads the highlighting mode from the embedded resource
        /// </summary>
        /// <param name="mode">The name of the mode to load.</param>
        /// <returns>The Hightlighting definition for the given language, or null 
        /// if there is no highlighting definition by the given name.
        /// </returns>
        public static IHighlightingDefinition LoadHighlightDefinition(string mode)
        {
            var assembly = Assembly.GetExecutingAssembly();
            Stream syntaxModeStream = null;
            try
            {
                syntaxModeStream = assembly.GetManifestResourceStream("AstVisualizer.Resources." + mode);

                using (var xshd_reader = new XmlTextReader(syntaxModeStream)
                {
                    XmlResolver = null,
                    DtdProcessing = DtdProcessing.Prohibit
                })
                {
                    syntaxModeStream = null;
                    return ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(xshd_reader, HighlightingManager.Instance);
                }
            }
            finally
            {
                if (syntaxModeStream != null)
                    syntaxModeStream.Dispose();

            }
        }

        public ViewModel(MainWindow v, Model model)
        {
            this.view = v;
            this.model = model;

            // Register the types for hightlighting. C#, VB.NET and TSQL are defined 
            // internally by the editor, so they do not need to be registered.
            HighlightingManager.Instance.RegisterHighlighting("Go", new[] { ".go" }, LoadHighlightDefinition("Go.xshd"));

            Properties.Settings.Default.PropertyChanged += (object sender, PropertyChangedEventArgs e) =>
            {
                // Save all the user's settings
                Properties.Settings.Default.Save();
            };

            ExitCommand = new RelayCommand(
                p => { Application.Current.Shutdown(); });

            OpenSourceCommand = new RelayCommand(
                p =>
                {
                    var (extension, l) = this.Language;

                    OpenFileDialog dlg = new OpenFileDialog();
                    dlg.DefaultExt = extension; // Default file extension
                    dlg.Filter = l + "  Files |*" + extension; // Filter files by extension

                    // Show open file dialog box
                    Nullable<bool> result = dlg.ShowDialog();

                    // Process save file dialog box results
                    if (result.Value)
                    {
                        view.SourceEditor.Text = File.ReadAllText(dlg.FileName);
                    }
                }
            );
            SaveSourceCommand = new RelayCommand(
                p =>
                {
                    var (e, l) = this.Language;

                    SaveFileDialog dlg = new SaveFileDialog
                    {
                        FileName = "document", // Default file name
                        DefaultExt = e, // Default file extension
                        Filter = l + "  Files |*" + e // Filter files by extension
                    };

                    // Show save file dialog box
                    Nullable<bool> result = dlg.ShowDialog();

                    // Process save file dialog box results
                    if (result.Value)
                    {
                        // Save document
                        File.WriteAllText(dlg.FileName, view.SourceEditor.Text);
                    }
                }
            );

            OpenAstCommand = new RelayCommand(
                p =>
                {
                    OpenFileDialog dlg = new OpenFileDialog
                    {
                        DefaultExt = ".xml", // Default file extension
                        Filter = "XML Files |*.xml" // Filter files by extension
                    };

                    // Show open file dialog box
                    Nullable<bool> result = dlg.ShowDialog();

                    // Process save file dialog box results
                    if (result.Value)
                    {
                        view.ResultsEditor.Text = File.ReadAllText(dlg.FileName);
                    }
                }
            );
            SaveAstCommand = new RelayCommand(
                p =>
                {
                    SaveFileDialog dlg = new SaveFileDialog
                    {
                        FileName = "document", // Default file name
                        DefaultExt = ".xml", // Default file extension
                        Filter = "XML Files |*.xml" // Filter files by extension
                    };

                    // Show save file dialog box
                    Nullable<bool> result = dlg.ShowDialog();

                    // Process save file dialog box results
                    if (result.Value)
                    {
                        // Save document
                        File.WriteAllText(dlg.FileName, view.ResultsEditor.Text);
                    }
                }
            );
            OpenQueryCommand = new RelayCommand(
                p =>
                {
                    OpenFileDialog dlg = new OpenFileDialog
                    {
                        DefaultExt = ".xq", // Default file extension
                        Filter = "xq Files |*.xq" // Filter files by extension
                    };

                    // Show open file dialog box
                    Nullable<bool> result = dlg.ShowDialog();

                    // Process save file dialog box results
                    if (result.Value)
                    {
                        view.QueryEditor.Text = File.ReadAllText(dlg.FileName);
                    }
                }
            );
            SaveQueryCommand = new RelayCommand(
                p =>
                {
                    SaveFileDialog dlg = new SaveFileDialog();
                    dlg.FileName = "document"; // Default file name
                    dlg.DefaultExt = ".xq"; // Default file extension
                    dlg.Filter = "xq Files |*.xq"; // Filter files by extension

                    // Show save file dialog box
                    Nullable<bool> result = dlg.ShowDialog();

                    // Process save file dialog box results
                    if (result.Value)
                    {
                        // Save document
                        File.WriteAllText(dlg.FileName, view.QueryEditor.Text);
                    }
                }
            );

            SaveQueryResultsCommand = new RelayCommand(
                p =>
                {
                    SaveFileDialog dlg = new SaveFileDialog
                    {
                        FileName = "document", // Default file name
                        DefaultExt = ".xml", // Default file extension
                        Filter = "XML Files |*.xml" // Filter files by extension
                    };

                    // Show save file dialog box
                    Nullable<bool> result = dlg.ShowDialog();

                    // Process save file dialog box results
                    if (result.Value)
                    {
                        // Save document
                        File.WriteAllText(dlg.FileName, view.QueryEditor.Text);
                    }
                }
            );

            ExecuteExtractionCommand = new RelayCommand(
                p => 
                {
                    this.Status = "";
                    this.Result = "";

                    // Get the current language extractor from the model
                    ILanguageExtractor extractor = this.Languages
                        .Where(l => l.Metadata.Name == Properties.Settings.Default.CurrentLanguage)
                        .Select(l => l.Value).FirstOrDefault();

                    (XDocument doc, IEnumerable<IDiagnosticItem> diagnostics) = extractor.Extract(this.view.SourceEditor.Text);

                    this.Result = doc != null ? doc.ToString() : "";

                    // Remove all the entries in the error list and the squigglies
                    this.DiagnosticItems.Clear();
                    this.view.SourceEditorTextMarkerService.RemoveAll(m => true);

                    if (diagnostics != null)
                    {
                        foreach (var d in diagnostics)
                        {
                            this.DiagnosticItems.Add(d);
                            int startOffset = this.view.SourceEditor.Document.GetOffset(d.Line, d.Column);
                            int endOffset = this.view.SourceEditor.Document.GetOffset(d.EndLine, d.EndColumn);
                            int length = endOffset - startOffset;
                            ITextMarker marker = this.view.SourceEditorTextMarkerService.Create(startOffset, length);
                            marker.MarkerTypes = TextMarkerTypes.SquigglyUnderline;
                            marker.MarkerColor = d.Severity == "Error" ? Colors.Red : Colors.Green;
                        }
                    }
                }
            );

            view.InputBindings.Add(new InputBinding(OpenSourceCommand, new KeyGesture(Key.O, ModifierKeys.Control)));
            view.InputBindings.Add(new InputBinding(SaveSourceCommand, new KeyGesture(Key.S, ModifierKeys.Control)));
            view.InputBindings.Add(new InputBinding(ExecuteExtractionCommand, new KeyGesture(Key.E, ModifierKeys.Control)));

            this.executeQueryCommand = new RelayCommand(
                p =>
                {
                    Processor processor = new Processor();
                    StringReader sr = new StringReader(this.view.ResultsEditor.Text);
                    XmlReader reader = XmlReader.Create(sr, new XmlReaderSettings()
                    {
                        ValidationFlags = System.Xml.Schema.XmlSchemaValidationFlags.None
                    });

                    XdmNode doc = null;
                    try
                    {
                        doc = processor.NewDocumentBuilder().Build(reader);
                    }
                    catch (Exception)
                    {
                    }

                    XQueryCompiler compiler = processor.NewXQueryCompiler();

                    // Add any namespaces needed...
                    // compiler.DeclareNamespace("saxon", "http://saxon.sf.net/");

                    try
                    {
                        XQueryExecutable exp = compiler.Compile(this.view.QueryEditor.Text);
                        XQueryEvaluator eval = exp.Load();

                        // The context node is always the root document.
                        if (doc != null)
                            eval.ContextItem = doc;

                        Serializer qout = processor.NewSerializer();
                        qout.SetOutputProperty(Serializer.METHOD, "xml");
                        qout.SetOutputProperty(Serializer.OMIT_XML_DECLARATION, "yes");
                        // Not available: qout.SetOutputProperty(Serializer.SAXON_INDENT_SPACES, "2");
                        // Do not put attributes on separate lines:
                        qout.SetOutputProperty(Serializer.INDENT, "no");

                        // Put the result of the XQuery query into a memory stream:
                        var output = new MemoryStream();
                        qout.SetOutputStream(output);

                        // Run the query:
                        eval.Run(qout);

                        // Harvest the result
                        output.Position = 0;
                        StreamReader resultReader = new StreamReader(output);
                        string result = resultReader.ReadToEnd();

                        // Normalize the strange looking output generated by the serializer
                        // if it is XML
                        try
                        {
                            var d = XDocument.Parse(result);
                            this.view.QueryResultsEditor.Text = d.ToString(SaveOptions.None);
                        }
                        catch
                        {
                            this.view.QueryResultsEditor.Text = result;
                        }
                    }
                    catch (Exception e)
                    {
                        this.view.QueryResultsEditor.Text = e.Message;

                    }
                },
                p => { return true; });
        }

        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly ObservableCollection<IDiagnosticItem> diagnosticItems = new ObservableCollection<IDiagnosticItem>();
        public ObservableCollection<IDiagnosticItem> DiagnosticItems
        {
            get { return this.diagnosticItems; }
        }

        public string CaretPositionString
        {
            get
            {
                return this.caretPositionString;
            }
            set
            {
                if (this.caretPositionString != value)
                {
                    this.caretPositionString = value;
                    this.OnPropertyChanged(nameof(CaretPositionString));
                }
            }
        }

        private string status = "";
        public string Status
        {
            get { return this.status; }
            set
            {
                if (this.status != value)
                {
                    this.status = value;
                    this.OnPropertyChanged(nameof(Status));
                }
            }
        }

        public string Result
        {
            get { return this.view.ResultsEditor.Text; }
            set
            {
                this.view.ResultsEditor.Text = value;
            }
        }

        public IEnumerable<Lazy<ILanguageExtractor, ILanguageExtractorData>> Languages
        {
            get { return this.model.Extractors;  }
        }
         
        public (string extension, string name) Language
        {
            get {

                ILanguageExtractorData extractorData = this.Languages
                    .Where(l => l.Metadata.Name == Properties.Settings.Default.CurrentLanguage)
                    .Select(l => l.Metadata).FirstOrDefault();

                return (extractorData.Extension, extractorData.Name);  }
        }

        public IEnumerable<string> LanguageNames
        {
            get { return this.model.Extractors.Select(e => e.Metadata.Name); }
        }

        public void LanguageSelectionChanged()
        {
            ILanguageExtractorData extractorData = this.Languages
                .Where(l => l.Metadata.Name == Properties.Settings.Default.CurrentLanguage)
                .Select(l => l.Metadata).FirstOrDefault();

            // Set the highlighting to the registered language
            this.view.SourceEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition(extractorData.Name);
            this.view.SourceEditor.Text = extractorData.Sample;
        }
    }
}
