(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: @Category Mandatory :)
(: @Language Xpp :)
(: @Author pvillads@microsoft.com :)

declare function local:findParmReferences($d, $i as xs:integer, $s as xs:string)
{
    (: Allow for 19 expansion parameters :)
    if ($i < 20) then
        if (contains($s, concat("%", $i))) then
            local:findParmReferences(($d, $i), $i+1, $s)
        else 
            local:findParmReferences($d, $i+1, $s)   
    else
        $d
};

declare function local:findUniqueParmReferences($s as xs:string)
{
    let $seq := local:findParmReferences((), 1, $s)
    return distinct-values($seq)
};

declare function local:stripQuotes($s as xs:string) as xs:string
{
    let $s1 := substring($s, 2)
    return substring($s1, 1, string-length($s1)-1)
};

declare function local:getParts($s as xs:string)
{
    let $stripped := local:stripQuotes($s)
    return if (contains($stripped, ":")) then
       (substring-before($stripped, ":"), substring-after($stripped, ":"))
    else
       ("", $stripped)
};

<Classes>
{
    for $c in /* 
    for $m in $c//Method 
    for $strfmt in $m//FunctionCall[@FunctionName='strfmt']
    let $formatString := $strfmt/*[1]
    where local-name($formatString) = "StringLiteralExpression" and $formatString[@IsLabel="True"] 
    
    (: Find the strings templates. For non-labels, this is just the 
       literal string. For labels, look up the matching labels :)
    let $strings := if ($formatString[@IsLabel='False']) then 
        ( $formatString/@Value ) (: Pattern is provided in code :)
    else (
        let $labelParts := local:getParts($formatString/@Value)
        for $label in /Labels/Label[@Symbol=$labelParts[2]] return $label/@Value
    )
        
    for $str in $strings
    let $parmReferences := local:findUniqueParmReferences($str)
    
    where count($parmReferences) != count($strfmt/*)-1     
    
    return <Res Artifact='{$c/@Artifact}' Label='{$formatString/@IsLabel}' 
             StartLine='{$formatString/@StartLine}' StartCol='{$formatString/@StartCol}'
            EndLine='{$formatString/@EndLine}' EndCol='{$formatString/@EndCol}'>
            { $str}
            </Res>
    
}                    
</Classes>
