(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Find switch statments with enumeration switch values :)
(: @Category Informational :)
(: @Language Xpp :)

declare function local:IsEnum($typename as xs:string)
{
    starts-with($typename, "Enumeration") 
};

<Results>
{
  for $a in /(Class | Table | Form | Query)
  for $m in $a//Method
  for $case in $m//SwitchStatement/*[1][local:IsEnum(@Type)]
  order by $a/@Name
  return <Result Artifact='{$a/@Artifact}'
    StartLine='{$case/@StartLine}' StartCol='{$case/@StartCol}' EndLine='{$case/@EndLine}' EndCol='{$case/@EndCol}'/>
}
</Results>