(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Find places where classes that implement IDisposable are not created in using statements :)
(: @Category Mandatory :)
(: @Language Xpp :)
(: @Author pvillads@microsoft.com :)

declare function local:Parents($c, $getClassByName)
{
  if ($c/@Extends != "") then
     ($c/@Extends, local:Parents($getClassByName($c/@Extends), $getClassByName))
  else
     ()
};

let $getClassByName := function($name) { /Class[@Name=$name] }

let $disposables := <Disposables>
{
    for $c in /Class
    where some $c1 in local:Parents($c, $getClassByName)
    satisfies $getClassByName($c1)/Implements[.='IDisposable']
    return <ImplementsIDisposable Name='{$c/@Name}' />
}
</Disposables>

return <DisposablesNotInUsingStatements Category='Mandatory' href='docs.microsoft.com/Socratex/DisposablesNotInUsingStatements' Version='1.0'>
{
  for $a in /(Class | Table | Form | Query)
  for $m in $a//Method
  for $instanceCreation in $m//NewCall
  where $disposables/ImplementsIDisposable[@Name=$instanceCreation/@Type]
  where not ($instanceCreation/ancestor::UsingStatement/Variable/$instanceCreation)

  return <DisposableNotInUsingStatements Artifact='{$a/@Artifact}' Method='{$m/@Name}'
    StartLine='{$instanceCreation/@StartLine}' StartCol='{$instanceCreation/@StartCol}' 
    EndLine='{$instanceCreation/@EndLine}' EndCol='{$instanceCreation/@EndCol}'/>
}
</DisposablesNotInUsingStatements>