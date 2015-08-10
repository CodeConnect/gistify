using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeConnect.Gistify.Extension.CodeAnalysis
{
    public class DiscoveryWalker : CSharpSyntaxWalker
    {
        SemanticModel _model;
        int _start, _end;

        /// <summary>
        /// Contains information of objects defined outside of the provided scope
        /// </summary>
        public List<ObjectInformation> DefinedOutside { get; private set; }

        public DiscoveryWalker(int start, int end, SemanticModel model)
        {
            _model = model;
            _start = start;
            _end = end;
            DefinedOutside = new List<ObjectInformation>();
        }

        public override void VisitFieldDeclaration(FieldDeclarationSyntax node)
        {
            base.VisitFieldDeclaration(node);
            foreach (var variable in node.Declaration.Variables)
            {
                var name = variable.Identifier.ToString();
                var pos = variable.Span.Start;
                var end = variable.Span.End;
                var symbol = _model.GetSymbolInfo(node).Symbol;
                var objectInfo = new ObjectInformation()
                {
                    Identifier = variable.Identifier.ToString(),
                    // TODO: fill the rest
                };
                DefinedOutside.Add(objectInfo);
            }
            var t = DefinedOutside;
        }

        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            base.VisitIdentifierName(node);
            if (node.Span.Start >= _start && node.Span.End <= _end)
            {
                // Process only nodes in the target range
                ProcessIdentifierName(node);
            }
        }

        private void ProcessIdentifierName(IdentifierNameSyntax node)
        {
            var symbol = _model.GetSymbolInfo(node).Symbol;
            if (symbol != null)
            {
                var t1 = symbol.ContainingType;
                var t2 = symbol.ContainingAssembly;
                var t3 = symbol.ContainingNamespace;
                var t4 = symbol.OriginalDefinition;
                var t = _model.GetTypeInfo(node).Type;

                foreach (var location in symbol.OriginalDefinition.Locations)
                {
                    // Process only nodes defined outside of the target range
                    if (location.IsInSource && location.SourceSpan.Start < _start && location.SourceSpan.End > _end)
                    {
                        var objectInfo = new ObjectInformation()
                        {
                            Identifier = node.Identifier.ToString(),
                            FullTypeName = _model.GetTypeInfo(node).Type.ToString(),
                            Namespace = symbol.ContainingNamespace.ToString(),
                            AssemblyName = symbol.ContainingAssembly.ToString()
                        };
                    }
                    
                }
            }
        }

        public override void VisitAssignmentExpression(AssignmentExpressionSyntax node)
        {
            base.VisitAssignmentExpression(node);

            // Analyze both sides:
            var rightSyntax = node.Right;
            var rightInfo = _model.GetSymbolInfo(rightSyntax).Symbol;
            var leftSyntax = node.Left;
            var leftInfo = _model.GetSymbolInfo(leftSyntax).Symbol;

            if (rightInfo != null)
            {
                var t1 = rightInfo.ContainingType;
                var t2 = rightInfo.ContainingAssembly;
                var t3 = rightInfo.ContainingNamespace;
            }
            if (leftInfo != null)
            {
                var t1 = leftInfo.ContainingType;
                var t2 = leftInfo.ContainingAssembly;
                var t3 = leftInfo.ContainingNamespace;
            }
        }

        public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            base.VisitPropertyDeclaration(node);

            var objectInfo = new ObjectInformation()
            {
                Identifier = node.Identifier.ToString(),
                // TODO: fill the rest
            };
            DefinedOutside.Add(objectInfo);
        }
    }
}