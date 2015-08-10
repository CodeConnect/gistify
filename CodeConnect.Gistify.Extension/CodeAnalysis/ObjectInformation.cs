using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeConnect.Gistify.Extension.CodeAnalysis
{
    public struct ObjectInformation
    {
        internal string FullTypeName
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
    }
}
