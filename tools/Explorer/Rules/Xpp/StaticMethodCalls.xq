(: Find calls to static methods on a type :)
<Results>
{
  for $c in /*
  for $x in $c//StaticMethodCall
    
    return <Result Artifact='{$c/@Artifact}' MethodName='{$x/@MethodName}' Type='{$x/@ClassName}'
            StartLine='{$x/@StartLine}' EndLine='{$x/@EndLine}' 
            StartCol='{$x/@StartCol}' EndCol='{$x/@EndCol}'/>
}
</Results>