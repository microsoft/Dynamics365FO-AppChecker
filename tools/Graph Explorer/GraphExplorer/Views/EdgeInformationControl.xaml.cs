using Neo4j.Driver;
using SocratexGraphExplorer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SocratexGraphExplorer.Views
{
    /// <summary>
    /// Interaction logic for EdgeInformationControl.xaml
    /// </summary>
    public partial class EdgeInformationControl : UserControl
    {
        private ObservableCollection<PropertyItem> properties { get; set; }
        private Model Model { get; set; }
        private IRelationship Edge { get; set; }

        public ObservableCollection<PropertyItem> Properties
        {
            get { return this.properties; }
        }

        public EdgeInformationControl(Model model,  IRelationship edge)
        {
            InitializeComponent();

            this.properties = new ObservableCollection<PropertyItem>();
            this.Model = model;
            this.Edge = edge;
            this.DataContext = this;

            this.Header.Text = string.Format("Edge {0}", edge.Type);

            this.Properties.Add(new PropertyItem() { Key = "Id", Value = edge.Id.ToString() });
            this.Properties.Add(new PropertyItem() { Key = "Start Node", Value = edge.StartNodeId.ToString() });
            this.Properties.Add(new PropertyItem() { Key = "End Node", Value = edge.EndNodeId.ToString() });

            foreach (var property in edge.Properties)
            {
                this.Properties.Add(new PropertyItem() { Key = property.Key, Value = property.Value.ToString() });
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

    }
}
