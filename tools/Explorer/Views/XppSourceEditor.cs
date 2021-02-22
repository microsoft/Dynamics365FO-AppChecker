// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;

namespace XppReasoningWpf
{
    using ICSharpCode.AvalonEdit.Folding;
    using System.Windows.Data;
    using System.Windows.Input;

    class XppSourceEditor : SourceEditor
    {
        public XppSourceEditor()
        {
            // FontFamily = "{Binding Source={x:Static settings:Settings.Default}, Path=SourceFont}"
            var fontFamilyBinding = new Binding("SourceFont")
            {
                Source = Properties.Settings.Default
            };
            this.SetBinding(FontFamilyProperty, fontFamilyBinding);

            // Configure the X++ folding manager. 
            // The indentation strategy is probably not needed since the view is readonly...
            this.TextArea.IndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.CSharp.CSharpIndentationStrategy(this.Options);
            var xppFoldingStrategy = new BraceFoldingStrategy();
            var xppFoldingManager = FoldingManager.Install(this.TextArea);
            xppFoldingStrategy.UpdateFoldings(xppFoldingManager, this.Document);

            this.SyntaxHighlighting = LoadHighlightDefinition("Xpp-Mode.xshd");

            Model.Tick += (object sender, EventArgs e) =>
            {
                xppFoldingStrategy.UpdateFoldings(xppFoldingManager, this.Document);
            };
        }

 
    }
}
