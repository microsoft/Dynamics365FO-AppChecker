// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.Win32;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml;
using System.ComponentModel;
using System.Xml.Linq;
using System.Windows.Threading;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace XppReasoningWpf
{
    using System.Xml.XPath;
    using BaseXInterface;
    using Wpf.Controls;

    // Authoring a shell extension to show xq files is documented here: https://www.codeproject.com/articles/533948/net-shell-extensions-shell-preview-handlers
    // How to assign this as the default handler for .xq files is described here: https://docs.microsoft.com/en-us/visualstudio/extensibility/specifying-file-handlers-for-file-name-extensions?view=vs-2019

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Model Model { get; set; }

        private readonly ViewModels.ViewModel ViewModel;

        public MainWindow()
        {
            SplashScreen splash = new SplashScreen("Images/SplashScreen with socrates.png");
            splash.Show(false);
            Thread.Sleep(2000);

            this.Model = new Model();
            this.ViewModel = new ViewModels.ViewModel(this, this.Model);
            this.DataContext = this.ViewModel;

            Properties.Settings.Default.PropertyChanged += SettingsChanged;

            InitializeComponent();

            // For some reason this is required for getting the commandparameter 
            // binding mechanism to work for menu items
            this.ExecuteQueryMenuItem.DataContext = this.ViewModel;

            this.ResultsEditor.TextArea.Caret.PositionChanged += ResultNavigated;

            // var isOnline = this.Model.Server.IsServerOnline();

            // If this is not zero, the combobox below disappears...
            splash.Close(TimeSpan.FromSeconds(0));

            ConnectionWindow w = new ConnectionWindow(this.Model);
            var result = w.ShowDialog();

            if (!(result.HasValue && result.Value))
            {
                Environment.Exit(0);
                return;
            }

            try
            {
                // This call may throw when Basex complains when connecting or
                // asking for the active databases.
                this.PopulateUIFromServerAsync();
            }
            catch(Exception e)
            {
                this.Model.Status = e.Message;
            }

            // Create the first query page
            this.ViewModel.CreateNewQueryTabItem();
        }

        /// <summary>
        /// Called when any of the properties are changed. We choose to save the 
        /// values every time, so changes survive crashes and unexpected closedowns.
        /// </summary>
        /// <param name="sender">Not used.</param>
        /// <param name="e">Not used.</param>
        private void SettingsChanged(object sender, PropertyChangedEventArgs e)
        {
            // Save all the user's settings
            Properties.Settings.Default.Save();
        }

        private async void PopulateUIFromServerAsync()
        {
            Model.Databases = await this.Model.Server.GetDatabasesAsync();

            // Initialize the model dropdown from the value stored in the settings
            // If that value no longer exists, set it to the first one.
            var selectedModel = Properties.Settings.Default.SelectedModel;
            if (selectedModel.Length > 0)
            {
                // Check if it still exists...
                // this.ModelDropdown.SelectedItem = this.model.Databases.FirstOrDefault();
                this.Model.SelectedDatabase = this.Model.Databases.FirstOrDefault();
            }
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            this.queryTabPage.SelectedIndex = 0;
        }

        /// <summary>
        /// Allows the user the possibility of not exiting if there are
        /// unsaved changes.
        /// </summary>
        /// <param name="e">The parameter that decides whether or not to veto the closing.</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            var dirtyEditors = false;
            foreach (Wpf.Controls.TabItem tab in this.queryTabPage.Items)
            {
                var editor = tab.Content as QueryEditor;
                if (editor.IsModified)
                {
                    dirtyEditors = true;
                    break;
                }
            }

            if (dirtyEditors)
            {
                // Ask the user if he wants to exit anyway.
                MessageBoxResult result = MessageBox.Show("There are one or more unsaved changes. Exit anyway?", "Unsaved changes", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                e.Cancel = result == MessageBoxResult.No;
            }
            else
            {
                e.Cancel = false;
            }

            base.OnClosing(e);
        }

        /// <summary>
        /// Called when the system is irretrievably closing down
        /// </summary>
        /// <param name="e">Not used</param>
        protected async override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            await this.ViewModel.ClosedownAsync();
        }

        private static bool IsCompoundName(string name)
        {
            var parts = name.Split(':');
            return parts.Length == 2;
        }

        private async Task<string> GetSourceDocAsync(string query)
        {
            using var session = await this.Model.GetSessionAsync(this.Model.SelectedDatabase.Name);
            return await session.DoQueryAsync(query);
        }

        /// <summary>
        /// Returns the source code for the artifact designated by the given name.
        /// </summary>
        /// <param name="name">The name can be either a compound name or a simple name.</param>
        /// <returns>If no artifact by the given name can be found, null is returned.</returns>
        private async Task<string> GetSourceAsync(string name, string language)
        {
            string sourceDoc = string.Empty;

            if (!string.IsNullOrEmpty(name))
            {
                if (language == "X++")
                {
                    if (IsCompoundName(name))
                    {
                        var parts = name.Split(':');
                        var kind = parts[0];

                        if (kind == "class" || kind == "table" || kind == "interface" || kind == "map" || kind == "view")
                        {
                            string xquery = string.Format(@"for $c in (/Class | /Table | /Interface | /Map | /View)[@Artifact='{0}'] where position()=1", name) + " return <Source Language='{$c/@Language}'>{$c/@Source}</Source>";
                            sourceDoc = await this.GetSourceDocAsync(xquery);
                        }
                        else if (kind == "form")
                        {
                            string xquery = string.Format(@"for $c in /Form[@Artifact='{0}'] where position()=1", name) + " return <Source Language='{$c/@Language}'>{$c/@Source}</Source>";
                            sourceDoc = await this.GetSourceDocAsync(xquery);
                        }
                        else if (kind == "query")
                        {
                            string xquery = string.Format(@"for $c in /Query[@Artifact='{0}']", name) + " return <Source Language='{$c/@Language}'>{$c/@Source}</Source>";
                            sourceDoc = await GetSourceDocAsync(xquery);
                        }
                    }
                    else
                    {
                        // It wasn't a compound name, so assume it is a class, table or interface.
                        string xquery = string.Format(@"for $c in (/Class | /Table | /Interface)[@Name='{0}'] where position()=1", name) + " return <Source Language='{$c/@Language}'>{$c/@Source}</Source>";
                        sourceDoc = await GetSourceDocAsync(xquery);
                    }
                }
                else if (language == "C#")
                {
                    string xquery;
                    if (IsCompoundName(name))
                    {
                        var parts = name.Split(':');
                        var kind = parts[0];

                        if (kind == "Type") // This comes from information extracted from assembly
                        {
                            xquery = string.Format(@"for $c in /Type[@Artifact='{0}']", name) + " return <Source Language='{$c/@Language}'>{$c/@Source}</Source>";
                        }
                        else
                        {
                            xquery = string.Format(@"for $c in //CompilationUnit for $cd in $c//{1}[@FullName='{0}']", name, kind) + " return <Source Language='{$c/@Language}'>{$c/@Source}</Source>";
                        }
                    }
                    else
                    {
                        // We assume that this is a class declaration.
                        xquery = string.Format(@"for $c in //CompilationUnit for $cd in $c//ClassDeclaration[@FullName='{0}'] where position()=1", name) + @" return <Source Language='{$c/@Language}'>{$c/@Source}</Source>";
                    }

                    sourceDoc = await GetSourceDocAsync(xquery);
                }
                else
                {
                    // The language is not X++, so no defaults apply.
                    string xquery = string.Format(@"let $c := /*[@Artifact='{0}' and @Language='{1}']", name, language) + " return <Source>{$c/@Source}</Source>";
                    sourceDoc = await GetSourceDocAsync(xquery);
                }

                if (sourceDoc.Length > 0)
                {
                    var doc = XDocument.Parse(sourceDoc);
                    if (doc != null && doc.Element("Source").Attribute("Source") != null)
                        return doc.Element("Source").Attribute("Source").Value;
                }
            }

            return null;
        }

        private static bool IsToplevelArtifact(XElement node)
        {
            var name = node.Name.LocalName;
            return string.Compare(name, "Class", StringComparison.OrdinalIgnoreCase) == 0
                || string.Compare(name, "Table", StringComparison.OrdinalIgnoreCase) == 0
                || string.Compare(name, "Query", StringComparison.OrdinalIgnoreCase) == 0
                || string.Compare(name, "Map", StringComparison.OrdinalIgnoreCase) == 0
                || string.Compare(name, "View", StringComparison.OrdinalIgnoreCase) == 0
                || string.Compare(name, "Form", StringComparison.OrdinalIgnoreCase) == 0
                || string.Compare(name, "Type", StringComparison.OrdinalIgnoreCase) == 0;
        }

        /// <summary>
        /// Called when the user changes the position in the result view.
        /// </summary>
        /// <param name="sender">Not used.</param>
        /// <param name="a">Not used.</param>
        private void ResultNavigated(object sender, EventArgs a)
        {
            var resultDocument = this.ResultsEditor.XmlDocument;

            if (resultDocument != null)
            {
                var caret = sender as ICSharpCode.AvalonEdit.Editing.Caret;

                // Get the current position and match it to an element in the document
                foreach (XElement element in resultDocument.Descendants())
                {
                    IXmlLineInfo li = (IXmlLineInfo)element;
                    if (li.LineNumber == caret.Line)
                    {
                        // Got the element corresponding to the current line number. 
                        // Show the information in the source editor.

                        // Find the root element. Either it is an element designating an X++ artifact
                        // like Class or Table, or it is a node that must be interpreted to find the 
                        // relevant source artifact.
                        XElement rootArtifact = element;
                        while (rootArtifact != null && !IsToplevelArtifact(rootArtifact))
                        {
                            rootArtifact = rootArtifact.Parent;
                        }

                        if (rootArtifact != null)
                        {
                            // It is an X++ artifact, like a class, table etc. Find the source text and show it.

                            // Find the closest position via the attribute
                            XElement positionElement = element;
                            int sl = -1, sc = 1, el = 1, ec = 1;

                            FindPositionsInSelfOrAncestor(positionElement, ref sl, ref sc, ref el, ref ec);

                            if (rootArtifact.Attribute("Name") != null && !string.IsNullOrEmpty(rootArtifact.Attribute("Name").Value))
                                this.ShowSourceAt(rootArtifact.Name.LocalName.ToLower() + ":" + rootArtifact.Attribute("Name").Value, "X++", sl, sc, el, ec);
                        }
                        else
                        {
                            // It was not an X++ artifact that can be shown directly in the editor. Determine the 
                            // information needed to show the source for the artifact.
                            var positionElement = element;

                            // See if the cursor is inside a node of the form:
                            //<Diagnostic>
                            //  ...
                            // <Path>dynamics://{$typeNamePair[1]}/{$typeNamePair[2]}/Method/{string($m/@Name)}</Path>
                            //  <Line>1</Line>
                            //  <Column>2</Column>
                            //  <EndLine>10</EndLine>
                            //  <EndColumn>16</EndColumn>
                            //</Diagnostic>
                            var diagnosticElement = positionElement.XPathSelectElement("ancestor-or-self::Diagnostic");
                            if (diagnosticElement != null)
                            {
                                bool ok = true;
                                int sl = -1, sc = 1, el = 1, ec = 1;

                                var line = diagnosticElement.XPathSelectElement("Line");
                                if (line != null)
                                    ok &= int.TryParse(line.Value, out sl);

                                var column = diagnosticElement.XPathSelectElement("Column");
                                if (column != null)
                                    ok &= int.TryParse(column.Value, out sc);

                                var endline = diagnosticElement.XPathSelectElement("EndLine");
                                if (endline != null)
                                    ok &= int.TryParse(endline.Value, out el);

                                var endcolumn = diagnosticElement.XPathSelectElement("EndColumn");
                                if (endcolumn != null)
                                    ok &= int.TryParse(endcolumn.Value, out ec);


                                if (ok)
                                {
                                    var pathNode = diagnosticElement.XPathSelectElement("Path");
                                    if (pathNode != null)
                                    {
                                        var path = pathNode.Value;
                                        if (path.StartsWith("dynamics://"))
                                        {
                                            path = path.Remove(0, "dynamics://".Length);
                                            var parts = path.Split('/');
                                            string artifact = parts[0] + ":" + parts[1];
                                            this.ShowSourceAt(artifact, "X++", sl, sc, el, ec);
                                        }
                                    }
                                }

                            }

                            // Find the nearest ancestor node with a <... Class="MyClass" .../> where the name
                            // can denote either a class or a table (since they share the same namespace),
                            // or a node with <... Artifact="class:MyClass" .../>
                            while (positionElement != null && positionElement.HasAttributes)
                            {
                                if (positionElement.Attribute("Class") != null)
                                {
                                    var name = positionElement.Attribute("Class").Value;

                                    // Check if there already is a tab page showing this class. If there is,
                                    // then make it the active editor and position correctly as per the XML element.
                                    // If there is no editor open for this class, open one and position.
                                    int sl = -1, sc = -1, el = -1, ec = -1;
                                    FindPositionsInSelf(positionElement, ref sl, ref sc, ref el, ref ec);

                                    this.ShowSourceAt(name, "X++", sl, sc, el, ec);
                                    return;
                                }
                                else if (positionElement.Attribute("Table") != null)
                                {
                                    var name = positionElement.Attribute("Table").Value;

                                    int sl = -1, sc = -1, el = -1, ec = -1;
                                    FindPositionsInSelf(positionElement, ref sl, ref sc, ref el, ref ec);

                                    this.ShowSourceAt(name, "X++", sl, sc, el, ec);
                                    return;
                                }
                                else if (positionElement.Attribute("Artifact") != null)
                                {
                                    string artifact = positionElement.Attribute("Artifact").Value;

                                    if (positionElement.Attribute("Language") != null)
                                    {
                                        string language = positionElement.Attribute("Language").Value;

                                        int sl = -1, sc = -1, el = -1, ec = -1;
                                        FindPositionsInSelf(positionElement, ref sl, ref sc, ref el, ref ec);

                                        this.ShowSourceAt(artifact, language, sl, sc, el, ec);
                                        return;
                                    }
                                    else
                                    {
                                        // There is no language provided. Assume X++ in this case. 
                                        // Artifact contains an encoded definition of the kind of artifact (i.e.
                                        // class, table, query, or form) that is requested, like Artifact="form:MyForm".
                                        var parts = artifact.Split(':');
                                        if (parts.Length == 2)
                                        {
                                            var kind = parts[0].ToLower();

                                            int sl = -1, sc = -1, el = -1, ec = -1;
                                            FindPositionsInSelf(positionElement, ref sl, ref sc, ref el, ref ec);

                                            if (kind == "form" 
                                             || kind == "query" 
                                             || kind == "table" || kind == "map" || kind == "view"
                                             || kind == "class"
                                             || kind == "dataentity")
                                            {
                                                this.ShowSourceAt(artifact, "X++", sl, sc, el, ec);
                                                return;
                                            }
                                            else if (kind == "type")
                                            {
                                                this.ShowSourceAt(artifact, "C#", sl, sc, el, ec);
                                            }
                                        }
                                    }
                                }

                                positionElement = positionElement.Parent;
                            }
                        }

                        break;
                    }
                }
            }
            else
            {
                // The document was not created, due to an error: The result could
                // not be parsed as an XML document. Issue a message to the user:
                // this.Model.Status = "The XML document is not wellformed, so navigation and source code rendering is not possible.";
            }
        }

        /// <summary>
        /// Show the source code designated by the given name. The view is positioned around
        /// the given coordinates.
        /// </summary>
        /// <param name="name">The name of the artifact to show. Note that the name can 
        /// be either a simple name (like "MyClass") or a compound name (like "form:MyForm").
        /// If the simple name is used, then it is assumed that it is either a class, interface 
        /// or table, since these share the same namespace.</param>
        /// <param name="sl">The source line where the selection starts.</param>
        /// <param name="sc">The source column where the selection starts.</param>
        /// <param name="el">The source line where the selection ends.</param>
        /// <param name="ec">The source column where the selection ends.</param>
        private async void ShowSourceAt(string name, string language, int sl, int sc, int el, int ec)
        {
            if (Properties.Settings.Default.UseDynamicsProtocol)
            {
                var parts = name.Split(':');

                if (parts.Length == 2)
                {
                    // Only support class for now.
                    // dynamics://Open/Class/SrsReportRunController/Line/1413/Column/74/ToLine/1415/ToColumn/10 
                    string path = string.Format(CultureInfo.InvariantCulture, "dynamics://open/{0}/{1}/Line/{2}/Column/{3}/ToLine/{4}/ToColumn/{5}", "class", parts[1], sl, sc, el, ec);
                    System.Diagnostics.ProcessStartInfo info = new ProcessStartInfo()
                    {
                        CreateNoWindow = true,
                        FileName = path,
                    };
                    Process.Start(info);
                }

                return;
            }

            // Look for a tab with this name.
            TabControl details = this.DetailsTab;
            foreach (Wpf.Controls.TabItem item in details.Items)
            {
                string id = item.Tag as string;
                if (id == name)
                {
                    // Got it. Go there and set the position.
                    details.SelectedItem = item;
                    var existing = item.Content as SourceEditor;
                    existing.SetPosition(sl, sc, el, ec);
                    return;
                }
            }

            // A new tab needs to be created and the source code fetched.
            var tab = new Wpf.Controls.TabItem()
            {
                Tag = name,
                Header = new TextBlock() { Text = name },
                Icon = new Image
                {
                    Source = new BitmapImage(new Uri("images/Hourglass_16x.png", UriKind.Relative)),
                    Width = 16,
                    Height = 16,
                }
            };
        
            var sourcePromise = this.GetSourceAsync(name, language);

            SourceEditor editor = null;

            if (language == "X++")
                editor = new XppSourceEditor();
            else if (language == "Python")
                editor = new PythonSourceEditor();
            else if (language == "C#")
                editor = new CSharpSourceEditor();
            else 
                editor = new SourceEditor();

            tab.Content = editor;
            details.Items.Add(tab);
            details.SelectedItem = tab;

            var text = await sourcePromise;

            editor.Text = text ?? "No source found for " + name + " in " + language;

            await editor.Dispatcher.BeginInvoke(new Action(delegate { editor.SetPosition(sl, sc, el, ec); }), DispatcherPriority.ApplicationIdle);

            tab.Icon = null;

        }

        /// <summary>
        /// Fetch the attributes that designate position from the given node. If there are
        /// no positional attributes, the reference parameters are not changed.
        /// </summary>
        /// <param name="positionElement">The root node from which the position is extracted. </param>
        /// <param name="sl">returns the start line</param>
        /// <param name="sc">returns the start column</param>
        /// <param name="el">returns the end line</param>
        /// <param name="ec">returns the end column</param>
        private static bool FindPositionsInSelf(XElement positionElement, ref int sl, ref int sc, ref int el, ref int ec)
        {
            if (positionElement.Attribute("StartLine") != null)
            {
                bool ok = int.TryParse(positionElement.Attribute("StartLine").Value, out sl);

                if (positionElement.Attribute("StartCol") != null)
                    ok &= int.TryParse(positionElement.Attribute("StartCol").Value, out sc);

                if (positionElement.Attribute("EndLine") != null)
                    ok &= int.TryParse(positionElement.Attribute("EndLine").Value, out el);

                if (positionElement.Attribute("EndCol") != null)
                    ok &= int.TryParse(positionElement.Attribute("EndCol").Value, out ec);

                return ok;
            }

            return false;
        }

        private static void FindPositionsInSelfOrAncestor(XElement positionElement, ref int sl, ref int sc, ref int el, ref int ec)
        {
            while (positionElement != null && positionElement.HasAttributes)
            {
                if (positionElement.Attribute("StartLine") != null)
                {
                    FindPositionsInSelf(positionElement, ref sl, ref sc, ref el, ref ec);
                    break;
                }

                positionElement = positionElement.Parent;
            }
        }

        #region Font size handling





        #endregion



        private async void ModelDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            {
                var db = e.AddedItems[0] as BaseXInterface.Database;
                this.Model.Status = $"{db.Name}. Items: {db.Resources}, Size: {db.Size}";

                using var session = await this.Model.GetSessionAsync("");
                this.ResultsEditor.Text = await session.DoQueryAsync($"db:info('{db.Name}')");
            }
        }

        private void SelectDatabaseMenu_Click(object sender, RoutedEventArgs e)
        {
            MenuItem m = e.OriginalSource as MenuItem;
            var selectedDatabaseName = m.Header as string;

            var selectedDatabase = this.ViewModel.Databases.Where(d => d.Name == selectedDatabaseName).First();
            this.ViewModel.SelectedDatabase = selectedDatabase;
        }

        private void QueryTabPage_TabItemClosing(object sender, TabItemCancelEventArgs e)
        {
            e.Cancel = !this.ViewModel.CloseQueryTab(e.TabItem);
        }

        private void QueryGroupBox_Drop(object sender, DragEventArgs e)
        {
            // Called when a file is dropped on the query pane
            string[] droppedFiles = null;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                droppedFiles = e.Data.GetData(DataFormats.FileDrop, true) as string[];
            }

            if (droppedFiles == null)
                return;

            foreach (var filename in droppedFiles)
            {
                this.ViewModel.OpenFileInTab(filename);
            }
        }
    }
}
