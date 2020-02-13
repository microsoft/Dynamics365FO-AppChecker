# Copyright (c) Microsoft Corporation.
# Licensed under the MIT license.

import unittest
import ast
from xml.etree import ElementTree
import xmlnodegenerator

# The most up to date description of the AST is
# https://docs.python.org/3.6/library/ast.html

def createTree(source: str):
    tree = ast.parse(source)
    generator = xmlnodegenerator.XmlNodeGenerator(source)
    root = generator.visitModule(tree)
    return root

# Statements

class ClassDefinitionTest(unittest.TestCase):
    def testClassWithExtension(self):
        root = createTree('''class MyClass(object):
    def foo(self, arg: str):
        pass''')

        classDefinitionNode = root.find("Statements/Class[@Name='MyClass']")
        assert classDefinitionNode != None

        extension = classDefinitionNode.find("Bases/Name[@Id='object']")
        assert extension != None

    def testClassNoExtension(self):
        root = createTree('''class MyClass:
    def foo():
        pass''')

        classDefinitionNode = root.find("Statements/Class[@Name='MyClass']")
        assert classDefinitionNode != None

        extension = root.find("Statements/Class/Bases/*")
        assert extension is None

    def testClassWithAttributedExtension(self):
        root = createTree('''class FuncCallVisitor(ast.NodeVisitor):
    def foo():
        pass''')

        classDefinitionNode = root.find("Statements/Class[@Name='FuncCallVisitor']")
        assert classDefinitionNode != None

        extension = classDefinitionNode.find("Bases/Attribute[@Id='NodeVisitor']/Name[@Id='ast']")
        assert extension != None

    def testClassWithComments(self):
        root = createTree('''class Bar:
    'This is a comment'
    def foo():
        pass''')

        classDefinitionNode = root.find("Statements/Class[@Name='Bar']")
        assert classDefinitionNode != None

        classDefinitionNode = root.find("Statements/Class[@Comment='This is a comment']")
        assert classDefinitionNode != None

class FunctionDefTest(unittest.TestCase):
    def testFunctionDefNoArgs(self):
        root = createTree('''def foo(): pass''')

        functionDefinitionNode = root.find("Statements/FunctionDefinition[@Name='foo']")
        assert functionDefinitionNode != None

        # There are no parameters:
        functionDefinitionNode = root.find("Statements/FunctionDefinition[@Name='foo']/Arguments")
        assert functionDefinitionNode != None

        functionDefinitionNode = root.find("Statements/FunctionDefinition[@Name='foo']/Arguments/*")
        assert functionDefinitionNode is None

    def testFunctionDefWithComment(self):
        root = createTree('''
def foo():
    'Interesting function'
    pass''')

        functionDefinitionNode = root.find("Statements/FunctionDefinition[@Name='foo']")
        assert functionDefinitionNode != None

        functionDefinitionNode = root.find("Statements/FunctionDefinition[@Comment='Interesting function']")
        assert functionDefinitionNode != None

    def testFunctionDefSimpleArgs(self):
        root =  createTree('''def foo(self, banana): pass''')

        functionDefinitionNode = root.find("Statements/FunctionDefinition[@Name='foo']")
        assert functionDefinitionNode != None

        # There are no parameters:
        argumentsNode = functionDefinitionNode.find("Arguments")
        assert argumentsNode != None

        argumentNode = argumentsNode.find("Argument[@Name='self']")
        assert argumentNode != None

        argumentNode = argumentsNode.find("Argument[@Name='banana']")
        assert argumentNode != None

    def testFunctionDefNoArgsWithReturn(self):
        root = createTree('''def foo() ->None: pass''')

        functionDefinitionNode = root.find("Statements/FunctionDefinition[@Name='foo']")
        assert functionDefinitionNode != None

        # There are no parameters:
        functionDefinitionNode = root.find("Statements/FunctionDefinition[@Name='foo']/Arguments")
        assert functionDefinitionNode != None

        functionDefinitionNode = root.find("Statements/FunctionDefinition[@Name='foo']/Arguments/*")
        assert functionDefinitionNode is None

        # The return annotation is 'None'
        functionDefinitionNode = root.find("Statements/FunctionDefinition[@Name='foo']/Returns/NameConstant[@Name='None']")
        assert functionDefinitionNode != None

    def testFunctionDefDefaultArgs(self):
        root =  createTree('''def foo(self, banana=1, orange=""): pass''')

        functionDefinitionNode = root.find("Statements/FunctionDefinition[@Name='foo']")
        assert functionDefinitionNode != None

        # There are no parameters:
        argumentsNode = functionDefinitionNode.find("Arguments")
        assert argumentsNode != None

        argumentNode = argumentsNode.find("Argument[@Name='self']")
        assert argumentNode != None

        argumentNode = argumentsNode.find("Argument[@Name='banana']/NumberLiteral[@Value='1']")
        assert argumentNode != None

        argumentNode = argumentsNode.find("Argument[@Name='orange']/StringLiteral[@Value='']")
        assert argumentNode != None

    def testFunctionDefStarredArgs(self):
        root =  createTree('''def foo(self, *arg1, **arg2): pass''')
        # In this syntax, the *arg means that all the positional parameters should
        # be passed as a list, while the **args causes the keyword parameters to be
        # collected in a dictionary (in the args2 parameter).

        functionDefinitionNode = root.find("Statements/FunctionDefinition[@Name='foo']")
        assert functionDefinitionNode != None

        argumentsNode = functionDefinitionNode.find("Arguments")
        assert argumentsNode != None

        argumentNode = argumentsNode.find("Argument[@Name='self']")
        assert argumentNode != None

        argumentNode = argumentsNode.find("Vararg/Argument[@Name='arg1']")
        assert argumentNode != None

        argumentNode = argumentsNode.find("KeywordArg/Argument[@Name='arg2']")
        assert argumentNode != None

    def testFunctionDefAnnotatedArgs(self):
        root = createTree('''def foo(self: int, banana: str) -> str: pass''')

        functionDefinitionNode = root.find("Statements/FunctionDefinition[@Name='foo']")
        assert functionDefinitionNode != None

        # There are no parameters:
        argumentsNode = functionDefinitionNode.find("Arguments")
        assert argumentsNode != None

        argumentNode = argumentsNode.find("Argument[@Name='self']")
        assert argumentNode != None
        parameterAnnotationNode = argumentNode.find("Annotation")
        assert parameterAnnotationNode != None

        argumentNode = argumentsNode.find("Argument[@Name='banana']")
        assert argumentNode != None
        parameterAnnotationNode = argumentNode.find("Annotation")
        assert parameterAnnotationNode != None

        functionDefinitionNode = root.find("Statements/FunctionDefinition[@Name='foo']/Returns/Name[@Id='str']")
        assert functionDefinitionNode != None

    def testAsyncFunctionDefWithPredefReturn(self):
        root = createTree('''async def foo() -> None: pass''')

        functionDefinitionNode = root.find("Statements/AsyncFunctionDefinition[@Name='foo']")
        assert functionDefinitionNode != None

        # There are no parameters:
        argumentsNode = functionDefinitionNode.find("Arguments")
        assert argumentsNode is None

        functionDefinitionNode = root.find("Statements/FunctionDefinition[@Name='foo']/Returns[@Type='None']")
        assert functionDefinitionNode != None

    def testAsyncFunctionDefWithPredefReturn(self):
        root = createTree('''async def foo() -> str: pass''')

        functionDefinitionNode = root.find("Statements/AsyncFunctionDefinition[@Name='foo']")
        assert functionDefinitionNode != None

        # There are no parameters:
        argumentsNode = functionDefinitionNode.find("Arguments")
        assert argumentsNode != None

        functionDefinitionNode = root.find("Statements/AsyncFunctionDefinition[@Name='foo']/Returns[@Name='str']")
        assert functionDefinitionNode != None

