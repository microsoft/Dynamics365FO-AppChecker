using ICSharpCode.AvalonEdit.Folding;
using SocratexGraphExplorer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SocratexGraphExplorer.Views
{
    class XppSourceEditor : SourceEditor
    {
        public XppSourceEditor(Model model): base(model)
        {
            // FontFamily = "{Binding Source={x:Static settings:Settings.Default}, Path=SourceFont}"
            var fontFamilyBinding = new Binding("SourceFont");
            fontFamilyBinding.Source = Properties.Settings.Default;
            this.SetBinding(FontFamilyProperty, fontFamilyBinding);

            // Configure the X++ folding manager. 
            // The indentation strategy is probably not needed since the view is readonly...
            this.TextArea.IndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.CSharp.CSharpIndentationStrategy(this.Options);
            var xppFoldingStrategy = new BraceFoldingStrategy();
            var xppFoldingManager = FoldingManager.Install(this.TextArea);
            xppFoldingStrategy.UpdateFoldings(xppFoldingManager, this.Document);

            this.SyntaxHighlighting = LoadHighlightDefinition("Xpp-Mode.xshd");

            //model.Tick += (object sender, EventArgs e) =>
            //{
            //    xppFoldingStrategy.UpdateFoldings(xppFoldingManager, this.Document);
            //};
        }
    }
}
