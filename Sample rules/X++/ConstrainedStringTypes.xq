(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Find References to all constrained string types :)
(: @Language Xpp :)
(: @Category Mandatory :)
(: @Author pvillads@microsoft.com :)

<ConstrainedStringTypes Category='Mandatory' href='docs.microsoft.com/Socratex/ConstantConditions' Version='1.0'>
{
  for $a in /(Class | Table | Form | Query)
  for $decl in $a//StringLengthType
  return <ConstrainedStringTypeReference Artifact='{$a/@Artifact}'
    StartLine='{$decl/@StartLine}' StartCol='{$decl/@StartCol}' EndLine='{$decl/@EndLine}' EndCol='{$decl/@EndCol}'>
    {$decl}
    </ConstrainedStringTypeReference>
}
</ConstrainedStringTypes>
