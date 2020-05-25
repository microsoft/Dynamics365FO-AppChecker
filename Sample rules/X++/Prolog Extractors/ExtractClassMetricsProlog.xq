(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: Extract interesting informatuon about classes as prolog clauses :)

declare function local:MethodComplexity($m as element(Method)) as xs:integer
{
  1 + count($m//IfStatement) + count($m//IfThenElseStatement) + count($m//WhileStatement) + count($m//DoWhileStatement)
    + count($m//ForStatement) + count($m/SearchStatement) + count($m//CaseValues/*) + count($m//ConditionalExpression)
};

declare function local:ClassClause($a, $functor as xs:string) as xs:string
{
    let $weightedMethodComplexity := sum(for $m in $a/Method return local:MethodComplexity($m))

    return "class(" || $functor || "('" || lower-case($a/@Name) || "'), " 
                    || "p('" || lower-case($a/@Package) || "'), "
                    || "{" 
                    ||    (if ($a/@IsAbstract) then "abstract: " || lower-case($a/@IsAbstract) || ", " else "")
                    ||    (if ($a/@IsFinal) then "final: "    || lower-case($a/@IsFinal)    || ", " else "")
                    ||    (if ($a/@IsStatic) then "static: "   || lower-case($a/@IsStatic)   || ", " else "")
                    ||    "loc: "  || $a/@EndLine - $a/@StartLine + 1 || ", "
                    ||    "nom: "  || count($a/Method) || ", "
                    ||    "wmc: "  || $weightedMethodComplexity || ", "
                    ||    "nopm: " || count($a/Method[lower-case(@IsPublic)="true"]) || ", "
                    ||    "noam: " || string(count($a/Method[@IsAbstract="true"])) || ", "
                    ||    "nopa: " || count($a/FieldDeclaration[lower-case(@IsPublic)="true"]) || ", "
                    ||    "nos: "  || count(for $stmt in $a/Method//* where ends-with(name($stmt), "Statement") return $stmt) || ", "
                    ||    "noa: "  || count($a/FieldDeclaration) 
                    || "})."    
};

let $packages := ('applicationfoundation', 'applicationplatform', '', 'applicationcommon',
        'directory', 'calendar', 'dimensions', 'currency',
        'unitofmeasure', 'measurement', 'sourcedocumentationtypes', 'sourcedocumentation',
        'ledger', 'electronicreportingdotnetutils', 'contactperson', 'datasharing',
        'policy', 'electronicreportingcore', 'banktypes', 'project', 
        'electronicreportingmapping', 'tax', 'subledger', 'personnelcore',
        'electronicreportingforax', 'businessprocess', 'casemanagement', 'generalledger',
        'electronicreporting', 'personnelmanagement', 'financialreporting', 'fiscalbooks',
        'taxengine', 'electronicreportingbusinessdoc', 'personnel', 'retail',
        'applicationsuite')

let $result := (
    for $a in /Class[lower-case(@Package)=$packages]
    return local:ClassClause($a, "c"), 

    for $a in /Table[lower-case(@Package)=$packages]
    return local:ClassClause($a, "t"),
    
    for $a in /Form[lower-case(@Package)=$packages]/Class
    return local:ClassClause($a, "f"),

    for $a in /Query[lower-case(@Package)=$packages]/Class
    return local:ClassClause($a, "t")
)

return $result