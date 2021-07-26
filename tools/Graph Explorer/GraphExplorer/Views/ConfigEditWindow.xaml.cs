using GraphExplorer.Models;
using GraphExplorer.ViewModels;
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

namespace GraphExplorer.Views
{
    using MaterialDesignExtensions.Controls;

    /// <summary>
    /// Interaction logic for ConfigEditWindow.xaml
    /// </summary>
    public partial class ConfigEditWindow : MaterialWindow
    {
        private EditorViewModel ViewModel { get; set; }
        private JavaScriptEditor Editor { get; set; }

        public ConfigEditWindow(Model model, EditorViewModel viewModel)
        {
            this.ViewModel = viewModel;

            this.InitializeComponent();

            this.Editor = new JavaScriptEditor
            {
                Text = this.ViewModel.StyleDocumentSource
            };

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
