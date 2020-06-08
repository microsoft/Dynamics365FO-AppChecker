(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Extract Fields (Members) in classes in CSV format :)

let $options := map { 'lax': false(), 'header': true() }

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
        <Artifact>{lower-case($a/@Artifact)}</Artifact>
        <Member>{lower-case($m/@Name)}</Member>
        <Type>{lower-case($m/@Type)}</Type>
        <Visibility>{string($visibility)}</Visibility>
     </Record>
}
</FieldsOnClasses>

return csv:serialize($r, $options)
