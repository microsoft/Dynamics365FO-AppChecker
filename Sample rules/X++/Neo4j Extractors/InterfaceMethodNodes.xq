(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Export interface method information in CSV format :)

let $options := map { 'lax': false(), 'header': true(), 'format': 'attributes'  }

let $r := <MethodsOnInterfaces>
{
    for $a in /Interface
    for $m in $a/Method
    return <Record>
        <Artifact name='Artifact:ID'>{ lower-case("/" || $a/@Package || "/interfaces/" || $a/@Name || "/methods/" || $m/@Name) }</Artifact>
        <Package name='Package'>{lower-case($a/@Package)}</Package>
        <Method name='Name'>{lower-case($m/@Name)}</Method>
        <Kind name="Kind">Method</Kind>
        <StartLine name='StartLine:int'>{xs:integer($m/@StartLine)}</StartLine>
        <StartCol name='StartCol:int'>{xs:integer($m/@StartCol)}</StartCol>
        <EndLine name='EndLine:int'>{xs:integer($m/@EndLine)}</EndLine>
        <EndCol name='EndCol:int'>{xs:integer($m/@EndCol)}</EndCol>
        <Label name=':LABEL'>Method</Label>
     </Record>
}
</MethodsOnInterfaces>

return csv:serialize($r, $options)
