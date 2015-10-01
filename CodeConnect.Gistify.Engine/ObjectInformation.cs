using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace CodeConnect.Gistify.Engine
{
    internal struct ObjectInformation
    {
        internal string TypeName
        {
            get; set;
        }
        internal string Namespace
        {
            get; set;
        }
        internal string AssemblyName
        {
            get; set;
        }
        internal string Identifier
        {
            get; set;
        }
        internal SymbolKind Kind
        {
            get; set;
        }

        public override string ToString()
        {
            return $"{Kind}: {TypeName} {Identifier}";
        }
    }
}
