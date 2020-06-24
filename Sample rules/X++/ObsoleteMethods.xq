(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)
   
(: Find obsolete methods, list the attribute parameters, and order by package and artifact name :)
(: @Language Xpp :)

<ObsoleteMethods  Category='Informational' href='docs.microsoft.com/Socratex/ObsoleteMethods' Version='1.0'>
{
  for $a in /*
  for $m in $a//Method
  for $oa in $m/AttributeList/Attribute[lower-case(@Name) = 'sysobsolete' or lower-case(@Name) = 'sysobsoleteattribute']
  order by $a/@Package, $a/@Name
  return <ObsoleteMethod Artifact='{$a/@Artifact}' Package='{$a/@Package}'
    StartLine='{$m/@StartLine}' StartCol='{$m/@StartCol}' EndLine='{$m/@EndLine}' EndCol='{$m/@EndCol}'>
        <Args> 
        {
           for $arg in $oa/AttributeExpression/*
           return <Arg>{$arg/@Value}</Arg>
        }
       </Args>
    </ObsoleteMethod>
}
</ObsoleteMethods>