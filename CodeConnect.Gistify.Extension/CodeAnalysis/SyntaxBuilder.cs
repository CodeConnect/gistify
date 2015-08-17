using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace CodeConnect.Gistify.Extension.CodeAnalysis
{
    internal class SyntaxBuilder
    {
        internal static SyntaxNode GetSnippet(SyntaxTree snippet, int startPos, int endPos)
        {
            throw new NotImplementedException();
        }

        internal static SyntaxNode GetGist(SyntaxNode usingStatements, SyntaxNode snippetCode)
        {
            throw new NotImplementedException();
        }

        internal static SyntaxNode GetUsingStatements(IEnumerable<ObjectInformation> objectInfos)
        {
            throw new NotImplementedException();
        }
    }
}