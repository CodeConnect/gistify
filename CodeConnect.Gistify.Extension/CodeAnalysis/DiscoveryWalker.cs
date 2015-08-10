using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeConnect.Gistify.Extension.CodeAnalysis
{
    public class DiscoveryWalker : CSharpSyntaxWalker
    {
        SemanticModel _model;
        int _start, _end;

        /// <summary>
        /// Contains information of objects defined outside of the provided scope
        /// </summary>
        public List<ObjectInformation> DefinedOutside { get; private set; }

        public DiscoveryWalker(int start, int end, SemanticModel model)
        {
            _model = model;
            _start = start;
            _end = end;
            DefinedOutside = new List<ObjectInformation>();
        }

        public override void VisitFieldDeclaration(FieldDeclarationSyntax node)
        {
            base.VisitFieldDeclaration(node);
            foreach (var variable in node.Declaration.Variables)
            {
                var objectInfo = new ObjectInformation()
                {
                    Identifier = variable.Identifier.ToString(),
                    // TODO: fill the rest
                };
                DefinedOutside.Add(objectInfo);
            }
            var t = DefinedOutside;
        }

        public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            base.VisitPropertyDeclaration(node);

            var objectInfo = new ObjectInformation()
            {
                Identifier = node.Identifier.ToString(),
                // TODO: fill the rest
            };
            DefinedOutside.Add(objectInfo);
        }
    }
}