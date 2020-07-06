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
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using System.Xml.XPath;

namespace XppReasoningWpf.ViewModels
{
    public class ViewModel : INotifyPropertyChanged
    {
        private Model model;
        private MainWindow view;

        /// <summary>
        /// Contains the mapping from a query editor to the result that this query produced.
        /// </summary>
        private IDictionary<QueryEditor, string> CachedQueryResult = new Dictionary<QueryEditor, string>();

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly ICommand exitApplicationCommand;
        public ICommand ExitApplicationCommand => this.exitApplicationCommand;

        private readonly ICommand keyboardExecuteQueryCommand;
        public ICommand KeyboardExecuteQueryCommand => this.keyboardExecuteQueryCommand;

        private readonly ICommand keyboardCheckQueryCommand;
        public ICommand KeyboardCheckQueryCommand => this.keyboardCheckQueryCommand;

        private readonly ICommand executeQueryCommand;
        public ICommand ExecuteQueryCommand => this.executeQueryCommand;

        private readonly ICommand checkQueryCommand;
        public ICommand CheckQueryCommand => this.checkQueryCommand;

        private readonly ICommand submitQueryCommand;
        public ICommand SubmitQueryCommand => this.submitQueryCommand;

        private readonly ICommand windowsCommand;
        public ICommand WindowsCommand
        {
            get => this.windowsCommand;
        }

        private readonly ICommand closeAllWindowsCommand;
        public ICommand CloseAllWindowsCommand
        {
            get => this.closeAllWindowsCommand;
        }

        private readonly ICommand saveCommand;
        public ICommand SaveCommand
        {
            get => this.saveCommand;
        }

        private readonly ICommand saveAsCommand;
        public ICommand SaveAsCommand
        {
            get => this.saveAsCommand;
        }

        private readonly ICommand openQueryCommand;
        public ICommand OpenQueryCommand
        {
            get => this.openQueryCommand;
        }

        private readonly ICommand createNewQueryCommand;
        public ICommand CreateNewQueryCommand
        {
            get => this.createNewQueryCommand;
        }

        private readonly ICommand openQueuedQueriesWindow;
        public ICommand OpenQueuedQueriesWindow
        {
            get => this.openQueuedQueriesWindow;
        }

