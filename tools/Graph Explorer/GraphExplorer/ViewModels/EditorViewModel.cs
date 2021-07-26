// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using mde = MaterialDesignExtensions.Controls;
using MaterialDesignThemes.Wpf;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using Microsoft.Win32;
using Neo4j.Driver;
using SocratexGraphExplorer.Common;
using SocratexGraphExplorer.Models;
using SocratexGraphExplorer.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MaterialDesignExtensions.Controls;
using MaterialDesignExtensions.Converters;
using MaterialDesignExtensions.Model;

namespace SocratexGraphExplorer.ViewModels
{
    delegate void NodeSelectedDelegate(INode node);
    delegate void EdgeSelectedDelegate(IRelationship edge);

    public enum RenderingMode { Graph, Text };

    public class EditorViewModel : INotifyPropertyChanged
    {
        private readonly MainWindow view;
        private readonly Models.Model model;
        private long selectedNode = 0;
        private long selectedEdge = 0;

        /// <summary>
        /// The list of plugins to handle rendering of node information. This list is
        /// populated by MEF.
        /// </summary>
        [ImportMany(typeof(INodeRendererPlugin))]
        public IEnumerable<Lazy<INodeRendererPlugin, IPluginMetadata>> NodePlugins { get; set; }

        /// <summary>
        /// The list of plugins to handle rendering of edge information. This list is
        /// populated by MEF.
        /// </summary>
        [ImportMany(typeof(IEdgeRendererPlugin))]
        public IEnumerable<Lazy<IEdgeRendererPlugin, IPluginMetadata>> EdgePlugins { get; set; }

        /// <summary>
        /// This dictionary maps the names of labels onto the renderers that 
        /// present the node information.
        /// </summary>
        private IDictionary<string, INodeRenderer> NodeRenderers { get; set; }

        /// <summary>
        /// This dictionary maps the names of labels onto the renderers that 
        /// present the edge (i.e. relationship) information.
        /// </summary>
        private IDictionary<string, IEdgeRenderer> EdgeRenderers { get; set; }

        private ConfigEditWindow configEditor;

        event NodeSelectedDelegate NodeSelected;
        event EdgeSelectedDelegate EdgeSelected;

        public event PropertyChangedEventHandler PropertyChanged;

        private IEnumerable<DatabaseDescriptor> databases;

        /// <summary>
        /// Gets or sets the databases in the neo4j instance.
        /// </summary>
        public IEnumerable<DatabaseDescriptor> Databases {
            get { return this.databases; }
            set
            {
                if (value != this.databases)
                {
                    this.databases = value;
                    this.OnPropertyChanged(nameof(this.Databases));
                }
            }
        }

        /// <summary>
        /// The event is triggered when a node is selected on the browser surface
        /// </summary>
        public long SelectedNode
        {
            get
            {
                return this.selectedNode;
            }
            set
            {
                if (this.selectedNode != value)
                {
                    this.selectedNode = value;
                    this.OnPropertyChanged(nameof(this.SelectedNode));
                }
            }
        }

        /// <summary>
        /// The event is triggered when an edge is selectged on the drawing surface.
        /// </summary>
        public long SelectedEdge
        {
            get
            {
                return this.selectedEdge;
            }
            set
            {
                if (this.selectedEdge != value)
                {
                    this.selectedEdge = value;
                    this.OnPropertyChanged(nameof(this.SelectedEdge));
                }
            }
        }

        private RenderingMode renderingMode;
        public RenderingMode RenderingMode
        {
            get { return this.renderingMode; }
            set
            {
                this.renderingMode = value;
                this.OnPropertyChanged(nameof(this.RenderingMode));
            }
        }

        private bool graphModeSelected = true;
        public bool GraphModeSelected
        {
            get { return this.graphModeSelected; }
            set
            {
                this.graphModeSelected = value;

                if (value)
                {
                    // Show the graph rendering surface
                    this.view.GraphColumn.Width = new GridLength(2, GridUnitType.Star);
                    this.view.TextColumn.Width = new GridLength(0);

                    this.RenderingMode = RenderingMode.Graph;
                }
                else
                {
                    // Show the text rendering surface
                    this.view.GraphColumn.Width = new GridLength(0);
                    this.RenderingMode = RenderingMode.Text;
                }

                this.OnPropertyChanged(nameof(this.GraphModeSelected));
            }
        }

