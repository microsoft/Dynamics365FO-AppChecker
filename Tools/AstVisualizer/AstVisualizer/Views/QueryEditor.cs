// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.Linq;

namespace AstVisualizer
{
    using ICSharpCode.AvalonEdit;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    public class QueryEditor : BindableEditor
    {
        private QueryViewModel viewModel;
        public QueryEditor(QueryViewModel viewModel)
            : this()
        {
            this.viewModel = viewModel;
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
        }
    }
}
