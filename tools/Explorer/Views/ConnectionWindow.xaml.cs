// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Threading;

namespace XppReasoningWpf
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class ConnectionWindow : Window
    {
        private readonly Model model;

        public ConnectionWindow(Model model)
        {
            this.DataContext = model;
            this.model = model;

            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            this.UsernameControl.Focus();

            // Initialize the icon to "Show Password" state
            ImgShowHide.Source = new BitmapImage(new Uri("pack://application:,,,/Images/fatter-eye.png", UriKind.Absolute));
        }

        public string Username { get { return this.UsernameControl.Text; } }
        public string Password { get { return this.PasswordControl.Password; } }

        private async void OkButtonClicked(object sender, RoutedEventArgs e)
        {
            this.StatusControl.Content = "Connecting...";

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
                    this.model.HostName = this.ServerNameControl.Text;

                    this.DialogResult = true;
                    this.model.CreateServer(Properties.Settings.Default.Server, Properties.Settings.Default.Port, this.Username, this.Password);

#if !NETCOREAPP
                var telemetry = (Application.Current as App).Telemetry;
                if (telemetry != null)
                    telemetry.Context.User.AccountId = this.Username;
#endif
                }
                else
                {
                    this.StatusControl.Content = "Unable to connect, or bad credentials provided.";
                    this.model.HostName = "";
                }
            }
            finally
            {
                Mouse.OverrideCursor = oldCursor;
            }
        }

        private void ImgShowHide_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            this.HidePassword();
        }
        private void ImgShowHide_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.ShowPassword();
        }
        private void ImgShowHide_MouseLeave(object sender, MouseEventArgs e)
        {
            this.HidePassword();
        }

        /// <summary>
        /// Called when the password text changes. When this happens the
        /// view switches back to hidden mode (showing ****) and any 
        /// residual connection message is removed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtPasswordbox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (PasswordControl.Password.Length > 0)
                ImgShowHide.Visibility = Visibility.Visible;
            else
                ImgShowHide.Visibility = Visibility.Hidden;

            this.StatusControl.Content = string.Empty;
        }

        private void ShowPassword()
        {
            ImgShowHide.Source = new BitmapImage(new Uri("pack://application:,,,/Images/fatter-eye-inverted.png", UriKind.Absolute));
            txtVisiblePasswordbox.Visibility = Visibility.Visible;
            PasswordControl.Visibility = Visibility.Hidden;
            txtVisiblePasswordbox.Text = PasswordControl.Password;
        }

        private void HidePassword()
        {
            ImgShowHide.Source = new BitmapImage(new Uri("pack://application:,,,/Images/fatter-eye.png", UriKind.Absolute));
            txtVisiblePasswordbox.Visibility = Visibility.Hidden;
            PasswordControl.Visibility = Visibility.Visible;
            PasswordControl.Focus();
        }

        /// <summary>
        /// Called when the user name is changed by the user. In this case any
        /// message about connection failure should be removed.
        /// </summary>
        /// <param name="sender">Not used</param>
        /// <param name="e">Not used</param>
        private void UserNameTextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.StatusControl != null)
            {
                this.StatusControl.Content = string.Empty;
            }
        }

        private void PortControl_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.StatusControl != null)
            {
                this.StatusControl.Content = string.Empty;
            }
        }

        private void ServerNameControl_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.StatusControl != null)
            {
                this.StatusControl.Content = string.Empty;
            }
        }
    }
}
