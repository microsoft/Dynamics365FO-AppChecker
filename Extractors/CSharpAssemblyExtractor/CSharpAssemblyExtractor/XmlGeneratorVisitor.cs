using System.Xml.Linq;

using ICSharpCode.Decompiler.CSharp.Syntax;
using ICSharpCode.Decompiler.CSharp.Syntax.PatternMatching;
using System;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.Decompiler.Util;

using Syntax = ICSharpCode.Decompiler.CSharp.Syntax;
using System.Xml.XPath;

namespace CSharpAssemblyExtractor
{
    internal class XmlGeneratorVisitor : IAstVisitor<XElement>
    {
        /// <summary>
        /// This is where the result is stored.
        /// </summary>
        private XDocument document = new XDocument();

        public XDocument Document { get { return document; } }

        private string source;
        private string assemblyFile;

        /// <summary>
        /// Creates an instance of the XmlGeneratorVisitor
        /// </summary>
        /// <param name="source">The source code of the artifact being visited.</param>
        public XmlGeneratorVisitor(string assemblyFile, string source)
        {
            this.source = source;
            this.assemblyFile = assemblyFile;
        }

        private IEnumerable<XAttribute> Positions(AstNode node)
        {
            return new[] { 
                new XAttribute("StartLine", node.StartLocation.Line), 
                new XAttribute("StartCol", node.StartLocation.Column),
                new XAttribute("EndLine", node.EndLocation.Line),
                new XAttribute("EndCol", node.EndLocation.Column)
            };
        }

        protected virtual void WriteAttributes(XElement parent, IEnumerable<AttributeSection> attributes)
        {
            foreach (AttributeSection attr in attributes)
            {
                var attributeELement = attr.AcceptVisitor<XElement>(this);
                parent.Add(attributeELement);
            }
        }

        protected virtual void WriteModifierTokens(XElement parent, IEnumerable<CSharpModifierToken> modifierTokens)
        {
            foreach (var modifier in modifierTokens.Select(t => t.Modifier))
            {
                parent.Add(new XAttribute(modifier.ToString(), "true"));
            }
        }

        protected virtual void WriteModifiers(XElement parent, Syntax.Modifiers modifier)
        {
            if ((modifier & Modifiers.Private) != 0)
            {
                parent.Add(new XAttribute("Private", true));
            }
            else if ((modifier & Modifiers.Internal) != 0)
            {
                parent.Add(new XAttribute("Internal", true));
            }
            else if ((modifier & Modifiers.Protected) != 0)
            {
                parent.Add(new XAttribute("Protected", true));
            }
            else if ((modifier & Modifiers.Public) != 0)
            {
                parent.Add(new XAttribute("Public", true));
            }

            if ((modifier & Modifiers.Abstract) != 0)
            {
                parent.Add(new XAttribute("Abstract", true));
            }
            if ((modifier & Modifiers.Virtual) != 0)
            {
                parent.Add(new XAttribute("Virtual", true));
            }
            if ((modifier & Modifiers.Sealed) != 0)
            {
                parent.Add(new XAttribute("Sealed", true));
            }
            if ((modifier & Modifiers.Static) != 0)
            {
                parent.Add(new XAttribute("Static", true));
            }
            if ((modifier & Modifiers.Override) != 0)
            {
                parent.Add(new XAttribute("Override", true));
            }
            if ((modifier & Modifiers.Readonly) != 0)
            {
                parent.Add(new XAttribute("Readonly", true));
            }
            if ((modifier & Modifiers.Const) != 0)
            {
                parent.Add(new XAttribute("Const", true));
            }
            if ((modifier & Modifiers.New) != 0)
            {
                parent.Add(new XAttribute("New", true));
            }
            if ((modifier | Modifiers.Partial) != 0)
            {
                parent.Add(new XAttribute("Partial", true));
            }
            if ((modifier & Modifiers.Extern) != 0)
            {
                parent.Add(new XAttribute("Extern", true));
            }
            if ((modifier & Modifiers.Volatile) != 0)
            {
                parent.Add(new XAttribute("Volatile", true));
            }
            if ((modifier & Modifiers.Unsafe) != 0)
            {
                parent.Add(new XAttribute("Unsafe", true));
            }
            if ((modifier & Modifiers.Async) != 0)
            {
                parent.Add(new XAttribute("Async", true));
            }
            if ((modifier & Modifiers.Ref) != 0)
            {
                parent.Add(new XAttribute("Ref", true));
            }
        }

        public virtual void WriteTypeParameters(XElement parent, IEnumerable<TypeParameterDeclaration> typeParameters)
        {
            if (typeParameters.Any())
            {
                var typeParametersElement = new XElement("TypeParameters");
                foreach (var tp in typeParameters)
                {
                    var tpe = tp.AcceptVisitor<XElement>(this);
                    typeParametersElement.Add(tpe);
                }
                parent.Add(typeParametersElement);
            }
        }

        public XElement VisitAnonymousMethodExpression(AnonymousMethodExpression anonymousMethodExpression)
        {
            var element = new XElement("AnonymousMethodExpression", 
                new XAttribute("IsAsync", anonymousMethodExpression.IsAsync),
                new XAttribute("HasParameterList", anonymousMethodExpression.HasParameterList),
                Positions(anonymousMethodExpression));

            foreach (var parameter in anonymousMethodExpression.Parameters)
            {
                var parameterElement = parameter.AcceptVisitor(this);
                element.Add(parameterElement);
            }

            var bodyElement = anonymousMethodExpression.Body.AcceptVisitor(this);
            element.Add(bodyElement);

            return element;
        }

        public XElement VisitAnonymousTypeCreateExpression(AnonymousTypeCreateExpression anonymousTypeCreateExpression)
        {
            var element = new XElement("AnonymousTypeCreateExpression", Positions(anonymousTypeCreateExpression));

            foreach (var initializer in anonymousTypeCreateExpression.Initializers)
            {
                var initializerElement = initializer.AcceptVisitor(this);
                element.Add(initializerElement);
            }

            return element;
        }

        public XElement VisitArrayCreateExpression(ArrayCreateExpression arrayCreateExpression)
        {
            var element = new XElement("ArrayCreateExpression", Positions(arrayCreateExpression));

            var typeElement = arrayCreateExpression.Type.AcceptVisitor(this);
            element.Add(typeElement);

            foreach (var argument in arrayCreateExpression.Arguments)
            {
                var argumentElement = argument.AcceptVisitor(this);
                element.Add(argumentElement);
            }

            foreach (var additionalArraySpecifier in arrayCreateExpression.AdditionalArraySpecifiers)
            {
                var additionalArraySpecifierElement = additionalArraySpecifier.AcceptVisitor(this);
                element.Add(additionalArraySpecifierElement);
            }

            if (arrayCreateExpression.Initializer != ArrayInitializerExpression.Null)
            {
                var initializerElement = arrayCreateExpression.Initializer.AcceptVisitor(this);
                element.Add(initializerElement);
            }

            return element;
        }

        public XElement VisitArrayInitializerExpression(ArrayInitializerExpression arrayInitializerExpression)
        {
            var element = new XElement("ArrayInitializerExpression", Positions(arrayInitializerExpression));

            foreach (var e in arrayInitializerExpression.Elements)
            {
                var elementElement = e.AcceptVisitor(this);
                element.Add(elementElement);
            }
            return element;
        }

        public XElement VisitAsExpression(AsExpression asExpression)
        {
            var element = new XElement("AsExpression", Positions(asExpression));

            var expressionElement = asExpression.Expression.AcceptVisitor(this);
            element.Add(expressionElement);

            var typeElement = asExpression.Type.AcceptVisitor(this);
            element.Add(typeElement);

            return element;
        }

