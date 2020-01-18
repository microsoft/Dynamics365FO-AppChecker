(: Calculates the visibility of all methods on classes and tables. :)
let $results := 
<Results>
{
  for $c in /Class | /Table
  
    let $allMethods := count($c/Method)
    let $privateMethods := count($c/Method[@IsPrivate = 'True'])
    let $protectedMethods := count($c/Method[@IsProtected = 'True']) (: Explicitly marked with protected keyword :)
    let $publicMethods := count($c/Method[@IsPublic = 'True'])
    let $internalMethods := count($c[@IsInternal = 'True'])
          
    (: Compensate for methods that lack a visibility keyword: They are protected. :)
    let $protectedMethods := $protectedMethods + ($allMethods - $privateMethods - $protectedMethods - $publicMethods - $internalMethods)  
      
    return <Result Artifact='{$c/@Artifact}' PrivateMethodCount='{$privateMethods}' ProtectedMethodCount='{$protectedMethods}' PublicMethodCount='{$publicMethods}' InternalMethodCount='{$internalMethods}'/>
}
</Results>

return $results