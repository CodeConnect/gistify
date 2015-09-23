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
    public class DiscoveryWalker : CSharpSyntaxWalker
    {
        SemanticModel _model;
        int _start, _end;

        /// <summary>
        /// Namespace of the first identifier in this snippet. We won't display usings for this one.
        /// </summary>
        public string SnippetsNamespace { get; private set; }

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

        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            // Process only nodes in the target range
            if (node.Span.Start < _end && node.Span.End > _start)
            {
                ProcessIdentifierName(node);
            }
        }

        private void ProcessIdentifierName(IdentifierNameSyntax node)
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
                        DefinedOutside.Add(objectInfo);
                    }

                }
            }
        }

    }
}