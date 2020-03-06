(: Show the tables that support multiple inheritance. :)
(: Both the code and the metadata are shown :)
<Results>
{
    for $t in Table[@SupportInheritance='True']
	    return <Result Artifact='{$t/@Artifact}' StartLine='1' StartCol='1'>
	               {$t/Metadata}
	           </Result>
}
</Results>