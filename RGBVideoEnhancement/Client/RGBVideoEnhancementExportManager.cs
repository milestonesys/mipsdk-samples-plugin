using VideoOS.Platform.Client;

namespace RGBVideoEnhancement.Client
{
	public class RGBVideoEnhancementExportManager : ExportManager
	{
		public RGBVideoEnhancementExportManager(ExportParameters exportParameters) : base(exportParameters)
		{			
		}

		public override bool IncludePluginFilesInExport
		{
			get { return true; }
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
