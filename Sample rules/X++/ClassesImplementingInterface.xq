(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Find classes that implement a particular interface :)
(: @Category Informational :)
(: @Language Xpp :)

<Implementations>
{
for $c in /Class
for $i in $c/Implements[.='SysSetup']
    return <Implements Artifact='{$c/@Artifact}' Implements='{$i/.}'  
        StartLine='{$c/@StartLine}'  StartCol='{$c/@StartCol}'
        EndLine='{$c/@EndLine}'  EndCol='{$c/@EndCol}'/>
}
</Implementations>