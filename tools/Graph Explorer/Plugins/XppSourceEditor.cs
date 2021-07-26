using ICSharpCode.AvalonEdit.Folding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace GraphExplorer.XppPlugin
{
    /// <summary>
    /// Custom editor for editing X++ source text. It is based on the Avaloneditor, but has the 
    /// X++ colorization rules and brace matching.
    /// </summary>
    class XppSourceEditor : SourceEditor
    {
        public XppSourceEditor(): base()
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
            var xppFoldingStrategy = new XppBraceFoldingStrategy();
            var xppFoldingManager = FoldingManager.Install(this.TextArea);
            xppFoldingStrategy.UpdateFoldings(xppFoldingManager, this.Document);

            this.IsReadOnly = true;

            this.SyntaxHighlighting = LoadHighlightDefinition("SocratexGraphExplorer.XppPlugin.Resources.Xpp-Mode.xshd", typeof(XppSourceEditor).Assembly);
        }
    }
}
