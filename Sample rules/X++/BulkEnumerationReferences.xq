(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

declare function local:SourceLines($s as xs:string, $fromLine as xs:integer, $toLine as xs:integer) as xs:string
{

    let $lines := tokenize($s, '(\r\n?|\n\r?)') 
    return string-join($lines[position() >= max((1, $fromLine - 3)) 
                          and position() <= min((count($lines), $toLine + 3)) ], '&#10;' )
};
 
(:
Add the enumeration types and the literals that you are interested in here. 
:)

let $allEnumerations := (
<Enumeration Name='NoYes'>
    <Literal Name="Yes" /> (: This is 0. The next is 16, unlilkely to be a problem :)
</Enumeration>, 

<Enumeration Name='LedgerPostingType'>
    <Literal Name="None" />
    <Literal Name="ExchRateGain" />
    <Literal Name="ExchRateLoss" />
    <Literal Name="InterCompany" />
    <Literal Name="Tax" />
    <Literal Name="VATRoundOff" />
    <Literal Name="Allocation" />
    <Literal Name="InvestmentDuty" />
    <Literal Name="Liquidity" />
    <Literal Name="MSTDiffSecond" />
    <Literal Name="ErrorAccount" />
    <Literal Name="MSTDiff" />
</Enumeration>
)

return <Enumerations>
{
    for $enum in $allEnumerations
    let $enumName := concat('enumeration(', lower-case($enum/@Name), ')')
    
    for $a in /(Form | Table | Class | Query)
    for $qste in $a//QualifiedStaticFieldExpression[lower-case(@IsEnum) = 'true']
    where exists($enum/Literal[lower-case(@Name)=lower-case($qste/@Name)])
    for $sq in $qste//StaticQualifier[contains(lower-case(@Type), $enumName)]
    order by $a/@Package, $sq/@Name

    return <EnumerationReference Artifact='{$a/@Artifact}' Package='{$a/@Package}'  Type='{$sq/@Name}' Literal='{$qste/@Name}'
        StartLine='{$qste/@StartLine}' EndLine='{$qste/@EndLine}'
        StartCol='{$qste/@StartCol}' EndCol='{$qste/@EndCol}' >
        {
            concat('&#10;', local:SourceLines($a/@Source, $qste/@StartLine, $qste/@EndLine), '&#10;')
        }
    </EnumerationReference>
}
</Enumerations>