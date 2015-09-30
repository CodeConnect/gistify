using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Composition;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CodeConnect.Gistify.Extension.Options
{
    class SavedOptions : INotifyPropertyChanged
    {
        #region Types, constants, fields

        enum DefaultAction
        {
            CopyToClipboard = 1,
            UploadAsGist = 2,
        }

        [Flags]
        enum AfterUpload
        {
            LaunchBrowser = 1,
            CopyLink = 2
        }

        const string COLLECTION_PATH = "Gistify";
        const string DEFAULT_ACTION_PROPERTY = "DefaultAction";
        const string AFTER_UPLOAD_PROPERTY = "AfterUpload";
        const string TOKEN_PROPERTY = "Token";

        readonly WritableSettingsStore _store;
        string _token;
        DefaultAction _defaultAction;
        AfterUpload _afterUpload;

        #endregion

        public SavedOptions()
        {
            var shellSettingsManager = new ShellSettingsManager(ServiceProvider.GlobalProvider);
            _store = shellSettingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);

            loadSettings();
            prepareForSaving();
        }

        #region Publicly available properties

        public bool DefaultActionClipboard
        {
            get
            {
                return _defaultAction == DefaultAction.CopyToClipboard;
            }
            set
            {
                _defaultAction = value ? DefaultAction.CopyToClipboard : DefaultAction.UploadAsGist;
                _store.SetInt32(COLLECTION_PATH, DEFAULT_ACTION_PROPERTY, (int)_defaultAction);
            }
        }

        public bool DefaultActionUpload
        {
            get
            {
                return _defaultAction == DefaultAction.UploadAsGist;
            }
            set
            {
                _defaultAction = value ? DefaultAction.UploadAsGist : DefaultAction.CopyToClipboard;
                _store.SetInt32(COLLECTION_PATH, DEFAULT_ACTION_PROPERTY, (int)_defaultAction);
            }
        }

        public bool AfterUploadLaunch
        {
            get
            {
                return _afterUpload.HasFlag(AfterUpload.LaunchBrowser);
            }
            set
            {
                if (value)
                {
                    _afterUpload |= AfterUpload.LaunchBrowser;
                }
                else
                {
                    _afterUpload &= ~AfterUpload.LaunchBrowser;
                }
                _store.SetInt32(COLLECTION_PATH, AFTER_UPLOAD_PROPERTY, (int)_afterUpload);
            }
        }

        public bool AfterUploadCopyLink
        {
            get
            {
                return _afterUpload.HasFlag(AfterUpload.CopyLink);
            }
            set
            {
                if (value)
                {
                    _afterUpload |= AfterUpload.CopyLink;
                }
                else
                {
                    _afterUpload &= ~AfterUpload.CopyLink;
                }
                _store.SetInt32(COLLECTION_PATH, AFTER_UPLOAD_PROPERTY, (int)_afterUpload);
            }
        }

        public string Token
        {
            get
            {
                return _token;
            }
            set
            {
                _token = value;
                _store.SetString(COLLECTION_PATH, TOKEN_PROPERTY, _token);
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Private methods

        private void loadSettings()
        {
            if (_store.PropertyExists(COLLECTION_PATH, TOKEN_PROPERTY))
            {
                _token = _store.GetString(COLLECTION_PATH, TOKEN_PROPERTY);
            }
            else
            {
                _token = String.Empty;
            }

            if (_store.PropertyExists(COLLECTION_PATH, DEFAULT_ACTION_PROPERTY))
            {
                var defaultActionInt = _store.GetInt32(COLLECTION_PATH, DEFAULT_ACTION_PROPERTY);
                _defaultAction = (DefaultAction)defaultActionInt;
            }
            else
            {
                _defaultAction = DefaultAction.CopyToClipboard;
            }

            if (_store.PropertyExists(COLLECTION_PATH, AFTER_UPLOAD_PROPERTY))
            {
                var afterUploadInt = _store.GetInt32(COLLECTION_PATH, AFTER_UPLOAD_PROPERTY);
                _afterUpload = (AfterUpload)afterUploadInt;
            }
            else
            {
                _afterUpload = AfterUpload.CopyLink | AfterUpload.LaunchBrowser;
            }
        }

        private void prepareForSaving()
        {
            if (!_store.CollectionExists(COLLECTION_PATH))
            {
                _store.CreateCollection(COLLECTION_PATH);
            }
        }

        #endregion

    }
}
