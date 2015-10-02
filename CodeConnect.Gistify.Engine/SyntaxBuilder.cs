using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System.Text;
using System.Linq;

[assembly: InternalsVisibleTo("CodeConnect.Gistify.Tests")]

namespace CodeConnect.Gistify.Engine
{
    internal class SyntaxBuilder
    {
        internal const string SPACER = "// . . . \n";

        internal static string AugmentSnippet(IEnumerable<ObjectInformation> objectInfos, SyntaxTree tree, int startPosition, int endPosition)
        {
            var usingStatements = SyntaxBuilder.getUsingStatements(objectInfos);
            var declarations = SyntaxBuilder.getDeclarations(objectInfos);
            var snippetCode = SyntaxBuilder.getSnippet(tree, startPosition, endPosition);
            var gist = SyntaxBuilder.buildGist(usingStatements, declarations, snippetCode);
            return gist;
        }

        /// <summary>
        /// Creates using statements for eligible elements
        /// </summary>
        /// <param name="objectInfos"></param>
        /// <returns></returns>
        private static string getUsingStatements(IEnumerable<ObjectInformation> objectInfos)
        {
            Dictionary<string, string> namespacesAndAssemblies = new Dictionary<string, string>();
            StringBuilder usingStatements = new StringBuilder();
            foreach (var objectInfo in objectInfos)
            {
                if (canHaveDeclaration(objectInfo))
                {
                    // This elment won't get a declaration, see if it needs a usings statement
                    continue;
                }
                var name = objectInfo.Namespace;
                if (namespacesAndAssemblies.ContainsKey(name))
                {
                    // Don't repeat using statements
                    continue;
                }
                usingStatements.AppendLine(createUsingStatement(objectInfo));
                namespacesAndAssemblies.Add(name, objectInfo.AssemblyName);
            }
            return usingStatements.ToString();
        }

        /// <summary>
        /// Decides whether the element needs a declaration or just a usings statement
        /// </summary>
        /// <param name="objectInfo"></param>
        /// <returns></returns>
        private static bool canHaveDeclaration(ObjectInformation objectInfo)
        {
            // This is a naive approach. From my basic tests it looks like NamedType comes from a declaration
            // and thus doesn't need another declaration
            return objectInfo.Kind != SymbolKind.NamedType;
        }

        private static string createUsingStatement(ObjectInformation objectInfo)
        {
            return $"using {objectInfo.Namespace}; // ({objectInfo.AssemblyName})";
        }

        /// <summary>
        /// Creates declarations for eligible elements
        /// </summary>
        /// <param name="objectInfos"></param>
        /// <returns></returns>
        private static string getDeclarations(IEnumerable<ObjectInformation> objectInfos)
        {
            StringBuilder declarations = new StringBuilder();
            HashSet<ObjectInformation> processedObjects = new HashSet<ObjectInformation>();

            foreach (var objectInfo in objectInfos.OrderBy(info => info.Kind))
            {
                if (!canHaveDeclaration(objectInfo))
                {
                    continue;
                }
                if (processedObjects.Contains(objectInfo))
                {
                    // Don't duplicate declarations
                    continue;
                }
                declarations.AppendLine(createDeclaration(objectInfo));
                processedObjects.Add(objectInfo);
            }
            return declarations.ToString();
        }

        private static string createDeclaration(ObjectInformation objectInfo)
        {
            return $"{objectInfo.TypeName} {objectInfo.Identifier}; // using {objectInfo.Namespace}; ({objectInfo.AssemblyName})";
        }

        private static string getSnippet(SyntaxTree tree, int startPos, int endPos)
        {
            var rawSnippet = tree.GetText().GetSubText(new Microsoft.CodeAnalysis.Text.TextSpan(startPos, endPos - startPos)).ToString();
            return removeWhitespace(rawSnippet);
        }

        private static string removeWhitespace(string rawSnippet)
        {
            var lines = rawSnippet.Split('\n');
            var lines2 = lines.Select(n => n).ToList();

            var leastWhitespace = lines.Min(
                l => String.IsNullOrWhiteSpace(l)
                ? int.MaxValue
                : l.TakeWhile(c => c == '\t' || c == ' ').Count()
                );
            var trimmedLines = lines.Select(
                l => String.IsNullOrWhiteSpace(l)
                ? l
                : l.Substring(leastWhitespace)).ToList();

            return String.Join("\n", trimmedLines);
        }

        private static string buildGist(string usingStatements, string declarations, string snippetCode)
        {
            string spacer = 
                String.IsNullOrEmpty(usingStatements) && String.IsNullOrEmpty(declarations) 
                ? String.Empty
                : SPACER;
            return String.Concat(usingStatements, declarations, spacer, snippetCode);
        }
    }
}