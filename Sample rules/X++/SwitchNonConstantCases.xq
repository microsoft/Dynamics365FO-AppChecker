(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Find switch statements that have case entries that are not constants :)
(: There is nothing semantically incorrect about this, except that it 
may have maintainablity problems :)

(: @Language Xpp :)
(: @Category Informational :)
<SwitchNonConstantCases>
{
  for $a in /(Class | Table | Form | Query)
  for $sw in $a//SwitchStatement
  for $cvs in $sw/CaseValues
  where not( $cvs/(IntLiteralExpression | StringLiteralExpression | Int64LiteralExpression | RealLiteralExpression | QualifiedStaticFieldExpression | Intrinsic))
  return <SwitchNonConstantCase Artifact='{$a/@Artifact}'
    StartLine='{$sw/@StartLine}' StartCol='{$sw/@StartCol}' EndLine='{$sw/@EndLine}' EndCol='{$sw/@EndCol}'>
    {
       (: $cvs :)
    }
    </SwitchNonConstantCase>
}
</SwitchNonConstantCases>