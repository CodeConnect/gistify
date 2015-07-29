using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeConnect.Gistify.Extension
{
    class GistifyEngine
    {
        internal static string PrepareGist(string filePath, int startPosition, int endPosition)
        {
            // TODO
            return "https://comealive.io";
        }

        internal static void GoToGist(string target)
        {
            if (String.IsNullOrWhiteSpace(target))
            {
                StatusBar.ShowStatus($"No link provided");
                return;
            }
            try
            {
                System.Diagnostics.Process.Start(target);
                StatusBar.ShowStatus($"Gist saved in {target}");
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                {
                    StatusBar.ShowStatus($"Unable to open a web browser.");
                }
#if DEBUG
                System.Diagnostics.Debugger.Break();
#endif
            }
            catch (System.Exception other)
            {
                StatusBar.ShowStatus($"Another error.");
#if DEBUG
                System.Diagnostics.Debugger.Break();
#endif
            }
        }
    }
}
