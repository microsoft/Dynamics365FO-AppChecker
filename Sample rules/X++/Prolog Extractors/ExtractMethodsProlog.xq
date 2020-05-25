declare function local:MethodComplexity($m as element(Method)) as xs:integer
{
  1 + count($m//IfStatement) + count($m//IfThenElseStatement) + count($m//WhileStatement) + count($m//DoWhileStatement)
    + count($m//ForStatement) + count($m/SearchStatement) + count($m//CaseValues/*) + count($m//ConditionalExpression)
};

declare function local:MethodClause($m as element(Method), $functor as xs:string) as xs:string
{
   let $visibility := if (lower-case($m/@IsPrivate) = 'true') then "private" 
                  else if (lower-case($m/@IsProtected) = 'true') then 'protected' 
                  else if (lower-case($m/@IsPublic) = 'true') then "public" 
                  else if (lower-case($m/@IsInternal) = 'true') then "internal"
                  else "public"
   
   return "method(" || $functor || ", m('" || lower-case($m/@Name) || "'), " 
                || "{" 
                ||    "access: '"  || $visibility                || "', "
                ||    "abstract: " || lower-case($m/@IsAbstract) || ", "
                ||    "final: "    || lower-case($m/@IsFinal)    || ", "
                ||    "static: "   || lower-case($m/@IsStatic)   || ", "
                ||    "loc: "  || $m/@EndLine - $m/@StartLine + 1 || ", "
                ||    "mc: "   || local:MethodComplexity($m) || ", "
                ||    "nos: "  || count(for $stmt in $m//* where ends-with(name($stmt), "Statement") return $stmt)
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

let $results := (
  for $a in /Class[lower-case(@Package)=$packages]
  for $m in $a/Method
  return local:MethodClause($m, "c('" || lower-case($a/@Name) || "')"),

  for $a in /Table[lower-case(@Package)=$packages]
  for $m in $a/Method
  return local:MethodClause($m, "t('" || lower-case($a/@Name) || "')"),
  
  for $a in /Form[lower-case(@Package)=$packages]/Class
  for $m in $a/Method
  return local:MethodClause($m,"f('" || lower-case($a/../@Name) || "')"),
  
  for $a in /Query[lower-case(@Package)=$packages]/Class
  for $m in $a/Method
  return local:MethodClause($m, "q('" || lower-case($a/../@Name) || "')")
)

return $results          
