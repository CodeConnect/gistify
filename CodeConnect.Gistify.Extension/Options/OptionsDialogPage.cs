using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;

namespace CodeConnect.Gistify.Extension.Options
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    //[CLSCompliant(false)]
    //[ComVisible(true)]
    [Guid("1D9ECCF3-5D2F-4112-9B25-264596873DC9")]
    public class OptionsDialogPage : UIElementDialogPage
    {
        OptionsDialogPageControl _optionsDialogControl;

        protected override UIElement Child
        {
            get { return _optionsDialogControl ?? (_optionsDialogControl = new OptionsDialogPageControl()); }
        }

        protected override void OnActivate(CancelEventArgs e)
        {
            base.OnActivate(e);
            _optionsDialogControl.DataContext = new SavedOptions();
        }

        protected override void OnApply(PageApplyEventArgs e)
        {
            // TODO: Currently everything just get saved. Provide cancelation
            base.OnApply(e);
        }
    }

}
