(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Identifies pure abstract classes, i.e. abstract classes that only have
 abstract methods. These are good candidates for interfaces. :)

(: @Language Xpp :)
(: @Category Informatoion :)

<PureAbstractClasses>
{
  for $a in /Class
  order by $a/@Name
  where count($a/Method) = count($a/Method[lower-case(@IsAbstract) = 'true'])
  return <PureAbstractClass Artifact='{$a/@Artifact}'
    StartLine='{$a/@StartLine}' StartCol='{$a/@StartCol}' EndLine='{$a/@EndLine}' EndCol='{$a/@EndCol}'/>
}
</PureAbstractClasses>