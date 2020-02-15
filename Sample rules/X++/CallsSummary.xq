(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: @Language Xpp :)
(: @Category Informational :)

let $r := <Calls>
{
  for $c in /Class
  for $m in $c/Method
  
  for $call in ($m//(StaticMethodCall | QualifiedCall/SimpleQualifier))
    return
         if (fn:local-name($call) ='StaticMethodCall') then
             <Call FromClass='{$c/@Name}' FromMethod='{$m/@Name}' 
                   ClassName='{$call/@ClassName}' MethodName= '{$call/@MethodName}'
                   StartLine='{$call/@StartLine}' StartCol='{$call/@StartCol}' EndLine='{$call/@EndLine}' EndCol='{$call/@EndCol}'/>
         else (: Instance call :)
             <Call FromClass='{$c/@Name}' FromMethod='{$m/@Name}'  
                   ClassName='{$call/@Type}' MethodName='{$call/../@MethodName}'
                   StartLine='{$call/@StartLine}' StartCol='{$call/@StartCol}' EndLine='{$call/@EndLine}' EndCol='{$call/@EndCol}'/>
}
</Calls>

let $orderedCalls := <CallsSummary>
{
    for $c in $r/Call
    group by $fc := $c/@FromClass
    return <Class Name='{$fc}'>
    {
      for $m in $c 
      group by $fm := $m/@FromMethod
      return <FromMethod Name='{$fm}' >
      {
         for $tcc in $m
         group by $tc := $tcc/@ClassName
         return <ToClass Name='{$tc}'>
         {
            for $tmc in $tcc
            group by $tm := $tmc/@MethodName
            return <ToMethod Name='{$tm}' Count='{count($tmc)}'/>
         }
         </ToClass>
      }
      </FromMethod>      
    }
    </Class>
}
</CallsSummary>

return $orderedCalls