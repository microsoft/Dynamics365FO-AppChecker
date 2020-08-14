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
    for $datasource in $a/FormDataSource
    for $m in $datasource/Method

    return <Record>
        <From name=":START_ID">{lower-case("/" || $a/@Package || "/forms/" || $a/@Name || "/DataSources/" || $datasource/@Name)}</From>
        <To name=":END_ID">{lower-case("/" || $a/@Package || "/forms/" || $a/@Name || "/DataSources/" || $datasource/@Name || "/methods/" || $m/@Name)}</To>
        <Label name=':TYPE'>DECLARES</Label>
     </Record>
}
</FormsDatasources>

return csv:serialize($r, $options)
