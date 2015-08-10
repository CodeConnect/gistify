using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeConnect.Gistify.Extension.CodeAnalysis
{
    class CodeAnalyzer
    {
        int _startPos, _endPos;
        SyntaxTree _snippet;

        public CodeAnalyzer(int startPos, int endPos, string fileName)
        {
            //_snippet = VSIntegration
        }

        /*
        /// <summary>
        /// Finds all declarations with the walker,
        /// tests them against the semantic model to learn about their namespaces and types (and assemblies)
        /// builds the additional syntax
        /// </summary>
        /// <param name="snippet">Snippet of code to analyze</param>
        /// <returns>Strign representation of the gist</returns>
        public string GenerateGist(SyntaxTree snippet)
        {
            foreach (var declaration in FindDeclarations(snippet, model))
            {
                // TODO: Create syntax
            }

            // TODO: Return combined syntax
        }
        */

        /// <summary>
        /// Finds all declarations with the walker,
        /// tests them against the semantic model to learn about their namespaces and types (and assemblies)
        /// builds the additional syntax
        /// </summary>
        /// <returns>List whose key is the position and value is the name of a declared variable</returns>
        public List<ObjectInformation> FindDeclarations(SyntaxTree syntax, SemanticModel model)
        {
            // use a walker to visit all declarations
            // use a semantic model to discover what actually are these declarations (what do they depend on)
            // TODO: get compilation of a document

            var walker = new DiscoveryWalker(_startPos, _endPos, model);
            walker.Visit(syntax.GetRoot());
            return walker.DefinedOutside;
        }

    }
}
