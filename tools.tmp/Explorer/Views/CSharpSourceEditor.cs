// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace XppReasoningWpf
{
    using System.Windows.Data;

    class CSharpSourceEditor : SourceEditor
    {
        public CSharpSourceEditor()
        {
            var fontFamilyBinding = new Binding("SourceFont");
            fontFamilyBinding.Source = Properties.Settings.Default;
            this.SetBinding(FontFamilyProperty, fontFamilyBinding);

            this.SyntaxHighlighting = this.LoadHighlightDefinition("CSharp-Mode.xshd");
        }
    }
}