        private bool textModeSelected = false;
        public bool TextModeSelected
        {
            get { return this.textModeSelected; }
            set
            {
                this.textModeSelected = value;

                if (value)
                {
                    this.view.TextColumn.Width = new GridLength(2, GridUnitType.Star);
                    this.view.GraphColumn.Width = new GridLength(0);
                    this.RenderingMode = RenderingMode.Text;
                }
                else
                {
                    this.view.TextColumn.Width = new GridLength(0);
                }
                this.OnPropertyChanged(nameof(this.TextModeSelected));
            }
        }
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private DatabaseDescriptor database = null;
        /// <summary>
        /// This is the currently selected database
        /// </summary>
        public DatabaseDescriptor Database
        {
            get { return this.database; }
            set
            {
                if (value != this.database)
                {
                    this.database = value;
                    Neo4jDatabase.Database = value;
                    this.OnPropertyChanged(nameof(this.Database));
                    this.OnPropertyChanged(nameof(this.Title));
                }
            }
        }

        public string Title {
            get {
                var s = Properties.Settings.Default.AppTitle + " - ";
                s += this.model.ConnectionString;
                if (this.Database != null)
                {
                    s += " : " + this.Database.Name;
                }
                return s;
            }
        }

        private readonly ICommand executeQueryCommand;
        public ICommand ExecuteQueryCommand => this.executeQueryCommand;

        public ICommand AboutCommand => new RelayCommand(
            p =>
            {
                var aboutBox = new AboutBox();
                aboutBox.Show();
            }
        );

        public ICommand ApplicationExitCommand => new RelayCommand(
            p =>
            {
                Application.Current.Shutdown();
            }
        );

        public ICommand ShowOnlyNodeCommand => new RelayCommand(
            p =>
            {
                var nodes = new HashSet<long>() { this.SelectedNode };
                this.model.NodesShown = nodes;
                this.SelectedNode = 0;
            },
            p =>
            {
                return (this.SelectedNode != 0) && this.model.NodesShown.Contains(this.SelectedNode);
            }
        );

        public ICommand HideNodeCommand => new RelayCommand(
            p =>
            {
                var nodes = this.model.NodesShown;
                nodes.Remove(this.SelectedNode);
                this.model.NodesShown = nodes;

                this.SelectedNode = 0;
            },
            p =>
            {
                return (this.SelectedNode != 0) && this.model.NodesShown.Contains(this.SelectedNode);
            }
        );

        public ICommand ShowOutgoingEdges => new RelayCommand(
            async p =>
            {
                // This is additive to the existing graph
                // Find all the nodes from the current node:
                var query = "match (n) -[]-> (q) where id(n) = $nodeId return q";
                var result = await this.model.ExecuteCypherAsync(query, new Dictionary<string, object>() { { "nodeId", this.SelectedNode } });

                var outgoing = Model.HarvestNodeIdsFromGraph(result);

                if (outgoing != null && outgoing.Any())
                {
                    var nodes = this.model.NodesShown;
                    nodes.UnionWith(outgoing);
                    this.model.NodesShown = nodes;
                }
            },
            p =>
            {
                return (this.SelectedNode != 0) && this.model.NodesShown.Contains(this.SelectedNode);
            }
        );

        public ICommand ShowIncomingEdges => new RelayCommand(
            async p =>
            {
                // This is additive to the existing graph
                // Find all the nodes from the current node:

                var query = "match (n) <-[]- (q) where id(n) = $nodeId return q";
                var result = await this.model.ExecuteCypherAsync(query, new Dictionary<string, object>() { { "nodeId", this.SelectedNode } });

                var incoming = Model.HarvestNodeIdsFromGraph(result);

                if (incoming != null && incoming.Any())
                {
                    var nodes = this.model.NodesShown;
                    nodes.UnionWith(incoming);
                    this.model.NodesShown = nodes;
                }
            },
            p =>
            {
                return (this.SelectedNode != 0) && this.model.NodesShown.Contains(this.SelectedNode);
            }
        );

