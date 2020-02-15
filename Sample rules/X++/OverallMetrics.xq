(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Provide interesting metrics for the system. :)
(: @Category Informational :)
(: @Language Xpp :)

<OverallMetrics Category='Informational' href='docs.microsoft.com/Socratex/OverallMetrics' Version='1.0'>
    <Models>{count(/Models/Model)}</Models>    
    <Classes Count='{count(/Class)}' Methods='{count(/Class/Method)}' Lines='{sum(/Class/@EndLine)}'/>
    <Tables Count='{count(/Table)}'  Methods='{count(/Table//Method)}' Lines='{sum(/Table/Method/(@EndLine - @StartLine + 1))}'/>
    <Queries Count='{count(/Query)}'  Methods='{count(/Query//Method)}' Lines='{sum(/Query//Method/(@EndLine - @StartLine + 1))}'/>
    <Forms Count='{count(/Query)}'  Methods='{count(/Form//Method)}' Lines='{sum(/Form//Method/(@EndLine - @StartLine + 1))}'/>
</OverallMetrics>