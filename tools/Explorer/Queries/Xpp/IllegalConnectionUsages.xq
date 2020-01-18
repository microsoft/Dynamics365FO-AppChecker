(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: @Category Mandatory :)
(: @Language Xpp :)
(: @Author pvillads@microsoft.com :)

let $connectionClasses := <Results>
    <Result Name='Connection'/>    
    <Result Name='UserConnection'/>
</Results>

return <IllegalConnectionUsages Category='Mandatory' href='docs.microsoft.com/Socratex/IllegalConnectionUsages' Version='1.0'>
{
  for $connectionClass in $connectionClasses
  for $a in /(Class | Table | Form | Query)
  for $m in $a//Method
  for $connectionCreation in $m//NewCall[@Type='Connection']
  where not ($connectionCreation/ancestor::UsingStatement/Variable/$connectionCreation)

  return <IllegalConnectionUsage Artifact='{$a/@Artifact}' Method='{$m/@Name}'
    StartLine='{$connectionCreation/@StartLine}' StartCol='{$connectionCreation/@StartCol}' 
    EndLine='{$connectionCreation/@EndLine}' EndCol='{$connectionCreation/@EndCol}'/>
}
</IllegalConnectionUsages>
