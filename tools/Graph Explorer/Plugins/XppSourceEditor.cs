using ICSharpCode.AvalonEdit.Folding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SocratexGraphExplorer.XppPlugin
{
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
            var xppFoldingStrategy = new BraceFoldingStrategy();
            var xppFoldingManager = FoldingManager.Install(this.TextArea);
            xppFoldingStrategy.UpdateFoldings(xppFoldingManager, this.Document);

            this.IsReadOnly = true;

            this.SyntaxHighlighting = LoadHighlightDefinition("SocratexGraphExplorer.XppPlugin.Resources.Xpp-Mode.xshd");
        }
    }
}
