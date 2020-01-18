(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Abstract classes that have no abstract methods :)
(: @Language Xpp :)
(: @Category BestPractice :)
(: @Author pvillads@microsoft.com :)

<AbstractClassWithNoAbstractMethods Category='BestPractice' href='docs.microsoft.com/Socratex/AbstractClassWithNoAbstractMethods' Version='1.0'>
{
  for $a in /Class[@IsAbstract='True']
  where not ($a/Method[@IsAbstract='True'])
  order by $a/@Name
  return <Result Artifact='{$a/@Artifact}'
    StartLine='{$a/@StartLine}' StartCol='{$a/@StartCol}' EndLine='{$a/@EndLine}' EndCol='{$a/@EndCol}'/>
}
</AbstractClassWithNoAbstractMethods>
