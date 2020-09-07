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
        public ConfigEditWindow()
        {
            InitializeComponent();

            this.ConfigEditor.SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance.GetDefinition("JavaScript");
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            this.ConfigEditor.Text = Properties.Settings.Default.Configuration;
        }
    }
}
