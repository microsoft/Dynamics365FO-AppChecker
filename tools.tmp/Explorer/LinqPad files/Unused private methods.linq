<Query Kind="Program">
  <Connection>
    <ID>47d6ffa3-c711-40ba-a172-fdaed0ce6864</ID>
    <Server>rdxp0128c1pvill</Server>
    <Database>DYNAMICSXREFDB</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <Reference>C:\Dev\root\Xpp Reasoning\BaseXInterface\bin\Debug\BaseXInterface.dll</Reference>
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
  for $c in (/Class | /Table)
    for $m in $c/Method[@IsPrivate='True' and @Name != 'typenew']
     return <PrivateMethod Class='{$c/@Name}' MethodName='{$m/@Name}' />
}
</Results>
return $results";

    XDocument sv;
    using (var session = s.GetSession("ApplicationFoundation"))
    {
        var queryResult = session.Execute("xquery " + query);
        sv = XDocument.Parse(queryResult, LoadOptions.SetLineInfo);
    }

    var privateMethods = sv.XPathSelectElements("/Results/PrivateMethod");

	foreach (var m in privateMethods)
	{
		string classpath = string.Format("/Classes/{0}/Methods/{1}", m.Attribute("Class").Value, m.Attribute("MethodName").Value);
		string tablepath = string.Format("/Tables/{0}/Methods/{1}", m.Attribute("Class").Value, m.Attribute("MethodName").Value);

		if (!Dependencies.Where(d => d.TargetPath == classpath || d.TargetPath == tablepath).Where(d => d.Kind == "MethodCall").Any())
		{
			m.Dump();
		}
	}
}

static BaseXInterface.BaseXServer s = new BaseXInterface.BaseXServer
		("localhost", 1984, "admin", "admin");