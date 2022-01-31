declare function local:GetDerived($classes, $base as xs:string*)
{
  for $c in $base
  return (for $d in $classes/Class[lower-case(@Extends) = $c] return $d/@Name)
};

let $classes := <Classes>{
  for $c in /Class[@Extends != ""] 
  return <Class Name='{lower-case($c/@Name)}' Extends='{lower-case($c/@Extends)}'/>
}</Classes>

let $seed := ('runbasebatch')

let $r1 := local:GetDerived($classes, $seed)
let $r2 := local:GetDerived($classes, $r1)
let $r3 := local:GetDerived($classes, $r2)
let $r4 := local:GetDerived($classes, $r3)
let $r5 := local:GetDerived($classes, $r4)
let $r6 := local:GetDerived($classes, $r5)

return <Results>
{
  for $c in ($r1, $r2, $r3, $r4, $r5) 
  let $cn := $classes/Class[@Name=$c]
  return <Result Name='{$c}' Extends='{$cn/@Extends}'/>
}
 </Results>

(:
return ($r1, "111111111111", $r2, "2222222222", $r3, "3333333333333", $r4, "444444444444", $r5, "5555555555", $r6)
:)