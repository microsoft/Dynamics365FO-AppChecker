package main

import (
	"fmt"
	"go/ast"
	"go/token"
	"strconv"

	"github.com/beevik/etree"
)

func walkIdentList(parent *etree.Element, list []*ast.Ident, fileset *token.FileSet) {
	for _, x := range list {
		Walk(parent, x, fileset)
	}
}

func walkExprList(parent *etree.Element, list []ast.Expr, fileset *token.FileSet) {
	for _, x := range list {
		Walk(parent, x, fileset)
	}
}

func walkStmtList(parent *etree.Element, list []ast.Stmt, fileset *token.FileSet) {
	for _, x := range list {
		Walk(parent, x, fileset)
	}
}

func walkDeclList(parent *etree.Element, list []ast.Decl, fileset *token.FileSet) {
	for _, x := range list {
		Walk(parent, x, fileset)
	}
}

// Walk the node provided
func Walk(parent *etree.Element, node ast.Node, fileset *token.FileSet) {
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
			Walk(commentGroupNode, c, fileset)
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
			Walk(fieldNode, n.Doc, fileset)
		}

		namesNode := fieldNode.CreateElement("Names")
		walkIdentList(namesNode, n.Names, fileset)

		Walk(fieldNode, n.Type, fileset)
		if n.Tag != nil {
			Walk(fieldNode, n.Tag, fileset)
		}
		if n.Comment != nil {
			Walk(fieldNode, n.Comment, fileset)
		}
		addPositionStartEnd(fieldNode, n.Pos(), n.End(), fileset)

	// https://golang.org/pkg/go/ast/#FieldList
	// A FieldList represents a list of Fields, enclosed by parentheses or braces.
	case *ast.FieldList:
		fieldListNode := parent.CreateElement("Fields")

		for _, f := range n.List {
			Walk(fieldListNode, f, fileset)
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

		addPositionStartEnd(identNode, n.Pos(), n.End(), fileset)

	case *ast.BadExpr:
		// nothing to do

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
		addPositionStartEnd(literalNode, n.Pos(), n.End(), fileset)

	// https://golang.org/pkg/go/ast/#Ellipsis
	// An Ellipsis node stands for the "..." type in a parameter list or the "..." length in an array type.
	case *ast.Ellipsis:
		ellipsisNode := parent.CreateElement("Ellipsis")
		if n.Elt != nil {
			Walk(ellipsisNode, n.Elt, fileset)
		}

		addPositionStartEnd(ellipsisNode, n.Pos(), n.End(), fileset)

	// https://golang.org/pkg/go/ast/#FuncLit
	// A FuncLit node represents a function literal.
	case *ast.FuncLit:
		funcLitNode := parent.CreateElement("FuncLit")
		Walk(funcLitNode, n.Type, fileset)
		Walk(funcLitNode, n.Body, fileset)
		addPositionStartEnd(funcLitNode, n.Pos(), n.End(), fileset)

	// https://golang.org/pkg/go/ast/#CompositeLit
	// A CompositeLit node represents a composite literal, i.e. array of struct literals.
	case *ast.CompositeLit:
		compositeLiteral := parent.CreateElement("CompositeLiteral")
		compositeLiteral.CreateAttr("Incomplete", strconv.FormatBool(n.Incomplete))

		if n.Type != nil {
			Walk(compositeLiteral, n.Type, fileset)
		}
		walkExprList(compositeLiteral, n.Elts, fileset)
		addPositionStartEnd(compositeLiteral, n.Pos(), n.End(), fileset)

	case *ast.ParenExpr:
		Walk(parent, n.X, fileset)

	case *ast.SelectorExpr:
		// https://golang.org/pkg/go/ast/#SelectorExpr
		// A SelectorExpr node represents an expression followed by a selector.
		selectorExprNode := parent.CreateElement("Selector")
		Walk(selectorExprNode, n.X, fileset)
		Walk(selectorExprNode, n.Sel, fileset)
		addPositionStartEnd(selectorExprNode, n.Pos(), n.End(), fileset)

	case *ast.IndexExpr:
		// https://golang.org/pkg/go/ast/#IndexExpr
		// An IndexExpr node represents an expression followed by an index.
		indexExprNode := parent.CreateElement("Index")
		Walk(indexExprNode, n.X, fileset)
		Walk(indexExprNode, n.Index, fileset)
		addPositionStartEnd(indexExprNode, n.Pos(), n.End(), fileset)

	case *ast.SliceExpr:
		// https://golang.org/pkg/go/ast/#SliceExpr
		// A SliceExpr node represents an expression followed by slice indices.
		sliceExprNode := parent.CreateElement("Slice")
		Walk(sliceExprNode, n.X, fileset)
		if n.Low != nil {
			Walk(sliceExprNode, n.Low, fileset)
		}
		if n.High != nil {
			Walk(sliceExprNode, n.High, fileset)
		}
		if n.Max != nil {
			Walk(sliceExprNode, n.Max, fileset)
		}
		addPositionStartEnd(sliceExprNode, n.Pos(), n.End(), fileset)

	case *ast.TypeAssertExpr:
		// https://golang.org/pkg/go/ast/#TypeAssertExpr
		// A TypeAssertExpr node represents an expression followed by a type assertion.
		typeAssertExprNode := parent.CreateElement("TypeAssert")
		Walk(typeAssertExprNode, n.X, fileset)
		if n.Type != nil {
			Walk(typeAssertExprNode, n.Type, fileset)
		}
		addPositionStartEnd(typeAssertExprNode, n.Pos(), n.End(), fileset)

	case *ast.CallExpr:
		// https://golang.org/pkg/go/ast/#CallExpr
		// A CallExpr node represents an expression followed by an argument list.
		callExprNode := parent.CreateElement("Call")
		Walk(callExprNode, n.Fun, fileset)
		walkExprList(callExprNode, n.Args, fileset)
		addPositionStartEnd(callExprNode, n.Pos(), n.End(), fileset)

	case *ast.StarExpr:
		// https://golang.org/pkg/go/ast/#StarExpr
		// A StarExpr node represents an expression of the form "*" Expression. Semantically it
		// could be a unary "*" expression, or a pointer type.
		starExprNode := parent.CreateElement("Star")
		Walk(starExprNode, n.X, fileset)
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
			unaryExpressionKind = "Receieve"
		} else if n.Op == token.XOR {
			unaryExpressionKind = "Xor"
		} else {
			panic("Unknown unary operator ")
		}
		unaryExprNode := parent.CreateElement(unaryExpressionKind)
		Walk(unaryExprNode, n.X, fileset)
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
		Walk(binaryExpressionNode, n.X, fileset)
		Walk(binaryExpressionNode, n.Y, fileset)
		addPositionStartEnd(binaryExpressionNode, n.Pos(), n.End(), fileset)

	// https://golang.org/pkg/go/ast/#KeyValueExpr
	// A KeyValueExpr node represents (key : value) pairs in composite literals.
	case *ast.KeyValueExpr:
		keyValuePairNode := parent.CreateElement("KeyValuePair")
		Walk(keyValuePairNode, n.Key, fileset)
		Walk(keyValuePairNode, n.Value, fileset)
		addPositionStartEnd(keyValuePairNode, n.Pos(), n.End(), fileset)

		// Types
	// https://golang.org/pkg/go/ast/#ArrayType
	// An ArrayType node represents an array or slice type.
	case *ast.ArrayType:
		arrayTypeNode := parent.CreateElement("ArrayType")
		if n.Len != nil {
			Walk(arrayTypeNode, n.Len, fileset)
		}
		Walk(arrayTypeNode, n.Elt, fileset)
		addPositionStartEnd(arrayTypeNode, n.Pos(), n.End(), fileset)

	// https://golang.org/pkg/go/ast/#StructType
	// A StructType node represents a struct type.
	case *ast.StructType:
		structTypeNode := parent.CreateElement("Struct")
		structTypeNode.CreateAttr("Incomplete", strconv.FormatBool(n.Incomplete))

		Walk(structTypeNode, n.Fields, fileset)
		addPositionStartEnd(structTypeNode, n.Pos(), n.End(), fileset)

	// https://golang.org/pkg/go/ast/#FuncType
	// A FuncType node represents a function type.
	case *ast.FuncType:
		funcTypeNode := parent.CreateElement("FuncType")
		if n.Params != nil {
			Walk(funcTypeNode, n.Params, fileset)
		}
		if n.Results != nil {
			Walk(funcTypeNode, n.Results, fileset)
		}
		addPositionStartEnd(funcTypeNode, n.Pos(), n.End(), fileset)

	// https://golang.org/pkg/go/ast/#InterfaceType
	// An InterfaceType node represents an interface type.
	case *ast.InterfaceType:
		interfaceTypeNode := parent.CreateElement("InterfaceType")
		Walk(interfaceTypeNode, n.Methods, fileset)
		addPositionStartEnd(interfaceTypeNode, n.Pos(), n.End(), fileset)

	// https://golang.org/pkg/go/ast/#MapType
	// A MapType node represents a map type.
	case *ast.MapType:
		mapTypeNode := parent.CreateElement("MapType")
		Walk(mapTypeNode, n.Key, fileset)
		Walk(mapTypeNode, n.Value, fileset)
		addPositionStartEnd(mapTypeNode, n.Pos(), n.End(), fileset)

	// https://golang.org/pkg/go/ast/#ChanType
	// A ChanType node represents a channel type.
	case *ast.ChanType:
		chanTypeNode := parent.CreateElement("ChanType")
		Walk(parent, n.Value, fileset)
		addPositionStartEnd(chanTypeNode, n.Pos(), n.End(), fileset)

	// Statements
	case *ast.BadStmt:
		// nothing to do

	// https://golang.org/pkg/go/ast/#DeclStmt
	// A DeclStmt node represents a declaration in a statement list.
	case *ast.DeclStmt:
		declStatementNode := parent.CreateElement("DeclStatement")
		Walk(declStatementNode, n.Decl, fileset)
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
		Walk(labeledStatementNode, n.Stmt, fileset)
		addPositionStartEnd(labeledStatementNode, n.Pos(), n.End(), fileset)

	// https://golang.org/pkg/go/ast/#ExprStmt
	// An ExprStmt node represents a (stand-alone) expression in a statement list.
	case *ast.ExprStmt:
		exprStatementNode := parent.CreateElement("ExpressionStatement")
		Walk(exprStatementNode, n.X, fileset)
		addPositionStartEnd(exprStatementNode, n.Pos(), n.End(), fileset)

	// https://golang.org/pkg/go/ast/#SendStmt
	// A SendStmt node represents a send statement.
	case *ast.SendStmt:
		sendStatementNode := parent.CreateElement("SendStatement")
		Walk(sendStatementNode, n.Chan, fileset)
		Walk(sendStatementNode, n.Value, fileset)
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
		Walk(incDecStatementNode, n.X, fileset)
		addPositionStartEnd(incDecStatementNode, n.Pos(), n.End(), fileset)

	// https://golang.org/pkg/go/ast/#AssignStmt
	// An AssignStmt node represents an assignment or a short variable declaration.
	case *ast.AssignStmt:
		assignmentStatementNode := parent.CreateElement("AssignmentStatement")
		walkExprList(assignmentStatementNode, n.Lhs, fileset)
		walkExprList(assignmentStatementNode, n.Rhs, fileset)
		addPositionStartEnd(assignmentStatementNode, n.Pos(), n.End(), fileset)

	// https://golang.org/pkg/go/ast/#GoStmt
	// A GoStmt node represents a go statement.
	case *ast.GoStmt:
		goStatementNode := parent.CreateElement("GoStatement")
		Walk(goStatementNode, n.Call, fileset)
		addPositionStartEnd(goStatementNode, n.Pos(), n.End(), fileset)

	case *ast.DeferStmt:
		// https://golang.org/pkg/go/ast/#DeferStmt
		// A DeferStmt node represents a defer statement.
		deferStatementNode := parent.CreateElement("DeferStatement")
		Walk(deferStatementNode, n.Call, fileset)
		addPositionStartEnd(deferStatementNode, n.Pos(), n.End(), fileset)

	case *ast.ReturnStmt:
		// https://golang.org/pkg/go/ast/#ReturnStmt
		// A ReturnStmt node represents a return statement.
		returnStatementNode := parent.CreateElement("ReturnStatement")
		walkExprList(returnStatementNode, n.Results, fileset)
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
		walkStmtList(blockStatementNode, n.List, fileset)
		addPositionStartEnd(blockStatementNode, n.Pos(), n.End(), fileset)

	case *ast.IfStmt:
		// https://golang.org/pkg/go/ast/#IfStmt
		// An IfStmt node represents an if statement or an if else statement
		ifStatementNode := parent.CreateElement("IfStatement")
		if n.Init != nil {
			Walk(ifStatementNode, n.Init, fileset)
		}
		Walk(ifStatementNode, n.Cond, fileset)
		Walk(ifStatementNode, n.Body, fileset)
		if n.Else != nil {
			Walk(ifStatementNode, n.Else, fileset)
		}
		addPositionStartEnd(ifStatementNode, n.Pos(), n.End(), fileset)

	case *ast.CaseClause:
		// https://golang.org/pkg/go/ast/#CaseClause
		// A CaseClause represents a case of an expression or type switch statement.
		caseClauseNode := parent.CreateElement("CaseClause")
		walkExprList(caseClauseNode, n.List, fileset)
		walkStmtList(caseClauseNode, n.Body, fileset)
		addPositionStartEnd(caseClauseNode, n.Pos(), n.End(), fileset)

	case *ast.SwitchStmt:
		// https://golang.org/pkg/go/ast/#SwitchStmt
		// A SwitchStmt node represents an expression switch statement.
		switchStatementNode := parent.CreateElement("SwitchStatement")
		if n.Init != nil {
			Walk(switchStatementNode, n.Init, fileset)
		}
		if n.Tag != nil {
			Walk(switchStatementNode, n.Tag, fileset)
		}
		Walk(parent, n.Body, fileset)
		addPositionStartEnd(switchStatementNode, n.Pos(), n.End(), fileset)

	case *ast.TypeSwitchStmt:
		// https://golang.org/pkg/go/ast/#TypeSwitchStmt
		// A TypeSwitchStmt node represents a type switch statement.
		typeSwitchStatementNode := parent.CreateElement("TypeSwitchStatement")
		if n.Init != nil {
			Walk(typeSwitchStatementNode, n.Init, fileset)
		}
		Walk(typeSwitchStatementNode, n.Assign, fileset)
		Walk(typeSwitchStatementNode, n.Body, fileset)
		addPositionStartEnd(typeSwitchStatementNode, n.Pos(), n.End(), fileset)

	case *ast.CommClause:
		// https://golang.org/pkg/go/ast/#CommClause
		// A CommClause node represents a case of a select statement.
		commClauseNode := parent.CreateElement("CommClause")
		if n.Comm != nil {
			Walk(commClauseNode, n.Comm, fileset)
		}
		walkStmtList(commClauseNode, n.Body, fileset)
		addPositionStartEnd(commClauseNode, n.Pos(), n.End(), fileset)

	case *ast.SelectStmt:
		// https://golang.org/pkg/go/ast/#SelectStmt
		// A SelectStmt node represents a select statement.
		selectStatementNode := parent.CreateElement("SelectStatement")
		Walk(selectStatementNode, n.Body, fileset)
		addPositionStartEnd(selectStatementNode, n.Pos(), n.End(), fileset)

	case *ast.ForStmt:
		// https://golang.org/pkg/go/ast/#ForStmt
		// A ForStmt represents a for statement.
		forStatementNode := parent.CreateElement("ForStatement")
		if n.Init != nil {
			Walk(forStatementNode, n.Init, fileset)
		}
		if n.Cond != nil {
			Walk(forStatementNode, n.Cond, fileset)
		}
		if n.Post != nil {
			Walk(forStatementNode, n.Post, fileset)
		}
		Walk(forStatementNode, n.Body, fileset)
		addPositionStartEnd(forStatementNode, n.Pos(), n.End(), fileset)

	case *ast.RangeStmt:
		// https://golang.org/pkg/go/ast/#RangeStmt
		// A RangeStmt represents a for statement with a range clause.
		rangeStatementNode := parent.CreateElement("RangeStatement")
		if n.Key != nil {
			Walk(rangeStatementNode, n.Key, fileset)
		}
		if n.Value != nil {
			Walk(rangeStatementNode, n.Value, fileset)
		}
		Walk(rangeStatementNode, n.X, fileset)
		Walk(rangeStatementNode, n.Body, fileset)
		addPositionStartEnd(rangeStatementNode, n.Pos(), n.End(), fileset)

	// Declarations
	case *ast.ImportSpec:
		importSpecNode := parent.CreateElement("Item")

		if n.Name != nil {
			importSpecNode.CreateAttr("Name", n.Name.Name)
		}
		importSpecNode.CreateAttr("Path", stripQuotes(n.Path.Value))

		if n.Doc != nil {
			Walk(importSpecNode, n.Doc, fileset)
		}

		// if n.Name != nil {
		// 	Walk(importSpecNode, n.Name, fileset)
		// }
		// Walk(parent, n.Path, fileset)
		if n.Comment != nil {
			Walk(importSpecNode, n.Comment, fileset)
		}
		addPositionStartEnd(importSpecNode, n.Pos(), n.End(), fileset)

	case *ast.ValueSpec:
		// https://golang.org/pkg/go/ast/#ValueSpec
		// A ValueSpec node represents a constant or variable declaration (ConstSpec or VarSpec production).
		valueSpecNode := parent.CreateElement("ValueSpec")
		if n.Doc != nil {
			Walk(valueSpecNode, n.Doc, fileset)
		}
		walkIdentList(valueSpecNode, n.Names, fileset)
		if n.Type != nil {
			Walk(valueSpecNode, n.Type, fileset)
		}
		walkExprList(valueSpecNode, n.Values, fileset)
		if n.Comment != nil {
			Walk(valueSpecNode, n.Comment, fileset)
		}
		addPositionStartEnd(valueSpecNode, n.Pos(), n.End(), fileset)

	case *ast.TypeSpec:
		// https://golang.org/pkg/go/ast/#TypeSpec
		// A TypeSpec node represents a type declaration (TypeSpec production).
		itemNode := parent.CreateElement("Item")
		itemNode.CreateAttr("Name", n.Name.Name)

		if n.Doc != nil {
			Walk(itemNode, n.Doc, fileset)
		}
		// Walk(itemNode, n.Name, fileset)
		Walk(itemNode, n.Type, fileset)
		if n.Comment != nil {
			Walk(itemNode, n.Comment, fileset)
		}
		addPositionStartEnd(itemNode, n.Pos(), n.End(), fileset)

	case *ast.BadDecl:
		// nothing to do

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
			Walk(declNode, n.Doc, fileset)
		}
		for _, s := range n.Specs {
			Walk(declNode, s, fileset)
		}
		addPositionStartEnd(declNode, n.Pos(), n.End(), fileset)

	case *ast.FuncDecl:
		funcNode := parent.CreateElement("Func")
		funcNode.CreateAttr("Name", n.Name.Name)

		if n.Doc != nil {
			Walk(funcNode, n.Doc, fileset)
		}
		if n.Recv != nil {
			Walk(funcNode, n.Recv, fileset)
		}
		// Walk(funcNode, n.Name, fileset)
		Walk(funcNode, n.Type, fileset)
		if n.Body != nil {
			Walk(funcNode, n.Body, fileset)
		}
		addPositionStartEnd(funcNode, n.Pos(), n.End(), fileset)

	// Files and packages
	case *ast.File:
		fileNode := parent.CreateElement("File")
		fileNode.CreateAttr("Package", n.Name.Name)
		fileNode.CreateAttr("Filename", fileset.File(n.Package).Name())

		if n.Doc != nil {
			Walk(fileNode, n.Doc, fileset)
		}
		// Walk(parent, n.Name, fileset)
		walkDeclList(fileNode, n.Decls, fileset)
		// don't walk n.Comments - they have been
		// visited already through the individual
		// nodes
		addPositionStartEnd(fileNode, n.Pos(), n.End(), fileset)

	case *ast.Package:
		packageNode := parent.CreateElement("Package")
		packageNode.CreateAttr("Name", n.Name)

		for _, f := range n.Files {
			Walk(packageNode, f, fileset)
		}
		addPositionStartEnd(packageNode, n.Pos(), n.End(), fileset)

	default:
		panic(fmt.Sprintf("ast.Walk: unexpected node type %T", n))
	}
}
