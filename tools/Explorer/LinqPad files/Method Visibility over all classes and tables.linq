<Query Kind="Program">
  <Reference Relative="..\..\BaseXInterface\bin\Debug\BaseXInterface.dll">C:\Users\pvillads\Desktop\Dynamics365FO-AppChecker\tools\BaseXInterface\bin\Debug\BaseXInterface.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\PresentationCore.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\PresentationFramework.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Xaml.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\WindowsBase.dll</Reference>
  <NuGetReference>OxyPlot.Wpf</NuGetReference>
  <Namespace>OxyPlot</Namespace>
  <Namespace>OxyPlot.Axes</Namespace>
  <Namespace>OxyPlot.Series</Namespace>
</Query>

[STAThread]
async void Main()
{
    var query = @"(: Calculates the visibility of all methods on classes and tables. :)
let $results :=
<Results>
{
  for $c in /Class | /Table

    let $allMethods := count($c/Method)
    let $privateMethods := count($c/Method[@IsPrivate = 'true'])
    let $protectedMethods := count($c/Method[@IsProtected = 'true'])
    let $publicMethods := count($c/Method[@IsPublic = 'true'])
    let $internalMethods := count($c/Method[@IsInternal = 'true'])
    let $undecoratedMethods := count($c/Method[@IsInternal='false' and @IsPrivate='false' and @IsProtected='false' and @IsPublic='false'])

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
    using (var session = await s.GetSessionAsync ("ApplicationFoundation"))
    {
        var queryResult = session.Execute("xquery " + query);
        sv = XDocument.Parse(queryResult, LoadOptions.SetLineInfo);
    }

    sv.Dump();

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

	Thread newWindowThread = new Thread(new ThreadStart(() =>
	{
		// create and show the window
		var view = new OxyPlot.Wpf.PlotView() { Model = model };

		var pngExporter = new OxyPlot.Wpf.PngExporter { Width = 600, Height = 400, Background = OxyColors.White };
		var bitmap = pngExporter.ExportToBitmap(model);
		System.Windows.Clipboard.SetImage(bitmap);

		// start the Dispatcher processing  
		System.Windows.Threading.Dispatcher.Run();
		PanelManager.DisplayWpfElement(view, "Method Visibility");
	}));
	newWindowThread.SetApartmentState(ApartmentState.STA);

	// make the thread a background thread  
	newWindowThread.IsBackground = true;

	// start the thread  
	newWindowThread.Start();
	Console.ReadLine();
}

static BaseXInterface.BaseXServer s = new BaseXInterface.BaseXServer
		("localhost", 1984, "admin", "admin");

// Define other methods and classes here