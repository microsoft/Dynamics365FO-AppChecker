(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: List classes with the number of derived classes :)
(: @Category Informational :)
(: @Language Xpp :)

<DerivedClassCounts>
{
    for $c in /Class
    let $count := count(/Class[@Extends=$c/@Name])
    where $count != 0
    order by $count descending
    return <DerivedClasses Name='{$c/@Name}' Count='{$count}' />
}
</DerivedClassCounts>
