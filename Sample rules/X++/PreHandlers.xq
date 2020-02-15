(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Names of all class methods that are pre handlers. :)
(: @Category Informational :)
(: @Language Xpp :)

<PostHandlers>
{
    for $c in /*
	for $m in $c//Method
	where $m/AttributeList/Attribute[@Name='PreHandlerFor']
	return <PreHandler Artifact='{$c/@Artifact}' 
	                   StartLine='{$m/@StartLine}' StartCol='{$m/@StartCol}'
	                   EndLine='{$m/@EndLine}' EndCol='{$m/@EndCol}' />
}
</PostHandlers>