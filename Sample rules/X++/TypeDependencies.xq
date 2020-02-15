(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: This query will determine type dependecies for all classes and tables.
   The predefined types are weeded out, since they are not really interesting :)

(: @Category Informational :)
(: @Language Xpp :)

<TypeDependencies>
{
    for $c in /(Class | Table | Form | Query)
    return <Artifact Artifact='{$c/@Artifact}' Package='{$c/@Package}'>
       {
           for $type in distinct-values(($c//Method//*/@Type))
           where not ($type = ('int', 'int64', 'void', 'str',  'guid', 'utcdatetime', 'date', 'anytype', 'boolean', 'nulltype'))
           
           return <DependsOn Type='{$type}' Count='{count(($c//Method//*[@Type=$type]))}'/>
       }
       </Artifact>
}
</TypeDependencies> 