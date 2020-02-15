(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Find all empty statements :)
(: @Category Informational :)
(: @Language C# :)

<EmptyStatements>
{
      for $c in //ClassDeclaration
      for $m in $c/MethodDeclaration
      for $e in $m//Block
      where not ($e/*)
      return <EmptyStatement Artifact='{$c/@Artifact}' Language="C#" Method='{$m/@Name}'
            StartLine='{$e/@StartLine}' StartCol='{$e/@StartCol}'
            EndLine='{$e/@EndLine}' EndCol='{$e/@EndCol}'/>
}
</EmptyStatements>
