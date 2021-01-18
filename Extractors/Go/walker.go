package main

import (
	"fmt"
	"go/ast"
	"go/importer"
	"go/token"
	"go/types"

	"log"
	"os"
	"strconv"

	"encoding/base64"
	"io/ioutil"

	"github.com/beevik/etree"
)

func walkIdentList(parent *etree.Element, list []*ast.Ident, fileset *token.FileSet, info *types.Info) {
	for _, x := range list {
		Walk(parent, x, fileset, info)
	}
}

func walkExprList(parent *etree.Element, list []ast.Expr, fileset *token.FileSet, info *types.Info) {
	for _, x := range list {
		Walk(parent, x, fileset, info)
	}
}

func walkStmtList(parent *etree.Element, list []ast.Stmt, fileset *token.FileSet, info *types.Info) {
	for _, x := range list {
		Walk(parent, x, fileset, info)
	}
}

func walkDeclList(parent *etree.Element, list []ast.Decl, fileset *token.FileSet, info *types.Info) {
	for _, x := range list {
		Walk(parent, x, fileset, info)
	}
}

// Walk the node provided
func Walk(parent *etree.Element, node ast.Node, fileset *token.FileSet, info *types.Info) {

	switch n := node.(type) {
	// Comments and fields
	// https://golang.org/pkg/go/ast/#Comment
	case *ast.Comment:
		commentNode := parent.CreateElement("Comment")
		commentNode.CreateText(n.Text)

	// A CommentGroup represents a sequence of comments with no other tokens and no empty lines between.
	// https://golang.org/pkg/go/ast/#CommentGroup
	case *ast.CommentGroup:
		commentGroupNode := parent.CreateElement("CommentGroup")
		for _, c := range n.List {
			Walk(commentGroupNode, c, fileset, info)
		}
		addPositionStartEnd(commentGroupNode, n.Pos(), n.End(), fileset)

	// https://golang.org/pkg/go/ast/#Field
	// A Field represents a Field declaration list in a struct type, a method list in an interface type,
	// or a parameter/result declaration in a signature. Field.Names is nil for unnamed parameters
	// (parameter lists which only contain types) and embedded struct fields. In the latter case,
	// the field name is the type name.
	case *ast.Field:
		fieldNode := parent.CreateElement("Field")
		if n.Doc != nil {
			Walk(fieldNode, n.Doc, fileset, info)
		}

		namesNode := fieldNode.CreateElement("Names")
		walkIdentList(namesNode, n.Names, fileset, info)

		Walk(fieldNode, n.Type, fileset, info)
		if n.Tag != nil {
			Walk(fieldNode, n.Tag, fileset, info)
		}
		if n.Comment != nil {
			Walk(fieldNode, n.Comment, fileset, info)
		}

		addPositionStartEnd(fieldNode, n.Pos(), n.End(), fileset)

	// https://golang.org/pkg/go/ast/#FieldList
	// A FieldList represents a list of Fields, enclosed by parentheses or braces.
	case *ast.FieldList:
		fieldListNode := parent.CreateElement("Fields")

		for _, f := range n.List {
			Walk(fieldListNode, f, fileset, info)
		}
		addPositionStartEnd(fieldListNode, n.Pos(), n.End(), fileset)

	// Expressions
	// https://golang.org/pkg/go/ast/#Ident
	// An Ident node represents an identifier.
	case *ast.Ident:
		// Nothing to do
		identNode := parent.CreateElement("Ident")
		identNode.CreateAttr("Name", n.Name)
		identNode.CreateAttr("IsExported", strconv.FormatBool(n.IsExported()))
		addTypeInformation(identNode, n, info)
		addPositionStartEnd(identNode, n.Pos(), n.End(), fileset)

	case *ast.BadExpr:
		badExprNode := parent.CreateElement("BadExpression")
		addPositionStartEnd(badExprNode, n.Pos(), n.End(), fileset)

	case *ast.BasicLit:
		var literalNode *etree.Element
		if n.Kind == token.INT {
			literalNode = parent.CreateElement("Int")
		} else if n.Kind == token.FLOAT {
			literalNode = parent.CreateElement("Float")
		} else if n.Kind == token.IMAG {
			literalNode = parent.CreateElement("Imaginary")
		} else if n.Kind == token.STRING {
			literalNode = parent.CreateElement("String")
		} else if n.Kind == token.CHAR {
			literalNode = parent.CreateElement("Char")
		}
		literalNode.CreateAttr("Value", stripQuotes(n.Value))
		addTypeInformation(literalNode, n, info)
		addPositionStartEnd(literalNode, n.Pos(), n.End(), fileset)

	// https://golang.org/pkg/go/ast/#Ellipsis
	// An Ellipsis node stands for the "..." type in a parameter list or the "..." length in an array type.
	case *ast.Ellipsis:
		ellipsisNode := parent.CreateElement("Ellipsis")
		if n.Elt != nil {
			Walk(ellipsisNode, n.Elt, fileset, info)
		}
		addTypeInformation(ellipsisNode, n, info)
		addPositionStartEnd(ellipsisNode, n.Pos(), n.End(), fileset)

	// https://golang.org/pkg/go/ast/#FuncLit
	// A FuncLit node represents a function literal.
	case *ast.FuncLit:
		funcLitNode := parent.CreateElement("FuncLit")
		Walk(funcLitNode, n.Type, fileset, info)
		Walk(funcLitNode, n.Body, fileset, info)
		addTypeInformation(funcLitNode, n, info)
		addPositionStartEnd(funcLitNode, n.Pos(), n.End(), fileset)

	// https://golang.org/pkg/go/ast/#CompositeLit
	// A CompositeLit node represents a composite literal, i.e. array of struct literals.
	case *ast.CompositeLit:
		compositeLiteral := parent.CreateElement("CompositeLiteral")
		compositeLiteral.CreateAttr("Incomplete", strconv.FormatBool(n.Incomplete))

		if n.Type != nil {
			Walk(compositeLiteral, n.Type, fileset, info)
		}
		walkExprList(compositeLiteral, n.Elts, fileset, info)
		addTypeInformation(compositeLiteral, n, info)
		addPositionStartEnd(compositeLiteral, n.Pos(), n.End(), fileset)

	case *ast.ParenExpr:
		Walk(parent, n.X, fileset, info)

	case *ast.SelectorExpr:
		// https://golang.org/pkg/go/ast/#SelectorExpr
		// A SelectorExpr node represents an expression followed by a selector.
		selectorExprNode := parent.CreateElement("Selector")
		Walk(selectorExprNode, n.X, fileset, info)
		Walk(selectorExprNode, n.Sel, fileset, info)
		addTypeInformation(selectorExprNode, n, info)
		addPositionStartEnd(selectorExprNode, n.Pos(), n.End(), fileset)

	case *ast.IndexExpr:
		// https://golang.org/pkg/go/ast/#IndexExpr
		// An IndexExpr node represents an expression followed by an index.
		indexExprNode := parent.CreateElement("Index")
		Walk(indexExprNode, n.X, fileset, info)
		Walk(indexExprNode, n.Index, fileset, info)
		addTypeInformation(indexExprNode, n, info)
		addPositionStartEnd(indexExprNode, n.Pos(), n.End(), fileset)

	case *ast.SliceExpr:
		// https://golang.org/pkg/go/ast/#SliceExpr
		// A SliceExpr node represents an expression followed by slice indices.
		sliceExprNode := parent.CreateElement("Slice")
		Walk(sliceExprNode, n.X, fileset, info)
		if n.Low != nil {
			Walk(sliceExprNode, n.Low, fileset, info)
		}
		if n.High != nil {
			Walk(sliceExprNode, n.High, fileset, info)
		}
		if n.Max != nil {
			Walk(sliceExprNode, n.Max, fileset, info)
		}
		addTypeInformation(sliceExprNode, n, info)
		addPositionStartEnd(sliceExprNode, n.Pos(), n.End(), fileset)

	case *ast.TypeAssertExpr:
		// https://golang.org/pkg/go/ast/#TypeAssertExpr
		// A TypeAssertExpr node represents an expression followed by a type assertion.
		typeAssertExprNode := parent.CreateElement("TypeAssert")
		Walk(typeAssertExprNode, n.X, fileset, info)
		if n.Type != nil {
			Walk(typeAssertExprNode, n.Type, fileset, info)
		}
		addTypeInformation(typeAssertExprNode, n, info)
		addPositionStartEnd(typeAssertExprNode, n.Pos(), n.End(), fileset)

	case *ast.CallExpr:
		// https://golang.org/pkg/go/ast/#CallExpr
		// A CallExpr node represents an expression followed by an argument list.
		callExprNode := parent.CreateElement("Call")
		Walk(callExprNode, n.Fun, fileset, info)
		walkExprList(callExprNode, n.Args, fileset, info)
		addTypeInformation(callExprNode, n, info)
		addPositionStartEnd(callExprNode, n.Pos(), n.End(), fileset)

	case *ast.StarExpr:
		// https://golang.org/pkg/go/ast/#StarExpr
		// A StarExpr node represents an expression of the form "*" Expression. Semantically it
		// could be a unary "*" expression, or a pointer type.
		starExprNode := parent.CreateElement("Star")
		Walk(starExprNode, n.X, fileset, info)
		addTypeInformation(starExprNode, n, info)
		addPositionStartEnd(starExprNode, n.Pos(), n.End(), fileset)

	// https://golang.org/pkg/go/ast/#UnaryExpr
	// Unary expressions
	case *ast.UnaryExpr: // NOT, Xor, Address of, Receive, monadic plus and minus
		var unaryExpressionKind string
		if n.Op == token.AND {
			unaryExpressionKind = "AddressOf"
		} else if n.Op == token.SUB {
			unaryExpressionKind = "UnaryMinus"
		} else if n.Op == token.ADD {
			unaryExpressionKind = "UnaryPlus"
		} else if n.Op == token.NOT {
			unaryExpressionKind = "Negation"
		} else if n.Op == token.ARROW {
			unaryExpressionKind = "Receive"
		} else if n.Op == token.XOR {
			unaryExpressionKind = "Xor"
		} else {
			panic("Unknown unary operator ")
		}
		unaryExprNode := parent.CreateElement(unaryExpressionKind)
		Walk(unaryExprNode, n.X, fileset, info)
		addTypeInformation(unaryExprNode, n, info)
		addPositionStartEnd(unaryExprNode, n.Pos(), n.End(), fileset)

	// https://golang.org/pkg/go/ast/#BinaryExpr
	// A BinaryExpr node represents a binary expression.
	case *ast.BinaryExpr:
		var operatorName string
		switch n.Op {
		case token.ADD:
			operatorName = "Plus"
		case token.SUB:
			operatorName = "Minus"
		case token.MUL:
			operatorName = "Multiply"
		case token.QUO:
			operatorName = "Divide"
		case token.REM:
			operatorName = "Remainder"
		case token.AND:
			operatorName = "BitwiseAnd"
		case token.OR:
			operatorName = "BitwiseOr"
		case token.XOR:
			operatorName = "Xor"
		case token.AND_NOT:
			operatorName = "BitClear"
		case token.SHL:
			operatorName = "ShiftLeft"
		case token.SHR:
			operatorName = "ShiftRight"
		case token.EQL:
			operatorName = "Equal"
		case token.NEQ:
			operatorName = "NotEqual"
		case token.LSS:
			operatorName = "LessThan"
		case token.LEQ:
			operatorName = "LessThanOrEqual"
		case token.GTR:
			operatorName = "GreaterThan"
		case token.GEQ:
			operatorName = "GreaterThanOrEqual"
		case token.LAND:
			operatorName = "And"
		case token.LOR:
			operatorName = "Or"
		default:
			panic("Unknown binary operator")
		}
		binaryExpressionNode := parent.CreateElement(operatorName)

		Walk(binaryExpressionNode, n.X, fileset, info)
		Walk(binaryExpressionNode, n.Y, fileset, info)

		addTypeInformation(binaryExpressionNode, n, info)
		addPositionStartEnd(binaryExpressionNode, n.Pos(), n.End(), fileset)

	// https://golang.org/pkg/go/ast/#KeyValueExpr
	// A KeyValueExpr node represents (key : value) pairs in composite literals.
	case *ast.KeyValueExpr:
		keyValuePairNode := parent.CreateElement("KeyValuePair")
		Walk(keyValuePairNode, n.Key, fileset, info)
		Walk(keyValuePairNode, n.Value, fileset, info)
		addTypeInformation(keyValuePairNode, n, info)
		addPositionStartEnd(keyValuePairNode, n.Pos(), n.End(), fileset)

	// Types
	// https://golang.org/pkg/go/ast/#ArrayType
	// An ArrayType node represents an array or slice type.
	case *ast.ArrayType:
		arrayTypeNode := parent.CreateElement("ArrayType")
		if n.Len != nil {
			Walk(arrayTypeNode, n.Len, fileset, info)
		}
		Walk(arrayTypeNode, n.Elt, fileset, info)
		addTypeInformation(arrayTypeNode, n, info)
		addPositionStartEnd(arrayTypeNode, n.Pos(), n.End(), fileset)

	// https://golang.org/pkg/go/ast/#StructType
	// A StructType node represents a struct type.
	case *ast.StructType:
		structTypeNode := parent.CreateElement("Struct")
		structTypeNode.CreateAttr("Incomplete", strconv.FormatBool(n.Incomplete))

		Walk(structTypeNode, n.Fields, fileset, info)
		addTypeInformation(structTypeNode, n, info)
		addPositionStartEnd(structTypeNode, n.Pos(), n.End(), fileset)

	// https://golang.org/pkg/go/ast/#FuncType
	// A FuncType node represents a function type.
	case *ast.FuncType:
		funcTypeNode := parent.CreateElement("FuncType")
		if n.Params != nil {
			Walk(funcTypeNode, n.Params, fileset, info)
		}
		if n.Results != nil {
			Walk(funcTypeNode, n.Results, fileset, info)
		}
		addTypeInformation(funcTypeNode, n, info)
		addPositionStartEnd(funcTypeNode, n.Pos(), n.End(), fileset)

	// https://golang.org/pkg/go/ast/#InterfaceType
	// An InterfaceType node represents an interface type.
	case *ast.InterfaceType:
		interfaceTypeNode := parent.CreateElement("InterfaceType")
		Walk(interfaceTypeNode, n.Methods, fileset, info)
		addTypeInformation(interfaceTypeNode, n, info)
		addPositionStartEnd(interfaceTypeNode, n.Pos(), n.End(), fileset)

	// https://golang.org/pkg/go/ast/#MapType
	// A MapType node represents a map type.
	case *ast.MapType:
		mapTypeNode := parent.CreateElement("MapType")
		Walk(mapTypeNode, n.Key, fileset, info)
		Walk(mapTypeNode, n.Value, fileset, info)
		addTypeInformation(mapTypeNode, n, info)
		addPositionStartEnd(mapTypeNode, n.Pos(), n.End(), fileset)

	// https://golang.org/pkg/go/ast/#ChanType
	// A ChanType node represents a channel type.
	case *ast.ChanType:
		chanTypeNode := parent.CreateElement("ChanType")
		Walk(parent, n.Value, fileset, info)
		addTypeInformation(chanTypeNode, n, info)
		addPositionStartEnd(chanTypeNode, n.Pos(), n.End(), fileset)

	// Statements
	case *ast.BadStmt:
		badStatementNode := parent.CreateElement("BadStatement")
		addPositionStartEnd(badStatementNode, n.Pos(), n.End(), fileset)

	// https://golang.org/pkg/go/ast/#DeclStmt
	// A DeclStmt node represents a declaration in a statement list.
	case *ast.DeclStmt:
		declStatementNode := parent.CreateElement("DeclStatement")
		Walk(declStatementNode, n.Decl, fileset, info)
		addPositionStartEnd(declStatementNode, n.Pos(), n.End(), fileset)

	// https://golang.org/pkg/go/ast/#EmptyStmt
	// An EmptyStmt node represents an empty statement. The "position" of the empty
	// statement is the position of the immediately following (explicit or implicit) semicolon.
	case *ast.EmptyStmt:
		emptyStatementNode := parent.CreateElement("EmptyStatement")
		addPositionStartEnd(emptyStatementNode, n.Pos(), n.End(), fileset)

	// https://golang.org/pkg/go/ast/#LabeledStmt
	// A LabeledStmt node represents a labeled statement.
	case *ast.LabeledStmt:
		labeledStatementNode := parent.CreateElement("LabeledStatement")

		// Get the label as a string and include as attribute
		labeledStatementNode.CreateAttr("Label", n.Label.Name)
		Walk(labeledStatementNode, n.Stmt, fileset, info)
		addPositionStartEnd(labeledStatementNode, n.Pos(), n.End(), fileset)

	// https://golang.org/pkg/go/ast/#ExprStmt
	// An ExprStmt node represents a (stand-alone) expression in a statement list.
	case *ast.ExprStmt:
		exprStatementNode := parent.CreateElement("ExpressionStatement")
		Walk(exprStatementNode, n.X, fileset, info)
		addPositionStartEnd(exprStatementNode, n.Pos(), n.End(), fileset)

	// https://golang.org/pkg/go/ast/#SendStmt
	// A SendStmt node represents a send statement.
	case *ast.SendStmt:
		sendStatementNode := parent.CreateElement("SendStatement")
		Walk(sendStatementNode, n.Chan, fileset, info)
		Walk(sendStatementNode, n.Value, fileset, info)
		addPositionStartEnd(sendStatementNode, n.Pos(), n.End(), fileset)

	// https://golang.org/pkg/go/ast/#IncDecStmt
	// An IncDecStmt node represents an increment or decrement statement.
	// The Tok token is either token.INC or token.DEC
	case *ast.IncDecStmt:
		var incDecStatementNode *etree.Element
		if n.Tok == token.INC {
			incDecStatementNode = parent.CreateElement("IncrementStatement")
		} else {
			incDecStatementNode = parent.CreateElement("DecrementStatement")
		}
		Walk(incDecStatementNode, n.X, fileset, info)
		addPositionStartEnd(incDecStatementNode, n.Pos(), n.End(), fileset)

	// https://golang.org/pkg/go/ast/#AssignStmt
	// An AssignStmt node represents an assignment or a short variable declaration.
	case *ast.AssignStmt:
		assignmentStatementNode := parent.CreateElement("AssignmentStatement")
		walkExprList(assignmentStatementNode, n.Lhs, fileset, info)
		walkExprList(assignmentStatementNode, n.Rhs, fileset, info)
		addPositionStartEnd(assignmentStatementNode, n.Pos(), n.End(), fileset)

	// https://golang.org/pkg/go/ast/#GoStmt
	// A GoStmt node represents a go statement.
	case *ast.GoStmt:
		goStatementNode := parent.CreateElement("GoStatement")
		Walk(goStatementNode, n.Call, fileset, info)
		addPositionStartEnd(goStatementNode, n.Pos(), n.End(), fileset)

	case *ast.DeferStmt:
		// https://golang.org/pkg/go/ast/#DeferStmt
		// A DeferStmt node represents a defer statement.
		deferStatementNode := parent.CreateElement("DeferStatement")
		Walk(deferStatementNode, n.Call, fileset, info)
		addPositionStartEnd(deferStatementNode, n.Pos(), n.End(), fileset)

	case *ast.ReturnStmt:
		// https://golang.org/pkg/go/ast/#ReturnStmt
		// A ReturnStmt node represents a return statement.
		returnStatementNode := parent.CreateElement("ReturnStatement")
		walkExprList(returnStatementNode, n.Results, fileset, info)
		addPositionStartEnd(returnStatementNode, n.Pos(), n.End(), fileset)

	case *ast.BranchStmt:
		// https://golang.org/pkg/go/ast/#BranchStmt
		// A BranchStmt node represents a break, continue, goto, or fallthrough statement.
		var branchStatementNode *etree.Element
		if n.Tok == token.BREAK {
			branchStatementNode = parent.CreateElement("BreakStatement")
		} else if n.Tok == token.CONTINUE {
			branchStatementNode = parent.CreateElement("ContinueStatement")
		} else if n.Tok == token.GOTO {
			branchStatementNode = parent.CreateElement("GotoStatement")
			branchStatementNode.CreateAttr("Label", n.Label.Name)
		} else {
			branchStatementNode = parent.CreateElement("FallthroughStatement")
		}
		addPositionStartEnd(branchStatementNode, n.Pos(), n.End(), fileset)

	case *ast.BlockStmt:
		// https://golang.org/pkg/go/ast/#BlockStmt
		// A BlockStmt node represents a braced statement list.
		blockStatementNode := parent.CreateElement("BlockStatement")
		walkStmtList(blockStatementNode, n.List, fileset, info)
		addPositionStartEnd(blockStatementNode, n.Pos(), n.End(), fileset)

	case *ast.IfStmt:
		// https://golang.org/pkg/go/ast/#IfStmt
		// An IfStmt node represents an if statement or an if else statement
		ifStatementNode := parent.CreateElement("IfStatement")
		if n.Init != nil {
			Walk(ifStatementNode, n.Init, fileset, info)
		}
		Walk(ifStatementNode, n.Cond, fileset, info)
		Walk(ifStatementNode, n.Body, fileset, info)
		if n.Else != nil {
			Walk(ifStatementNode, n.Else, fileset, info)
		}
		addPositionStartEnd(ifStatementNode, n.Pos(), n.End(), fileset)

	case *ast.CaseClause:
		// https://golang.org/pkg/go/ast/#CaseClause
		// A CaseClause represents a case of an expression or type switch statement.
		caseClauseNode := parent.CreateElement("CaseClause")
		walkExprList(caseClauseNode, n.List, fileset, info)
		walkStmtList(caseClauseNode, n.Body, fileset, info)
		addPositionStartEnd(caseClauseNode, n.Pos(), n.End(), fileset)

	case *ast.SwitchStmt:
		// https://golang.org/pkg/go/ast/#SwitchStmt
		// A SwitchStmt node represents an expression switch statement.
		switchStatementNode := parent.CreateElement("SwitchStatement")
		if n.Init != nil {
			Walk(switchStatementNode, n.Init, fileset, info)
		}
		if n.Tag != nil {
			Walk(switchStatementNode, n.Tag, fileset, info)
		}
		Walk(parent, n.Body, fileset, info)
		addPositionStartEnd(switchStatementNode, n.Pos(), n.End(), fileset)

	case *ast.TypeSwitchStmt:
		// https://golang.org/pkg/go/ast/#TypeSwitchStmt
		// A TypeSwitchStmt node represents a type switch statement.
		typeSwitchStatementNode := parent.CreateElement("TypeSwitchStatement")
		if n.Init != nil {
			Walk(typeSwitchStatementNode, n.Init, fileset, info)
		}
		Walk(typeSwitchStatementNode, n.Assign, fileset, info)
		Walk(typeSwitchStatementNode, n.Body, fileset, info)
		addPositionStartEnd(typeSwitchStatementNode, n.Pos(), n.End(), fileset)

	case *ast.CommClause:
		// https://golang.org/pkg/go/ast/#CommClause
		// A CommClause node represents a case of a select statement.
		commClauseNode := parent.CreateElement("CommClause")
		if n.Comm != nil {
			Walk(commClauseNode, n.Comm, fileset, info)
		}
		walkStmtList(commClauseNode, n.Body, fileset, info)
		addPositionStartEnd(commClauseNode, n.Pos(), n.End(), fileset)

	case *ast.SelectStmt:
		// https://golang.org/pkg/go/ast/#SelectStmt
		// A SelectStmt node represents a select statement.
		selectStatementNode := parent.CreateElement("SelectStatement")
		Walk(selectStatementNode, n.Body, fileset, info)
		addPositionStartEnd(selectStatementNode, n.Pos(), n.End(), fileset)

	case *ast.ForStmt:
		// https://golang.org/pkg/go/ast/#ForStmt
		// A ForStmt represents a for statement.
		forStatementNode := parent.CreateElement("ForStatement")
		if n.Init != nil {
			Walk(forStatementNode, n.Init, fileset, info)
		}
		if n.Cond != nil {
			Walk(forStatementNode, n.Cond, fileset, info)
		}
		if n.Post != nil {
			Walk(forStatementNode, n.Post, fileset, info)
		}
		Walk(forStatementNode, n.Body, fileset, info)
		addPositionStartEnd(forStatementNode, n.Pos(), n.End(), fileset)

	case *ast.RangeStmt:
		// https://golang.org/pkg/go/ast/#RangeStmt
		// A RangeStmt represents a for statement with a range clause.
		rangeStatementNode := parent.CreateElement("RangeStatement")
		if n.Key != nil {
			Walk(rangeStatementNode, n.Key, fileset, info)
		}
		if n.Value != nil {
			Walk(rangeStatementNode, n.Value, fileset, info)
		}
		Walk(rangeStatementNode, n.X, fileset, info)
		Walk(rangeStatementNode, n.Body, fileset, info)
		addPositionStartEnd(rangeStatementNode, n.Pos(), n.End(), fileset)

	// Declarations
	case *ast.ImportSpec:
		importSpecNode := parent.CreateElement("Item")

		if n.Name != nil {
			importSpecNode.CreateAttr("Name", n.Name.Name)
		}
		importSpecNode.CreateAttr("Path", stripQuotes(n.Path.Value))

		if n.Doc != nil {
			Walk(importSpecNode, n.Doc, fileset, info)
		}

		// if n.Name != nil {
		// 	Walk(importSpecNode, n.Name, fileset)
		// }
		// Walk(parent, n.Path, fileset)
		if n.Comment != nil {
			Walk(importSpecNode, n.Comment, fileset, info)
		}
		addPositionStartEnd(importSpecNode, n.Pos(), n.End(), fileset)

	case *ast.ValueSpec:
		// https://golang.org/pkg/go/ast/#ValueSpec
		// A ValueSpec node represents a constant or variable declaration (ConstSpec or VarSpec production).
		valueSpecNode := parent.CreateElement("ValueSpec")
		if n.Doc != nil {
			Walk(valueSpecNode, n.Doc, fileset, info)
		}
		walkIdentList(valueSpecNode, n.Names, fileset, info)
		if n.Type != nil {
			Walk(valueSpecNode, n.Type, fileset, info)
		}
		walkExprList(valueSpecNode, n.Values, fileset, info)
		if n.Comment != nil {
			Walk(valueSpecNode, n.Comment, fileset, info)
		}
		addPositionStartEnd(valueSpecNode, n.Pos(), n.End(), fileset)

	case *ast.TypeSpec:
		// https://golang.org/pkg/go/ast/#TypeSpec
		// A TypeSpec node represents a type declaration (TypeSpec production).
		itemNode := parent.CreateElement("Item")
		itemNode.CreateAttr("Name", n.Name.Name)

		if n.Doc != nil {
			Walk(itemNode, n.Doc, fileset, info)
		}
		// Walk(itemNode, n.Name, fileset)
		Walk(itemNode, n.Type, fileset, info)
		if n.Comment != nil {
			Walk(itemNode, n.Comment, fileset, info)
		}
		addPositionStartEnd(itemNode, n.Pos(), n.End(), fileset)

	case *ast.BadDecl:
		badDeclNode := parent.CreateElement("BadDeclaration")
		addPositionStartEnd(badDeclNode, n.Pos(), n.End(), fileset)

	case *ast.GenDecl:
		var declkind string = ""
		if n.Tok == token.IMPORT {
			declkind = "Import"
		} else if n.Tok == token.CONST {
			declkind = "Constant"
		} else if n.Tok == token.TYPE {
			declkind = "Type"
		} else if n.Tok == token.VAR {
			declkind = "Var"
		}

		declNode := parent.CreateElement(declkind)

		if n.Doc != nil {
			Walk(declNode, n.Doc, fileset, info)
		}
		for _, s := range n.Specs {
			Walk(declNode, s, fileset, info)
		}
		addPositionStartEnd(declNode, n.Pos(), n.End(), fileset)

	case *ast.FuncDecl:
		funcNode := parent.CreateElement("Func")
		funcNode.CreateAttr("Name", n.Name.Name)

		if n.Doc != nil {
			Walk(funcNode, n.Doc, fileset, info)
		}
		if n.Recv != nil {
			Walk(funcNode, n.Recv, fileset, info)
		}
		// Walk(funcNode, n.Name, fileset)
		Walk(funcNode, n.Type, fileset, info)
		if n.Body != nil {
			Walk(funcNode, n.Body, fileset, info)
		}
		addPositionStartEnd(funcNode, n.Pos(), n.End(), fileset)

	// Files and packages
	case *ast.File:
		fileNode := parent.CreateElement("File")
		fileNode.CreateAttr("Package", n.Name.Name)
		fileNode.CreateAttr("Filename", fileset.File(n.Package).Name())

		file, err := os.Open(fileset.File(n.Package).Name())
		if err != nil {
			log.Fatal(err)
		}

		source, err := ioutil.ReadAll(file)
		// Encode the file in Base64, since this XML implementation does not
		// quote illegal characters (like <, > etc.).
		encoded := base64.StdEncoding.EncodeToString([]byte(source))
		fileNode.CreateAttr("Source", encoded)

		defer func() {
			if err = file.Close(); err != nil {
				log.Fatal(err)
			}
		}()

		if n.Doc != nil {
			Walk(fileNode, n.Doc, fileset, info)
		}
		// Walk(parent, n.Name, fileset)
		walkDeclList(fileNode, n.Decls, fileset, info)
		// don't walk n.Comments - they have been
		// visited already through the individual
		// nodes
		addPositionStartEnd(fileNode, n.Pos(), n.End(), fileset)

	case *ast.Package:
		packageNode := parent.CreateElement("Package")
		packageNode.CreateAttr("Language", "go")
		packageNode.CreateAttr("Name", n.Name)

		// Do what is needed to implement type resolution.
		fileRefs := []*ast.File{}
		for _, v := range n.Files {
			fileRefs = append(fileRefs, v)
		}

		conf := types.Config{Importer: importer.Default()}
		conf.Check(n.Name, fileset, fileRefs, info)

		for _, f := range n.Files {
			Walk(packageNode, f, fileset, info)
		}
		addPositionStartEnd(packageNode, n.Pos(), n.End(), fileset)

	default:
		panic(fmt.Sprintf("ast.Walk: unexpected node type %T", n))
	}
}
