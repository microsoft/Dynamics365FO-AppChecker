package main

import (
	"flag"
	"fmt"
	"go/ast"
	"go/parser"
	"go/token"
	"os"
	"path"
	"strconv"
	"strings"

	"github.com/beevik/etree"
)

func main() {
	sourceDirectoryPtr := flag.String("Source", "", "The source directory")
	targetDirectoryPtr := flag.String("Target", "", "The target directory")

	flag.Parse()

	// Basic validation:
	if *sourceDirectoryPtr == "" || *targetDirectoryPtr == "" {
		usage()
		return
	}

	// Check that the source directory exists
	if !checkDirectory(*sourceDirectoryPtr) {
		panic(*sourceDirectoryPtr + " is not a directory or does not exist")
	}
	// Check that the target directory can be created.
	if !createDirectory(*targetDirectoryPtr) {
		panic("The target directory " + *targetDirectoryPtr + " could not be created.")
	}

	extract(*sourceDirectoryPtr, *targetDirectoryPtr)
}

// Prints usage information to the user.
func usage() {
	fmt.Println(`Usage:
    Extractor -Source=<sourcedirectory> -Target=<targetdirectory>

The arguments are:
    Source: The root of the directory structure containing go source.
            The name must denote an existing directory.
    Target: The root directory that will contain the extracted files.
            This directory will be created.`)
}

func extract(sourceDir string, targetDir string) bool {
	fs := token.NewFileSet()
	pkgs, _ := parser.ParseDir(fs, sourceDir, nil, parser.AllErrors|parser.ParseComments)

	for name, pkg := range pkgs {
		extractPackage(path.Join(targetDir, name), name, pkg, fs)
	}

	return true
}

func extractPackage(targetdir string, name string, pkg *ast.Package, fileset *token.FileSet) {

	doc := etree.NewDocument()
	doc.WriteSettings.UseCRLF = true

	Walk(&doc.Element, pkg, fileset)

	doc.Indent(2)
	doc.WriteToFile(path.Join(targetdir, name+".xml"))
}

// func extractFile(targetdir string, filename string, parent *etree.Element, file *ast.File, fileset *token.FileSet) {

// 	// ast.Print(fileset, file)

// 	fileXML := parent.CreateElement("File")
// 	fileXML.CreateAttr("Package", file.Name.Name)
// 	fileXML.CreateAttr("Filename", fileset.File(file.Package).Name())

// 	for _, decl := range file.Decls {
// 		extractDecl(fileXML, decl, fileset)
// 	}

// 	for _, imp := range file.Imports {
// 		extractImportSpec(fileXML, imp, fileset)
// 	}
// }

// func extractDecl(parent *etree.Element, declaration ast.Decl, fileset *token.FileSet) {
// 	// A GenDecl node represents an import, constant, type or variable declaration.
// 	gendecl, isGenDecl := declaration.(*ast.GenDecl)
// 	if isGenDecl {
// 		if gendecl.Tok == token.IMPORT {
// 			extractImport(parent, gendecl, fileset)
// 		} else if gendecl.Tok == token.CONST {
// 			extractConst(parent, gendecl, fileset)
// 		} else if gendecl.Tok == token.TYPE {
// 			extractType(parent, gendecl, fileset)
// 		} else if gendecl.Tok == token.VAR {
// 			extractVar(parent, gendecl, fileset)
// 		}
// 		return
// 	}

// 	funcdecl, isFuncDecl := declaration.(*ast.FuncDecl)
// 	if isFuncDecl {
// 		extractFuncDecl(parent, funcdecl, fileset)
// 		return
// 	}
// }

// func extractFuncDecl(parent *etree.Element, funcdecl *ast.FuncDecl, fileset *token.FileSet) {
// 	funcXML := parent.CreateElement("Func")
// 	funcXML.CreateAttr("Name", funcdecl.Name.Name)
// 	funcXML.CreateAttr("IsExported", strconv.FormatBool(funcdecl.Name.IsExported()))

// 	addPositionStartEnd(funcXML, funcdecl.Pos(), funcdecl.End(), fileset)
// }

// func extractVar(parent *etree.Element, gendecl *ast.GenDecl, fileset *token.FileSet) {
// 	varXML := parent.CreateElement("Var")
// 	addPositionStartEnd(varXML, gendecl.Pos(), gendecl.End(), fileset)

// 	for _, spec := range gendecl.Specs {
// 		valueSpec := spec.(*ast.ValueSpec)
// 		extractValueSpec(varXML, valueSpec, fileset)
// 	}
// }

// func extractConst(parent *etree.Element, gendecl *ast.GenDecl, fileset *token.FileSet) {
// 	constXML := parent.CreateElement("Const")
// 	addPositionStartEnd(constXML, gendecl.Pos(), gendecl.End(), fileset)

