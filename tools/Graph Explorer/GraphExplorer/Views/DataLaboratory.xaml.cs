using MaterialDesignExtensions.Controls;
using GraphExplorer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using unvell.ReoGrid;
using Neo4j.Driver;
using System.Linq;
using unvell.ReoGrid.IO.OpenXML.Schema;
using GraphExplorer.ViewModels;
using GraphExplorer.Core.netcore;

namespace GraphExplorer.Views
{
    /// <summary>
    /// Interaction logic for DataLab.xaml
    /// </summary>
    public partial class DataLaboratory : MaterialWindow
    {
        private EditorViewModel ViewModel {get; set; }

        Graph graph;
        private Graph Graph
        {
            get => this.graph;
            set
            {
                this.graph = value;
                this.Populate(this.graph);
            }
        }

        public static DataLaboratory CreateDataLaboratory(EditorViewModel viewModel)
        {
            var res = new DataLaboratory(viewModel);
            res.DataContext = viewModel;

            res.Initialize();
            res.Populate(viewModel.Graph);
            return res;
        }

        private void PopulateNode(IEnumerable<Node> nodes, string label)
        {
            IDictionary<string, int> propertyColumns = new Dictionary<string, int>() { { "Id", 0 }, { "Label", 1 } };

            // Create the worksheet
            unvell.ReoGrid.Worksheet worksheet = this.Nodes.Worksheets.Create(label);
            worksheet.ColumnHeaders[0].Text = "Id";
            worksheet.ColumnHeaders[1].Text = "Label";
            worksheet.FreezeToCell(0, 2, FreezeArea.Left);

            // The Id field is not editable
            worksheet.BeforeCellEdit += (s, e) => e.IsCancelled = e.Cell.Column == 0;

            // Add to the collection of worksheets.
            this.Nodes.Worksheets.Add(worksheet);

            int row = 0;
            int maxcol = 2;

            // Populate each row in the worksheet with the contributions from the
            // predefined fields and the properties.
            foreach (var node in nodes)
            {
                worksheet.SetCellData(new CellPosition(row, 0), node.Id);
                worksheet.SetCellData(new CellPosition(row, 1), label);

                foreach (var property in node.Properties)
                {
                    var propertyName = property.Key;
                    var propertyValue = property.Value;

                    // If we have already seen the property, place it there.
                    if (!propertyColumns.TryGetValue(propertyName, out int col))
                    {
                        // We have not seen this property before. Allocate a new column number
                        col = maxcol;
                        propertyColumns.Add(propertyName, maxcol);
                        worksheet.ColumnHeaders[col].Text = propertyName;

                        maxcol += 1;
                    }
                    worksheet.SetCellData(new CellPosition(row, col), propertyValue);
                }

                row += 1;
            }

            if (worksheet != null)
            {
                worksheet.RowCount = row + 1;
                worksheet.ColumnCount = maxcol;
            }
        }

        private int PopulateNodes(Graph g)
        {
            var result = 0;
            foreach (var nodeLabel in g.Nodes.Select(n => n.Labels[0]).Distinct())
            {
                this.PopulateNode(g.Nodes.Where(n => n.Labels[0] == nodeLabel), nodeLabel);
                result += 1;
            }
            return result;
        }

        private void PopulateEdge(IEnumerable<Edge> edges, string type)
        {
            IDictionary<string, int> propertyColumns = new Dictionary<string, int>() { { "Id", 0 }, { "Type", 1 }, { "From", 2 }, { "To", 3 } };

            // Create the worksheet
            unvell.ReoGrid.Worksheet worksheet = this.Edges.Worksheets.Create(type);
            worksheet.ColumnHeaders[0].Text = "Id";
            worksheet.ColumnHeaders[1].Text = "Type";
            worksheet.ColumnHeaders[2].Text = "From";
            worksheet.ColumnHeaders[3].Text = "To";
            worksheet.FreezeToCell(0, 2, FreezeArea.Left);
            
            // The Id field is not editable
            worksheet.BeforeCellEdit += (s, e) => e.IsCancelled = e.Cell.Column == 0;

            // Add to the collection of worksheets.
            this.Edges.Worksheets.Add(worksheet);

            int row = 0;
            int maxcol = 4;

            // Populate each row in the worksheet with the contributions from the
            // predefined fields and the properties.
            foreach (var edge in edges)
            {
                worksheet.SetCellData(new CellPosition(row, 0), edge.Id);
                worksheet.SetCellData(new CellPosition(row, 1), edge.Type);
                worksheet.SetCellData(new CellPosition(row, 2), edge.From);
                worksheet.SetCellData(new CellPosition(row, 3), edge.To);

                foreach (var property in edge.Properties)
                {
                    var propertyName = property.Key;
                    var propertyValue = property.Value;

                    // If we have already seen the property, place it there.
                    if (!propertyColumns.TryGetValue(propertyName, out int col))
                    {
                        // We have not seen this property before. Allocate a new column number
                        col = maxcol;
                        propertyColumns.Add(propertyName, maxcol);
                        worksheet.ColumnHeaders[col].Text = propertyName;

                        maxcol += 1;
                    }
                    worksheet.SetCellData(new CellPosition(row, col), propertyValue);
                }

                row += 1;
            }

            if (worksheet != null)
            {
                worksheet.RowCount = row + 1;
                worksheet.ColumnCount = maxcol;
            }
        }

