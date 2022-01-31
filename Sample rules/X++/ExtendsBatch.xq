<ExtendsRunbaseBatch>
{
  for $c in /Class
  where $c[lower-case(@Extends) = 'runbasebatch']
  return <Result Artifact='{$c/@Name}'/>
}
</ExtendsRunbaseBatch>
