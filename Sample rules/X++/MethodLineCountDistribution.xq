(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Provide a breakdown of the distribution of the lines of code in X++ methods :)
(: @Language Xpp :)
(: @Category Informational :)

let $ms := <MethodSizes>
{
  for $a in /(Class | Table | Form | Query)
  for $m in $a//Method
  return <MethodSize LineCount='{$m/@EndLine}'/>
}</MethodSizes>

(: Split the numbers in 200 slots compute the count within each slot :)
let $min := min($ms/MethodSize/number(@LineCount))
let $max := max($ms/MethodSize/number(@LineCount))
let $steps := 1000
let $increment := ($max - $min) div $steps
let $increments := (for $i in 1 to $steps return ($i - 1)*$increment)

let $r := (for $q in 2 to $steps 
           return count(for $t in $ms/* 
                        where number($t/@LineCount) < $increments[$q] 
                          and number($t/@LineCount) > $increments[$q - 1] return 1))

return $r