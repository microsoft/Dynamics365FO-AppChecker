// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.Windows;
using System.Windows.Controls;
using XppReasoningWpf.ViewModels;

namespace XppReasoningWpf.Views
{
    /// <summary>
    /// Interaction logic for WIndowsWindow.xaml
    /// </summary>
    public partial class WindowsWindow : Window
    {
        private TabControl SourceTabs;

        public WindowsWindow(TabControl sourceTabs)
        {
            InitializeComponent();
            this.DataContext = new SourceWindowViewModel(this, sourceTabs);

            this.SourceTabs = sourceTabs; 
            
         }
    }
}
