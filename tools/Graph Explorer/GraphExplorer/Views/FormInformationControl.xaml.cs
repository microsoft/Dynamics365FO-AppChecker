using Neo4j.Driver;
using SocratexGraphExplorer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SocratexGraphExplorer.Views
{
    /// <summary>
    /// Interaction logic for ClassInformationControl.xaml
    /// </summary>
    public partial class FormInformationControl : UserControl
    {
        private readonly Model model;
        private readonly INode node;
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

        public FormInformationControl(Model model, INode node)
        {
            this.model = model;
            this.node = node;

            InitializeComponent();

            this.DataContext = this;
            this.ClassEditor = new XppSourceEditor(model);
            this.SourceEditorBox.Content = this.ClassEditor;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.Header.Text = string.Format("{0} {1}", node.Labels[0], node.Properties["Name"] as string);
            properties.Add(new PropertyItem() { Key = "Id", Value = node.Id.ToString() });
            properties.Add(new PropertyItem() { Key = "Package", Value = (node.Properties["Package"].ToString()) });
            properties.Add(new PropertyItem() { Key = "Name", Value = (node.Properties["Name"].ToString()) });

            properties.Add(new PropertyItem() { Key = "Lines of Code", Value = (node.Properties["LOC"].ToString()) });
            properties.Add(new PropertyItem() { Key = "Weighted Method Count", Value = (node.Properties["WMC"].ToString()) });
            properties.Add(new PropertyItem() { Key = "Private methods", Value = (node.Properties["NOPM"].ToString()) });
            properties.Add(new PropertyItem() { Key = "Methods", Value = (node.Properties["NOM"].ToString()) });
            properties.Add(new PropertyItem() { Key = "Statements", Value = (node.Properties["NOS"].ToString()) });

            var base64Source = node.Properties["base64Source"] as string;
            var sourceArray = Convert.FromBase64String(base64Source);
            var source = Encoding.ASCII.GetString(sourceArray);
            this.ClassEditor.Text = source;
        }

        private async void AddNodes(string query, IDictionary<string, object> parameters)
        {
            var queryResult = await model.ExecuteCypherAsync(query, parameters);
            var result = Model.HarvestNodeIdsFromGraph(queryResult);

            if (result != null && result.Any())
            {
                var nodes = this.model.NodesShown;
                nodes.UnionWith(result);
                this.model.NodesShown = nodes;
            }
        }

        private void ShowMethods(object sender, RoutedEventArgs e)
        {
            AddNodes(
                "match (c:Form) -[:DECLARES]-> (m:Method) where id(c) = {nodeId} return m", 
                new Dictionary<string, object>() { { "nodeId", node.Id} });
        }

        private void ShowControls(object sender, RoutedEventArgs e)
        {
            AddNodes(
                "match (c:Form) -[:CONTROL]-> (fc:FormControl) where id(c) = {nodeId} return fc",
                new Dictionary<string, object>() { { "nodeId", node.Id } });
        }

        private void ShowDatasources(object sender, RoutedEventArgs e)
        {
            AddNodes(
                "match (c:Form) -[:DATASOURCE]-> (fds:FormDataSource) where id(c) = {nodeId} return fds",
                new Dictionary<string, object>() { { "nodeId", node.Id } });
        }
    }
}
