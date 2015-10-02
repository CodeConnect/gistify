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

        /// <summary>
        /// Finds elements used by the code in specified region of provided syntax.
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="model"></param>
        /// <param name="startPosition"></param>
        /// <param name="endPosition"></param>
        /// <returns></returns>
        internal static IEnumerable<ObjectInformation> FindObjects(SyntaxTree tree, SemanticModel model, int startPosition, int endPosition)
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

        /// <summary>
        /// Called by the base class. Processes identifiers that are in desired range.
        /// </summary>
        /// <param name="node"></param>
        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            // Process only nodes in the target range
            if (node.Span.Start < _end && node.Span.End > _start)
            {
                processIdentifierName(node);
            }
        }

        /// <summary>
        /// Examines the identifier and if applicable, creates an instance of ObjectInformation
        /// which will be consumed later by the SyntaxBuilder
        /// </summary>
        /// <param name="node"></param>
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
                var typeName = type?.Name ?? String.Empty;
                // Handle generics (TODO: recursively go through all levels)
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

                // Attempt to remove trivial identifiers like String.Empty
                var definitionName = symbol.OriginalDefinition.ToDisplayString();
                var actualName = typeName + "." + symbol.MetadataName;
                if (String.Compare(definitionName, actualName, ignoreCase: true) == 0)
                {
                    // This identifier is already well defined in the snippet.
                    return;
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
                            Kind = symbol.Kind,
                        };
                        definedWithinSnippet.Add(objectInfo);
                    }
                }
            }
        }
    }
}