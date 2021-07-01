﻿using Neo4j.Driver;
using SocratexGraphExplorer.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace SocratexGraphExplorer.XppPlugin
{
    /// <summary>
    /// Interaction logic for ClassInformationControl.xaml
    /// </summary>
    public partial class TableNodeRenderer : UserControl, INodeRenderer
    {
        private readonly IModel model;
        private INode node;

        private SourceEditor ClassEditor { set; get; }

        private readonly ObservableCollection<PropertyItem> properties = new ObservableCollection<PropertyItem>();

        public ObservableCollection<PropertyItem> Properties
        {
            get { return this.properties; }
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

        public TableNodeRenderer(IModel model)
        {
            this.model = model;

            InitializeComponent();

            this.DataContext = this;
            this.ClassEditor = new XppSourceEditor();
            this.SourceEditorBox.Content = this.ClassEditor;
        }

        public void SelectNodeAsync(INode node)
        {
            this.node = node;

            this.Header.Text = string.Format("{0} {1}", node.Labels[0], node.Properties["Name"] as string);
            properties.Add(new PropertyItem() { Key = "Id", Value = node.Id.ToString() });
            properties.Add(new PropertyItem() { Key = "Package", Value = (node.Properties["Package"].ToString()) });
            properties.Add(new PropertyItem() { Key = "Name", Value = (node.Properties["Name"].ToString()) });

            properties.Add(new PropertyItem() { Key = "Lines of Code", Value = (node.Properties["LOC"].ToString()) });
            properties.Add(new PropertyItem() { Key = "Weighted Method Count", Value = (node.Properties["WMC"].ToString()) });
            properties.Add(new PropertyItem() { Key = "Fields", Value = (node.Properties["NOA"].ToString()) });
            properties.Add(new PropertyItem() { Key = "Methods", Value = (node.Properties["NOM"].ToString()) });
            properties.Add(new PropertyItem() { Key = "Statements", Value = (node.Properties["NOS"].ToString()) });

            var base64Source = node.Properties["base64Source"] as string;
            var sourceArray = Convert.FromBase64String(base64Source);
            var source = Encoding.ASCII.GetString(sourceArray);
            this.ClassEditor.Text = source;
        }

        private async void ShowMethods(object sender, RoutedEventArgs e)
        {
            await this.model.AddNodesAsync("match (c:Table) -[:DECLARES]-> (m:Method) where id(c) = $nodeId return m", new Dictionary<string, object>() { { "nodeId", node.Id } });
        }

        private async void ShowFields(object sender, RoutedEventArgs e)
        {
            await this.model.AddNodesAsync("match (c:Class) -[:DECLARES]-> (m:ClassMember) where id(c) = $nodeId return m", new Dictionary<string, object>() { { "nodeId", node.Id } });
        }
    }
}
