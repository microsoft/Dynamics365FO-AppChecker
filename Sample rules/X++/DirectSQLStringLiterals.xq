(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Find references to string literals that are likely to contain
Direct SQL text. :)

(: @Language Xpp :)
(: @Category Informational :)


<SqlStringLiterals>
{
  for $a in /*
  for $sl in $a//StringLiteralExpression
  where contains(lower-case($sl/@Value), "select " ) 
     or contains(lower-case($sl/@Value), "update " )
     or contains(lower-case($sl/@Value), "insert " )
     or contains(lower-case($sl/@Value), "delete " )
  order by string-length($sl/@Value) descending
  return <Literal Artifact='{$a/@Artifact}' Package='{$a/@Package}' 
        StartLine='{$sl/@StartLine}' EndLine='{$sl/@EndLine}' StartCol='{$sl/@StartCol}' EndCol='{$sl/@EndCol}'
        Value='{$sl/@Value}' >
  </Literal>
  
}
</SqlStringLiterals>
