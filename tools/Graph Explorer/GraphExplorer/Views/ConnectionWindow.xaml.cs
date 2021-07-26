using GraphExplorer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GraphExplorer.Views
{
    /// <summary>
    /// Interaction logic for ConnectionWindow.xaml
    /// </summary>
    public partial class ConnectionWindow : Window
    {
        private Model model;

        public ConnectionWindow(Model model)
        {
            this.InitializeComponent();

            this.DataContext = model;
            this.model = model;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            this.UsernameControl.Focus();
        }

        public string Username { get { return this.UsernameControl.Text; } }
        public string Password { get { return this.PasswordControl.Password; } }

        private async void OkButtonClicked(object sender, RoutedEventArgs e)
        {
            this.StatusControl.Text = "Connecting...";

            // Allow this UI change to propagate through the message pump:
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(() => { }));

            var oldCursor = Mouse.OverrideCursor;
            Mouse.OverrideCursor = Cursors.Wait; // set the cursor to loading spinner

            try
            {
                bool connectionEstablished = await this.model.IsServerOnlineAsync(
                    Properties.Settings.Default.Server, Properties.Settings.Default.Port,
                    this.Username, this.Password);

                if (connectionEstablished)
                {
                    this.model.Username = this.Username;
                    this.model.Server = this.ServerNameControl.Text;
                    this.DialogResult = true;
                }
                else
                {
                    this.StatusControl.Foreground = Brushes.Red;
                    this.StatusControl.Text = "Unable to connect, or bad credentials provided.";
                }
            }
            finally
            {
                Mouse.OverrideCursor = oldCursor;
            }
        }

        /// <summary>
        /// Called when the user name is changed by the user. In this case any
        /// message about connection failure should be removed.
        /// </summary>
        /// <param name="sender">Not used</param>
        /// <param name="e">Not used</param>
        private void UserNameTextChanged(object sender, TextChangedEventArgs e)
        {
            this.StatusControl.Text = string.Empty;
        }
    }
}

