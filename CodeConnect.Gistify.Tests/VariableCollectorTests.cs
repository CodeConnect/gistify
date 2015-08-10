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
            var tree = TestHelpers.GetTestSyntaxTreeWithCode(@"public class SampleClass
            {
                static int staticField = 1;
                int instanceField = 2;
                int uninitializedInstanceField;
                int instanceProperty { get; set; }

                static SampleClass()
                {
                    int magic = 1;
                    magic += staticField;
                }

                void Test1()
                {
                    int magic = 1;
                    magic += instanceField;
                    magic += instanceProperty;
                }

                void Test2()
                {
                    uninitializedInstanceField = 3;
                    InstanceProperty = 3;
                }
            }");
            var compilation = TestHelpers.CreateCompilation(tree);
            var model = compilation.GetSemanticModel(tree);

            var walker = new DiscoveryWalker(400, 700, model);
            walker.Visit(tree.GetRoot());
        }
    }
}