class ImportTest(unittest.TestCase):
    def runTest(self):
        source = "import ast"
        root = createTree(source)

        # Check that the tree contains the a Module element.
        assert root.tag == 'Module'

        importNode = root.find("Statements/Import")

        assert importNode != None
        # assert s == source

class ImportFromTest(unittest.TestCase):
    def testSimpleImportFrom(self):
        root = createTree("from MyModule import Banana")

        importFromNode = root.find("Statements/ImportFrom[@Module='MyModule']")
        assert importFromNode != None

        namesNode = importFromNode.find("Names/Name[@Id='Banana']")
        assert namesNode != None

    def testMultipleImportFrom(self):
        root = createTree("from MyModule import Banana as b, Orange as s")

        importFromNode = root.find("Statements/ImportFrom[@Module='MyModule']")
        assert importFromNode != None

        namesNode = importFromNode.find("Names/Name[@Id='Banana']")
        assert namesNode != None

        namesNode = importFromNode.find("Names/Name[@AsName='b']")
        assert namesNode != None

        namesNode = importFromNode.find("Names/Name[@Id='Orange']")
        assert namesNode != None

        namesNode = importFromNode.find("Names/Name[@AsName='s']")
        assert namesNode != None

    def testImportFromAll(self):
        root = createTree("from MyModule import *")

        importFromNode = root.find("Statements/ImportFrom[@Module='MyModule']")
        assert importFromNode != None

        namesNode = importFromNode.find("Names/Name[@Id='*']")
        assert namesNode != None

    def testImportWithParentDirSpec(self):
        root = createTree("from .. import Banana")
        assert root != None

        node = root.find("Statements/ImportFrom/Names/Name[@Id='Banana']")
        assert node != None

class WhileTest(unittest.TestCase):
    def testSimpleWhile(self):
        root = createTree("while 1: pass")

        whileStatement = root.find("Statements/While")
        assert whileStatement

        testNode = whileStatement.find("NumberLiteral")
        assert testNode != None

        bodyNode = root.find("Statements/While/Statements[1]/Pass")
        assert bodyNode != None

        orelseNode = root.find("Statements/While/Statements[2]/*")
        assert orelseNode is None

    def testWhileWithOrElse(self):
        root = createTree('''while 1:
    pass
else: return''')

        whileStatement = root.find("Statements/While")
        assert whileStatement

        testNode = whileStatement.find("NumberLiteral")
        assert testNode != None

        bodyNode = root.find("Statements/While/Statements[1]/Pass")
        assert bodyNode != None

        orelseNode = root.find("Statements/While/Statements[2]/Return")
        assert orelseNode != None

