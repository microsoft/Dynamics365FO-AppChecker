<T>
{
    for $a in /(Class | Table | Form | Query)
    for $queryWithHint in $a//(FindStatement | SelectStatement)//JoinSpecification/Query[@UsesHint='true']
    return <R Artifact='{$a/@Artifact}' 
        StartLine='{$queryWithHint/@StartLine}' EndLine='{$queryWithHint/@EndLine}'
        StartCol='{$queryWithHint/@StartCol}' EndCol='{$queryWithHint/@EndCol}' />
}
</T>

