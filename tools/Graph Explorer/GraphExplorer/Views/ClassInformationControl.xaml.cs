using ICSharpCode.AvalonEdit.Highlighting;
using Neo4j.Driver;
using SocratexGraphExplorer.Models;
using SocratexGraphExplorer.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace SocratexGraphExplorer.Views
{
    /// <summary>
    /// Interaction logic for ClassInformationControl.xaml
    /// </summary>
    public partial class ClassInformationControl : UserControl
    {
        private Model model;
        private EditorViewModel viewModel;
        private INode node;
        private SourceEditor ClassEditor { set; get; }

        private ObservableCollection<PropertyItem> properties = new ObservableCollection<PropertyItem>();

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

        public ClassInformationControl(Model model, EditorViewModel viewModel, INode node)
        {
            this.model = model;
            this.viewModel = viewModel;
            this.node = node;

            InitializeComponent();

            this.DataContext = this;
            this.ClassEditor = new XppSourceEditor(model);
            this.SourceEditorBox.Content = this.ClassEditor;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var extendsQuery = string.Format("match (c:Class) -[:EXTENDS]-> (q) where id(c) = {0} return q", node.Id);
            var extendsQueryPromise = model.ExecuteCypherAsync(extendsQuery);

            var extendedByQuery = string.Format("match (c:Class) <-[:EXTENDS]- (q) where id(c) = {0} return count(q) as cnt", node.Id);
            var extendedByQueryPromise = model.ExecuteCypherAsync(extendedByQuery);

            var implementsCountQuery = string.Format("match (c:Class) -[:IMPLEMENTS]-> (i) where id(c)={0} return count(i) as cnt", node.Id);
            var implementsCountQueryPromise = model.ExecuteCypherAsync(implementsCountQuery);

            this.Header.Text = string.Format("{0} {1}", node.Labels[0], node.Properties["Name"] as string);
            properties.Add(new PropertyItem() { Key = "Id", Value = node.Id.ToString() });
            properties.Add(new PropertyItem() { Key = "Package", Value = (node.Properties["Package"].ToString()) });
            properties.Add(new PropertyItem() { Key = "Name", Value = (node.Properties["Name"].ToString()) });

            var extendsName = "Object";
            var extendsQueryResult = await extendsQueryPromise;
            if (extendsQueryResult != null && extendsQueryResult.Any())
            {
                extendsName = (extendsQueryResult[0].Values["q"] as INode).Properties["Name"].ToString();
            }
            properties.Add(new PropertyItem() { Key = "Extends", Value = extendsName });

            var extendedByQueryResult = await extendedByQueryPromise;
            if (extendedByQueryResult != null)
            {
                properties.Add(new PropertyItem() { Key = "Extended by", Value = extendedByQueryResult[0].Values["cnt"].ToString() });
            }

            var implementsCountQueryResult = await implementsCountQueryPromise;
            if (implementsCountQueryResult != null)
            {
                properties.Add(new PropertyItem() { Key = "Implements", Value = implementsCountQueryResult[0].Values["cnt"].ToString() });
            }

            properties.Add(new PropertyItem() { Key = "Lines of Code", Value = (node.Properties["LOC"].ToString()) });
            properties.Add(new PropertyItem() { Key = "Weighted Method Count", Value = (node.Properties["WMC"].ToString()) });
            properties.Add(new PropertyItem() { Key = "Abstract methods", Value = (node.Properties["NOAM"].ToString()) });
            properties.Add(new PropertyItem() { Key = "Fields", Value = (node.Properties["NOA"].ToString()) });
            properties.Add(new PropertyItem() { Key = "Methods", Value = (node.Properties["NOM"].ToString()) });
            properties.Add(new PropertyItem() { Key = "Statements", Value = (node.Properties["NOS"].ToString()) });
            properties.Add(new PropertyItem() { Key = "Final", Value = (node.Properties["IsFinal"].ToString()) });
            properties.Add(new PropertyItem() { Key = "Abstract", Value = (node.Properties["IsAbstract"].ToString()) });
            properties.Add(new PropertyItem() { Key = "Static", Value = (node.Properties["IsStatic"].ToString()) });

            var base64Source = node.Properties["base64Source"] as string;
            var sourceArray = Convert.FromBase64String(base64Source);
            var source = Encoding.ASCII.GetString(sourceArray);
            this.ClassEditor.Text = source;

        }

        private async void ShowBaseClass(object sender, RoutedEventArgs e)
        {
            var extendsQuery = string.Format("match (c:Class) -[:EXTENDS]-> (q) where id(c) = {0} return q limit 1", node.Id);
            var extendsQueryResult = await model.ExecuteCypherAsync(extendsQuery);
            var result = Model.HarvestNodeIdsFromGraph(extendsQueryResult);

            if (result != null)
            {
                var nodes = this.model.NodesShown;
                nodes.UnionWith(result);
                this.model.NodesShown = nodes;
            }
        }

        private async void ShowBaseClasses(object sender, RoutedEventArgs e)
        {
            var extendsQuery = string.Format("match (c:Class) -[:EXTENDS*]-> (q) where id(c) = {0} return q", node.Id);
            var extendsQueryResult = await model.ExecuteCypherAsync(extendsQuery);
            var result = Model.HarvestNodeIdsFromGraph(extendsQueryResult);

            if (result != null && result.Any())
            {
                var nodes = this.model.NodesShown;
                nodes.UnionWith(result);
                this.model.NodesShown = nodes;
            }
        }

        private async void ShowDerivedClasses(object sender, RoutedEventArgs e)
        {
            var extendsQuery = string.Format("match (c:Class) <-[:EXTENDS]- (q) where id(c) = {0} return q", node.Id);
            var extendsQueryResult = await model.ExecuteCypherAsync(extendsQuery);
            var result = Model.HarvestNodeIdsFromGraph(extendsQueryResult);

            if (result != null)
            {
                var nodes = this.model.NodesShown;
                nodes.UnionWith(result);
                this.model.NodesShown = nodes;
            }
        }

        private async void ShowImplementedInterfaces(object sender, RoutedEventArgs e)
        {
            var implementsCountQuery = string.Format("match (c:Class) -[:IMPLEMENTS]-> (i) where id(c)={0} return i", node.Id);
            var implementsCountQueryResult = await model.ExecuteCypherAsync(implementsCountQuery);
            var result = Model.HarvestNodeIdsFromGraph(implementsCountQueryResult);

            if (result != null)
            {
                var nodes = this.model.NodesShown;
                nodes.UnionWith(result);
                this.model.NodesShown = nodes;
            }
        }

        private async void ShowMethods(object sender, RoutedEventArgs e)
        {
            var extendsQuery = string.Format("match (c:Class) -[:DECLARES]-> (m:Method) where id(c) = {0} return m", node.Id);
            var extendsQueryResult = await model.ExecuteCypherAsync(extendsQuery);
            var result = Model.HarvestNodeIdsFromGraph(extendsQueryResult);

            if (result != null && result.Any())
            {
                var nodes = this.model.NodesShown;
                nodes.UnionWith(result);
                this.model.NodesShown = nodes;
            }
        }

        private async void ShowFields(object sender, RoutedEventArgs e)
        {
            var extendsQuery = string.Format("match (c:Class) -[:DECLARES]-> (m:ClassMember) where id(c) = {0} return m", node.Id);
            var extendsQueryResult = await model.ExecuteCypherAsync(extendsQuery);
            var result = Model.HarvestNodeIdsFromGraph(extendsQueryResult);

            if (result != null && result.Any())
            {
                var nodes = this.model.NodesShown;
                nodes.UnionWith(result);
                this.model.NodesShown = nodes;
            }
        }

    }
}