class ForTest(unittest.TestCase):
    def testSimpleFor(self):
        root = createTree("for i in foo(10, banana=4): pass")

        forStatement = root.find("Statements/For")
        assert forStatement

        # TODO check the name (i in this case)
        # TODO Check the expression

        bodyNode = forStatement.find("Statements[1]/Pass")
        assert bodyNode != None

        orelseNode = forStatement.find("Statements[2]/*")
        assert orelseNode is None

    def testAsyncFor(self):
        root = createTree('''async def f():
    async for i in foo(10, banana=4):
        pass''')

        forStatement = root.find("Statements/AsyncFunctionDefinition/Statements/AsyncFor")
        assert forStatement

        bodyNode = forStatement.find("Statements/Pass")
        assert bodyNode != None

        orelseNode = forStatement.find("Statements[2]/*")
        assert orelseNode is None

class TryExceptionTest(unittest.TestCase):
    def testTryExceptionSingle(self):
        root = createTree('''try:
    return 1
except IOError:
    return 2''')

        node = root.find("Statements/TryExcept/Statements/Return/NumberLiteral[@Value='1']")
        assert node != None

        node = root.find("Statements/TryExcept/Handler/Statements/Return/NumberLiteral[@Value='2']")
        assert node != None

        node = root.find("Statements/TryExcept/Handler/Name[@Id='IOError']")
        assert node != None

    def testTryExceptionSingleWithAlias(self):
        root = createTree('''try:
    return 1
except IOError as name:
    return 2''')

        node = root.find("Statements/TryExcept/Handler[@Alias='name']")
        assert node != None

    def testTryExceptionMultiple(self):
        root = createTree('''try:
    return 1
except (IOError, ValueError):
    return 2''')

        node = root.find("Statements/TryExcept/Statements/Return/NumberLiteral[@Value='1']")
        assert node != None

        node = root.find("Statements/TryExcept/Handler/Statements/Return/NumberLiteral[@Value='2']")
        assert node != None

        node = root.find("Statements/TryExcept/Handler/Tuple/Name[@Id='IOError']")
        assert node != None
        node = root.find("Statements/TryExcept/Handler/Tuple/Name[@Id='ValueError']")
        assert node != None

    def testTryExceptionWithCatchAll(self):
        root = createTree('''try:
    return 1
except:
    return 2''')

        node = root.find("Statements/TryExcept/Statements/Return/NumberLiteral[@Value='1']")
        assert node != None

        node = root.find("Statements/TryExcept/Handler/Statements/Return/NumberLiteral[@Value='2']")
        assert node != None

        node = root.find("Statements/TryExcept/Handler/Name")
        assert node is None

    def testTryExceptionWithElse(self):
        root = createTree('''try:
    return 1
except:
    return 2
else:
    return 3''')

        node = root.find("Statements/TryExcept/Statements[1]/Return/NumberLiteral[@Value='1']")
        assert node != None

        node = root.find("Statements/TryExcept/Handler/Name")
        assert node is None

        node = root.find("Statements/TryExcept/Handler/Statements/Return/NumberLiteral[@Value='2']")
        assert node != None

        node = root.find("Statements/TryExcept/Statements[2]/Return/NumberLiteral[@Value='3']")
        assert node != None

    def testTryExceptionElseAndFinally(self):
        root = createTree('''try:
    return 1
except:
    return 2
else:
    return 3
finally:
    return 4''')

        node = root.find("Statements/TryExcept/Statements[1]/Return/NumberLiteral[@Value='1']")
        assert node != None

        node = root.find("Statements/TryExcept/Handler/Name")
        assert node is None

        node = root.find("Statements/TryExcept/Handler/Statements/Return/NumberLiteral[@Value='2']")
        assert node != None

        node = root.find("Statements/TryExcept/Statements[2]/Return/NumberLiteral[@Value='3']")
        assert node != None

        node = root.find("Statements/TryExcept/Statements[3]/Return/NumberLiteral[@Value='4']")
        assert node != None

class RaiseTest(unittest.TestCase):
    def testRaiseWithoutArguments(self):
        root = createTree("raise")

        node = root.find("Statements/Raise")
        assert node != None

    def testRaiseWithSingleArgument(self):
        root = createTree("raise IOError()")

        node = root.find("Statements/Raise/Call")
        assert node != None

    def testRaiseWithTwoArguments(self):
        root = createTree("raise IOError from original")

        node = root.find("Statements/Raise")
        assert node != None

class ReturnTests(unittest.TestCase):
    def testReturnNoValue(self):
        root = createTree("return")

        # Check that the tree contains the a Return element.
        assert root.findall("Statements/Return")

        # ... without any subtree for the expression
        assert not root.findall("Statements/Return/*")

    def testReturnWithValue(self):
        root = createTree("return 1")

        # Check that the tree contains the a Return element.
        returnNode = root.find("Statements/Return")
        assert returnNode != None

        # ... with a subtree for the expression
        returnNodeWithSubtree = root.findall("Statements/Return/NumberLiteral[@Value='1']")
        assert returnNodeWithSubtree != None

class PassTest(unittest.TestCase):
    def runTest(self):
        root = createTree("pass")

        passNode = root.find("Statements/Pass")
        assert passNode != None

class BreakTest(unittest.TestCase):
    def runTest(self):
        root = createTree("break")

        node = root.find("Statements/Break")
        assert node != None

