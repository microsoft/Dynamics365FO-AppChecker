(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Forms must not contain data access primitives in the init method :)
(: @Category Performance :)
(: @Language Xpp :)
(: @Author pvillads@microsoft.com :)

<DataAccessLoopInInitViolation>
{
    for $f in /Form
    for $m in $f//Method[lower-case(@Name)="init"]//SearchStatement
    return <Violation Artifact='{$f/@Artifact}'
        StartLine='{$m/@StartLine}' EndLine='{$m/@EndLine}'
        StartCol='{$m/@StartCol}' EndCol='{$m/@EndCol}' />

}
</DataAccessLoopInInitViolation>