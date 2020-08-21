(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Identify search statements (i.e. while select ... { }) where the buffers
  introduced in the select or join are assigned values in the body. 
  Example:
      while select * from T {
        T = foo();
      } 
:)
(: @Category BestPractice :)
(: @Language Xpp :)

<BufferOverwriteInSearch>
{
   for $c in /(Class | Table | Form | Query)
   for $m in $c//Method
   for $ss in $m//SearchStatement
   for $q in $ss/Query
   for $statement in $ss/CompoundStatement
   for $assignmentStatement in $statement//AssignEqualStatement
   let $simpleField := $assignmentStatement/*[1]
   where local-name($simpleField) ='SimpleField'
   and lower-case($simpleField/@Name) = lower-case($q/@BufferName)
   
   return <Res Artifact='{$c/@Artifact}' Package='{$c/@Package}'
       StartLine='{$assignmentStatement/@StartLine}' EndLine='{$assignmentStatement/@EndLine}' 
       StartCol='{$assignmentStatement/@StartCol}' EndCol='{$assignmentStatement/@EndCol}'>
   </Res>
}
</BufferOverwriteInSearch>
