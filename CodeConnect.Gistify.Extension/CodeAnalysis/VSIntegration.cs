using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeConnect.Gistify.Extension.CodeAnalysis
{
    class VSIntegration
    {

        internal string GetDocumentSyntaxRoot(string filePath)
        {
            var currentSolution = SolutionManager.CurrentSolution;
            var project = currentSolution.Projects.Where(n => n.Documents.Any(m => m.FilePath == filePath)).FirstOrDefault();
            var document = project.Documents.Where(n => n.FilePath == filePath).Single();
            var root = document.GetSyntaxRootAsync();
            return root.Result;
        }

    }
}
