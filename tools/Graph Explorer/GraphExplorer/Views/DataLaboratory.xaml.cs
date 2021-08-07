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

namespace GraphExplorer.Views
{
    /// <summary>
    /// Interaction logic for DataLab.xaml
    /// </summary>
    public partial class DataLaboratory : MaterialWindow
    {
        private EditorViewModel ViewModel {get; set; }

        public async static Task<DataLaboratory> CreateDataLaboratory(EditorViewModel viewModel)
        {
            var res = new DataLaboratory(viewModel);
            res.DataContext = viewModel;
            await res.InitializeAsync();
            return res;
        }

        private async Task InitializeAsync()
        {
            // Todo: Remve this. Listen to the Graph event from the view model.
            var graph = await Neo4jDatabase.ExecuteQueryGraphAsync("match p = (m: Method) -[r:CALLS]-> (m1: Method) where r.Count > 3 return p limit 30");
            //var records = await cursor.ToListAsync();
            //var dict = Neo4jDatabase.GenerateJSONParts(records);

            // Get rid of the predefined worksheet. We will add our own below:
            this.Nodes.Worksheets.RemoveAt(0);
            this.Edges.Worksheets.RemoveAt(0);

            IDictionary<string, int> propertyColumns = null;

            int row = 0;
            var nodes = graph.Nodes;
            int maxcol = 2;

            unvell.ReoGrid.Worksheet worksheet = null;
            foreach (var node in nodes)
            {
                var id = node.Id;
                var labels = node.Labels;

                // Assume only one for now
                var label = "";
                if (labels.Length > 0)
                {
                    label = labels[0];
                }

                worksheet = this.Nodes.Worksheets.Where(w => w.Name == label).FirstOrDefault();
                if (worksheet == null)
                {
                    // Create the worksheet
                    worksheet = this.Nodes.Worksheets.Create(label);
                    this.Nodes.Worksheets.Add(worksheet);

                    worksheet.FreezeToCell(0, 2, FreezeArea.Left);
                    row = 0;

                    propertyColumns = new Dictionary<string, int>();
                    maxcol = 2;

                    worksheet.ColumnHeaders[0].Text = "Id";
                    worksheet.ColumnHeaders[1].Text = "Label";

                    // The Id field is not editable
                    worksheet.BeforeCellEdit += (s, e) => e.IsCancelled = e.Cell.Column == 0;
                }

                worksheet.SetCellData(new CellPosition(row, 0), id);
                worksheet.SetCellData(new CellPosition(row, 1), label);

                var properties = node.Properties;

                foreach (var property in properties)
                {
                    var propertyName = property.Key;
                    var propertyValue = property.Value;

                    // If we have already seen the property, place it there.
                    if (!propertyColumns.TryGetValue(propertyName, out int col))
                    {
                        // We have not seen this property before. Allocate a new column number
                        propertyColumns.Add(propertyName, maxcol);
                        col = maxcol;
                        maxcol += 1;
                    }
                    worksheet.SetCellData(new CellPosition(row, col), propertyValue);

                    worksheet.ColumnHeaders[col].Text = propertyName;
                    col += 1;
                }

                row += 1;
            }

            if (worksheet != null)
            {
                worksheet.RowCount = row + 1;
                worksheet.ColumnCount = maxcol;
            }

            var edges = graph.Edges;
            foreach (var edge in edges)
            {
                var id = edge.Id;
                var from = edge.From;
                var to = edge.To;
                var type = edge.Type;
                var properties = edge.Properties;

                worksheet = this.Edges.Worksheets.Where(w => w.Name == type).FirstOrDefault();
                if (worksheet == null)
                {
                    // Create the worksheet
                    worksheet = this.Edges.Worksheets.Create(type);
                    this.Edges.Worksheets.Add(worksheet);

                    worksheet.FreezeToCell(0, 4, FreezeArea.Left);
                    row = 0;

                    propertyColumns = new Dictionary<string, int>();
                    maxcol = 4;

                    worksheet.ColumnHeaders[0].Text = "Id";
                    worksheet.ColumnHeaders[1].Text = "Type";
                    worksheet.ColumnHeaders[2].Text = "From";
                    worksheet.ColumnHeaders[3].Text = "To";

                    // The Id field is not editable
                    worksheet.BeforeCellEdit += (s, e) => e.IsCancelled = e.Cell.Column < 4;
                }

                worksheet.SetCellData(new CellPosition(row, 0), id);
                worksheet.SetCellData(new CellPosition(row, 1), type);
                worksheet.SetCellData(new CellPosition(row, 2), from);
                worksheet.SetCellData(new CellPosition(row, 3), to);

                properties = edge.Properties;

                foreach (var property in properties)
                {
                    var propertyName = property.Key;
                    var propertyValue = property.Value;

                    // If we have already seen the property, place it there.
                    if (!propertyColumns.TryGetValue(propertyName, out int col))
                    {
                        // We have not seen this property before. Allocate a new column number
                        propertyColumns.Add(propertyName, maxcol);
                        col = maxcol;
                        maxcol += 1;
                    }
                    worksheet.SetCellData(new CellPosition(row, col), propertyValue);

                    worksheet.ColumnHeaders[col].Text = propertyName;
                    col += 1;
                }

                row += 1;
            }

            if (worksheet != null)
            {
                worksheet.RowCount = row + 1;
                worksheet.ColumnCount = maxcol;
            }
        }

        private DataLaboratory(EditorViewModel viewModel)
        {
            this.ViewModel = viewModel;

            this.InitializeComponent();

            var backgroundColorBrush = this.FindResource("MaterialDesignPaper") as SolidColorBrush;
            var foregroundColorBrush = this.FindResource("MaterialDesignBody") as SolidColorBrush;

            ControlAppearanceStyle rgcs = ControlAppearanceStyle.CreateDefaultControlStyle();
            // create control style instance with theme colors
            // rgcs = new ControlAppearanceStyle(backgroundColorBrush.Color, foregroundColorBrush.Color, false);

            rgcs[ControlAppearanceColors.GridBackground] = backgroundColorBrush.Color;
            rgcs[ControlAppearanceColors.GridText] = foregroundColorBrush.Color;

            // apply appearance style
            this.Nodes.ControlStyle = rgcs;
            this.Edges.ControlStyle = rgcs;

            // For all the worksheets: Record a handler for onclicked in a cell. Also,
            // the first column (i.e. the id column) is not editable.
            //foreach (var sheet in this.Nodes.Worksheets)
            //{
            //    sheet.AfterCellEdit += this.Sheet_AfterCellEdit;
            //    // Disable editing of the ID column


            //    // Add filtering. This only sets it for id
            //    sheet.CreateColumnFilter(0,0, 0);
            //}

            //foreach (var sheet in this.Edges.Worksheets)
            //{
            //    sheet.AfterCellEdit += this.Sheet_AfterCellEdit;
            //    // Disable editing of the ID column
            //    sheet.BeforeCellEdit += (s, e) => e.IsCancelled = e.Cell.Column <= 2; // Fistt three are not editable
            //    sheet.ColumnHeaders[0].Text = "Id";
            //    sheet.CreateColumnFilter(0, 0, 0);
            //}
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
