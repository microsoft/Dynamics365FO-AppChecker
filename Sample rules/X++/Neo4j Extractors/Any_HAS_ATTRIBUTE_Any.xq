(: Build a map with all the class names :)

(: The argument is expected to be lower cased :)
declare function local:LookupAttributeNameAndPackage($classes, $attributeName as xs:string) 
{
     if (map:contains($classes, $attributeName)) then
         (map:get($classes, $attributeName), $attributeName)
     else if (map:contains($classes, $attributeName ||  "attribute")) then
         (map:get($classes, $attributeName ||  "attribute"),  $attributeName || "attribute")
     else ()
};

declare function local:AttributeValuesString($attr)
{
  string-join(for $arg in $attr/AttributeExpression/*
                  return if ($arg/@FunctionName) then $arg/@FunctionName || "(" || 
                    (if ($arg/@Arg1) 
                     then $arg/@Arg1 || (if ($arg/@Arg2 != '(null)')
                                        then "," ||  $arg/@Arg2 || (if ($arg/@Arg3 != '(null)')
                                                                    then ("," || $arg/@Arg3) 
                                                                    else "")
                                        else "")
                     else "") || ")"
                  else $arg/@Value, ";")
};

(: Map from Class name onto package name :)
let $classes := map:merge(
    for $c in /Class
    return map:entry(lower-case($c/@Name), (lower-case($c/@Package), lower-case($c/@Name)))
)

let $options := map { 'lax': false(), 'header': true(), 'format': 'attributes' }

let $r := <Attributes>
{
  (
    (: Toplevel artifacts :)
    for $c in /Class
    for $attr in $c/AttributeList/Attribute

    let $pair := local:LookupAttributeNameAndPackage($classes,  lower-case($attr/@Name))
                     
    where $pair[1] and $pair[2]
    return <Record>
        <From name=':START_ID'>{ lower-case("/" || $c/@Package || "/classes/" || $c/@Name) }</From>
        <To name=':END_ID'>{ lower-case("/" || $pair[1] || "/classes/" || string($pair[2])) }</To>
        <Args name='Args:string[]'>{local:AttributeValuesString($attr)}</Args>
        <Type name=':TYPE'>HAS_ATTRIBUTE</Type>
    </Record>
    ,

    for $c in /Form/Class
    for $attr in $c/AttributeList/Attribute

    let $pair := local:LookupAttributeNameAndPackage($classes,  lower-case($attr/@Name))
    where $pair[1] and $pair[2]
    return <Record>
        <From name=':START_ID'>{ lower-case("/" || $c/../@Package || "/forms/" || $c/../@Name) }</From>
        <To name=':END_ID'>{ lower-case("/" || $pair[1] || "/classes/" || string($pair[2])) }</To>
        <Args name='Args:string[]'>{local:AttributeValuesString($attr)}</Args>
        <Type name=':TYPE'>HAS_ATTRIBUTE</Type>
    </Record>
    ,
    for $c in /Query/Class
    for $attr in $c/AttributeList/Attribute

    let $pair := local:LookupAttributeNameAndPackage($classes,  lower-case($attr/@Name))
    where $pair[1] and $pair[2]
    return <Record>
        <From name=':START_ID'>{ lower-case("/" || $c/../@Package || "/queries/" || $c/../@Name) }</From>
        <To name=':END_ID'>{ lower-case("/" || $pair[1] || "/classes/" || string($pair[2])) }</To>
        <Args name='Args:string[]'>{local:AttributeValuesString($attr)}</Args>
        <Type name=':TYPE'>HAS_ATTRIBUTE</Type>
    </Record>
    ,
    for $c in /Map
    for $attr in $c/AttributeList/Attribute

    let $pair := local:LookupAttributeNameAndPackage($classes,  lower-case($attr/@Name))
    where $pair[1] and $pair[2]
    return <Record>
        <From name=':START_ID'>{ lower-case("/" || $c/../@Package || "/maps/" || $c/../@Name) }</From>
        <To name=':END_ID'>{ lower-case("/" || $pair[1] || "/classes/" || string($pair[2])) }</To>
        <Args name='Args:string[]'>{local:AttributeValuesString($attr)}</Args>
        <Type name=':TYPE'>HAS_ATTRIBUTE</Type>
    </Record>
    ,
    for $c in /View
    for $attr in $c/AttributeList/Attribute

    let $pair := local:LookupAttributeNameAndPackage($classes,  lower-case($attr/@Name))
    where $pair[1] and $pair[2]
    return <Record>
        <From name=':START_ID'>{ lower-case("/" || $c/../@Package || "/views/" || $c/../@Name) }</From>
        <To name=':END_ID'>{ lower-case("/" || $pair[1] || "/classes/" || string($pair[2])) }</To>
        <Args name='Args:string[]'>{local:AttributeValuesString($attr)}</Args>        
        <Type name=':TYPE'>HAS_ATTRIBUTE</Type>
    </Record>
    ,
    (: Methods :)
    for $c in /Class
    for $m in $c/Method
    for $attr in $m/AttributeList/Attribute

    let $pair := local:LookupAttributeNameAndPackage($classes,  lower-case($attr/@Name))
    where $pair[1] and $pair[2]
    
    return <Record>
        <From name=':START_ID'>{ lower-case("/" || $c/@Package || "/classes/" || $c/@Name || "/methods/" || $m/@Name) }</From>
        <To name=':END_ID'>{ lower-case("/" || $pair[1] || "/classes/" || string($pair[2])) }</To>
        <Args name='Args:string[]'>{local:AttributeValuesString($attr)}</Args>        
        <Type name=':TYPE'>HAS_ATTRIBUTE</Type>
    </Record>
    ,
    for $c in /Table
    for $m in $c/Method
    for $attr in $m/AttributeList/Attribute

    let $pair := local:LookupAttributeNameAndPackage($classes,  lower-case($attr/@Name))
    where $pair[1] and $pair[2]
    
    return <Record>
        <From name=':START_ID'>{ lower-case("/" || $c/@Package || "/tables/" || $c/@Name || "/methods/" || $m/@Name) }</From>
        <To name=':END_ID'>{ lower-case("/" || $pair[1] || "/classes/" || string($pair[2])) }</To>
        <Args name='Args:string[]'>{local:AttributeValuesString($attr)}</Args>        
        <Type name=':TYPE'>HAS_ATTRIBUTE</Type>
    </Record>
    ,
    
    for $c in /View
    for $m in $c/Method
    for $attr in $m/AttributeList/Attribute

    let $pair := local:LookupAttributeNameAndPackage($classes,  lower-case($attr/@Name))
    where $pair[1] and $pair[2]
    
    return <Record>
        <From name=':START_ID'>{ lower-case("/" || $c/@Package || "/views/" || $c/@Name || "/methods/" || $m/@Name) }</From>
        <To name=':END_ID'>{ lower-case("/" || $pair[1] || "/classes/" || string($pair[2])) }</To>
        <Args name='Args:string[]'>{local:AttributeValuesString($attr)}</Args>        
        <Type name=':TYPE'>HAS_ATTRIBUTE</Type>
    </Record>
    ,

    for $c in /Map
    for $m in $c/Method
    for $attr in $m/AttributeList/Attribute

    let $pair := local:LookupAttributeNameAndPackage($classes,  lower-case($attr/@Name))
    where $pair[1] and $pair[2]
    
    return <Record>
        <From name=':START_ID'>{ lower-case("/" || $c/@Package || "/maps/" || $c/@Name || "/methods/" || $m/@Name) }</From>
        <To name=':END_ID'>{ lower-case("/" || $pair[1] || "/classes/" || string($pair[2])) }</To>
        <Args name='Args:string[]'>{local:AttributeValuesString($attr)}</Args>        
        <Type name=':TYPE'>HAS_ATTRIBUTE</Type>
    </Record>
    ,
      
    for $c in /Form
    for $m in $c/Class/Method
    for $attr in $m/AttributeList/Attribute

    let $pair := local:LookupAttributeNameAndPackage($classes,  lower-case($attr/@Name))
    where $pair[1] and $pair[2]
    
    return <Record>
        <From name=':START_ID'>{ lower-case("/" || $c/@Package || "/forms/" || $c/@Name || "/methods/" || $m/@Name) }</From>
        <To name=':END_ID'>{ lower-case("/" || $pair[1] || "/classes/" || string($pair[2])) }</To>
        <Args name='Args:string[]'>{local:AttributeValuesString($attr)}</Args>        
        <Type name=':TYPE'>HAS_ATTRIBUTE</Type>
    </Record>
    ,
     
    for $c in /Query
    for $m in $c/Class/Method
    for $attr in $m/AttributeList/Attribute

    let $pair := local:LookupAttributeNameAndPackage($classes,  lower-case($attr/@Name))
    where $pair[1] and $pair[2]
    
    return <Record>
        <From name=':START_ID'>{ lower-case("/" || $c/@Package || "/queries/" || $c/@Name || "/methods/" || $m/@Name) }</From>
        <To name=':END_ID'>{ lower-case("/" || $pair[1] || "/classes/" || string($pair[2])) }</To>
        <Args name='Args:string[]'>{local:AttributeValuesString($attr)}</Args>
        <Type name=':TYPE'>HAS_ATTRIBUTE</Type>
    </Record>     
 )    
}
</Attributes>

return csv:serialize($r, $options)