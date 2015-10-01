using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.CodeAnalysis.CSharp;
using CodeConnect.Gistify.Engine;

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
            var start = 582;
            var end = start + 125 + 2;

            var objectInfos = DiscoveryWalker.FindObjects(tree, model, start, end);
            var snippet = SyntaxBuilder.AugmentSnippet(objectInfos, tree, start, end);

            var lines = snippet.Split('\n');
            var usings = lines.Where(l => l.StartsWith("using ")).ToList();
            var declarations = lines.Where(l => l.Contains("// using")).ToList();

            Assert.AreEqual(2, objectInfos.Count());
            Assert.AreEqual(1, usings.Count);
            Assert.AreEqual(2, declarations.Count);
        }

        [TestMethod]
        public void FindDeclarationsFromMethod2()
        {
            Microsoft.CodeAnalysis.SyntaxTree tree = TestHelpers.GetSampleTree1();
            var compilation = TestHelpers.CreateCompilation(tree);
            var model = compilation.GetSemanticModel(tree);

            // Test 2:
            var start = 748;
            var end = start + 202 + 5;

            var objectInfos = DiscoveryWalker.FindObjects(tree, model, start, end);
            var snippet = SyntaxBuilder.AugmentSnippet(objectInfos, tree, start, end);

            var lines = snippet.Split('\n');
            var usings = lines.Where(l => l.StartsWith("using ")).ToList();
            var declarations = lines.Where(l => l.Contains("// using")).ToList();

            Assert.AreEqual(3, objectInfos.Count());
            Assert.AreEqual(1, usings.Count);
            Assert.AreEqual(2, declarations.Count);
        }

        [TestMethod]
        public void FindDeclarationsFromLineOfCode()
        {
            Microsoft.CodeAnalysis.SyntaxTree tree = TestHelpers.GetSampleTree1();
            var compilation = TestHelpers.CreateCompilation(tree);
            var model = compilation.GetSemanticModel(tree);

            var start = 582 + 56;
            var end = start + 23;

            var objectInfos = DiscoveryWalker.FindObjects(tree, model, start, end);
            var snippet = SyntaxBuilder.AugmentSnippet(objectInfos, tree, start, end);

            var lines = snippet.Split('\n');
            var usings = lines.Where(l => l.StartsWith("using ")).ToList();
            var declarations = lines.Where(l => l.Contains("// using")).ToList();

            Assert.AreEqual(2, objectInfos.Count());
            Assert.AreEqual(1, usings.Count);
            Assert.AreEqual(2, declarations.Count);
        }

        [TestMethod]
        public void IgnoreStringEmpty()
        {
            Microsoft.CodeAnalysis.SyntaxTree tree = TestHelpers.GetSampleTree2();
            var compilation = TestHelpers.CreateCompilation(tree);
            var model = compilation.GetSemanticModel(tree);

            var start = 179;
            var end = start + 120;

            var objectInfos = DiscoveryWalker.FindObjects(tree, model, start, end);
            var snippet = SyntaxBuilder.AugmentSnippet(objectInfos, tree, start, end);

            var lines = snippet.Split('\n');
            var usings = lines.Where(l => l.StartsWith("using ")).ToList();
            var declarations = lines.Where(l => l.Contains("// using")).ToList();

            Assert.AreEqual(0, objectInfos.Count());
            Assert.AreEqual(1, usings.Count);
            Assert.AreEqual(0, declarations.Count);
        }
    }
}