// 	for _, spec := range gendecl.Specs {
// 		valueSpec := spec.(*ast.ValueSpec)
// 		extractValueSpec(constXML, valueSpec, fileset)
// 	}
// }

// func extractType(parent *etree.Element, gendecl *ast.GenDecl, fileset *token.FileSet) {
// 	typeXML := parent.CreateElement("Type")
// 	addPositionStartEnd(typeXML, gendecl.Pos(), gendecl.End(), fileset)

// 	for _, spec := range gendecl.Specs {
// 		itemXML := typeXML.CreateElement("Item")

// 		typeSpec := spec.(*ast.TypeSpec)
// 		if typeSpec.Name != nil {
// 			itemXML.CreateAttr("Name", typeSpec.Name.Name)
// 		}

// 		if typeSpec.Comment != nil {
// 			itemXML.CreateAttr("Comment", typeSpec.Comment.Text())
// 		}
// 	}
// }

// func extractValueSpec(parent *etree.Element, valueSpec *ast.ValueSpec, fileset *token.FileSet) {
// 	itemXML := parent.CreateElement("Item")

// 	if valueSpec.Doc != nil {
// 		itemXML.CreateAttr("Doc", valueSpec.Doc.Text())
// 	}

// 	namesXML := itemXML.CreateElement("Names")
// 	for _, name := range valueSpec.Names {
// 		namesXML.CreateElement("Name").CreateAttr("Name", name.Name)
// 	}

// 	if valueSpec.Type != nil {
// 		extractExpr(itemXML, valueSpec.Type, fileset)
// 	}

// 	addPositionStartEnd(itemXML, valueSpec.Pos(), valueSpec.End(), fileset)
// }

// func extractExpr(parent *etree.Element, expr ast.Expr, fileset *token.FileSet) {
// 	Walk(parent, expr, fileset)
// }

// func traverse(parent *etree.Element, n ast.Node, fileset *token.FileSet) ast.Visitor {
// 	switch n.(type) {
// 	case *ast.BinaryExpr:
// 		{
// 			be := n.(*ast.BinaryExpr)
// 			fmt.Println(be.Op)
// 			return visitorFunc(traverse)
// 		}

// 	case *ast.Ident:
// 		{
// 			id := n.(*ast.Ident)
// 			fmt.Println(id.Name)
// 		}
// 	}
// 	return visitorFunc(traverse)
// }

func extractImport(parent *etree.Element, importDeclaration *ast.GenDecl, fileset *token.FileSet) {
	importXML := parent.CreateElement("Import")
	addPositionStartEnd(importXML, importDeclaration.Pos(), importDeclaration.End(), fileset)

	for _, spec := range importDeclaration.Specs {
		importSpec := spec.(*ast.ImportSpec)
		extractImportSpec(importXML, importSpec, fileset)
	}
}

func extractImportSpec(parent *etree.Element, importSpec *ast.ImportSpec, fileset *token.FileSet) {
	item := parent.CreateElement("Item")

	if importSpec.Name != nil {
		item.CreateAttr("Name", importSpec.Name.Name)
	}

	if importSpec.Path != nil {
		item.CreateAttr("Path", stripQuotes(importSpec.Path.Value))
	}

	if importSpec.Doc != nil {
		item.CreateAttr("Doc", importSpec.Doc.Text())
	}

	addPositionStartEnd(item, importSpec.Pos(), importSpec.End(), fileset)
}

func stripQuotes(s string) string {
	return strings.TrimSuffix(strings.TrimPrefix(s, "\""), "\"")
}

func addPosition(node *etree.Element, pos token.Pos, fileset *token.FileSet) {
	if pos.IsValid() {
		var position token.Position = fileset.Position(pos)
		node.CreateAttr("Line", strconv.Itoa(position.Line))
		node.CreateAttr("Column", strconv.Itoa(position.Column))
	}
}

func addPositionStartEnd(node *etree.Element, pos1 token.Pos, pos2 token.Pos, fileset *token.FileSet) {
	if pos1.IsValid() {
		var position1 token.Position = fileset.Position(pos1)
		node.CreateAttr("StartLine", strconv.Itoa(position1.Line))
		node.CreateAttr("StartColumn", strconv.Itoa(position1.Column))
	}
	if pos2.IsValid() {
		var position2 token.Position = fileset.Position(pos2)
		node.CreateAttr("EndLine", strconv.Itoa(position2.Line))
		node.CreateAttr("EndColumn", strconv.Itoa(position2.Column))
	}
}

// Check that the given path exists and is a directory
func checkDirectory(path string) bool {
	stat, err := os.Stat(path)

	_, ok := err.(*os.PathError)
	if ok {
		return false
	}

	if os.IsNotExist(err) {
		return false
	}

	if stat != nil && !stat.IsDir() {
		return false
	}

	return true
}

func createDirectory(path string) bool {
	err := os.MkdirAll(path, 0755)
	return err == nil
}
