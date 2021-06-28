using Neo4j.Driver;
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
    /// Interaction logic for ClassInformationControl.xaml
    /// </summary>
    public partial class ClassNodeRenderer : UserControl, INodeRenderer
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

        public ClassNodeRenderer(IModel model)
        {
            this.model = model;

            InitializeComponent();

            this.DataContext = this;
            this.ClassEditor = new XppSourceEditor();
            this.SourceEditorBox.Content = this.ClassEditor;
        }

        public async void SelectNodeAsync(INode node)
        {
            this.node = node;

            var extendsQuery = "match (c:Class) -[:EXTENDS]-> (q) where id(c) = $nodeId return q";
            var extendsQueryPromise = model.ExecuteCypherAsync(extendsQuery, new Dictionary<string, object>() { { "nodeId", node.Id } });

            var extendedByQuery = "match (c:Class) <-[:EXTENDS]- (q) where id(c) = $nodeId return count(q) as cnt";
            var extendedByQueryPromise = model.ExecuteCypherAsync(extendedByQuery, new Dictionary<string, object>() { { "nodeId", node.Id } });

            var implementsCountQuery = "match (c:Class) -[:IMPLEMENTS]-> (i) where id(c)=$nodeId return count(i) as cnt";
            var implementsCountQueryPromise = model.ExecuteCypherAsync(implementsCountQuery, new Dictionary<string, object>() { { "nodeId", node.Id } });

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
            await this.model.AddNodesAsync("match (c:Class) -[:EXTENDS]-> (q) where id(c) = $nodeId return q limit 1", new Dictionary<string, object>() { { "nodeId", node.Id } });
        }

        private async void ShowBaseClasses(object sender, RoutedEventArgs e)
        {
            await this.model.AddNodesAsync("match (c:Class) -[:EXTENDS*]-> (q) where id(c) = $nodeId return q", new Dictionary<string, object>() { { "nodeId", node.Id } });
        }

        private async void ShowDerivedClasses(object sender, RoutedEventArgs e)
        {
            await this.model.AddNodesAsync("match (c:Class) <-[:EXTENDS]- (q) where id(c) = $nodeId return q", new Dictionary<string, object>() { { "nodeId", node.Id } });
        }

        private async void ShowImplementedInterfaces(object sender, RoutedEventArgs e)
        {
            await this.model.AddNodesAsync("match (c:Class) -[:IMPLEMENTS]-> (i) where id(c)=$nodeId return i", new Dictionary<string, object>() { { "nodeId", node.Id } });
        }

        private async void ShowMethods(object sender, RoutedEventArgs e)
        {
            await this.model.AddNodesAsync("match (c:Class) -[:DECLARES]-> (m:Method) where id(c) = $nodeId return m", new Dictionary<string, object>() { { "nodeId", node.Id } });
        }

        private async void ShowFields(object sender, RoutedEventArgs e)
        {
            await this.model.AddNodesAsync("match (c:Class) -[:DECLARES]-> (m:ClassMember) where id(c) = $nodeId return m", new Dictionary<string, object>() { { "nodeId", node.Id } });
        }
    }
}
