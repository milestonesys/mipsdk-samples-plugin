using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace SequenceViewer.Client
{
	public partial class SequenceViewerPropertiesUserControl : PropertiesUserControl
	{

		#region private fields

		private SequenceViewerViewItemManager _viewItemManager;

		#endregion

		#region Initialization & Dispose

		public SequenceViewerPropertiesUserControl(SequenceViewerViewItemManager viewItemManager)
		{
			_viewItemManager = viewItemManager;
			InitializeComponent();
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
