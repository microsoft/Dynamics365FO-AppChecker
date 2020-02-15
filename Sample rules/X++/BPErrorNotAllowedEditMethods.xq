(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Finds all edit and display methods :)
(: @Category BestPractice/CAR :)
(: @Language Xpp :)
(: @Tags BP, CAR :)

<BPErrorNotAllowedEditMethods Category='Mandatory' href='docs.microsoft.com/Socratex/BPErrorNotAllowedEditMethod' Version='1.0'>
{
    for $a in /(Form | Class | Table | Query)
        for $m in $a//Method[@IsEdit = 'True']
            return <BPErrorNotAllowedEditMethod Artifact='{$a/@Artifact}' Method='{$m/@Name}'
               StartLine='{$m/@StartLine}' StartCol='{$m/@StartCol}'
               EndLine='{$m/@EndLine}' EndCol='{$m/@EndCol}' />
}
</BPErrorNotAllowedEditMethods>
