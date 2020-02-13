# Copyright (c) Microsoft Corporation.
# Licensed under the MIT license.

# TODO
# Fixup the artifact such that it will work in the SE.

import ast
import xml.etree.ElementTree as ET

# The most up to date description of the AST is
# https://docs.python.org/3.6/library/ast.html

class XmlNodeGenerator(object):
    """Generates XML from the python abstract syntax tree"""

    def __init__(self, source: str):
        self._source = source

    def visitModule(self, moduleNode: ast.Module) -> ET.ElementTree:
        '''Visits modules'''
        result = ET.Element("Module")
        result.attrib["StartLine"] = "1"
        result.attrib["StartCol"] = "1"

        content = self.visitStatements(moduleNode.body)
        result.append(content)
        return result

    # Statements
    def visitStatements(self, statementList) -> ET.ElementTree:
        ''' Visit a list of statements '''
        result = ET.Element("Statements")

        for stmt in statementList:
            stmt = self.visitStatement(stmt)
            result.append(stmt)

        return result

    def visitStatement(self, statementNode) -> [ET.ElementTree]:
        ''' Visit any statement '''
        if isinstance(statementNode, ast.ClassDef):
            result = self.visitClassDefStatement(statementNode)
        elif isinstance(statementNode, ast.AsyncFunctionDef):
            result = self.visitAsyncFunctionDefStatement(statementNode)
        elif isinstance(statementNode, ast.FunctionDef):
            result = self.visitFunctionDefStatement(statementNode)
        elif isinstance(statementNode, ast.Return):
            result = self.visitReturnStatement(statementNode)
        elif isinstance(statementNode, ast.If):
            result = self.visitIfStatement(statementNode)
        elif isinstance(statementNode, ast.While):
            result = self.visitWhileStatement(statementNode)
        elif isinstance(statementNode, ast.For):
            result = self.visitForStatement(statementNode)
        elif isinstance(statementNode, ast.AsyncFor):
            result = self.visitAsyncForStatement(statementNode)
        elif isinstance(statementNode, ast.Try):
            result = self.visitTryExceptStatement(statementNode)
        elif isinstance(statementNode, ast.AsyncWith):
            result = self.visitAsyncWithStatement(statementNode)
        elif isinstance(statementNode, ast.With):
            result = self.visitWithStatement(statementNode)
        elif isinstance(statementNode, ast.Raise):
            result = self.visitRaiseStatement(statementNode)
        elif isinstance(statementNode, ast.Pass):
            result = self.visitPassStatement(statementNode)
        elif isinstance(statementNode, ast.Break):
            result = self.visitBreakStatement(statementNode)
        elif isinstance(statementNode, ast.Continue):
            result = self.visitContinueStatement(statementNode)
        elif isinstance(statementNode, ast.ImportFrom):
            result =  self.visitImportFromStatement(statementNode)
        elif isinstance(statementNode, ast.Import):
            result = self.visitImportStatement(statementNode)
        elif isinstance(statementNode, ast.Expr):
            result = self.visitExpressionStatement(statementNode)
        elif isinstance(statementNode, ast.Delete):
            result = self.visitDeleteStatement(statementNode)
        elif isinstance(statementNode, ast.Assert):
            result = self.visitAssertStatement(statementNode)
        elif isinstance(statementNode, ast.Assign):
            result = self.visitAssignmentStatement(statementNode)
        elif isinstance(statementNode, ast.AugAssign):
            result = self.visitAugmentedAssignmentStatement(statementNode)
        elif isinstance(statementNode, ast.AnnAssign):
            result = self.visitAnnotatedAssignmentStatement(statementNode)
        elif isinstance(statementNode, ast.Global):
            result = self.visitGlobalStatement(statementNode)
        elif isinstance(statementNode, ast.Nonlocal):
            result = self.visitNonlocalStatement(statementNode)
        else:
            raise NotImplementedError("Unknown statement type: " + str(type(statementNode)))

        result.attrib["StartLine"] = str(statementNode.lineno)
        result.attrib["StartCol"]  = str(statementNode.col_offset+1)
        # result.attrib["EndLine"] = str(statementNode.end_lineno)
        # result.attrib["EndCol"] = str(statementNode.end_col_offset)

        return result

    def visitClassDefStatement(self, classDefNode: ast.ClassDef) -> ET.ElementTree:
        ''' Visit a class definition
             ClassDef(identifier name, expr* bases, keyword* keywords, stmt* body, expr* decorator_list)'''

        result = ET.Element("Class")
        result.set("Name", classDefNode.name)

        bases = ET.Element("Bases")
        for base in classDefNode.bases:
            bases.append(self.visitExpression(base))
        result.append(bases)

        # todo: Keywords??

        for stmt in classDefNode.body:
            stmtNode = self.visitStatement(stmt)
            result.append(stmtNode)

        decoratorsNode = ET.Element("Decorators")
        for decorator in classDefNode.decorator_list:
            decoratorsNode.append(self.visitExpression(decorator))
        result.append(decoratorsNode)

        docString = ast.get_docstring(classDefNode, clean=True)
        if docString != None:
            result.set("Comment", docString)

        return result

    def visitFunctionDefStatement(self, functionDefinitionStatement: ast.FunctionDef):
        ''' Visits a function definition
            FunctionDef(identifier name, arguments args, stmt* body, expr* decorator_list)'''
        result = ET.Element("FunctionDefinition")
        result.set("Name", functionDefinitionStatement.name)

        if functionDefinitionStatement.returns:
            returnsNode = ET.Element("Returns")
            returnsNode.append(self.visitExpression(functionDefinitionStatement.returns))
            result.append(returnsNode)

        result.append(self.visitArguments(functionDefinitionStatement.args))
        result.append(self.visitStatements(functionDefinitionStatement.body))

        decorators = ET.Element("Decorators")
        for decoratorNode in functionDefinitionStatement.decorator_list:
            decorators.append(self.visitExpression(decoratorNode))
        result.append(decorators)

        docString = ast.get_docstring(functionDefinitionStatement, clean=True)
        if docString != None:
            result.set("Comment", docString)

        return result

    def visitAsyncFunctionDefStatement(self, functionDefinitionStatement: ast.FunctionDef):
        ''' Visits a function definition
            FunctionDef(identifier name, arguments args, stmt* body, expr* decorator_list)'''
        result = ET.Element("AsyncFunctionDefinition")
        result.set("Name", functionDefinitionStatement.name)

        if functionDefinitionStatement.returns:
            returnsNode = ET.Element("Returns")
            returnsNode.append(self.visitExpression(functionDefinitionStatement.returns))
            result.append(returnsNode)

        result.append(self.visitArguments(functionDefinitionStatement.args))
        result.append(self.visitStatements(functionDefinitionStatement.body))

        decorators = ET.Element("Decorators")
        for decoratorNode in functionDefinitionStatement.decorator_list:
            decoratorNode.append(self.visitExpression(decoratorNode))

        docString = ast.get_docstring(functionDefinitionStatement, clean=True)
        if docString != None:
            result.set("Comment", docString)

        result.append(decorators)
        return result

    def visitArguments(self, arguments: ast.arguments):
        ''' Visit an arguments definition
            arguments = (arg* args, arg? vararg, arg* kwonlyargs, expr* kw_defaults  arg? kwarg, expr* defaults) '''

        argumentsNode = ET.Element("Arguments")

        firstDefaultArg = len(arguments.args) - len(arguments.defaults) + 1
        argNo = 1
        for arg in arguments.args:
            argumentNode = self.visitArg(arg)
            argumentsNode.append(argumentNode)
            if argNo >= firstDefaultArg:
                defaultNode = arguments.defaults[argNo-firstDefaultArg]
                argumentNode.append(self.visitExpression(defaultNode))
            argNo += 1

        if arguments.vararg != None:
            vararg = ET.Element("Vararg")
            vararg.append(self.visitArg(arguments.vararg))
            argumentsNode.append(vararg)

        if arguments.kwonlyargs:
            kwonlyargs = ET.Element("KeywordOnlyArgs")
            for kwonlyarg in arguments.kwonlyargs:
                kwonlyargs.append(self.visitArg(kwonlyarg))
            argumentsNode.append(kwonlyargs)

        if arguments.kw_defaults:
            defaults = ET.Element("KeywordDefaults")
            for default in arguments.kw_defaults:
                kwonlyargs.append(self.visitExpression(default))
            argumentsNode.append(defaults)

        if arguments.kwarg != None:
            kwarg = ET.Element("KeywordArg")
            kwarg.append(self.visitArg(arguments.kwarg))
            argumentsNode.append(kwarg)

        return argumentsNode

    def visitArg(self, arg: ast.arg):
        ''' Visit an argument '''
        argumentNode = ET.Element("Argument")
        argumentNode.set("Name", arg.arg)
        if arg.annotation:
            annotationNode = ET.Element("Annotation")
            annotationNode.append(self.visitExpression(arg.annotation))
            argumentNode.append(annotationNode)

        return argumentNode

    def visitReturnStatement(self, returnStatement: ast.Return):
        ''' Visit a return statement
        Return(expr? value) '''
        result = ET.Element('Return')

        if returnStatement.value:
            result.append(self.visitExpression(returnStatement.value))

        return result

    def visitDeleteStatement(self, delStatement: ast.Del) -> ET.ElementTree:
        ''' Visit the del statement
            Delete(expr* targets) '''
        result = ET.Element("Del")

        for expr in delStatement.targets:
            result.append(self.visitExpression(expr))

        return result

    def visitTryExceptStatement(self, tryExceptStatement: ast.Try):
        ''' Visit a try except statement
            TryExcept(stmt* body, excepthandler* handlers, stmt* orelse, stmt* finalbody) '''
        result = ET.Element("TryExcept")

        result.append(self.visitStatements(tryExceptStatement.body))

        for handler in tryExceptStatement.handlers:
            result.append(self.visitExceptionHandler(handler))

        result.append(self.visitStatements(tryExceptStatement.orelse))
        result.append(self.visitStatements(tryExceptStatement.finalbody))

        return result

    def visitExceptionHandler(self, handler: ast.excepthandler):
        ''' excepthandler = ExceptHandler(expr? type, expr? name, stmt* body)
	                                      attributes (int lineno, int col_offset) '''
        result = ET.Element("Handler")
        result.set("StartLine", str(handler.lineno))
        result.set("StartCol", str(handler.col_offset + 1))

        if handler.type != None:
            result.append(self.visitExpression(handler.type))

        if handler.name != None:
            result.set("Alias", handler.name)

        result.append(self.visitStatements(handler.body))
        return result

    def visitRaiseStatement(self, node: ast.Raise) -> ET.ElementTree:
        ''' Visit the raise statement
            Raise(expr? cause, expr? exc) '''

        result = ET.Element("Raise")
        if node.exc != None:
            result.append(self.visitExpression(node.exc))

        if node.cause != None:
            result.append(self.visitExpression(node.cause))

        return result

    def visitImportStatement(self, importNode: ast.Import) -> ET.ElementTree:
        ''' Visit an import statement
       	    Import(alias* names) '''
        result = ET.Element("Import")

        for name in importNode.names:
            alias = ET.Element("Alias")
            alias.set("Name", name.name)
            result.append(alias)

        return result

    def visitImportFromStatement(self, importFromNode: ast.ImportFrom) -> ET.ElementTree:
        ''' Visit an import statement
            ImportFrom(identifier? module, alias* names, int? level)'''
        result = ET.Element("ImportFrom")

        if importFromNode.module != None:
            result.set("Module", importFromNode.module)

        names = ET.Element("Names")
        for name in importFromNode.names:
            nameNode = ET.Element("Name")
            nameNode.set("Id", name.name)

            if name.asname != None:
                nameNode.set("AsName", name.asname)
            names.append(nameNode)

        result.append(names)
        return result

    def visitForStatement(self, forNode: ast.For) -> ET.ElementTree:
        ''' Visit a For statement
            For(expr target, expr iter, stmt* body, stmt* orelse)'''
        result = ET.Element("For")
        result.append(self.visitExpression(forNode.target))
        result.append(self.visitExpression(forNode.iter))
        result.append(self.visitStatements(forNode.body))
        result.append(self.visitStatements(forNode.orelse))
        return result

    def visitAsyncForStatement(self, forNode: ast.AsyncFor) -> ET.ElementTree:
        ''' Visit an asynchronous For statement
            AsyncFor(expr target, expr iter, stmt* body, stmt* orelse) '''
        result = ET.Element("AsyncFor")
        result.append(self.visitExpression(forNode.target))
        result.append(self.visitExpression(forNode.iter))
        result.append(self.visitStatements(forNode.body))
        result.append(self.visitStatements(forNode.orelse))
        return result

    def visitWhileStatement(self, whileNode: ast.While) -> ET.ElementTree:
        ''' Visit a while statement
            While(expr test, stmt* body, stmt* orelse) '''
        result = ET.Element("While")
        result.append(self.visitExpression(whileNode.test))
        result.append(self.visitStatements(whileNode.body))
        result.append(self.visitStatements(whileNode.orelse))
        return result

    def visitIfStatement(self, ifStatement: ast.If) -> ET.ElementTree:
        ''' Visits an If statement
            If(expr test, stmt* body, stmt* orelse) '''
        result = ET.Element("If")

        result.append(self.visitExpression(ifStatement.test))
        result.append(self.visitStatements(ifStatement.body))
        result.append(self.visitStatements(ifStatement.orelse))

        return result

    def visitWithItem(self, node: ast.withitem) -> ET.ElementTree:
        ''' Visit a with item
            (expr context_expr, expr? optional_vars) '''
        result = ET.Element("Item")
        result.append(self.visitExpression(node.context_expr))

        if node.optional_vars != None:
            result.append(self.visitExpression(node.optional_vars))

        return result

    def visitWithStatement(self, node: ast.With):
        ''' Visits a with statement
            With(withitem* items, stmt* body) '''

        result = ET.Element("With")

        for item in node.items:
            result.append(self.visitWithItem(item))

        result.append(self.visitStatements(node.body))
        return result

    def visitAsyncWithStatement(self, node: ast.With):
        ''' Visits a with statement
            With(withitem* items, stmt* body) '''

        result = ET.Element("AsyncWith")

        for item in node.items:
            result.append(self.visitWithItem(item))

        result.append(self.visitStatements(node.body))
        return result