        public XElement VisitAssignmentExpression(AssignmentExpression assignmentExpression)
        {
            var element = new XElement("AssignmentExpression", 
                new XAttribute("Operator", assignmentExpression.Operator),
                Positions(assignmentExpression));

            var leftElement = assignmentExpression.Left.AcceptVisitor(this);
            element.Add(leftElement);

            var rightElement = assignmentExpression.Right.AcceptVisitor(this);
            element.Add(rightElement);

            return element;
        }

        public XElement VisitBaseReferenceExpression(BaseReferenceExpression baseReferenceExpression)
        {
            var element = new XElement("BaseReferenceExpression", Positions(baseReferenceExpression));
            return element;
        }

        public XElement VisitBinaryOperatorExpression(BinaryOperatorExpression binaryOperatorExpression)
        {
            var element = new XElement("BinaryOperatorExpression", 
                new XAttribute("Operator", binaryOperatorExpression.Operator),
                Positions(binaryOperatorExpression));

            var leftElement = binaryOperatorExpression.Left.AcceptVisitor(this);
            element.Add(leftElement);

            var rightElement = binaryOperatorExpression.Right.AcceptVisitor(this);
            element.Add(rightElement);

            return element;
        }

        public XElement VisitCastExpression(CastExpression castExpression)
        {
            var element = new XElement("CastExpression", Positions(castExpression));

            var typeElement = castExpression.Type.AcceptVisitor(this);
            element.Add(typeElement);

            var expressionElement = castExpression.Expression.AcceptVisitor(this);
            element.Add(expressionElement);

            return element;
        }

        public XElement VisitCheckedExpression(CheckedExpression checkedExpression)
        {
            var element = new XElement("", Positions(checkedExpression));

            var expressionElement = checkedExpression.Expression.AcceptVisitor(this);
            element.Add(expressionElement);

            return element;
        }

        public XElement VisitConditionalExpression(Syntax.ConditionalExpression conditionalExpression)
        {
            var element = new XElement("ConditionalExpression", Positions(conditionalExpression));

            var conditionElement = conditionalExpression.Condition.AcceptVisitor(this);
            element.Add(conditionElement);

            var trueExpressionElement = conditionalExpression.TrueExpression.AcceptVisitor(this);
            element.Add(trueExpressionElement);

            var falseExpressionElement = conditionalExpression.FalseExpression.AcceptVisitor(this);
            element.Add(falseExpressionElement);

            return element;
        }

        public XElement VisitDeclarationExpression(DeclarationExpression declarationExpression)
        {
            var element = new XElement("DeclarationExpression", Positions(declarationExpression));

            var typeElement = declarationExpression.Type.AcceptVisitor(this);
            element.Add(typeElement);

            var designationElement = declarationExpression.Designation.AcceptVisitor(this);
            element.Add(designationElement);

            return element;
        }

        public XElement VisitDefaultValueExpression(DefaultValueExpression defaultValueExpression)
        {
            var element = new XElement("DefaultValueExpression", Positions(defaultValueExpression));
            
            var typeElement = defaultValueExpression.Type.AcceptVisitor(this);
            element.Add(typeElement);

            return element;
        }

        public XElement VisitDirectionExpression(DirectionExpression directionExpression)
        {
            var element = new XElement("DirectionExpression", 
                new XAttribute("FieldDirection", directionExpression.FieldDirection), 
                Positions(directionExpression));

            var expressionElement = directionExpression.Expression.AcceptVisitor(this);
            element.Add(expressionElement);

            return element;
        }

        public XElement VisitIdentifierExpression(IdentifierExpression identifierExpression)
        {
            var element = new XElement("IdentifierExpression",
                new XAttribute("Identifier", identifierExpression.Identifier),
                Positions(identifierExpression));

            foreach (var typeArgument in identifierExpression.TypeArguments)
            {
                var typeArgumentElement = typeArgument.AcceptVisitor(this);
                element.Add(typeArgumentElement);
            }
            return element;
        }

        public XElement VisitIndexerExpression(IndexerExpression indexerExpression)
        {
            var element = new XElement("IndexerExpression", Positions(indexerExpression));

            var targetElement = indexerExpression.Target.AcceptVisitor(this);
            element.Add(targetElement);

            foreach (var argument in indexerExpression.Arguments)
            {
                var argumentElement = argument.AcceptVisitor(this);
                element.Add(argumentElement);
            }
            return element;
        }

        public XElement VisitInterpolatedStringExpression(InterpolatedStringExpression interpolatedStringExpression)
        {
            var element = new XElement("InterpolatedStringExpression", Positions(interpolatedStringExpression));
            
            foreach (var content in interpolatedStringExpression.Content)
            {
                var contentElement = content.AcceptVisitor(this);
                element.Add(contentElement);
            }

            return element;
        }

        public XElement VisitInvocationExpression(Syntax.InvocationExpression invocationExpression)
        {
            var element = new XElement("InvocationExpression",
                Positions(invocationExpression));

            var targetElement = invocationExpression.Target.AcceptVisitor(this);
            element.Add(targetElement);

            foreach (var argument in invocationExpression.Arguments)
            {
                var argumentElement = argument.AcceptVisitor(this);
                element.Add(argumentElement);
            }
            return element;
        }

        public XElement VisitIsExpression(IsExpression isExpression)
        {
            var element = new XElement("IsExpression", Positions(isExpression));

            var expressionElement = isExpression.Expression.AcceptVisitor(this);
            element.Add(expressionElement);

            var typeElement = isExpression.Type.AcceptVisitor(this);
            element.Add(typeElement);

            return element;
        }

        public XElement VisitLambdaExpression(Syntax.LambdaExpression lambdaExpression)
        {
            var element = new XElement("LambdaExpression", 
                new XAttribute("IsAsync", lambdaExpression.IsAsync),
                Positions(lambdaExpression));

            foreach (var paramter in lambdaExpression.Parameters)
            {
                var parameterElement = paramter.AcceptVisitor(this);
                element.Add(parameterElement);
            }

            var bodyElement = lambdaExpression.Body.AcceptVisitor(this);
            element.Add(bodyElement);

            return element;
        }

        public XElement VisitMemberReferenceExpression(MemberReferenceExpression memberReferenceExpression)
        {
            var element = new XElement("MemberReferenceExpression",
                new XAttribute("Name", memberReferenceExpression.MemberName),
               Positions(memberReferenceExpression));

            var targetElement = memberReferenceExpression.Target.AcceptVisitor(this);
            element.Add(targetElement);

            return element;
        }

        public XElement VisitNamedArgumentExpression(NamedArgumentExpression namedArgumentExpression)
        {
            var element = new XElement("NamedArgumentExpression", 
                new XAttribute("Name", namedArgumentExpression.Name),
                Positions(namedArgumentExpression));

            var expressionElement = namedArgumentExpression.Expression.AcceptVisitor(this);
            element.Add(expressionElement);

            return element;
        }

        public XElement VisitNamedExpression(NamedExpression namedExpression)
        {
            var element = new XElement("NamedExpression", 
                new XAttribute("Name", namedExpression.Name),
                Positions(namedExpression));
            
            var expressionElement = namedExpression.Expression.AcceptVisitor(this);
            element.Add(expressionElement);

            return element;
        }

        public XElement VisitNullReferenceExpression(NullReferenceExpression nullReferenceExpression)
        {
            var element = new XElement("NullReferenceExpression", Positions(nullReferenceExpression));
            return element;
        }

        public XElement VisitObjectCreateExpression(ObjectCreateExpression objectCreateExpression)
        {
            var element = new XElement("ObjectCreateExpression", Positions(objectCreateExpression));

            var typeElement = objectCreateExpression.Type.AcceptVisitor(this);
            element.Add(typeElement);

            foreach(var argument in objectCreateExpression.Arguments)
            {
                var argumentElement = argument.AcceptVisitor(this);
                element.Add(argumentElement);
            }

            if (objectCreateExpression.Initializer != ArrayInitializerExpression.Null)
            {
                var initializerElement = objectCreateExpression.Initializer.AcceptVisitor(this);
                element.Add(initializerElement);
            }

            return element;
        }

