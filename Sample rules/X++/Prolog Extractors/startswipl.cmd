REM Copyright (c) Microsoft Corporation.
REM Licensed under the MIT license.
REM Start SWI prolog with the extracted files as parameters. This will 
REM load the files so they are ready for querying. You may want to use 
REM the clauses.pl file to enter some clauses into the prolog scope.
"C:\Program Files\swipl\bin\swipl.exe" ExtractAttributesProlog.pl ^
ExtractClassDelegatesProlog.pl ^
ExtractClassFieldsProlog.pl ^
ExtractClassInheritanceProlog.pl ^
ExtractClassInterfaceImplementationsProlog.pl ^
ExtractClassMetricsProlog.pl ^
ExtractFormsDefProlog.pl ^
ExtractInterfaceAttributesProlog.pl ^
ExtractInterfaceInheritanceProlog.pl ^
ExtractInterfaceMethodsProlog.pl ^
ExtractInterfacesProlog.pl ^
ExtractMethodAttributesProlog.pl ^
ExtractMethodsProlog.pl ^
ExtractPackageDependenciesProlog.pl ^
ExtractPackagesProlog.pl ^
ExtractQueryDataSourcesProlog.pl ^
ExtractQueryDefProlog.pl ^
ExtractTableDefProlog.pl ^
ExtractTableFieldsProlog.pl