class ContinueTest(unittest.TestCase):
    def runTest(self):
        root = createTree("continue")

        # Check that the tree contains the a Return element.
        node = root.find("Statements/Continue")
        assert node != None

class WithTest(unittest.TestCase):
    def testWithSimple(self):
        root = createTree('''with open("x.txt") as f: pass''')

        node = root.find("Statements/With")
        assert node != None

        node = root.find("Statements/With/Item/Name[@Id='f']")
        assert node != None

        node = root.find("Statements/With/Item/Call/Name[@Id='open']")
        assert node != None

        node = root.find("Statements/With/Statements/Pass")
        assert node != None

    def testWithAsync(self):
        root = createTree('''async def foo():
    async with open("x.txt") as f:
        pass''')

        withNode = root.find("Statements/AsyncFunctionDefinition/Statements/AsyncWith")
        assert withNode != None

        node = withNode.find("Item/Name[@Id='f']")
        assert node != None

        node = withNode.find("Item/Call/Name[@Id='open']")
        assert node != None

        node = withNode.find("Statements/Pass")
        assert node != None

class IfTest(unittest.TestCase):
    def runTest(self):
        root = createTree('''if 1:
    return 10
else:
    return 20''')

        assert root.find("Statements/If")

        ifPart = root.find("Statements/If/Statements/Return/NumberLiteral[@Value='10']")
        elsePart = root.find("Statements/If/Statements/Return/NumberLiteral[@Value='20']")

        # ... with a subtree for the expression
        assert ifPart != None, "If part was not found"
        assert elsePart != None, "Else part not found"

class AssertTest(unittest.TestCase):
    def testAssertSimple(self):
        root = createTree("assert a is None")

        node = root.find("Statements/Assert/Is")
        assert node != None

    def testAssertWithExplanation(self):
        root = createTree('''assert a is None, "a should not be empty"''')

        node = root.find("Statements/Assert/Is")
        assert node != None

        node = root.find("Statements/Assert/StringLiteral")
        assert node != None

class AssignTest(unittest.TestCase):
    def testAssignSimple(self):
        root = createTree("a = 3")

        node = root.find("Statements/Assign/NumberLiteral[@Value='3']")
        assert node != None

    def testAssignMultiple(self):
        root = createTree("[a, b] = 3, 4")

        node = root.find("Statements/Assign/List/Name[@Id='a']")
        assert node != None
        node = root.find("Statements/Assign/List/Name[@Id='b']")
        assert node != None

        node = root.find("Statements/Assign/Tuple/NumberLiteral[@Value='3']")
        assert node != None

        node = root.find("Statements/Assign/Tuple/NumberLiteral[@Value='4']")
        assert node != None

    def testAugmentedAssignPlus(self):
        root = createTree("a += 3")

        node = root.find("Statements/AugmentedAssign[@op='Plus']/NumberLiteral[@Value='3']")
        assert node != None

    def testAugmentedAssignMinus(self):
        root = createTree("a -= 3")

        node = root.find("Statements/AugmentedAssign[@op='Minus']/NumberLiteral[@Value='3']")
        assert node != None

    def testAugmentedAssignMultiply(self):
        root = createTree("a *= 3")

        node = root.find("Statements/AugmentedAssign[@op='Multiply']/NumberLiteral[@Value='3']")
        assert node != None

    def testAugmentedAssignDivide(self):
        root = createTree("a /= 3")

        node = root.find("Statements/AugmentedAssign[@op='Divide']/NumberLiteral[@Value='3']")
        assert node != None

    def testAugmentedAssignMod(self):
        root = createTree("a %= 3")

        node = root.find("Statements/AugmentedAssign[@op='Mod']/NumberLiteral[@Value='3']")
        assert node != None

    def testAugmentedAssignMod(self):
        root = createTree("a **= 3")

        node = root.find("Statements/AugmentedAssign[@op='Power']/NumberLiteral[@Value='3']")
        assert node != None

    def testAugmentedAssignMod(self):
        root = createTree("a <<= 3")

        node = root.find("Statements/AugmentedAssign[@op='LeftShift']/NumberLiteral[@Value='3']")
        assert node != None

    def testAugmentedAssignMod(self):
        root = createTree("a >>= 3")

        node = root.find("Statements/AugmentedAssign[@op='RightShift']/NumberLiteral[@Value='3']")
        assert node != None

    def testAugmentedAssignMod(self):
        root = createTree("a &= 3")

        node = root.find("Statements/AugmentedAssign[@op='And']/NumberLiteral[@Value='3']")
        assert node != None

    def testAugmentedAssignMod(self):
        root = createTree("a %= 3")

        node = root.find("Statements/AugmentedAssign[@op='Mod']/NumberLiteral[@Value='3']")
        assert node != None

    def testAugmentedAssignMod(self):
        root = createTree("a ^= 3")

        node = root.find("Statements/AugmentedAssign[@op='Xor']/NumberLiteral[@Value='3']")
        assert node != None


    def testAugmentedAssignFloorDiv(self):
        root = createTree("a //= 3")

        node = root.find("Statements/AugmentedAssign[@op='FloorDiv']/NumberLiteral[@Value='3']")
        assert node != None

    def testAnnotatedAssignment(self): # TODO
        root = createTree("v: int = 44")
        assert root != None

        node = root.find("Statements/AnnotatedAssign")
        assert node != None

        node = root.find("Statements/AnnotatedAssign[@Simple='1']")
        assert node != None

        node = root.find("Statements/AnnotatedAssign/*[1][@Id='v']")
        assert node != None

        node = root.find("Statements/AnnotatedAssign/*[2][@Id='int']")
        assert node != None

        node = root.find("Statements/AnnotatedAssign/NumberLiteral[@Value='44']")
        assert node != None

