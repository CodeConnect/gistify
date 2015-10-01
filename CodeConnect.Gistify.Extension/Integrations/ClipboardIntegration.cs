using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CodeConnect.Gistify.Extension.Integrations
{
    static class ClipboardIntegration
    {
        public static void HandleAugmentedSnippet(string snippet)
        {
            Clipboard.SetText(snippet);
            StatusBar.ShowStatus("Gistify copied this snippet to the clipboard.");
        }
    }
}
