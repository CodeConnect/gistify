using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeConnect.Gistify.Tests
{
    [TestClass]
    public class VariableCollectorTests
    {
        [TestMethod]
        public void FindDeclarations()
        {
            var tree = CSharpSyntaxTree.ParseText(@"public class BaseMethodDeclarations
            {
                static BaseMethodDeclarations()
                {
                    int magic = 1;
                    magic += 1;
                }
            }");
            var compilation = TestHelpers.CreateCompilation(tree);
            var model = compilation.GetSemanticModel(tree);

            var tracker = new TrackingRewriter();
            var trackedTree = tracker.Visit(tree.GetRoot());
            // 2 = 1 log method start + 1 log method end
            Assert.AreEqual(2, TestHelpers.GetAddedInvocationsCount(tree.GetRoot(), trackedTree));

            var myMethod = tree.GetRoot().DescendantNodes().OfType<BaseMethodDeclarationSyntax>().Single();
            var rewriter = new MethodRewriter(model);
            var rewrittenMethod = rewriter.Visit(myMethod);
            var str = rewrittenMethod.NormalizeWhitespace().ToFullString();

            // ASSERT:
            // 1 invocation to log method start
            // 1 invocation to log method end
            // 0 invocation to logger, from parameters
            // 2 invocation to logger, from body
            Assert.AreEqual(4, TestHelpers.GetAddedInvocationsCount(myMethod, rewrittenMethod));
            var oldNewLines = myMethod.ToFullString().Count(n => n == '\n');
            var newNewLines = rewrittenMethod.ToFullString().Count(n => n == '\n');
            Assert.AreEqual(oldNewLines, newNewLines);
        }
    }
}
