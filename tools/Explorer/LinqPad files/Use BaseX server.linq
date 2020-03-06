<Query Kind="Program">
  <Reference Relative="..\..\BaseXInterface\bin\Debug\BaseXInterface.dll">C:\Users\pvillads\source\repos\SocrateX\Explorer\BaseXInterface\bin\Debug\BaseXInterface.dll</Reference>
  <Namespace>System.Xml.Linq</Namespace>
</Query>

void Main()
{
    using (var session = s.GetSession("ApplicationFoundation"))
    {
        var sv = XDocument.Parse(session.Execute ("xquery " + "/Class[@Name='AifAsyncResult']"), LoadOptions.SetLineInfo);
        sv.Dump();
    }
}

static BaseXInterface.BaseXServer s = new BaseXInterface.BaseXServer
		("localhost", 1984, "admin", "admin");

// Define other methods and classes here