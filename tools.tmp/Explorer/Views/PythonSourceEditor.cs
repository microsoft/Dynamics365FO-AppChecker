// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace XppReasoningWpf
{
    using System.Windows.Data;

    class PythonSourceEditor : SourceEditor
    {
        public PythonSourceEditor()
        {
            var fontFamilyBinding = new Binding("SourceFont");
            fontFamilyBinding.Source = Properties.Settings.Default;
            this.SetBinding(FontFamilyProperty, fontFamilyBinding);

            this.SyntaxHighlighting = this.LoadHighlightDefinition("Python-Mode.xshd");
        }
    }
}