        public ICommand ShowEdges => new RelayCommand(
            async p =>
            {
                // This is additive to the existing graph
                // Find all the edges (both incoming and outgoing) from the current node:
                var q = "match (f) -[]- (n) where id(n) = $nodeId return f";
                var result = await this.model.ExecuteCypherAsync(q, new Dictionary<string, object>() { { "nodeId", this.SelectedNode } });

                var outgoing = Model.HarvestNodeIdsFromGraph(result);

                if (outgoing != null && outgoing.Any())
                {
                    var nodes = this.model.NodesShown;
                    nodes.UnionWith(outgoing);
                    this.model.NodesShown = nodes;
                }
            },
            p =>
            {
                return (this.SelectedNode != 0) && this.model.NodesShown.Contains(this.SelectedNode);
            }
        );


        private readonly ICommand printGraphCommand;
        public ICommand PrintGraphCommand => this.printGraphCommand;

        public string ErrorMessage
        {
            get { return this.model.ErrorMessage; }
            set
            {
                this.OnPropertyChanged(nameof(this.ErrorMessage));
            }
        }

        public ICommand IncreaseQueryFontSize
        {
            get => new RelayCommand(
                p =>
                {
                    this.model.QueryFontSize += 2;
                },
                p =>
                {
                    return this.model.QueryFontSize < 48;
                }
            );
        }

        public ICommand DecreaseQueryFontSize
        {
            get => new RelayCommand(
                p =>
                {
                    this.model.QueryFontSize -= 2;
                },
                p =>
                {
                    return this.model.QueryFontSize > 8;
                }
            );
        }

