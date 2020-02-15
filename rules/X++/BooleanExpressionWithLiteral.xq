(: Identify methods that contain true or false literals in an And or Or expression :)
(: @Language Xpp :)
(: @Category Mandatory :)


<Diagnostics Category='Mandatory' href='docs.microsoft.com/Socratex/BooleanWithLiterals' Version='1.0'>
{
    for $c in /*
    for $m in $c//Method
    let $exprs := $m//OrExpression/BooleanLiteralExpression
                | $m//AndExpression/BooleanLiteralExpression
    where $exprs
    return
      <Diagnostic>
        <Moniker>BooleanExpressionWithLiteral</Moniker>
        <Severity>Error</Severity>
        <Path>{string($c/@PathPrefix)}/Method/{string($m/@Name)}</Path>
        <Message>Logical operators are applied to boolean literals. Remove as appropriate while keeping expression semantics.</Message>
        <DiagnosticType>AppChecker</DiagnosticType>
        <Line>{string($exprs[1]/@StartLine)}</Line>
        <Column>{string($exprs[1]/@StartCol)}</Column>
        <EndLine>{string($exprs[1]/@EndLine)}</EndLine>
        <EndColumn>{string($exprs[1]/@EndCol)}</EndColumn>
      </Diagnostic>
}
</Diagnostics>
