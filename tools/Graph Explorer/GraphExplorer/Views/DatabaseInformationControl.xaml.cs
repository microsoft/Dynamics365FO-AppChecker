using SocratexGraphExplorer.Models;
using SocratexGraphExplorer.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SocratexGraphExplorer.Views
{
    /// <summary>
    /// Interaction logic for DatabaseInformationControl.xaml
    /// </summary>
    public partial class DatabaseInformationControl : UserControl
    {
        private readonly Model Model;
        private readonly EditorViewModel ViewModel;

        public DatabaseInformationControl(Model model, EditorViewModel viewModel)
        {
            this.Model = model;
            this.ViewModel = viewModel;

            InitializeComponent();

            this.DataContext = this;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var metadataQuery = @"
CALL db.labels() YIELD label
RETURN {name:'labels', data:COLLECT(label)[..1000]} AS result
UNION ALL
CALL db.relationshipTypes() YIELD relationshipType
RETURN {name:'relationshipTypes', data:COLLECT(relationshipType)[..1000]} AS result
UNION ALL
MATCH () RETURN { name:'nodes', data:count(*) } AS result
UNION ALL
MATCH ()-[]->() RETURN { name:'relationships', data: count(*)} AS result";

            var result = await Model.ExecuteCypherAsync(metadataQuery);
            var labels = ((result[0].Values["result"] as Dictionary<string, object>)["data"] as List<object>);
            var labelsCount = (long)(result[2].Values["result"] as IDictionary<string, object>)["data"];

            this.LabelsPrompt.Text = string.Format("Node Labels ({0})", labelsCount);

            foreach (var o in labels)
            {
                var chip = new MaterialDesignThemes.Wpf.Chip()
                {
                    Margin = new Thickness(5,0,5,0),
                    Content = o as string,
                    IsDeletable = false,
                    ToolTip = "Calculating...",
                };

                chip.ToolTipOpening += LabelTooltipOpening;
                this.Nodes.Children.Add(chip);
            }

            var relationships = ((result[1].Values["result"] as Dictionary<string, object>)["data"] as List<object>);
            var relationshipCount = (long)(result[3].Values["result"] as IDictionary<string, object>)["data"];

            this.RelationshipsPrompt.Text = string.Format("Relationships ({0})", relationshipCount);

            foreach (var o in relationships)
            {
                var border = new Border()
                {
                    Margin = new Thickness(5, 0, 5, 0),
                    Child = new Label() { Content = "-[" + (o as string) + "]-"},
                    ToolTip = "Calculating...",
                };

                border.ToolTipOpening += EdgeToolTipOpening;
                this.Relationships.Children.Add(border);
            }
        }

        private async void EdgeToolTipOpening(object sender, ToolTipEventArgs e)
        {
            var container = sender as Border;
            var label = container.Child as Label;
            var relationshipGlyph = label.Content as string;
            var relationship = relationshipGlyph.Replace("-[", "");
            relationship = relationship.Replace("]-", "");

            var query = string.Format("match ()-[c:{0}]-() return count(c) as Count", relationship);
            var result = await Model.ExecuteCypherAsync(query);

            container.ToolTip = string.Format("Database contains {0} instances", result[0].Values["Count"]);
        }

        private async void LabelTooltipOpening(object sender, ToolTipEventArgs e)
        {
            var chip = sender as MaterialDesignThemes.Wpf.Chip;
            var label = chip.Content as string;

            var query = string.Format("match(c:{0}) return count(c) as Count", label);
            var result = await Model.ExecuteCypherAsync(query);

            chip.ToolTip = string.Format("Database contains {0} instances", result[0].Values["Count"]);
        }
    }
}
