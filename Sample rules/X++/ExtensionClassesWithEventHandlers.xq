(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Get artifacts that are extention classes but contain data event handlers :)
(: @Category BestPractice :)
(: @Language Xpp :)

<Results>
{
  for $a in /Class
  where $a/AttributeList/Attribute[lower-case(@Name) = 'extensionof']
  for $m in $a//Method
  where $m/AttributeList/Attribute[lower-case(@Name)='dataeventhandler'
                                or lower-case(@Name)='formcontroleventhandler'
                                or lower-case(@Name)='formdatafieldeventhandler'
                                or lower-case(@Name)='formdatasourceeventhandler'
                                or lower-case(@Name)='formeventhandler'
                                or lower-case(@Name)='prehandlerfor'
                                or lower-case(@Name)='posthandlerfor'
                                or lower-case(@Name)='dataeventhandlerattribute'
                                or lower-case(@Name)='formcontroleventhandlerattribute'
                                or lower-case(@Name)='formdatafieldeventhandlerattribute'
                                or lower-case(@Name)='formdatasourceeventhandlerattribute'
                                or lower-case(@Name)='formeventhandlerattribute'
						        or lower-case(@Name)='prehandlerforattribute'
                                or lower-case(@Name)='posthandlerforattribute'
]
  order by $a/@Package
  return <Result Artifact='{$a/@Artifact}' Package='{$a/@Package}'
    StartLine='{$a/@StartLine}' StartCol='{$a/@StartCol}' EndLine='{$a/@EndLine}' EndCol='{$a/@EndCol}'/>
}
</Results>