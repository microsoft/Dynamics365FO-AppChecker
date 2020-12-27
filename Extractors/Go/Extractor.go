package main

import (
	"flag"
	"fmt"
	"go/ast"
	"go/parser"
	"go/scanner"
	"go/token"
	"go/types"
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
		panic(*sourceDirectoryPtr + " is not a directory or it does not exist")
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
    Source: The root of the directory structure containing go sources.
            The argument must designate an existing directory.
    Target: The root directory that will contain the extracted files.
            This directory will be created.`)
}

func extract(sourceDir string, targetDir string) {
	fs := token.NewFileSet()
	pkgs, diag := parser.ParseDir(fs, sourceDir, nil, parser.AllErrors|parser.ParseComments)

	doc := createDiagnosticDocument(diag)
	errorFilename := "errors.xml"
	errorFilepath := path.Join(targetDir, errorFilename)

	if doc != nil {
		doc.WriteToFile(errorFilepath)
	} else {
		// No diagnostics, so delete any old error files.
		os.Remove(errorFilepath)

		for name, pkg := range pkgs {
			extractPackage(path.Join(targetDir, name), name, pkg, diag, fs)
		}
	}
}

// Create a diagnostic document if there are any diagnostics recorded. If there
// are no errors to report, the function returns nil.
func createDiagnosticDocument(diags error) *etree.Document {

	if diags != nil {
		errlist := diags.(scanner.ErrorList)

		if errlist.Len() > 0 {
			doc := etree.NewDocument()
			doc.WriteSettings.UseCRLF = true
			doc.Indent(2)

			root := doc.CreateElement("Diagnostics")

			for _, e := range errlist {
				diagNode := root.CreateElement("Diagnostic")
				diagNode.CreateAttr("Message", e.Msg)
				if e.Pos.IsValid() {
					diagNode.CreateAttr("Filename", e.Pos.Filename)
					diagNode.CreateAttr("StartLine", strconv.Itoa(e.Pos.Line))
					diagNode.CreateAttr("EndLine", strconv.Itoa(e.Pos.Column))
				}
			}

			return doc
		}
	}
	return nil
}

func extractPackage(targetdir string, name string, pkg *ast.Package, diags error, fileset *token.FileSet) {

	doc := etree.NewDocument()
	doc.WriteSettings.UseCRLF = true
	doc.Indent(2)

	// Initialize the data structure used for name resolution. Provide
	// an *ast.Ident and get an object back, once this has been populated.
	// The information required must be initialized here.
	info := &types.Info{
		Defs:  make(map[*ast.Ident]types.Object),     // Definitions of user defined entities
		Uses:  make(map[*ast.Ident]types.Object),     // references
		Types: make(map[ast.Expr]types.TypeAndValue), // provide map from an expr to its type
	}

	Walk(&doc.Element, pkg, fileset, info)

	// Create a directory if the package given by name has not been seen before
	if !checkDirectory(targetdir) {
		createDirectory(targetdir)
	}

	// Write the output file:
	outputFilename := name + ".xml"
	fileName := path.Join(targetdir, outputFilename)
	doc.WriteToFile(fileName)
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

func addTypeInformation(element *etree.Element, node ast.Expr, info *types.Info) {
	if info != nil {
		if tv, ok := info.Types[node]; ok {
			element.CreateAttr("Type", tv.Type.String())
			element.CreateAttr("Mode", mode(tv))
		}
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
