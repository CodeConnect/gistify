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
        OptionsViewModel _viewModel;

        protected override UIElement Child
        {
            get { return _optionsDialogControl ?? (_optionsDialogControl = new OptionsDialogPageControl()); }
        }

        protected override void OnActivate(CancelEventArgs e)
        {
            base.OnActivate(e);
            // Build ViewModel based on the Model and pass it to the view
            _viewModel = new OptionsViewModel(SavedOptions.Instance);
            _optionsDialogControl.DataContext = _viewModel;
        }

        /// <summary>
        /// Fired before OnClosed. Fired only when closing via Apply.
        /// That's why we can't detect clicking Cancel
        /// </summary>
        /// <param name="e"></param>
        protected override void OnApply(PageApplyEventArgs e)
        {
            if (e.ApplyBehavior == ApplyKind.Apply)
            {
                SavedOptions.Instance.SaveOptions();
            }
            base.OnApply(e);
        }

        /// <summary>
        /// Fired when closing either via Apply or Cancel. Fired after OnApply
        /// Ensures that the model doesn't have changes in case of canceling.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            SavedOptions.Instance.LoadOptions();
        }
    }

}