class DelTest(unittest.TestCase):
    def testDel(self):
        root = createTree("del a")

        node = root.find("Statements/Del/Name[@Id='a']")
        assert node != None

class ScopeTest(unittest.TestCase):
    def testGlobal(self):
        root = createTree("global a,b")
        assert root != None, "Unable to parse the global statement"

        node = root.find("Statements/Global")
        assert node != None, "Did not find global statement"

        # Check the Identifier children
        node = root.find("Statements/Global/Identifier[@Name='a']")
        assert node != None, "Did not find first global identifier"

        node = root.find("Statements/Global/Identifier[@Name='b']")
        assert node != None, "Did not find second global identifier"

    def testNonlocal(self):
        root = createTree("nonlocal a,b")
        assert root != None, "Unable to parse the nonlocal statement"

        node = root.find("Statements/Nonlocal")
        assert node != None, "Did not find nonlocal statement"

        # Check the Identifier children
        node = root.find("Statements/Nonlocal/Identifier[@Name='a']")
        assert node != None, "Did not find first nonlocal identifier"

        node = root.find("Statements/Nonlocal/Identifier[@Name='b']")
        assert node != None, "Did not find second nonlocal identifier"

# Expression tests
class LiteralTest(unittest.TestCase):
    def testJoinedStr(self):
        root = createTree('''f"{1}{2}"''')
        assert root != None, "No joined string found"

        node = root.find("Statements/ExpressionStatement/JoinedStr")
        assert node != None, "Tree does not contain joined string"

        node = root.find("Statements/ExpressionStatement/JoinedStr/FormattedValue/NumberLiteral[@Value='1']")
        assert node != None, "First number literal not found"
        node = root.find("Statements/ExpressionStatement/JoinedStr/FormattedValue/NumberLiteral[@Value='2']")
        assert node != None, "Second number literal not found"

class AddTest(unittest.TestCase):
    def runTest(self):
        root = createTree('''1 + 2''')

        node = root.find("Statements/ExpressionStatement/Add")
        assert node != None, "No add node found"

class SubtractTest(unittest.TestCase):
    def runTest(self):
        root = createTree('''1 - 2''')

        node = root.find("Statements/ExpressionStatement/Subtract")
        assert node != None, "No subtract node found"

class MultiplyTest(unittest.TestCase):
    def runTest(self):
        root = createTree('''1 * 2''')

        node = root.find("Statements/ExpressionStatement/Multiply")
        assert node != None, "No multiply node found"

class DivideTest(unittest.TestCase):
    def runTest(self):
        root = createTree('''1 / 2''')

        node = root.find("Statements/ExpressionStatement/Divide")
        assert node != None, "No divide node found"

class FloorDivideTest(unittest.TestCase):
    def runTest(self):
        root = createTree('''1 // 2''')

        node = root.find("Statements/ExpressionStatement/FloorDivide")
        assert node != None, "No floor divide node found"

class PowerTest(unittest.TestCase):
    def runTest(self):
        root = createTree('''2 ** 3''')

        node = root.find("Statements/ExpressionStatement/Power")
        assert node != None, "No power node found"

class BitAndTest(unittest.TestCase):
    def runTest(self):
        root = createTree('''2 & 3''')

        node = root.find("Statements/ExpressionStatement/BitwiseAnd")
        assert node != None, "No bitwise and node found"

class BitOrTest(unittest.TestCase):
    def runTest(self):
        root = createTree('''2 | 3''')

        node = root.find("Statements/ExpressionStatement/BitwiseOr")
        assert node != None, "No bitwise or node found"

class BitXorTest(unittest.TestCase):
    def runTest(self):
        root = createTree('''2 ^ 3''')

        node = root.find("Statements/ExpressionStatement/BitwiseXor")
        assert node != None, "No bitwise xor node found"

class BitLeftShiftTest(unittest.TestCase):
    def runTest(self):
        root = createTree('''2 << 3''')

        node = root.find("Statements/ExpressionStatement/LeftShift")
        assert node != None, "No left shift node found"

class BitRightShiftTest(unittest.TestCase):
    def runTest(self):
        root = createTree('''2 >> 3''')

        node = root.find("Statements/ExpressionStatement/RightShift")
        assert node != None, "No right shift node found"

class MonadicOperatorTests(unittest.TestCase):
    def testMonadicMinus(self):
        root = createTree('''-1''')

        node = root.find("Statements/ExpressionStatement/MonadicMinus")
        assert node != None, "No monadic minus node found"

    def testMonadicPlus(self):
        root = createTree('''+1''')

        node = root.find("Statements/ExpressionStatement/MonadicPlus")
        assert node != None, "No monadic plus node found"

    def testMonadicInvert(self):
        root = createTree('''~1''')

        node = root.find("Statements/ExpressionStatement/Invert")
        assert node != None, "No monadic invert (~) node found"

    def testMonadicNot(self):
        root = createTree('''not True''')

        node = root.find("Statements/ExpressionStatement/Not")
        assert node != None, "No monadic not node found"

