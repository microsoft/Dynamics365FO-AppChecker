(: Identify methods that contain true or false literals in an And or Or expression :)
(: @Language Xpp :)
(: @Category Mandatory :)
(: @Author pvillads@microsoft.com :)

<Diagnostics Category='Mandatory' href='docs.microsoft.com/Socratex/BooleanWithLiterals' Version='1.0'>
{
    for $c in /Class | /Table | /Form | /Query
    for $m in $c//Method
    let $exprs := $m//OrExpression/BooleanLiteralExpression
                | $m//AndExpression/BooleanLiteralExpression
    where $exprs
    order by $m/@Name ascending
    let $typeNamePair := fn:tokenize($c/@Artifact, ":")
    return <Diagnostic>
      <Moniker>BooleanExpressionWithLiteral</Moniker>
      <Severity>Error</Severity>
      <Path>dynamics://{$typeNamePair[1]}/{$typeNamePair[2]}/Method/{string($m/@Name)}</Path>
      <Message>The &amp;&amp; and || operators are used on the boolean literals true and false. Remove as appropriate while keeping expression semantics. For instance: true &amp;&amp; expression should be just expression</Message>
      <DiagnosticType>AppChecker</DiagnosticType>
      <Line>{string($exprs/@StartLine)}</Line>
      <Column>{string($exprs/@StartCol)}</Column>
      <EndLine>{string($exprs/@EndLine)}</EndLine>
      <EndColumn>{string($exprs/@EndCol)}</EndColumn>
    </Diagnostic>
}
</Diagnostics>
