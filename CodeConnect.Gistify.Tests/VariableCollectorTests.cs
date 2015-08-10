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
        public void FindDeclarations()
        {
            var tree = TestHelpers.GetTestSyntaxTreeWithCode(@"public class BaseMethodDeclarations
            {
                static int staticField = 1;
                int instanceField = 2;
                int uninitializedInstanceField;
                int instanceProperty { get; set; }

                static BaseMethodDeclarations()
                {
                    int magic = 1;
                    magic += 1;
                }
            }");
            var compilation = TestHelpers.CreateCompilation(tree);
            var model = compilation.GetSemanticModel(tree);

            var walker = new DiscoveryWalker(0, 10, model);
            walker.Visit(tree.GetRoot());
        }
    }
}
