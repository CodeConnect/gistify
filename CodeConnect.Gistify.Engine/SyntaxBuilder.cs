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
                var name = objectInfo.Namespace;
                if (namespacesAndAssemblies.ContainsKey(name))
                {
                    continue;
                }
                usingStatements.AppendLine(createUsingStatement(objectInfo));
                namespacesAndAssemblies.Add(name, objectInfo.AssemblyName);
            }
            return usingStatements.ToString();
        }

        private static string createUsingStatement(ObjectInformation objectInfo)
        {
            return $"using {objectInfo.Namespace}; // in assembly {objectInfo.AssemblyName}";
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
            string previousKind = String.Empty;

            foreach (var objectInfo in objectInfos.OrderBy(info => info.Kind))
            {
                if (previousKind != objectInfo.Kind)
                {
                    declarations.AppendLine($"// {objectInfo.Kind}:");
                    previousKind = objectInfo.Kind;
                }
                if (processedObjects.Contains(objectInfo))
                {
                    continue;
                }
                declarations.AppendLine(createDeclaration(objectInfo));
                processedObjects.Add(objectInfo);
            }
            declarations.AppendLine("/* --- */");
            return declarations.ToString();
        }

        private static string createDeclaration(ObjectInformation objectInfo)
        {
            return $"{objectInfo.TypeName} {objectInfo.Identifier}; // using {objectInfo.Namespace}";
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
            return String.Concat(/*usingStatements, */declarations, snippetCode);
        }
    }
}