(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Extract package dependencies in CSV format :)

let $options := map { 'lax': false(), 'header': true() }

let $r := <Packages>
{
    for $m in /Models/Model
    for $ref in $m/ModuleReferences/ModuleReference
    return <Record>
       <Package>{lower-case($m/@Name)}</Package>
       <References>{lower-case($ref/@Name)}</References>
    </Record>
}
</Packages>

return csv:serialize($r, $options)