        public XElement VisitOutVarDeclarationExpression(OutVarDeclarationExpression outVarDeclarationExpression)
        {
            var element = new XElement("OutVarDeclarationExpression", Positions(outVarDeclarationExpression));

            var typeElement = outVarDeclarationExpression.Type.AcceptVisitor(this);
            element.Add(typeElement);

            var variableElement = outVarDeclarationExpression.Variable.AcceptVisitor(this);
            element.Add(variableElement);

            return element;
        }

        public XElement VisitParenthesizedExpression(ParenthesizedExpression parenthesizedExpression)
        {
            var element = new XElement("ParenthesizedExpression", Positions(parenthesizedExpression));

            var expressionElement = parenthesizedExpression.Expression.AcceptVisitor(this);
            element.Add(expressionElement);

            return element;
        }

        public XElement VisitPointerReferenceExpression(PointerReferenceExpression pointerReferenceExpression)
        {
            var element = new XElement("PointerReferenceExpression", 
                new XAttribute("MemberName", pointerReferenceExpression.MemberName),
                Positions(pointerReferenceExpression));

            var targetElement = pointerReferenceExpression.Target.AcceptVisitor(this);
            element.Add(targetElement);

            foreach (var typeArgument in pointerReferenceExpression.TypeArguments)
            {
                var typeArgumentElement = typeArgument.AcceptVisitor(this);
                element.Add(typeArgumentElement);
            }

            return element;
        }

        public XElement VisitPrimitiveExpression(PrimitiveExpression primitiveExpression)
        {
            var element = new XElement("PrimitiveExpression",
                new XAttribute("LiteralFormat", primitiveExpression.Format.ToString()),
                new XAttribute("Value", primitiveExpression.Value),
               Positions(primitiveExpression));

            return element;
        }

        public XElement VisitSizeOfExpression(SizeOfExpression sizeOfExpression)
        {
            var element = new XElement("SizeOfExpression", Positions(sizeOfExpression));

            var typeElement = sizeOfExpression.Type.AcceptVisitor(this);
            element.Add(typeElement);

            return element;
        }

        public XElement VisitStackAllocExpression(StackAllocExpression stackAllocExpression)
        {
            var element = new XElement("StackAllocExpression", Positions(stackAllocExpression));
            
            var typeElement = stackAllocExpression.Type.AcceptVisitor(this);
            element.Add(typeElement);

            if (stackAllocExpression.CountExpression != Syntax.Expression.Null)
            {
                var countExpressionElement = stackAllocExpression.CountExpression.AcceptVisitor(this);
                element.Add(countExpressionElement);
            }

            var initializerElement = stackAllocExpression.Initializer.AcceptVisitor(this);
            element.Add(initializerElement);

            return element;
        }

        public XElement VisitThisReferenceExpression(ThisReferenceExpression thisReferenceExpression)
        {
            var element = new XElement("ThisReferenceExpression", Positions(thisReferenceExpression));
            return element;
        }

        public XElement VisitThrowExpression(ThrowExpression throwExpression)
        {
            var element = new XElement("ThrowExpression", Positions(throwExpression));

            if (throwExpression.Expression != Syntax.Expression.Null)
            {
                var throwExpressionElement = throwExpression.Expression.AcceptVisitor(this);
                element.Add(throwExpressionElement);
            }

            return element;
        }

        public XElement VisitTupleExpression(TupleExpression tupleExpression)
        {
            var element = new XElement("TupleExpression", Positions(tupleExpression));

            foreach (var e in tupleExpression.Elements)
            {
                var elementElement = e.AcceptVisitor(this);
                element.Add(elementElement);
            }

            return element;
        }

        public XElement VisitTypeOfExpression(TypeOfExpression typeOfExpression)
        {
            var element = new XElement("TypeOfExpression", Positions(typeOfExpression));

            var typeElement = typeOfExpression.Type.AcceptVisitor(this);
            element.Add(typeElement);

            return element;
        }

        public XElement VisitTypeReferenceExpression(TypeReferenceExpression typeReferenceExpression)
        {
            var element = new XElement("TypeReferenceExpression",
                Positions(typeReferenceExpression));

            var typeElement = typeReferenceExpression.Type.AcceptVisitor(this);
            element.Add(typeElement);

            return element;
        }

        public XElement VisitUnaryOperatorExpression(UnaryOperatorExpression unaryOperatorExpression)
        {
            var element = new XElement("UnaryOperatorExpression", 
                new XAttribute("Operator", unaryOperatorExpression.Operator),
                Positions(unaryOperatorExpression));

            var expressionElement = unaryOperatorExpression.Expression.AcceptVisitor(this);
            element.Add(expressionElement);

            return element;
        }

        public XElement VisitUncheckedExpression(UncheckedExpression uncheckedExpression)
        {
            var element = new XElement("UncheckedExpression", Positions(uncheckedExpression));

            var expressionElement = uncheckedExpression.Expression.AcceptVisitor(this);
            element.Add(expressionElement);

            return element;
        }

        public XElement VisitUndocumentedExpression(UndocumentedExpression undocumentedExpression)
        {
            throw new NotImplementedException();
        }

        public XElement VisitWithInitializerExpression(WithInitializerExpression withInitializerExpression)
        {
            var element = new XElement("WithInitializerExpression", Positions(withInitializerExpression));

            var expressionElement = withInitializerExpression.Expression.AcceptVisitor(this);
            element.Add(expressionElement);

            if (withInitializerExpression.Initializer != ArrayInitializerExpression.Null)
            {
                var initializerElement = withInitializerExpression.Initializer.AcceptVisitor(this);
                element.Add(initializerElement);
            }
            return element;
        }

        public XElement VisitQueryExpression(QueryExpression queryExpression)
        {
            var element = new XElement("QueryExpression", Positions(queryExpression));

            foreach (var clause in queryExpression.Clauses)
            {
                var clauseElement = clause.AcceptVisitor(this);
                element.Add(clauseElement);
            }

            return element;
        }

        public XElement VisitQueryContinuationClause(QueryContinuationClause queryContinuationClause)
        {
            var element = new XElement("QueryContinuationClause", 
                new XAttribute("Identifier", queryContinuationClause.Identifier),
                Positions(queryContinuationClause));

            var precedingQueryElement = queryContinuationClause.PrecedingQuery.AcceptVisitor(this);
            element.Add(precedingQueryElement);

            return element;
        }

        public XElement VisitQueryFromClause(QueryFromClause queryFromClause)
        {
            var element = new XElement("QueryFromClause",
                new XAttribute("Identifier", queryFromClause.Identifier),
                Positions(queryFromClause));

            if (queryFromClause.Type != AstType.Null)
            {
                var typeElement = queryFromClause.Type.AcceptVisitor(this);
                element.Add(typeElement);
            }

            var expressionElement = queryFromClause.Expression.AcceptVisitor(this);
            element.Add(expressionElement);

            return element;
        }

        public XElement VisitQueryLetClause(QueryLetClause queryLetClause)
        {
            var element = new XElement("QueryLetClause",
                new XAttribute("Identifier", queryLetClause.Identifier), 
                Positions(queryLetClause));

            var expressionElement = queryLetClause.Expression.AcceptVisitor(this);
            element.Add(expressionElement);

            return element;
        }

        public XElement VisitQueryWhereClause(QueryWhereClause queryWhereClause)
        {
            var element = new XElement("QueryWhereClause", Positions(queryWhereClause));

            var conditionElement = queryWhereClause.Condition.AcceptVisitor(this);
            element.Add(conditionElement);

            return element;
        }