        public ICommand ShowCypherRefcard
        {
            get => new RelayCommand(
                p =>
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = "https://neo4j.com/docs/cypher-refcard/current/",
                        UseShellExecute = true
                    };
                    Process.Start(psi);
                });
        }
        public ICommand ShowSettingsCommand
        {
            get => new RelayCommand(
                p =>
                {
                    this.configEditor = new ConfigEditWindow(this.model, this)
                    {
                        DataContext = this
                    };
                    this.configEditor.ShowDialog();
                    this.configEditor.Owner = this.view;
                }
            );
        }

        public ICommand ShowDatabaseParametersCommand
        {
            get => new RelayCommand(
                async p =>
                {
                    this.ShowDatabaseInfoPanel();
                }
            );
        }

        public ICommand ImportStyleCommand
        {
            get => new RelayCommand(
                async p =>
                {
                    // TODO experimental
                    DataLab d = await DataLab.CreateDataLaboratory(); 
                    d.Show();

                    // TODO Put in the material dialog here.
                    // Open a Javascript (.js) file with the file open dialog
                    //var openFileDialog = new OpenFileDialog()
                    //{
                    //    CheckFileExists = true,
                    //    AddExtension = true,
                    //    DefaultExt = ".js",
                    //    Filter = "Javascript|*.js",
                    //    Multiselect = false,
                    //    Title = "Select style document to import",
                    //};

                    //if (openFileDialog.ShowDialog() == true)
                    //{
                    //    var styleScriptSource = System.IO.File.ReadAllText(openFileDialog.FileName);
                    //    this.model.StyleDocumentSource = styleScriptSource;
                    //}
                });
        }

        public ICommand ExportStyleCommand
        {
            get => new RelayCommand(
                async p =>
                {
                    // Temporary: Use this as a launchpoint for the rendering options dialog
                    var d = new RenderingOptions();
                    d.Show();

                    var labels = await Neo4jDatabase.GetNodeLabels();

                    // Export the bare-bones style Javascript style document.
                    //var saveFileDialog = new SaveFileDialog()
                    //{
                    //    AddExtension = true,
                    //    CheckFileExists = true,
                    //    CheckPathExists = true,
                    //    DefaultExt = "js",
                    //    Filter = "Javascript|*.js",
                    //    OverwritePrompt = true,
                    //    Title = "Save generic style document",
                    //};

                    //if (saveFileDialog.ShowDialog() == true)
                    //{
                    //    var assembly = Assembly.GetExecutingAssembly();
                    //    var s = assembly.GetManifestResourceStream("SocratexGraphExplorer.Resources.GenericStyleDocument.js");

                    //    using var reader = new StreamReader(s);
                    //    File.WriteAllText(saveFileDialog.FileName, reader.ReadToEnd());
                    //}
                });
        }

        public ICommand SaveQueryCommand
        {
            get => new RelayCommand(
                p =>
                {
                    this.SaveQueryFile();
                });
        }

        public ICommand OpenQueryCommand
        {
            get => new RelayCommand(
                p =>
                {
                    this.OpenQueryFile();
                });
        }

        public int QueryEditorFontSize
        {
            get => this.model.QueryFontSize;
            set { this.model.QueryFontSize = value; }
        }

        public int QueryFont => model.QueryFontSize;

        public string CaretPositionString
        {
            get
            {
                return this.model.CaretPositionString;
            }
            set
            {
                this.OnPropertyChanged(nameof(this.CaretPositionString));
            }
        }

        public string StyleDocumentSource
        {
            get { return this.model.StyleDocumentSource; }
            set
            {
                if (this.model.StyleDocumentSource != value)
                {
                    this.model.StyleDocumentSource = value;
                    this.OnPropertyChanged(nameof(this.StyleDocumentSource));
                }
            }
        }

        public bool ShowLineNumbers
        {
            get { return this.model.ShowLineNumbers; }
            set
            {
                if (value != this.model.ShowLineNumbers)
                {
                    this.model.ShowLineNumbers = value;
                    this.OnPropertyChanged(nameof(this.ShowLineNumbers));
                }
            }
        }


        public bool IsDarkMode
        {
            get { return this.model.IsDarkMode; }
            set
            {
                if (value != this.model.IsDarkMode)
                {
                    this.model.IsDarkMode = value;
                    this.OnPropertyChanged(nameof(this.IsDarkMode));
                }
            }
        }

        public bool ShowNavigationButtons
        {
            get
            {
                return this.model.ShowNavigationButtons;
            }
            set
            {
                this.model.ShowNavigationButtons = value;
                this.OnPropertyChanged(nameof(this.ShowNavigationButtons));
            }
        }

        public bool AllowKeyboardNavigation
        {
            get
            {
                return Properties.Settings.Default.AllowKeyboardNavigation;
            }
            set
            {
                Properties.Settings.Default.AllowKeyboardNavigation = value;
                this.OnPropertyChanged(nameof(this.AllowKeyboardNavigation));
            }
        }

        public async Task SetGraphSizeAsync(WebView2 browser)
        {
            var snippet = "setGraphSize(" + (browser.RenderSize.Width - 16).ToString() + "," + (browser.RenderSize.Height - 16).ToString() + ");";
            await browser.ExecuteScriptAsync(snippet);
        }

        private void SetTheme(bool darkMode)
        {
            var paletteHelper = new PaletteHelper();

            ITheme theme = paletteHelper.GetTheme();
            IBaseTheme baseTheme = darkMode ? (IBaseTheme)new MaterialDesignDarkTheme() : (IBaseTheme)new MaterialDesignLightTheme();
            theme.SetBaseTheme(baseTheme);
            paletteHelper.SetTheme(theme);
        }

        public async Task OnInitializedAsync()
        {
            this.Databases = await Neo4jDatabase.GetDatabases();
            if (this.Databases != null)
            {
                var d = this.Databases.Where(d => d.IsDefault).FirstOrDefault();
                if (d != null)
                {
                    this.Database = d;
                }
            }
        }

        public EditorViewModel(MainWindow v, Models.Model model)
        {
            this.view = v;
            this.model = model;

            // Initialize the mapping from names onto renderers.
            this.NodeRenderers = new Dictionary<string, INodeRenderer>();
            this.EdgeRenderers = new Dictionary<string, IEdgeRenderer>();

            // Load the list of renderers through MEF.
            var catalog = new AggregateCatalog();

            // Put in the directory where the table extractor lives into the catalog
            // Use the directory where the main executable is found
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            catalog.Catalogs.Add(new DirectoryCatalog(assemblyPath));

            CompositionContainer container = new CompositionContainer(catalog);

            try
            {
                // Load all the plugins in the catalog, i.e. in the directory provided.
                container.SatisfyImportsOnce(this);

                // at this point the extractor should have been filled in. The metadata is
                // also available.
                foreach (var plugin in this.NodePlugins)
                {
                    this.NodeRenderers[plugin.Metadata.Label] = plugin.Value.CreateRenderer(model);
                }

                foreach (var plugin in this.EdgePlugins)
                {
                    this.EdgeRenderers[plugin.Metadata.Label] = plugin.Value.CreateRenderer(model);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            NodeSelected += this.UpdateNodeInfoPage;
            EdgeSelected += this.UpdateEdgeInfoPage;

            this.SetTheme(this.model.IsDarkMode);

            // Handler for events in this view model
            this.PropertyChanged += (object sender, PropertyChangedEventArgs e) =>
            {
            };

            // Handler for events bubbling up from the model.
            model.PropertyChanged += async (object sender, PropertyChangedEventArgs e) =>
            {
                if (e.PropertyName == nameof(Model.QueryFontSize))
                {
                    this.QueryEditorFontSize = this.model.QueryFontSize;
                    this.OnPropertyChanged(nameof(this.QueryEditorFontSize));
                }
                else if (e.PropertyName == nameof(Model.ErrorMessage))
                {
                    this.ErrorMessage = this.model.ErrorMessage;
                    this.OnPropertyChanged(nameof(this.ErrorMessage));
                }
                else if (e.PropertyName == nameof(Model.CaretPositionString))
                {
                    this.CaretPositionString = this.model.CaretPositionString;
                }
                else if (e.PropertyName == nameof(Model.EditorPosition))
                {
                    var p = this.model.EditorPosition;

                    this.view.CypherEditor.TextArea.Caret.Position = new ICSharpCode.AvalonEdit.TextViewPosition(p.Item1, p.Item2);
                    this.view.CypherEditor.TextArea.Caret.BringCaretToView();
                }
                else if (e.PropertyName == nameof(Model.IsDarkMode))
                {
                    this.SetTheme(this.model.IsDarkMode);

                    // Update the view
                    await this.RepaintNodesAsync(this.model.NodesShown);
                }
                else if (e.PropertyName == nameof(this.AllowKeyboardNavigation) || e.PropertyName == nameof(this.ShowNavigationButtons))
                {
                    await this.RepaintNodesAsync(this.model.NodesShown);
                }
                else if (e.PropertyName == nameof(Model.StyleDocumentSource))
                {
                    // Store the file along with the script
                    var uri = this.model.ScriptUri;
                    var fileName = uri.LocalPath;

                    var directory = Path.GetDirectoryName(fileName);
                    var configFileName = Path.Combine(directory, "Config.js");
                    File.WriteAllText(configFileName, model.StyleDocumentSource);

                    // The config file was changed, but the browser will keep a cached copy. To avoid
                    // this, the script is updated to reflect a new file name.
                    var script = File.ReadAllText(fileName);

                    // The script contains a reference to the config file:
                    //     <script type="text/javascript" src="Config.js?ts=1234567890"></script>
                    // The digit sequence needs to be changed so that the browser does not
                    // use a cached version of the config file.
                    var idx = script.IndexOf("Config.js?ts=") + "Config.js?ts=".Length;

                    var cnt = 0;
                    while (char.IsDigit(script[idx + cnt]))
                        cnt++;

                    script = script.Remove(idx, cnt);
                    script = script.Insert(idx, DateTime.Now.Ticks.ToString());

                    File.WriteAllText(fileName, script);

                    this.view.Browser.NavigationCompleted += this.Browser_NavigationCompleted;

                    this.view.Browser.Reload();
                }
                else if (e.PropertyName == nameof(Model.NodesShown))
                {
                    // The nodes have been changed, so do a repaint
                    await this.RepaintNodesAsync(this.model.NodesShown);
                }
                else if (e.PropertyName == nameof(Model.QueryResults))
                {
                    // Update the set of nodes shown on the surface.
                    this.model.NodesShown = Model.HarvestNodeIdsFromGraph(this.model.QueryResults);


                }
            };

            this.executeQueryCommand = new RelayCommand(
                 async p =>
                 {
                     string source = this.GetSource();

                     this.SelectedNode = 0;
                     this.SelectedEdge = 0;

                     // First execute the query to get the result graph in memory:
                     List<IRecord> result = await this.model.ExecuteCypherAsync(source);
                     if (result != null)
                     {
                         var canBeRenderedAsGraph = Model.CanBeRenderedAsGraph(result);

                         // The query executed correctly. 
                         // If the user wanted to see all the edges from the selected nodes, a new
                         // query is fired.
                         if (this.model.ConnectResultNodes && canBeRenderedAsGraph)
                         {
                             // Calculate the new result where all the edges are included.
                             var nodes = Model.HarvestNodeIdsFromGraph(result);
                             result = await this.GetGraphFromNodes(nodes);
                         }

                         // Store the results in the model.
                         this.model.QueryResults = result;

                         // Go from graph mode to text mode if the result cannot be rendered as a graph.
                         // Do not switch back to graph mode automatically?

                         if (this.GraphModeSelected && !canBeRenderedAsGraph)
                         {
                             this.GraphModeSelected = false;
                             this.TextModeSelected = true;
                         }

                         if (this.graphModeSelected)
                         {
                             // Render the data in the graph view
                             string resultJson = Neo4jDatabase.GenerateJSON(result);

                             await this.RepaintNodesAsync(resultJson);
                         }
                         else
                         {
                             // Draw as html
                             var html = Neo4jDatabase.GenerateHtml(this.model.QueryResults);
                             this.view.TextBrowser.NavigateToString(html);
                         }
                     }
                 },

                 // Running is allowed when there is text there to submit as a query and
                 // there is a connection to the database.
                 p =>
                 {
                     return v.CypherEditor.Document.Text.Any();
                 });

            this.printGraphCommand = new RelayCommand(
                async p =>
                {
                    await this.view.Browser.ExecuteScriptAsync("this.print();");
                }
            );
        }

        private async void Browser_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            var browser = sender as WebView2;
            // Redraw what was there with the new style.
            await this.SetGraphSizeAsync(browser);
            await this.RepaintNodesAsync(this.model.NodesShown);
            browser.NavigationCompleted -= this.Browser_NavigationCompleted;
        }

        /// <summary>
        /// Get the cypher source to execute from the cypher editor. If no selection is 
        /// active in the editor, the whoile buffer is executed. If one or more lines are 
        /// selected, the selected lines are returned.
        /// </summary>
        /// <returns>The buffer content or the selected lines, as described.</returns>
        private string GetSource()
        {
            var editor = this.view.CypherEditor;
            if (editor.SelectionLength > 0)
            {
                return editor.SelectedText;
            }

            return editor.Document.Text;
        }

        /// <summary>
        /// Get the graph that contains the nodes provided, and also all the edges
        /// between the nodes.
        /// </summary>
        /// <param name="nodes">The set of nodes in the graph</param>
        /// <returns>The set of records to render, including all the edges connecting the 
        /// nodes provided.</returns>
        private async Task<List<IRecord>> GetGraphFromNodes(HashSet<long> nodes)
        {
            if (nodes != null)
            {
                var query = "match (n) where id(n) in $nodeIds "
                      + "optional match (n) -[r]- (m) "
                      + "where id(n) in $nodeIds " +
                        "  and id(m) in $nodeIds " +
                        "return n,m,r";

                var results = await Neo4jDatabase.ExecuteQueryAsync(query, new Dictionary<string, object>() { { "nodeIds", nodes.ToArray() } });
                return await results.ToListAsync();
            }

            return null;
        }

        private static string JavascriptColorString(Color color)
        {
            return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }

        public async Task RepaintNodesAsync(string json)
        {
            await view.Browser.EnsureCoreWebView2Async();

            var backgroundColorBrush = this.view.FindResource("MaterialDesignPaper") as SolidColorBrush;
            var foregroundColorBrush = this.view.FindResource("MaterialDesignBody") as SolidColorBrush;

            var backgroundColor = JavascriptColorString(backgroundColorBrush.Color);
            var foregroundColor = JavascriptColorString(foregroundColorBrush.Color);

            var showNavigationButtons = this.ShowNavigationButtons;
            var allowKeyboardNavigation = this.AllowKeyboardNavigation;

            var drawInvocation = string.Format("draw({0}, '{1}', '{2}', {3}, {4});", json, backgroundColor, foregroundColor, showNavigationButtons.ToString().ToLowerInvariant(), allowKeyboardNavigation.ToString().ToLowerInvariant());
            await view.Browser.ExecuteScriptAsync(drawInvocation);
        }

        public async Task RepaintNodesAsync(HashSet<long> nodes)
        {
            // TODO this does not look correct. It seems to always get the nodes connected by edges that are not included in the query
            var results = await this.GetGraphFromNodes(nodes);
            if (results != null)
            {
                string resultJson = Neo4jDatabase.GenerateJSON(results);
                await RepaintNodesAsync(resultJson);
            }
        }

        private void UpdateNodeInfoPage(INode node)
        {
            if (!this.NodeRenderers.TryGetValue(node.Labels[0], out INodeRenderer renderer))
            {
                // Use the default one, by convention called ()
                renderer = this.NodeRenderers["()"];
            }

            if (this.view.ContextualInformation.Content != renderer)
            {
                this.view.ContextualInformation.Content = renderer;
            }

            renderer.SelectNodeAsync(node);
        }

        private void UpdateEdgeInfoPage(IRelationship edge)
        {
            if (!this.EdgeRenderers.TryGetValue(edge.Type, out IEdgeRenderer renderer))
            {
                // Use the default one, by convention called ()
                renderer = this.EdgeRenderers["()"];
            }

            if (this.view.ContextualInformation.Content != renderer)
            {
                this.view.ContextualInformation.Content = renderer;
            }

            renderer.SelectEdgeAsync(edge);
        }

        private void ShowDatabaseInfoPanel()
        {
            UserControl child = new DatabaseInformationControl(this, this.model);
            this.view.ContextualInformation.Content = child;
        }

        private void UpdateProperties(object nodeOrEdge)
        {
            if (nodeOrEdge is INode n)
            {
                this.NodeSelected?.Invoke(n);
            }
            else
            {
                if (this.EdgeSelected != null)
                {
                    IRelationship r = nodeOrEdge as IRelationship;
                    this.EdgeSelected(r);
                }
            }
        }

        /// <summary>
        /// Populate the list of ListView items containing node or edge properties.
        /// </summary>
        /// <param name="records"></param>
        public void UpdatePropertyListView(List<IRecord> records)
        {
            if (!records.Any())
                return;

            App.Current.Dispatcher.Invoke(() =>
            {
                var v = records[0];
                KeyValuePair<string, object> f = v.Values.FirstOrDefault();

                // Call the delegate so every one interested can react
                this.UpdateProperties(f.Value);
            });
        }

        private async void SaveQueryFile()
        {
            var dialogArgs = new mde.SaveFileDialogArguments()
            {
                Width = 800,
                Height = 1600,
                Filters = "Cypher files|*.cypher|All files|*.*",
                FilterIndex = 0,
                ShowHiddenFilesAndDirectories = false,
                ShowSystemFilesAndDirectories = false,
                CreateNewDirectoryEnabled = true,
                ForceFileExtensionOfFileFilter = true,
                PathPartsAsButtons = true,
                CurrentDirectory = Properties.Settings.Default.SourceFileDirectory,
            };

            var result = await mde.SaveFileDialog.ShowDialogAsync("RootDialog", dialogArgs);

            if (result.Confirmed)
            {
                this.view.CypherEditor.Save(result.File);
            }
        }

        /// <summary>
        /// Open a file open dialog to read a cypher file.
        /// </summary>
        private async void OpenQueryFile()
        {
            // open file
            var dialogArgs = new mde.OpenFileDialogArguments()
            {
                Width = 800,
                Height = 1600,
                Filters = "Cypher files|*.cypher|All files|*.*",
                FilterIndex = 0,
                ShowHiddenFilesAndDirectories = false,
                CurrentDirectory = Properties.Settings.Default.SourceFileDirectory,
            };

            var result = await mde.OpenFileDialog.ShowDialogAsync("RootDialog", dialogArgs);

            if (result.Confirmed)
            {
                var file = result.FileInfo;
                using (var stream = file.OpenRead())
                {
                    this.view.CypherEditor.Load(stream);
                }
            }
        }

        public void EnterSourceInCypherEditor(string source)
        {
            this.view.CypherEditor.Text += "\n\n" + source;
        }

        /// <summary>
        /// Called when the application closes down. Do any cleanup here.
        /// </summary>
        public void Close()
        {
            this.model.Close();
        }
    }
}
