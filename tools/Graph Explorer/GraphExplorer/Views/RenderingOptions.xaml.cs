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
    /// Interaction logic for RenderingOptions.xaml
    /// </summary>
    public partial class RenderingOptions : MaterialWindow
    {
        private RenderingOptionsViewModel ViewModel { get; set; }

        public RenderingOptions()
        {
            this.InitializeComponent();
            this.ViewModel = new ViewModels.RenderingOptionsViewModel(this);

            this.DataContext = this.ViewModel;
        }
    }
}
