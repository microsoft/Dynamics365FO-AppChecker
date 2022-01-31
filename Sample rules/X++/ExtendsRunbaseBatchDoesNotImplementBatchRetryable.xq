declare namespace functx = "http://www.functx.com";
declare function functx:is-value-in-sequence
  ( $value as xs:anyAtomicType? ,
    $seq as xs:anyAtomicType* )  as xs:boolean {

   $value = $seq
 };

<ExtendsRunbaseBatchDoesNotImplementBatchRetryable>
{
  for $c in /Class
  let $implements := (for $i in $c/Implements return lower-case($i/text()))
  where $c[lower-case(@Extends) = 'runbasebatch']
  where not(functx:is-value-in-sequence('batchretryable', $implements))
  return <Result Artifact='{$c/@Name}'/>
}
</ExtendsRunbaseBatchDoesNotImplementBatchRetryable>