        public XElement VisitQueryJoinClause(QueryJoinClause queryJoinClause)
        {
            var element = new XElement("QueryJoinClause", 
                new XAttribute("IsGroupJoin", queryJoinClause.IsGroupJoin),
                new XAttribute("IntoIdentifier", queryJoinClause.IntoIdentifier),
                new XAttribute("JoinIdentifier", queryJoinClause.JoinIdentifier),
                Positions(queryJoinClause));

            if (queryJoinClause.EqualsExpression != Syntax.Expression.Null)
            {
                var equalsExpressionElement = queryJoinClause.EqualsExpression.AcceptVisitor(this);
                element.Add(equalsExpressionElement);
            }

            if (queryJoinClause.OnExpression != Syntax.Expression.Null)
            {
                var onExpressionElement = queryJoinClause.OnExpression.AcceptVisitor(this);
                element.Add(onExpressionElement);
            }

            if (queryJoinClause.InExpression != Syntax.Expression.Null)
            {
                var inExpressionElement = queryJoinClause.InExpression.AcceptVisitor(this);
                element.Add(inExpressionElement);
            }

            var typeElement = queryJoinClause.Type.AcceptVisitor(this);
            element.Add(typeElement);

            return element;
        }

        public XElement VisitQueryOrderClause(QueryOrderClause queryOrderClause)
        {
            var element = new XElement("QueryOrderClause", Positions(queryOrderClause));

            foreach (var ordering in queryOrderClause.Orderings)
            {
                var orderElement = ordering.AcceptVisitor(this);
                element.Add(orderElement);
            }

            return element;
        }

        public XElement VisitQueryOrdering(QueryOrdering queryOrdering)
        {
            var element = new XElement("QueryOrdering", 
                new XAttribute("Direction", queryOrdering.Direction),
                Positions(queryOrdering));

            var expressionElement = queryOrdering.Expression.AcceptVisitor(this);
            element.Add(expressionElement);

            return element;
        }

        public XElement VisitQuerySelectClause(QuerySelectClause querySelectClause)
        {
            var element = new XElement("QuerySelectClause", Positions(querySelectClause));

            var expressionElement = querySelectClause.Expression.AcceptVisitor(this);
            element.Add(expressionElement);

            return element;
        }

        public XElement VisitQueryGroupClause(QueryGroupClause queryGroupClause)
        {
            var element = new XElement("QueryGroupClause", Positions(queryGroupClause));

            var projectionElement = queryGroupClause.Projection.AcceptVisitor(this);
            element.Add(projectionElement);

            var keyClauseElement = queryGroupClause.Key.AcceptVisitor(this);
            element.Add(queryGroupClause);

            return element;
        }

        public XElement VisitAttribute(Syntax.Attribute attribute)
        {
            var element = new XElement("Attribute", 
                new XAttribute("HasArgumentList", attribute.HasArgumentList),
                Positions(attribute));

            foreach (var argument in attribute.Arguments)
            {
                var argumentExpression = argument.AcceptVisitor(this);
                element.Add(argumentExpression);
            }

            return element;
        }

        public XElement VisitAttributeSection(AttributeSection attributeSection)
        {
            var element = new XElement("AttributeSection",
                 new XAttribute("AttributeTarget", attributeSection.AttributeTarget),
                 Positions(attributeSection));

            foreach (var attribute in attributeSection.Attributes)
            {
                var attributeElement = attribute.AcceptVisitor(this);
                element.Add(attributeElement);
            }

            return element;
        }

        public XElement VisitDelegateDeclaration(DelegateDeclaration delegateDeclaration)
        {
            var element = new XElement("DelegateDeclaration", Positions(delegateDeclaration));

            foreach (var typeParameter in delegateDeclaration.TypeParameters)
            {
                var typeParameterElement = typeParameter.AcceptVisitor(this);
                element.Add(typeParameterElement);
            }

            foreach (var parameter in delegateDeclaration.Parameters)
            {
                var parameterElement = parameter.AcceptVisitor(this);
                element.Add(parameterElement);
            }

            foreach (var constraint in delegateDeclaration.Constraints)
            {
                var constraintElement = constraint.AcceptVisitor(this);
                element.Add(constraintElement);
            }

            return element;
        }

        public XElement VisitNamespaceDeclaration(NamespaceDeclaration namespaceDeclaration)
        {
            var element = new XElement("NamespaceDeclaration", 
                new XAttribute("Name", namespaceDeclaration.FullName),
                Positions(namespaceDeclaration));

            foreach (var member in namespaceDeclaration.Members)
            {
                var memberElement = member.AcceptVisitor<XElement>(this);
                element.Add(memberElement);
            }

            return element;
        }

        public XElement VisitTypeDeclaration(TypeDeclaration typeDeclaration)
        {
            var element = new XElement("TypeDeclaration", 
                new XAttribute("Name", typeDeclaration.Name),
                new XAttribute("ClassType", typeDeclaration.ClassType),
                Positions(typeDeclaration));

            WriteModifierTokens(element, typeDeclaration.ModifierTokens);
            WriteTypeParameters(element, typeDeclaration.TypeParameters);

            var baseTypesElement = new XElement("BaseTypes");
            element.Add(baseTypesElement);

            foreach (var bt in typeDeclaration.BaseTypes)
            {
                var baseTypeElement = bt.AcceptVisitor<XElement>(this);
                baseTypesElement.Add(baseTypeElement);
            }

            // Type constraints
            if (typeDeclaration.Constraints.Any())
            {
                var constraintsElement = new XElement("Constraints");
                element.Add(constraintsElement);

                foreach (Constraint constraint in typeDeclaration.Constraints)
                {
                    var c = constraint.AcceptVisitor(this);
                    constraintsElement.Add(c);
                }
            }

            WriteAttributes(element, typeDeclaration.Attributes);

            foreach (var member in typeDeclaration.Members)
            {
                var memberElement = member.AcceptVisitor(this);
                element.Add(memberElement);
            }

            return element;
        }

        public XElement VisitUsingAliasDeclaration(UsingAliasDeclaration usingAliasDeclaration)
        {
            var element = new XElement("UsingAliasDeclaration", 
                new XAttribute("Alias", usingAliasDeclaration.Import),
                Positions(usingAliasDeclaration));

            var importElement = usingAliasDeclaration.Import.AcceptVisitor(this);
            element.Add(importElement);

            return element;
        }

        public XElement VisitUsingDeclaration(UsingDeclaration usingDeclaration)
        {
            var element = new XElement("UsingDeclaration",
                Positions(usingDeclaration));

            var importElement = usingDeclaration.Import.AcceptVisitor<XElement>(this);
            element.Add(importElement);

            return element;
        }

        public XElement VisitExternAliasDeclaration(ExternAliasDeclaration externAliasDeclaration)
        {
            throw new NotImplementedException();
        }

        public XElement VisitBlockStatement(BlockStatement blockStatement)
        {
            var element = new XElement("BlockStatement",
                Positions(blockStatement));

            foreach(var statement in blockStatement.Statements)
            {
                var statementElement = statement.AcceptVisitor(this);
                element.Add(statementElement);
            }

            return element;
        }

        public XElement VisitBreakStatement(BreakStatement breakStatement)
        {
            var element = new XElement("BreakStatement", Positions(breakStatement));
            return element;
        }

        public XElement VisitCheckedStatement(CheckedStatement checkedStatement)
        {
            var element = new XElement("CheckedStatement", Positions(checkedStatement));

            var bodyElement = checkedStatement.Body.AcceptVisitor(this);
            element.Add(bodyElement);

            return element;
        }

        public XElement VisitContinueStatement(ContinueStatement continueStatement)
        {
            var element = new XElement("ContinueStatement", Positions(continueStatement));
            return element;
        }

        public XElement VisitDoWhileStatement(DoWhileStatement doWhileStatement)
        {
            var element = new XElement("DoWhileStatement", Positions(doWhileStatement));

            var embeddedStatementElement = doWhileStatement.EmbeddedStatement.AcceptVisitor(this);
            element.Add(embeddedStatementElement);

            var conditionElement = doWhileStatement.Condition.AcceptVisitor(this);
            element.Add(conditionElement);

            return element;
        }

