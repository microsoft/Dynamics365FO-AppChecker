(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Provide a breakdown of the distribution of the lines of code in X++ artifacts :)
(: @Language Xpp :)
(: @Category Informational :)

let $ms := <ClassSizes>
{
  for $a in /(Class | Table | Form | Query)
  return <ClassSize LineCount='{$a/@EndLine}'/>
}</ClassSizes>

(: Split the numbers in 200 slots compute the count within each slot :)
let $min := min($ms/ClassSize/number(@LineCount))
let $max := max($ms/ClassSize/number(@LineCount))
let $steps := 2000
let $increment := ($max - $min) div $steps
let $increments := (for $i in 1 to $steps return ($i - 1)*$increment)

return <ClassSizes ChunkSize='{$increment}'>
{
  for $q in 2 to $steps 
  return <Sizes Size='{$increments[$q]}' Count='{sum(for $t in $ms/* 
             where number($t/@LineCount) < $increments[$q] 
               and number($t/@LineCount) > $increments[$q - 1] return 1)}' />
}
</ClassSizes>
