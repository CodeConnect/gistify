using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeConnect.Gistify.Extension.CodeAnalysis
{
    class CodeAnalyzer
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns>List whose key is the position and value is the name of a declared variable</returns>
        public SortedList<int, string> FindDeclarations( /* syntax tree and semantic model? */)
        {
            // use a walker to visit all declarations
            // use a semantic model to discover what actually are these declarations (what do they depend on)
            // TODO: get compilation of a document
        }
    }
}
