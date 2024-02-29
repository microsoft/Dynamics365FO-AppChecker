<Entities>
{
    for $e in /Entity
    return <Entity Artifact='{$e/@Artifact}'
        StartLine='{$e/@StartLine}' EndLine='{$e/@EndLine}'
        StartCol='{$e/@StartCol}' EndCol='{$e/@EndCol}' />
}
</Entities>