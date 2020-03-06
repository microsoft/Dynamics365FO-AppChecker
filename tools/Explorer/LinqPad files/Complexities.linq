<Query Kind="Program">
  <Reference Relative="..\..\BaseXInterface\bin\Debug\BaseXInterface.dll">C:\Dev\root\Xpp Reasoning\BaseXInterface\bin\Debug\BaseXInterface.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\PresentationCore.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\PresentationFramework.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Xaml.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\WindowsBase.dll</Reference>
  <NuGetReference>OxyPlot.Wpf</NuGetReference>
  <Namespace>OxyPlot</Namespace>
  <Namespace>OxyPlot.Axes</Namespace>
  <Namespace>OxyPlot.Series</Namespace>
  <Namespace>System.Xml.Linq</Namespace>
</Query>

void Main()
{
  using (BaseXInterface.DatabaseSession session = s.GetSession("ApplicationPlatform"))
  {
    var documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    var queriesFolder = @"C:\Users\pvillads\Documents\SocrateX\Queries\Xpp";
    var query = System.IO.File.ReadAllText(queriesFolder + @"\complexities.xq");

    var result = session.Execute("xquery " + query);
    var sv = XDocument.Parse(result, LoadOptions.SetLineInfo);

    var model = new PlotModel();
    model.Title = "Complexity";
    var series = new ColumnSeries() { Title = "Complexity", LabelFormatString = "{0:0}", LabelPlacement = LabelPlacement.Middle };

    var xAxis = new CategoryAxis() { Position = AxisPosition.Bottom, Angle = 45 };
    model.Axes.Add(xAxis);
    model.Series.Add(series);

    var complexityEntries = sv.XPathSelectElements("/Complexities/Complexity[@ComplexityNumber > 0]");
    foreach (var complexityEntry in complexityEntries)
    {
      var names = complexityEntry.Attribute("Method").Value.Split('.');
      var twoLines = names[0] + "\n" + names[1];
      var complexity = int.Parse(complexityEntry.Attribute("ComplexityNumber").Value); ;
      xAxis.Labels.Add(twoLines);
      series.Items.Add(new ColumnItem(complexity));
    }

    var view = new OxyPlot.Wpf.PlotView() { Model = model };

    var pngExporter = new OxyPlot.Wpf.PngExporter { Width = 600, Height = 400, Background = OxyColors.White };
    var bitmap = pngExporter.ExportToBitmap(model);
    System.Windows.Clipboard.SetImage(bitmap);

    PanelManager.DisplayWpfElement(view, "Class Complexity");
  }
}



static BaseXInterface.BaseXServer s = new BaseXInterface.BaseXServer
("localhost", 1984, "admin", "admin");
