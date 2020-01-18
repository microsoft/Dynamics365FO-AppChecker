(: Calculates the visibility of all class methods and fields. :)
let $results := 
<Results>
{
  for $c in /Class
  
    let $allMethods := count($c/Method)
    let $privateMethods := count($c/Method[@IsPrivate = 'True'])
    let $protectedMethods := count($c/Method[@IsProtected = 'True']) (: Explicitly marked with protected keyword :)
    let $publicMethods := count($c/Method[@IsPublic = 'True'])
    let $internalMethods := count($c[@IsInternal = 'True'])
    let $privateFields := count($c/FieldDeclaration[@IsPrivate = 'True'])
    let $protectedFields := count($c/FieldDeclaration[@IsProtected ='True'])
    let $publicFields := count($c/FieldDeclaration[@IsPublic = 'True'])
    let $internalFields := count($c/FieldDeclaration[@IsInternal = 'True'])
          
    (: Compensate for methods that lack a visibility keyword: They are protected. :)
    let $protectedMethods := $protectedMethods + ($allMethods - $privateMethods - $protectedMethods - $publicMethods - $internalMethods)  
      
    return <Result Artifact='{$c/@Artifact}' PrivateMethodCount='{$privateMethods}' ProtectedMethodCount='{$protectedMethods}' PublicMethodCount='{$publicMethods}' InternalMethodCount='{$internalMethods}'
         PrivateFieldCount='{$privateFields}' ProtectedFieldCount='{$protectedFields}' PublicFieldCount='{$publicFields}' InternalFieldCount='{$internalFields}'/>
}
</Results>
return $results