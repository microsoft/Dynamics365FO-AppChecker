(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Artifacts that do not have any documentation on the class definition. :)
(: @Category BestPractice :)
(: @Language Xpp :)
(: @Author pvillads@microsoft.com :)

<ClassesWithoutDocumentation  Category='BestPractice' href='docs.microsoft.com/Socratex/ClassesWithoutDocumentation' Version='1.0'>
{
  for $c in (/Class | /Table | /Form)[@Comments = ""]
      return <ClassWithoutDocumentation Artifact='{$c/@Artifact}' />
} 
</ClassesWithoutDocumentation>