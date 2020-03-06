// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.Windows;
using System.Windows.Input;

namespace AstVisualizer
{
    using LanguageExtractorInterfaces;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal ViewModel viewModel
        {
            get { return this.DataContext as ViewModel; }
        }

        internal Model Model
        {
            private set; get;
        }

        void EditorPositionChanged(object sender, EventArgs a)
        {
            var caret = sender as ICSharpCode.AvalonEdit.Editing.Caret;
            var vm = this.DataContext as ViewModel;
            vm.CaretPositionString = string.Format("Line: {0} Column: {1}", caret.Line, caret.Column);
        }

        public MainWindow()
        {
            this.InitializeComponent();
            this.Model = new Model();
            this.DataContext = new ViewModel(this, this.Model);

            this.SourceEditor.TextArea.Caret.PositionChanged += EditorPositionChanged;
        }

        private void ResultsEditor_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if (e.Delta > 0)
                {
                    if (Properties.Settings.Default.ResultsFontSize < 48)
                        Properties.Settings.Default.ResultsFontSize += 1;
                }
                else
                {
                    if (Properties.Settings.Default.ResultsFontSize > 8)
                        Properties.Settings.Default.ResultsFontSize -= 1;
                }
            }
        }

        private void SourceEditor_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if (e.Delta > 0)
                {
                    if (Properties.Settings.Default.SourceFontSize < 48)
                        Properties.Settings.Default.SourceFontSize += 1;
                }
                else
                {
                    if (Properties.Settings.Default.SourceFontSize > 8)
                        Properties.Settings.Default.SourceFontSize -= 1;
                }
            }
        }

        protected void QueryEditor_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if (e.Delta > 0)
                {
                    if (Properties.Settings.Default.QueryFontSize < 48)
                        Properties.Settings.Default.QueryFontSize += 1;
                }
                else
                {
                    if (Properties.Settings.Default.QueryFontSize > 8)
                        Properties.Settings.Default.QueryFontSize -= 1;
                }
            }
        }

        protected void QueryResultsEditor_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if (e.Delta > 0)
                {
                    if (Properties.Settings.Default.QueryResultsFontSize < 48)
                        Properties.Settings.Default.QueryResultsFontSize += 1;
                }
                else
                {
                    if (Properties.Settings.Default.QueryResultsFontSize > 8)
                        Properties.Settings.Default.QueryResultsFontSize -= 1;
                }
            }
        }

        private void ErrorList_Selected(object sender, RoutedEventArgs e)
        {
            IDiagnosticItem selectedItem = this.ErrorList.SelectedItem as IDiagnosticItem;

            if (selectedItem != null)
            {
                var startOffset = this.SourceEditor.Document.GetOffset(selectedItem.Line, selectedItem.Column);
                var endOffset = this.SourceEditor.Document.GetOffset(selectedItem.EndLine, selectedItem.EndColumn);

                this.SourceEditor.TextArea.Caret.Position = new ICSharpCode.AvalonEdit.TextViewPosition(selectedItem.Line, selectedItem.Column);

                var selection = ICSharpCode.AvalonEdit.Editing.Selection.Create(this.SourceEditor.TextArea, startOffset, endOffset);
                this.SourceEditor.TextArea.Selection = selection;
            }
        }

        void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate the dialog box
            var dlg = new AboutBox();

            // Open the dialog box modally 
            dlg.ShowDialog();
        }

        private void LangaugeSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            this.viewModel.LanguageSelectionChanged();
        }
    }
}
