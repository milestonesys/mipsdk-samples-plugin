using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VideoOS.Platform.Client;

namespace SiteLicense.Client
{
	/// <summary>
	/// This class is included to instruct the Smart Client export function to include
	/// this plugin and DLLs with the exported material.
	/// The license itself are export automatically.
	/// Any data that this plugin need to work in offline mode should be copied to 
	/// the defined data folder in the ExportParameters.
	/// </summary>
	public class SiteLicenseExportManager : ExportManager
	{
		public SiteLicenseExportManager(ExportParameters exportParameters)
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
