using SocratexGraphExplorer.Models;
using SocratexGraphExplorer.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SocratexGraphExplorer.Views
{
    /// <summary>
    /// Interaction logic for ConfigEditWindow.xaml
    /// </summary>
    public partial class ConfigEditWindow : Window
    {
        private EditorViewModel ViewModel { get; set; }
        private JavaScriptEditor Editor { get; set; }

        public ConfigEditWindow(Model model, EditorViewModel viewModel)
        {
            this.ViewModel = viewModel;

            InitializeComponent();

            this.Editor = new JavaScriptEditor();
            this.Editor.Text = this.ViewModel.StyleDocumentSource;

            this.OptionsEditor.Content = this.Editor;
        }

        /// <summary>
        /// When the dialog is dismissed, the changes are persisted.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            this.ViewModel.StyleDocumentSource = this.Editor.Text; // Not needed. Using binding
        }
    }
}