class LogicalOperatorTests(unittest.TestCase):
    def testAndOperator(self):
        root = createTree('''1 and 2''')

        node = root.find("Statements/ExpressionStatement/And")
        assert node != None

    def testOrOperator(self):
        root = createTree('''1 or 2''')

        node = root.find("Statements/ExpressionStatement/Or")
        assert node != None

class RelationalOperatorTests(unittest.TestCase):
    def testRelationalEquals(self):
        root = createTree('''1 == 2''')

        node = root.find("Statements/ExpressionStatement/Equals/NumberLiteral[@Value='1']")
        assert node != None, "Left node of == node not found"

        node = root.find("Statements/ExpressionStatement/Equals/NumberLiteral[@Value='2']")
        assert node != None, "Right node of == node not found"

    def testRelationalNotEquals(self):
        root = createTree('''1 != 2''')

        node = root.find("Statements/ExpressionStatement/NotEquals/NumberLiteral[@Value='1']")
        assert node != None, "Left node of != node not found"

        node = root.find("Statements/ExpressionStatement/NotEquals/NumberLiteral[@Value='2']")
        assert node != None, "Right node of != node not found"

    def testRelationalGreaterThan(self):
        root = createTree('''1 > 2''')

        node = root.find("Statements/ExpressionStatement/GreaterThan/NumberLiteral[@Value='1']")
        assert node != None, "Left node of > node not found"

        node = root.find("Statements/ExpressionStatement/GreaterThan/NumberLiteral[@Value='2']")
        assert node != None, "Right node of > node not found"

    def testRelationalGreaterOrEqual(self):
        root = createTree('''1 >= 2''')

        node = root.find("Statements/ExpressionStatement/GreaterThanOrEqual/NumberLiteral[@Value='1']")
        assert node != None, "Left node of >= node not found"

        node = root.find("Statements/ExpressionStatement/GreaterThanOrEqual/NumberLiteral[@Value='2']")
        assert node != None, "Right node of >= node not found"

    def testRelationalLessThan(self):
        root = createTree('''1 < 2''')

        node = root.find("Statements/ExpressionStatement/LessThan/NumberLiteral[@Value='1']")
        assert node != None, "Left node of < node not found"

        node = root.find("Statements/ExpressionStatement/LessThan/NumberLiteral[@Value='2']")
        assert node != None, "Right node of < node not found"

    def testRelationalLessThanOrEqual(self):
        root = createTree('''1 <= 2''')

        node = root.find("Statements/ExpressionStatement/LessThanOrEqual/NumberLiteral[@Value='1']")
        assert node != None, "Left node of <= node not found"

        node = root.find("Statements/ExpressionStatement/LessThanOrEqual/NumberLiteral[@Value='2']")
        assert node != None, "Right node of <= node not found"

    def testRelationalBetween(self):
        root = createTree('''1 < x < 2''')

        node = root.find("Statements/ExpressionStatement/LessThan/NumberLiteral[@Value='1']")
        assert node != None, "Left node of < node not found"

        node = root.find("Statements/ExpressionStatement/LessThan/LessThan/Name[@Id='x']")
        assert node != None, "Right node of < node not found"

        node = root.find("Statements/ExpressionStatement/LessThan/LessThan/NumberLiteral[@Value='2']")
        assert node != None, "Right node of < node not found"

class TernaryIfExpressionTests(unittest.TestCase):
    def testIfExpression(self):
        root = createTree("a if condition else b")

        node = root.find("Statements/ExpressionStatement/Conditional")
        assert node != None
        assert len(node) == 3

        node = root.find("Statements/ExpressionStatement/Conditional/Name[@Id='a']")
        assert node != None
        node = root.find("Statements/ExpressionStatement/Conditional/Name[@Id='condition']")
        assert node != None
        node = root.find("Statements/ExpressionStatement/Conditional/Name[@Id='b']")
        assert node != None

class CallTest(unittest.TestCase):
    def testCallDefaultAndPositionalArg(self):
        root = createTree("foo(1,3,5, sep='.', end='<', flush=True)")

        printNode = root.find("Statements/ExpressionStatement/Call/Name[@Id='foo']")
        assert printNode != None

        positionalParameters = root.findall('Statements/ExpressionStatement/Call/PositionalArgument')
        assert len(positionalParameters) == 3

        keywordParameters = root.findall('Statements/ExpressionStatement/Call/KeywordArgument')
        assert len(keywordParameters) == 3

    def testCallWithAttribute(self):
        root = createTree("compile(ast.PyCF_ONLY_AST)")

        attr = root.find("Statements/ExpressionStatement/Call/PositionalArgument/Attribute[@Id='PyCF_ONLY_AST']")
        assert attr != None

        name = attr.find("Name[@Id='ast']")
        assert name != None

    def testCallWithParameterLists(self):
        root = createTree("Foo(*args, **kwargs)")

        call = root.find("Statements/ExpressionStatement/Call");
        assert call != None

        positional = call.find("PositionalArgument/Starred/Name[@Id='args']")
        assert positional != None

        keyword = call.find("KeywordArgument/Name[@Id='kwargs']")
        assert keyword != None

