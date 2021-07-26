using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using ICSharpCode.AvalonEdit.Search;
using GraphExplorer.Models;
using GraphExplorer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Xml;

namespace GraphExplorer.Views
{
    class JavaScriptEditor : SourceEditor
    {
        public JavaScriptEditor() : base()
        {
            // FontFamily = "{Binding Source={x:Static settings:Settings.Default}, Path=SourceFont}"
            // var fontFamilyBinding = new Binding("SourceFont");
            //fontFamilyBinding.Source = Properties.Settings.Default;
            //this.SetBinding(FontFamilyProperty, fontFamilyBinding);

            // Configure the X++ folding manager. 
            // The indentation strategy is probably not needed since the view is readonly...
            this.TextArea.IndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.CSharp.CSharpIndentationStrategy(this.Options);
            var foldingStrategy = new JavaScriptBraceFoldingStrategy();
            var foldingManager = FoldingManager.Install(this.TextArea);
            foldingStrategy.UpdateFoldings(foldingManager, this.Document);

            //using (var stream = Assembly.GetAssembly(typeof(ICSharpCode.AvalonEdit.TextEditor)).GetManifestResourceStream("ICSharpCode.AvalonEdit.Highlighting.Resources.JavaScript-Mode.xshd"))
            //{
            //    using (var reader = new XmlTextReader(stream))
            //    {
            //        this.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
            //        SearchPanel.Install(this);
            //    }
            //}

            this.SyntaxHighlighting = LoadHighlightDefinition("ICSharpCode.AvalonEdit.Highlighting.Resources.JavaScript-Mode.xshd", Assembly.GetAssembly(typeof(ICSharpCode.AvalonEdit.TextEditor)));
        }
    }
}
