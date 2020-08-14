(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Names of all class methods that are pre handlers. :)
(: @Category Informational :)
(: @Language Xpp :)

let $options := map { 'lax': false(), 'header': true(), 'format': 'attributes' }

let $handlers := <PostHandlers>
{
    for $c in /Class[lower-case(@Package)='applicationsuite']
    for $m in $c//Method
    for $handler in $m/AttributeList/Attribute[lower-case(@Name)='posthandlerfor']
    let $arg1 := $handler/AttributeExpression[1]/IntrinsicAttributeLiteral
    let $arg2 := $handler/AttributeExpression[2]/IntrinsicAttributeLiteral
    let $kind := switch (lower-case($arg1/@FunctionName)) 
       case "formstr" return ("forms", /Form[lower-case(@Name)=lower-case($arg1/@Arg1)]/@Package)
       case "classstr" return ("classes", /Class[lower-case(@Name)=lower-case($arg1/@Arg1)]/@Package)
       case "tablestr" return ("tables", /Table[lower-case(@Name)=lower-case($arg1/@Arg1)]/@Package)
       case "querystr" return ("queries", /Query[lower-case(@Name)=lower-case($arg1/@Arg1)]/@Package)
       case "viewstr" return ("views", /Query[lower-case(@Name)=lower-case($arg1/@Arg1)]/@Package)
       case "mapstr" return ("maps", /Map[lower-case(@Name)=lower-case($arg1/@Arg1)]/@Package)       
       case "dataentityviewstr" return ("dataentityviews", /Entity[lower-case(@Name)=lower-case($arg1/@Arg1)]/@Package)
       default return "?" 
   
    return <Record>
        <Handler name=':START_ID'>{ lower-case("/" || $c/@Package || "/classes/" || $c/@Name || "/methods/" || $m/@Name) }</Handler>
        <Artifact name=':END_ID'>{ lower-case("/" || $kind[2] || "/" || $kind[1] || "/" || $arg1/@Arg1 || "/methods/" || $arg2/@Arg2)}</Artifact>
        <Type name=':TYPE'>POSTHANDLERFOR</Type>
    </Record>
}
</PostHandlers>

return csv:serialize($handlers, $options)