#	      -- Doesn't capture requirement that locals must be
#	      -- defined if globals is
#	      -- still supports use as a function!
#	      | Exec(expr body, expr? globals, expr? locals)

    def visitPassStatement(self, passStatement: ast.Pass) -> ET.ElementTree:
        result = ET.Element("Pass")
        return result

    def visitBreakStatement(self, breakStatement: ast.Break) -> ET.ElementTree:
        result = ET.Element("Break")
        return result

    def visitContinueStatement(self, continueStatement: ast.Continue) -> ET.ElementTree:
        result = ET.Element("Continue")
        return result

    def visitAssertStatement(self, assertStatement: ast.Assert) -> ET.ElementTree:
        ''' Visit the Assert statement
            Assert(expr test, expr? msg) '''
        result = ET.Element("Assert")
        result.append(self.visitExpression(assertStatement.test))

        if assertStatement.msg != None:
            result.append(self.visitExpression(assertStatement.msg))
        return result

    def visitExpressionStatement(self, expressionStatement: ast.Expr) -> ET.ElementTree:
        ''' Visits an expression statement
            Expr(expr value)'''
        result = ET.Element("ExpressionStatement")
        result.append(self.visitExpression(expressionStatement.value))
        return result

    def visitAssignmentStatement(self, assignmentStatement: ast.Assign):
        ''' Visits an assignment statement
            Assign(expr* targets, expr value) '''
        result = ET.Element("Assign")

        for expr in assignmentStatement.targets:
            result.append(self.visitExpression(expr))

        result.append(self.visitExpression(assignmentStatement.value))
        return result

    def visitAugmentedAssignmentStatement(self, assignmentStatement: ast.Assign):
        ''' Visits an augmented assignment statement
            AugAssign(expr target, operator op, expr value) '''
        result = ET.Element("AugmentedAssign")
        result.append(self.visitExpression(assignmentStatement.target))

        if isinstance(assignmentStatement.op, ast.Add):
            result.set("op", "Plus") # +=
        elif isinstance(assignmentStatement.op, ast.Sub):
            result.set("op", "Minus") # -=
        elif isinstance(assignmentStatement.op, ast.Mult):
            result.set("op", "Multiply") # *=
        elif isinstance(assignmentStatement.op, ast.Div):
            result.set("op", "Divide") # /=
        elif isinstance(assignmentStatement.op, ast.Mod):
            result.set("op", "Mod") # %=
        elif isinstance(assignmentStatement.op, ast.Pow):
            result.set("op", "Power") # **=
        elif isinstance(assignmentStatement.op, ast.LShift):
            result.set("op", "LeftShift") # >>=
        elif isinstance(assignmentStatement.op, ast.RShift):
            result.set("op", "RightShift") # <<=
        elif isinstance(assignmentStatement.op, ast.And):
            result.set("op", "And") # &=
        elif isinstance(assignmentStatement.op, ast.Or):
            result.set("op", "Or") # |=
        elif isinstance(assignmentStatement.op, ast.BitXor):
            result.set("op", "Xor") # ^=
        elif isinstance(assignmentStatement.op, ast.FloorDiv):
            result.set("op", "FloorDiv") # //=
        else:
            raise NotImplementedError("Unimplemented augmented operator: " + str(type(assignmentStatement.op)))

        result.append(self.visitExpression(assignmentStatement.value))
        return result

    def visitAnnotatedAssignmentStatement(self, assignmentStatement: ast.AnnAssign):
        ''' Visits an annotated assignment statement
            AnnAssign(expr target, expr annotation, expr? value, int simple) '''
        result = ET.Element("AnnotatedAssign")

        result.append(self.visitExpression(assignmentStatement.target))
        result.append(self.visitExpression(assignmentStatement.annotation))

        if assignmentStatement.value != None:
            result.append(self.visitExpression(assignmentStatement.value))

        result.set("Simple", str(assignmentStatement.simple))

        return result

    def visitGlobalStatement(self, globalStatement: ast.Global) -> ET.Element:
        '''Visits a global statement, disambiguating a variable reference:
           Global(identifier* names)'''
        result = ET.Element("Global")

        for e in globalStatement.names:
            node = ET.Element("Identifier")
            node.set("Name", e)
            result.append(node)

        return result

    def visitNonlocalStatement(self, statement: ast.Nonlocal) -> ET.Element:
        '''Visits the nonlocal statement:
           Nonlocal(identifier* names)'''
        result = ET.Element("Nonlocal")

        for e in statement.names:
            node = ET.Element("Identifier")
            node.set("Name", e)
            result.append(node)

        return result

    # Expressions

    #expr =
    #     -- the grammar constrains where yield expressions can occur
    #     | Bytes(bytes s)
    #     | Constant(constant value)

    #     -- the following expression can appear in assignment context
    #     | Attribute(expr value, identifier attr, expr_context ctx)

    def visitExpression(self, expression):
        ''' Visits any expression '''
        if isinstance(expression, ast.Num):
            result = self.visitNumberLiteral(expression)
        elif isinstance(expression, ast.Str):
            result = self.visitStringLiteral(expression)
        elif isinstance(expression, ast.Bytes):
            result = self.visitBytesLiteral(expression)
        elif isinstance(expression, ast.BinOp):
            result = self.visitBinOpExpression(expression)
        elif isinstance(expression, ast.BoolOp):
            result = self.visitBoolOpExpression(expression)
        elif isinstance(expression, ast.UnaryOp):
            result = self.visitUnaryOpExpression(expression)
        elif isinstance(expression, ast.Compare):
            result = self.visitCompareExpression(expression)
        elif isinstance(expression, ast.IfExp):
            result = self.visitTernaryIfExpression(expression)
        elif isinstance(expression, ast.Name):
            result = self.visitName(expression)
        elif isinstance(expression, ast.NameConstant):
            result = self.visitNameConstant(expression)
        elif isinstance(expression, ast.List):
            result = self.visitList(expression)
        elif isinstance(expression, ast.Set):
            result = self.visitSet(expression)
        elif isinstance(expression, ast.Dict):
            result = self.visitDict(expression)
        elif isinstance(expression, ast.Tuple):
            result = self.visitTuple(expression)
        elif isinstance(expression, ast.ListComp):
            result = self.visitListComprehension(expression)
        elif isinstance(expression, ast.SetComp):
            result = self.visitSetComprehension(expression)
        elif isinstance(expression, ast.DictComp):
            result = self.visitDictComprehension(expression)
        elif isinstance(expression, ast.Call):
            result = self.visitCall(expression)
        elif isinstance(expression, ast.Subscript):
            result = self.visitSubscript(expression)
        elif isinstance(expression, ast.Slice):
            result = self.visitSlice(expression)
        elif isinstance(expression, ast.Ellipsis):
            result = self.visitEllipsis(expression)
        elif isinstance(expression, ast.Attribute):
            result = self.visitAttribute(expression)
        elif isinstance(expression, ast.Starred):
            result = self.visitStarred(expression)
        elif isinstance(expression, ast.GeneratorExp):
            result = self.visitGenerator(expression)
        elif isinstance(expression, ast.Lambda):
            result = self.visitLambda(expression)
        elif isinstance(expression, ast.Yield):
            result = self.visitYield(expression)
        elif isinstance(expression, ast.YieldFrom):
            result = self.visitYieldFrom(expression)
        elif isinstance(expression, ast.Await):
            result = self.visitAwait(expression)
        elif isinstance(expression, ast.JoinedStr):
            result = self.visitJoinedStr(expression)
        elif isinstance(expression, ast.FormattedValue):
            result = self.visitFormattedValue(expression)
        else:
            raise NotImplementedError("This expression type is not implemented: " + str(type(expression)))

        result.attrib["StartLine"] = str(expression.lineno)
        result.attrib["StartCol"] = str(expression.col_offset+1)
        # result.attrib["EndLine"] = str(statementNode.end_lineno)
        # result.attrib["EndCol"] = str(statementNode.end_col_offset)

        return result

    def visitNumberLiteral(self, node: ast.Num) -> ET.ElementTree:
        ''' A number - integer, float, or complex. The n attribute stores the value,
        already converted to the relevant type. '''
        result = ET.Element("NumberLiteral")
        result.set("Value", str(node.n))
        return result

    def visitStringLiteral(self, node: ast.Str):
        ''' A string. The s attribute holds the value. In Python 2, the same type holds
        unicode strings too. '''
        result = ET.Element("StringLiteral")
        result.set("Value", node.s)
        return result

    def visitBytesLiteral(self, node: ast.Bytes):
        ''' Visit the byte literal, in its many forms '''
        result = ET.Element("BytesLiteral")
        result.set("Value", repr(node.s))

        for b in node.s:
            byte = ET.Element("Byte")
            byte.set("Value", str(b))
            result.append(byte)

        return result

    def visitJoinedStr (self, node: ast.JoinedStr) -> ET.ElementTree:
        '''Visit the joined string (i.e. the string with placeholders)
           JoinedStr(expr* values)'''
        result = ET.Element("JoinedStr")

        for e in node.values:
            result.append(self.visitExpression(e))

        return result

    def visitFormattedValue(self, node: ast.JoinedStr) -> ET.ElementTree:
        '''Visit the formatted value
           FormattedValue(expr value, int? conversion, expr? format_spec)'''
        result = ET.Element("FormattedValue")

        result.append(self.visitExpression(node.value))
        if node.conversion != None:
            result.set("Conversion", str(node.conversion))

        if node.format_spec != None:
            result.append(self.visitExpression(node.format_spec))

        return result



    def visitNameConstant(self, node: ast.NameConstant) -> ET.ElementTree:
        ''' Visit the name constant
            NameConstant(singleton value) where singleton: None, True or False '''
        result = ET.Element("NameConstant")
        result.set("Name", str(node.value))

        return result

    def visitName(self, node: ast.Name):
        result = ET.Element("Name")
        result.set("Id", node.id)
        result.set("Context", self.contextString(node.ctx))

        return result

    def contextString(self, node: ast.expr_context) -> str:
        ''' Returns the context as a string
            expr_context = Load | Store | Del | AugLoad | AugStore | Param '''
        if isinstance(node, ast.Load):
            return "load"
        elif isinstance(node, ast.Store):
            return "store"
        elif isinstance(node, ast.Del):
            return "del"
        elif isinstance(node, ast.AugLoad):
            return "augload"
        elif isinstance(node, ast.AugStore):
            return "augstore"
        elif isinstance(node, ast.Param):
            return "param"
        else:
            raise NotImplementedError("Unknown expression context: " + str(type(node)))

    def visitList(self, node: ast.List) -> ET.ElementTree:
        ''' Visit the list literal
            List(expr* elts, expr_context ctx) '''

        result = ET.Element("List")
        result.set("Context", self.contextString(node.ctx))

        for expr in node.elts:
            result.append(self.visitExpression(expr))

        return result

    def visitDict(self, node: ast.Set) -> ET.ElementTree:
        ''' Visit the set literal
            Dict(expr* keys, expr* values) '''

        result = ET.Element("Dict")
        for i in range(0, len(node.keys)):
            element = ET.Element("DictElement")
            element.append(self.visitExpression(node.keys[i]))
            element.append(self.visitExpression(node.values[i]))
            result.append(element)

        return result

    def visitSet(self, node: ast.Dict) -> ET.ElementTree:
        ''' Visit the dict literal
            Set(expr* e) '''

        result = ET.Element("Set")

        for expr in node.elts:
            result.append(self.visitExpression(expr))

        return result

    def visitTuple(self, node: ast.Tuple) -> ET.ElementTree:
        ''' Visit the tuple
            Tuple(expr* elts, expr_context ctx) '''
        result = ET.Element("Tuple")

        for expr in node.elts:
            result.append(self.visitExpression(expr))

        result.set( "Context", self.contextString(node.ctx))

        return result

    def visitCall(self, node: ast.Call):
        ''' A function call. func is the function, which will often be a Name or Attribute object.

        Call(expr func, expr* args, keyword* keywords)

        args holds a list of the arguments passed by position.
        keywords holds a list of keyword objects representing arguments passed by keyword.

        When compiling a Call node, args and keywords are required, but they can be empty lists. '''

        result = ET.Element("Call")
        result.append(self.visitExpression(node.func))

        for expr in node.args:
            positionalArgument = ET.Element("PositionalArgument")
            positionalArgument.append(self.visitExpression(expr))
            result.append(positionalArgument)

        for keyword in node.keywords:
            keywordArgument = ET.Element("KeywordArgument")
            if keyword.arg != None:
                keywordArgument.set("Name", keyword.arg)
            keywordArgument.append(self.visitExpression(keyword.value))
            result.append(keywordArgument)

        return result

    def visitSlice(self, node: ast.slice):
        ''' Visit the slice. A slice can be
            slice = Slice(expr? lower, expr? upper, expr? step)
          | ExtSlice(slice* dims)
          | Index(expr value) '''
        if isinstance(node, ast.Slice):
            result = ET.Element("Slice")

            start = ET.Element("Lower")
            if node.lower != None:
                start.append(self.visitExpression(node.lower))

            end = ET.Element("Upper")
            if node.upper != None:
                end.append(self.visitExpression(node.upper))

            step = ET.Element("Step")
            if node.step != None:
                step.append(self.visitExpression(node.step))

            result.append(start)
            result.append(end)
            result.append(step)
            return result
        elif isinstance(node, ast.ExtSlice):

            result = ET.Element("ExtSlice")
            for dim in node.dims:
                result.append(self.visitSlice(dim))
            return result

        elif isinstance(node, ast.Index):
            result = ET.Element("Index")
            result.append(self.visitExpression(node.value))
            return result
        else:
            raise NotImplementedError("This slice operator is not implemented")

    def visitEllipsis(self, node: ast.Ellipsis):
        result = ET.Element("Ellipsis")
        return result

    def visitSubscript(self, node: ast.Subscript):
        ''' Visit the subscript
            Subscript(expr value, slice slice, expr_context ctx) '''
        result = ET.Element("Subscript")

        result.set("Context", self.contextString(node.ctx))
        result.append(self.visitExpression(node.value))
        result.append(self.visitSlice(node.slice))
        return result

    def createCompareNode(self, op: ast.cmpop) -> ET.ElementTree:
        if isinstance(op, ast.Eq):
            result = ET.Element("Equals")
        elif isinstance(op, ast.NotEq):
            result = ET.Element("NotEquals")
        elif isinstance(op, ast.Lt):
            result = ET.Element("LessThan")
        elif isinstance(op, ast.LtE):
            result = ET.Element("LessThanOrEqual")
        elif isinstance(op, ast.Gt):
            result = ET.Element("GreaterThan")
        elif isinstance(op, ast.GtE):
            result = ET.Element("GreaterThanOrEqual")
        elif isinstance(op, ast.Is):
            result = ET.Element("Is")
        elif isinstance(op, ast.IsNot):
            result = ET.Element("IsNot")
        elif isinstance(op, ast.In):
            result = ET.Element("In")
        elif isinstance(op, ast.NotIn):
            result = ET.Element("IsNot")

        return result

    def createCompareTail(self, operators, comparators) -> ET.ElementTree:
        if operators:
            result = self.createCompareNode(operators[0])
            result.append(self.visitExpression(comparators[0]))
            result.append(self.createCompareTail(operators[1:], comparators[1:]))
            return result

        # There are no further operators
        return self.visitExpression(comparators[0])

    def visitCompareExpression(self, node: ast.Compare) -> ET.ElementTree:
        ''' Visit the compare exppression
            Compare(expr left, cmpop* ops, expr* comparators) '''

        result = self.createCompareNode(node.ops[0])
        result.append(self.visitExpression(node.left))
        result.append(self.createCompareTail(node.ops[1:], node.comparators))

        return result

    def visitUnaryOpExpression(self, node: ast.UnaryOp):
        ''' Visit the unary (i.e. monadic) operator '''
        if isinstance(node.op, ast.USub):
            result = ET.Element("MonadicMinus")
        elif isinstance(node.op, ast.UAdd):
            result = ET.Element("MonadicPlus")
        elif isinstance(node.op, ast.Not):
            result = ET.Element("Not")
        elif isinstance(node.op, ast.Invert):
            result = ET.Element("Invert")
        else:
            raise NotImplementedError("The unary operator is not implemented: " + str(type(node)))

        result.append(self.visitExpression(node.operand))
        return result

    def visitComprehension(self, node: ast.comprehension):
        ''' Visit the comprehension
            comprehension = (expr target, expr iter, expr* ifs, int is_async) '''
        result = ET.Element("Comprehension")
        result.set("Async", str(node.is_async))
        result.append(self.visitExpression(node.target))
        result.append(self.visitExpression(node.iter))

        for condition in node.ifs:
            result.append(self.visitExpression(condition))

        return result

    def visitListComprehension(self, node: ast.ListComp):
        ''' Visit the list comprehension
            ListComp(expr elt, comprehension* generators) '''
        result = ET.Element("ListComprehension")

        result.append(self.visitExpression(node.elt))
        for generator in node.generators:
            result.append(self.visitComprehension(generator))

        return result

    def visitSetComprehension(self, node: ast.ListComp):
        ''' Visit the list comprehension
            ListComp(expr elt, comprehension* generators) '''
        result = ET.Element("SetComprehension")

        result.append(self.visitExpression(node.elt))
        for generator in node.generators:
            result.append(self.visitComprehension(generator))

        return result

    def visitDictComprehension(self, node: ast.DictComp):
        ''' Visit the dictionary comprehension
            DictComp(expr key, expr value, comprehension* generators)  '''
        result = ET.Element("DictComprehension")

        result.append(self.visitExpression(node.key))
        result.append(self.visitExpression(node.value))

        for generator in node.generators:
            result.append(self.visitComprehension(generator))

        return result

    def visitLambda(self, node: ast.Lambda):
        ''' Visit the lambda expression.
            Lambda(arguments args, expr body) '''
        result = ET.Element("Lambda")
        result.append(self.visitArguments(node.args))
        result.append(self.visitExpression(node.body))
        return result

    def visitYield(self, node: ast.Yield):
        ''' Visit the yield expression
            Yield(expr? value) '''
        result = ET.Element("Yield")

        if node.value != None:
            result.append(self.visitExpression(node.value))

        return result

    def visitYieldFrom(self, node: ast.Yield):
        ''' Visit the yield from expression
            YieldFrom(expr value) '''
        result = ET.Element("YieldFrom")
        result.append(self.visitExpression(node.value))

        return result

    def visitAwait(self, node: ast.Await):
        ''' Visit the await expression
            Await(expr value) '''
        result = ET.Element("Await")
        result.append(self.visitExpression(node.value))

        return result

    def visitGenerator(self, node: ast.GeneratorExp):
        ''' Visit the generator expression
            GeneratorExp(expr elt, comprehension* generators) '''
        result = ET.Element("Generator")
        result.append(self.visitExpression(node.elt))

        for generator in node.generators:
            result.append(self.visitComprehension(generator))

        return result

    def visitBoolOpExpression(self, node: ast.BoolOp):
        ''' Visit the boolean binary operators (i.e. and, or)
            BoolOp(boolop op, expr* values)'''

        if isinstance(node.op, ast.And):
            return self.visitAndExpression(node.values)
        elif isinstance(node.op, ast.Or):
            return self.visitOrExpression(node.values)
        else:
            raise NotImplementedError("Unknown logical operator: " + str(type(node)))

    def visitAndExpression(self, values):
        result = ET.Element("And")

        for expr in values:
            result.append(self.visitExpression(expr))

        return result

    def visitOrExpression(self, values):
        result = ET.Element("Or")

        for expr in values:
            result.append(self.visitExpression(expr))

        return result

    def visitBinOpExpression(self, node: ast.BinOp):
        ''' Visit the binary operator
            BinOp(expr left, operator op, expr right).
            The operators are
            Add Sub Mult Div FloorDiv Mod Pow LShift RShift BitOr BitXor BitAnd MatMult '''

        if isinstance(node.op, ast.Add):
            result = ET.Element("Add")
        elif isinstance(node.op, ast.Sub):
            result = ET.Element("Subtract")
        elif isinstance(node.op, ast.Mult):
            result = ET.Element("Multiply")
        elif isinstance(node.op, ast.Div):
            result = ET.Element("Divide")
        elif isinstance(node.op, ast.FloorDiv):
            result = ET.Element("FloorDivide")
        elif isinstance(node.op, ast.Mod):
            result = ET.Element("Modulo")
        elif isinstance(node.op, ast.Pow):
            result = ET.Element("Power")
        elif isinstance(node.op, ast.LShift):
            result = ET.Element("LeftShift")
        elif isinstance(node.op, ast.RShift):
            result = ET.Element("RightShift")
        elif isinstance(node.op, ast.BitOr):
            result = ET.Element("BitwiseOr")
        elif isinstance(node.op, ast.BitAnd):
            result = ET.Element("BitwiseAnd")
        elif isinstance(node.op, ast.BitXor):
            result = ET.Element("BitwiseXor")
        elif isinstance(node.op, ast.MatMult):
            result = ET.Element("MatrixMultiply")
        else:
            raise NotImplementedError("Unknown binary operator: " + str(type(node)))

        result.append(self.visitExpression(node.left))
        result.append(self.visitExpression(node.right))
        return result

    def visitAttribute(self, node: ast.Attribute) -> ET.ElementTree:
        ''' Visit the attribute
            Attribute(expr value, identifier attr, expr_context ctx) '''
        result = ET.Element("Attribute")

        result.append(self.visitExpression(node.value))
        result.set("Id", node.attr)
        result.set("Context", self.contextString(node.ctx))

        return result

    def visitStarred(self, node: ast.Starred):
        ''' Visit the starred variable
            Starred(expr value, expr_context ctx) '''
        result = ET.Element("Starred")
        result.set("Context", self.contextString(node.ctx))
        result.append(self.visitExpression(node.value))
        return result

    def visitTernaryIfExpression(self, node: ast.IfExp) -> ET.ElementTree:
        ''' Visit the ternary conditional operator
            IfExp(expr test, expr body, expr orelse) '''

        result = ET.Element("Conditional")
        result.append(self.visitExpression(node.test))
        result.append(self.visitExpression(node.body))
        result.append(self.visitExpression(node.orelse))
        return result
    # Misc



