using System;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI.Controls;

namespace DataExport.Client
{
    public class DataExportViewItemPlugin : ViewItemPlugin
    {
        public override void Init()
        {
        }

        public override void Close()
        {
        }

        public override Guid Id
        {
            get { return DataExportDefinition.DataExportViewItemPlugin; }
        }

        public override VideoOSIconSourceBase IconSource { get => DataExportDefinition.PluginIcon; protected set => base.IconSource = value; }
 
        public override string Name
        {
            get { return DataExportDefinition.ViewItemName; }
        }

        public override ViewItemManager GenerateViewItemManager()
        {
            return new DataExportViewItemManager();
        }
    }
}
