﻿using Neo4j.Driver;
using SocratexGraphExplorer.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SocratexGraphExplorer.XppPlugin
{
    /// <summary>
    /// Interaction logic for EmptyInformationControl.xaml
    /// </summary>
    public partial class MethodNodeRenderer : UserControl, INodeRenderer
    {
        private readonly IModel model;
        private INode node;

        private SourceEditor MethodEditor { set; get; }
        private readonly ObservableCollection<PropertyItem> properties = new ObservableCollection<PropertyItem>();

        public ObservableCollection<PropertyItem> Properties
        {
            get { return this.properties; }
        }

        public MethodNodeRenderer(IModel model)
        {
            this.model = model;

            InitializeComponent();

            this.DataContext = this;
            this.MethodEditor = new XppSourceEditor();
            this.SourceEditorBox.Content = this.MethodEditor;
        }

        public async void SelectNodeAsync(INode node)
        {
            this.node = node;

            var incomingCallsQuery = model.ExecuteCypherAsync(
                 "match(m1:Method) -[r: CALLS]->(m:Method) where id(m) = $nodeId return count(r) as methods, sum(r.Count) as count",
                 new Dictionary<string, object>() { { "nodeId", node.Id } });

            var outGoingCallsQuery = model.ExecuteCypherAsync(
                "match(m:Method) -[r:CALLS]->(m1:Method) where id(m) = $nodeId return count(r) as methods, sum(r.Count) as count",
                new Dictionary<string, object>() { { "nodeId", node.Id } });

            this.Header.Text = string.Format("{0} {1}", node.Labels[0], node.Properties["Name"] as string);
            properties.Add(new PropertyItem() { Key = "Id", Value = node.Id.ToString() });
            properties.Add(new PropertyItem() { Key = "Package", Value = node.Properties["Package"].ToString() });
            properties.Add(new PropertyItem() { Key = "Artifact", Value = node.Properties["Artifact"].ToString() });
            properties.Add(new PropertyItem() { Key = "Lines of Code", Value = node.Properties["LOC"].ToString() });
            properties.Add(new PropertyItem() { Key = "Complexity", Value = node.Properties["CMP"].ToString() });
            properties.Add(new PropertyItem() { Key = "Visibility", Value = node.Properties["Visibility"].ToString() });
            properties.Add(new PropertyItem() { Key = "Final", Value = node.Properties["IsFinal"].ToString() });
            properties.Add(new PropertyItem() { Key = "Abstract", Value = node.Properties["IsAbstract"].ToString() });
            properties.Add(new PropertyItem() { Key = "Static", Value = node.Properties["IsStatic"].ToString() });

            var artifact = node.Properties["Artifact"].ToString();

            var parts = artifact.Split('/');
            var toplevelArtifact = "/" + parts[1] + "/" + parts[2] + "/" + parts[3];

            var query = "match (p) where p.Artifact=$artifact return p limit 1";
            var c = await model.ExecuteCypherAsync(query, new Dictionary<string, object>() { { "artifact", toplevelArtifact } });

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

            var incomingCallsResult = await incomingCallsQuery;
            if (incomingCallsResult != null)
            {
                properties.Add(new PropertyItem() { Key = "Incoming calls", Value = incomingCallsResult[0].Values["count"].ToString() });
                properties.Add(new PropertyItem() { Key = "Calling methods", Value = incomingCallsResult[0].Values["methods"].ToString() });
            }

            var outgoingCallsResult = await outGoingCallsQuery;
            if (outgoingCallsResult != null)
            {
                properties.Add(new PropertyItem() { Key = "Outgoing calls", Value = outgoingCallsResult[0].Values["count"].ToString() });
                properties.Add(new PropertyItem() { Key = "Called methods", Value = outgoingCallsResult[0].Values["methods"].ToString() });
            }

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

        private async void ShowDeclaringEntityClicked(object sender, RoutedEventArgs e)
        {
            await this.model.AddNodesAsync("match p=(c) -[*]-> (n:Method) where id(n) = $nodeId return p order by length(p) desc limit 1", new Dictionary<string, object>() { { "nodeId", node.Id } });
        }

        private async void ShowCallersButtonClicked(object sender, RoutedEventArgs e)
        {
            await this.model.AddNodesAsync("match (c) -[:CALLS]-> (n) where id(n) = $nodeId return c", new Dictionary<string, object>() { { "nodeId", node.Id } });
        }

        private async void ShowCalleesButtonClicked(object sender, RoutedEventArgs e)
        {
            await this.model.AddNodesAsync("match (c) <-[:CALLS]- (n) where id(n) = $nodeId return c", new Dictionary<string, object>() { { "nodeId", node.Id } });
        }
    }
}
