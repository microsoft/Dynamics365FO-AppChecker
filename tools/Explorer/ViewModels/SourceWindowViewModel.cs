// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using XppReasoningWpf.Views;

namespace XppReasoningWpf.ViewModels
{
    class SourceWindowViewModel
    {
        private ObservableCollection<SourceWindowDescriptor> tabDescriptors = new ObservableCollection<SourceWindowDescriptor>();

        private static string TabTitle(TabItem ti)
        {
            return (ti.Header as TextBlock).Text;
        }

        public SourceWindowViewModel(WindowsWindow window, TabControl sourceTabs)
        {
            foreach (TabItem t in sourceTabs.Items.OfType<TabItem>().OrderBy(ti => TabTitle(ti)))
            {
                this.tabDescriptors.Add(new SourceWindowDescriptor(TabTitle(t)));
            }

            window.SourceList.ItemsSource = this.tabDescriptors;

            this.closeSourceTabCommand = new RelayCommand(
                p => {
                    // Close all the selected items in the list view
                    foreach (SourceWindowDescriptor descriptor in window.SourceList.SelectedItems.OfType<SourceWindowDescriptor>().ToList())
                    {
                        var tabHeader = descriptor.Name;
                        foreach (TabItem ti in sourceTabs.Items.OfType<TabItem>().ToList())
                        {
                            if (TabTitle(ti) == tabHeader)
                            {
                                sourceTabs.Items.Remove(ti);
                                this.tabDescriptors.Remove(descriptor);
                                break;
                            }
                        }
                    }
                },

                p => { return window.SourceList.SelectedItems.Count > 0; }
            );

            this.activateSourceTabCommand = new RelayCommand(
               p => {
                    SourceWindowDescriptor descriptor = window.SourceList.SelectedItem as SourceWindowDescriptor;

                    var tabHeader = descriptor.Name;
                    foreach (TabItem ti in sourceTabs.Items.OfType<TabItem>().ToList())
                    {
                        if (TabTitle(ti) == tabHeader)
                        {
                            sourceTabs.SelectedItem = ti;
                            break;
                        }
                    }
                    window.Close();
               },

               p => { return window.SourceList.SelectedItems.Count == 1; }
            );
        }

        private readonly ICommand activateSourceTabCommand;
        public ICommand ActivateSourceTabCommand
        {
            get => this.activateSourceTabCommand;
        }

        private readonly ICommand closeSourceTabCommand;
        public ICommand CloseSourceTabCommand
        {
            get => this.closeSourceTabCommand;
        }

    }
}
