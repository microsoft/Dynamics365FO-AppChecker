// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSqlExtractor
{
    using Microsoft.SqlServer.TransactSql.ScriptDom;
    using System.Runtime.CompilerServices;
    using System.Xml.Linq;

    class TSQLVisitor : TSqlConcreteFragmentVisitor
    {
        private readonly XElement rootElement;

        /// <summary>
        /// Indicates whether or not the source should be included in the node defining classes and 
        /// other top level compilation units. This should really be fetched from a settings file or
        /// an environment variable.
        /// </summary>
        private bool IncludeSource { get { return true; } }

        /// <summary>
        /// This is the stack of XML elements built during extraction. This is needed because there
        /// is no payload information that can be passed to each visit* method
        /// </summary>
        private readonly Stack<XElement> stack = new Stack<XElement>();

        private static string ElementName(TSqlFragment node)
        {
            var fullName = node.ToString();
            return fullName.Split('.').Last();
        }
        private static XElement CreateElement(TSqlFragment node)
        {
            string nodeKind = ElementName(node);
            return new XElement(nodeKind);
        }

        private TSQLVisitor(XElement root)
        {
            this.rootElement = root;
        }

        public static XDocument Generate(TSqlFragment fragment, string source = "")
        {
            var document = new XDocument();
            var compilationElement = new XElement("Compilation",
                new XAttribute("Version", "0.1"),
                new XAttribute("Language", "SQL"),
                new XAttribute("Source", source));

            document.Add(compilationElement);

            var visitor = new TSQLVisitor(compilationElement);
            visitor.stack.Push(compilationElement);
            fragment.Accept(visitor);

            return document;
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropAsymmetricKeyStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("RemoveProviderKey", node.RemoveProviderKey));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ExecutableProcedureReference node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropCredentialStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsDatabaseScoped", node.IsDatabaseScoped));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.OpenRowsetTableReference node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateAvailabilityGroupStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropCertificateStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SimpleAlterFullTextIndexAction node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ActionKind", node.ActionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.OpenQueryTableReference node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.BulkOpenRowset node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AvailabilityReplica node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.InternalOpenRowset node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterPartitionFunctionStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsSplit", node.IsSplit));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterFullTextIndexStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ProcedureReferenceName node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterPartitionSchemeStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SetStopListAlterFullTextIndexAction node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("WithNoPopulation", node.WithNoPopulation));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterAvailabilityGroupStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("AlterAvailabilityGroupStatementType", node.AlterAvailabilityGroupStatementType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AddSearchPropertyListAction node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.OpenJsonTableReference node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterServerConfigurationSetFailoverClusterPropertyStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.OnOffPrincipalOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionState", node.OptionState));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SearchedCaseExpression node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.LiteralPrincipalOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.IdentifierPrincipalOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterServerConfigurationDiagnosticsLogMaxSizeOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("SizeUnit", node.SizeUnit));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SimpleCaseExpression node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.WindowsCreateLoginSource node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterViewStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsRebuild", node.IsRebuild));
            element.Add(new XAttribute("IsDisable", node.IsDisable));
            element.Add(new XAttribute("WithCheckOption", node.WithCheckOption));
            element.Add(new XAttribute("IsMaterialized", node.IsMaterialized));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ExternalCreateLoginSource node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ViewOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterServerConfigurationDiagnosticsLogOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AsymmetricKeyCreateLoginSource node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SearchedWhenClause node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterServerConfigurationSetDiagnosticsLogStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.PasswordAlterPrincipalOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("MustChange", node.MustChange));
            element.Add(new XAttribute("Unlock", node.Unlock));
            element.Add(new XAttribute("Hashed", node.Hashed));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateViewStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("WithCheckOption", node.WithCheckOption));
            element.Add(new XAttribute("IsMaterialized", node.IsMaterialized));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SimpleWhenClause node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterLoginOptionsStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterServerConfigurationBufferPoolExtensionSizeOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("SizeUnit", node.SizeUnit));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CertificateCreateLoginSource node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SetSearchPropertyListAlterFullTextIndexAction node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("WithNoPopulation", node.WithNoPopulation));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.PrincipalOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.PasswordCreateLoginSource node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Hashed", node.Hashed));
            element.Add(new XAttribute("MustChange", node.MustChange));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropAlterFullTextIndexAction node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("WithNoPopulation", node.WithNoPopulation));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AddAlterFullTextIndexAction node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("WithNoPopulation", node.WithNoPopulation));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.OpenXmlTableReference node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterServerConfigurationSoftNumaOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterColumnAlterFullTextIndexAction node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("WithNoPopulation", node.WithNoPopulation));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ExecutableStringList node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateSearchPropertyListStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SemanticTableReference node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("SemanticFunctionType", node.SemanticFunctionType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterSearchPropertyListStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterServerConfigurationSetSoftNumaStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.NullIfExpression node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterServerConfigurationHadrClusterOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("IsLocal", node.IsLocal));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropSearchPropertyListAction node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.IIfCall node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AdHocDataSource node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropSearchPropertyListStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterServerConfigurationSetHadrClusterStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateLoginStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CoalesceExpression node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterServerConfigurationFailoverClusterPropertyOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.FullTextTableReference node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("FullTextFunctionType", node.FullTextFunctionType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AdHocTableReference node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropAssemblyStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("WithNoDependents", node.WithNoDependents));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateColumnStoreIndexStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            if (node.Clustered.HasValue)
                element.Add(new XAttribute("Clustered", node.Clustered));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.OrderByClause node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ParameterlessCall node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ParameterlessCallType", node.ParameterlessCallType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DiskStatementOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.QualifiedJoin node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("QualifiedJoinType", node.QualifiedJoinType));
            element.Add(new XAttribute("JoinHint", node.JoinHint));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.OverClause node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.OdbcQualifiedJoinTableReference node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ResultSetsExecuteOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ResultSetsOptionKind", node.ResultSetsOptionKind));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DiskStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("DiskStatementType", node.DiskStatementType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.JoinParenthesisTableReference node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.PartitionFunctionCall node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.QuerySpecification node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("UniqueRowFilter", node.UniqueRowFilter));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.UseFederationStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Filtering", node.Filtering));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.RightFunctionCall node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.FromClause node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ResultSetDefinition node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ResultSetType", node.ResultSetType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropFederationStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.LeftFunctionCall node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SelectScalarExpression node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SelectStarExpression node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.UserDefinedTypeCallTarget node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.QueryParenthesisExpression node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ScalarSubquery node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.IdentityFunctionCall node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ExecuteOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.BeginEndAtomicBlockStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.GroupByClause node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("GroupByOption", node.GroupByOption));
            element.Add(new XAttribute("All", node.All));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.TemporalClause node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("TemporalClauseType", node.TemporalClauseType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.BeginEndBlockStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ExpressionGroupingSpecification node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("DistributedAggregation", node.DistributedAggregation));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.StatementList node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SelectiveXmlIndexPromotedPath node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsSingleton", node.IsSingleton));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CompositeGroupingSpecification node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterFunctionStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CubeGroupingSpecification node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.RollupGroupingSpecification node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.OdbcConvertSpecification node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.WithinGroupClause node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("HasGraphPath", node.HasGraphPath));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.GrandTotalGroupingSpecification node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ExecuteStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.GroupingSetsGroupingSpecification node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.WindowDelimiter node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("WindowDelimiterType", node.WindowDelimiterType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ExtractFromExpression node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.OutputClause node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.OutputIntoClause node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.OdbcFunctionCall node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ParametersUsed", node.ParametersUsed));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.HavingClause node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.WindowFrameClause node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("WindowFrameType", node.WindowFrameType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterFederationStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Kind", node.Kind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SelectSetVariable node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("AssignmentKind", node.AssignmentKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.InlineResultSetDefinition node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ResultSetType", node.ResultSetType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropPartitionSchemeStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.TryParseCall node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropSynonymStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SecondaryRoleReplicaOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("AllowConnections", node.AllowConnections));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropAggregateStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ParseCall node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ExecuteContext node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Kind", node.Kind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.PrimaryRoleReplicaOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("AllowConnections", node.AllowConnections));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropApplicationRoleStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.TryConvertCall node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropFullTextCatalogStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropFullTextIndexStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ConvertCall node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.FailoverModeReplicaOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Value", node.Value));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropLoginStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ExecuteParameter node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsOutput", node.IsOutput));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropRoleStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AvailabilityModeReplicaOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Value", node.Value));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SchemaDeclarationItemOpenjson node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("AsJson", node.AsJson));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropTypeStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropUserStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SchemaDeclarationItem node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropMasterKeyStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.LiteralReplicaOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropPartitionFunctionStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropSymmetricKeyStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("RemoveProviderKey", node.RemoveProviderKey));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CastCall node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ExecuteSpecification node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.MultiPartIdentifierCallTarget node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateFederationStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ExpressionCallTarget node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DataModificationTableReference node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropAvailabilityGroupStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ChangeTableChangesTableReference node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ResultColumnDefinition node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ChangeTableVersionTableReference node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterAvailabilityGroupFailoverOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.BooleanTernaryExpression node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("TernaryExpressionType", node.TernaryExpressionType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.FunctionCall node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("UniqueRowFilter", node.UniqueRowFilter));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.TopRowFilter node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Percent", node.Percent));
            element.Add(new XAttribute("WithTies", node.WithTies));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterAvailabilityGroupFailoverAction node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ActionType", node.ActionType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.OffsetClause node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SchemaObjectResultSetDefinition node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ResultSetType", node.ResultSetType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AtTimeZoneCall node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterAvailabilityGroupAction node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ActionType", node.ActionType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.UnaryExpression node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("UnaryExpressionType", node.UnaryExpressionType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.BinaryQueryExpression node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("BinaryQueryExpressionType", node.BinaryQueryExpressionType));
            element.Add(new XAttribute("All", node.All));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.TryCastCall node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.VariableTableReference node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.LiteralAvailabilityGroupOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.VariableMethodCallTableReference node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterLoginEnableDisableStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsEnable", node.IsEnable));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.TableHintsOptimizerHint node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("HintKind", node.HintKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterLoginAddDropCredentialStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsAdd", node.IsAdd));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateServerAuditSpecificationStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("AuditState", node.AuditState));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterServerAuditSpecificationStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("AuditState", node.AuditState));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ScalarFunctionReturnType node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropServerAuditSpecificationStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateEventSessionStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("SessionScope", node.SessionScope));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.XmlDataTypeReference node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("XmlDataTypeOption", node.XmlDataTypeOption));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.Identifier node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Value", node.Value));
            element.Add(new XAttribute("QuoteType", node.QuoteType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateServerAuditStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.EventSessionStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("SessionScope", node.SessionScope));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterServerAuditStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("RemoveWhere", node.RemoveWhere));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.UserDataTypeReference node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropServerAuditStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.EventSessionObjectName node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AuditTarget node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("TargetKind", node.TargetKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SqlDataTypeReference node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("SqlDataTypeOption", node.SqlDataTypeOption));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterProcedureStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsForReplication", node.IsForReplication));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.QueueDelayAuditOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropCryptographicProviderStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AuditGuidAuditOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.OnFailureAuditOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OnFailureAction", node.OnFailureAction));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterCryptographicProviderStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Option", node.Option));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.EventDeclaration node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.StateAuditOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Value", node.Value));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SelectFunctionReturnType node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.TableHint node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("HintKind", node.HintKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SensitivityClassificationOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Type", node.Type));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AddSensitivityClassificationStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateTriggerStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("TriggerType", node.TriggerType));
            element.Add(new XAttribute("WithAppend", node.WithAppend));
            element.Add(new XAttribute("IsNotForReplication", node.IsNotForReplication));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SchemaObjectFunctionTableReference node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropSensitivityClassificationStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.TargetDeclaration node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.NamedTableReference node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ForPath", node.ForPath));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AuditSpecificationPart node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsDrop", node.IsDrop));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.EventDeclarationCompareFunctionParameter node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DeclareTableVariableStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateOrAlterTriggerStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("TriggerType", node.TriggerType));
            element.Add(new XAttribute("WithAppend", node.WithAppend));
            element.Add(new XAttribute("IsNotForReplication", node.IsNotForReplication));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AuditActionSpecification node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DatabaseAuditAction node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ActionKind", node.ActionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SourceDeclaration node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DeclareTableVariableBody node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("AsDefined", node.AsDefined));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AuditActionGroupReference node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Group", node.Group));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.EventDeclarationSetParameter node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateDatabaseAuditSpecificationStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("AuditState", node.AuditState));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.TableDefinition node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterDatabaseAuditSpecificationStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("AuditState", node.AuditState));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropDatabaseAuditSpecificationStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateProcedureStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsForReplication", node.IsForReplication));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.TableValuedFunctionReturnType node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterBrokerPriorityStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropResourcePoolStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ExternalResourcePoolStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ExecuteAsFunctionOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ExternalResourcePoolParameter node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ParameterType", node.ParameterType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateBrokerPriorityStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ExternalResourcePoolAffinitySpecification node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("AffinityType", node.AffinityType));
            element.Add(new XAttribute("IsAuto", node.IsAuto));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.InlineFunctionOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionState", node.OptionState));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateExternalResourcePoolStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.BrokerPriorityParameter node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsDefaultOrAny", node.IsDefaultOrAny));
            element.Add(new XAttribute("ParameterType", node.ParameterType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterExternalResourcePoolStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.FunctionOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropExternalResourcePoolStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ExecuteAsProcedureOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.WorkloadGroupResourceParameter node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ParameterType", node.ParameterType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropWorkloadGroupStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.WorkloadGroupImportanceParameter node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ParameterValue", node.ParameterValue));
            element.Add(new XAttribute("ParameterType", node.ParameterType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ProcedureOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterWorkloadGroupStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateWorkloadGroupStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.XmlNamespaces node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterResourcePoolStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.MethodSpecifier node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateResourcePoolStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.MaxSizeAuditTargetOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsUnlimited", node.IsUnlimited));
            element.Add(new XAttribute("Unit", node.Unit));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateCryptographicProviderStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.RetentionDaysAuditTargetOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.MaxRolloverFilesAuditTargetOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsUnlimited", node.IsUnlimited));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropFullTextStopListStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.LiteralAuditTargetOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateOrAlterProcedureStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsForReplication", node.IsForReplication));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.WithCtesAndXmlNamespaces node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.OnOffAuditTargetOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Value", node.Value));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.FullTextStopListAction node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsAdd", node.IsAdd));
            element.Add(new XAttribute("IsAll", node.IsAll));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.EventRetentionSessionOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Value", node.Value));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CommonTableExpression node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterFullTextStopListStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterDatabaseEncryptionKeyStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Regenerate", node.Regenerate));
            element.Add(new XAttribute("Algorithm", node.Algorithm));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.XmlNamespacesAliasElement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ProcedureReference node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropDatabaseEncryptionKeyStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ResourcePoolStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateFullTextStopListStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsSystemStopList", node.IsSystemStopList));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.XmlNamespacesDefaultElement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ResourcePoolParameter node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ParameterType", node.ParameterType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropBrokerPriorityStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ResourcePoolAffinitySpecification node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("AffinityType", node.AffinityType));
            element.Add(new XAttribute("IsAuto", node.IsAuto));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateDatabaseEncryptionKeyStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Algorithm", node.Algorithm));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateTypeTableStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.InsertMergeAction node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ProcessAffinityRange node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.MoveConversationStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ViewDistributionOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.UpdateForClause node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.GetConversationGroupStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterServerConfigurationStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ProcessAffinity", node.ProcessAffinity));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ReceiveStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsConversationGroupIdWhere", node.IsConversationGroupIdWhere));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.JsonForClauseOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SendStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CellsPerObjectSpatialIndexOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.JsonForClause node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterSchemaStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ObjectKind", node.ObjectKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterAsymmetricKeyStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Kind", node.Kind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.GridParameter node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Parameter", node.Parameter));
            element.Add(new XAttribute("Value", node.Value));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.XmlForClauseOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterServiceMasterKeyStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Kind", node.Kind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.GridsSpatialIndexOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.BeginConversationTimerStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.XmlForClause node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.BeginDialogStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsConversation", node.IsConversation));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ViewRoundRobinDistributionPolicy node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ReadOnlyForClause node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.EndConversationStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("WithCleanup", node.WithCleanup));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.BoundingBoxParameter node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Parameter", node.Parameter));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.OptimizerHint node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("HintKind", node.HintKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterServerConfigurationSetExternalAuthenticationStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterServerConfigurationBufferPoolExtensionContainerOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.RevertStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.VariableValuePair node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsForUnknown", node.IsForUnknown));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateOrAlterViewStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("WithCheckOption", node.WithCheckOption));
            element.Add(new XAttribute("IsMaterialized", node.IsMaterialized));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropContractStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterServerConfigurationBufferPoolExtensionOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropEndpointStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.UseHintList node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("HintKind", node.HintKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropMessageTypeStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterServerConfigurationSetBufferPoolExtensionStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropQueueStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.OptimizeForOptimizerHint node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsForUnknown", node.IsForUnknown));
            element.Add(new XAttribute("HintKind", node.HintKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropRemoteServiceBindingStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropRouteStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ForceSeekTableHint node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("HintKind", node.HintKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterServerConfigurationExternalAuthenticationContainerOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropServiceStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterServerConfigurationExternalAuthenticationOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AddSignatureStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsCounter", node.IsCounter));
            element.Add(new XAttribute("ElementKind", node.ElementKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ViewForAppendOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropSignatureStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsCounter", node.IsCounter));
            element.Add(new XAttribute("ElementKind", node.ElementKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.LiteralOptimizerHint node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("HintKind", node.HintKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropEventNotificationStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ExecuteAsStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("WithNoRevert", node.WithNoRevert));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ScalarExpressionDialogOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.OnOffDialogOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionState", node.OptionState));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.BrowseForClause node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.TSqlScript node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterEventSessionStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StatementType", node.StatementType));
            element.Add(new XAttribute("SessionScope", node.SessionScope));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ExecuteAsTriggerOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.TSqlBatch node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ExistsPredicate node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.OnOffSessionOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionState", node.OptionState));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SubqueryComparisonPredicate node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ComparisonType", node.ComparisonType));
            element.Add(new XAttribute("SubqueryComparisonPredicateType", node.SubqueryComparisonPredicateType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.MaxDispatchLatencySessionOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsInfinite", node.IsInfinite));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.TriggerAction node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("TriggerActionType", node.TriggerActionType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.MergeStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.InlineDerivedTable node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.MergeSpecification node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.LiteralSessionOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Unit", node.Unit));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.MergeActionClause node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Condition", node.Condition));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.QueryDerivedTable node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.MemoryPartitionSessionOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Value", node.Value));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.UpdateMergeAction node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.LiteralTableHint node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("HintKind", node.HintKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DeleteMergeAction node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ExpressionWithSortOrder node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("SortOrder", node.SortOrder));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterTriggerStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("TriggerType", node.TriggerType));
            element.Add(new XAttribute("WithAppend", node.WithAppend));
            element.Add(new XAttribute("IsNotForReplication", node.IsNotForReplication));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.LikePredicate node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("NotDefined", node.NotDefined));
            element.Add(new XAttribute("OdbcEscape", node.OdbcEscape));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.IdentifierSnippet node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Script", node.Script));
            element.Add(new XAttribute("Value", node.Value));
            element.Add(new XAttribute("QuoteType", node.QuoteType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.TSqlStatementSnippet node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Script", node.Script));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.InPredicate node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("NotDefined", node.NotDefined));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.BackupCertificateStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ActiveForBeginDialog", node.ActiveForBeginDialog));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.BoundingBoxSpatialIndexOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ViewHashDistributionPolicy node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.BackupServiceMasterKeyStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SpatialIndexRegularOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.RestoreServiceMasterKeyStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsForce", node.IsForce));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SelectStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.BackupMasterKeyStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.RestoreMasterKeyStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsForce", node.IsForce));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.IndexTableHint node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("HintKind", node.HintKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.TriggerObject node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("TriggerScope", node.TriggerScope));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ScalarExpressionSnippet node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Script", node.Script));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateSpatialIndexStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("SpatialIndexingScheme", node.SpatialIndexingScheme));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.BooleanExpressionSnippet node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Script", node.Script));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.UserDefinedTypePropertyAccess node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.StatementListSnippet node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Script", node.Script));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterResourceGovernorStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Command", node.Command));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SelectStatementSnippet node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Script", node.Script));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.FullTextPredicate node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("FullTextFunctionType", node.FullTextFunctionType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SchemaObjectNameSnippet node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Script", node.Script));
            element.Add(new XAttribute("Count", node.Count));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.TriggerOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.TSqlFragmentSnippet node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Script", node.Script));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropEventSessionStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("SessionScope", node.SessionScope));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ProcedureParameter node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsVarying", node.IsVarying));
            element.Add(new XAttribute("Modifier", node.Modifier));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropStatisticsStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ExternalTableDistributionOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropTableStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropProcedureStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ExternalTableLiteralOrIdentifierOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropFunctionStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropViewStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropDefaultStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropRuleStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropTriggerStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("TriggerScope", node.TriggerScope));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropSchemaStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("DropBehavior", node.DropBehavior));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.EncryptedValueParameter node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ParameterKind", node.ParameterKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.RaiseErrorLegacyStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.RaiseErrorStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("RaiseErrorOptions", node.RaiseErrorOptions));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ColumnEncryptionAlgorithmNameParameter node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ParameterKind", node.ParameterKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.TruncateTableStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SetUserStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("WithNoReset", node.WithNoReset));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropColumnEncryptionKeyStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ShutdownStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("WithNoWait", node.WithNoWait));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ReconfigureStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("WithOverride", node.WithOverride));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ColumnEncryptionKeyValue node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.FileStreamOnDropIndexOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CheckpointStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.KillQueryNotificationSubscriptionStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("All", node.All));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.KillStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("WithStatusOnly", node.WithStatusOnly));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ColumnMasterKeyNameParameter node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ParameterKind", node.ParameterKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.UseStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ThrowStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.KillStatsJobStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterColumnEncryptionKeyStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("AlterType", node.AlterType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ExternalTableRejectTypeOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Value", node.Value));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropIndexClause node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterExternalDataSourceStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("DataSourceType", node.DataSourceType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CursorId node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsGlobal", node.IsGlobal));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateExternalDataSourceStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("DataSourceType", node.DataSourceType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.OpenCursorStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CloseCursorStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ExternalDataSourceLiteralOrIdentifierOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CryptoMechanism node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("CryptoMechanismType", node.CryptoMechanismType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.OpenSymmetricKeyStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CloseSymmetricKeyStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("All", node.All));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.OpenMasterKeyStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CloseMasterKeyStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DeallocateCursorStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropExternalTableStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.FetchType node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Orientation", node.Orientation));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.BackwardsCompatibleDropIndexClause node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ExternalTableReplicatedDistributionPolicy node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropIndexStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.MoveToDropIndexOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ExternalTableRoundRobinDistributionPolicy node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ExternalTableShardedDistributionPolicy node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.WhereClause node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateExternalTableStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.FetchCursorStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropDatabaseStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SetVariableStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("SeparatorType", node.SeparatorType));
            element.Add(new XAttribute("FunctionCallExists", node.FunctionCallExists));
            element.Add(new XAttribute("AssignmentKind", node.AssignmentKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateColumnEncryptionKeyStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterDatabaseScopedConfigurationClearStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Secondary", node.Secondary));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DatabaseConfigurationClearOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SecurityPredicateAction node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ActionType", node.ActionType));
            element.Add(new XAttribute("SecurityPredicateType", node.SecurityPredicateType));
            element.Add(new XAttribute("SecurityPredicateOperation", node.SecurityPredicateOperation));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DatabaseConfigurationSetOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.OnOffPrimaryConfigurationOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionState", node.OptionState));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.MaxDopConfigurationOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Primary", node.Primary));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.GenericConfigurationOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropSequenceStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterDatabaseCollateStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("UseCurrent", node.UseCurrent));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterDatabaseRebuildLogStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("UseCurrent", node.UseCurrent));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterSequenceStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterDatabaseAddFileStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsLog", node.IsLog));
            element.Add(new XAttribute("UseCurrent", node.UseCurrent));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterDatabaseAddFileGroupStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ContainsFileStream", node.ContainsFileStream));
            element.Add(new XAttribute("ContainsMemoryOptimizedData", node.ContainsMemoryOptimizedData));
            element.Add(new XAttribute("UseCurrent", node.UseCurrent));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateSequenceStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterDatabaseRemoveFileGroupStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("UseCurrent", node.UseCurrent));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterDatabaseRemoveFileStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("UseCurrent", node.UseCurrent));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ContainmentDatabaseOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Value", node.Value));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.NextValueForExpression node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AutoCreateStatisticsDatabaseOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("HasIncremental", node.HasIncremental));
            element.Add(new XAttribute("IncrementalState", node.IncrementalState));
            element.Add(new XAttribute("OptionState", node.OptionState));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.OnOffDatabaseOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionState", node.OptionState));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DatabaseOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CompressionDelayIndexOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("TimeUnit", node.TimeUnit));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterDatabaseSetStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("UseCurrent", node.UseCurrent));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterDatabaseTermination node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ImmediateRollback", node.ImmediateRollback));
            element.Add(new XAttribute("NoWait", node.NoWait));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterDatabaseModifyFileGroupStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("MakeDefault", node.MakeDefault));
            element.Add(new XAttribute("UpdatabilityOption", node.UpdatabilityOption));
            element.Add(new XAttribute("UseCurrent", node.UseCurrent));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DataTypeSequenceOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("NoValue", node.NoValue));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterDatabaseModifyFileStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("UseCurrent", node.UseCurrent));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterDatabaseModifyNameStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("UseCurrent", node.UseCurrent));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ScalarExpressionSequenceOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("NoValue", node.NoValue));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SequenceOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("NoValue", node.NoValue));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.PredicateSetStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Options", node.Options));
            element.Add(new XAttribute("IsOn", node.IsOn));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterDatabaseScopedConfigurationSetStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Secondary", node.Secondary));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SetStatisticsStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Options", node.Options));
            element.Add(new XAttribute("IsOn", node.IsOn));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SetRowCountStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SetOffsetsStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Options", node.Options));
            element.Add(new XAttribute("IsOn", node.IsOn));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropColumnMasterKeyStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.GeneralSetCommand node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("CommandType", node.CommandType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SetFipsFlaggerCommand node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ComplianceLevel", node.ComplianceLevel));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ColumnMasterKeyEnclaveComputationsParameter node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ParameterKind", node.ParameterKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SetCommandStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SetTransactionIsolationLevelStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Level", node.Level));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ColumnMasterKeyPathParameter node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ParameterKind", node.ParameterKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SetTextSizeStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SetIdentityInsertStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsOn", node.IsOn));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ColumnMasterKeyStoreProviderNameParameter node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ParameterKind", node.ParameterKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SetErrorLevelStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateDatabaseStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("AttachMode", node.AttachMode));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateSecurityPolicyStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("NotForReplication", node.NotForReplication));
            element.Add(new XAttribute("ActionType", node.ActionType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.FileGroupDefinition node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsDefault", node.IsDefault));
            element.Add(new XAttribute("ContainsFileStream", node.ContainsFileStream));
            element.Add(new XAttribute("ContainsMemoryOptimizedData", node.ContainsMemoryOptimizedData));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.FileGrowthFileDeclarationOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Units", node.Units));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterSecurityPolicyStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("NotForReplication", node.NotForReplication));
            element.Add(new XAttribute("ActionType", node.ActionType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.MaxSizeFileDeclarationOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Units", node.Units));
            element.Add(new XAttribute("Unlimited", node.Unlimited));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SecurityPolicyOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("OptionState", node.OptionState));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SizeFileDeclarationOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Units", node.Units));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.FileNameFileDeclarationOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.NameFileDeclarationOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsNewName", node.IsNewName));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateColumnMasterKeyStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.FileDeclarationOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.FileDeclaration node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsPrimary", node.IsPrimary));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropSecurityPolicyStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CursorOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropExternalDataSourceStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CursorDefinition node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateRouteStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.RemoteDataArchiveAlterTableOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("RdaTableOption", node.RdaTableOption));
            element.Add(new XAttribute("MigrationState", node.MigrationState));
            element.Add(new XAttribute("IsMigrationStateSpecified", node.IsMigrationStateSpecified));
            element.Add(new XAttribute("IsFilterPredicateSpecified", node.IsFilterPredicateSpecified));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterQueueStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.RemoteDataArchiveTableOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("RdaTableOption", node.RdaTableOption));
            element.Add(new XAttribute("MigrationState", node.MigrationState));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateQueueStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.IndexDefinition node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Unique", node.Unique));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DurabilityTableOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("DurabilityTableOptionKind", node.DurabilityTableOptionKind));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SystemTimePeriodDefinition node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.MemoryOptimizedTableOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionState", node.OptionState));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.IndexType node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            if (node.IndexTypeKind.HasValue)
                element.Add(new XAttribute("IndexTypeKind", node.IndexTypeKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.PartitionSpecifier node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("All", node.All));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.FileTableConstraintNameTableOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterIndexStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("All", node.All));
            element.Add(new XAttribute("AlterIndexType", node.AlterIndexType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateXmlIndexStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Primary", node.Primary));
            element.Add(new XAttribute("SecondaryXmlIndexType", node.SecondaryXmlIndexType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.FileTableCollateFileNameTableOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.IgnoreDupKeyIndexOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            if (node.SuppressMessagesOption.HasValue)
                element.Add(new XAttribute("SuppressMessagesOption", node.SuppressMessagesOption));
            element.Add(new XAttribute("OptionState", node.OptionState));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.OnlineIndexOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionState", node.OptionState));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.WaitAtLowPriorityOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.MaxDurationOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            if (node.Unit.HasValue)
                element.Add(new XAttribute("Unit", node.Unit));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.LockEscalationTableOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Value", node.Value));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterRouteStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.IndexExpressionOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.FileStreamOnTableOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateIndexStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Translated80SyntaxTo90", node.Translated80SyntaxTo90));
            element.Add(new XAttribute("Unique", node.Unique));
            if (node.Clustered.HasValue)
                element.Add(new XAttribute("Clustered", node.Clustered));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.FileTableDirectoryTableOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.FileGroupOrPartitionScheme node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateSelectiveXmlIndexStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsSecondary", node.IsSecondary));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.IndexStateOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionState", node.OptionState));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterTableSetStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.RemoteDataArchiveDatabaseOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionState", node.OptionState));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.RouteOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.LowPriorityLockWaitTableSwitchOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropClusteredConstraintStateOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionState", node.OptionState));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropClusteredConstraintValueOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropClusteredConstraintMoveOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterTableSwitchStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropClusteredConstraintWaitAtLowPriorityLockOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterTableDropTableElement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("TableElementType", node.TableElementType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterTableConstraintModificationStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ExistingRowsCheckEnforcement", node.ExistingRowsCheckEnforcement));
            element.Add(new XAttribute("ConstraintEnforcement", node.ConstraintEnforcement));
            element.Add(new XAttribute("All", node.All));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterTableDropTableElementStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterTableTriggerModificationStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("TriggerEnforcement", node.TriggerEnforcement));
            element.Add(new XAttribute("All", node.All));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterTableAddTableElementStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ExistingRowsCheckEnforcement", node.ExistingRowsCheckEnforcement));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.EnableDisableTriggerStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("TriggerEnforcement", node.TriggerEnforcement));
            element.Add(new XAttribute("All", node.All));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.TryCatchStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SystemVersioningTableOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionState", node.OptionState));
            element.Add(new XAttribute("ConsistencyCheckEnabled", node.ConsistencyCheckEnabled));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.QueueExecuteAsOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.QueueValueOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.RemoteDataArchiveDbServerSetting node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("SettingKind", node.SettingKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.QueueProcedureOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.QueueStateOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionState", node.OptionState));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.RemoteDataArchiveDbCredentialSetting node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("SettingKind", node.SettingKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ExecuteAsClause node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ExecuteAsOption", node.ExecuteAsOption));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.RemoteDataArchiveDbFederatedServiceAccountSetting node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsOn", node.IsOn));
            element.Add(new XAttribute("SettingKind", node.SettingKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateSynonymStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateTypeUddtStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.RetentionPeriodDefinition node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Units", node.Units));
            element.Add(new XAttribute("IsInfinity", node.IsInfinity));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateTypeUdtStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.QueueOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.OrderIndexOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.OnlineIndexLowPriorityLockWaitOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterTableFileTableNamespaceStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsEnable", node.IsEnable));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterRoleStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterAssemblyStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsDropAll", node.IsDropAll));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.RenameAlterRoleAction node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AddMemberAlterRoleAction node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateAssemblyStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropMemberAlterRoleAction node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateServerRoleStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterServerRoleStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropServerRoleStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropExternalFileFormatStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.UserLoginOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("UserLoginOptionType", node.UserLoginOptionType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateExternalFileFormatStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("FormatType", node.FormatType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateUserStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterUserStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DeclareCursorStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ReturnStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.UpdateStatisticsStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateStatisticsStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AssemblyOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.LiteralStatisticsOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.OnOffStatisticsOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionState", node.OptionState));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.StatisticsPartitionRange node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ExternalFileFormatUseDefaultTypeOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ExternalFileFormatUseDefaultType", node.ExternalFileFormatUseDefaultType));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ResampleStatisticsOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.StatisticsOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ExternalFileFormatContainerOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ExternalFileFormatLiteralOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateRoleStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.OnOffAssemblyOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionState", node.OptionState));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.FullTextCatalogAndFileGroup node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("FileGroupIsFirst", node.FileGroupIsFirst));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SearchPropertyListFullTextIndexOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsOff", node.IsOff));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.StopListFullTextIndexOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsOff", node.IsOff));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ChangeTrackingFullTextIndexOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Value", node.Value));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterTableAlterPartitionStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsSplit", node.IsSplit));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AssemblyName node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterTableRebuildStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.FullTextIndexColumn node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StatisticalSemantics", node.StatisticalSemantics));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.LowPriorityLockWaitAbortAfterWaitOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("AbortAfterWait", node.AbortAfterWait));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterTableChangeTrackingModificationStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsEnable", node.IsEnable));
            element.Add(new XAttribute("TrackColumnsUpdated", node.TrackColumnsUpdated));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.LowPriorityLockWaitMaxDurationOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            if (node.Unit.HasValue)
                element.Add(new XAttribute("Unit", node.Unit));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateFullTextIndexStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.HadrDatabaseOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("HadrOption", node.HadrOption));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DropXmlSchemaCollectionStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterApplicationRoleStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateApplicationRoleStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.PermissionSetAssemblyOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("PermissionSetOption", node.PermissionSetOption));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ApplicationRoleOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AddFileSpec node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.EventTypeContainer node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("EventType", node.EventType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterMasterKeyStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Option", node.Option));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateXmlSchemaCollectionStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.EventNotificationObjectScope node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Target", node.Target));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterXmlSchemaCollectionStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateEventNotificationStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("WithFanIn", node.WithFanIn));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.EventGroupContainer node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("EventGroup", node.EventGroup));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateMasterKeyStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ColumnReferenceExpression node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ColumnType", node.ColumnType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.HadrAvailabilityGroupDatabaseOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("HadrOption", node.HadrOption));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DelayedDurabilityDatabaseOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Value", node.Value));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateCredentialStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsDatabaseScoped", node.IsDatabaseScoped));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.WaitForStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("WaitForOption", node.WaitForOption));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterCredentialStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsDatabaseScoped", node.IsDatabaseScoped));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateSchemaStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateMessageTypeStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ValidationMethod", node.ValidationMethod));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterMessageTypeStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ValidationMethod", node.ValidationMethod));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.UpdateSpecification node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateAggregateStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateEndpointStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("State", node.State));
            element.Add(new XAttribute("Protocol", node.Protocol));
            element.Add(new XAttribute("EndpointType", node.EndpointType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.UpdateStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterEndpointStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("State", node.State));
            element.Add(new XAttribute("Protocol", node.Protocol));
            element.Add(new XAttribute("EndpointType", node.EndpointType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.InsertSpecification node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("InsertOption", node.InsertOption));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.EndpointAffinity node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Kind", node.Kind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.InsertStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.WsdlPayloadOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsNone", node.IsNone));
            element.Add(new XAttribute("Kind", node.Kind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.EnabledDisabledPayloadOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsEnabled", node.IsEnabled));
            element.Add(new XAttribute("Kind", node.Kind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.WhileStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SoapMethod node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Action", node.Action));
            element.Add(new XAttribute("Format", node.Format));
            element.Add(new XAttribute("Schema", node.Schema));
            element.Add(new XAttribute("Kind", node.Kind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DeleteStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.IPv4 node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CompressionEndpointProtocolOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsEnabled", node.IsEnabled));
            element.Add(new XAttribute("Kind", node.Kind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.PortsEndpointProtocolOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("PortTypes", node.PortTypes));
            element.Add(new XAttribute("Kind", node.Kind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DeleteSpecification node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AuthenticationEndpointProtocolOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("AuthenticationTypes", node.AuthenticationTypes));
            element.Add(new XAttribute("Kind", node.Kind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.LiteralEndpointProtocolOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Kind", node.Kind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ListenerIPEndpointProtocolOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsAll", node.IsAll));
            element.Add(new XAttribute("Kind", node.Kind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ReadTextStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("HoldLock", node.HoldLock));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateContractStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SecurityTargetObject node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ObjectKind", node.ObjectKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DbccOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DbccNamedLiteral node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Name", node.Name));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.Permission node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateAsymmetricKeyStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("EncryptionAlgorithm", node.EncryptionAlgorithm));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreatePartitionFunctionStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Range", node.Range));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterAuthorizationStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ToSchemaOwner", node.ToSchemaOwner));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.PartitionParameterType node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreatePartitionSchemeStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsAll", node.IsAll));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.RevokeStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("GrantOptionFor", node.GrantOptionFor));
            element.Add(new XAttribute("CascadeOption", node.CascadeOption));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DenyStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("CascadeOption", node.CascadeOption));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.OnOffRemoteServiceBindingOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionState", node.OptionState));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.UserRemoteServiceBindingOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.GrantStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("WithGrantOption", node.WithGrantOption));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateRemoteServiceBindingStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.UpdateTextStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Bulk", node.Bulk));
            element.Add(new XAttribute("WithLog", node.WithLog));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CertificateOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Kind", node.Kind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateCertificateStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ActiveForBeginDialog", node.ActiveForBeginDialog));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.WriteTextStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Bulk", node.Bulk));
            element.Add(new XAttribute("WithLog", node.WithLog));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterCertificateStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Kind", node.Kind));
            element.Add(new XAttribute("ActiveForBeginDialog", node.ActiveForBeginDialog));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ContractMessage node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("SentBy", node.SentBy));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.FileEncryptionSource node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsExecutable", node.IsExecutable));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.LineNoStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AssemblyEncryptionSource node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterRemoteServiceBindingStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ProviderEncryptionSource node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.LoginTypePayloadOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsWindows", node.IsWindows));
            element.Add(new XAttribute("Kind", node.Kind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.LiteralPayloadOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Kind", node.Kind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SessionTimeoutPayloadOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsNever", node.IsNever));
            element.Add(new XAttribute("Kind", node.Kind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.BuiltInFunctionTableReference node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ContinueStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.GlobalFunctionTableReference node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ComputeClause node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SaveTransactionStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ComputeFunction node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ComputeFunctionType", node.ComputeFunctionType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.PivotedTableReference node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.RollbackTransactionStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.UnpivotedTableReference node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.UnqualifiedJoin node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("UnqualifiedJoinType", node.UnqualifiedJoinType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CommitTransactionStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("DelayedDurabilityOption", node.DelayedDurabilityOption));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.TableSampleClause node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("System", node.System));
            element.Add(new XAttribute("TableSampleClauseOption", node.TableSampleClauseOption));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ColumnWithSortOrder node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("SortOrder", node.SortOrder));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.BooleanNotExpression node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.BreakStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.GraphMatchCompositeExpression node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ArrowOnRight", node.ArrowOnRight));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.GraphMatchExpression node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ArrowOnRight", node.ArrowOnRight));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.LiteralAtomicBlockOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.GraphMatchRecursivePredicate node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Function", node.Function));
            element.Add(new XAttribute("AnchorOnLeft", node.AnchorOnLeft));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.GraphMatchNodeExpression node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("UsesLastNode", node.UsesLastNode));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.IdentifierAtomicBlockOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.BinaryExpression node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("BinaryExpressionType", node.BinaryExpressionType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.GraphMatchLastNodePredicate node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.OnOffAtomicBlockOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionState", node.OptionState));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.BooleanIsNullExpression node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsNot", node.IsNot));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.BooleanBinaryExpression node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("BinaryExpressionType", node.BinaryExpressionType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.BeginTransactionStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Distributed", node.Distributed));
            element.Add(new XAttribute("MarkDefined", node.MarkDefined));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.BooleanComparisonExpression node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ComparisonType", node.ComparisonType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.BooleanParenthesisExpression node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.GraphMatchPredicate node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateDefaultStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ServiceContract node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Action", node.Action));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterServiceStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.IfStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.KeySourceKeyOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.LabelStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Value", node.Value));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateSymmetricKeyStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlgorithmKeyOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Algorithm", node.Algorithm));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.MultiPartIdentifier node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Count", node.Count));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AuthenticationPayloadOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Protocol", node.Protocol));
            element.Add(new XAttribute("TryCertificateFirst", node.TryCertificateFirst));
            element.Add(new XAttribute("Kind", node.Kind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SchemaObjectName node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Count", node.Count));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.RolePayloadOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Role", node.Role));
            element.Add(new XAttribute("Kind", node.Kind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CharacterSetPayloadOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsSql", node.IsSql));
            element.Add(new XAttribute("Kind", node.Kind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ChildObjectName node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Count", node.Count));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SchemaPayloadOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsStandard", node.IsStandard));
            element.Add(new XAttribute("Kind", node.Kind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.EncryptionPayloadOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("EncryptionSupport", node.EncryptionSupport));
            element.Add(new XAttribute("AlgorithmPartOne", node.AlgorithmPartOne));
            element.Add(new XAttribute("AlgorithmPartTwo", node.AlgorithmPartTwo));
            element.Add(new XAttribute("Kind", node.Kind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DbccStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("DllName", node.DllName));
            element.Add(new XAttribute("Command", node.Command));
            element.Add(new XAttribute("ParenthesisRequired", node.ParenthesisRequired));
            element.Add(new XAttribute("OptionsUseJoin", node.OptionsUseJoin));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.IdentityValueKeyOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ProviderKeyNameKeyOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateFunctionStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateServiceStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateOrAlterFunctionStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterFullTextCatalogStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Action", node.Action));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateFullTextCatalogStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsDefault", node.IsDefault));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.GoToStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateRuleStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DeclareVariableElement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterSymmetricKeyStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsAdd", node.IsAdd));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DeclareVariableStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreationDispositionKeyOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsCreateNew", node.IsCreateNew));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.OnOffFullTextCatalogOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionState", node.OptionState));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.GraphRecursiveMatchQuantifier node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsPlusSign", node.IsPlusSign));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.InsertBulkColumnDefinition node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("NullNotNull", node.NullNotNull));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ExternalTableColumnDefinition node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.QueryStoreTimeCleanupPolicyOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.MaxLiteral node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("LiteralType", node.LiteralType));
            element.Add(new XAttribute("Value", node.Value));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AutomaticTuningDatabaseOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("AutomaticTuningState", node.AutomaticTuningState));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AutomaticTuningOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("Value", node.Value));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DefaultLiteral node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("LiteralType", node.LiteralType));
            element.Add(new XAttribute("Value", node.Value));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AutomaticTuningForceLastGoodPlanOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("Value", node.Value));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AutomaticTuningCreateIndexOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("Value", node.Value));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.IdentifierLiteral node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("LiteralType", node.LiteralType));
            element.Add(new XAttribute("QuoteType", node.QuoteType));
            element.Add(new XAttribute("Value", node.Value));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AutomaticTuningDropIndexOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("Value", node.Value));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AutomaticTuningMaintainIndexOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("Value", node.Value));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.NullLiteral node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("LiteralType", node.LiteralType));
            element.Add(new XAttribute("Value", node.Value));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.FileStreamDatabaseOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            if (node.NonTransactedAccess.HasValue)
                element.Add(new XAttribute("NonTransactedAccess", node.NonTransactedAccess));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CatalogCollationOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            if (node.CatalogCollation.HasValue)
                element.Add(new XAttribute("CatalogCollation", node.CatalogCollation));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.StringLiteral node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("LiteralType", node.LiteralType));
            element.Add(new XAttribute("IsNational", node.IsNational));
            element.Add(new XAttribute("IsLargeObject", node.IsLargeObject));
            element.Add(new XAttribute("Value", node.Value));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.MaxSizeDatabaseOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Units", node.Units));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterTableAlterIndexStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("AlterIndexType", node.AlterIndexType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.BinaryLiteral node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("LiteralType", node.LiteralType));
            element.Add(new XAttribute("IsLargeObject", node.IsLargeObject));
            element.Add(new XAttribute("Value", node.Value));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ColumnStorageOptions node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsFileStream", node.IsFileStream));
            element.Add(new XAttribute("SparseOption", node.SparseOption));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.IntegerLiteral node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("LiteralType", node.LiteralType));
            element.Add(new XAttribute("Value", node.Value));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.IdentityOptions node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsIdentityNotForReplication", node.IsIdentityNotForReplication));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ColumnEncryptionAlgorithmParameter node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ParameterKind", node.ParameterKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.NumericLiteral node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("LiteralType", node.LiteralType));
            element.Add(new XAttribute("Value", node.Value));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.QueryStoreMaxPlansPerQueryOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ColumnEncryptionTypeParameter node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("EncryptionType", node.EncryptionType));
            element.Add(new XAttribute("ParameterKind", node.ParameterKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.RealLiteral node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("LiteralType", node.LiteralType));
            element.Add(new XAttribute("Value", node.Value));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ColumnEncryptionDefinition node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.MoneyLiteral node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("LiteralType", node.LiteralType));
            element.Add(new XAttribute("Value", node.Value));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ColumnDefinition node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsPersisted", node.IsPersisted));
            element.Add(new XAttribute("IsRowGuidCol", node.IsRowGuidCol));
            if (node.GeneratedAlways.HasValue)
                element.Add(new XAttribute("GeneratedAlways", node.GeneratedAlways));
            element.Add(new XAttribute("IsHidden", node.IsHidden));
            element.Add(new XAttribute("IsMasked", node.IsMasked));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AlterTableAlterColumnStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("AlterTableAlterColumnOption", node.AlterTableAlterColumnOption));
            if (node.GeneratedAlways.HasValue)
                element.Add(new XAttribute("GeneratedAlways", node.GeneratedAlways));
            element.Add(new XAttribute("IsHidden", node.IsHidden));
            element.Add(new XAttribute("IsMasked", node.IsMasked));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ColumnEncryptionKeyNameParameter node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ParameterKind", node.ParameterKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.OdbcLiteral node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("LiteralType", node.LiteralType));
            element.Add(new XAttribute("OdbcLiteralType", node.OdbcLiteralType));
            element.Add(new XAttribute("IsNational", node.IsNational));
            element.Add(new XAttribute("Value", node.Value));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.QueryStoreIntervalLengthOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ParenthesisExpression node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CursorDefaultDatabaseOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsLocal", node.IsLocal));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.RecoveryDatabaseOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Value", node.Value));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SchemaObjectNameOrValueExpression node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.TargetRecoveryTimeDatabaseOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Unit", node.Unit));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.PageVerifyDatabaseOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Value", node.Value));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.IdentifierOrScalarExpression node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.PartnerDatabaseOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("PartnerOption", node.PartnerOption));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.WitnessDatabaseOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsOff", node.IsOff));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.IdentifierOrValueExpression node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Value", node.Value));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ParameterizationDatabaseOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsSimple", node.IsSimple));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.LiteralDatabaseOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.GlobalVariableExpression node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Name", node.Name));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.IdentifierDatabaseOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ChangeTrackingDatabaseOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionState", node.OptionState));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.LiteralOptionValue node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.LiteralRange node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.QueryStoreDataFlushIntervalOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.QueryStoreSizeCleanupPolicyOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Value", node.Value));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.QueryStoreCapturePolicyOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Value", node.Value));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.QueryStoreDesiredStateOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Value", node.Value));
            element.Add(new XAttribute("OperationModeSpecified", node.OperationModeSpecified));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.QueryStoreMaxStorageSizeOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.VariableReference node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Name", node.Name));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.QueryStoreDatabaseOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Clear", node.Clear));
            element.Add(new XAttribute("ClearAll", node.ClearAll));
            element.Add(new XAttribute("OptionState", node.OptionState));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AcceleratedDatabaseRecoveryDatabaseOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionState", node.OptionState));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ChangeRetentionChangeTrackingOptionDetail node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Unit", node.Unit));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.OnOffOptionValue node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionState", node.OptionState));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AutoCleanupChangeTrackingOptionDetail node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsOn", node.IsOn));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CreateTableStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("AsEdge", node.AsEdge));
            element.Add(new XAttribute("AsFileTable", node.AsFileTable));
            element.Add(new XAttribute("AsNode", node.AsNode));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.FederationScheme node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.BackupDatabaseStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SecurityUserClause80 node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("UserType80", node.UserType80));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.BackupTransactionLogStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.RestoreStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Kind", node.Kind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.Privilege80 node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("PrivilegeType80", node.PrivilegeType80));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.RestoreOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ScalarExpressionRestoreOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.PrivilegeSecurityElement80 node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.MoveRestoreOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.StopRestoreOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsStopAt", node.IsStopAt));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CommandSecurityElement80 node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("All", node.All));
            element.Add(new XAttribute("CommandOptions", node.CommandOptions));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.FileStreamRestoreOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.BackupOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.BackupEncryptionOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Algorithm", node.Algorithm));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DeviceInfo node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("DeviceType", node.DeviceType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ColumnDefinitionBase node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SecurityPrincipal node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("PrincipalType", node.PrincipalType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.OrderBulkInsertOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsUnique", node.IsUnique));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.LiteralBulkInsertOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.BulkInsertOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SqlCommandIdentifier node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Value", node.Value));
            element.Add(new XAttribute("QuoteType", node.QuoteType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.InsertBulkStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.BulkInsertStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DenyStatement80 node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("CascadeOption", node.CascadeOption));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.BackupRestoreFileInfo node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ItemKind", node.ItemKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.MirrorToClause node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.RevokeStatement80 node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("GrantOptionFor", node.GrantOptionFor));
            element.Add(new XAttribute("CascadeOption", node.CascadeOption));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.GrantStatement80 node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("WithGrantOption", node.WithGrantOption));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.UniqueConstraintDefinition node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            if (node.Clustered.HasValue)
                element.Add(new XAttribute("Clustered", node.Clustered));
            element.Add(new XAttribute("IsPrimaryKey", node.IsPrimaryKey));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.GraphConnectionConstraintDefinition node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("DeleteAction", node.DeleteAction));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.TableNonClusteredIndexType node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.TableClusteredIndexType node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("ColumnStore", node.ColumnStore));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.RowValue node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.TableIndexOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.PrintStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ExecuteInsertSource node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.TableHashDistributionPolicy node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.UpdateCall node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.TableReplicateDistributionPolicy node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.TSEqualCall node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.TableDistributionOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.TableDataCompressionOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.TableRoundRobinDistributionPolicy node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SecurityTargetObjectName node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.TablePartitionOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.SelectInsertSource node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.GraphConnectionBetweenNodes node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.NullableConstraintDefinition node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Nullable", node.Nullable));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.AssignmentSetClause node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("AssignmentKind", node.AssignmentKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ForeignKeyConstraintDefinition node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("DeleteAction", node.DeleteAction));
            element.Add(new XAttribute("UpdateAction", node.UpdateAction));
            element.Add(new XAttribute("NotForReplication", node.NotForReplication));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DefaultConstraintDefinition node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("WithValues", node.WithValues));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.FunctionCallSetClause node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CheckConstraintDefinition node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("NotForReplication", node.NotForReplication));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.DataCompressionOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("CompressionLevel", node.CompressionLevel));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.RenameEntityStatement node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("RenameEntityType", node.RenameEntityType));
            if (node.SeparatorType.HasValue)
                element.Add(new XAttribute("SeparatorType", node.SeparatorType));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.ValuesInsertSource node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("IsDefaultValues", node.IsDefaultValues));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.LocationOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.TablePartitionOptionSpecifications node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("Range", node.Range));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.CompressionPartitionRange node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }
        public override void ExplicitVisit(Microsoft.SqlServer.TransactSql.ScriptDom.TruncateTargetTableSwitchOption node)
        {
            var element = CreateElement(node);
            this.stack.Peek().Add(element);
            element.Add(new XAttribute("TruncateTarget", node.TruncateTarget));
            element.Add(new XAttribute("OptionKind", node.OptionKind));
            element.Add(new XAttribute("StartLine", node.StartLine));
            element.Add(new XAttribute("StartColumn", node.StartColumn));
            this.stack.Push(element);
            base.ExplicitVisit(node);
            this.stack.Pop();
        }

    }
}