        public XElement VisitEmptyStatement(EmptyStatement emptyStatement)
        {
            var element = new XElement("EmptyStatement", Positions(emptyStatement));
            return element;
        }

        public XElement VisitExpressionStatement(ExpressionStatement expressionStatement)
        {
            var element = new XElement("ExpressionStatement",
                Positions(expressionStatement));

            var expressionElement = expressionStatement.Expression.AcceptVisitor(this);
            element.Add(expressionElement);

            return element;
        }

        public XElement VisitFixedStatement(FixedStatement fixedStatement)
        {
            throw new NotImplementedException();
        }

        public XElement VisitForeachStatement(ForeachStatement foreachStatement)
        {
            var element = new XElement("ForeachStatement", 
                new XAttribute("IsAsync", foreachStatement.IsAsync),
                Positions(foreachStatement));

            var variableTypeElement = foreachStatement.VariableType.AcceptVisitor(this);
            element.Add(variableTypeElement);

            var variableDesignationElemnent = foreachStatement.VariableDesignation.AcceptVisitor(this);
            element.Add(variableDesignationElemnent);

            var inExpressionElement = foreachStatement.InExpression.AcceptVisitor(this);
            element.Add(inExpressionElement);
            
            var embeddedStatementElement = foreachStatement.EmbeddedStatement.AcceptVisitor(this);
            element.Add(embeddedStatementElement);

            return element;
        }

        public XElement VisitForStatement(ForStatement forStatement)
        {
            var element = new XElement("ForStatement", Positions(forStatement));

            foreach (var initializer in forStatement.Initializers)
            {
                var initializerElement = initializer.AcceptVisitor(this);
                element.Add(initializerElement);
            }

            var conditionElement = forStatement.Condition.AcceptVisitor(this);
            element.Add(conditionElement);

            foreach (var iterator in forStatement.Iterators)
            {
                var iteratorElement = iterator.AcceptVisitor(this);
                element.Add(iteratorElement);
            }

            var embeddedStatementElement = forStatement.EmbeddedStatement.AcceptVisitor(this);
            element.Add(embeddedStatementElement);

            return element;
        }

        public XElement VisitGotoCaseStatement(GotoCaseStatement gotoCaseStatement)
        {
            throw new NotImplementedException();
        }

        public XElement VisitGotoDefaultStatement(GotoDefaultStatement gotoDefaultStatement)
        {
            throw new NotImplementedException();
        }

        public XElement VisitGotoStatement(GotoStatement gotoStatement)
        {
            var element = new XElement("GotoStatement", 
                new XAttribute("Label", gotoStatement.Label),
                Positions(gotoStatement));

            return element;
        }

        public XElement VisitIfElseStatement(IfElseStatement ifElseStatement)
        {
            var element = new XElement("IfElseStatement", Positions(ifElseStatement));

            var conditionElement = ifElseStatement.Condition.AcceptVisitor(this);
            element.Add(conditionElement);

            var trueStatementElement = ifElseStatement.TrueStatement.AcceptVisitor(this);
            element.Add(trueStatementElement);

            if (ifElseStatement.FalseStatement != Statement.Null)
            {
                var falseStatementElement = ifElseStatement.FalseStatement.AcceptVisitor(this);
                element.Add(falseStatementElement);
            }
            return element;
        }

        public XElement VisitLabelStatement(LabelStatement labelStatement)
        {
            var element = new XElement("LabelStatement", 
                new XAttribute("Label", labelStatement.Label),
                Positions(labelStatement));

            return element;
        }

        public XElement VisitLockStatement(LockStatement lockStatement)
        {
            var element = new XElement("LockStatement", Positions(lockStatement));

            var expressionElement = lockStatement.Expression.AcceptVisitor(this);
            element.Add(expressionElement);

            var embeddedStatementElement = lockStatement.EmbeddedStatement.AcceptVisitor(this);
            element.Add(embeddedStatementElement);

            return element;
        }

        public XElement VisitReturnStatement(ReturnStatement returnStatement)
        {
            var element = new XElement("ReturnStatement", Positions(returnStatement));

            if (returnStatement.Expression != Syntax.Expression.Null)
            {
                var expressionElement = returnStatement.Expression.AcceptVisitor(this);
                element.Add(expressionElement);
            }

            return element;
        }

        public XElement VisitSwitchStatement(SwitchStatement switchStatement)
        {
            var element = new XElement("VisitSwitchStatement", Positions(switchStatement));

            var expressionElement = switchStatement.Expression.AcceptVisitor(this);
            element.Add(expressionElement);

            foreach (var switchSection in switchStatement.SwitchSections)
            {
                var switchSectionElement = switchSection.AcceptVisitor(this);
                element.Add(switchSectionElement);
            }

            return element;
        }

        public XElement VisitSwitchSection(Syntax.SwitchSection switchSection)
        {
            var element = new XElement("SwitchSection", Positions(switchSection));

            foreach (var caseLabel in switchSection.CaseLabels)
            {
                var caseLabelElement = caseLabel.AcceptVisitor(this);
                element.Add(caseLabelElement);
            }

            foreach (var statements in switchSection.Statements)
            {
                var statementElement = statements.AcceptVisitor(this);
                element.Add(statementElement);
            }

            return element;
        }

        public XElement VisitCaseLabel(CaseLabel caseLabel)
        {
            var element = new XElement("CaseLabel", Positions(caseLabel));

            if (caseLabel.Expression != Syntax.Expression.Null)
            {
                var expressionElement = caseLabel.Expression.AcceptVisitor(this);
                element.Add(expressionElement);
            }

            return element;
        }

        public XElement VisitSwitchExpression(Syntax.SwitchExpression switchExpression)
        {
            var element = new XElement("SwitchExpression", Positions(switchExpression));

            var expressionElement = switchExpression.Expression.AcceptVisitor(this);
            element.Add(expressionElement);

            foreach (var switchSection in switchExpression.SwitchSections)
            {
                var switchSectionElement = switchSection.AcceptVisitor(this);
                element.Add(switchSectionElement);
            }

            return element;
        }

        public XElement VisitSwitchExpressionSection(SwitchExpressionSection switchExpressionSection)
        {
            var element = new XElement("SwitchExpressionSection", Positions(switchExpressionSection));
            
            var patternElement = switchExpressionSection.Pattern.AcceptVisitor(this);
            element.Add(patternElement);

            var bodyElement = switchExpressionSection.Body.AcceptVisitor(this);
            element.Add(bodyElement);

            return element;
        }

        public XElement VisitThrowStatement(ThrowStatement throwStatement)
        {
            var element = new XElement("ThrowStatement", Positions(throwStatement));
            if (throwStatement.Expression != Syntax.Expression.Null)
            {
                var expressionElement = throwStatement.Expression.AcceptVisitor(this);
                element.Add(expressionElement);
            }

            return element;
        }

        public XElement VisitTryCatchStatement(TryCatchStatement tryCatchStatement)
        {
            var element = new XElement("TryCatchStatement", Positions(tryCatchStatement));

            var tryBlockElement = tryCatchStatement.TryBlock.AcceptVisitor(this);
            element.Add(tryBlockElement);

            foreach (var catchClause in tryCatchStatement.CatchClauses)
            {
                var catchClauseElement = catchClause.AcceptVisitor(this);
                element.Add(catchClauseElement);
            }

            if (tryCatchStatement.FinallyBlock != BlockStatement.Null)
            {
                var finallyBlockElement = tryCatchStatement.FinallyBlock.AcceptVisitor(this);
                element.Add(finallyBlockElement);
            }

            return element;
        }

        public XElement VisitCatchClause(CatchClause catchClause)
        {
            var element = new XElement("CatchClause", 
                new XAttribute("VariableName", catchClause.VariableName),
                Positions(catchClause));

            if (catchClause.Type != AstType.Null)
            {
                var typeElement = catchClause.Type.AcceptVisitor(this);
                element.Add(typeElement);
            }

            if (catchClause.Condition != Syntax.Expression.Null)
            {
                var conditionElement = catchClause.Condition.AcceptVisitor(this);
                element.Add(conditionElement);
            }

            return element;
        }

