using MaterialDesignExtensions.Controls;
using SocratexGraphExplorer.Models;
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

namespace SocratexGraphExplorer.Views
{
    /// <summary>
    /// Interaction logic for DataLab.xaml
    /// </summary>
    public partial class DataLab : MaterialWindow
    {
        public async static Task<DataLab> CreateDataLaboratory()
        {
            var res = new DataLab();
            await res.InitializeAsync();
            return res;
        }

        private async Task InitializeAsync()
        {
            var cursor = await Neo4jDatabase.ExecuteQueryAsync("match (c:Class) -[r]->(p) return * limit 3");
            var records = await cursor.ToListAsync();
            var dict = Neo4jDatabase.GenerateJSONParts(records);

            // Get rid of the predefined worksheet. We will add our own below:
            this.Nodes.Worksheets.RemoveAt(0);
            this.Edges.Worksheets.RemoveAt(0);

            IDictionary<string, int> propertyColumns = null;

            int row = 0;
            var nodes = dict["nodes"];
            int maxcol = 2;

            foreach (IDictionary<string, object> node in nodes as Dictionary<long, object>.ValueCollection)
            {
                var id = node["id"] ;
                var labels = node["labels"] as string[];

                // Assume only one for now
                var label = "";
                if (labels.Length > 0)
                {
                    label = labels[0];
                }

                var worksheet = this.Nodes.Worksheets.Where(w => w.Name == label).FirstOrDefault();
                if (worksheet == null)
                {
                    // Create the worksheet
                    worksheet = this.Nodes.Worksheets.Create(label);
                    this.Nodes.Worksheets.Add(worksheet);
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

                var properties = node["properties"] as IDictionary<string, object>;

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

            var edges = dict["edges"];
            foreach (IDictionary<string, object> edge in edges as Dictionary<long, object>.ValueCollection)
            {
                var id = edge["id"];
                var from = edge["from"];
                var to = edge["to"];
                var type = edge["type"] as string;
                var properties = edge["properties"] as IDictionary<string, object>;

                var worksheet = this.Edges.Worksheets.Where(w => w.Name == type).FirstOrDefault();
                if (worksheet == null)
                {
                    // Create the worksheet
                    worksheet = this.Edges.Worksheets.Create(type);
                    this.Edges.Worksheets.Add(worksheet);
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

                properties = edge["properties"] as IDictionary<string, object>;

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
        }

        private DataLab()
        {
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

            // For all the worksheets: Record a handler for onclicked in a cell. Alson,
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
    }
}
