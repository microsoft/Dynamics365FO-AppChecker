let $packages := distinct-values(/Class/@Package)

for $p in $packages
return $p || ',' || sum(/(Class | Form | Query | Table)[@Package=$p]/@EndLine)