class SliceTest(unittest.TestCase):
    def testSliceNone(self):
        root = createTree("a[:]")
        assert root != None

        slice = root.find("Statements/ExpressionStatement/Subscript/Slice")
        assert slice != None

        node = slice.find("Lower/*")
        assert node is None

        node = slice.find("Upper/*")
        assert node is None

        node = slice.find("Step/*")
        assert node is None

    def testSliceOnlyLower(self):
        root = createTree("a[1:]")
        assert root != None

        slice = root.find("Statements/ExpressionStatement/Subscript/Slice")
        assert slice != None

        node = slice.find("Lower/NumberLiteral[@Value='1']")
        assert node != None

        node = slice.find("Upper/*")
        assert node is None

        node = slice.find("Step/*")
        assert node is None

    def testSliceOnlyUpper(self):
        root = createTree("a[:10]")
        assert root != None

        slice = root.find("Statements/ExpressionStatement/Subscript/Slice")
        assert slice != None

        node = slice.find("Lower/*")
        assert node is None

        node = slice.find("Upper/NumberLiteral[@Value='10']")
        assert node != None

        node = slice.find("Step/*")
        assert node is None

    def testSliceLowerAndUpper(self):
        root = createTree("a[1:10]")
        assert root != None

        slice = root.find("Statements/ExpressionStatement/Subscript/Slice")
        assert slice != None

        node = slice.find("Lower/NumberLiteral[@Value='1']")
        assert node != None

        node = slice.find("Upper/NumberLiteral[@Value='10']")
        assert node != None

        node = slice.find("Step/*")
        assert node is None

    def testSliceNoneWithStep(self):
        root = createTree("a[::1]")
        assert root != None

        slice = root.find("Statements/ExpressionStatement/Subscript/Slice")
        assert slice != None

        node = slice.find("Lower/*")
        assert node is None

        node = slice.find("Upper/*")
        assert node is None

        node = slice.find("Step/NumberLiteral[@Value='1']")
        assert node != None

    def testSliceOnlyLowerWithStep(self):
        root = createTree("a[1::1]")
        assert root != None

        slice = root.find("Statements/ExpressionStatement/Subscript/Slice")
        assert slice != None

        node = slice.find("Lower/NumberLiteral[@Value='1']")
        assert node != None

        node = slice.find("Upper/*")
        assert node is None

        node = slice.find("Step/NumberLiteral[@Value='1']")
        assert node != None

    def testSliceOnlyUpperWithStep(self):
        root = createTree("a[:10:1]")
        assert root != None

        slice = root.find("Statements/ExpressionStatement/Subscript/Slice")
        assert slice != None

        node = slice.find("Lower/*")
        assert node is None

        node = slice.find("Upper/NumberLiteral[@Value='10']")
        assert node != None

        node = slice.find("Step/NumberLiteral[@Value='1']")
        assert node != None

    def testSliceLowerAndUpperWithStep(self):
        root = createTree("a[1:10:1]")
        assert root != None
        slice = root.find("Statements/ExpressionStatement/Subscript/Slice")
        assert slice != None

        node = slice.find("Lower/NumberLiteral[@Value='1']")
        assert node != None

        node = slice.find("Upper/NumberLiteral[@Value='10']")
        assert node != None

        node = slice.find("Step/NumberLiteral[@Value='1']")
        assert node != None

    def testSliceExtended(self):
        root = createTree("a[0, 1:2, ...]")
        assert root != None

        node = root.find("Statements/ExpressionStatement/Subscript")
        assert node != None

        name = node.find("Name[@Id='a']")
        assert name != None

        ext = node.find("ExtSlice")
        assert ext != None

        firstIndex = ext.find("Index/NumberLiteral[@Value='0']")
        assert firstIndex != None

        firstSlice = ext.find("Slice/Lower/NumberLiteral[@Value='1']")
        assert firstSlice != None

        firstSlice = ext.find("Slice/Upper/NumberLiteral[@Value='2']")
        assert firstSlice != None

        ellipsis = ext.find("Index/Ellipsis")
        assert ellipsis != None

class StarTest(unittest.TestCase):
    def testStar(self):
        root = createTree("a, *b = (1, 2, 3)")
        assert root != None

        assign = root.find("Statements/Assign")
        assert assign != None

        a = assign.find("Tuple/Name[@Id='a']")
        assert a != None

        b = assign.find("Tuple/Starred/Name[@Id='b']")
        assert b != None

class ComprehensionTests(unittest.TestCase):
    def _checkComprehension(self, comprehension):
        name = comprehension.find("Name[@Id='x']")
        assert name != None

        function = comprehension.find("Call/Name[@Id='range']")
        assert function != None

        assert comprehension.find("Equals/NumberLiteral[@Value='0']") != None

    def testListComprehension(self):
        root = createTree("[x for x in range(10) if x % 2 == 0]")

        listComprehension = root.find("Statements/ExpressionStatement/ListComprehension")
        assert listComprehension != None

        name = listComprehension.find("Name[@Id='x']")
        assert name != None

        comprehension = listComprehension.find("Comprehension")
        assert comprehension != None

        self._checkComprehension(comprehension)

    def testSetComprehension(self):
        root = createTree("{x for x in range(10) if x % 2 == 0}")

        setComprehension = root.find("Statements/ExpressionStatement/SetComprehension")
        assert setComprehension != None

        name = setComprehension.find("Name[@Id='x']")
        assert name != None

        comprehension = setComprehension.find("Comprehension")
        assert comprehension != None

        self._checkComprehension(comprehension)

    def testDictComprehension(self):
        root = createTree("{x : chr(65+x) for x in range(10) if x % 2 == 0}")

        dictComprehension = root.find("Statements/ExpressionStatement/DictComprehension")
        assert dictComprehension != None

        name = dictComprehension.find("Name[@Id='x']")
        assert name != None

        comprehension = dictComprehension.find("Comprehension")
        assert comprehension != None

        self._checkComprehension(comprehension)

    def testGenerator(self):
        root = createTree("(x for x in range(10) if x % 2 == 0)")

        generator = root.find("Statements/ExpressionStatement/Generator")
        assert generator != None

        name = generator.find("Name[@Id='x']")
        assert name != None

        comprehension = generator.find("Comprehension")
        assert comprehension != None

        self._checkComprehension(comprehension)

