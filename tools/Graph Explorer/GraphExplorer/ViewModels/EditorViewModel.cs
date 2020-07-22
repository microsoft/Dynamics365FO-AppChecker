// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using CefSharp;
using Microsoft.Win32;
using SocratexGraphExplorer.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace SocratexGraphExplorer.ViewModels
{
    public class EditorViewModel : INotifyPropertyChanged
    {
        private readonly MainWindow view;
        private readonly Models.Model model;
        private HashSet<long> shownNodes;

        public event PropertyChangedEventHandler PropertyChanged;
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
                this.shownNodes.Remove(this.model.SelectedNode);
                var nodeIdsString = this.CommaSeparatedString(this.shownNodes);

                var q = "match (n) where id(n) in [" + nodeIdsString + "] "
                      + "optional match (n) -[r]- (m) "
                      + "where id(n) in [" + nodeIdsString + "] " +
                        "  and id(m) in [" + nodeIdsString + "] " +
                        "return n,m,r";

                var response = await view.Browser.EvaluateScriptAsync("draw", q);
                this.model.SelectedNode = 0;
            },
            p =>
            {
                return (this.model.SelectedNode != 0) && this.shownNodes.Contains(this.model.SelectedNode);
            }
        );

        public ICommand ApplicationExitCommand => new RelayCommand(
            p => {
                System.Windows.Application.Current.Shutdown();
            }
        );

        public ObservableCollection<PropertyItem> NodeProperties
        {
            get
            {
                return this.model.NodeProperties;
            }
        }

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
                     
                     this.model.SelectedNode = 0;
                     this.model.SelectedEdge = 0;

                     if (this.model.ConnectResultNodes)
                     {
                         // First execute the query to get the result graph in memory:
                         var result = await this.model.ExecuteCypherAsync(source);
                         if (result != null)
                         {
                             // The query executed correctly. Now get the nodes so we can generate the
                             // query to show the nodes with all their connections.
                             this.shownNodes = this.model.HarvestNodeIdsFromGraph(result);
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
