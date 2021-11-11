(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: This query enumerates internal interfaces that are implemented in classes across
  package boundaries. :)
(: @Category Informational :)
(: @Language Xpp :)

let $internalInterfaces := <InternalInterfaces>
{
  for $i in /Interface
  where $i[@IsInternal='true']
  return <InternalInterface Artifact='{$i/@Artifact}' Name='{lower-case($i/@Name)}' Package='{$i/@Package}'/>
  }
</InternalInterfaces>

(: Find classes implementing the interfaces :)
let $classes := <Classes>
{
  for $c in /Class/Implements
  let $interfaceName := lower-case($c/text())
  for $i in $internalInterfaces/InternalInterface[@Name=$interfaceName]
  where lower-case($i/@Package) != lower-case($c/../@Package)
  return <T InterfaceName='{$interfaceName}' Artifact='{$c/../@Artifact}' InterfacePackage='{$i/@Package}' ClassPackage='{$c/../@Package}'>
  </T>
}
</Classes>

return ($classes)
