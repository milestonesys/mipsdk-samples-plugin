using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Client.Export;

namespace DataExport
{
    public class DataExportManager : ExportManager
    {
        private int _progress;
        private readonly List<Item> _exportViewItems = new List<Item>();
        private string _error;
        private readonly string _destinationDirectory;

        public DataExportManager(ExportParameters exportParameters) : base(exportParameters)
        {
            if (exportParameters == null)
            {
                return;
            }

            _destinationDirectory = exportParameters.DestinationDirectory;
            var items = exportParameters.ViewItems.Where(
                item =>
                    item.Name == DataExportDefinition.ViewItemName &&
                    item.GetViewItemProperties().ContainsKey(DataExportDefinition.NoteNamePropertyName));

            _exportViewItems.AddRange(items);
        }

        public override string LastErrorMessage
        {
            get
            {
                return _error;
            }
        }

        public override int Progress
        {
            get
            {
                return _progress;
            }
        }

        public override void ExportCancelled()
        {
            // Potentially implement cancellation of an ongoing export here
            // In this sample the export will be so fast that it does not make sense
        }

        public override void ExportComplete()
        {
            // If we had something to clean up this should be done here
        }

        public override void ExportFailed()
        {
        }

        public override void ExportStarting()
        {
            if (_exportViewItems.Count > 0)
            {
                int progressStep = 100 / _exportViewItems.Count;
                try
                {
                    foreach (var item in _exportViewItems)
                    {
                        var properties = item.GetViewItemProperties();
                        var noteName = properties[DataExportDefinition.NoteNamePropertyName];
                        var timeIntervals = item.GetExportIntervals(); // since data in this sample is not time-related we have no use for this, but typically this is used to determine what to export
                        if (DataExportDefinition.SampleDataProvider.ContainsKey(noteName))
                        {
                            var filePath = Path.Combine(_destinationDirectory, noteName + DataExportDefinition.ExportFileExtension);
                            StreamWriter file = File.CreateText(filePath);
                            file.Write(DataExportDefinition.SampleDataProvider[noteName]);
                            file.Close();
                        }
                        _progress += progressStep;
                    }
                }
                catch (Exception ex)
                {
                    _error = ex.Message;
                }
            }
            _progress = 100;
        }

        public override bool IncludePluginFilesInExport
        {
            get
            {
                return true;
            }
        }

        public override ulong? EstimateSizeOfExport()
        {
            ulong estimatedSize = 0;
            foreach (var item in _exportViewItems)
            {
                var properties = item.GetViewItemProperties();
                var noteName = properties[DataExportDefinition.NoteNamePropertyName];
                var timeIntervals = item.GetExportIntervals(); // since data in this sample is not time-related we have no use for this, but typically this is used to determine what to export
                if (DataExportDefinition.SampleDataProvider.ContainsKey(noteName))
                {
                    estimatedSize += (ulong)DataExportDefinition.SampleDataProvider[noteName].Length;
                }
            }
            return estimatedSize;
        }

        public override void EstimationCancelled()
        {
            // Potentially implement cancellation of the estimation process here
            // In this sample the estimation will be so fast that it does not make sense
        }

        public override bool ExportPerformed
        {
            get
            {
                // Let the system know whether we performed an export (were included in the export) or not
                return _exportViewItems.Any();
            }
        }
    }
}
