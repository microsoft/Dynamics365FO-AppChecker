<Query Kind="Program">
  <Reference Relative="..\..\BaseXInterface\bin\Debug\BaseXInterface.dll">C:\Users\pvillads\source\repos\SocrateX\Explorer\BaseXInterface\bin\Debug\BaseXInterface.dll</Reference>
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
    var query = @"(: Calculates the visibility of all methods on classes and tables. :)
let $results :=
<Results>
{
  for $c in /Class | /Table

    let $allMethods := count($c/Method)
    let $privateMethods := count($c/Method[@IsPrivate = 'True'])
    let $protectedMethods := count($c/Method[@IsProtected = 'True'])
    let $publicMethods := count($c/Method[@IsPublic = 'True'])
    let $internalMethods := count($c/Method[@IsInternal = 'True'])
    let $undecoratedMethods := count($c/Method[@IsInternal='False' and @IsPrivate='False' and @IsProtected='False' and @IsPublic='False'])

    return <Result Artifact='{$c/@Artifact}'
        PrivateMethodCount='{$privateMethods}'
        ProtectedMethodCount='{$protectedMethods}'
        PublicMethodCount='{$publicMethods}'
        InternalMethodCount='{$internalMethods}'
        UnDecoratedMethodCount='{$undecoratedMethods}'/>
}
</Results>

return <Totals 
  PrivateMethodCount='{sum($results/Result/@PrivateMethodCount)}'
  ProtectedMethodCount='{sum($results/Result/@ProtectedMethodCount)}'
  PublicMethodCount='{sum($results/Result/@PublicMethodCount)}'
  UnDecoratedMethodCount='{sum($results/Result/@UnDecoratedMethodCount)}'
  InternalMethodCount='{sum($results/Result/@InternalMethodCount)}' />";

    XDocument sv;
    using (var session = s.GetSession("ApplicationFoundation"))
    {
        var queryResult = session.Execute("xquery " + query);
        sv = XDocument.Parse(queryResult, LoadOptions.SetLineInfo);
    }

    // sv.Dump();

	var model = new PlotModel();
	model.Title = "Method Visibility over all classes and tables.";
	var series = new PieSeries() { Title = "Visibility" };
	model.Series.Add(series);

    var privateSlice = new PieSlice("Private", int.Parse(sv.Element("Totals").Attribute("PrivateMethodCount").Value));
	var protectedSlice = new PieSlice("Protected", int.Parse(sv.Element("Totals").Attribute("ProtectedMethodCount").Value));
	var publicSlice = new PieSlice("Public", int.Parse(sv.Element("Totals").Attribute("PublicMethodCount").Value ));	
	var undecoratedSlice = new PieSlice("Undecorated", int.Parse(sv.Element("Totals").Attribute("UnDecoratedMethodCount").Value ));
	var internalSlice = new PieSlice("Internal", int.Parse(sv.Element("Totals").Attribute("InternalMethodCount").Value));
	
	privateSlice.Fill = OxyColors.Green;
	protectedSlice.Fill = OxyColors.Yellow;
	publicSlice.Fill = OxyColors.Red;
	undecoratedSlice.Fill = OxyColors.Pink;
	internalSlice.Fill = OxyColors.Blue;
	
	series.Slices.Add(privateSlice); 
	series.Slices.Add(protectedSlice);
	series.Slices.Add(publicSlice);
	series.Slices.Add(undecoratedSlice);
	series.Slices.Add(internalSlice);
	
	var view = new OxyPlot.Wpf.PlotView() { Model = model };

	var pngExporter = new OxyPlot.Wpf.PngExporter { Width = 600, Height = 400, Background = OxyColors.White };
	var bitmap = pngExporter.ExportToBitmap(model);
	System.Windows.Clipboard.SetImage(bitmap);

	PanelManager.DisplayWpfElement(view, "Method Visibility");
}

static BaseXInterface.BaseXServer s = new BaseXInterface.BaseXServer
		("localhost", 1984, "admin", "admin");

// Define other methods and classes here