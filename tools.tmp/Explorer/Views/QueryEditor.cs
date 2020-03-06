// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.Linq;

namespace XppReasoningWpf
{
    using ICSharpCode.AvalonEdit;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    public class QueryEditor : SourceEditor
    {
        private static readonly Snippets snippets = new Snippets();

        public QueryEditor(ViewModels.ViewModel viewModel)
            :this()
        {
            this.InputBindings.Add(new KeyBinding(viewModel.KeyboardExecuteQueryCommand, new KeyGesture(Key.F5, ModifierKeys.None, "F5")));
            this.InputBindings.Add(new KeyBinding(viewModel.KeyboardExecuteQueryCommand, new KeyGesture(Key.E, ModifierKeys.Control, "Ctrl-E")));
            this.InputBindings.Add(new KeyBinding(viewModel.KeyboardCheckQueryCommand, new KeyGesture(Key.F5, ModifierKeys.Control, "Ctrl+F5")));
        }

        public QueryEditor()
        {
            var fontFamilyBinding = new Binding("QueryFont")
            {
                Source = Properties.Settings.Default
            };
            this.SetBinding(FontFamilyProperty, fontFamilyBinding);

            var fontSizeBinding = new Binding("QueryFontSize")
            {
                Source = Properties.Settings.Default
            };
            this.SetBinding(FontSizeProperty, fontSizeBinding);

            var showLineNumbersBinding = new Binding("ShowLineNumbers")
            {
                Source = Properties.Settings.Default
            };
            this.SetBinding(ShowLineNumbersProperty, showLineNumbersBinding);

            this.SyntaxHighlighting = this.LoadHighlightDefinition("XQuery.xshd");

            this.Options = new TextEditorOptions
            {
                ConvertTabsToSpaces = true,
                IndentationSize = 4,
            };

            ICSharpCode.AvalonEdit.Search.SearchPanel.Install(this.TextArea);

            this.IsReadOnly = false;

            // Adding to the context menu...
            var contextMenuItems = (this.ContextMenu.ItemsSource as Control[]).ToList();
            contextMenuItems.Add(new Separator());
            var templatesMenuItem = new MenuItem
            {
                Header = "Snippets"
            };
            contextMenuItems.Add(templatesMenuItem);

            foreach (var snippetPair in snippets.NamedSnippets)
            {
                var snippetName = snippetPair.Key;
                var snippet = snippetPair.Value;

                templatesMenuItem.Items.Add(new MenuItem() {
                    Header = snippetName,
                    Command = new RelayCommand(
                        p => {
                            snippet.Insert(this.TextArea);
                        })
                });
            }

            this.ContextMenu.ItemsSource = contextMenuItems;
        }

        protected override void MouseWheelHandler(object sender, MouseWheelEventArgs e)
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

    }
}
