(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Extract View field information :)
let $options := map { 'lax': false(), 'header': true() }

let $r := <TableWithFields>
{
    for $t in /View
    for $f in $t/Metadata/Fields/AxViewField
    let $mandatory := if ($f/Mandatory) then "true" else "false"
    let $visible := if ($f/Visible) then "true" else "false"
    let $edt := if ($f/ExtendedDataType) then $f/ExtendedDataType else ""
    let $label := if ($f/Label) then $f/Label else ""

    return  <Record>
       <Package>{lower-case($t/@Package)}</Package>
       <ViewName>{lower-case($t/@Name)}</ViewName>
       <FieldName>{lower-case($f/Name)}</FieldName>
       <Type>{lower-case($f/@Q{http://www.w3.org/2001/XMLSchema-instance}type)}</Type>
       <Label>{lower-case($label)}</Label>
       <Mandatory>{lower-case($mandatory)}</Mandatory>
       <Visible>{lower-case($visible)}</Visible>
       <ExtendedDataType>{lower-case($edt)}</ExtendedDataType>
    </Record>
}
</TableWithFields>

return csv:serialize($r, $options)
