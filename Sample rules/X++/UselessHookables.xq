(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: This query checks for [hookable(false)] being applied on private methods
which provides no value, since private methods are not hookable by default :)

(: @Category BestPractice :)
(: @Language Xpp :)

<UselessHookables Category='BestPractice' href='docs.microsoft.com/Socratex/UselessHookables' Version='1.0'>
{
    for $a in /(Class | Table | Query | Form)
    for $m in $a//Method where $m/@IsPrivate='true'
    for $attr in $m/AttributeList/Attribute[lower-case(@Name)="hookable"]
    where lower-case($attr/AttributeExpression/BooleanAttributeLiteral/@Value)="false"
     
    return <UselessHookable Artifact='{$a/@Artifact}'
        StartLine= '{$attr/@StartLine}' EndLine= '{$attr/@EndLine}'
        StartCol= '{$attr/@Startol}' EndCol= '{$attr/@EndCol}' />
}
</UselessHookables>