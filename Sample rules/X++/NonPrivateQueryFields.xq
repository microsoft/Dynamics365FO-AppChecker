(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Find references to query fields that are not private. Query fields
   cannot be addressed from outside, since it is not possible to derive
   from queries ir access query fields from outside the query. 
:)
(: @Language Xpp :)
(: @Category Informational :)

<NonPrivateQueryFields>
{
    for $query in /Query
    for $field in $query/Class/FieldDeclaration[@IsPrivate='false']
    return <NonPrivateQueryFields Field='{$field/@Name}'  Artifact='{$query/@Artifact}' 
                StartLine='{$field/@StartLine}' EndLine='{$field/@EndLine}'
                StarCol='{$field/@StartCol}' EndCol='{$field/@EndCol}'/> 
}
</NonPrivateQueryFields>