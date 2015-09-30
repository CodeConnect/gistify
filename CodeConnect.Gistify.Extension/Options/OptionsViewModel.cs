using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeConnect.Gistify.Extension.Options
{
    class OptionsViewModel
    {
        SavedOptions _model;

        public OptionsViewModel(SavedOptions model)
        {
            model.LoadOptions();
            _model = model;
        }

        public bool DefaultActionClipboard
        {
            get
            {
                return _model.DefaultActionValue == SavedOptions.DefaultAction.CopyToClipboard;
            }
            set
            {
                _model.DefaultActionValue = value ? SavedOptions.DefaultAction.CopyToClipboard : SavedOptions.DefaultAction.UploadAsGist;
            }
        }

        public bool DefaultActionUpload
        {
            get
            {
                return _model.DefaultActionValue == SavedOptions.DefaultAction.UploadAsGist;
            }
            set
            {
                _model.DefaultActionValue = value ? SavedOptions.DefaultAction.UploadAsGist : SavedOptions.DefaultAction.CopyToClipboard;
            }
        }

        public bool AfterUploadLaunch
        {
            get
            {
                return _model.AfterUploadValue.HasFlag(SavedOptions.AfterUpload.LaunchBrowser);
            }
            set
            {
                if (value)
                {
                    _model.AfterUploadValue |= SavedOptions.AfterUpload.LaunchBrowser;
                }
                else
                {
                    _model.AfterUploadValue &= ~SavedOptions.AfterUpload.LaunchBrowser;
                }
            }
        }

        public bool AfterUploadCopyLink
        {
            get
            {
                return _model.AfterUploadValue.HasFlag(SavedOptions.AfterUpload.CopyLink);
            }
            set
            {
                if (value)
                {
                    _model.AfterUploadValue |= SavedOptions.AfterUpload.CopyLink;
                }
                else
                {
                    _model.AfterUploadValue &= ~SavedOptions.AfterUpload.CopyLink;
                }

            }
        }

        public string Token
        {
            get
            {
                return _model.TokenValue;
            }
            set
            {
                _model.TokenValue = value;
            }
        }
    }

}
