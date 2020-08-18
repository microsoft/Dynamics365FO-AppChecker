﻿using SocratexGraphExplorer.Models;
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

namespace SocratexGraphExplorer.Views
{
    /// <summary>
    /// Interaction logic for ConnectionWindow.xaml
    /// </summary>
    public partial class ConnectionWindow : Window
    {
        private Model model;

        public ConnectionWindow(Model model)
        {
            InitializeComponent();

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
            this.StatusControl.Foreground = Brushes.Black;
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
                    this.model.Server = this.ServerNameControl.Text;
                    this.DialogResult = true;
                }
                else
                {
                    this.StatusControl.Foreground = Brushes.Red;
                    this.StatusControl.Content = "Unable to connect, or bad credentials provided.";
                }
            }
            finally
            {
                Mouse.OverrideCursor = oldCursor;
            }
        }

        //private void ImgShowHide_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        //{
        //    this.HidePassword();
        //}

        //private void ImgShowHide_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    this.ShowPassword();
        //}

        //private void ImgShowHide_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    this.HidePassword();
        //}

        //        /// <summary>
        //        /// Called when the password text changes. When this happens the
        //        /// view switches back to hidden mode (showing ****) and any 
        //        /// residual connection message is removed.
        //        /// </summary>
        //        /// <param name="sender"></param>
        //        /// <param name="e"></param>
        //        private void TxtPasswordbox_PasswordChanged(object sender, RoutedEventArgs e)
        //        {
        //            if (PasswordControl.Password.Length > 0)
        //                IconShowHide.Visibility = Visibility.Visible;
        //            else
        //                IconShowHide.Visibility = Visibility.Hidden;

        //            this.StatusControl.Content = string.Empty;
        //        }

        //        private void ShowPassword()
        //        {
        //            this.IconShowHide.Kind = MaterialDesignThemes.Wpf.PackIconKind.EyeOutline;
        ////            ImgShowHide.Source = new BitmapImage(new Uri("pack://application:,,,/Images/fatter-eye-inverted.png", UriKind.Absolute));
        //            txtVisiblePasswordbox.Visibility = Visibility.Visible;
        //            PasswordControl.Visibility = Visibility.Hidden;
        //            txtVisiblePasswordbox.Text = PasswordControl.Password;
        //        }

        //        private void HidePassword()
        //        {
        //            this.IconShowHide.Kind = MaterialDesignThemes.Wpf.PackIconKind.Eye;
        ////            ImgShowHide.Source = new BitmapImage(new Uri("pack://application:,,,/Images/fatter-eye.png", UriKind.Absolute));
        //            txtVisiblePasswordbox.Visibility = Visibility.Hidden;
        //            PasswordControl.Visibility = Visibility.Visible;
        //            PasswordControl.Focus();
        //        }

        /// <summary>
        /// Called when the user name is changed by the user. In this case any
        /// message about connection failure should be removed.
        /// </summary>
        /// <param name="sender">Not used</param>
        /// <param name="e">Not used</param>
        private void UserNameTextChanged(object sender, TextChangedEventArgs e)
        {
            this.StatusControl.Content = string.Empty;
        }
    }
}

