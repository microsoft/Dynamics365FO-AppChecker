// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.IO;
using System.Linq;
using System.Windows;

namespace GraphExplorer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var wnd = new MainWindow(e.Args);
            wnd.Show();
        }
    }
}
