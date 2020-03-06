<Query Kind="Program">
  <Reference>C:\users\pvillads\source\repos\Socratex\explorer\BaseXInterface\bin\Debug\BaseXInterface.dll</Reference>
  <Namespace>System.Xml.Linq</Namespace>
</Query>

void Main()
{
    string queryResult;
    using (var session = s.GetDatabases("ApplicationFoundation")
    {
        queryResult = session.Execute (@"xquery <Comments>
{ for $c in /Class | /Table 
  for $m in $c/Method[@Comments != '']
	let $comment := $m/@Comments

	return <Comment Class='{$c/@Name}' MethodName='{$m/@Name}' Type='{$m/@Type}' Comment='{$comment}' ReturnType='{$m/@ReturnType}'
	                StartLine='{$m/@StartLine}' EndLine='{$m/@EndLine}' StartCol='{$m/@StartCol}' EndCol='{$m/@EndCol}'>
	       {
		       for $parm in $m/ParameterDeclaration
			       return <Parm Name='{$parm/@Name}'/>
		   }
	       </Comment>
} 
</Comments>");
    }

    var sv = XDocument.Parse(queryResult, LoadOptions.SetLineInfo);	
	// sv.Dump();
	
	var comments = sv.XPathSelectElements("/Comments/Comment");

    // This is where we store the results.
    var result = XDocument.Parse("<Results></Results>").Root;
	
	foreach (var commentNode in comments)
	{
		var c = commentNode.Attributes("Comment").First().Value;

		// Get all the lines that start with /// and remove the ///
		var lines = c.Split('\n').Select(l => l.Trim(' ')).Where(l => l.StartsWith("///")).Select(l => l.Remove(0, 3));
		c = string.Join("\n", lines);

		XElement commentDoc = null;
		string message = "";
		try
		{
			var d = XDocument.Parse("<Comment>" + c + "</Comment>");
			commentDoc = d.Root;
		}
		catch (Exception e)
		{
			message=e.Message;
		}

		if (commentDoc == null)
		{
			// Not well formed XML
			XElement error = new XElement("Error", c,
				new XAttribute("Class", commentNode.Attribute("Class").Value),
				new XAttribute("Method", commentNode.Attribute("MethodName").Value),
				new XAttribute("Kind", "Not wellformed XML"),
				new XAttribute("StartLine", commentNode.Attribute("StartLine").Value),
				new XAttribute("EndLine", commentNode.Attribute("EndLine").Value),
				new XAttribute("StartCol", commentNode.Attribute("StartCol").Value),
				new XAttribute("EndCol", commentNode.Attribute("EndCol").Value),
				new XAttribute("Message", message));
			result.Add(error);
		}
		else
		{
			// Get the summary tag.
			XElement summaryNode = null;
			if (commentDoc.FirstNode != null)
			{
				summaryNode = commentDoc.FirstNode.XPathSelectElement("/Comment/summary");
			}

			if (summaryNode == null)
			{
				result.Add(new XElement("Error", c,
					new XAttribute("Class", commentNode.Attribute("Class").Value),
					new XAttribute("Method", commentNode.Attribute("MethodName").Value),
					new XAttribute("Kind", "No summary tag"),
					new XAttribute("StartLine", commentNode.Attribute("StartLine").Value),
					new XAttribute("EndLine", commentNode.Attribute("EndLine").Value),
					new XAttribute("StartCol", commentNode.Attribute("StartCol").Value),
					new XAttribute("EndCol", commentNode.Attribute("EndCol").Value)));
			}
			else
			{
				var summaryText = summaryNode.Value;
				if (summaryText.Length < 12)
				{
					result.Add(new XElement("Error", c,
						new XAttribute("Class", commentNode.Attribute("Class").Value),
						new XAttribute("Method", commentNode.Attribute("MethodName").Value),
						new XAttribute("Kind", "Too little summary text"),
						new XAttribute("StartLine", commentNode.Attribute("StartLine").Value),
						new XAttribute("EndLine", commentNode.Attribute("EndLine").Value),
						new XAttribute("StartCol", commentNode.Attribute("StartCol").Value),
						new XAttribute("EndCol", commentNode.Attribute("EndCol").Value)));
				}
			}
			
			// Check that the parameters are described
			var sourceParams = commentNode.XPathSelectElements("./Parm");
			
			if (sourceParams.Any())
			{
				// Check that all are described
				var sourceParamsCount = sourceParams.Count();
				var paramNodes = commentDoc.XPathSelectElements("/Comment/param");
				if (paramNodes.Count() != sourceParamsCount)
				{
					result.Add(new XElement("Error",
						new XAttribute("Class", commentNode.Attribute("Class").Value),
						new XAttribute("Method", commentNode.Attribute("MethodName").Value),
						new XAttribute("Kind", "Param count discrepancy"),
						new XAttribute("StartLine", commentNode.Attribute("StartLine").Value),
						new XAttribute("EndLine", commentNode.Attribute("EndLine").Value),
						new XAttribute("StartCol", commentNode.Attribute("StartCol").Value),
						new XAttribute("EndCol", commentNode.Attribute("EndCol").Value)));
				}
				
				foreach (var sourceParam in sourceParams)
				{
					var paramName = sourceParam.Attribute("Name").Value;
					var paramTags = commentDoc.XPathSelectElements(string.Format("/Comment/param[@name='{0}']", paramName));

					if (!paramTags.Any())
					{
						result.Add(new XElement("Error", 
							new XAttribute("Class", commentNode.Attribute("Class").Value),
							new XAttribute("Method", commentNode.Attribute("MethodName").Value),
							new XAttribute("Kind", "Param not described"),
							new XAttribute("Message", paramName),
							new XAttribute("StartLine", commentNode.Attribute("StartLine").Value),
							new XAttribute("EndLine", commentNode.Attribute("EndLine").Value),
							new XAttribute("StartCol", commentNode.Attribute("StartCol").Value),
							new XAttribute("EndCol", commentNode.Attribute("EndCol").Value)));
					}
					else if (paramTags.Count() > 1)
					{
						result.Add(new XElement("Error",
							new XAttribute("Class", commentNode.Attribute("Class").Value),
							new XAttribute("Method", commentNode.Attribute("MethodName").Value),
							new XAttribute("Kind", "Param multiple description"),
							new XAttribute("Message", paramName),
							new XAttribute("StartLine", commentNode.Attribute("StartLine").Value),
							new XAttribute("EndLine", commentNode.Attribute("EndLine").Value),
							new XAttribute("StartCol", commentNode.Attribute("StartCol").Value),
							new XAttribute("EndCol", commentNode.Attribute("EndCol").Value)));
					}					
					else
					{
						if (paramTags.First().Value.Length < 12)
						{
							result.Add(new XElement("Error",
								new XAttribute("Class", commentNode.Attribute("Class").Value),
								new XAttribute("Method", commentNode.Attribute("MethodName").Value),
								new XAttribute("Kind", "Too little param text"),
								new XAttribute("Message", paramName),
								new XAttribute("StartLine", commentNode.Attribute("StartLine").Value),
								new XAttribute("EndLine", commentNode.Attribute("EndLine").Value),
								new XAttribute("StartCol", commentNode.Attribute("StartCol").Value),
								new XAttribute("EndCol", commentNode.Attribute("EndCol").Value)));
						}
					}
				}
			}
			else
			{
				// There are no params in the method; check there are parms in the documentation comments
				var paramNodes = commentDoc.XPathSelectElements("/Comment/param");
				if (paramNodes.Any())
				{
					result.Add(new XElement("Error",
						new XAttribute("Class", commentNode.Attribute("Class").Value),
						new XAttribute("Method", commentNode.Attribute("MethodName").Value),
						new XAttribute("Kind", "No method parameters to document"),
						new XAttribute("StartLine", commentNode.Attribute("StartLine").Value),
						new XAttribute("EndLine", commentNode.Attribute("EndLine").Value),
						new XAttribute("StartCol", commentNode.Attribute("StartCol").Value),
						new XAttribute("EndCol", commentNode.Attribute("EndCol").Value)));
				}
			}

			// If the method has a result that is not void, there should
			// be a returns tag in the comment documentation
			var returnType = commentNode.Attribute("ReturnType").Value;
            if (!string.IsNullOrEmpty(returnType) && returnType != "void")
			{
				// A type was provided. (Constructors have an empty returntype)
			}

		}		
	}
	
	result.Dump(); //result.Document.Save(@"c:\users\pvillads\desktop\res.xml");
}

static BaseXInterface.BaseXServer s = new BaseXInterface.BaseXServer
		("localhost", 1984, "admin", "admin");

// Define other methods and classes here