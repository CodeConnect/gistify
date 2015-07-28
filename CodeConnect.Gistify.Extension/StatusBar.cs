using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeConnect.Gistify.Extension
{
    internal class StatusBar
    {
        static IVsStatusbar _statusBar;

        internal static void Initialize(IVsStatusbar vsStatusBar)
        {
            _statusBar = vsStatusBar;
        }

        internal static void ShowStatus(string message)
        {
            if (_statusBar == null)
            {
                return;
            }
            int frozen;
            _statusBar.IsFrozen(out frozen);
            if (frozen == 0)
            {
                _statusBar.SetText(message);
            }
        }
    }
}
