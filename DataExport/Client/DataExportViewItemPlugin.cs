using System;
using VideoOS.Platform.Client;

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

        public override System.Drawing.Image Icon
        {
            get { return DataExportDefinition.TreeNodeImage; }
        }

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
