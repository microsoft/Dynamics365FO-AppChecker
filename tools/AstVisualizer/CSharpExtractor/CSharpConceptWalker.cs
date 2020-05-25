// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpExtractor
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    using System.Xml.Linq;

    /// <summary>
    /// This class can be used if it is required to do traversal where individual notes are processed. 
    /// It also has a general Visit node method. First the VisitNode method is called, 
    /// and then the more specialized Visit* method is called.
    /// </summary>
    class CSharpConceptWalker : CSharpSyntaxWalker
    {
        private readonly XDocument document;
        private readonly CSharpCompilation compilation;
        private readonly SyntaxTree tree;
        private readonly SemanticModel semanticModel;
        private SymbolDisplayFormat symbolDisplayFormat;
        private readonly IDictionary<string, string> classBaseList = new Dictionary<string, string>();

        /// <summary>
        /// Indicates whether or not the source should be included in the node defining classes and 
        /// other top level compilation units. This should really be fetched from a settings file or
        /// an environment variable.
        /// </summary>
        private bool IncludeSource {  get { return true; } }

        /// <summary>
        /// This is the stack of XML elements built during extraction. This is needed because there
        /// is no payload information that can be passed to each visit* method
        /// </summary>
        private Stack<XElement> stack = new Stack<XElement>();

        public XDocument Document { get { return this.document;  } }

        private void AddLocationAttributes(XElement element, SyntaxNode node)
        {
            element.Add(new XAttribute("StartLine", node.GetLocation().GetLineSpan().StartLinePosition.Line+1));
            element.Add(new XAttribute("StartCol", node.GetLocation().GetLineSpan().StartLinePosition.Character + 1));
            element.Add(new XAttribute("EndLine", node.GetLocation().GetLineSpan().EndLinePosition.Line+1));
            element.Add(new XAttribute("EndCol", node.GetLocation().GetLineSpan().EndLinePosition.Character + 1));
        }

        private void AddTypeAttribute(ExpressionSyntax node, XElement element)
        {
            var expressionType = this.semanticModel.GetTypeInfo(node);
            element.Add(new XAttribute("Type", TypeName(expressionType.Type)));
        }

        private static string ElementName(SyntaxNode node)
        {
            return node.Kind().ToString();
        }

        private static XElement CreateElement(SyntaxNode node)
        {
            string nodeKind = ElementName(node);
            return new XElement(nodeKind);
        }

        private string TypeName(ITypeSymbol typeSymbol)
        {
            return typeSymbol?.ToDisplayString(this.symbolDisplayFormat) ?? string.Empty;
        }

        /// <summary>
        /// Creates an instance of trhe Concept walker for a compilationunit in the compilation
        /// </summary>
        /// <param name="compilation">The compilation under which this compilationunit is compiled.</param>
        /// <param name="tree">The compilation unit</param>
        public CSharpConceptWalker(CSharpCompilation compilation, SyntaxTree tree, XDocument compilationDocument)
        {
            this.compilation = compilation;
            this.tree = tree;
            this.semanticModel = compilation.GetSemanticModel(tree);

            this.symbolDisplayFormat = new SymbolDisplayFormat(
                miscellaneousOptions: SymbolDisplayMiscellaneousOptions.UseSpecialTypes,
                typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces);

            this.document = compilationDocument;
            stack.Push(compilationDocument.Root);
        }

        public override void Visit(SyntaxNode node)
        {
            // Console.WriteLine(ElementName(node));
            base.Visit(node);
        }

        /// <summary>
        /// Visits the given VisitAccessorDeclaration.
        /// </summary>
        /// <param name="node">The syntax node describing the VisitAccessorDeclaration.</param>
        public override void VisitAccessorDeclaration(AccessorDeclarationSyntax node)
        {
            var element = CreateElement(node);


            // TODO
            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitAccessorDeclaration(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitAccessorList.
        /// </summary>
        /// <param name="node">The syntax node describing this VisitAccessorList.</param>
        public override void VisitAccessorList(AccessorListSyntax node)
        {
            var element = CreateElement(node);


            // TODO
            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitAccessorList(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitAliasQualifiedName.
        /// </summary>
        /// <param name="node">The syntax node describing this VisitAliasQualifiedName.</param>
        public override void VisitAliasQualifiedName(AliasQualifiedNameSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitAliasQualifiedName(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitAnonymousMethodExpression.
        /// </summary>
        /// <param name="node">The node describing this VisitAnonymousMethodExpression.</param>
        public override void VisitAnonymousMethodExpression(AnonymousMethodExpressionSyntax node)
        {
            var element = CreateElement(node);


            // TODO
            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitAnonymousMethodExpression(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitAnonymousObjectCreationExpression.
        /// </summary>
        /// <param name="node">The node describing this VisitAnonymousObjectCreationExpression.</param>
        public override void VisitAnonymousObjectCreationExpression(AnonymousObjectCreationExpressionSyntax node)
        {
            var element = CreateElement(node);


            // TODO
            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitAnonymousObjectCreationExpression(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitAnonymousObjectMemberDeclarator.
        /// </summary>
        /// <param name="node">The node describing this VisitAnonymousObjectMemberDeclarator.</param>
        public override void VisitAnonymousObjectMemberDeclarator(AnonymousObjectMemberDeclaratorSyntax node)
        {
            var element = CreateElement(node);


            // TODO
            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitAnonymousObjectMemberDeclarator(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitArgument.
        /// </summary>
        /// <param name="node">The node describing this VisitArgument.</param>
        public override void VisitArgument(ArgumentSyntax node)
        {
            var element = CreateElement(node);


            // TODO
            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitArgument(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitArgumentList.
        /// </summary>
        /// <param name="node">The node describing this VisitArgumentList.</param>
        public override void VisitArgumentList(ArgumentListSyntax node)
        {
            var element = CreateElement(node);


            // TODO
            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitArgumentList(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitArrayCreationExpression.
        /// </summary>
        /// <param name="node">The node describing this VisitArrayCreationExpression.</param>
        public override void VisitArrayCreationExpression(ArrayCreationExpressionSyntax node)
        {
            var element = CreateElement(node);


            // TODO
            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitArrayCreationExpression(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitArrayRankSpecifier.
        /// </summary>
        /// <param name="node">The node describing this VisitArrayRankSpecifier.</param>
        public override void VisitArrayRankSpecifier(ArrayRankSpecifierSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitArrayRankSpecifier(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitArrayType.
        /// </summary>
        /// <param name="node">The node describing this VisitArrayType.</param>
        public override void VisitArrayType(ArrayTypeSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitArrayType(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitArrowExpressionClause.
        /// </summary>
        /// <param name="node">The node describing this VisitArrowExpressionClause.</param>
        public override void VisitArrowExpressionClause(ArrowExpressionClauseSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitArrowExpressionClause(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitAssignmentExpression.
        /// </summary>
        /// <param name="node">The node describing this VisitAssignmentExpression.</param>
        public override void VisitAssignmentExpression(AssignmentExpressionSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitAssignmentExpression(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitAttribute.
        /// </summary>
        /// <param name="node">The node describing this VisitAttribute.</param>
        public override void VisitAttribute(AttributeSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitAttribute(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitAttributeArgument.
        /// </summary>
        /// <param name="node">The node describing this VisitAttributeArgument.</param>
        public override void VisitAttributeArgument(AttributeArgumentSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitAttributeArgument(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitAttributeArgumentList.
        /// </summary>
        /// <param name="node">The node describing this VisitAttributeArgumentList.</param>
        public override void VisitAttributeArgumentList(AttributeArgumentListSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitAttributeArgumentList(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitAttributeList.
        /// </summary>
        /// <param name="node">The node describing this VisitAttributeList.</param>
        public override void VisitAttributeList(AttributeListSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitAttributeList(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitAttributeTargetSpecifier.
        /// </summary>
        /// <param name="node">The node describing this VisitAttributeTargetSpecifier.</param>
        public override void VisitAttributeTargetSpecifier(AttributeTargetSpecifierSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitAttributeTargetSpecifier(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitAwaitExpression.
        /// </summary>
        /// <param name="node">The node describing this VisitAwaitExpression.</param>
        public override void VisitAwaitExpression(AwaitExpressionSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitAwaitExpression(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitBadDirectiveTrivia.
        /// </summary>
        /// <param name="node">The node describing this VisitBadDirectiveTrivia.</param>
        public override void VisitBadDirectiveTrivia(BadDirectiveTriviaSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitBadDirectiveTrivia(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitBaseExpression.
        /// </summary>
        /// <param name="node">The node describing this VisitBaseExpression.</param>
        public override void VisitBaseExpression(BaseExpressionSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitBaseExpression(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitBaseList.
        /// </summary>
        /// <param name="node">The node describing this VisitBaseList.</param>
        public override void VisitBaseList(BaseListSyntax node)
        {
            var element = CreateElement(node);
            //var symbol = this.compilation.GetSymbolsWithName(n => n == node.Identifier.Text).First();
            //INamedTypeSymbol classSymbol = symbol as INamedTypeSymbol;

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitBaseList(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitBinaryExpression.
        /// </summary>
        /// <param name="node">The node describing this VisitBinaryExpression.</param>
        public override void VisitBinaryExpression(BinaryExpressionSyntax node)
        {
            var element = CreateElement(node);

            var expressionType = this.semanticModel.GetTypeInfo(node);
            element.Add(new XAttribute("Type", TypeName(expressionType.Type)));

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitBinaryExpression(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitBlock.
        /// </summary>
        /// <param name="node">The node describing this VisitBlock.</param>
        public override void VisitBlock(BlockSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitBlock(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitBracketedArgumentList.
        /// </summary>
        /// <param name="node">The node describing this VisitBracketedArgumentList.</param>
        public override void VisitBracketedArgumentList(BracketedArgumentListSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitBracketedArgumentList(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitBracketedParameterList.
        /// </summary>
        /// <param name="node">The node describing this VisitBracketedParameterList.</param>
        public override void VisitBracketedParameterList(BracketedParameterListSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitBracketedParameterList(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitBreakStatement.
        /// </summary>
        /// <param name="node">The node describing this VisitBreakStatement.</param>
        public override void VisitBreakStatement(BreakStatementSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitBreakStatement(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitCasePatternSwitchLabel.
        /// </summary>
        /// <param name="node">The node describing this VisitCasePatternSwitchLabel.</param>
        public override void VisitCasePatternSwitchLabel(CasePatternSwitchLabelSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitCasePatternSwitchLabel(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitCaseSwitchLabel.
        /// </summary>
        /// <param name="node">The node describing this VisitCaseSwitchLabel.</param>
        public override void VisitCaseSwitchLabel(CaseSwitchLabelSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitCaseSwitchLabel(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitCastExpression.
        /// </summary>
        /// <param name="node">The node describing this VisitCastExpression.</param>
        public override void VisitCastExpression(CastExpressionSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitCastExpression(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitCatchClause.
        /// </summary>
        /// <param name="node">The node describing this VisitCatchClause.</param>
        public override void VisitCatchClause(CatchClauseSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitCatchClause(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitCatchDeclaration.
        /// </summary>
        /// <param name="node">The node describing this VisitCatchDeclaration.</param>
        public override void VisitCatchDeclaration(CatchDeclarationSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitCatchDeclaration(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitCatchFilterClause.
        /// </summary>
        /// <param name="node">The node describing this VisitCatchFilterClause.</param>
        public override void VisitCatchFilterClause(CatchFilterClauseSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitCatchFilterClause(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitCheckedExpression.
        /// </summary>
        /// <param name="node">The node describing this VisitCheckedExpression.</param>
        public override void VisitCheckedExpression(CheckedExpressionSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitCheckedExpression(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitCheckedStatement.
        /// </summary>
        /// <param name="node">The node describing this VisitCheckedStatement.</param>
        public override void VisitCheckedStatement(CheckedStatementSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitCheckedStatement(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitClassDeclaration.
        /// </summary>
        /// <param name="node">The node describing this VisitClassDeclaration.</param>
        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            var element = CreateElement(node);
            element.Add(new XAttribute("Name", node.Identifier.Text));

            var triviaString = node.GetLeadingTrivia().ToString();
            element.Add(new XAttribute("Comment", triviaString.Contains("//") || triviaString.Contains("/*") ? triviaString : ""));
            
            var symbol = this.compilation.GetSymbolsWithName(n => n == node.Identifier.Text).First();
            INamedTypeSymbol classSymbol = symbol as INamedTypeSymbol;

            element.Add(new XAttribute("Artifact", classSymbol.ToDisplayString(this.symbolDisplayFormat)));
            element.Add(new XAttribute("FullName", classSymbol.ToDisplayString(this.symbolDisplayFormat)));
            element.Add(new XAttribute("IsAbstract", classSymbol.IsAbstract));
            element.Add(new XAttribute("IsSealed", classSymbol.IsSealed));

            element.Add(new XAttribute("DeclaredAccessibility", classSymbol.DeclaredAccessibility.ToString()));

            element.Add(new XAttribute("BaseType", classSymbol.BaseType.ToDisplayString()));
            element.Add(new XAttribute("IsGenericType", classSymbol.IsGenericType));

            this.classBaseList.Clear();
            this.classBaseList.Add(classSymbol.BaseType.Name, classSymbol.BaseType.ToDisplayString());
            foreach (INamedTypeSymbol baseInterface in classSymbol.Interfaces)
            {
                var iname = baseInterface.ToDisplayString();
                this.classBaseList.Add(baseInterface.Name, baseInterface.ToDisplayString());
            }

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitClassDeclaration(node);
            this.stack.Pop();
        }


        /// <summary>
        /// Visits the given VisitClassOrStructConstraint.
        /// </summary>
        /// <param name="node">The node describing this VisitClassOrStructConstraint.</param>
        public override void VisitClassOrStructConstraint(ClassOrStructConstraintSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitClassOrStructConstraint(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitCompilationUnit.
        /// </summary>
        /// <param name="node">The node describing this VisitCompilationUnit.</param>
        public override void VisitCompilationUnit(CompilationUnitSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            if (this.IncludeSource)
            {
                element.Add(new XAttribute("Source", node.ToString()));
            }

            element.Add(new XAttribute("FilePath", node.SyntaxTree.FilePath));

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitCompilationUnit(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitConditionalAccessExpression.
        /// </summary>
        /// <param name="node">The node describing this VisitConditionalAccessExpression.</param>
        public override void VisitConditionalAccessExpression(ConditionalAccessExpressionSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitConditionalAccessExpression(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitConditionalExpression.
        /// </summary>
        /// <param name="node">The node describing this VisitConditionalExpression.</param>
        public override void VisitConditionalExpression(ConditionalExpressionSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitConditionalExpression(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitConstantPattern.
        /// </summary>
        /// <param name="node">The node describing this VisitConstantPattern.</param>
        public override void VisitConstantPattern(ConstantPatternSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitConstantPattern(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitConstructorConstraint.
        /// </summary>
        /// <param name="node">The node describing this VisitConstructorConstraint.</param>
        public override void VisitConstructorConstraint(ConstructorConstraintSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitConstructorConstraint(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitConstructorDeclaration.
        /// </summary>
        /// <param name="node">The node describing this VisitConstructorDeclaration.</param>
        public override void VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitConstructorDeclaration(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitConstructorInitializer.
        /// </summary>
        /// <param name="node">The node describing this VisitConstructorInitializer.</param>
        public override void VisitConstructorInitializer(ConstructorInitializerSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitConstructorInitializer(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitContinueStatement.
        /// </summary>
        /// <param name="node">The node describing this VisitContinueStatement.</param>
        public override void VisitContinueStatement(ContinueStatementSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitContinueStatement(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitConversionOperatorDeclaration.
        /// </summary>
        /// <param name="node">The node describing this VisitConversionOperatorDeclaration.</param>
        public override void VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitConversionOperatorDeclaration(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitConversionOperatorMemberCref.
        /// </summary>
        /// <param name="node">The node describing this VisitConversionOperatorMemberCref.</param>
        public override void VisitConversionOperatorMemberCref(ConversionOperatorMemberCrefSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitConversionOperatorMemberCref(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitCrefBracketedParameterList.
        /// </summary>
        /// <param name="node">The node describing this VisitCrefBracketedParameterList.</param>
        public override void VisitCrefBracketedParameterList(CrefBracketedParameterListSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitCrefBracketedParameterList(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitCrefParameter.
        /// </summary>
        /// <param name="node">The node describing this VisitCrefParameter.</param>
        public override void VisitCrefParameter(CrefParameterSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitCrefParameter(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitCrefParameterList.
        /// </summary>
        /// <param name="node">The node describing this VisitCrefParameterList.</param>
        public override void VisitCrefParameterList(CrefParameterListSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitCrefParameterList(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitDeclarationExpression.
        /// </summary>
        /// <param name="node">The node describing this VisitDeclarationExpression.</param>
        public override void VisitDeclarationExpression(DeclarationExpressionSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitDeclarationExpression(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitDeclarationPattern.
        /// </summary>
        /// <param name="node">The node describing this VisitDeclarationPattern.</param>
        public override void VisitDeclarationPattern(DeclarationPatternSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitDeclarationPattern(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitDefaultExpression.
        /// </summary>
        /// <param name="node">The node describing this VisitDefaultExpression.</param>
        public override void VisitDefaultExpression(DefaultExpressionSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitDefaultExpression(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitDefaultSwitchLabel.
        /// </summary>
        /// <param name="node">The node describing this VisitDefaultSwitchLabel.</param>
        public override void VisitDefaultSwitchLabel(DefaultSwitchLabelSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitDefaultSwitchLabel(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitDefineDirectiveTrivia.
        /// </summary>
        /// <param name="node">The node describing this VisitDefineDirectiveTrivia.</param>
        public override void VisitDefineDirectiveTrivia(DefineDirectiveTriviaSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitDefineDirectiveTrivia(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitDelegateDeclaration.
        /// </summary>
        /// <param name="node">The node describing this VisitDelegateDeclaration.</param>
        public override void VisitDelegateDeclaration(DelegateDeclarationSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitDelegateDeclaration(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitDestructorDeclaration.
        /// </summary>
        /// <param name="node">The node describing this VisitDestructorDeclaration.</param>
        public override void VisitDestructorDeclaration(DestructorDeclarationSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitDestructorDeclaration(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitDiscardDesignation.
        /// </summary>
        /// <param name="node">The node describing this VisitDiscardDesignation.</param>
        public override void VisitDiscardDesignation(DiscardDesignationSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitDiscardDesignation(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitDocumentationCommentTrivia.
        /// </summary>
        /// <param name="node">The node describing this VisitDocumentationCommentTrivia.</param>
        public override void VisitDocumentationCommentTrivia(DocumentationCommentTriviaSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitDocumentationCommentTrivia(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitDoStatement.
        /// </summary>
        /// <param name="node">The node describing this VisitDoStatement.</param>
        public override void VisitDoStatement(DoStatementSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitDoStatement(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitElementAccessExpression.
        /// </summary>
        /// <param name="node">The node describing this VisitElementAccessExpression.</param>
        public override void VisitElementAccessExpression(ElementAccessExpressionSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitElementAccessExpression(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitElementBindingExpression.
        /// </summary>
        /// <param name="node">The node describing this VisitElementBindingExpression.</param>
        public override void VisitElementBindingExpression(ElementBindingExpressionSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitElementBindingExpression(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitElifDirectiveTrivia.
        /// </summary>
        /// <param name="node">The node describing this VisitElifDirectiveTrivia.</param>
        public override void VisitElifDirectiveTrivia(ElifDirectiveTriviaSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitElifDirectiveTrivia(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitElseClause.
        /// </summary>
        /// <param name="node">The node describing this VisitElseClause.</param>
        public override void VisitElseClause(ElseClauseSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitElseClause(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitElseDirectiveTrivia.
        /// </summary>
        /// <param name="node">The node describing this VisitElseDirectiveTrivia.</param>
        public override void VisitElseDirectiveTrivia(ElseDirectiveTriviaSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitElseDirectiveTrivia(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitEmptyStatement.
        /// </summary>
        /// <param name="node">The node describing this VisitEmptyStatement.</param>
        public override void VisitEmptyStatement(EmptyStatementSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitEmptyStatement(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitEndIfDirectiveTrivia.
        /// </summary>
        /// <param name="node">The node describing this VisitEndIfDirectiveTrivia.</param>
        public override void VisitEndIfDirectiveTrivia(EndIfDirectiveTriviaSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitEndIfDirectiveTrivia(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitEndRegionDirectiveTrivia.
        /// </summary>
        /// <param name="node">The node describing this VisitEndRegionDirectiveTrivia.</param>
        public override void VisitEndRegionDirectiveTrivia(EndRegionDirectiveTriviaSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitEndRegionDirectiveTrivia(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitEnumDeclaration.
        /// </summary>
        /// <param name="node">The node describing this VisitEnumDeclaration.</param>
        public override void VisitEnumDeclaration(EnumDeclarationSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitEnumDeclaration(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitEnumMemberDeclaration.
        /// </summary>
        /// <param name="node">The node describing this VisitEnumMemberDeclaration.</param>
        public override void VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitEnumMemberDeclaration(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitEqualsValueClause.
        /// </summary>
        /// <param name="node">The node describing this VisitEqualsValueClause.</param>
        public override void VisitEqualsValueClause(EqualsValueClauseSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitEqualsValueClause(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitErrorDirectiveTrivia.
        /// </summary>
        /// <param name="node">The node describing this VisitErrorDirectiveTrivia.</param>
        public override void VisitErrorDirectiveTrivia(ErrorDirectiveTriviaSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitErrorDirectiveTrivia(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitEventDeclaration.
        /// </summary>
        /// <param name="node">The node describing this VisitEventDeclaration.</param>
        public override void VisitEventDeclaration(EventDeclarationSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitEventDeclaration(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitEventFieldDeclaration.
        /// </summary>
        /// <param name="node">The node describing this VisitEventFieldDeclaration.</param>
        public override void VisitEventFieldDeclaration(EventFieldDeclarationSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitEventFieldDeclaration(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitExplicitInterfaceSpecifier.
        /// </summary>
        /// <param name="node">The node describing this VisitExplicitInterfaceSpecifier.</param>
        public override void VisitExplicitInterfaceSpecifier(ExplicitInterfaceSpecifierSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitExplicitInterfaceSpecifier(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitExpressionStatement.
        /// </summary>
        /// <param name="node">The node describing this VisitExpressionStatement.</param>
        public override void VisitExpressionStatement(ExpressionStatementSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitExpressionStatement(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitExternAliasDirective.
        /// </summary>
        /// <param name="node">The node describing this VisitExternAliasDirective.</param>
        public override void VisitExternAliasDirective(ExternAliasDirectiveSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitExternAliasDirective(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitFieldDeclaration.
        /// </summary>
        /// <param name="node">The node describing this VisitFieldDeclaration.</param>
        public override void VisitFieldDeclaration(FieldDeclarationSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitFieldDeclaration(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitFinallyClause.
        /// </summary>
        /// <param name="node">The node describing this VisitFinallyClause.</param>
        public override void VisitFinallyClause(FinallyClauseSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitFinallyClause(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitFixedStatement.
        /// </summary>
        /// <param name="node">The node describing this VisitFixedStatement.</param>
        public override void VisitFixedStatement(FixedStatementSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitFixedStatement(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitForEachStatement.
        /// </summary>
        /// <param name="node">The node describing this VisitForEachStatement.</param>
        public override void VisitForEachStatement(ForEachStatementSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitForEachStatement(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitForEachVariableStatement.
        /// </summary>
        /// <param name="node">The node describing this VisitForEachVariableStatement.</param>
        public override void VisitForEachVariableStatement(ForEachVariableStatementSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitForEachVariableStatement(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitForStatement.
        /// </summary>
        /// <param name="node">The node describing this VisitForStatement.</param>
        public override void VisitForStatement(ForStatementSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitForStatement(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitFromClause.
        /// </summary>
        /// <param name="node">The node describing this VisitFromClause.</param>
        public override void VisitFromClause(FromClauseSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitFromClause(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitGenericName.
        /// </summary>
        /// <param name="node">The node describing this VisitGenericName.</param>
        public override void VisitGenericName(GenericNameSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitGenericName(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitGlobalStatement.
        /// </summary>
        /// <param name="node">The node describing this VisitGlobalStatement.</param>
        public override void VisitGlobalStatement(GlobalStatementSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitGlobalStatement(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitGotoStatement.
        /// </summary>
        /// <param name="node">The node describing this VisitGotoStatement.</param>
        public override void VisitGotoStatement(GotoStatementSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitGotoStatement(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitGroupClause.
        /// </summary>
        /// <param name="node">The node describing this VisitGroupClause.</param>
        public override void VisitGroupClause(GroupClauseSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitGroupClause(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitIdentifierName.
        /// </summary>
        /// <param name="node">The node describing this VisitIdentifierName.</param>
        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            var element = CreateElement(node);

            SymbolInfo symbol = this.semanticModel.GetSymbolInfo(node);

            element.Add(new XAttribute("Name", symbol.Symbol?.Name ?? string.Empty));

            if (symbol.Symbol is ILocalSymbol)
            {
                ILocalSymbol localSymbol = symbol.Symbol as ILocalSymbol;
                element.Add(new XAttribute("Type", TypeName(localSymbol.Type)));
            }

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitIdentifierName(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitIfDirectiveTrivia.
        /// </summary>
        /// <param name="node">The node describing this VisitIfDirectiveTrivia.</param>
        public override void VisitIfDirectiveTrivia(IfDirectiveTriviaSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitIfDirectiveTrivia(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitIfStatement.
        /// </summary>
        /// <param name="node">The node describing this VisitIfStatement.</param>
        public override void VisitIfStatement(IfStatementSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitIfStatement(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitImplicitArrayCreationExpression.
        /// </summary>
        /// <param name="node">The node describing this VisitImplicitArrayCreationExpression.</param>
        public override void VisitImplicitArrayCreationExpression(ImplicitArrayCreationExpressionSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitImplicitArrayCreationExpression(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitImplicitElementAccess.
        /// </summary>
        /// <param name="node">The node describing this VisitImplicitElementAccess.</param>
        public override void VisitImplicitElementAccess(ImplicitElementAccessSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitImplicitElementAccess(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitIncompleteMember.
        /// </summary>
        /// <param name="node">The node describing this VisitIncompleteMember.</param>
        public override void VisitIncompleteMember(IncompleteMemberSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitIncompleteMember(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitIndexerDeclaration.
        /// </summary>
        /// <param name="node">The node describing this VisitIndexerDeclaration.</param>
        public override void VisitIndexerDeclaration(IndexerDeclarationSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitIndexerDeclaration(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitIndexerMemberCref.
        /// </summary>
        /// <param name="node">The node describing this VisitIndexerMemberCref.</param>
        public override void VisitIndexerMemberCref(IndexerMemberCrefSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitIndexerMemberCref(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitInitializerExpression.
        /// </summary>
        /// <param name="node">The node describing this VisitInitializerExpression.</param>
        public override void VisitInitializerExpression(InitializerExpressionSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitInitializerExpression(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitInterfaceDeclaration.
        /// </summary>
        /// <param name="node">The node describing this VisitInterfaceDeclaration.</param>
        public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            var element = CreateElement(node);

            element.Add(new XAttribute("name", node.Identifier.Text));

            var triviastring = node.GetLeadingTrivia().ToString();
            element.Add(new XAttribute("Comment", triviastring.Contains("//") || triviastring.Contains("/*") ? triviastring : ""));

            var symbol = this.compilation.GetSymbolsWithName(n => n == node.Identifier.Text).First();
            INamedTypeSymbol interfaceSymbol = symbol as INamedTypeSymbol;

            element.Add(new XAttribute("Artifact", interfaceSymbol.ToDisplayString(this.symbolDisplayFormat)));
            element.Add(new XAttribute("FullName", interfaceSymbol.ToDisplayString(this.symbolDisplayFormat)));
            element.Add(new XAttribute("DeclaredAccessibility", interfaceSymbol.DeclaredAccessibility.ToString()));
            element.Add(new XAttribute("IsGenericType", interfaceSymbol.IsGenericType));

            this.classBaseList.Clear();
            foreach (INamedTypeSymbol baseInterface in interfaceSymbol.Interfaces)
            {
                var iname = baseInterface.ToDisplayString();
                this.classBaseList.Add(baseInterface.Name, baseInterface.ToDisplayString());
            }

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitInterfaceDeclaration(node);
            this.stack.Pop();

        }

        /// <summary>
        /// Visits the given VisitInterpolatedStringExpression.
        /// </summary>
        /// <param name="node">The node describing this VisitInterpolatedStringExpression.</param>
        public override void VisitInterpolatedStringExpression(InterpolatedStringExpressionSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitInterpolatedStringExpression(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitInterpolatedStringText.
        /// </summary>
        /// <param name="node">The node describing this VisitInterpolatedStringText.</param>
        public override void VisitInterpolatedStringText(InterpolatedStringTextSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitInterpolatedStringText(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitInterpolation.
        /// </summary>
        /// <param name="node">The node describing this VisitInterpolation.</param>
        public override void VisitInterpolation(InterpolationSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitInterpolation(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitInterpolationAlignmentClause.
        /// </summary>
        /// <param name="node">The node describing this VisitInterpolationAlignmentClause.</param>
        public override void VisitInterpolationAlignmentClause(InterpolationAlignmentClauseSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitInterpolationAlignmentClause(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitInterpolationFormatClause.
        /// </summary>
        /// <param name="node">The node describing this VisitInterpolationFormatClause.</param>
        public override void VisitInterpolationFormatClause(InterpolationFormatClauseSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitInterpolationFormatClause(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitInvocationExpression.
        /// </summary>
        /// <param name="node">The node describing this VisitInvocationExpression.</param>
        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitInvocationExpression(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitIsPatternExpression.
        /// </summary>
        /// <param name="node">The node describing this VisitIsPatternExpression.</param>
        public override void VisitIsPatternExpression(IsPatternExpressionSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitIsPatternExpression(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitJoinClause.
        /// </summary>
        /// <param name="node">The node describing this VisitJoinClause.</param>
        public override void VisitJoinClause(JoinClauseSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitJoinClause(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitJoinIntoClause.
        /// </summary>
        /// <param name="node">The node describing this VisitJoinIntoClause.</param>
        public override void VisitJoinIntoClause(JoinIntoClauseSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitJoinIntoClause(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitLabeledStatement.
        /// </summary>
        /// <param name="node">The node describing this VisitLabeledStatement.</param>
        public override void VisitLabeledStatement(LabeledStatementSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitLabeledStatement(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitLetClause.
        /// </summary>
        /// <param name="node">The node describing this VisitLetClause.</param>
        public override void VisitLetClause(LetClauseSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitLetClause(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitLineDirectiveTrivia.
        /// </summary>
        /// <param name="node">The node describing this VisitLineDirectiveTrivia.</param>
        public override void VisitLineDirectiveTrivia(LineDirectiveTriviaSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitLineDirectiveTrivia(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitLiteralExpression.
        /// </summary>
        /// <param name="node">The node describing this VisitLiteralExpression.</param>
        public override void VisitLiteralExpression(LiteralExpressionSyntax node)
        {
            var element = CreateElement(node);

            this.AddTypeAttribute(node, element);
            this.AddLocationAttributes(element, node);

            string literal = node.Token.Text;
            element.Add(new XAttribute("Value", literal));

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitLiteralExpression(node);
            this.stack.Pop();
        }



        /// <summary>
        /// Visits the given VisitLoadDirectiveTrivia.
        /// </summary>
        /// <param name="node">The node describing this VisitLoadDirectiveTrivia.</param>
        public override void VisitLoadDirectiveTrivia(LoadDirectiveTriviaSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitLoadDirectiveTrivia(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitLocalDeclarationStatement.
        /// </summary>
        /// <param name="node">The node describing this VisitLocalDeclarationStatement.</param>
        public override void VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitLocalDeclarationStatement(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitLocalFunctionStatement.
        /// </summary>
        /// <param name="node">The node describing this VisitLocalFunctionStatement.</param>
        public override void VisitLocalFunctionStatement(LocalFunctionStatementSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitLocalFunctionStatement(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitLockStatement.
        /// </summary>
        /// <param name="node">The node describing this VisitLockStatement.</param>
        public override void VisitLockStatement(LockStatementSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitLockStatement(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitMakeRefExpression.
        /// </summary>
        /// <param name="node">The node describing this VisitMakeRefExpression.</param>
        public override void VisitMakeRefExpression(MakeRefExpressionSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitMakeRefExpression(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitMemberAccessExpression.
        /// </summary>
        /// <param name="node">The node describing this VisitMemberAccessExpression.</param>
        public override void VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitMemberAccessExpression(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitMemberBindingExpression.
        /// </summary>
        /// <param name="node">The node describing this VisitMemberBindingExpression.</param>
        public override void VisitMemberBindingExpression(MemberBindingExpressionSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitMemberBindingExpression(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitMethodDeclaration.
        /// </summary>
        /// <param name="node">The node describing this VisitMethodDeclaration.</param>
        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            var element = CreateElement(node);
            element.Add(new XAttribute("Name", node.Identifier.Text));

            var symbol = this.compilation.GetSymbolsWithName(n => n == node.Identifier.Text).Last();

            IMethodSymbol methodSymbol = symbol as IMethodSymbol;

            element.Add(new XAttribute("IsAbstract", methodSymbol.IsAbstract));
            element.Add(new XAttribute("IsSealed", methodSymbol.IsSealed));
            element.Add(new XAttribute("IsAsync", methodSymbol.IsAsync));
            element.Add(new XAttribute("IsOverride", methodSymbol.IsOverride));
            element.Add(new XAttribute("IsVirtual", methodSymbol.IsVirtual));
            element.Add(new XAttribute("DeclaredAccessibility", methodSymbol.DeclaredAccessibility.ToString()));

            var triviaString = node.GetLeadingTrivia().ToString();
            element.Add(new XAttribute("Comment", triviaString.Contains("//") || triviaString.Contains("/*") ? triviaString : ""));

            if (methodSymbol.ReturnsVoid)
            {
                element.Add(new XAttribute("ReturnType", "void"));
            }
            else
            {
                element.Add(new XAttribute("ReturnType", TypeName(methodSymbol.ReturnType)));
            }

            if (methodSymbol.IsOverride)
            {
                element.Add(new XAttribute("OverridesMethodIn", methodSymbol.OriginalDefinition.ContainingType.ToString()));
            }

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitMethodDeclaration(node);
            this.stack.Pop();
        }


        /// <summary>
        /// Visits the given VisitNameColon.
        /// </summary>
        /// <param name="node">The node describing this VisitNameColon.</param>
        public override void VisitNameColon(NameColonSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitNameColon(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitNameEquals.
        /// </summary>
        /// <param name="node">The node describing this VisitNameEquals.</param>
        public override void VisitNameEquals(NameEqualsSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitNameEquals(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitNameMemberCref.
        /// </summary>
        /// <param name="node">The node describing this VisitNameMemberCref.</param>
        public override void VisitNameMemberCref(NameMemberCrefSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitNameMemberCref(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitNamespaceDeclaration.
        /// </summary>
        /// <param name="node">The node describing this VisitNamespaceDeclaration.</param>
        public override void VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
        {
            var element = CreateElement(node);

            INamespaceSymbol s = this.semanticModel.GetDeclaredSymbol(node);
            element.Add(new XAttribute("Name", s.ToDisplayString(this.symbolDisplayFormat)));

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitNamespaceDeclaration(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitNullableType.
        /// </summary>
        /// <param name="node">The node describing this VisitNullableType.</param>
        public override void VisitNullableType(NullableTypeSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitNullableType(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitObjectCreationExpression.
        /// </summary>
        /// <param name="node">The node describing this VisitObjectCreationExpression.</param>
        public override void VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitObjectCreationExpression(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitOmittedArraySizeExpression.
        /// </summary>
        /// <param name="node">The node describing this VisitOmittedArraySizeExpression.</param>
        public override void VisitOmittedArraySizeExpression(OmittedArraySizeExpressionSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitOmittedArraySizeExpression(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitOmittedTypeArgument.
        /// </summary>
        /// <param name="node">The node describing this VisitOmittedTypeArgument.</param>
        public override void VisitOmittedTypeArgument(OmittedTypeArgumentSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitOmittedTypeArgument(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitOperatorDeclaration.
        /// </summary>
        /// <param name="node">The node describing this VisitOperatorDeclaration.</param>
        public override void VisitOperatorDeclaration(OperatorDeclarationSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitOperatorDeclaration(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitOperatorMemberCref.
        /// </summary>
        /// <param name="node">The node describing this VisitOperatorMemberCref.</param>
        public override void VisitOperatorMemberCref(OperatorMemberCrefSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitOperatorMemberCref(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitOrderByClause.
        /// </summary>
        /// <param name="node">The node describing this VisitOrderByClause.</param>
        public override void VisitOrderByClause(OrderByClauseSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitOrderByClause(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitOrdering.
        /// </summary>
        /// <param name="node">The node describing this VisitOrdering.</param>
        public override void VisitOrdering(OrderingSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitOrdering(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitParameter.
        /// </summary>
        /// <param name="node">The node describing this VisitParameter.</param>
        public override void VisitParameter(ParameterSyntax node)
        {
            var element = CreateElement(node);

            IParameterSymbol symbol = semanticModel.GetDeclaredSymbol(node);
            element.Add(new XAttribute("Name", symbol.Name));
            element.Add(new XAttribute("Type", TypeName(symbol.Type)));
            element.Add(new XAttribute("IsParams", symbol.IsParams));
            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitParameter(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitParameterList.
        /// </summary>
        /// <param name="node">The node describing this VisitParameterList.</param>
        public override void VisitParameterList(ParameterListSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitParameterList(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitParenthesizedExpression.
        /// </summary>
        /// <param name="node">The node describing this VisitParenthesizedExpression.</param>
        public override void VisitParenthesizedExpression(ParenthesizedExpressionSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitParenthesizedExpression(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitParenthesizedLambdaExpression.
        /// </summary>
        /// <param name="node">The node describing this VisitParenthesizedLambdaExpression.</param>
        public override void VisitParenthesizedLambdaExpression(ParenthesizedLambdaExpressionSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitParenthesizedLambdaExpression(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitParenthesizedVariableDesignation.
        /// </summary>
        /// <param name="node">The node describing this VisitParenthesizedVariableDesignation.</param>
        public override void VisitParenthesizedVariableDesignation(ParenthesizedVariableDesignationSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitParenthesizedVariableDesignation(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitPointerType.
        /// </summary>
        /// <param name="node">The node describing this VisitPointerType.</param>
        public override void VisitPointerType(PointerTypeSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitPointerType(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitPostfixUnaryExpression.
        /// </summary>
        /// <param name="node">The node describing this VisitPostfixUnaryExpression.</param>
        public override void VisitPostfixUnaryExpression(PostfixUnaryExpressionSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitPostfixUnaryExpression(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitPragmaChecksumDirectiveTrivia.
        /// </summary>
        /// <param name="node">The node describing this VisitPragmaChecksumDirectiveTrivia.</param>
        public override void VisitPragmaChecksumDirectiveTrivia(PragmaChecksumDirectiveTriviaSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitPragmaChecksumDirectiveTrivia(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitPragmaWarningDirectiveTrivia.
        /// </summary>
        /// <param name="node">The node describing this VisitPragmaWarningDirectiveTrivia.</param>
        public override void VisitPragmaWarningDirectiveTrivia(PragmaWarningDirectiveTriviaSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitPragmaWarningDirectiveTrivia(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitPredefinedType.
        /// </summary>
        /// <param name="node">The node describing this VisitPredefinedType.</param>
        public override void VisitPredefinedType(PredefinedTypeSyntax node)
        {
            var element = CreateElement(node);

            this.AddTypeAttribute(node, element);
            this.AddLocationAttributes(element, node);

            var typeInfo = semanticModel.GetTypeInfo(node);
            var name = typeInfo.ConvertedType?.ToString() ?? string.Empty;
            var ns = typeInfo.ConvertedType?.ContainingNamespace;
            string fullName = ns != null ? ns + "." + name : name;
            element.Add(new XAttribute("FullyQualifiedType", fullName));

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitPredefinedType(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitPrefixUnaryExpression.
        /// </summary>
        /// <param name="node">The node describing this VisitPrefixUnaryExpression.</param>
        public override void VisitPrefixUnaryExpression(PrefixUnaryExpressionSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitPrefixUnaryExpression(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitPropertyDeclaration.
        /// </summary>
        /// <param name="node">The node describing this VisitPropertyDeclaration.</param>
        public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            var element = CreateElement(node);

            element.Add(new XAttribute("Name", node.Identifier.Text));

            var triviaString = node.GetLeadingTrivia().ToString();
            element.Add(new XAttribute("Comment", triviaString.Contains("//") || triviaString.Contains("/*") ? triviaString : ""));

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitPropertyDeclaration(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitQualifiedCref.
        /// </summary>
        /// <param name="node">The node describing this VisitQualifiedCref.</param>
        public override void VisitQualifiedCref(QualifiedCrefSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitQualifiedCref(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitQualifiedName.
        /// </summary>
        /// <param name="node">The node describing this VisitQualifiedName.</param>
        public override void VisitQualifiedName(QualifiedNameSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitQualifiedName(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitQueryBody.
        /// </summary>
        /// <param name="node">The node describing this VisitQueryBody.</param>
        public override void VisitQueryBody(QueryBodySyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitQueryBody(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitQueryContinuation.
        /// </summary>
        /// <param name="node">The node describing this VisitQueryContinuation.</param>
        public override void VisitQueryContinuation(QueryContinuationSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitQueryContinuation(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitQueryExpression.
        /// </summary>
        /// <param name="node">The node describing this VisitQueryExpression.</param>
        public override void VisitQueryExpression(QueryExpressionSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitQueryExpression(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitReferenceDirectiveTrivia.
        /// </summary>
        /// <param name="node">The node describing this VisitReferenceDirectiveTrivia.</param>
        public override void VisitReferenceDirectiveTrivia(ReferenceDirectiveTriviaSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitReferenceDirectiveTrivia(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitRefExpression.
        /// </summary>
        /// <param name="node">The node describing this VisitRefExpression.</param>
        public override void VisitRefExpression(RefExpressionSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitRefExpression(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitRefType.
        /// </summary>
        /// <param name="node">The node describing this VisitRefType.</param>
        public override void VisitRefType(RefTypeSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitRefType(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitRefTypeExpression.
        /// </summary>
        /// <param name="node">The node describing this VisitRefTypeExpression.</param>
        public override void VisitRefTypeExpression(RefTypeExpressionSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitRefTypeExpression(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitRefValueExpression.
        /// </summary>
        /// <param name="node">The node describing this VisitRefValueExpression.</param>
        public override void VisitRefValueExpression(RefValueExpressionSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitRefValueExpression(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitRegionDirectiveTrivia.
        /// </summary>
        /// <param name="node">The node describing this VisitRegionDirectiveTrivia.</param>
        public override void VisitRegionDirectiveTrivia(RegionDirectiveTriviaSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitRegionDirectiveTrivia(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitReturnStatement.
        /// </summary>
        /// <param name="node">The node describing this VisitReturnStatement.</param>
        public override void VisitReturnStatement(ReturnStatementSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitReturnStatement(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitSelectClause.
        /// </summary>
        /// <param name="node">The node describing this VisitSelectClause.</param>
        public override void VisitSelectClause(SelectClauseSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitSelectClause(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitShebangDirectiveTrivia.
        /// </summary>
        /// <param name="node">The node describing this VisitShebangDirectiveTrivia.</param>
        public override void VisitShebangDirectiveTrivia(ShebangDirectiveTriviaSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitShebangDirectiveTrivia(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitSimpleBaseType.
        /// </summary>
        /// <param name="node">The node describing this VisitSimpleBaseType.</param>
        public override void VisitSimpleBaseType(SimpleBaseTypeSyntax node)
        {
            var element = CreateElement(node);
            var fullName = node.Type.ToString().Contains(".") ? node.Type.ToString() :
                this.classBaseList.ContainsKey(node.Type.ToString()) ? this.classBaseList[node.Type.ToString()] : node.Type.ToString();
            element.Add(new XAttribute("FullyQualifiedBaseType", fullName));

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitSimpleBaseType(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitSimpleLambdaExpression.
        /// </summary>
        /// <param name="node">The node describing this VisitSimpleLambdaExpression.</param>
        public override void VisitSimpleLambdaExpression(SimpleLambdaExpressionSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitSimpleLambdaExpression(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitSingleVariableDesignation.
        /// </summary>
        /// <param name="node">The node describing this VisitSingleVariableDesignation.</param>
        public override void VisitSingleVariableDesignation(SingleVariableDesignationSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitSingleVariableDesignation(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitSizeOfExpression.
        /// </summary>
        /// <param name="node">The node describing this VisitSizeOfExpression.</param>
        public override void VisitSizeOfExpression(SizeOfExpressionSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitSizeOfExpression(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitSkippedTokensTrivia.
        /// </summary>
        /// <param name="node">The node describing this VisitSkippedTokensTrivia.</param>
        public override void VisitSkippedTokensTrivia(SkippedTokensTriviaSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitSkippedTokensTrivia(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitStackAllocArrayCreationExpression.
        /// </summary>
        /// <param name="node">The node describing this VisitStackAllocArrayCreationExpression.</param>
        public override void VisitStackAllocArrayCreationExpression(StackAllocArrayCreationExpressionSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitStackAllocArrayCreationExpression(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitStructDeclaration.
        /// </summary>
        /// <param name="node">The node describing this VisitStructDeclaration.</param>
        public override void VisitStructDeclaration(StructDeclarationSyntax node)
        {
            var element = CreateElement(node);

            element.Add(new XAttribute("Name", node.Identifier.Text));

            var triviaString = node.GetLeadingTrivia().ToString();
            element.Add(new XAttribute("Comment", triviaString.Contains("//") || triviaString.Contains("/*") ? triviaString : ""));

            var symbol = this.compilation.GetSymbolsWithName(n => n == node.Identifier.Text).First();
            INamedTypeSymbol classSymbol = symbol as INamedTypeSymbol;

            element.Add(new XAttribute("Artifact", classSymbol.ToDisplayString(this.symbolDisplayFormat)));
            element.Add(new XAttribute("FullName", classSymbol.ToDisplayString(this.symbolDisplayFormat)));
            element.Add(new XAttribute("DeclaredAccessibility", classSymbol.DeclaredAccessibility.ToString()));

            element.Add(new XAttribute("IsGenericType", classSymbol.IsGenericType));

            this.classBaseList.Clear();
            this.classBaseList.Add(classSymbol.BaseType.Name, classSymbol.BaseType.ToDisplayString());
            foreach (INamedTypeSymbol baseInterface in classSymbol.Interfaces)
            {
                var iname = baseInterface.ToDisplayString();
                this.classBaseList.Add(baseInterface.Name, baseInterface.ToDisplayString());
            }

            this.AddLocationAttributes(element, node);
            
            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitStructDeclaration(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitSwitchSection.
        /// </summary>
        /// <param name="node">The node describing this VisitSwitchSection.</param>
        public override void VisitSwitchSection(SwitchSectionSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitSwitchSection(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitSwitchStatement.
        /// </summary>
        /// <param name="node">The node describing this VisitSwitchStatement.</param>
        public override void VisitSwitchStatement(SwitchStatementSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitSwitchStatement(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitThisExpression.
        /// </summary>
        /// <param name="node">The node describing this VisitThisExpression.</param>
        public override void VisitThisExpression(ThisExpressionSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitThisExpression(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitThrowExpression.
        /// </summary>
        /// <param name="node">The node describing this VisitThrowExpression.</param>
        public override void VisitThrowExpression(ThrowExpressionSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitThrowExpression(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitThrowStatement.
        /// </summary>
        /// <param name="node">The node describing this VisitThrowStatement.</param>
        public override void VisitThrowStatement(ThrowStatementSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitThrowStatement(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitTryStatement.
        /// </summary>
        /// <param name="node">The node describing this VisitTryStatement.</param>
        public override void VisitTryStatement(TryStatementSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitTryStatement(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitTupleElement.
        /// </summary>
        /// <param name="node">The node describing this VisitTupleElement.</param>
        public override void VisitTupleElement(TupleElementSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitTupleElement(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitTupleExpression.
        /// </summary>
        /// <param name="node">The node describing this VisitTupleExpression.</param>
        public override void VisitTupleExpression(TupleExpressionSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitTupleExpression(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitTupleType.
        /// </summary>
        /// <param name="node">The node describing this VisitTupleType.</param>
        public override void VisitTupleType(TupleTypeSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitTupleType(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitTypeOfExpression.
        /// </summary>
        /// <param name="node">The node describing this VisitTypeOfExpression.</param>
        public override void VisitTypeOfExpression(TypeOfExpressionSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitTypeOfExpression(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitTypeParameter.
        /// </summary>
        /// <param name="node">The node describing this VisitTypeParameter.</param>
        public override void VisitTypeParameter(TypeParameterSyntax node)
        {
            var element = CreateElement(node);

            element.Add(new XAttribute("TypeName", node.Identifier.ValueText));

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitTypeParameter(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitTypeParameterConstraintClause.
        /// </summary>
        /// <param name="node">The node describing this VisitTypeParameterConstraintClause.</param>
        public override void VisitTypeParameterConstraintClause(TypeParameterConstraintClauseSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitTypeParameterConstraintClause(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitTypeParameterList.
        /// </summary>
        /// <param name="node">The node describing this VisitTypeParameterList.</param>
        public override void VisitTypeParameterList(TypeParameterListSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitTypeParameterList(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitUndefDirectiveTrivia.
        /// </summary>
        /// <param name="node">The node describing this VisitUndefDirectiveTrivia.</param>
        public override void VisitUndefDirectiveTrivia(UndefDirectiveTriviaSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitUndefDirectiveTrivia(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitUnsafeStatement.
        /// </summary>
        /// <param name="node">The node describing this VisitUnsafeStatement.</param>
        public override void VisitUnsafeStatement(UnsafeStatementSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitUnsafeStatement(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitUsingDirective.
        /// </summary>
        /// <param name="node">The node describing this VisitUsingDirective.</param>
        public override void VisitUsingDirective(UsingDirectiveSyntax node)
        {
            var element = CreateElement(node);

            element.Add(new XAttribute("Name", node.Name.ToString()));
            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitUsingDirective(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitUsingStatement.
        /// </summary>
        /// <param name="node">The node describing this VisitUsingStatement.</param>
        public override void VisitUsingStatement(UsingStatementSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitUsingStatement(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitVariableDeclaration.
        /// </summary>
        /// <param name="node">The node describing this VisitVariableDeclaration.</param>
        public override void VisitVariableDeclaration(VariableDeclarationSyntax node)
        {
            var element = CreateElement(node);

            var triviaString = node.GetLeadingTrivia().ToString();
            element.Add(new XAttribute("Comment", triviaString.Contains("//") || triviaString.Contains("/*") ? triviaString : ""));

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitVariableDeclaration(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitVariableDeclarator.
        /// </summary>
        /// <param name="node">The node describing this VisitVariableDeclarator.</param>
        public override void VisitVariableDeclarator(VariableDeclaratorSyntax node)
        {
            var element = CreateElement(node);

            ISymbol symbol = this.semanticModel.GetDeclaredSymbol(node);
            string name;
            if (symbol is ILocalSymbol)
            {
                ILocalSymbol s = symbol as ILocalSymbol;
                name = s.Name;
                element.Add(new XAttribute("Type", TypeName(s.Type)));
            }
            else if (symbol is IFieldSymbol)
            {
                IFieldSymbol s = symbol as IFieldSymbol;
                name = s.Name;
                element.Add(new XAttribute("Type", TypeName(s.Type)));
            }
            else // TODO Put in an assertion here so we can get the type correctly for other cases.
            {
                name = symbol.Name;
            }
            element.Add(new XAttribute("Name", name));

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitVariableDeclarator(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitWarningDirectiveTrivia.
        /// </summary>
        /// <param name="node">The node describing this VisitWarningDirectiveTrivia.</param>
        public override void VisitWarningDirectiveTrivia(WarningDirectiveTriviaSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitWarningDirectiveTrivia(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitWhenClause.
        /// </summary>
        /// <param name="node">The node describing this VisitWhenClause.</param>
        public override void VisitWhenClause(WhenClauseSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitWhenClause(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitWhereClause.
        /// </summary>
        /// <param name="node">The node describing this VisitWhereClause.</param>
        public override void VisitWhereClause(WhereClauseSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitWhereClause(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitWhileStatement.
        /// </summary>
        /// <param name="node">The node describing this VisitWhileStatement.</param>
        public override void VisitWhileStatement(WhileStatementSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitWhileStatement(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitXmlCDataSection.
        /// </summary>
        /// <param name="node">The node describing this VisitXmlCDataSection.</param>
        public override void VisitXmlCDataSection(XmlCDataSectionSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitXmlCDataSection(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitXmlComment.
        /// </summary>
        /// <param name="node">The node describing this VisitXmlComment.</param>
        public override void VisitXmlComment(XmlCommentSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitXmlComment(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitXmlCrefAttribute.
        /// </summary>
        /// <param name="node">The node describing this VisitXmlCrefAttribute.</param>
        public override void VisitXmlCrefAttribute(XmlCrefAttributeSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitXmlCrefAttribute(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitXmlElement.
        /// </summary>
        /// <param name="node">The node describing this VisitXmlElement.</param>
        public override void VisitXmlElement(XmlElementSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitXmlElement(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitXmlElementEndTag.
        /// </summary>
        /// <param name="node">The node describing this VisitXmlElementEndTag.</param>
        public override void VisitXmlElementEndTag(XmlElementEndTagSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitXmlElementEndTag(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitXmlElementStartTag.
        /// </summary>
        /// <param name="node">The node describing this VisitXmlElementStartTag.</param>
        public override void VisitXmlElementStartTag(XmlElementStartTagSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitXmlElementStartTag(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitXmlEmptyElement.
        /// </summary>
        /// <param name="node">The node describing this VisitXmlEmptyElement.</param>
        public override void VisitXmlEmptyElement(XmlEmptyElementSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitXmlEmptyElement(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitXmlName.
        /// </summary>
        /// <param name="node">The node describing this VisitXmlName.</param>
        public override void VisitXmlName(XmlNameSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitXmlName(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitXmlNameAttribute.
        /// </summary>
        /// <param name="node">The node describing this VisitXmlNameAttribute.</param>
        public override void VisitXmlNameAttribute(XmlNameAttributeSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitXmlNameAttribute(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitXmlPrefix.
        /// </summary>
        /// <param name="node">The node describing this VisitXmlPrefix.</param>
        public override void VisitXmlPrefix(XmlPrefixSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitXmlPrefix(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitXmlProcessingInstruction.
        /// </summary>
        /// <param name="node">The node describing this VisitXmlProcessingInstruction.</param>
        public override void VisitXmlProcessingInstruction(XmlProcessingInstructionSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitXmlProcessingInstruction(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitXmlText.
        /// </summary>
        /// <param name="node">The node describing this VisitXmlText.</param>
        public override void VisitXmlText(XmlTextSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitXmlText(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitXmlTextAttribute.
        /// </summary>
        /// <param name="node">The node describing this VisitXmlTextAttribute.</param>
        public override void VisitXmlTextAttribute(XmlTextAttributeSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitXmlTextAttribute(node);
            this.stack.Pop();
        }

        /// <summary>
        /// Visits the given VisitYieldStatement.
        /// </summary>
        /// <param name="node">The node describing this VisitYieldStatement.</param>
        public override void VisitYieldStatement(YieldStatementSyntax node)
        {
            var element = CreateElement(node);

            this.AddLocationAttributes(element, node);

            this.stack.Peek().Add(element);
            this.stack.Push(element);
            base.VisitYieldStatement(node);
            this.stack.Pop();
        }
    }
}
