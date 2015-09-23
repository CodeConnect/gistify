using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeConnect.Gistify.Engine
{
    public static class CodeAnalyzer
    {
        public static string AugmentSelection(Document document, int startPosition, int endPosition)
        {
            var tree = document.GetSyntaxTreeAsync().Result;
            var model = document.GetSemanticModelAsync().Result;
            var objectInfos = FindDeclarations(tree, model, startPosition, endPosition);
            var augmentedSelection = AugmentSnippet(objectInfos, tree, startPosition, endPosition);
            return augmentedSelection;
        }

        /// <summary>
        /// Finds all declarations with the walker,
        /// tests them against the semantic model to learn about their namespaces and types (and assemblies)
        /// builds the additional syntax
        /// </summary>
        /// <param name="snippet">Snippet of code to analyze</param>
        /// <returns>Strign representation of the gist</returns>
        public static string AugmentSnippet(IEnumerable<ObjectInformation> objectInfos, SyntaxTree snippet, int startPosition, int endPosition)
        {
            var usingStatements = SyntaxBuilder.GetUsingStatements(objectInfos);
            var declarations = SyntaxBuilder.GetDeclarations(objectInfos);
            var snippetCode = SyntaxBuilder.GetSnippet(snippet, startPosition, endPosition);
            var gist = SyntaxBuilder.GetGist(usingStatements, declarations, snippetCode);
            return gist;
        }

        /// <summary>
        /// Finds all declarations with the walker,
        /// tests them against the semantic model to learn about their namespaces and types (and assemblies)
        /// builds the additional syntax
        /// </summary>
        /// <returns>List whose key is the position and value is the name of a declared variable</returns>
        public static IEnumerable<ObjectInformation> FindDeclarations(SyntaxTree syntax, SemanticModel model, int startPosition, int endPosition)
        {
            var walker = new DiscoveryWalker(startPosition, endPosition, model);
            walker.Visit(syntax.GetRoot());
            return walker.DefinedOutside;
        }

    }
}
