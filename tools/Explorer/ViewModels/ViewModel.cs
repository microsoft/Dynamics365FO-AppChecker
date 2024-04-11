// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
using BaseXInterface;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using System.Xml.XPath;
using XppReasoningWpf.OpenAI;
using static Azure.Core.HttpHeader;

namespace XppReasoningWpf.ViewModels
{
    public class ViewModel : INotifyPropertyChanged
    {
        private readonly Model model;
        private readonly MainWindow view;

        /// <summary>
        /// Contains the mapping from a query editor to the result that this query produced.
        /// </summary>
        private readonly IDictionary<QueryEditor, string> CachedQueryResult = new Dictionary<QueryEditor, string>();

        public event PropertyChangedEventHandler PropertyChanged;

        private PromptEvaluator AIPromptEvaluator = null;

        #region Commands
        public ICommand ExitApplicationCommand { get; private set; }

        public ICommand KeyboardExecuteQueryCommand { get; private set; }

        public ICommand KeyboardCheckQueryCommand { get; private set; }

        public ICommand ExecuteQueryCommand { get; private set; }

        public ICommand ExecuteAICommand { get; private set; }

        public ICommand CheckQueryCommand { get; private set; }

        public ICommand SubmitQueryCommand { get; private set; }

        public ICommand WindowsCommand { get; private set; }

        public ICommand CloseAllWindowsCommand { get; private set; }

        public ICommand ClearLogCommand{ get; private set; }

        public ICommand SaveCommand { get; private set; }

        public ICommand SaveAsCommand { get; private set; }

        public ICommand OpenQueryCommand { get; private set; }

        public ICommand CreateNewQueryCommand { get; private set; }

        public ICommand OpenQueuedQueriesWindow { get; private set; }

        public ICommand AboutBoxCommand { get; private set; }

        public ICommand XQueryHelpCommand { get; private set; }

        public ICommand ShowExternalVariablesDialogCommand { get; private set; }
        
        public ICommand BaseXHelpCommand { get; private set; }

        public ICommand SaveResultsCommand { get; private set; }

        public ICommand IncreaseResultsFontSizeCommand { get; private set; }

        public ICommand DecreaseResultsFontSizeCommand { get; private set; }

        public ICommand IncreaseAIFontSizeCommand { get; private set; }

        public ICommand DecreaseAIFontSizeCommand { get; private set; }

        public ICommand IncreaseLogFontSizeCommand { get; private set; }

        public ICommand DecreaseLogFontSizeCommand { get; private set; }

        public ICommand IncreaseQueryFontSizeCommand { get; private set; }

        public ICommand DecreaseQueryFontSizeCommand { get; private set; }

        public ICommand ResultsUndoCommand { get; private set; }
 
        public ICommand ResultsRedoCommand { get; private set; }

        public ICommand QueryUndoCommand { get; private set; }

        public ICommand QueryRedoCommand { get; private set; }
        #endregion

        private Views.SubmittedQueriesWindow queuedQueriesWindow = null;
        private Views.SubmittedQueriesWindow QueuedQueriesWindow
        {
            get
            {
                if (this.queuedQueriesWindow == null)
                {
                    this.queuedQueriesWindow = new Views.SubmittedQueriesWindow(this.model);
                    this.queuedQueriesWindow.Closed += (o, s) =>
                    {
                        this.queuedQueriesWindow = null;
                    };
                }
                return this.queuedQueriesWindow;
            }
        }

