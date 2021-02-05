using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace VideoReplay.Client
{
	public partial class VideoReplayPropertiesUserControl : PropertiesUserControl
	{

		#region private fields

		private VideoReplayViewItemManager _viewItemManager;

		#endregion

		#region Initialization & Dispose

		/// <summary>
		/// This class is created by the ViewItemManager.  
		/// </summary>
		/// <param name="viewItemManager"></param>
		public VideoReplayPropertiesUserControl(VideoReplayViewItemManager viewItemManager)
		{
			_viewItemManager = viewItemManager;
			InitializeComponent();
		}

		/// <summary>
		/// Setup events and message receivers and load stored configuration.
		/// </summary>
		public override void Init()
		{
		}

		/// <summary>
		/// Perform any cleanup stuff and event -=
		/// </summary>
		public override void Close()
		{
		}


		#endregion

	}

	internal class ComboBoxNode
	{
		internal Item Item { get; private set; }
		internal ComboBoxNode(Item item)
		{
			Item = item;
		}

		public override string ToString()
		{
			return Item.Name;
		}
	}
}
