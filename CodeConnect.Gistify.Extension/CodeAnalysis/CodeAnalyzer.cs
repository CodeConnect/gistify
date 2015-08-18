using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeConnect.Gistify.Extension.CodeAnalysis
{
    public static class CodeAnalyzer
    {
        public static string GetGistFromSnippet(int startPosition, int endPosition, string filePath)
        {
            var document = VSIntegration.GetDocument(filePath);
            var tree = document.GetSyntaxTreeAsync().Result;
            var model = document.GetSemanticModelAsync().Result;
            var objectInfos = FindDeclarations(tree, model, startPosition, endPosition);
            var gist = GenerateGist(objectInfos, tree, startPosition, endPosition);
            return gist;
        }

        /// <summary>
        /// Finds all declarations with the walker,
        /// tests them against the semantic model to learn about their namespaces and types (and assemblies)
        /// builds the additional syntax
        /// </summary>
        /// <param name="snippet">Snippet of code to analyze</param>
        /// <returns>Strign representation of the gist</returns>
        public static string GenerateGist(IEnumerable<ObjectInformation> objectInfos, SyntaxTree snippet, int startPosition, int endPosition)
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
            // use a walker to visit all declarations
            // use a semantic model to discover what actually are these declarations (what do they depend on)
            // TODO: get compilation of a document

            var walker = new DiscoveryWalker(startPosition, endPosition, model);
            walker.Visit(syntax.GetRoot());
            return walker.DefinedOutside;
        }

    }
}
