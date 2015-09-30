using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Settings;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeConnect.Gistify.Extension.Options
{
    internal class SavedOptions
    {
        #region Types, constants, fields

        internal enum DefaultAction
        {
            CopyToClipboard = 1,
            UploadAsGist = 2,
        }

        [Flags]
        internal enum AfterUpload
        {
            LaunchBrowser = 1,
            CopyLink = 2
        }

        const string COLLECTION_PATH = "Gistify";
        const string DEFAULT_ACTION_PROPERTY = "DefaultAction";
        const string AFTER_UPLOAD_PROPERTY = "AfterUpload";
        const string TOKEN_PROPERTY = "Token";

        readonly WritableSettingsStore _store;
        private static SavedOptions _instance;

        #endregion

        internal static SavedOptions Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SavedOptions();
                }
                return _instance;
            }
        }

        internal void SaveOptions()
        {
            if (!_store.CollectionExists(COLLECTION_PATH))
            {
                _store.CreateCollection(COLLECTION_PATH);
            }
            _store.SetString(COLLECTION_PATH, TOKEN_PROPERTY, TokenValue);
            _store.SetInt32(COLLECTION_PATH, DEFAULT_ACTION_PROPERTY, (int)DefaultActionValue);
            _store.SetInt32(COLLECTION_PATH, AFTER_UPLOAD_PROPERTY, (int)AfterUploadValue);
        }

        #region Model's properties

        internal string TokenValue
        {
            get; set;
        }
        internal DefaultAction DefaultActionValue
        {
            get; set;
        }
        internal AfterUpload AfterUploadValue
        {
            get; set;
        }

        #endregion

        #region Private methods

        private SavedOptions()
        {
            var shellSettingsManager = new ShellSettingsManager(ServiceProvider.GlobalProvider);
            _store = shellSettingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);

            LoadOptions();
        }

        internal void LoadOptions()
        {
            if (_store.PropertyExists(COLLECTION_PATH, TOKEN_PROPERTY))
            {
                TokenValue = _store.GetString(COLLECTION_PATH, TOKEN_PROPERTY);
            }
            else
            {
                TokenValue = String.Empty;
            }

            if (_store.PropertyExists(COLLECTION_PATH, DEFAULT_ACTION_PROPERTY))
            {
                var defaultActionInt = _store.GetInt32(COLLECTION_PATH, DEFAULT_ACTION_PROPERTY);
                DefaultActionValue = (DefaultAction)defaultActionInt;
            }
            else
            {
                DefaultActionValue = DefaultAction.CopyToClipboard;
            }

            if (_store.PropertyExists(COLLECTION_PATH, AFTER_UPLOAD_PROPERTY))
            {
                var afterUploadInt = _store.GetInt32(COLLECTION_PATH, AFTER_UPLOAD_PROPERTY);
                AfterUploadValue = (AfterUpload)afterUploadInt;
            }
            else
            {
                AfterUploadValue = AfterUpload.CopyLink | AfterUpload.LaunchBrowser;
            }
        }

        #endregion
    }
}
