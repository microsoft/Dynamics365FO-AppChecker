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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SocratexGraphExplorer.Views
{
    using SocratexGraphExplorer.ViewModels;
using System.ComponentModel.Composition.Primitives;

/// <summary>
/// Interaction logic for NodeConfigurationControl.xaml
/// </summary>
    public partial class NodeConfigurationControl : UserControl
    {
        public NodeConfigurationViewModel ViewModel { get; private set; }
        public static PathFigure PathFigure { get; private set; }

        public NodeConfigurationControl()
        {
            this.ViewModel = new NodeConfigurationViewModel(this);

            this.InitializeComponent();
            this.DataContext = this.ViewModel;

            //EllipseGeometry myEllipseGeometry = new EllipseGeometry();
            //myEllipseGeometry.Center = new System.Windows.Point(25, 25);
            //myEllipseGeometry.RadiusX = 23;
            //myEllipseGeometry.RadiusY = 23;
            //var pathGeometry = PathGeometry.CreateFromGeometry(myEllipseGeometry);

            //this.ViewModel.Init();
            var star = DrawStar(30, 25, 25);
        }

  
        private static string DrawStar(double r, double x, double y)
        {
            var pathGeometry = new PathGeometry();

            // the change in radius and the offset is here to center the shape
            r *= 0.82;
            y += 0.1 * r;

            Point startPoint = new Point() { X = x, Y = y };

            for (int n = 0; n < 10; n++)
            {
                double radius = n % 2 == 0 ? r * 1.3 : r * 0.5;

                var endPoint = new Point() { 
                    X = startPoint.X + radius * Math.Sin((n * 2 * Math.PI) / 10), 
                    Y= startPoint.Y - radius * Math.Cos((n * 2 * Math.PI) / 10)
                };

                var l = new LineGeometry(startPoint, endPoint);
                startPoint = endPoint;
                pathGeometry.AddGeometry(l);
            }

            return pathGeometry.ToString();
        }

    }
}
