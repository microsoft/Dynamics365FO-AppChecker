// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.ComponentModel;
using System.Windows;
using XppReasoningWpf.ViewModels;

namespace XppReasoningWpf.Views
{
    /// <summary>
    /// Interaction logic for WIndowsWindow.xaml
    /// </summary>
    public partial class SubmittedQueriesWindow : Window
    {
        private readonly SubmittedQueriesViewModel viewModel;
        private bool closing = false;

        public SubmittedQueriesWindow(Model model)
        {
            viewModel = new SubmittedQueriesViewModel(this, model);

            this.DataContext = this.viewModel;

            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            this.closing = true;
        }

        protected override async void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            await this.viewModel.GetUpdatesAsync(() => this.closing);
        }
    }
}
