// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.Web.WebView2.Core;
using SocratexGraphExplorer.Models;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using SocratexGraphExplorer.Views;
using System.Configuration;
using System.IO;
using MaterialDesignColors;
using System.Linq;
using MaterialDesignThemes.Wpf;
using Microsoft.Web.WebView2.Wpf;
using System.Threading.Tasks;
using SocratexGraphExplorer.SourceEditor;

namespace SocratexGraphExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ViewModels.EditorViewModel ViewModel { private set; get; }
        private readonly Model model;

        public MainWindow(string[] args)
        {
            //SplashScreen splash = new SplashScreen("Images/SplashScreen with socrates.png");
            //splash.Show(false);
            //Thread.Sleep(2000);

            InitializeComponent();
            StateChanged += MainWindowStateChangeRaised;

            InitializeAsync().ContinueWith((f) => {
            });

            this.CypherEditor.TextArea.Caret.PositionChanged += (object sender, EventArgs a) =>
            {
                var caret = sender as ICSharpCode.AvalonEdit.Editing.Caret;
                this.model.CaretPositionString = string.Format(CultureInfo.CurrentCulture, "Line: {0} Column: {1}", caret.Line, caret.Column);
            };

            // If a configuration file argument is passed, then read this file and store it in the configuration
            if (args.Length != 0)
            {
                var fileName = args[0];
                try
                {
                    var content = File.ReadAllText(fileName);
                    Properties.Settings.Default.Configuration = content;
                }
                catch
                {
                }
            }

            this.model = new Models.Model();
            this.ViewModel = new ViewModels.EditorViewModel(this, model);

            this.InputBindings.Add(new KeyBinding(ViewModel.ExecuteQueryCommand, new KeyGesture(Key.F5, ModifierKeys.None, "F5")));
            this.InputBindings.Add(new KeyBinding(ViewModel.ExecuteQueryCommand, new KeyGesture(Key.E, ModifierKeys.Control, "Ctrl-E")));

            this.DataContext = this.ViewModel;

            // splash.Close(TimeSpan.FromSeconds(0));

            string password;
            // if (!model.IsDebugMode)
            {
                // Now show the connection dialog
                var connectionWindow = new Views.ConnectionWindow(this.model);
                var connectionResult = connectionWindow.ShowDialog();

                // If the user dismissed the dialog, the system should just be shut down,
                // since there is not a lot that can be done without a connection.
                if (!(connectionResult.HasValue && connectionResult.Value))
                {
                    Environment.Exit(0);
                    return;
                }
                password = connectionWindow.Password;
            }
            //else
            //{
            //    password = "test";
            //}

            // Now that the value of the connection parameters have been set,
            // the global connection to the database can be established.
            this.model.CreateNeo4jDriver(password);
        }

        private async Task InitializeAsync()
        {
            // Make sure everything is set up before doing anything with the browser
            await this.Browser.EnsureCoreWebView2Async(null);
            await this.TextBrowser.EnsureCoreWebView2Async(null);

            this.CypherEditor.SyntaxHighlighting = AvalonSourceEditor.LoadHighlightDefinition("SocratexGraphExplorer.Resources.Cypher-mode.xshd", typeof(MainWindow).Assembly);

            // Set up a function to call when the user clicks on something in the graph browser.
            Browser.WebMessageReceived += async (object sender, CoreWebView2WebMessageReceivedEventArgs args) =>
            {
                string message = args.WebMessageAsJson;
                // The payload will be empty if the user clicked in the empty space
                // it will be {edge: id} if an edge is selected
                // it will be {node: id) if a node is selected
                //var item = System.Text.Json.JsonSerializer.Deserialize<ClickedItem>(message);

                var e = Newtonsoft.Json.Linq.JObject.Parse(message);

                if (e.ContainsKey("nodeId"))
                {
                    var id = e["nodeId"].ToObject<long>();

                    var cypher = "MATCH (c) where id(c) = {id} return c limit 1";
                    this.ViewModel.SelectedNode = id;
                    var nodeResult = await this.model.ExecuteCypherAsync(cypher, new Dictionary<string, object>() { { "id", id } });
                    this.ViewModel.UpdatePropertyListView(nodeResult);

                }
                else if (e.ContainsKey("edgeId"))
                {
                    var id = e["edgeId"].ToObject<long>();

                    var cypher = "MATCH (c) -[r]- (d) where id(r) = {id} return r limit 1";
                    this.ViewModel.SelectedEdge = id;
                    var edgeResult = await this.model.ExecuteCypherAsync(cypher, new Dictionary<string, object>() { { "id", id } });
                    this.ViewModel.UpdatePropertyListView(edgeResult);
                }
                else
                {
                    // blank space selected
                }
            };

            this.Browser.SizeChanged += async (object sender, SizeChangedEventArgs e) =>
            {
                var browser = sender as WebView2;
                //await browser.EnsureCoreWebView2Async();
                await this.ViewModel.SetGraphSizeAsync(browser);
            };

            this.Browser.NavigationCompleted += Browser_NavigationCompleted;
            // The debugger does not work in Edge if the source does not come from a file.
            // Load the script into a temporary file, and use that file in the URI that
            // the debugger loads.
            this.Browser.Source = this.model.ScriptUri;

            this.TextBrowser.NavigateToString(@"<html>
    <head>
        <style>
            html, body, .container {
                height: 100%;
            }
            .container {
                font-size: 24;
                color: lightgray;
                display: flex;
                align-items: center;
                justify-content: center;
            }
        </style>
    </head>
    <body class='container'>
        No information
    </body>
</html>");
            this.ViewModel.GraphModeSelected = true;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            this.ViewModel.Close();
        }

        /// <summary>
        /// This method is called after the browser has finished loading the document. The 
        /// size is set to fill out the frame. The SizeChanged event is triggered too early
        /// to have any effect.
        /// </summary>
        /// <param name="sender">The browser graph view</param>
        /// <param name="e">The event args. Not used.</param>
        private async void Browser_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            var browser = sender as WebView2;
            await browser.EnsureCoreWebView2Async();
            await this.ViewModel.SetGraphSizeAsync(browser);

            browser.NavigationCompleted -= Browser_NavigationCompleted;
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        // Minimize
        private void CommandBinding_Executed_Minimize(object sender, ExecutedRoutedEventArgs e)
        {
            // SystemCommands.MinimizeWindow(this);
        }

        // Maximize
        private void CommandBinding_Executed_Maximize(object sender, ExecutedRoutedEventArgs e)
        {
            // SystemCommands.MaximizeWindow(this);
        }

        // Restore
        private void CommandBinding_Executed_Restore(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }

        // Close
        private void CommandBinding_Executed_Close(object sender, ExecutedRoutedEventArgs e)
        {
            // SystemCommands.CloseWindow(this);
        }

        // State change. This code compensates for the fact that the windows chrome
        // adds a phantom border of 8 pixels around the windows frame.
        private void MainWindowStateChangeRaised(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                MainWindowBorder.BorderThickness = new Thickness(8);
                RestoreButton.Visibility = Visibility.Visible;
                MaximizeButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                MainWindowBorder.BorderThickness = new Thickness(0);
                RestoreButton.Visibility = Visibility.Collapsed;
                MaximizeButton.Visibility = Visibility.Visible;
            }
        }

    }
}