        private string log = string.Empty;
        public string Log
        {
            get => this.log;
            set
            {
                this.log = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Log)));
            }
        }

        public string QueryResult
        {
            get { return this.model.QueryResult; }
            set
            {
                // The query result streams through here. It is not stored in the view model,
                // only in the model.
                // The error information from BaseX is not reliable enough to form the basis
                // of an error squiggley.
                //string s = value;
                //var r = Regex.Match(s, @"Stopped at \.,\s(\d+)/(\d+):");
                //if (r.Success)
                //{
                //    Group lineGroup = r.Groups[1];
                //    Group columnGroup = r.Groups[2];
                //    int line = int.Parse(lineGroup.Value);
                //    int column = int.Parse(columnGroup.Value);

                //    QueryEditor queryEditor = this.view.queryTabPage.SelectedContent as QueryEditor;

                //    var startOffset = queryEditor.Document.GetOffset(line, column);
                //    ITextMarker marker = queryEditor.TextMarkerService.Create(startOffset, 1);
                //    marker.MarkerTypes = TextMarkerTypes.SquigglyUnderline;
                //    marker.MarkerColor = Colors.Red;
                //}

                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(QueryResult)));
            }
        }

        private string title = Properties.Settings.Default.AppTitle;
        public string Title
        {
            get
            {
                return this.title;
            }

            set
            {
                this.title = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
            }
        }

        public string Status
        {
            get { return model.Status; }
            set
            {
                this.model.Status = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Status)));
            }
        }

        public string CaretPositionString
        {
            get { return model.CaretPositionString; }
            set
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CaretPositionString)));
            }
        }

        public void StartWaiting()
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StartWaiting)));
        }

        public void EndWaiting()
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StartWaiting)));
        }


        public Database SelectedDatabase
        {
            get { return model.SelectedDatabase; }
            set
            {
                this.model.SelectedDatabase = value;

                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedDatabase)));
            }
        }

        public ObservableCollection<Database> Databases
        {
            get { return model.Databases; }
            set
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Databases)));
            }
        }

        public void OpenWindowsDialog()
        {
            var dialog = new XppReasoningWpf.Views.WindowsWindow(view.DetailsTab);
            dialog.ShowDialog();
        }

        public void OpenFileInTab(string filename)
        {
            var name = Path.GetFileNameWithoutExtension(filename);
            var tab = this.CreateNewQueryTabItem(name, filename, File.ReadAllText(filename));
            this.view.queryTabPage.SelectedItem = tab;
        }

        private void OpenQueryFile()
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                DefaultExt = ".xq",
                Filter = "XQuery files (*.xq)|*.xq|All files (*.*)|*.*",
            };

            var documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialog.InitialDirectory = string.Format(Properties.Settings.Default.QueriesPath, documentsFolder);

            bool? res = dialog.ShowDialog();

            if (res.HasValue && res.Value)
            {
                foreach (var filename in dialog.FileNames)
                {
                    this.OpenFileInTab(filename);
                }
            }

        }

        /// <summary>
        /// Save the content of the editor into the filename provided.
        /// </summary>
        /// <param name="filename">The name of the file.</param>
        /// <param name="e">Not used.</param>
        private static void SaveQueryFile(string filename, QueryEditor e)
        {
            try
            {
                System.IO.File.WriteAllText(filename, e.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during write of file " + filename + ". " + ex.Message);
            }
        }

        private static string SaveQueryFileAs(QueryEditor e)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                DefaultExt = ".xq",
                AddExtension = true,
                Filter = "XQuery files (*.xq)|*.xq|All files (*.*)|*.*",
            };

            var documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialog.InitialDirectory = string.Format(Properties.Settings.Default.QueriesPath, documentsFolder);

            bool? res = dialog.ShowDialog();

            if (res.HasValue && res.Value)
            {
                var stream = dialog.OpenFile();
                e.Save(stream);
                return dialog.FileName;
            }

            return null;
        }

        private static string ExtractBetweenStrings(string source, string start, string end)
        {
            int startIndex = source.IndexOf(start) + start.Length;
            int endIndex = source.IndexOf(end, startIndex);
            if (startIndex < 0 || endIndex < 0)
                return string.Empty; // or throw an exception
            return source.Substring(startIndex, endIndex - startIndex);
        }

        /// <summary>
        /// Execute the query in the database session
        /// </summary>
        /// <param name="query">The query to execute. This can be either an XQuery string,
        /// or a natural language string that will be submitted to Open AI to get a query.</param>
        /// <param name="session">The BaseX database that handles the query.</param>
        /// <returns>The result of the XQuery database query.</returns>
        async internal Task<string> ExecuteQueryAsync(string query, Session session, PromptEvaluator evaluator)
        {
            string result = "";
            Stopwatch timer = new Stopwatch();
            try
            {
                var settings = Properties.Settings.Default;
                timer.Start();

                var resultingQuery = await evaluator.EvaluatePromptAsync(query);

                // Condition the result from the AI to get the query
                // and the explanation.
                var generatedBasexQuery = ExtractBetweenStrings(resultingQuery, "Query->", "<-Query");
                var explanation = ExtractBetweenStrings(resultingQuery, "E->", "<-E");
                var basexQuery = string.Empty;

                if (generatedBasexQuery.Any())
                {
                    // The system provided a query, so use it.
                    basexQuery = generatedBasexQuery;
                }
                else
                {
                    basexQuery = ExtractBetweenStrings(resultingQuery, "ProvidedQuery->", "<-ProvidedQuery");
                }
                this.Log = $"Query: {query}\nbasexQuery: {basexQuery}Explanation: {explanation}\n\n";

                result = await session.DoQueryAsync(basexQuery,
                    new Tuple<string, string>("database", model.SelectedDatabase.Name),
                    new Tuple<string, string>("server", model.HostName),
                    new Tuple<string, string>(settings.ExternalVariableName1, settings.ExternalVariableValue1),
                    new Tuple<string, string>(settings.ExternalVariableName2, settings.ExternalVariableValue2),
                    new Tuple<string, string>(settings.ExternalVariableName3, settings.ExternalVariableValue3),
                    new Tuple<string, string>(settings.ExternalVariableName4, settings.ExternalVariableValue4),
                    new Tuple<string, string>(settings.ExternalVariableName5, settings.ExternalVariableValue5),
                    new Tuple<string, string>(settings.ExternalVariableName6, settings.ExternalVariableValue6),
                    new Tuple<string, string>(settings.ExternalVariableName7, settings.ExternalVariableValue7));

                return result;
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            finally
            {
                timer.Stop();
                model.Status = "Query elapsed time: " + timer.ElapsedMilliseconds.ToString() + " ms";
            }

            return result;
        }

        private bool sourcePaneActive = false;
        public bool SourcePaneActive
        {
            get { return this.sourcePaneActive;  }
            private set
            {
                if (this.sourcePaneActive != value)
                {
                    this.sourcePaneActive = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SourcePaneActive)));
                }
            }
        }

        // This is the selected source editor
        private XppSourceEditor selectedEditor = null;

        // The number of tab pages that are opened. This is used to determine
        // whether or not the AI pane should be shown.
        private int tabPagesOpen = 0;

        /// <summary>
        /// This method is called when a tab is created, deleted or changed. It is
        /// called from the view.
        /// </summary>
        /// <param name="sender">The tab control.</param>
        /// <param name="e">The event specifying what happened.</param>
        public void DetailsTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var tabItem = e.AddedItems[0] as TabItem;
                var editor = tabItem.Content as XppSourceEditor;
                this.selectedEditor = editor;
            }
            else
            {
                this.selectedEditor = null;
            }

            this.tabPagesOpen = this.tabPagesOpen + e.AddedItems.Count - e.RemovedItems.Count;
            this.SourcePaneActive = tabPagesOpen > 0;
        }

        public TabItem CreateNewQueryTabItem()
        {
            var name = this.NewQueryTabTitle("Query");
            return this.CreateNewQueryTabItem(name, "", "");
        }

        /// <summary>
        /// Get a name for a new tab page.
        /// </summary>
        /// <returns>The new, hitherto unused name.</returns>
        private string NewQueryTabTitle(string prefix)
        {
            int no = 0;

            Regex r = new Regex(prefix + "([0-9]+)");
            // count the tabs that are called <prefix><n> and return n+1
            foreach (TabItem page in this.view.queryTabPage.Items)
            {
                Match m = r.Match(page.Header as string);

                if (m.Success)
                {
                    if (int.TryParse(m.Groups[1].Value, out int thisNumber))
                    {
                        if (thisNumber > no)
                            no = thisNumber;
                    }
                }
            }

            return string.Format(CultureInfo.CurrentUICulture, "{0}{1}", prefix, no + 1);
        }

        public TabItem CreateNewQueryTabItem(string name, string path, string text)
        {
            // If the user has already opened a tab by this name, and the text
            // is the same, then that tab is returned. If there is an existing one, but
            // the content has been changed, a new one is created and the name is 
            // made unambiguous by adding an index.

            foreach (Wpf.Controls.TabItem tab in this.view.queryTabPage.Items)
            {
                if (string.CompareOrdinal(tab.Title, name) == 0)
                {
                    // The name is already in use, but is not changed (since no ending "*").
                    return tab;
                }
                if (string.CompareOrdinal(tab.Title, name + "*") == 0)
                {
                    // The name is already in use, and is modified (since ending "*").
                    name = this.NewQueryTabTitle(name);
                }
            }

            // It is vital that the IsModified call is made after the text is set.
#pragma warning disable IDE0017 // Simplify object initialization
            var editor = new QueryEditor(this);
#pragma warning restore IDE0017 // Simplify object initialization
            editor.Text = text;
            editor.WordWrap = true;
            editor.IsModified = false;

            var item = new Wpf.Controls.TabItem()
            {
                Header = name,
                Tag = new Tuple<string, PromptEvaluator>(path, new PromptEvaluator(PromptEvaluator.FindSystemPrompt)),
                ToolTip = "Unsaved " + name,
                Content = editor,
            };

            item.GotFocus += this.QueryTabGotFocus;
            item.ToolTipOpening += this.QueryTabToolTip;

            // Add it to the tab page
            this.view.queryTabPage.Items.Add(item);

            this.CachedQueryResult[editor] = "";

            // Wire up the modified event on the editor to update the tab title
            var dpd = DependencyPropertyDescriptor.FromProperty(QueryEditor.IsModifiedProperty, typeof(QueryEditor));

            // Wire in code that updates the title as the content transitions
            // from modified to not modified.
            dpd.AddValueChanged(editor, (o, e) =>
            {
                var qe = o as QueryEditor;

                if (qe.IsModified)
                    item.Title += "*";
                else
                {
                    if (item.Title != null)
                        item.Title = item.Title.TrimEnd(new[] { '*' });
                }
            });

            return item;
        }

        private void QueryTabToolTip(object source, RoutedEventArgs args)
        {
            var tab = source as Wpf.Controls.TabItem;
            var tag = tab.Tag as string;

            if (!string.IsNullOrEmpty(tag))
            {
                tab.ToolTip = tag;
            }
        }

        /// <summary>
        /// Closes a tab in the queries tab. If the source is modified (i.e. dirty)
        /// the user is asked if he wants to discard changes.
        /// </summary>
        /// <param name="source">The tab item that is to be closed</param>
        /// <param name="args">Not used.</param>
        public bool CloseQueryTab(Wpf.Controls.TabItem item)
        {
            var content = item.Content as QueryEditor;

            if (content.IsModified)
            {
                // Ask the user if he wants to discard the changes.
                MessageBoxResult result = MessageBox.Show("Discard unsaved changes?", "Unsaved changes", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result != MessageBoxResult.Yes)
                {
                    return false;
                }
            }

            if (item.Parent is TabControl)
            {
                // TODO: Should the structure containing the mapping of name to highest value index
                // be updated too?
                this.CachedQueryResult.Remove(item.Content as QueryEditor);
                // tabControl.Items.Remove(item);
            }
            return true;
        }

        private void QueryTabGotFocus(object source, RoutedEventArgs args)
        {
            // Switch the result view back to any cached value
            var queryEditor = (source as Wpf.Controls.TabItem).Content as QueryEditor;

            if (this.CachedQueryResult.ContainsKey(queryEditor))
                this.view.ResultsEditor.Text = this.CachedQueryResult[queryEditor];
        }

        private static T? FindParentWindow<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);

            // Check if this is the root of the tree
            if (parent == null)
                return null;

            var parentWindow = parent;
            if (parentWindow is not null and T)
            {
                return (T)parentWindow;
            }
            else
            {
                //use recursion until it reaches a Window
                return FindParentWindow<T>(parent);
            }
        }

        private static string IncreaseNumberAfterUnderscore(string input)
        {
            // Check if the input is null or empty
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentException("Input string cannot be null or empty.");
            }

            // Find the last underscore in the string
            int underscoreIndex = input.LastIndexOf('_');
            if (underscoreIndex == -1 || underscoreIndex == input.Length - 1)
            {
                return input + "_1";
            }

            // Extract the substring after the last underscore
            string numberString = input.Substring(underscoreIndex + 1);

            // Try to parse the substring as an integer
            if (int.TryParse(numberString, out int result))
            {
                return input.Substring(0, underscoreIndex) + "_" + (result + 1).ToString();
            }
            else
            {
                return input + "_1";
            }
        }
        public ViewModel(MainWindow view, Model model)
        {
            this.view = view;
            this.model = model;

            model.DatabaseChanging += (databaseName) => {
                this.Status = $"Opening database {databaseName}...";
            };

            model.DatabaseChanged += (databaseName) => {
                this.Status = "";
            };

            // Take care of events bubbling up from the model.
            model.PropertyChanged += (object sender, PropertyChangedEventArgs e) =>
            {
                if (e.PropertyName == "CaretPositionString")
                {
                    this.CaretPositionString = model.CaretPositionString;
                }
                else if (e.PropertyName == "Status")
                {
                    this.Status = model.Status;
                }
                else if (e.PropertyName == "Databases")
                {
                    this.Databases = model.Databases;
                }
                else if (e.PropertyName == "SelectedDatabase")
                {
                    this.SelectedDatabase = model.SelectedDatabase;
                    this.UpdateConnectionInfo();
                }
                else if (e.PropertyName == "QueryResult")
                {
                    this.QueryResult = model.QueryResult;
                }
                else if (e.PropertyName == "Username" || e.PropertyName == "HostName" || e.PropertyName == "SelectedDatabase")
                {
                    this.UpdateConnectionInfo();
                }
                else
                {
                    throw new ArgumentException("Property " + e.PropertyName + " was not handled");
                }
            };

            this.AboutBoxCommand = new RelayCommand(
                p =>
                {
                    var aboutBox = new Views.AboutBox();
                    aboutBox.Show();
                }
            );

            this.XQueryHelpCommand = new RelayCommand(
                p =>
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = "https://www.w3.org/standards/xml/query",
                        UseShellExecute = true
                    };
                    Process.Start(psi);
                }
            );

            this.BaseXHelpCommand = new RelayCommand(
                p =>
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = "http://BaseX.org",
                        UseShellExecute = true
                    };
                    Process.Start(psi);
                }
            );

            this.ShowExternalVariablesDialogCommand = new RelayCommand(
                p =>
                {
                    var window = new ExternalVariablesControl();
                    window.ShowDialog();
                }
            );

            this.SaveResultsCommand = new RelayCommand(
                p =>
                {
                    var dialog = new SaveFileDialog
                    {
                        DefaultExt = ".xml",
                        AddExtension = true,
                        Filter = "XML files (*.xml)|*.xml|CSV (Comma delimited) (*.csv)|*.csv|All files (*.*)|*.*",
                    };

                    var documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    dialog.InitialDirectory = string.Format(Properties.Settings.Default.QueriesPath, documentsFolder);

                    bool? res = dialog.ShowDialog();

                    if (res.HasValue && res.Value)
                    {
                        var stream = dialog.OpenFile();
                        this.view.ResultsEditor.Save(stream);
                    }
                }
            );
            
            this.OpenQueryCommand = new RelayCommand(
                p =>
                {
                    this.OpenQueryFile();
                }
            );

            this.ExitApplicationCommand = new RelayCommand(
                p => {
                    this.view.Close();
                });

            this.KeyboardExecuteQueryCommand = new RelayCommand(
                p =>
                {
                    var item = view.queryTabPage.SelectedItem as Wpf.Controls.TabItem;
                    this.ExecuteQueryCommand.Execute(item.Content);
                },
                p =>
                {
                    var item = view.queryTabPage.SelectedItem as Wpf.Controls.TabItem;
                    return this.ExecuteQueryCommand.CanExecute(item.Content);
                });

            this.KeyboardCheckQueryCommand = new RelayCommand(
                p =>
                {
                    var item = view.queryTabPage.SelectedItem as Wpf.Controls.TabItem;
                    this.CheckQueryCommand.Execute(item.Content);
                },
                p =>
                {
                    var item = view.queryTabPage.SelectedItem as Wpf.Controls.TabItem;
                    return this.CheckQueryCommand.CanExecute(item.Content);
                });


            this.WindowsCommand = new RelayCommand(
                p =>
                {
                    this.OpenWindowsDialog();
                },
                p => { return true; }
            );

            this.CloseAllWindowsCommand = new RelayCommand(
                p =>
                {
                    this.view.DetailsTab.Items.Clear();
                },
                p => this.view.DetailsTab.Items.Count > 0
            );

            this.SaveCommand = new RelayCommand(
                p => // The parameter is the index of the selected tab
                {
                    var tab = view.queryTabPage.SelectedValue as Wpf.Controls.TabItem;
                    var editor = tab.Content as QueryEditor;

                    if (string.IsNullOrEmpty(tab.Tag as string))
                    {
                        var filename = SaveQueryFileAs(editor);
                        if (filename != null)
                        {
                            // The user entered a file, he did not cancel
                            tab.Tag = filename;

                            // Update the tab's title
                            tab.Title = Path.GetFileNameWithoutExtension(filename);
                            // Remove dirty indication
                            editor.IsModified = false;
                        }
                    }
                    else
                    {
                        SaveQueryFile(tab.Tag as string, editor);
                        editor.IsModified = false;
                    }
                },
                p =>
                {
                    return true;
                }
            );

            this.SaveAsCommand = new RelayCommand(
                p => // The parameter is the index of the selected tab
                {
                    var tab = view.queryTabPage.SelectedValue as Wpf.Controls.TabItem;
                    var editor = tab.Content as QueryEditor;

                    var filename = SaveQueryFileAs(editor);
                    if (filename != null)
                    {
                        tab.Tag = filename;

                        // Update the tab's title
                        tab.Title = Path.GetFileNameWithoutExtension(filename);

                        editor.IsModified = false;
                    }
                },
                p =>
                {
                    return true;
                }
            );

            this.CreateNewQueryCommand = new RelayCommand(
                p =>
                {
                    this.view.queryTabPage.SelectedItem = this.CreateNewQueryTabItem();
                }
            );

            this.OpenQueuedQueriesWindow = new RelayCommand(
                p =>
                {
                    // Open the queued queries window. This is a singleton
                    this.QueuedQueriesWindow.Show();
                    this.QueuedQueriesWindow.Activate();

                });


            this.IncreaseResultsFontSizeCommand = new RelayCommand(
                p1 => Properties.Settings.Default.ResultsFontSize += 2,
                p2 => this.view.ResultsEditor != null && this.view.ResultsEditor.FontSize < 48
            );
        

            this.DecreaseResultsFontSizeCommand = new RelayCommand(
                p1 => Properties.Settings.Default.ResultsFontSize -= 2,
                p2 => this.view.ResultsEditor != null && this.view.ResultsEditor.FontSize > 8
            );

            this.IncreaseLogFontSizeCommand = new RelayCommand(
                p1 => Properties.Settings.Default.LogFontSize += 2,
                p2 => this.view.Log != null && this.view.Log.FontSize < 48
            );

            this.DecreaseLogFontSizeCommand = new RelayCommand(
                p1 => Properties.Settings.Default.LogFontSize -= 2,
                p2 => this.view.Log != null && this.view.Log.FontSize > 8
            );

            this.IncreaseAIFontSizeCommand = new RelayCommand(
                p1 => Properties.Settings.Default.AIFontSize += 2,
                p2 => this.view.AIEditor != null && this.view.AIEditor.FontSize < 48
            );

            this.DecreaseAIFontSizeCommand = new RelayCommand(
                p1 => Properties.Settings.Default.AIFontSize -= 2,
                p2 => this.view.AIEditor != null && this.view.AIEditor.FontSize > 8
            );

            this.IncreaseQueryFontSizeCommand = new RelayCommand(
                p1 => Properties.Settings.Default.QueryFontSize += 2,
                p2 =>
                {
                    if (this.view.queryTabPage == null)
                        return false;
                    else
                    {
                        return this.view.queryTabPage.SelectedContent is QueryEditor queryEditor && queryEditor.FontSize < 48;
                    }
                });
        

            this.DecreaseQueryFontSizeCommand = new RelayCommand(
                p1 => Properties.Settings.Default.QueryFontSize -= 2,
                p2 =>
                {
                    if (this.view.queryTabPage == null)
                        return false;
                    else
                    {
                        return this.view.queryTabPage.SelectedContent is QueryEditor queryEditor && queryEditor.FontSize > 8;
                    }
                }
            );
        
            this.ResultsUndoCommand = new RelayCommand(
                p =>
                {
                    ResultsEditor editor = p as ResultsEditor;
                    editor.Undo();
                },
                p =>
                {
                    if (p is ResultsEditor editor)
                        return editor.CanUndo;
                    else
                        return false;
                }
            );
        
            this.ResultsRedoCommand = new RelayCommand(
                p =>
                {
                    ResultsEditor editor = p as ResultsEditor;
                    editor.Redo();
                },
                p =>
                {
                    if (p is ResultsEditor editor)
                        return editor.CanRedo;
                    else
                        return false;
                }
            );

            this.QueryUndoCommand = new RelayCommand(
                p =>
                {
                    QueryEditor editor = p as QueryEditor;
                    editor.Undo();
                },
                p =>
                {
                    if (p is QueryEditor editor)
                        return editor.CanUndo;
                    else
                        return false;
                }
            );
        

            this.QueryRedoCommand = new RelayCommand(
                p =>
                {
                    QueryEditor editor = p as QueryEditor;
                    editor.Redo();
                },
                p =>
                {
                    if (p is QueryEditor editor)
                        return editor.CanRedo;
                    else
                        return false;
                }
            );

            this.ClearLogCommand = new RelayCommand(
                p =>
                {
                    this.Log = string.Empty;
                });

            // Dictionary<string, string> derivedTabNames = new Dictionary<string, string>();

            this.ExecuteAICommand = new RelayCommand(
                async p =>
                {
                    if (this.AIPromptEvaluator == null)
                    {
                        this.AIPromptEvaluator = new PromptEvaluator(PromptEvaluator.ManipulateSystemPrompt);
                    }

                    // We know that a source tab is selected, otherwise we would not be
                    // able to execute the command.
                    // TODO: If there is a selection, use it. Otherwise use the whole editor content.
                    var currentTabItem = this.selectedEditor.Parent as TabItem;
                    var currentTabName = (currentTabItem.Header as TextBlock).Text;

                    var newTabName = IncreaseNumberAfterUnderscore(currentTabName);

                    var sourceCode = this.selectedEditor.Text;

                    string result;
                    var currentCursor = selectedEditor.Cursor;
                    try
                    {
                        selectedEditor.Cursor = Cursors.Wait;
                        var prompt = (string)p;
                        result = await this.AIPromptEvaluator.EvaluatePromptAsync(sourceCode + Environment.NewLine + prompt);
                    }
                    finally
                    {
                        selectedEditor.Cursor = currentCursor;
                    }

                    var tabPage = currentTabItem.Parent as TabControl;
                    var newTab = new Wpf.Controls.TabItem()
                    {
                        Tag = newTabName,
                        Header = new TextBlock() { Text = newTabName },
                    };

                    var newEditor = new XppSourceEditor();
                    newEditor.Text = result;
                    newEditor.WordWrap = true;
                    newEditor.IsReadOnly = false;
                    newTab.Content = newEditor;
                    tabPage.Items.Add(newTab);

                    // Now that we have a result: Open it in a new tab page.

                });

            this.ExecuteQueryCommand = new RelayCommand(
                async p =>
                {
                    var queryEditor = p as QueryEditor;
                    string query;

                    if (queryEditor.SelectionLength > 0)
                    {
                        // The user selected some text, so use that as the query.
                        query = queryEditor.SelectedText;
                    }
                    else
                    {
                        // No selection, so assume whole editor content
                        query = queryEditor.Text;
                    }

                    string result;
                    Stopwatch queryExecutionTime = new Stopwatch();
                    queryExecutionTime.Start();

                    var tabItem = queryEditor.Parent as Wpf.Controls.TabItem;
                    using (var session = await model.Server.GetSessionAsync(this.model.SelectedDatabase.Name))
                    {
                        try
                        {
                            tabItem.Icon = new Image
                            {
                                Source = new BitmapImage(new Uri("images/Hourglass_16x.png", UriKind.Relative)),
                                Width = 16,
                                Height = 16,
                            };

                            var tabPageInfo = tabItem.Tag as Tuple<string, PromptEvaluator>;
                            var evaluator = tabPageInfo.Item2;

                            result = await this.ExecuteQueryAsync(query, session, evaluator);
                        }
                        catch (Exception e)
                        {
                            result = e.Message;
                        }
                        finally
                        {
                            tabItem.Icon = null;
                        }
                    }

                    queryExecutionTime.Stop();

                    // Log the result in the QueryResult so we can select it again if the 
                    // user comes back to the query editor.
                    this.CachedQueryResult[queryEditor] = result;

#if !NETCOREAPP
                    // Log the fact that a query has been evaluated to telemetry.
                    (Application.Current as App).Telemetry?.TrackEvent(
                        "QueryExecution", new Dictionary<string, string>()
                        {
                            ["Query"] = query,
                            ["ExecutionTime"] = queryExecutionTime.ElapsedMilliseconds.ToString()
                        }
                    );
#endif
                    this.model.QueryResult = result;
                },

                // Running is allowed when there is text there to submit as a query and
                // there is a connection to the database.
                p =>
                {
                    if (p is QueryEditor queryEditor)
                        return (queryEditor.Text.Length > 0) && this.SelectedDatabase != null && !string.IsNullOrEmpty(this.SelectedDatabase.Name);
                    else
                        return false;
                });

            this.CheckQueryCommand = new RelayCommand(
               async p =>
               {
                   var queryEditor = p as QueryEditor;
                   string query;

                   if (queryEditor.SelectionLength > 0)
                   {
                       // The user selected some text, so use that as the query.
                       query = queryEditor.SelectedText;
                   }
                   else
                   {
                       // No selection, so assume whole editor content
                       query = queryEditor.Text;
                   }

                   string result;
                   Stopwatch queryCheckTime = new Stopwatch();
                   queryCheckTime.Start();

                   using (var session = await model.Server.GetSessionAsync(this.model.SelectedDatabase.Name))
                   {
                       try
                       {
                           result = await session.CheckQueryAsync(query);
                       }
                       catch (Exception e)
                       {
                           result = e.Message;
                       }
                   }

                   queryCheckTime.Stop();

                   // Log the result in the QueryResult so we can select it again if the 
                   // user comes back to the query editor.
                   this.CachedQueryResult[queryEditor] = result;

#if !NETCOREAPP
                   // Log the fact that a query has been evaluated to telemetry.
                   (Application.Current as App).Telemetry?.TrackEvent(
                      "CheckQuery", new Dictionary<string, string>()
                      {
                          ["Query"] = query,
                          ["ExecutionTime"] = queryCheckTime.ElapsedMilliseconds.ToString()
                      }
                  );
#endif
                   this.model.QueryResult = result;
               },

               // Running is allowed when there is text there to submit as a query and
               // there is a connection to the database.
               p =>
               {
                   if (p is QueryEditor queryEditor)
                       return (queryEditor.Text.Length > 0) && this.SelectedDatabase != null && !string.IsNullOrEmpty(this.SelectedDatabase.Name);
                   else
                       return false;
               });


            this.SubmitQueryCommand = new RelayCommand(
              async p =>
              {
                  var queryEditor = p as QueryEditor;
                  string query;

                  if (queryEditor.SelectionLength > 0)
                  {
                      // The user selected some text, so use that as the query.
                      query = queryEditor.SelectedText;
                  }
                  else
                  {
                      // No selection, so assume whole editor content
                      query = queryEditor.Text;
                  }

                  string result;
                  try
                  {
                      using var session = await this.model.GetSessionAsync(this.model.SelectedDatabase.Name);
                      result = await session.SubmitQueryAsync(query);
                  }
                  catch (Exception e)
                  {
                      result = e.Message;
                  }
                  this.model.JobIdToQuery[result] = query;
                  this.model.QueryResult = result;
              },

               // Running is allowed when there is text there to submit as a query and
               // there is a connection to the database.
               p =>
               {
                   if (p is QueryEditor queryEditor)
                       return (queryEditor.Text.Length > 0);
                   else
                       return false;
               });
        }

        /// <summary>
        /// Called when the system shuts down. This method will stop all interactive jobs
        /// that the user may have started, so they do not burden the server.
        /// </summary>
        public async Task ClosedownAsync()
        {
            // Shut down any running interactive queries
            string result = "";
            const string jobDetailsQuery = "xquery jobs:list-details()";

            using (var session = this.model.GetSessionAsync("").Result)
            {
                result = $"<Jobs>{session.Execute(jobDetailsQuery)}</Jobs>";
            }
            XDocument document = XDocument.Parse(result);

            using (var session = this.model.GetSessionAsync("").Result)
            {
                foreach (var job in document.Document.XPathSelectElements("//job"))
                {
                    var jobId = job.Attribute("id").Value;
                    var user = job.Attribute("user").Value;
                    var jobType = job.Attribute("type").Value;

                    // Stop this user's query (but not any submitted queries)
                    if (string.Compare(jobType, "XQuery") == 0
                     && string.Compare(user, this.model.Username) == 0)
                    {
                        session.Execute($"xquery jobs:stop('{jobId}')");
                    }
                }
            }
            await this.model.CloseConnectionToServerAsync();
        }

        private void UpdateConnectionInfo()
        {
            this.Title = Properties.Settings.Default.AppTitle + "  - " + this.model.ConnectionString;
        }
    }
}
