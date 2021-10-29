declare namespace functx = "http://www.functx.com";
declare function functx:is-value-in-sequence
  ( $value as xs:anyAtomicType? ,
    $seq as xs:anyAtomicType* )  as xs:boolean {

   $value = $seq
 };

<ImplementBatchRetryableWithFalse>
{
  for $c in /Class
  let $implements := (for $i in $c/Implements return lower-case($i/text()))
  where functx:is-value-in-sequence('batchretryable', $implements)
  let $retryable := $c/Method[lower-case(@Name) = 'isretryable']
  where ($retryable//ReturnStatement/BooleanLiteralExpression[@Value="true"])
  return <Result Artifact='{$c/@Artifact}' 
    StartLine='{$retryable/@StartLine}' EndLine='{$retryable/@EndLine}'
    StartCol='{$retryable/@StartCol}' EndCol='{$retryable/@EndCol}' />
}
</ImplementBatchRetryableWithFalse>