        public ICommand ResultsUndoCommand
        {
            get => new RelayCommand(
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
        }

        public ICommand ResultsRedoCommand
        {
            get => new RelayCommand(
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
        }

        public ICommand QueryUndoCommand
        {
            get => new RelayCommand(
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
        }

        public ICommand QueryRedoCommand
        {
            get => new RelayCommand(
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
        }

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


        public string QueryResult
        {
            get { return this.model.QueryResult; }
            set
            {
                // if (this.model.QueryResult != value)
                {
                    // this.model.QueryResult = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(QueryResult)));
                }
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
        private void SaveQueryFile(string filename, QueryEditor e)
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

        private string SaveQueryFileAs(QueryEditor e)
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

        async public Task<string> ExecuteQueryAsync(string query, Session session)
        {
            string result = "";
            Stopwatch timer = new Stopwatch();
            try
            {
                var settings = Properties.Settings.Default;
                timer.Start();

                result = await session.DoQueryAsync(query,
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
            // the content has been changed, a a new one is created and the name is 
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
            editor.IsModified = false;

            var item = new Wpf.Controls.TabItem()
            {
                Header = name,
                Tag = path,
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

            if (item.Parent is TabControl tabControl)
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

        public ViewModel(MainWindow view, Model model)
        {
            this.view = view;
            this.model = model;

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
                    this.UpdateConnectionInfo(model);
                }
                else if (e.PropertyName == "QueryResult")
                {
                    this.QueryResult = model.QueryResult;
                }
                else if (e.PropertyName == "Username" || e.PropertyName == "HostName" || e.PropertyName == "SelectedDatabase")
                {
                    this.UpdateConnectionInfo(model);
                }
                else
                {
                    throw new ArgumentException("Property " + e.PropertyName + " was not handled");
                }
            };

            this.openQueryCommand = new RelayCommand(
                p =>
                {
                    this.OpenQueryFile();
                }
            );

            this.exitApplicationCommand = new RelayCommand(
                p => {
                    Application.Current.Shutdown();
                });

            this.keyboardExecuteQueryCommand = new RelayCommand(
                p =>
                {
                    var item = view.queryTabPage.SelectedItem as Wpf.Controls.TabItem;
                    this.executeQueryCommand.Execute(item.Content);
                },
                p =>
                {
                    var item = view.queryTabPage.SelectedItem as Wpf.Controls.TabItem;
                    return this.executeQueryCommand.CanExecute(item.Content);
                });

            this.keyboardCheckQueryCommand = new RelayCommand(
                p =>
                {
                    var item = view.queryTabPage.SelectedItem as Wpf.Controls.TabItem;
                    this.checkQueryCommand.Execute(item.Content);
                },
                p =>
                {
                    var item = view.queryTabPage.SelectedItem as Wpf.Controls.TabItem;
                    return this.checkQueryCommand.CanExecute(item.Content);
                });


            this.windowsCommand = new RelayCommand(
                p =>
                {
                    this.OpenWindowsDialog();
                },
                p => { return true; }
            );

            this.closeAllWindowsCommand = new RelayCommand(
                p =>
                {
                    this.view.DetailsTab.Items.Clear();
                },
                p => this.view.DetailsTab.Items.Count > 0
            );

            this.saveCommand = new RelayCommand(
                p => // The parameter is the index of the selected tab
                {
                    var tab = view.queryTabPage.SelectedValue as Wpf.Controls.TabItem;
                    var editor = tab.Content as QueryEditor;

                    if (string.IsNullOrEmpty(tab.Tag as string))
                    {
                        var filename = this.SaveQueryFileAs(editor);
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
                        this.SaveQueryFile(tab.Tag as string, editor);
                        editor.IsModified = false;
                    }
                },
                p =>
                {
                    return true;
                }
            );

            this.saveAsCommand = new RelayCommand(
                p => // The parameter is the index of the selected tab
                {
                    var tab = view.queryTabPage.SelectedValue as Wpf.Controls.TabItem;
                    var editor = tab.Content as QueryEditor;

                    var filename = this.SaveQueryFileAs(editor);
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

            this.createNewQueryCommand = new RelayCommand(
                p =>
                {
                    this.view.queryTabPage.SelectedItem = this.CreateNewQueryTabItem();
                }
            );


            this.openQueuedQueriesWindow = new RelayCommand(
                p =>
                {
                    // Open the queued queries window. This is a singleton
                    this.QueuedQueriesWindow.Show();
                    this.QueuedQueriesWindow.Activate();

                });

            this.executeQueryCommand = new RelayCommand(
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
                    using (var session = model.Server.GetSession(this.model.SelectedDatabase.Name))
                    {
                        try
                        {
                            tabItem.Icon = new Image
                            {
                                Source = new BitmapImage(new Uri("images/Hourglass_16x.png", UriKind.Relative)),
                                Width = 16,
                                Height = 16,
                            };
                            result = await this.ExecuteQueryAsync(query, session);
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

            this.checkQueryCommand = new RelayCommand(
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

                   using (var session = model.Server.GetSession(this.model.SelectedDatabase.Name))
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


            this.submitQueryCommand = new RelayCommand(
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
                      using (var session = this.model.GetSession(this.model.SelectedDatabase.Name))
                      {
                          result = await session.SubmitQueryAsync(query);
                      }
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
        public void Closedown()
        {
            // Shut down any running interactive queries
            string result = "";
            const string jobDetailsQuery = "xquery jobs:list-details()";

            using (var session = this.model.GetSession(""))
            {
                result = $"<Jobs>{session.Execute(jobDetailsQuery)}</Jobs>";
            }
            XDocument document = XDocument.Parse(result);

            // Build the list from the server:
            IList<SubmittedQueryDescriptor> serverList = new List<SubmittedQueryDescriptor>();

            using (var session = this.model.GetSession(""))
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
            this.model.CloseConnectionToServer();
        }

        private void UpdateConnectionInfo(Model model)
        {
            this.Title = Properties.Settings.Default.AppTitle + "  - " + this.model.ConnectionString;
        }
    }
}
