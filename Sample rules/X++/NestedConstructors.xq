declare namespace functx = "http://www.functx.com";
declare function functx:depth-of-node
  ( $node as node()? )  as xs:integer {

   count($node/ancestor-or-self::node())
 } ;

<Constructors>
{
    for $c in /Class
    for $m in $c/Method[@IsConstructor="true"]
    for $super in $m//ExpressionStatement/SuperCall
    let $depth := functx:depth-of-node($super)
    where $depth > 5
    return <Constructor Artifact='{$c/@Artifact}' Depth='{$depth}' Package='{$c/@Package}'
        StartLine='{$super/@StartLine}' StartCol='{$super/@StartCol}'
        EndLine='{$super/@EndLine}' EndCol='{$super/@EndCol}' />
}
</Constructors>