(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: This query finds all data event handler methods that are placed
in extension classes. This is not the correct way to use extension classes,
and doing this will typically make the handler methods available on any
table, since the first parameter of such handlers is of type Common :)

(: @Language Xpp :)
(: @Category Informational :)

<StaticHandlerMethodsInExtensions>
{
    for $c in /Class
    where $c[ends-with(lower-case(@Name), '_extension')]
      and not ($c/AttributeList/Attribute[lower-case(@Name) = 'extensionof'])

    for $m in $c//Method
    where $m[@IsStatic = 'true'] 
      and $m[@IsPrivate='false'] 
      and $m/AttributeList/Attribute[contains(lower-case(@Name), 'handler')]
    order by $c/@Package, $c/@Name    
 
    return <StaticHandlerMethod Artifact='{$c/@Artifact}'  Package='{$c/@Package}'
                                Method='{$m/@Name}' 
                                InAdvertentlyExtending='{$m/ParameterDeclaration[1]/@Type}'
                                StartLine='{$m/@StartLine}' StartCol='{$m/@StartCol}' 
                                EndLine='{$m/@EndLine}' EndCol='{$m/@EndCol}'/>
}
</StaticHandlerMethodsInExtensions>
