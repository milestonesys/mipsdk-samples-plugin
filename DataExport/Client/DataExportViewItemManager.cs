using System;
using System.IO;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace DataExport.Client
{
    public class DataExportViewItemManager : ViewItemManager
    {
        private string _noteName;

        public DataExportViewItemManager() : base("DataExportViewItemManager")
        {
        }

        #region Methods overridden 
        public override void PropertiesLoaded()
        {
            _noteName = GetProperty(DataExportDefinition.NoteNamePropertyName);

            if (_noteName == null)
            {
                NoteName = GenerateInitialNoteName();
            }

            if (EnvironmentManager.Instance.SmartClientInOfflineMode && !string.IsNullOrEmpty(_noteName))
            {
                try
                {
                    StreamReader file = File.OpenText(Path.Combine(EnvironmentManager.Instance.SmartClientOfflineDirectory, _noteName + DataExportDefinition.ExportFileExtension));
                    DataExportDefinition.SampleDataProvider[_noteName] = file.ReadToEnd();
                    file.Close();
                }
                catch (Exception) { }
            }
        }

        private string GenerateInitialNoteName()
        {
            return "Note - " + DateTime.Now.ToString("O").Replace(':', '-');
        }

        public override PropertiesWpfUserControl GeneratePropertiesWpfUserControl()
        {
            return new DataExportPropertiesWpfUserControl(this);
        }

        public override ViewItemWpfUserControl GenerateViewItemWpfUserControl()
        {
            return new DataExportViewItemWpfUserControl(this);
        }
       
        #endregion

        public string NoteName
        {
            get { return _noteName; }
            set
            {
                if (_noteName != null && value != null && DataExportDefinition.SampleDataProvider.ContainsKey(_noteName))
                {
                    DataExportDefinition.SampleDataProvider[value] = DataExportDefinition.SampleDataProvider[_noteName];
                }

                _noteName = value;
                SetProperty(DataExportDefinition.NoteNamePropertyName, _noteName);
                SaveProperties();
            }
        }
    }
}
