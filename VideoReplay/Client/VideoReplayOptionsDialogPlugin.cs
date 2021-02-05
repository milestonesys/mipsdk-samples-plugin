using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VideoOS.Platform.Client;

namespace VideoReplay.Client
{
	public class VideoReplayOptionsDialogPlugin : OptionsDialogPlugin
	{

		public override void Init()
		{
		}
		public override void Close()
		{
		}

		/// <summary>
		/// Create a UserControl to place on the options dialog.
		/// </summary>
		/// <returns></returns>
		public override OptionsDialogUserControl GenerateUserControl()
		{
			return new VideoReplayOptionsDialogUserControl();
		}

		/// <summary>
		/// A unique ID of this plugin
		/// </summary>
		public override Guid Id
		{
			get { return VideoReplayDefinition.VideoReplayOptionsDialog; }
		}

		/// <summary>
		/// The name displayed on the side selection.
		/// </summary>
		public override string Name
		{
			get { return "VideoReplay Options"; }
		}

		/// <summary>
		/// Method called when you need to save the user changes.
		/// Return true for OK, and false if the "GetLastSaveError" contains an error message.
		/// </summary>
		/// <returns></returns>
		public override bool SaveChanges()
		{
			SaveProperties(true);
			return true;
		}

		public override string GetLastSaveError()
		{
			return "";
		}
	}
}
