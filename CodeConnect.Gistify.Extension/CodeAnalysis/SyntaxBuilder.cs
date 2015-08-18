using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System.Text;
using System.Linq;

namespace CodeConnect.Gistify.Extension.CodeAnalysis
{
    public class SyntaxBuilder
    {
        public static string GetSnippet(SyntaxTree snippet, int startPos, int endPos)
        {
            return snippet.GetText().GetSubText(new Microsoft.CodeAnalysis.Text.TextSpan(startPos, endPos - startPos)).ToString();
        }

        public static string GetGist(string usingStatements, string declarations, string snippetCode)
        {
            return String.Concat(/*usingStatements, */declarations, snippetCode);
        }

        public static string GetUsingStatements(IEnumerable<ObjectInformation> objectInfos)
        {
            Dictionary<string, string> namespacesAndAssemblies = new Dictionary<string, string>();
            StringBuilder usingStatements = new StringBuilder();
            foreach (var objectInfo in objectInfos)
            {
                var name = objectInfo.Namespace;
                if (namespacesAndAssemblies.ContainsKey(name))
                {
                    continue;
                }
                usingStatements.AppendLine(createUsingStatement(objectInfo));
                namespacesAndAssemblies.Add(name, objectInfo.AssemblyName);
            }
            return usingStatements.ToString();
        }

        public static string GetDeclarations(IEnumerable<ObjectInformation> objectInfos)
        {
            StringBuilder declarations = new StringBuilder();
            HashSet<ObjectInformation> processedObjects = new HashSet<ObjectInformation>();
            string previousKind = String.Empty;

            foreach (var objectInfo in objectInfos.OrderBy(info => info.Kind))
            {
                if (previousKind != objectInfo.Kind)
                {
                    declarations.AppendLine($"// {objectInfo.Kind}:");
                    previousKind = objectInfo.Kind;
                }
                if (processedObjects.Contains(objectInfo))
                {
                    continue;
                }
                declarations.AppendLine(createDeclaration(objectInfo));
                processedObjects.Add(objectInfo);
            }
            declarations.AppendLine("/* --- */");
            return declarations.ToString();
        }

        private static string createUsingStatement(ObjectInformation objectInfo)
        {
            return $"using {objectInfo.Namespace}; // in assembly {objectInfo.AssemblyName}";
        }

        private static string createDeclaration(ObjectInformation objectInfo)
        {
            return $"{objectInfo.TypeName} {objectInfo.Identifier}; // using {objectInfo.Namespace}";
        }
    }
}