        public XElement VisitUncheckedStatement(UncheckedStatement uncheckedStatement)
        {
            var element = new XElement("UncheckedStatement", Positions(uncheckedStatement));

            var bodyElement = uncheckedStatement.AcceptVisitor(this);
            element.Add(bodyElement);
            
            return element;
        }

        public XElement VisitUnsafeStatement(UnsafeStatement unsafeStatement)
        {
            var element = new XElement("UnsafeStatement", Positions(unsafeStatement));

            var bodyElement = unsafeStatement.AcceptVisitor(this);
            element.Add(bodyElement);

            return element;
        }

        public XElement VisitUsingStatement(UsingStatement usingStatement)
        {
            var element = new XElement("UsingStatement",
                new XAttribute("IsAsync", usingStatement.IsAsync),
                new XAttribute("IsEnhanced", usingStatement.IsEnhanced),
                Positions(usingStatement));

            var resourceAcquisitionElement = usingStatement.ResourceAcquisition.AcceptVisitor(this);
            element.Add(resourceAcquisitionElement);

            var embeddedStatementElement = usingStatement.EmbeddedStatement.AcceptVisitor(this);
            element.Add(embeddedStatementElement);

            return element;
        }

        public XElement VisitVariableDeclarationStatement(VariableDeclarationStatement variableDeclarationStatement)
        {
            var element = new XElement("VariableDeclarationStatement", Positions(variableDeclarationStatement));
           
            WriteModifiers(element, variableDeclarationStatement.Modifiers);

            var typeElement = variableDeclarationStatement.Type.AcceptVisitor(this);
            element.Add(typeElement);

            foreach (var variable in variableDeclarationStatement.Variables)
            {
                var variableElement = variable.AcceptVisitor(this);
                element.Add(variableElement);
            }

            return element;
        }

        public XElement VisitLocalFunctionDeclarationStatement(LocalFunctionDeclarationStatement localFunctionDeclarationStatement)
        {
            var element = new XElement("LocalFunctionDeclarationStatement", Positions(localFunctionDeclarationStatement));

            var declarationElement = localFunctionDeclarationStatement.Declaration.AcceptVisitor(this);
            element.Add(declarationElement);

            return element;
        }

        public XElement VisitWhileStatement(WhileStatement whileStatement)
        {
            var element = new XElement("WhileStatement", Positions(whileStatement));

            var conditionElement = whileStatement.Condition.AcceptVisitor(this);
            element.Add(conditionElement);

            var embeddedStatementElement = whileStatement.EmbeddedStatement.AcceptVisitor(this);
            element.Add(embeddedStatementElement);

            return element;
        }

        public XElement VisitYieldBreakStatement(YieldBreakStatement yieldBreakStatement)
        {
            var element = new XElement("YieldBreakStatement", Positions(yieldBreakStatement));
            return element;
        }

        public XElement VisitYieldReturnStatement(YieldReturnStatement yieldReturnStatement)
        {
            var element = new XElement("YieldReturnStatement", Positions(yieldReturnStatement));

            var expressionElement = yieldReturnStatement.Expression.AcceptVisitor(this);
            element.Add(expressionElement);

            return element;
        }

        public XElement VisitAccessor(Accessor accessor)
        {
            var element = new XElement("Accessor", Positions(accessor));

            if (accessor.Body != BlockStatement.Null)
            {
                var bodyElement = accessor.Body.AcceptVisitor(this);
                element.Add(bodyElement);
            }
            return element;
        }

        public XElement VisitConstructorDeclaration(ConstructorDeclaration constructorDeclaration)
        {
            var element = new XElement("ConstructorDeclaration",
                new XAttribute("Kind", constructorDeclaration.SymbolKind.ToString()),
                Positions(constructorDeclaration));

            var parametersElement = new XElement("Parameters");
            element.Add(parametersElement);

            foreach(var parameter in constructorDeclaration.Parameters)
            {
                var parameterElement = parameter.AcceptVisitor(this);
                parametersElement.Add(parameterElement);
            }

            if (constructorDeclaration.Initializer != ConstructorInitializer.Null)
            {
                var constructorInitializerElement = constructorDeclaration.Initializer.AcceptVisitor(this);
                element.Add(constructorInitializerElement);
            }
            
            if (constructorDeclaration.Body != BlockStatement.Null)
            {
                var bodyelement = constructorDeclaration.Body.AcceptVisitor(this);
                element.Add(bodyelement);
            }

            return element;
        }

        public XElement VisitConstructorInitializer(ConstructorInitializer constructorInitializer)
        {
            var element = new XElement("ConstructorInitializer",
                new XAttribute("ConstructorInitializerType", constructorInitializer.ConstructorInitializerType.ToString()),
               Positions(constructorInitializer));
            
            foreach (var argument in constructorInitializer.Arguments)
            {
                var argumentElement = argument.AcceptVisitor(this);
                element.Add(argumentElement);
            }   

            return element;
        }

        public XElement VisitDestructorDeclaration(DestructorDeclaration destructorDeclaration)
        {
            throw new NotImplementedException();
        }

        public XElement VisitEnumMemberDeclaration(EnumMemberDeclaration enumMemberDeclaration)
        {
            var element = new XElement("EnumMemberDeclaration", Positions(enumMemberDeclaration));

            if (enumMemberDeclaration.Initializer != Syntax.Expression.Null)
            {
                var enumMemberDeclarationElement = enumMemberDeclaration.AcceptVisitor(this);
                element.Add(enumMemberDeclarationElement);
            }

            return element;
        }

        public XElement VisitEventDeclaration(EventDeclaration eventDeclaration)
        {
            var element = new XElement("EventDeclaration",
                new XAttribute("Name", eventDeclaration.Name),
                Positions(eventDeclaration));

            foreach (var variable in eventDeclaration.Variables)
            {
                var variableElement = variable.AcceptVisitor(this);
                element.Add(variableElement);
            }

            return element;
        }

        public XElement VisitCustomEventDeclaration(CustomEventDeclaration customEventDeclaration)
        {
            var element = new XElement("CustomEventDeclaration", Positions(customEventDeclaration));

            if (customEventDeclaration.AddAccessor != Accessor.Null)
            {
                var addAccessorElement = customEventDeclaration.AddAccessor.AcceptVisitor(this);
                element.Add(addAccessorElement);
            }

            if (customEventDeclaration.RemoveAccessor != Accessor.Null)
            {
                var removeAccessorElement = customEventDeclaration.RemoveAccessor.AcceptVisitor(this);
                element.Add(removeAccessorElement);
            }

            return element;
        }

        public XElement VisitFieldDeclaration(FieldDeclaration fieldDeclaration)
        {
            var element = new XElement("FieldDeclaration", Positions(fieldDeclaration));

            foreach (var variable in fieldDeclaration.Variables)
            {
                var variableElement = variable.AcceptVisitor(this);
                element.Add(variableElement);
            }

            return element;
        }

        public XElement VisitIndexerDeclaration(IndexerDeclaration indexerDeclaration)
        {
            var element = new XElement("IndexerDeclaration", 
                new XAttribute("Name", indexerDeclaration.Name),
                Positions(indexerDeclaration));

            foreach (var parameter in indexerDeclaration.Parameters)
            {
                var parameterElement = parameter.AcceptVisitor(this);
                element.Add(parameterElement);
            }

            if (indexerDeclaration.Getter != Accessor.Null)
            {
                var getterElement = indexerDeclaration.Getter.AcceptVisitor(this);
                element.Add(getterElement);
            }

            if (indexerDeclaration.Setter != Accessor.Null)
            {
                var setterElement = indexerDeclaration.Getter.AcceptVisitor(this);
                element.Add(setterElement);
            }

            if (indexerDeclaration.ExpressionBody != Syntax.Expression.Null)
            {
                var expressionElement = indexerDeclaration.ExpressionBody.AcceptVisitor(this);
                element.Add(expressionElement);
            }

            return element;
        }

