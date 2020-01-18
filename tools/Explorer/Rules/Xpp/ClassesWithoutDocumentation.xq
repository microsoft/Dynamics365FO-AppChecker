(: Artifacts that do not have any documentation on the class definition. :)
(: The quality of the documentation is not checked in this query.   :)
<Results>
{
  for $c in (/Class | /Table | /Form)[@Comments = ""]
      return <NonCommentedClass Artifact='{$c/@Artifact}' />
} 
</Results>