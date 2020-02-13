(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Identify recursive methods :)
return <RecursiveMethods>
{
  for $a in /(Class | Table | Query | Form)
  return (
    for $m in $a//Method[lower-case(@IsStatic)='true']
      for $call in $m//StaticMethodCall
      where $call[@ClassName=$a/@Name and @MethodName=$m/@Name]
      return
         <RecursiveStatic Artifact='{$a/@Artifact}' FromClass='{$a/@Name}' FromMethod='{$m/@Name}' 
                   ClassName='{$call/@ClassName}' MethodName= '{$call/@MethodName}'
                   StartLine='{$call/@StartLine}' StartCol='{$call/@StartCol}' EndLine='{$call/@EndLine}' EndCol='{$call/@EndCol}'/>
   , 
    for $m in $a//Method[lower-case(@IsStatic) !='true']
      for $call in $m//QualifiedCall/SimpleQualifier
      where $call[@Type=$a/@Name and $m/@Name = $call/../@MethodName]
      return
         <RecursiveInstance Artifact='{$a/@Artifact}' FromClass='{$a/@Name}' FromMethod='{$m/@Name}'  
                   ToClass='{$call/@Type}' ToMethod='{$call/../@MethodName}'
                   StartLine='{$call/@StartLine}' StartCol='{$call/@StartCol}' EndLine='{$call/@EndLine}' EndCol='{$call/@EndCol}'/>
  )  
}
</RecursiveMethods>