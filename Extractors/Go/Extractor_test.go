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
	"os"
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
	Walk(&doc.Element, tree, fileset, nil)

	return doc
}

func createExpressionDocument(snippet string, t *testing.T) *etree.Document {
	doc := etree.NewDocument()
	fileset := token.NewFileSet()
	tree, err := parser.ParseExprFrom(fileset, "", []byte(snippet), parser.ParseComments)
	if err != nil {
		t.Fatal("Test snippet does not compile cleanly")
	}

	// ast.Print(fileset, tree)
	Walk(&doc.Element, tree, fileset, nil)

	return doc
}

func TestTypeResolution(t *testing.T) {
	// https://golang.org/pkg/go/types/
	// tutorial is here: https://github.com/golang/example/tree/master/gotypes

	fset := token.NewFileSet()

	const hello = `package main

	import "fmt"

	// This is the main function
	func main() {
		// Construct a string with constant folding.
		s := "Hello, world" + " Banana"
		fmt.Println(s)
		x := "asd".Len()
	}`

	const input = `package main

	var m = make(map[string]int)

	func main() {
		v, ok := m["hello, " + "world"]
		print(rune(v), ok)
	}`

	// Parse the input string, []byte, or io.Reader,
	// recording position information in fset.
	// ParseFile returns an *ast.File, a syntax tree.
	f, err := parser.ParseFile(fset, "hello.go", input, parser.ParseComments)
	if err != nil {
		log.Fatal(err) // parse error
	}

	// A Config controls various options of the type checker.
	// The defaults work fine except for one setting:
	// we must specify how to deal with imports.
	conf := types.Config{Importer: importer.Default()}

	// Initialize the data structure used for name resolution. Provide
	// an *ast.Ident and get an object back, once this has been populated.
	// The information required must be initialized here.
	info := &types.Info{
		Defs:  make(map[*ast.Ident]types.Object),     // Definitions of user defined entities
		Uses:  make(map[*ast.Ident]types.Object),     // references
		Types: make(map[ast.Expr]types.TypeAndValue), // provide map from an expr to its type
	}

	// Type-check the package containing only file f. Store the results
	// in the info structure. Here there is only one file to deal with,
	// but the API allows you to pass an array of ast.Files.
	// Check returns a *types.Package instance.
	pkg, err := conf.Check("cmd/hello", fset, []*ast.File{f}, info)
	if err != nil {
		log.Fatal(err) // type error(s) encountered.
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

	// ast.Print(fset, f)
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
	// A BadExpr node is a placeholder for expressions containing syntax errors
	// for which no correct expression nodes can be created.

}
func TestBasicLit(t *testing.T) {
	doc := createExpressionDocument(`func() int { return 42 }`, t)

	var nInt *etree.Element = doc.FindElement("/FuncLit/BlockStatement/ReturnStatement/Int")
	if nInt == nil {
		t.Fatal("Did not find root int literal")
	}
	var intValue = nInt.SelectAttrValue("Value", "")
	if intValue != "42" {
		t.Fatal("Did not find root int literal value")
	}

	doc = createExpressionDocument(`func() float { return 1.00 }`, t)
	nFloat := doc.FindElement("/FuncLit/BlockStatement/ReturnStatement/Float")
	if nFloat == nil {
		t.Fatal("Did not find root float literal")
	}
	var floatValue = nFloat.SelectAttrValue("Value", "")
	if floatValue != "1.00" {
		t.Fatal("Did not find root float literal value")
	}

	doc = createExpressionDocument(`func() float { return 2.1i }`, t)
	nImaginary := doc.FindElement("/FuncLit/BlockStatement/ReturnStatement/Imaginary")
	if nImaginary == nil {
		t.Fatal("Did not find root imaginary literal")
	}
	var imaginaryValue = nImaginary.SelectAttrValue("Value", "")
	if imaginaryValue != "2.1i" {
		t.Fatal("Did not find root imaginary literal value")
	}

	doc = createExpressionDocument(`func() float { return "Banana" }`, t)
	nString := doc.FindElement("/FuncLit/BlockStatement/ReturnStatement/String")
	if nString == nil {
		t.Fatal("Did not find root string literal")
	}
	var stringValue = nString.SelectAttrValue("Value", "")
	if stringValue != "Banana" {
		t.Fatal("Did not find root string literal value")
	}

	doc = createExpressionDocument(`func() float { return 'a' }`, t)
	nChar := doc.FindElement("/FuncLit/BlockStatement/ReturnStatement/Char")
	if nChar == nil {
		t.Fatal("Did not find root char literal")
	}
	var charValue = nChar.SelectAttrValue("Value", "")
	if charValue != "'a'" {
		t.Fatal("Did not find root char literal value")
	}
}
func TestEllipsis(t *testing.T) {

}
func TestFuncLit(t *testing.T) {

}
func TestCompositeLit(t *testing.T) {

}
func TestParenExpr(t *testing.T) {
	doc := createExpressionDocument(`func() float { return 3 * (1+2) }`, t)
	nMultiply := doc.FindElement("/FuncLit/BlockStatement/ReturnStatement/Multiply")
	if nMultiply == nil {
		t.Fatal("Did not find Multiplication node")
	}

	// left node is a value node (int literal 3) and the right node
	var left = nMultiply.FindElement("Int[@Value='3']")
	if left == nil {
		t.Fatal("Did not find LHS of multiplication")
	}

	var right1 = nMultiply.FindElement("Plus/Int[@Value='1']")
	if right1 == nil {
		t.Fatal("Did not find LHS of RHS of multiplication")
	}
	var right2 = nMultiply.FindElement("Plus/Int[@Value='2']")
	if right2 == nil {
		t.Fatal("Did not find RHS of RHS of multiplication")
	}
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
	doc := createExpressionDocument(`func () int { return +42 }`, t)
	var nPlus *etree.Element = doc.FindElement("/FuncLit/BlockStatement/ReturnStatement/UnaryPlus/Int[@Value='42']")
	if nPlus == nil {
		t.Fatal("Did not find unary plus node")
	}

	doc = createExpressionDocument(`func () int { return -42 }`, t)
	var nMinus *etree.Element = doc.FindElement("/FuncLit/BlockStatement/ReturnStatement/UnaryMinus/Int[@Value='42']")
	if nMinus == nil {
		t.Fatal("Did not find unary minus node")
	}

	doc = createExpressionDocument(`func () bool { return !true }`, t)
	var nNot *etree.Element = doc.FindElement("/FuncLit/BlockStatement/ReturnStatement/Negation/Ident[@Name='true']")
	if nNot == nil {
		t.Fatal("Did not find unary negation node")
	}

	doc = createExpressionDocument(`
	func () bool {
		var bits byte = 0x0F
		return ^bits
	}`, t)
	var nXor *etree.Element = doc.FindElement("/FuncLit/BlockStatement/ReturnStatement/Xor/Ident[@Name='bits']")
	if nXor == nil {
		t.Fatal("Did not find unary XOR node")
	}

	doc = createExpressionDocument(`
	func () {
		a := 45
		_ = &a
	}`, t)
	var nAddressOf *etree.Element = doc.FindElement("/FuncLit/BlockStatement/AssignmentStatement/AddressOf/Ident[@Name='a']")
	if nAddressOf == nil {
		t.Fatal("Did not find unary AddressOf node")
	}

	doc = createExpressionDocument(`
	func () {
		ch := make(chan int)
		_ = <-ch
	}`, t)
	var nReceive *etree.Element = doc.FindElement("/FuncLit/BlockStatement/AssignmentStatement/Receive/Ident[@Name='ch']")
	if nReceive == nil {
		t.Fatal("Did not find unary Receive node")
	}

}

func TestBinaryExpr(t *testing.T) {
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
}

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
	doc := createExpressionDocument(`func() { ; }`, t)

	doc.Indent(2)
	doc.WriteTo(os.Stdout)

	nEmpty := doc.FindElement("/FuncLit/BlockStatement/EmptyStatement")
	if nEmpty == nil {
		t.Fatal("Did not find empty statement node")
	}

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
	doc := createExpressionDocument(`func(a int) { }`, t)

	var n *etree.Element = doc.FindElement("/FuncLit/BlockStatement")
	if n == nil {
		t.Fatal("Did not find root block statement")
	}

	children := doc.FindElements("/FuncLit/BlockStatement/ReturnStatement/*")
	if len(children) != 0 {
		t.Fatal("Naked return statement should not have children")
	}
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
