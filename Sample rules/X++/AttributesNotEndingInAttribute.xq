(: Copyright (c) Microsoft Corporation.
Licensed under the MIT license. :)

(: Finds attribute classes that do not end in Attribute, like MyImportantAttribute :)

<AttributeClassesNotEndingInAttribute>
{
   for $c in /Class[@Extends="SysAttribute"]
   where not (ends-with(lower-case($c/@Name), 'attribute'))
   return <Attribute Name='{$c/@Name}'/>
   
}
</AttributeClassesNotEndingInAttribute>

 