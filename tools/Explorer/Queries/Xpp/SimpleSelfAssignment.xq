(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Query for finding self-assignments :)
(: @Category Mandatory :)
(: @Language Xpp :)
(: @Author pvillads@microsoft.com :)

<SimpleSelfAssignments>
{
for $a in /(Class | Table | Query | Form)
for $m in $a//Method
for $assignment in $m//AssignEqualStatement
where $assignment/FieldExpression
let $lhs := $assignment/Field
let $rhs := $assignment/FieldExpression/SimpleField
where $lhs/@Name = $rhs/@Name
return <SelfAssignment Artifact='{$a/@Artifact}'
      StartLine='{$assignment/@StartLine}' StartCol='{$assignment/@StartCol}'
      EndLine='{$assignment/@EndLine}' EndCol='{$assignment/@EndCol}'/>

} 
</SimpleSelfAssignments>

