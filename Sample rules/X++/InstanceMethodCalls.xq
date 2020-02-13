(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Find calls to instannce methods on a type :)
(: @Category Informational :)
(: @Language Xpp :)
(: @Author pvillads@microsoft.com :)

<Results>
{
  let $type := 'TextBuffer'

  for $c in /*
  for $x in $c//QualifiedCall/SimpleQualifier[@Type=$type]
    let $qcall := $x/..
    let $qualifier := $x
    
    return <Result Artifact='{$c/@Artifact}' MethodName='{$qcall/@MethodName}' Type='{$qualifier/@Type}'
            StartLine='{$x/@StartLine}' EndLine='{$x/@EndLine}' 
            StartCol='{$x/@StartCol}' EndCol='{$x/@EndCol}'/>
}
</Results>