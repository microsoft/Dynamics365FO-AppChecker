package main

// Test the walker functionality

import (
	"fmt"
	"go/ast"
	"go/importer"
	"go/parser"
	"go/token"
	"go/types"
	"log"
	"testing"

	"github.com/beevik/etree"
)

func createFileDocument(snippet string, t *testing.T) *etree.Document {
	doc := etree.NewDocument()
	fileset := token.NewFileSet()
	tree, err := parser.ParseFile(fileset, "dummy.go", snippet, parser.ParseComments)
	if err != nil {
		t.Fatal("Test snippet does not compile cleanly")
	}

	ast.Print(fileset, tree)
	Walk(&doc.Element, tree, fileset)

	return doc
}

func createExpressionDocument(snippet string, t *testing.T) *etree.Document {
	doc := etree.NewDocument()
	fileset := token.NewFileSet()
	tree, err := parser.ParseExprFrom(fileset, "", []byte(snippet), parser.ParseComments)
	if err != nil {
		t.Fatal("Test snippet does not compile cleanly")
	}

	ast.Print(fileset, tree)
	Walk(&doc.Element, tree, fileset)

	return doc
}

func TestTypeResolution(t *testing.T) {
	fset := token.NewFileSet()

	const hello = `package main

	import "fmt"

	func main() {
		s := "Hello, world" + " Banana"
		fmt.Println(s)
	}`

	// Parse the input string, []byte, or io.Reader,
	// recording position information in fset.
	// ParseFile returns an *ast.File, a syntax tree.
	f, err := parser.ParseFile(fset, "hello.go", hello, 0)
	if err != nil {
		log.Fatal(err) // parse error
	}

	// A Config controls various options of the type checker.
	// The defaults work fine except for one setting:
	// we must specify how to deal with imports.
	conf := types.Config{Importer: importer.Default()}

	info := &types.Info{
		Defs: make(map[*ast.Ident]types.Object),
		Uses: make(map[*ast.Ident]types.Object),
	}

	// Type-check the package containing only file f.
	// Check returns a *types.Package.
	pkg, err := conf.Check("cmd/hello", fset, []*ast.File{f}, info)
	if err != nil {
		log.Fatal(err) // type error
	}

	for id, obj := range info.Defs {
		fmt.Printf("%s: %q defines %v\n",
			fset.Position(id.Pos()), id.Name, obj)
	}
	for id, obj := range info.Uses {
		fmt.Printf("%s: %q uses %v\n",
			fset.Position(id.Pos()), id.Name, obj)
	}

	fmt.Printf("Package  %q\n", pkg.Path())
	fmt.Printf("Name:    %s\n", pkg.Name())
	fmt.Printf("Imports: %s\n", pkg.Imports())
	fmt.Printf("Scope:   %s\n", pkg.Scope())

	ast.Print(fset, f)
}

func TestCommentGroup(t *testing.T) {

}
func TestField(t *testing.T) {

}
func TestFieldList(t *testing.T) {

}
func TestIdent(t *testing.T) {

}
func TestBadExpr(t *testing.T) {

}
func TestBasicLit(t *testing.T) {

}
func TestEllipsis(t *testing.T) {

}
func TestFuncLit(t *testing.T) {

}
func TestCompositeLit(t *testing.T) {

}
func TestParenExpr(t *testing.T) {

}
func TestSelectorExpr(t *testing.T) {

}
func TestIndexExpr(t *testing.T) {

}
func TestSliceExpr(t *testing.T) {

}
func TestTypeAssertExpr(t *testing.T) {

}
func TestCallExpr(t *testing.T) {

}
func TestStarExpr(t *testing.T) {

}
func TestUnaryExpr(t *testing.T) {

} // NOT, Xor, Address of, Receive, monadic plus and minus
func TestBinaryExpr(t *testing.T) {

}

// case token.ADD(t *testing.T) { }
// case token.SUB(t *testing.T) { }
// case token.MUL(t *testing.T) { }
// case token.QUO(t *testing.T) { }
// case token.REM(t *testing.T) { }
// case token.AND(t *testing.T) { }
// case token.OR(t *testing.T) { }
// case token.XOR(t *testing.T) { }
// case token.AND_NOT(t *testing.T) { }
// case token.SHL(t *testing.T) { }
// case token.SHR(t *testing.T) { }
// case token.EQL(t *testing.T) { }
// case token.NEQ(t *testing.T) { }
// case token.LSS(t *testing.T) { }
// case token.LEQ(t *testing.T) { }
// case token.GTR(t *testing.T) { }
// case token.GEQ(t *testing.T) { }
// case token.LAND(t *testing.T) { }
// case token.LOR(t *testing.T) { }
func TestKeyValueExpr(t *testing.T) {

}
func TestArrayType(t *testing.T) {

}
func TestStructType(t *testing.T) {

}
func TestFuncType(t *testing.T) {

}
func TestInterfaceType(t *testing.T) {

}
func TestMapType(t *testing.T) {

}
func TestChanType(t *testing.T) {

}
func TestBadStmt(t *testing.T) {

}
func TestDeclStmt(t *testing.T) {

}
func TestEmptyStmt(t *testing.T) {

}
func TestLabeledStmt(t *testing.T) {

}
func TestExprStmt(t *testing.T) {

}
func TestSendStmt(t *testing.T) {

}
func TestIncDecStmt(t *testing.T) {

}
func TestAssignStmt(t *testing.T) {

}
func TestGoStmt(t *testing.T) {

}
func TestDeferStmt(t *testing.T) {

}

func TestReturnStatementValue(t *testing.T) {
	doc := createExpressionDocument(`func(a int) bool { return true }`, t)

	var n *etree.Element = doc.FindElement("/FuncLit/BlockStatement/ReturnStatement/Ident[@Name='true']")
	if n == nil {
		t.Fatal("Did not find root return statement")
	}
}

func TestReturnStatementNoValue(t *testing.T) {
	doc := createExpressionDocument(`func(a int) { return }`, t)

	var n *etree.Element = doc.FindElement("/FuncLit/BlockStatement/ReturnStatement")
	if n == nil {
		t.Fatal("Did not find root return statement")
	}

	children := doc.FindElements("/FuncLit/BlockStatement/ReturnStatement/*")
	if len(children) != 0 {
		t.Fatal("Naked return statement should not have children")
	}
}

func TestBranchStmt(t *testing.T) {

}
func TestBlockStmt(t *testing.T) {

}
func TestIfStmt(t *testing.T) {

}
func TestCaseClause(t *testing.T) {

}
func TestSwitchStmt(t *testing.T) {

}
func TestTypeSwitchStmt(t *testing.T) {

}
func TestCommClause(t *testing.T) {

}
func TestSelectStmt(t *testing.T) {

}
func TestForStmt(t *testing.T) {

}
func TestRangeStmt(t *testing.T) {

}
func TestImportSpec(t *testing.T) {

}
func TestValueSpec(t *testing.T) {

}
func TestTypeSpec(t *testing.T) {

}
func TestBadDecl(t *testing.T) {

}
func TestGenDecl(t *testing.T) {

}
func TestFuncDecl(t *testing.T) {

}
func TestFile(t *testing.T) {

}
func TestPackage(t *testing.T) {

}
