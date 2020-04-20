package main

// Test the walker functionality

import (
	"go/ast"
	"go/parser"
	"go/token"
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

func TestComment(t *testing.T) {
	doc := createFileDocument(`
// This is the package comment.
package main

// This comment is associated with the hello constant.
const hello = "Hello, World!" // line comment 1

// This comment is associated with the foo variable.
var foo = hello // line comment 2

// This comment is associated with the main function.
func main() {
	fmt.Println(hello) // line comment 3
}`, t)

	doc.Indent(2)
	s, _ := doc.WriteToString()
	// fmt.Print(s)

	// TODO
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
