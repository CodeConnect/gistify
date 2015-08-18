using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.CodeAnalysis.CSharp;
using CodeConnect.Gistify.Extension.CodeAnalysis;

namespace CodeConnect.Gistify.Tests
{
    [TestClass]
    public class VariableCollectorTests
    {
        [TestMethod]
        public void FindDeclarationsFromMethod1()
        {
            Microsoft.CodeAnalysis.SyntaxTree tree = TestHelpers.GetSampleTree1();
            var compilation = TestHelpers.CreateCompilation(tree);
            var model = compilation.GetSemanticModel(tree);

            // Test 1:
            var start = 389 + 130;
            var end = 563 + 130;
            var walker = new DiscoveryWalker(start, end, model);
            walker.Visit(tree.GetRoot());

            var definedOutside = walker.DefinedOutside;
            var snippet = SyntaxBuilder.GetSnippet(tree, start, end);
            var usings = SyntaxBuilder.GetUsingStatements(definedOutside);
            var declarations = SyntaxBuilder.GetDeclarations(definedOutside);

            Assert.AreEqual(2, definedOutside.Count);
            Assert.AreEqual(1, usings.Length);
            Assert.AreEqual(2, declarations);
        }

        [TestMethod]
        public void FindDeclarationsFromMethod2()
        {
            Microsoft.CodeAnalysis.SyntaxTree tree = TestHelpers.GetSampleTree1();
            var compilation = TestHelpers.CreateCompilation(tree);
            var model = compilation.GetSemanticModel(tree);

            // Test 2:
            var start = 581 + 130;
            var end = 723 + 130;
            var walker = new DiscoveryWalker(start, end, model);
            walker.Visit(tree.GetRoot());

            var definedOutside = walker.DefinedOutside;
            var snippet = SyntaxBuilder.GetSnippet(tree, start, end);
            var usings = SyntaxBuilder.GetUsingStatements(definedOutside);
            var declarations = SyntaxBuilder.GetDeclarations(definedOutside);

            Assert.AreEqual(2, definedOutside.Count);
            Assert.AreEqual(1, usings.Length);
            Assert.AreEqual(2, declarations);
        }

        [TestMethod]
        public void FindDeclarationsFromLineOfCode()
        {
            Microsoft.CodeAnalysis.SyntaxTree tree = TestHelpers.GetSampleTree1();
            var compilation = TestHelpers.CreateCompilation(tree);
            var model = compilation.GetSemanticModel(tree);

            var start = 628;
            var end = 640;
            var walker = new DiscoveryWalker(start, end, model);
            walker.Visit(tree.GetRoot());

            var definedOutside = walker.DefinedOutside;
            var snippet = SyntaxBuilder.GetSnippet(tree, start, end);
            var usings = SyntaxBuilder.GetUsingStatements(definedOutside);
            var declarations = SyntaxBuilder.GetDeclarations(definedOutside);

            Assert.AreEqual(2, definedOutside.Count);
            Assert.AreEqual(1, usings.Length);
            Assert.AreEqual(2, declarations);
        }

    }
}