        public XElement VisitMethodDeclaration(MethodDeclaration methodDeclaration)
        {
            var element = new XElement("MethodDeclaration",
                new XAttribute("Name", methodDeclaration.Name),
                new XAttribute("SymbolKind", methodDeclaration.SymbolKind.ToString()),
                Positions(methodDeclaration));

            WriteModifierTokens(element, methodDeclaration.ModifierTokens);

            if (methodDeclaration.PrivateImplementationType != AstType.Null)
            {
                var implementedInterfacesElement = new XElement("PrivateImplementationType");
                element.Add(implementedInterfacesElement);

                var implementedInterfaceElement = methodDeclaration.PrivateImplementationType.AcceptVisitor(this);
                implementedInterfacesElement.Add(implementedInterfaceElement);
            }

            var returnTypeElement = methodDeclaration.ReturnType.AcceptVisitor(this);
            element.Add(returnTypeElement);

            foreach (var typeParameter in methodDeclaration.TypeParameters)
            {
                var typeParameterElement = typeParameter.AcceptVisitor(this);
                element.Add(typeParameterElement);
            }

            foreach (var constraint in methodDeclaration.Constraints)
            {
                var constraintElement = constraint.AcceptVisitor(this);
                element.Add(constraintElement);
            }

            var parametersElement = new XElement("Parameters");
            element.Add(parametersElement);
            foreach (var parameter in methodDeclaration.Parameters)
            {
                var parameterElement = parameter.AcceptVisitor(this);
                parametersElement.Add(parameterElement);
            }

            if (methodDeclaration.Body != BlockStatement.Null)
            {
                var bodyElement = methodDeclaration.Body.AcceptVisitor(this);
                element.Add(bodyElement);
            }

            return element;
        }

        public XElement VisitOperatorDeclaration(OperatorDeclaration operatorDeclaration)
        {
            var element = new XElement("OperatorDeclaration", 
                new XAttribute("OperatorType", operatorDeclaration.OperatorType),
                Positions(operatorDeclaration));

            foreach (var parameter in operatorDeclaration.Parameters)
            {
                var paramterElement = parameter.AcceptVisitor(this);
                element.Add(paramterElement);
            }

            var bodyElement = operatorDeclaration.Body.AcceptVisitor(this);
            element.Add(bodyElement);

            return element;
        }

        public XElement VisitParameterDeclaration(ParameterDeclaration parameterDeclaration)
        {
            var element = new XElement("ParameterDeclaration",
                new XAttribute("Name", parameterDeclaration.Name),
                new XAttribute("HasThisModifier", parameterDeclaration.HasThisModifier),
                new XAttribute("Modifier", parameterDeclaration.ParameterModifier.ToString()),
               Positions(parameterDeclaration));

            if (parameterDeclaration.Type != Syntax.AstType.Null)
            {
                var parameterTypeElement = parameterDeclaration.Type.AcceptVisitor(this);
                element.Add(parameterTypeElement);
            }

            foreach (var attribute in parameterDeclaration.Attributes)
            {
                var attributeElement = attribute.AcceptVisitor<XElement>(this);
                element.Add(attributeElement);
            }

            if (parameterDeclaration.DefaultExpression != Syntax.Expression.Null)
            {
                var expressionElement = parameterDeclaration.DefaultExpression.AcceptVisitor<XElement>(this);
                element.Add(expressionElement);
            }

            return element;
        }

        public XElement VisitPropertyDeclaration(PropertyDeclaration propertyDeclaration)
        {
            var element = new XElement("PropertyDeclaration", Positions(propertyDeclaration));

            if (propertyDeclaration.Getter != Accessor.Null)
            {
                var getterElement = propertyDeclaration.Getter.AcceptVisitor(this);
                element.Add(getterElement);
            }

            if (propertyDeclaration.Setter != Accessor.Null)
            {
                var setterElement = propertyDeclaration.Setter.AcceptVisitor(this);
                element.Add(setterElement);
            }

            if (propertyDeclaration.Initializer != Syntax.Expression.Null)
            {
                var initializerElement = propertyDeclaration.Initializer.AcceptVisitor(this);
                element.Add(initializerElement);
            }

            if (propertyDeclaration.ExpressionBody != Syntax.Expression.Null)
            {
                var expressionElement = propertyDeclaration.ExpressionBody.AcceptVisitor(this);
                element.Add(expressionElement);
            }

            return element;
        }

        public XElement VisitVariableInitializer(VariableInitializer variableInitializer)
        {
            var element = new XElement("VariableInitializer", 
                new XAttribute("Name", variableInitializer.Name),
                Positions(variableInitializer));

            if (variableInitializer.Initializer != Syntax.Expression.Null)
            {
                var initializerElement = variableInitializer.Initializer.AcceptVisitor(this);
                element.Add(initializerElement);
            }

            return element;
        }

        public XElement VisitFixedFieldDeclaration(FixedFieldDeclaration fixedFieldDeclaration)
        {
            throw new NotImplementedException();
        }

        public XElement VisitFixedVariableInitializer(FixedVariableInitializer fixedVariableInitializer)
        {
            var element = new XElement("FixedVariableInitializer", 
                new XAttribute("Name", fixedVariableInitializer.Name),
                Positions(fixedVariableInitializer));
            
            var countExpressionElement = fixedVariableInitializer.CountExpression.AcceptVisitor(this);
            element.Add(countExpressionElement);

            return element;
        }

        public XElement VisitSyntaxTree(SyntaxTree syntaxTree)
        {
            var root = new XElement("Type", 
                new XAttribute("Language", "C#"),
                new XAttribute("Assembly", this.assemblyFile),
                new XAttribute("Source", this.source));
            document.Add(root);

            foreach (AstNode node in syntaxTree.Children)
            {
                var e = node.AcceptVisitor(this);
                root.Add(e);
            }

            // Find the Type and create a artifact attribute based on the 
            // namespace declatation and the name of the type.
            var fullyQualifiedName = string.Empty;
            var ns = root.XPathSelectElement("NamespaceDeclaration");
            if (ns != null)
            {
                fullyQualifiedName = ns.Attribute("Name").Value + ".";
            }

            var typeDeclaration = ns.XPathSelectElement("TypeDeclaration");
            if (typeDeclaration != null)
            {
                fullyQualifiedName += typeDeclaration.Attribute("Name").Value;
            }

            root.Add(new XAttribute("Artifact", $"Type:{fullyQualifiedName}"));
            return this.document.Root;
        }

        public XElement VisitSimpleType(SimpleType simpleType)
        {
            var element = new XElement("SimpleType", 
                new XAttribute("Name", simpleType.Identifier),
                Positions(simpleType));

            if (simpleType.TypeArguments.Any())
            {
                var typeArgumentsNode = new XElement("TypeArguments");
                foreach (var typeArg in simpleType.TypeArguments)
                {
                    var typeArgumentNode = typeArg.AcceptVisitor<XElement>(this);
                    typeArgumentsNode.Add(typeArgumentNode);
                }
                element.Add(typeArgumentsNode);
            }
            return element;
        }

        public XElement VisitMemberType(MemberType memberType)
        {
            var element = new XElement("MemberType",
                new XAttribute("IsDoubleColon", memberType.IsDoubleColon),
                new XAttribute("MemberName", memberType.MemberName),
                Positions(memberType));

            var targetElement = memberType.Target.AcceptVisitor(this);
            element.Add(targetElement);

            foreach (var typeArgument in memberType.TypeArguments)
            {
                var typeArgumentElement = typeArgument.AcceptVisitor<XElement>(this);
                element.Add(typeArgumentElement);
            }

            return element;
        }

