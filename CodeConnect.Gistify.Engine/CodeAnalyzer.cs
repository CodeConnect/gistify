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
        /// <summary>
        /// The entry point to Gistify's engine. Takes the document and the extent of the selection,
        /// and outputs the augmented snippet.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="startPosition"></param>
        /// <param name="endPosition"></param>
        /// <returns>String with the augmented snippet.</returns>
        public static string AugmentSelection(Document document, int startPosition, int endPosition)
        {
            var tree = document.GetSyntaxTreeAsync().Result;
            var model = document.GetSemanticModelAsync().Result;
            var objectInfos = DiscoveryWalker.FindDeclarations(tree, model, startPosition, endPosition);
            var augmentedSelection = SyntaxBuilder.AugmentSnippet(objectInfos, tree, startPosition, endPosition);
            return augmentedSelection;
        }
    }
}
