(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Export class extension information in CSV format :)

let $options := map { 'lax': false(), 'header': true() ,'format': 'attributes' }

(: Cache all classes for looking up package :)
let $classes := map:merge(
  for $c in /Class
  return map:entry(lower-case($c/@Name), lower-case($c/@Package))
)

(: Cache the extension classes :)
let $extensions := <Extensions>
{
    for $c in /Class
    for $attr in $c/AttributeList/Attribute[lower-case(@Name)='extensionof']
    where lower-case($attr/AttributeExpression/IntrinsicAttributeLiteral/@FunctionName) = 'classstr'
    let $extensionof := lower-case($attr/AttributeExpression/IntrinsicAttributeLiteral/@Arg1)
    return <Extension ExtendedType='{$extensionof}' ExtensionClass='{lower-case($c/@Name)}' ExtensionClassPackage='{lower-case($c/@Package)}'>
    </Extension>
}</Extensions>

let $r := <ExtensionsOnMethods>
{
  for $e in $extensions/Extension
  return <Record>
      <Extension name=':START_ID'>{ lower-case("/" || $e/@ExtensionClassPackage || "/classes/" || $e/@ExtensionClass)}</Extension>
      <Extended name=':END_ID'>{ lower-case("/" || map:get($classes, $e/@ExtendedType) || "/classes/" || $e/@ExtendedType) }</Extended>
      <Type name=':TYPE'>EXTENSIONOF</Type>
  </Record>
  
}
</ExtensionsOnMethods>

return csv:serialize($r, $options)
