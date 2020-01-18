(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Extract information about attributes on toplevel artifacts as prolog clauses :)

(:~ The following functions are needed to traverse the attributes. The code is
    made more complicated by the fact that attributes can contain nested 
    container values. The code is duplicated elsewhere, so any changes must 
    be replacated where attributes are handled (in toplevel artifact, on 
    interfaces and on methods, and possibly fields. :)

declare function local:strip-quotes($s as xs:string) as xs:string
{
    if ((starts-with($s, "'") and ends-with($s, "'"))
    or (starts-with($s, '"') and ends-with($s, '"'))) then
        substring($s, 2, string-length($s)-2)
    else
        $s
};

declare function local:RenderAttributeLiteral($arg) as xs:string
{
    let $img := switch ($arg/local-name())
        case "StringAttributeLiteral" return '"' || local:strip-quotes($arg/@Value) || '"'
        case "BooleanAttributeLiteral" return lower-case($arg/@Value)
        case "IntAttributeLiteral" return lower-case($arg/@Value)
        case "RealAttributeLiteral" return $arg/@Value
        case "ContainerAttributeLiteral" return "[" || local:RenderAttributeArguments($arg) || "]"
        case "EnumAttributeLiteral" return "e('" || lower-case($arg/@TypeName) || "', '" || lower-case($arg/@Literal) || "')"
        case "IntrinsicAttributeLiteral" return lower-case($arg/@FunctionName) || "(" ||
                                            (if ($arg/@Arg1 != "(null)") then
                                                "'" || lower-case(local:strip-quotes($arg/@Arg1)) || "'" ||
                                                (if ($arg/@Arg2 != "(null)") then
                                                    ", '" || lower-case(local:strip-quotes($arg/@Arg2)) || "'" || 
                                                    (if ($arg/@Arg3 != "(null)") then
                                                        ", '" || lower-case(local:strip-quotes($arg/@Arg3)) || "'"
                                                    else "")
                                                else "")
                                            else "") || ")"
                                                  
        default return "****" || $arg/local-name()
    return $img
};

declare function local:RenderAttributeArgument($arg as element(AttributeExpression)) as xs:string
{
    let $l := (for $lit in $arg/* return local:RenderAttributeLiteral($lit))
    return string-join($l, ", ")
};

declare function local:RenderAttributeArguments($attr) as xs:string
{
    let $args := (
        for $arg in $attr/AttributeExpression
        return local:RenderAttributeArgument($arg)
    )
    return string-join($args, ", ")
};

(: The argument is either an attribute or a ContainerAttribute literal :)
(: The attribute suffix is removed by convention :)
declare function local:RenderAttribute($attr) as xs:string
{
    let $name := lower-case(if (ends-with(lower-case($attr/@Name), "attribute")) then 
      substring($attr/@Name, 1, string-length($attr/@Name) - 9)
    else $attr/@Name)
    
    return if ($attr/AttributeExpression) then
        "a('" || $name || "', "  || local:RenderAttributeArguments($attr) || ")"
    else
        "a('" || $name || "')"
};

let $packages := ('applicationfoundation', 'applicationplatform', '', 'applicationcommon',
        'directory', 'calendar', 'dimensions', 'currency',
        'unitofmeasure', 'measurement', 'sourcedocumentationtypes', 'sourcedocumentation',
        'ledger', 'electronicreportingdotnetutils', 'contactperson', 'datasharing',
        'policy', 'electronicreportingcore', 'banktypes', 'project', 
        'electronicreportingmapping', 'tax', 'subledger', 'personnelcore',
        'electronicreportingforax', 'businessprocess', 'casemanagement', 'generalledger',
        'electronicreporting', 'personnelmanagement', 'financialreporting', 'fiscalbooks',
        'taxengine', 'electronicreportingbusinessdoc', 'personnel', 'retail',
        'applicationsuite')

let $result := (
  for $a in /Class[lower-case(@Package)=$packages]
  order by $a/@Name
  for $attr in $a/AttributeList/Attribute
  return "attribute(c('" || lower-case($a/@Name) || "'), " || local:RenderAttribute($attr) ||  ").",


  for $a in /Table[lower-case(@Package)=$packages]
  order by $a/@Name
  for $attr in $a/AttributeList/Attribute
  return "attribute(t('" || lower-case($a/@Name) || "'), " || local:RenderAttribute($attr) ||  ").",
  
  for $a in /Form[lower-case(@Package)=$packages]
  order by $a/@Name
  for $attr in $a/Class/AttributeList/Attribute
  return "attribute(f('" || lower-case($a/@Name) || "'), " || local:RenderAttribute($attr) ||  ").",
  
  for $a in /Query[lower-case(@Package)=$packages]
  order by $a/@Name
  for $attr in $a/Class/AttributeList/Attribute
  return "attribute(q('" || lower-case($a/@Name) || "'), " || local:RenderAttribute($attr) ||  ")." 
)

return $result