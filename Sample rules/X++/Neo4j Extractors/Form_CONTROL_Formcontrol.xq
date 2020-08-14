(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Export form information in CSV format. :)

(: @Category Informational :)
(: @Language Xpp :)
(: @Author pvillads@microsoft.com :)

let $options := map { 'lax': false(), 'header': true(), 'format': 'attributes' }

let $r := <FormsDatasources>
{
    for $a in /Form
    for $control in $a/FormControl

    return <Record>
        <From name=":START_ID">{lower-case("/" || $a/@Package || "/forms/" || $a/@Name)}</From>
        <To name=":END_ID">{lower-case("/" || $a/@Package || "/forms/" || $a/@Name || "/controls/" || $control/@Name)}</To>
        <Label name=':TYPE'>CONTROL</Label>
     </Record>
}
</FormsDatasources>

return csv:serialize($r, $options)
