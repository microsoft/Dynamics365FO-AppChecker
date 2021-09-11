using GraphExplorer.ViewModels;
using MaterialDesignExtensions.Controls;
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
    /// <summary>
    /// Interaction logic for ConfigurationWindow.xaml
    /// </summary>
    public partial class ConfigurationWindow : MaterialWindow
    {
        private ConfigurationWindowViewModel ViewModel { get; set; }

        public ConfigurationWindow()
        {
            this.ViewModel = new ConfigurationWindowViewModel();

            this.InitializeComponent();

            this.DataContext = this.ViewModel;
        }
    }
}
