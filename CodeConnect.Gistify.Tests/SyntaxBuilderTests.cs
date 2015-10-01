using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.CodeAnalysis.CSharp;
using CodeConnect.Gistify.Engine;

namespace CodeConnect.Gistify.Tests
{
    [TestClass]
    public class SyntaxBuilderTests
    {
        [TestMethod]
        public void GetSnippetTrimsWhitespace()
        {
            Microsoft.CodeAnalysis.SyntaxTree tree = TestHelpers.GetSampleTree1();
            var snippet = SyntaxBuilder.GetSnippet(tree, 582, 582 + 125 + 2);
            Assert.IsFalse(snippet.Contains("\t"));
            Assert.IsFalse(snippet.Contains("    "));
        }

        [TestMethod]
        public void GetSnippetTrimsUnevenWhitespace()
        {
            Microsoft.CodeAnalysis.SyntaxTree tree = TestHelpers.GetSampleTree1();
            var snippet = SyntaxBuilder.GetSnippet(tree, 582 - 20, 582 + 125 + 2);
            Assert.IsFalse(snippet.Contains("\t"));
            Assert.IsFalse(snippet.Contains("     "));
        }
    }
}
