(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Provide some rudimentary statistics on the current application :)
(: @Language Xpp :)
(: @Category Informational :)

<Statistics>
  <Classes Count='{count(/Class)}'>
    <Methods Count='{count(/Class/Method)}'/>
    <LOC Count='{sum(for $c in /Class return $c/@EndLine)}'/>
  </Classes>
  <Tables Count='{count(/Table)}'>
    <Methods Count='{count(/Table/Method)}'/>
    <LOC Count='{sum(for $c in /Table/Method return $c/@EndLine - $c/@StartLine + 1)}'/>
    <Fields Count='{count(/Table/Metadata/Fields/AxTableField)}'/>
  </Tables>
  <Queries Count='{count(/Query)}'>
    <Methods Count='{count(/Query/Class/Method)}'/>
    <LOC Count='{sum(for $c in /Query/Class return $c/@EndLine)}'/>
  </Queries>
  <Views Count='{count(/View)}'>
    <Methods Count='{count(/View/Method)}'/>
    <LOC Count='{sum(for $c in /View/Method return $c/@EndLine - $c/@StartLine + 1)}'/>
    <Fields Count='{count(/View/Metadata/Fields/AxViewField)}'/>
  </Views>
  <Maps Count='{count(/Map)}'>
    <Methods Count='{count(/Map/Method)}'/>
    <LOC Count='{sum(for $c in /Map/Method return $c/@EndLine - $c/@StartLine + 1)}'/>
    <Fields Count='{count(/Map/Metadata/Fields/AxMapBaseField)}'/>
  </Maps>
  <Forms Count='{count(/Form)}'>
    <Methods Count='{count(/Form//Method)}'/>
    <LOC Count='{sum(for $c in /Form/Class return $c/@EndLine)}'/>
  </Forms>
</Statistics>