        public XElement VisitTupleType(TupleAstType tupleType)
        {
            var element = new XElement("TupleAstType",
                 Positions(tupleType));

            foreach (var tupleElement in tupleType.Elements)
            {
                var tupleElementElement = tupleElement.AcceptVisitor(this);
                element.Add(tupleElementElement);
            }

            return element;
        }

        public XElement VisitTupleTypeElement(TupleTypeElement tupleTypeElement)
        {
            var element = new XElement("TupleTypeElement", 
                new XAttribute("Name", tupleTypeElement.Name),
                Positions(tupleTypeElement));

            var typeElement = tupleTypeElement.Type.AcceptVisitor(this);
            element.Add(typeElement);

            return element;
        }

        public XElement VisitFunctionPointerType(FunctionPointerAstType functionPointerType)
        {
            throw new NotImplementedException();
        }

        public XElement VisitInvocationType(InvocationAstType invocationType)
        {
            var element = new XElement("InvocationAstType", Positions(invocationType));

            var baseTypeElement = invocationType.BaseType.AcceptVisitor(this);
            element.Add(baseTypeElement);

            foreach (var argument in invocationType.Arguments)
            {
                var argumentElement = argument.AcceptVisitor(this);
                element.Add(argumentElement);
            }

            return element;
        }

        public XElement VisitComposedType(ComposedType composedType)
        {
            var element = new XElement("ComposedType",
                new XAttribute("HasRefSpecifier", composedType.HasRefSpecifier),
                new XAttribute("HasReadonlySpecifier", composedType.HasReadOnlySpecifier),
                new XAttribute("HasNullableSpecifier", composedType.HasNullableSpecifier),
                new XAttribute("HasOnlyNullableSpecifier", composedType.HasOnlyNullableSpecifier),
                new XAttribute("PointerRank", composedType.PointerRank),
                Positions(composedType));

            var baseTypeElement = composedType.BaseType.AcceptVisitor(this);
            element.Add(baseTypeElement);

            foreach (var attribute in composedType.Attributes)
            {
                var attributeElement = attribute.AcceptVisitor(this);
                element.Add(attributeElement);
            }

            foreach (var arraySpecifier in composedType.ArraySpecifiers)
            {
                var arraySpecifierElement = arraySpecifier.AcceptVisitor(this);
                element.Add(arraySpecifierElement);
            }  
            
            return element;
        }

        public XElement VisitArraySpecifier(ArraySpecifier arraySpecifier)
        {
            var element = new XElement("ArraySpecifier",
                new XAttribute("Dimensions", arraySpecifier.Dimensions),
                Positions(arraySpecifier));

            return element;
        }

        public XElement VisitPrimitiveType(Syntax.PrimitiveType primitiveType)
        {
            var element = new XElement("PrimitiveType",
                new XAttribute("Name", primitiveType.Keyword),
                Positions(primitiveType));

            return element;
        }

        /// <summary>
        /// These are comments that are inserted by the decompiler, not 
        /// comments provided by the user.
        /// </summary>
        /// <param name="comment">Comment provided by the decompiler.</param>
        /// <returns>An XML element containing the error message</returns>
        public XElement VisitComment(Comment comment)
        {
            var element = new XElement("Comment", 
                new XAttribute("IsDocumentation", comment.IsDocumentation),
                new XAttribute("Content", comment.Content),
                Positions(comment));
            return element;
        }

        public XElement VisitPreProcessorDirective(PreProcessorDirective preProcessorDirective)
        {
            var element = new XElement("PreProcessorDirective",
                new XAttribute("Type", preProcessorDirective.Type),
                new XAttribute("Argument", preProcessorDirective.Argument),
                new XAttribute("Take", preProcessorDirective.Take),
                Positions(preProcessorDirective));
            return element;
        }

        public XElement VisitDocumentationReference(DocumentationReference documentationReference)
        {
            var element = new XElement("DocumentationReference",
                new XAttribute("HasParameterList", documentationReference.HasParameterList),
                new XAttribute("MemberName", documentationReference.MemberName),
                new XAttribute("OperatorType", documentationReference.OperatorType),
                Positions(documentationReference));

            var declaringType = documentationReference.DeclaringType.AcceptVisitor(this);
            element.Add(declaringType);

            var conversionOperatorReturnTypeElement = documentationReference.ConversionOperatorReturnType.AcceptVisitor(this);
            element.Add(conversionOperatorReturnTypeElement);

            foreach (var parameter in documentationReference.Parameters)
            {
                var parameterElement = parameter.AcceptVisitor(this);
                element.Add(parameterElement);
            }

            foreach (var typeArgument in documentationReference.TypeArguments)
            {
                var typeArgumentElement = typeArgument.AcceptVisitor(this);
                element.Add(typeArgumentElement);
            }

            return element;
        }

        public XElement VisitTypeParameterDeclaration(TypeParameterDeclaration typeParameterDeclaration)
        {
            var element = new XElement("TypeParameterDeclaration", 
                new XAttribute("Name", typeParameterDeclaration.Name),
                new XAttribute("Variance", typeParameterDeclaration.Variance.ToString()),
                Positions(typeParameterDeclaration));

            foreach(var attribute in typeParameterDeclaration.Attributes)
            {
                var attributeElement = attribute.AcceptVisitor<XElement>(this);
                element.Add(attributeElement);
            }

            return element;
        }

        public XElement VisitConstraint(Constraint constraint)
        {
            var element = new XElement("Constraint",
                Positions(constraint));

            var typeParameterElement = constraint.TypeParameter.AcceptVisitor<XElement>(this);
            element.Add(typeParameterElement);

            foreach(var basetype in constraint.BaseTypes)
            {
                var basetypeElement = basetype.AcceptVisitor<XElement>(this);
                element.Add(basetypeElement);
            }

            return element;
        }

        public XElement VisitCSharpTokenNode(CSharpTokenNode cSharpTokenNode)
        {
            throw new NotImplementedException();
        }

        public XElement VisitIdentifier(Identifier identifier)
        {
            var element = new XElement("Identifier", 
                new XAttribute("Name", identifier.Name),
                Positions(identifier));

            return element;
        }

        public XElement VisitInterpolation(Interpolation interpolation)
        {
            var element = new XElement("Interpolation", Positions(interpolation));

            if (interpolation.Suffix != null)
            {
                element.Add(new XAttribute("Suffix", interpolation.Suffix));
            }

            var expressionElement = interpolation.Expression.AcceptVisitor(this);
            element.Add(expressionElement);

            return element;
        }

        public XElement VisitInterpolatedStringText(InterpolatedStringText interpolatedStringText)
        {
            var element = new XElement("InterpolatedStringText", 
                new XAttribute("Text", interpolatedStringText.Text),
                Positions(interpolatedStringText));

            return element;
        }

        public XElement VisitSingleVariableDesignation(SingleVariableDesignation singleVariableDesignation)
        {
            var element = new XElement("SingleVariableDesignation", 
                new XAttribute("Identifier", singleVariableDesignation.Identifier),
                Positions(singleVariableDesignation));

            return element;
        }

        public XElement VisitParenthesizedVariableDesignation(ParenthesizedVariableDesignation parenthesizedVariableDesignation)
        {
            var element = new XElement("ParenthesizedVariableDesignation", Positions(parenthesizedVariableDesignation));

            foreach (var designation in parenthesizedVariableDesignation.VariableDesignations)
            {
                var designationElement = designation.AcceptVisitor(this);
                element.Add(designationElement);
            }

            return element;
        }

        public XElement VisitNullNode(AstNode nullNode)
        {
            throw new NotImplementedException();
        }

        public XElement VisitErrorNode(AstNode errorNode)
        {
            throw new NotImplementedException();
        }

        public XElement VisitPatternPlaceholder(AstNode placeholder, Pattern pattern)
        {
            throw new NotImplementedException();
        }
    }
}