        private int PopulateEdges(Graph g)
        {
            var result = 0;
            foreach (var edgeType in g.Edges.Select(n => n.Type).Distinct())
            {
                this.PopulateEdge(g.Edges.Where(e => e.Type == edgeType), edgeType);
                result += 1;
            }
            return result;
        }

        void Populate(Graph g)
        {
            // Get rid of any existing worksheets:
            this.Nodes.Worksheets.Clear();
            this.Edges.Worksheets.Clear();

            var nodeWorksheets = this.PopulateNodes(g);
            var edgeWorksheets = this.PopulateEdges(g);

            if (nodeWorksheets == 0)
            {
                // leave an unmarked worksheet with an ID column and no data
                var worksheet = this.Nodes.Worksheets.Create("");
                worksheet.ColumnHeaders[0].Text = "Id";
                worksheet.RowCount = 1;
                worksheet.ColumnCount = 1;
                this.Nodes.Worksheets.Add(worksheet);
            }

            if (edgeWorksheets == 0)
            {
                // leave an unmarked worksheet with an ID column and no data
                var worksheet = this.Edges.Worksheets.Create("");
                worksheet.ColumnHeaders[0].Text = "Id";
                worksheet.ColumnHeaders[1].Text = "Type";
                worksheet.ColumnHeaders[2].Text = "From";
                worksheet.ColumnHeaders[3].Text = "To";
                worksheet.RowCount = 1;
                worksheet.ColumnCount = 1;
                this.Edges.Worksheets.Add(worksheet);
            }
        }

        private void Initialize()
        {
            // Get rid of the predefined worksheet. We will add our own below:
            //this.Nodes.Worksheets.RemoveAt(0);
            //this.Edges.Worksheets.RemoveAt(0);
        }

        public void UpdateStyle()
        {
            var backgroundColorBrush = this.FindResource("MaterialDesignPaper") as SolidColorBrush;
            var foregroundColorBrush = this.FindResource("MaterialDesignBody") as SolidColorBrush;

            ControlAppearanceStyle rgcs = ControlAppearanceStyle.CreateDefaultControlStyle();
            // create control style instance with theme colors
            rgcs = new ControlAppearanceStyle(backgroundColorBrush.Color, foregroundColorBrush.Color, false);

            //rgcs[ControlAppearanceColors.GridBackground] = backgroundColorBrush.Color;
            //rgcs[ControlAppearanceColors.GridText] = foregroundColorBrush.Color;
            //rgcs[ControlAppearanceColors.ColHeadText] = foregroundColorBrush.Color;
            //rgcs[ControlAppearanceColors.RowHeadText] = foregroundColorBrush.Color;
            //rgcs[ControlAppearanceColors.SheetTabBackground] = backgroundColorBrush.Color;
            //rgcs[ControlAppearanceColors.SheetTabText] = foregroundColorBrush.Color;

            // apply appearance style
            this.Nodes.ControlStyle = rgcs;
            this.Edges.ControlStyle = rgcs;
        }

        private DataLaboratory(EditorViewModel viewModel)
        {
            this.ViewModel = viewModel;

            this.InitializeComponent();

            this.UpdateStyle();

            this.ViewModel.PropertyChanged += (object _, System.ComponentModel.PropertyChangedEventArgs e) =>
            {
                if (e.PropertyName == "Graph")
                {
                    var g = this.ViewModel.Graph;
                    this.Graph = g;
                }
            };

            //// For all the worksheets: Record a handler for onclicked in a cell. Also,
            //// the first column (i.e. the id column) is not editable.
            ////foreach (var sheet in this.Nodes.Worksheets)
            ////{
            ////    sheet.AfterCellEdit += this.Sheet_AfterCellEdit;
            ////    // Disable editing of the ID column


            ////    // Add filtering. This only sets it for id
            ////    sheet.CreateColumnFilter(0,0, 0);
            ////}

            ////foreach (var sheet in this.Edges.Worksheets)
            ////{
            ////    sheet.AfterCellEdit += this.Sheet_AfterCellEdit;
            ////    // Disable editing of the ID column
            ////    sheet.BeforeCellEdit += (s, e) => e.IsCancelled = e.Cell.Column <= 2; // Fistt three are not editable
            ////    sheet.ColumnHeaders[0].Text = "Id";
            ////    sheet.CreateColumnFilter(0, 0, 0);
            ////}
        }

        private void Sheet_AfterCellEdit(object sender, unvell.ReoGrid.Events.CellAfterEditEventArgs e)
        {
            // Mark the cell as edited so a script to update the values
            // in the database can be authored.
            // e.Cell.Tag = id
            // throw new NotImplementedException();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            this.ViewModel.Laboratory = null;
        }
    }
}
