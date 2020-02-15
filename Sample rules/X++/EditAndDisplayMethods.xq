(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Find Edit and display methods :)
(: @Category Informational :)
(: @Language Xpp :)

<EditAndDisplayMethods>
{
   for $c in /(Table | Class | Form)
   for $m in $c//Method 
   where lower-case($m/@IsDisplay)="true" or lower-case($m/@IsEdit)="true" 
   return <EditAndDisplayMethod Artifact='{$c/@Artifact}' Method='{$m/@Name}' 
		StartLine='{$m/@StartLine}' EndLine='{$m/@EndLine}' StartCol='{$m/@StartCol}' EndCol='{$m/@EndCol}'/>
}
</EditAndDisplayMethods>
