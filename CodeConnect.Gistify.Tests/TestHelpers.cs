using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeConnect.Gistify.Tests
{
    static class TestHelpers
    {
        internal static SyntaxTree GetTestSyntaxTreeWithCode(string testCode)
        {
            var tree = CSharpSyntaxTree.ParseText(@"
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeConnect.Gistify.MockNamespace
{
" + testCode + @"
}");
            return tree;
        }

        internal static Compilation CreateCompilation(SyntaxTree tree)
        {
            var mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
            var compilation = CSharpCompilation.Create("MyCompilation",
                syntaxTrees: new[] { tree }, references: new[] { mscorlib });
            return compilation;
        }
    }
}
