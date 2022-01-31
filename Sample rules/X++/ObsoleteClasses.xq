(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)
   
(: Find obsolete classes, list the attribute parameters, and order by package and artifact name :)
(: @Language Xpp :)

<ObsoleteClasses>
{
  for $c in /Class
  order by $c/@Package, $c/@Name  
  for $a in $c/AttributeList/Attribute[@Name = 'SysObsolete' or @Name = 'SysObsoleteAttribute']
  let $reason := $a/AttributeExpression[1]/StringAttributeLiteral/@Value
  let $severity := $a/AttributeExpression[2]/BooleanAttributeLiteral/@Value
  let $date := $a/AttributeExpression[3]
  
  return <ObsoleteClass Artifact='{$c/@Artifact}' Reason='{$reason}' Breaking='{if ($severity) then $severity else 'false(default)'}' 
    StartLine='{$a/@StartLine}' EndLine='{$a/@EndLine}' StartCol='{$a/@StartCol}' EndCol='{$a/@EndCol}'>
  {

  }
  </ObsoleteClass>
}
</ObsoleteClasses>