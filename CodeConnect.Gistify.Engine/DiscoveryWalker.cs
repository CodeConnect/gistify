using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeConnect.Gistify.Engine
{
    internal class DiscoveryWalker : CSharpSyntaxWalker
    {
        SemanticModel _model;
        int _start, _end;

        internal static IEnumerable<ObjectInformation> FindDeclarations(SyntaxTree tree, SemanticModel model, int startPosition, int endPosition)
        /// <summary>
        /// Finds elements used by the code in specified region of provided syntax.
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="model"></param>
        /// <param name="startPosition"></param>
        /// <param name="endPosition"></param>
        /// <returns></returns>
        {
            var walker = new DiscoveryWalker(startPosition, endPosition, model);
            walker.Visit(tree.GetRoot());
            return walker.definedWithinSnippet;
        }

        /// <summary>
        /// Contains information of objects defined outside of the provided scope
        /// </summary>
        private List<ObjectInformation> definedWithinSnippet { get; set; }

        private DiscoveryWalker(int start, int end, SemanticModel model)
        {
            _model = model;
            _start = start;
            _end = end;
            definedWithinSnippet = new List<ObjectInformation>();
        }

        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            // Process only nodes in the target range
            if (node.Span.Start < _end && node.Span.End > _start)
            {
                processIdentifierName(node);
            }
        }

        private void processIdentifierName(IdentifierNameSyntax node)
        {
            var symbol = _model.GetSymbolInfo(node).Symbol;
            var type = _model.GetTypeInfo(node).Type;
            if (type == null || symbol == null)
            {
                // We need a symbol
                // Method names are an example of identifiers with no types, and we ignore them
                return;
            }
            if (node.IsVar)
            {
                return;
            }
            if (symbol != null)
            {
                // Not used:
                // var fullTypeName = type?.ToString() ?? String.Empty;
                // var containingType = symbol.ContainingType;
                var typeName = type?.Name ?? String.Empty;
                var namedType = type as INamedTypeSymbol;
                if (namedType != null)
                {
                    List<string> argumentNames = new List<string>();
                    foreach (var argument in namedType.TypeArguments)
                    {
                        var argumentName = argument.Name;
                        argumentNames.Add(argumentName);
                    }
                    if (argumentNames.Any())
                    {
                        typeName = $"{type?.Name}<{String.Join(", ", argumentNames)}>";
                    }
                }

                foreach (var location in symbol.OriginalDefinition.Locations)
                {
                    // Process only nodes defined outside of the target range
                    if (!location.IsInSource || !(location.SourceSpan.Start < _end && location.SourceSpan.End > _start))
                    {
                        var objectInfo = new ObjectInformation()
                        {
                            Identifier = node?.Identifier.ToString() ?? String.Empty,
                            TypeName = typeName ?? String.Empty,
                            Namespace = type?.ContainingNamespace?.ToString() ?? String.Empty,
                            AssemblyName = type?.ContainingAssembly?.Name ?? String.Empty,
                            Kind = symbol.Kind.ToString(),
                        };
                        definedWithinSnippet.Add(objectInfo);
                    }

                }
            }
        }

    }
}