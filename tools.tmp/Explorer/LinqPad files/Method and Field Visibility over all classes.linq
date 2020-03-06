<Query Kind="Program">
  <Reference Relative="..\..\BaseXInterface\bin\Debug\BaseXInterface.dll">C:\users\pvillads\source\repos\Socratex\explorer\BaseXInterface\bin\Debug\BaseXInterface.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\PresentationCore.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\PresentationFramework.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Xaml.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\WindowsBase.dll</Reference>
  <NuGetReference>OxyPlot.Wpf</NuGetReference>
  <Namespace>OxyPlot</Namespace>
  <Namespace>OxyPlot.Axes</Namespace>
  <Namespace>OxyPlot.Series</Namespace>
  <Namespace>System.Windows</Namespace>
  <Namespace>System.Windows.Controls</Namespace>
  <Namespace>System.Xml.Linq</Namespace>
</Query>

void Main()
{
  var query = @"let $results :=
  <Results>
  {
    for $c in /Class

    let $allMethods := count($c/Method)
    let $privateMethods := count($c/Method[@IsPrivate = 'True'])
    let $protectedMethods := count($c/Method[@IsProtected = 'True']) (: Explicitly marked with protected keyword :)
    let $publicMethods := count($c/Method[@IsPublic = 'True'])
    let $internalMethods := count($c[@IsInternal = 'True'])
    let $privateFields := count($c/FieldDeclaration[@IsPrivate = 'True'])
    let $protectedFields := count($c/FieldDeclaration[@IsProtected ='True'])
    let $publicFields := count($c/FieldDeclaration[@IsPublic = 'True'])
    let $internalFields := count($c/FieldDeclaration[@IsInternal = 'True'])

    (: Compensate for methods that lack a visibility keyword: They are protected. :)
    let $protectedMethods := $protectedMethods + ($allMethods - $privateMethods - $protectedMethods - $publicMethods - $internalMethods)

    return <Result Class='{$c/@Name}' PrivateMethodCount='{$privateMethods}' ProtectedMethodCount='{$protectedMethods}' PublicMethodCount='{$publicMethods}' InternalMethodCount='{$internalMethods}'
  PrivateFieldCount='{$privateFields}' ProtectedFieldCount='{$protectedFields}' PublicFieldCount='{$publicFields}' InternalFieldCount='{$internalFields}'/>
  }
  </Results>

  return <Totals
    PrivateMethodCount='{sum($results/Result/@PrivateMethodCount)}'
    ProtectedMethodCount='{sum($results/Result/@ProtectedMethodCount)}'
    PublicMethodCount='{sum($results/Result/@PublicMethodCount)}'
    InternalMethodCount='{sum($results/Result/@InternalMethodCount)}'
    PrivateFieldCount='{sum($results/Result/@PrivateFieldCount)}'
    ProtectedFieldCount='{sum($results/Result/@ProtectedFieldCount)}'
    PublicFieldCount='{sum($results/Result/@PublicFieldCount)}'
    InternalFieldCount='{sum($results/Result/@InternalFieldCount)}' />";
  
    XDocument sv;
    using (var session = s.GetSession("ApplicationFoundation"))
    {
        var queryResult = session.Execute("xquery " + query);
        sv = XDocument.Parse(queryResult, LoadOptions.SetLineInfo);
    }

    var methodModel = new PlotModel();
    methodModel.Title = "Method Visibility over all classes.";
    var series = new PieSeries() { Title = "Visibility" };
    methodModel.Series.Add(series);
    
    var privateSlice = new PieSlice("Private", int.Parse(sv.Element("Totals").Attribute("PrivateMethodCount").Value));
    var protectedSlice = new PieSlice("Protected", int.Parse(sv.Element("Totals").Attribute("ProtectedMethodCount").Value));
    var publicSlice = new PieSlice("Public", int.Parse(sv.Element("Totals").Attribute("PublicMethodCount").Value));
    var internalSlice = new PieSlice("Internal", int.Parse(sv.Element("Totals").Attribute("InternalMethodCount").Value));
    
    privateSlice.Fill = OxyColors.Green;
    protectedSlice.Fill = OxyColors.Yellow;
    publicSlice.Fill = OxyColors.Red;
    internalSlice.Fill = OxyColors.Blue;
    
    series.Slices.Add(privateSlice);
    series.Slices.Add(protectedSlice);
    series.Slices.Add(publicSlice);
    series.Slices.Add(internalSlice);
    
    var methodView = new OxyPlot.Wpf.PlotView() { Model = methodModel };
    
    var fieldModel = new PlotModel();
    fieldModel.Title = "Field Visibility over all classes.";
    series = new PieSeries() { Title = "Visibility" };
    fieldModel.Series.Add(series);
    
    privateSlice = new PieSlice("Private", int.Parse(sv.Element("Totals").Attribute("PrivateFieldCount").Value));
    protectedSlice = new PieSlice("Protected", int.Parse(sv.Element("Totals").Attribute("ProtectedFieldCount").Value));
    publicSlice = new PieSlice("Public", int.Parse(sv.Element("Totals").Attribute("PublicFieldCount").Value));
    internalSlice = new PieSlice("Internal", int.Parse(sv.Element("Totals").Attribute("InternalFieldCount").Value));
    
    privateSlice.Fill = OxyColors.Green;
    protectedSlice.Fill = OxyColors.Yellow;
    publicSlice.Fill = OxyColors.Red;
    internalSlice.Fill = OxyColors.Blue;
    
    series.Slices.Add(privateSlice);
    series.Slices.Add(protectedSlice);
    series.Slices.Add(publicSlice);
    series.Slices.Add(internalSlice);
    
    var fieldView = new OxyPlot.Wpf.PlotView() { Model = fieldModel };
    
    var g = new Grid() { ShowGridLines = true };
    
    g.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(50, GridUnitType.Star) });
    g.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(50, GridUnitType.Star) });
    
    g.Children.Add(methodView);
    Grid.SetColumn(methodView, 0);
    g.Children.Add(fieldView);
    Grid.SetColumn(fieldView, 1);
    
    PanelManager.DisplayWpfElement(g, "Visibility");
}

static BaseXInterface.BaseXServer s = new BaseXInterface.BaseXServer
("localhost", 1984, "admin", "admin");

// Define other methods and classes here