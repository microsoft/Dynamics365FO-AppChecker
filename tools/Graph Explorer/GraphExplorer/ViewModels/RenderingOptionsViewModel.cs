using MaterialDesignColors;
using MaterialDesignColors.Recommended;
using Neo4j.Driver;
using GraphExplorer.Models;
using GraphExplorer.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GraphExplorer.ViewModels
{
    internal class RenderingOptionsViewModel : ViewModelBase
    {
        private RenderingOptionsModel model = new RenderingOptionsModel();
        private RenderingOptions View { get; set; }

        public Color SelectedColor { get; set; }

        public RenderingOptionsViewModel(RenderingOptions view)
        {
            this.View = view;

            // Set the nodes tab as selected by default
            //this.View.NodesButton.IsChecked = true;

            // Handler for events in this view model
            this.PropertyChanged += (object sender, PropertyChangedEventArgs e) =>
            {
            };
        }

        public Brush MyBrush {
            get { return Brushes.Orange; }
        }

        public IEnumerable<string> ColorNames { get; } = new[]
{
            // "White", "Gray", "Black",
            "IndianRed", "Red", "DarkRed",
            "LightGreen", "Green", "DarkGreen",
            "LightBlue", "Blue", "DarkBlue",
            "LightYellow", "Yellow", "YellowGreen",
            "OrangeRed", "Orange", "DarkOrange",
        };

        public IEnumerable<string> NodeLabels
        {
            get
            {
                return model.GetNodeLabels();
            }
        }

        public IEnumerable<string> EdgeLabels
        {
            get
            {
                return model.GetEdgeLabels();
            }
        }

        public IEnumerable<string> GeneralOptions
        {
            get { return new[] { "Background", "Font" };  }
        }
    }
}
