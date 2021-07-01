(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Get exists and notexists joins where fields are specified in the query.
   This has no meaning, since no table buffer will actually be fetched for
   exists and notexists joins.  :)
(: @Category Informational :)
(: @Language Xpp :)

<ExistJoinsWithFields>
{
  for $a in /*
  for $q in $a//Query//JoinSpecification[@Kind=('ExistsJoin', 'NotExistsJoin')]/Query/ExplicitSelection
  order by $a/@Package
  return <ExistJoinsWithField Artifact='{$a/@Artifact}' Package='{$a/@Package}'
      StartLine='{$q/@StartLine}' EndLine='{$q/@EndLine}' 
      StartCol='{$q/@StartCol}' EndCol='{$q/@EndCol}' /> 
}
</ExistJoinsWithFields>

