using ICSharpCode.AvalonEdit.Highlighting;
using Neo4j.Driver;
using SocratexGraphExplorer.Models;
using SocratexGraphExplorer.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace SocratexGraphExplorer.Views
{
    /// <summary>
    /// Interaction logic for EmptyInformationControl.xaml
    /// </summary>
    public partial class MethodInformationControl : UserControl
    {
        private Model model;
        private EditorViewModel viewModel;
        private INode node;
        private SourceEditor MethodEditor { set; get; }
        private ObservableCollection<PropertyItem> properties = new ObservableCollection<PropertyItem>();

        public ObservableCollection<PropertyItem> Properties
        {
            get { return this.properties; }
        }
        
        public MethodInformationControl(Model model, EditorViewModel viewModel, INode node)
        {
            this.model = model;
            this.viewModel = viewModel;
            this.node = node;

            InitializeComponent();

            this.DataContext = this;
            this.MethodEditor = new XppSourceEditor(model);
            this.SourceEditorBox.Content = this.MethodEditor;

            //this.MethodEditor.SyntaxHighlighting = LoadHighlightDefinition("Xpp-Mode.xshd");

            this.Header.Text = string.Format("{0} {1}{2}{3}", node.Labels[0], node.Properties["Class"] as string, (bool)node.Properties["IsStatic"] ? "::" : ".", node.Properties["Method"] as string);
            properties.Add(new PropertyItem() { Key = "Id", Value = node.Id.ToString() });
            properties.Add(new PropertyItem() { Key = "Package", Value = (node.Properties["Package"].ToString()) });
            properties.Add(new PropertyItem() { Key = "Class", Value = (node.Properties["Class"].ToString()) });
            properties.Add(new PropertyItem() { Key = "Lines of Code", Value = (node.Properties["LOC"].ToString()) });
            properties.Add(new PropertyItem() { Key = "Complexity", Value = (node.Properties["CMP"].ToString()) });
            properties.Add(new PropertyItem() { Key = "Visibility", Value = (node.Properties["Visibility"].ToString()) });
            properties.Add(new PropertyItem() { Key = "Final", Value = (node.Properties["IsFinal"].ToString()) });
            properties.Add(new PropertyItem() { Key = "Abstract", Value = (node.Properties["IsAbstract"].ToString()) });
            properties.Add(new PropertyItem() { Key = "Static", Value = (node.Properties["IsStatic"].ToString()) });
        }

        /// <summary>
        /// Called when the user releases the right mouse in the property grid.
        /// </summary>
        /// <param name="sender">The control that is selected</param>
        /// <param name="args">The arguments. Not used.</param>
        public void OnMouseRightButtonUp(object sender, EventArgs args)
        {
            TextBlock control = sender as TextBlock;
            Clipboard.SetText(control.Text);
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var query = string.Format("match (c) -[:DECLARES]->(m:Method) where id(m)=  {0} return c", node.Id);
            var c = await model.ExecuteCypherAsync(query);

            if (c != null)
            {
                var declaringNode = c.First().Values.Values.First() as INode;
                var base64Source = declaringNode.Properties["base64Source"] as string;
                var sourceArray = Convert.FromBase64String(base64Source);
                var source = Encoding.ASCII.GetString(sourceArray);
                this.MethodEditor.Text = source;

                var startLine = (long)node.Properties["StartLine"];
                var endLine = (long)node.Properties["EndLine"];
                var startCol = (long)node.Properties["StartCol"];
                var endCol = (long)node.Properties["EndCol"];

                var startOffset = this.MethodEditor.Document.GetOffset((int)startLine, (int)startCol);
                var endOffset = this.MethodEditor.Document.GetOffset((int)endLine, (int)endCol);

                this.MethodEditor.TextArea.Caret.Position = new ICSharpCode.AvalonEdit.TextViewPosition((int)startLine, (int)startCol);

                var selection = ICSharpCode.AvalonEdit.Editing.Selection.Create(this.MethodEditor.TextArea, startOffset, endOffset);
                this.MethodEditor.TextArea.Selection = selection;

                this.MethodEditor.TextArea.Caret.BringCaretToView();
            }

            var incomingCallsQuery = model.ExecuteCypherAsync(
                string.Format("match(m1:Method) -[r: CALLS]->(m:Method) where id(m) = {0} return count(r) as methods, sum(r.Count) as count", node.Id));
            var outGoingCallsQuery = model.ExecuteCypherAsync(
                string.Format("match(m:Method) -[r:CALLS]->(m1:Method) where id(m) = {0} return count(r) as methods, sum(r.Count) as count", node.Id));

            var incomingCallsResult = await incomingCallsQuery;
            if (incomingCallsResult != null)
            {
                properties.Add(new PropertyItem() { Key = "Incoming calls", Value = incomingCallsResult[0].Values["count"].ToString() });
                properties.Add(new PropertyItem() { Key = "Calling methods", Value = incomingCallsResult[0].Values["methods"].ToString() });
            }

            var outgoingCallsResult = await outGoingCallsQuery;
            if (incomingCallsResult != null)
            {
                properties.Add(new PropertyItem() { Key = "Outgoing calls", Value = outgoingCallsResult[0].Values["count"].ToString() });
                properties.Add(new PropertyItem() { Key = "Called methods", Value = outgoingCallsResult[0].Values["methods"].ToString() });
            }

        }

        private async void ShowDeclaringEntityClicked(object sender, RoutedEventArgs e)
        {
            var containingEntityQuery = string.Format("match (c) -[:DECLARES]-> (n) where id(n) = {0} return c", node.Id.ToString());
            var containingQueryResult = await model.ExecuteCypherAsync(containingEntityQuery);

            var entityNode = containingQueryResult[0].Values["c"] as INode;
            if (entityNode != null)
            {
                var entityId = entityNode.Id;

                var nodes = this.model.NodesShown;
                nodes.Add(entityId);
                this.model.NodesShown = nodes;
            }
        }

        private async void ShowCallersButtonClicked(object sender, RoutedEventArgs e)
        {
            var callersQuery = string.Format("match (c) -[:CALLS]-> (n) where id(n) = {0} return c", node.Id.ToString());
            var callersQueryResult = await model.ExecuteCypherAsync(callersQuery);
            var result = Model.HarvestNodeIdsFromGraph(callersQueryResult);

            if (result != null && result.Any())
            {
                var nodes = this.model.NodesShown;
                nodes.UnionWith(result);
                this.model.NodesShown = nodes;
            }
        }

        private async void ShowCalleesButtonClicked(object sender, RoutedEventArgs e)
        {
            var calleesQuery = string.Format("match (c) <-[:CALLS]- (n) where id(n) = {0} return c", node.Id.ToString());
            var calleesQueryResult = await model.ExecuteCypherAsync(calleesQuery);
            var result = Model.HarvestNodeIdsFromGraph(calleesQueryResult);

            var nodes = this.model.NodesShown;
            nodes.UnionWith(result);
            this.model.NodesShown = nodes;
        }
    }   
}
