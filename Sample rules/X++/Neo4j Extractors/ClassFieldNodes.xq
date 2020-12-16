(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Export Class Fields (Members) in CSV format :)

let $options := map { 'lax': false(), 'header': true(), 'format': 'attributes'  }

let $r := <FieldsOnClasses>
{
    for $a in /Class
    for $m in $a/FieldDeclaration
    let $visibility := if (lower-case($m/@IsPrivate) = 'true') then "private"
                  else if (lower-case($m/@IsProtected) = 'true') then 'protected'
                  else if (lower-case($m/@IsPublic) = 'true') then "public"
                  else if (lower-case($m/@IsInternal) = 'true') then "internal"
                  else "protected"
    return <Record>
        <Artifact name='Artifact:ID'>{ lower-case("/" || $a/@Package || "/classes/" || $a/@Name || "/fields/" || $m/@Name) }</Artifact>
        <Package name='Package'>{lower-case($a/@Package)}</Package>
        <Member name="Name">{lower-case($m/@Name)}</Member>
        <Kind name="Kind">ClassMember</Kind>
        <Type name='Type'>{lower-case($m/@Type)}</Type>
        <Visibility name='Visibility'>{string($visibility)}</Visibility>
        <IsStatic name='IsStatic:boolean'>{string(lower-case($m/@IsStatic))}</IsStatic>
        <StartLine name='StartLine:int'>{xs:integer($m/@StartLine)}</StartLine>
        <StartCol name='StartCol:int'>{xs:integer($m/@StartCol)}</StartCol>
        <EndLine name='EndLine:int'>{xs:integer($m/@EndLine)}</EndLine>
        <EndCol name='EndCol:int'>{xs:integer($m/@EndCol)}</EndCol>
        <Label name=':LABEL'>ClassMember</Label>
     </Record>
}
</FieldsOnClasses>

return csv:serialize($r, $options)
