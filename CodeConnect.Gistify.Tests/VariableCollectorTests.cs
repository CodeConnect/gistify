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
            var walker = new DiscoveryWalker(389 + 130, 563 + 130, model);
            walker.Visit(tree.GetRoot());

            var definedOutside = walker.DefinedOutside;
            Assert.AreEqual(2, definedOutside.Count);
        }

        [TestMethod]
        public void FindDeclarationsFromMethod2()
        {
            Microsoft.CodeAnalysis.SyntaxTree tree = TestHelpers.GetSampleTree1();
            var compilation = TestHelpers.CreateCompilation(tree);
            var model = compilation.GetSemanticModel(tree);

            // Test 2:
            var walker = new DiscoveryWalker(581 + 130, 723 + 130, model);
            walker.Visit(tree.GetRoot());

            var definedOutside = walker.DefinedOutside;
            Assert.AreEqual(2, definedOutside.Count);
        }

        [TestMethod]
        public void FindDeclarationsFromLineOfCode()
        {
            Microsoft.CodeAnalysis.SyntaxTree tree = TestHelpers.GetSampleTree1();
            var compilation = TestHelpers.CreateCompilation(tree);
            var model = compilation.GetSemanticModel(tree);

            var walker = new DiscoveryWalker(475 + 130, 498 + 130, model);
            walker.Visit(tree.GetRoot());

            var definedOutside = walker.DefinedOutside;
            Assert.AreEqual(2, definedOutside.Count);
        }

    }
}
