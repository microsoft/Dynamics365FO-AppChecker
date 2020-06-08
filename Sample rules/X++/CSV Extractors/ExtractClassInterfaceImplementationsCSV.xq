(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Extract information about interfaces inmplemented by classes in CSV format :)

let $options := map { 'lax': false(), 'header': true() }

let $r := <ClassImplementations>
{
  for $c in /Class
  for $i in $c/Implements
  return <Record>
     <Artifact>{lower-case($c/@Artifact)}</Artifact>
     <Implements>{lower-case($i/text())}</Implements>
  </Record>
}
</ClassImplementations>

return csv:serialize($r, $options)
