using VideoOS.Platform.Client;

namespace TimelineViewItem.Client
{
    /// <summary>
    /// This class is included to instruct the Smart Client export function to include
    /// this plugin and DLLs with the exported material.
    /// Any data that this plugin need to work in offline mode should be copied to 
    /// the defined data folder in the ExportParameters.
    /// </summary>
    public class TimelineViewItemExportManager : ExportManager
	{
		public TimelineViewItemExportManager(ExportParameters exportParameters)
			: base(exportParameters)
		{
			
		}

		public override void ExportCancelled()
		{
		}

		public override void ExportComplete()
		{
		}

		public override void ExportFailed()
		{
		}

		public override void ExportStarting()
		{
		}

		public override string LastErrorMessage
		{
			get { return ""; }
		}

		public override int Progress
		{
			get { return 100; }
		}
	}
}
