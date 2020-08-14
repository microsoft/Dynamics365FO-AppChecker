(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Export View field information :)

let $options := map { 'lax': false(), 'header': true(), 'format': 'attributes' }

let $r := <ViewFields>
{
    for $t in /View
    for $f in $t/Metadata/Fields/AxViewField
    let $mandatory := if ($f/Mandatory) then "true" else "false"
    let $visible := if ($f/Visible) then "true" else "false"
    let $edt := if ($f/ExtendedDataType) then $f/ExtendedDataType else ""
    let $label := if ($f/Label) then $f/Label else ""

    return <Record>
        <Artifact name='Artifact:ID'>{ lower-case("/" || $t/@Package || "/views/" || $t/@Name || "/fields/" || $f/Name) }</Artifact>
        <Package name='Package'>{lower-case($t/@Package)}</Package>
        <Kind name='Kind'>ViewField</Kind>        
        <ViewName name='View'>{lower-case($t/@Name)}</ViewName>
        <Field name="Name">{lower-case($f/@Name)}</Field>
        <Type name='Type'>{lower-case($f/@Q{http://www.w3.org/2001/XMLSchema-instance}type)}</Type>
        <Visibility name='Visible:Boolean'>{string($visible)}</Visibility>
        <Mandatory name='Mandatory:Boolean'>{$mandatory}</Mandatory>
        <ViewLabel name='Label'>{lower-case($label)}</ViewLabel>
        <ExtendedDataType name='EDT'>{lower-case($edt)}</ExtendedDataType>
        <Label name=':LABEL'>ViewField</Label>
     </Record>
}
</ViewFields>

return csv:serialize($r, $options)