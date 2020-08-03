// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using CefSharp;
using SocratexGraphExplorer.Models;
using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace SocratexGraphExplorer
{
    // https://github.com/cefsharp/cefsharp/issues/2246
    // https://github.com/cefsharp/CefSharp/wiki/General-Usage#3-how-do-you-expose-a-net-class-to-javascript
    // https://github.com/cefsharp/CefSharp/wiki/JavaScript-Binding-API

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ViewModels.EditorViewModel ViewModel { private set; get; }
        private Model model;

        public MainWindow()
        {
            SplashScreen splash = new SplashScreen("Images/SplashScreen with socrates.png");
            splash.Show(false);
            Thread.Sleep(2000);

            InitializeComponent();

            this.Browser.LoadError += (object sender, LoadErrorEventArgs e) =>
            {
            };

            this.Browser.LoadingStateChanged += (sender, args) =>
            {
                //Wait for the Page to finish loading
                if (args.IsLoading == false)
                {
                    //CefSharpSettings.LegacyJavascriptBindingEnabled = true;
                    //CefSharpSettings.WcfEnabled = true;
                    // Browser.JavascriptObjectRepository.Register("csObject", new Views.BrowserCSObjects(), isAsync: true, options: BindingOptions.DefaultBinder);

                    // this.Browser.ExecuteScriptAsync("alert('All Resources Have Loaded');");
                }
            };

            // This will be called when JS needs to resolve an object:
            this.Browser.JavascriptObjectRepository.ResolveObject += (sender, e) =>
            {
                var repo = e.ObjectRepository;
                if (e.ObjectName == "csObject")
                {
                    CefSharpSettings.WcfEnabled = true;
                    BindingOptions options = new BindingOptions()
                    {
                        CamelCaseJavascriptNames = false,
                    };
                    repo.Register("csObject", new Views.BrowserCSObjects(this.model, this.ViewModel), isAsync: true, options: options);
                }
            };

            this.CypherEditor.TextArea.Caret.PositionChanged += (object sender, EventArgs a) =>
            {
                var caret = sender as ICSharpCode.AvalonEdit.Editing.Caret;
                this.model.CaretPositionString = string.Format(CultureInfo.CurrentCulture, "Line: {0} Column: {1}", caret.Line, caret.Column);
            };

            Loaded += MainWindow_Loaded;

            splash.Close(TimeSpan.FromSeconds(0));

            this.model = new Models.Model();
            this.ViewModel = new ViewModels.EditorViewModel(this, model);

            this.InputBindings.Add(new KeyBinding(ViewModel.ExecuteQueryCommand, new KeyGesture(Key.F5, ModifierKeys.None, "F5")));
            this.InputBindings.Add(new KeyBinding(ViewModel.ExecuteQueryCommand, new KeyGesture(Key.E, ModifierKeys.Control, "Ctrl-E")));

            if (!this.model.IsDebugMode)
            {
                this.FileMenu.Items.Remove(this.DeveloperToolsMenuItem);
            }

            // Set up the menu context handler for the browser:
            // this.Browser.MenuHandler = new BrowserContextMenuHandler(this.ViewModel);

            this.DataContext = this.ViewModel;

            string password;
            if (!model.IsDebugMode)
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
            else
            {
                password = "test";
            }

            this.model.Password = password;

            // Now that the value of the connection parameters have been set,
            // the global connection to the database is established.
            this.model.CreateNeo4jDriver(password);
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Browser.LoadHtml(this.model.Source);
            this.TextBrowser.LoadLargeHtmlString(@"<html>
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

        private async void Browser_SizeChangedAsync(object sender, SizeChangedEventArgs e)
        {
            if (this.Browser.CanExecuteJavascriptInMainFrame)
                await this.Browser.EvaluateScriptAsync("setVizSize", e.NewSize.Width - 20, e.NewSize.Height - 20);
        }

    }
}
