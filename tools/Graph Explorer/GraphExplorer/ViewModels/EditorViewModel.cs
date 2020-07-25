// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using CefSharp;
using Microsoft.Win32;
using Neo4j.Driver;
using SocratexGraphExplorer.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SocratexGraphExplorer.ViewModels
{
    delegate void NodeSelectedDelegate(INode node);
    delegate void EdgeSelectedDelegate(IRelationship edge);

    public class EditorViewModel : INotifyPropertyChanged
    {
        private readonly MainWindow view;
        private readonly Models.Model model;
        private HashSet<long> shownNodes;
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



        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Title => Properties.Settings.Default.AppTitle + "  - " + this.model.ConnectionString;

        private readonly ICommand executeQueryCommand;
        public ICommand ExecuteQueryCommand => this.executeQueryCommand;

        public ICommand HideNodeCommand => new RelayCommand(
            async p =>
            {
                this.shownNodes.Remove(this.SelectedNode);
                var nodeIdsString = this.CommaSeparatedString(this.shownNodes);

                var q = "match (n) where id(n) in [" + nodeIdsString + "] "
                      + "optional match (n) -[r]- (m) "
                      + "where id(n) in [" + nodeIdsString + "] " +
                        "  and id(m) in [" + nodeIdsString + "] " +
                        "return n,m,r";

                var response = await view.Browser.EvaluateScriptAsync("draw", q);
                this.SelectedNode = 0;
            },
            p =>
            {
                return (this.SelectedNode != 0) && this.shownNodes.Contains(this.SelectedNode);
            }
        );

        public ICommand ApplicationExitCommand => new RelayCommand(
            p => {
                System.Windows.Application.Current.Shutdown();
            }
        );

        public ICommand ShowOnlyNodeCommand => new RelayCommand(
            async p =>
            {
                this.shownNodes.Clear();
                this.shownNodes.Add(this.SelectedNode);

                var q = "match (n) where id(n) = " + this.SelectedNode + " return n";

                var response = await view.Browser.EvaluateScriptAsync("draw", q);
                this.SelectedNode = 0;
            },
            p =>
            {
                return (this.SelectedNode != 0) && this.shownNodes.Contains(this.SelectedNode);
            }
        );

        public ICommand ShowOutgoingEdges => new RelayCommand(
            async p =>
            {
                // This is additive to the existing graph
                // Find all the nodes from the current node:
                var q = "match (n) -[]-> (q) where id(n) = " + this.SelectedNode + " return q";
                var result = await this.model.ExecuteCypherAsync(q);

                var outgoing = this.HarvestNodeIdsFromGraph(result);

                this.shownNodes.UnionWith (outgoing);
                var nodeIdsString = this.CommaSeparatedString(this.shownNodes);

                q = "match (n) where id(n) in [" + nodeIdsString + "] "
                    + "optional match (n) -[r]- (m) "
                    + "where id(n) in [" + nodeIdsString + "] " +
                    "  and id(m) in [" + nodeIdsString + "] " +
                    "return n,m,r";

                var response = await view.Browser.EvaluateScriptAsync("draw", q);
                // Keep the current node selected
            },
            p =>
            {
                return (this.SelectedNode != 0) && this.shownNodes.Contains(this.SelectedNode);
            }
        );

        public ICommand ShowIncomingEdges => new RelayCommand(
            async p =>
            {
                // This is additive to the existing graph
                // Find all the nodes from the current node:
                var q = "match (n) <-[]- (q) where id(n) = " + this.SelectedNode + " return q";
                var result = await this.model.ExecuteCypherAsync(q);

                var outgoing = this.HarvestNodeIdsFromGraph(result);

                this.shownNodes.UnionWith(outgoing);
                var nodeIdsString = this.CommaSeparatedString(this.shownNodes);

                q = "match (n) where id(n) in [" + nodeIdsString + "] "
                    + "optional match (n) -[r]- (m) "
                    + "where id(n) in [" + nodeIdsString + "] " +
                    "  and id(m) in [" + nodeIdsString + "] " +
                    "return n,m,r";

                var response = await view.Browser.EvaluateScriptAsync("draw", q);
                // Keep the current node selected
            },
            p =>
            {
                return (this.SelectedNode != 0) && this.shownNodes.Contains(this.SelectedNode);
            }
        );

        public ICommand ShowEdges => new RelayCommand(
            async p =>
            {
                // This is additive to the existing graph
                // Find all the nodes from the current node:
                var q = "match (f) -[] -> (n) -[]-> (t) where id(n) = " + this.SelectedNode + " return t,f";
                var result = await this.model.ExecuteCypherAsync(q);

                var outgoing = this.HarvestNodeIdsFromGraph(result);

                this.shownNodes.UnionWith(outgoing);
                var nodeIdsString = this.CommaSeparatedString(this.shownNodes);

                q = "match (n) where id(n) in [" + nodeIdsString + "] "
                    + "optional match (n) -[r]- (m) "
                    + "where id(n) in [" + nodeIdsString + "] " +
                    "  and id(m) in [" + nodeIdsString + "] " +
                    "return n,m,r";

                var response = await view.Browser.EvaluateScriptAsync("draw", q);
                // Keep the current node selected
            },
            p =>
            {
                return (this.SelectedNode != 0) && this.shownNodes.Contains(this.SelectedNode);
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

            model.PropertyChanged += (object sender, PropertyChangedEventArgs e) =>
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
                             this.shownNodes = this.HarvestNodeIdsFromGraph(result);
                             var nodeIdsString = CommaSeparatedString(this.shownNodes);
                             var q = "match (n) where id(n) in [" + nodeIdsString + "] "
                                   + "optional match (n) -[r]- (m) " 
                                   + "where id(n) in [" + nodeIdsString + "] " +
                                     "  and id(m) in [" + nodeIdsString + "] " +
                                     "return n,m,r";
                             var response = await v.Browser.EvaluateScriptAsync("draw", q);
                             if (!response.Success)
                             {
                                 this.model.ErrorMessage = response.Message;
                             }
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

        private string CommaSeparatedString(HashSet<long> set)
        {
            string result = string.Empty;
            bool first = true;
            foreach (var val in set)
            {
                if (!first)
                    result += ",";
                result += val.ToString();
                first = false;
            }
            return result;
        }

        public HashSet<long> HarvestNodeIdsFromGraph(List<IRecord> records)
        {
            if (!records.Any())
                return null;

            var result = new HashSet<long>();
            App.Current.Dispatcher.Invoke(() =>
            {
                foreach (var record in records)
                {
                    foreach (var v in record.Values)
                    {
                        if (v.Value is IPath)
                        {
                            // There are two nodes connected by an edge:
                            var path = v.Value as IPath;
                            // The start and end can be the same, for self referenctial nodes.
                            result.Add(path.Start.Id);
                            result.Add(path.End.Id);
                        }
                        else if (v.Value is INode)
                        {
                            var n = v.Value as INode;
                            result.Add(n.Id);
                        }
                    }
                }
            });
            return result;
        }

        private void UpdateNodeInfoPage(INode node)
        {
            // TODO this should not be hardcoded, but MEF should be used to find a plugin
            // that is able to handle (i.e. create a user control) for the node with the 
            // given label. What happens if there are more labels? Not defined at this time.
            UserControl child;

            if (string.Compare(node.Labels[0], "Method") == 0)
            {
                child = new SocratexGraphExplorer.Views.MethodInformationControl(this.model, node);
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
