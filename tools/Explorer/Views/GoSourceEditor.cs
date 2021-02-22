// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace XppReasoningWpf
{
    using System.Windows.Data;

    class GoSourceEditor : SourceEditor
    {
        public GoSourceEditor()
        {
            var fontFamilyBinding = new Binding("SourceFont")
            {
                Source = Properties.Settings.Default
            };
            this.SetBinding(FontFamilyProperty, fontFamilyBinding);

            this.SyntaxHighlighting = LoadHighlightDefinition("Go.xshd");
        }
    }
}