class YieldTests(unittest.TestCase):
    def testYield(self):
        root = createTree("yield 1")
        assert root != None

        node = root.find("Statements/ExpressionStatement/Yield/NumberLiteral[@Value='1']")
        assert node != None

    def testYieldFrom(self):
        root = createTree("yield from generator()")
        assert root != None

        node = root.find("Statements/ExpressionStatement/YieldFrom/Call/Name[@Id='generator']")
        assert node != None

    def testAwait(self):
        root = createTree('''async def ping_local():
    return await ping_server('192.168.1.1')''')
        assert root != None

        node = root.find("Statements/AsyncFunctionDefinition/Statements/Return/Await/Call/Name[@Id='ping_server']")
        assert node != None

class NameConstantTests(unittest.TestCase):
    def testNoneConstant(self):
        root = createTree("None")

        node = root.find("Statements/ExpressionStatement/NameConstant[@Name='None']")
        assert root != None

    def testTrueConstant(self):
        root = createTree("True")

        node = root.find("Statements/ExpressionStatement/NameConstant[@Name='True']")
        assert root != None

    def testFalseConstant(self):
        root = createTree("False")

        node = root.find("Statements/ExpressionStatement/NameConstant[@Name='False']")
        assert root != None

class ListTests(unittest.TestCase):
    def testEmptyList(self):
        root = createTree("[]")

        node = root.find("Statements/ExpressionStatement/List")
        assert node != None

        node = root.find("Statements/ExpressionStatement/List/*")
        assert node is None

    def testNonEmptyList(self):
        root = createTree("[1, 2, 3]")

        node = root.find("Statements/ExpressionStatement/List/NumberLiteral[@Value='1']")
        assert node != None
        node = root.find("Statements/ExpressionStatement/List/NumberLiteral[@Value='2']")
        assert node != None
        node = root.find("Statements/ExpressionStatement/List/NumberLiteral[@Value='3']")
        assert node != None

class SetTests(unittest.TestCase):

    # Note that {} designates an empty dict, not an empty set.

    def testNonEmptySet(self):
        root = createTree("{1, 2, 3}")

        node = root.find("Statements/ExpressionStatement/Set/NumberLiteral[@Value='1']")
        assert node != None
        node = root.find("Statements/ExpressionStatement/Set/NumberLiteral[@Value='2']")
        assert node != None
        node = root.find("Statements/ExpressionStatement/Set/NumberLiteral[@Value='3']")
        assert node != None

class DictTests(unittest.TestCase):
    def testEmptyDict(self):
        root = createTree("{}")

        node = root.find("Statements/ExpressionStatement/Dict")
        assert node != None

        node = root.find("Statements/ExpressionStatement/Dict/*")
        assert node is None

    def testNonEmptyDict(self):
        root = createTree("{'one': 1, 'two': 2, 'three': 3}")

        node = root.find("Statements/ExpressionStatement/Dict/DictElement/StringLiteral[@Value='one']../NumberLiteral[@Value='1']")
        assert node != None
        node = root.find("Statements/ExpressionStatement/Dict/DictElement/StringLiteral[@Value='two']../NumberLiteral[@Value='2']")
        assert node != None
        node = root.find("Statements/ExpressionStatement/Dict/DictElement/StringLiteral[@Value='three']../NumberLiteral[@Value='3']")
        assert node != None

class TupleTests(unittest.TestCase):
    def testEmptyTuple(self):
        root = createTree("()")

        node = root.find("Statements/ExpressionStatement/Tuple")
        assert node != None

        node = root.find("Statements/ExpressionStatement/Tuple/*")
        assert node is None

    def testNonEmptyTuple(self):
        root = createTree("(1, 'one')")

        node = root.find("Statements/ExpressionStatement/Tuple/StringLiteral[@Value='one']../NumberLiteral[@Value='1']")
        assert node != None
        node = root.find("Statements/ExpressionStatement/Tuple/NumberLiteral[@Value='1']")
        assert node != None

class LambdaTests(unittest.TestCase):
    def testLambdaWithArgs(self):
        root = createTree("lambda x: x + 2")
        assert root != None

        lambdaNode = root.find("Statements/ExpressionStatement/Lambda")
        assert lambdaNode != None

        arg = lambdaNode.find("Arguments/Argument[@Name='x']")
        assert arg != None

        body = lambdaNode.find("Add")
        assert body != None

if __name__ == '__main__':
    unittest.main()
