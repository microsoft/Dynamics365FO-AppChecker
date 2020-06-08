(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

let $options := map { 'lax': false(), 'header': true() }

let $r := <Calls>
{
  for $c in /(Class | Table | Query | Form | View)
  for $m in $c//Method

  for $call in ($m//(StaticMethodCall | QualifiedCall/(SimpleQualifier | ExpressionQualifier)))
    return
         if (fn:local-name($call) ='StaticMethodCall') then
             <Call FromClass='{$c/@Name}' FromMethod='{$m/@Name}'
                   ClassName='{$call/@ClassName}' MethodName= '{$call/@MethodName}' />
         else (: Instance call :)
             <Call FromClass='{$c/@Name}' FromMethod='{$m/@Name}'
                   ClassName='{$call/@Type}' MethodName='{$call/../@MethodName}' />
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


let $records := <Record>
{
  for $c in $orderedCalls//Class
  for $fm in $c/FromMethod
  for $tc in $fm/ToClass
  for $tm in $tc/ToMethod
  return <Record>
     <FromClass>{lower-case($c/@Name)}</FromClass>
     <FromMethod>{lower-case($fm/@Name)}</FromMethod>
     <ToClass>{lower-case($tc/@Name)}</ToClass>
     <ToMethod>{lower-case($tm/@Name)}</ToMethod>
     <Count>{string($tm/@Count)}</Count>
   </Record>
}
</Record>

return csv:serialize($records, $options)
