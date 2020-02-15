(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Show the tables that support inheritance. :)
(: Both the code and the metadata are shown :)
(: @Category Informational :)
(: @Language Xpp :)

<Results>
{
    for $t in Table[@SupportInheritance='True']
	    return <Result Artifact='{$t/@Artifact}' StartLine='1' StartCol='1'>
	               {$t/Metadata}
	           </Result>
}
</Results>