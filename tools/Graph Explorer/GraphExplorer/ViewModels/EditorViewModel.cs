// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using CefSharp;
using Microsoft.Win32;
using Neo4j.Driver;
using SocratexGraphExplorer.Models;
using SocratexGraphExplorer.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        event NodeSelectedDelegate NodeSelected;
        event EdgeSelectedDelegate EdgeSelected;

        public event PropertyChangedEventHandler PropertyChanged;

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
                    this.OnPropertyChanged(nameof(SelectedNode));
                }
            }
        }

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
                    this.OnPropertyChanged(nameof(SelectedEdge));
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
                this.OnPropertyChanged(nameof(RenderingMode));
            }
        }

        private bool graphModeSelected = true;
        public bool GraphModeSelected
        {
            get { return this.graphModeSelected;  }
            set
            {
                this.graphModeSelected = value;

                if (value)
                {
                    this.view.GraphColumn.Width = new GridLength(2, GridUnitType.Star);
                    this.RenderingMode = RenderingMode.Graph;
                }
                else
                {
                    this.view.GraphColumn.Width = new GridLength(0);
                }

                this.OnPropertyChanged(nameof(GraphModeSelected));
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
                    this.RenderingMode = RenderingMode.Text;
                }
                else
                {
                    this.view.TextColumn.Width = new GridLength(0);
                }
                this.OnPropertyChanged(nameof(TextModeSelected));
            }
        }
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Title => Properties.Settings.Default.AppTitle + "  - " + this.model.ConnectionString;

        private readonly ICommand executeQueryCommand;
        public ICommand ExecuteQueryCommand => this.executeQueryCommand;

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

        public ICommand ApplicationExitCommand => new RelayCommand(
            p => {
                System.Windows.Application.Current.Shutdown();
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

        public ICommand ShowOutgoingEdges => new RelayCommand(
            async p =>
            {
                // This is additive to the existing graph
                // Find all the nodes from the current node:
                var q = "match (n) -[]-> (q) where id(n) = " + this.SelectedNode + " return q";
                var result = await this.model.ExecuteCypherAsync(q);

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
                var q = "match (n) <-[]- (q) where id(n) = " + this.SelectedNode + " return q";
                var result = await this.model.ExecuteCypherAsync(q);

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
                var q = "match (f) -[] -> (n) -[]-> (t) where id(n) = " + this.SelectedNode + " return t,f";
                var result = await this.model.ExecuteCypherAsync(q);

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
                this.OnPropertyChanged(nameof(ErrorMessage));
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

        public ICommand ShowDevToolsCommand
        {
            get => new RelayCommand(
                p => 
                {
                    this.view.Browser.ShowDevTools();
                }
            );
        }

        public ICommand SaveQueryCommand
        {
            get => new RelayCommand(
                p => {
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
            set { this.model.QueryFontSize = value;  } 
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
                this.OnPropertyChanged(nameof(CaretPositionString));
            }
        }

        public EditorViewModel(MainWindow v, Models.Model model)
        {
            this.view = v;
            this.model = model;

            NodeSelected += UpdateNodeInfoPage;

            EdgeSelected += UpdateEdgeInfoPage;

            this.PropertyChanged += async (object sender, PropertyChangedEventArgs e) =>
            {
                if (e.PropertyName == nameof(RenderingMode))
                {
                    if (this.RenderingMode == RenderingMode.Text)
                    {
                        // The user switched to text mode from fraph mode. The user may have made
                        // changes that are not reflected in the graph, but the current graph is
                        // represented in the Nodes structure.
                        if (this.model.NodesShown != null)
                        {
                            var nodeIdsString = Model.CommaSeparatedString(this.model.NodesShown);

                            var q = "match (n) where id(n) in [" + nodeIdsString + "] "
                                  + "optional match (n) -[r]- (m) "
                                  + "where id(n) in [" + nodeIdsString + "] " +
                                    "  and id(m) in [" + nodeIdsString + "] " +
                                    "return n,m,r";

                            var res = await this.model.ExecuteCypherAsync(q);
                            var html = Model.GenerateHtml(res);
                            this.view.TextBrowser.LoadLargeHtmlString(html);
                        }
                    }
                }
            };

            model.PropertyChanged += async (object sender, PropertyChangedEventArgs e) =>
            {
                if (e.PropertyName == "QueryFontSize")
                {
                    QueryEditorFontSize = this.model.QueryFontSize;
                    this.OnPropertyChanged(nameof(QueryEditorFontSize));
                }
                else if (e.PropertyName == "ErrorMessage")
                {
                    this.ErrorMessage = this.model.ErrorMessage;
                    this.OnPropertyChanged(nameof(ErrorMessage));
                }
                else if (e.PropertyName == "CaretPositionString")
                {
                    this.CaretPositionString = this.model.CaretPositionString;
                }
                else if (e.PropertyName == nameof(Model.NodesShown))
                {
                    // The nodes have been changed, so do a repaint
                    await this.RepaintNodesAsync(this.model.NodesShown);
                }
                else if (e.PropertyName == nameof(Model.QueryResults))
                {
                    if (this.GraphModeSelected)
                    {
                        this.model.NodesShown = Model.HarvestNodeIdsFromGraph(this.model.QueryResults);
                    }
                    else
                    {
                        var html = Model.GenerateHtml(this.model.QueryResults);
                        this.view.TextBrowser.LoadLargeHtmlString(html);
                    }
                }
            };

            this.executeQueryCommand = new RelayCommand(
                 async p =>
                 {
                     string source = v.CypherEditor.Document.Text;
                     
                     this.SelectedNode = 0;
                     this.SelectedEdge = 0;

                     if (this.model.ConnectResultNodes)
                     {
                         // First execute the query to get the result graph in memory:
                         var result = await this.model.ExecuteCypherAsync(source);
                         if (result != null)
                         {
                             // The query executed correctly. Now get the nodes so we can generate the
                             // query to show the nodes with all their connections.
                             //this.model.NodesShown = Model.HarvestNodeIdsFromGraph(result);
                             this.model.QueryResults = result;
                         }
                     }
                     else
                     {
                         // Send the query prepended with the EXPLAIN keyword to check the query.
                         var result = await this.model.ExecuteCypherAsync("explain " + source);
                         if (result != null)
                         {
                             // Send the query to the browser:
                             var response = await v.Browser.EvaluateScriptAsync("draw", v.CypherEditor.Document.Text);
                             if (!response.Success)
                             {
                                 this.model.ErrorMessage = response.Message;
                             }
                         }
                     }
                 },

                 // Running is allowed when there is text there to submit as a query and
                 // there is a connection to the database.
                 p =>
                 {
                     return true;
                 });

            this.printGraphCommand = new RelayCommand(
                p=>
                {
                    this.view.Browser.Print();
                }
            );
        }

        public async Task RepaintNodesAsync(HashSet<long> nodes)
        {
            if (nodes != null)
            {
                var nodeIdsString = Model.CommaSeparatedString(nodes);

                var q = "match (n) where id(n) in [" + nodeIdsString + "] "
                      + "optional match (n) -[r]- (m) "
                      + "where id(n) in [" + nodeIdsString + "] " +
                        "  and id(m) in [" + nodeIdsString + "] " +
                        "return n,m,r";

                await view.Browser.EvaluateScriptAsync("draw", q);
            }
        }

         private void UpdateNodeInfoPage(INode node)
        {
            // TODO this should not be hardcoded, but MEF should be used to find a plugin
            // that is able to handle (i.e. create a user control) for the node with the 
            // given label. What happens if there are more labels? Not defined at this time.
            UserControl child;

            if (string.Compare(node.Labels[0], "Method") == 0)
            {
                child = new SocratexGraphExplorer.Views.MethodInformationControl(this.model, this, node);
            }
            else if (string.Compare(node.Labels[0], "Class") == 0)
            {
                child = new SocratexGraphExplorer.Views.ClassInformationControl(this.model, this, node);
            }
            else if (string.Compare(node.Labels[0], "Table") == 0)
            {
                child = new SocratexGraphExplorer.Views.TableInformationControl(this.model, this, node);
            }
            else if (string.Compare(node.Labels[0], "Form") == 0)
            {
                child = new SocratexGraphExplorer.Views.FormInformationControl(this.model, this, node);
            }
            else
            {
                child = new SocratexGraphExplorer.Views.EmptyInformationControl();
            }
            this.view.ContextualInformation.Content = child;
        }

        private void UpdateEdgeInfoPage(IRelationship edge)
        {
            UserControl child = new SocratexGraphExplorer.Views.EmptyInformationControl();
            this.view.ContextualInformation.Content = child;
        }

        private void UpdateProperties(object nodeOrEdge)
        {
            var n = nodeOrEdge as INode;
            if (n != null)
            {
                if (this.NodeSelected != null)
                {
                    this.NodeSelected(n);
                }
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
        public void GeneratePropertyNodeListView(List<IRecord> records)
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

        private void SaveQueryFile()
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                DefaultExt = ".cypher",
                AddExtension = true,
                Filter = "Cypher files (*.cypher)|*.cypher|All files (*.*)|*.*",
            };

            bool? res = dialog.ShowDialog();

            if (res.HasValue && res.Value)
            {
                this.view.CypherEditor.Save(dialog.FileName);
            }
        }

        private void OpenQueryFile()
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                DefaultExt = ".cypher",
                Multiselect = false,
                Filter = "Cypher files (*.cypher)|*.cypher|All files (*.*)|*.*",
            };

            bool? res = dialog.ShowDialog();

            if (res.HasValue && res.Value)
            {
                var stream = dialog.OpenFile();
                this.view.CypherEditor.Load(stream);
            }
        }
    }
}
