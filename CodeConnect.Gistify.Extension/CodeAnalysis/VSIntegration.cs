using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeConnect.Gistify.Extension.CodeAnalysis
{
    static class VSIntegration
    {

        internal static Document GetDocument(string filePath)
        {
            var currentSolution = SolutionManager.CurrentSolution;
            var project = currentSolution.Projects.Where(n => n.Documents.Any(m => m.FilePath == filePath)).FirstOrDefault();
            var document = project.Documents.Where(n => n.FilePath == filePath).Single();
            return document;
        }

    }
}
