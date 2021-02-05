using System;
using System.Windows.Forms;
using System.Xml;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace ServiceTest.Client
{
	public partial class ServiceTestPropertiesUserControl : PropertiesUserControl
	{

		#region private fields

		private ServiceTestViewItemManager _viewItemManager;

		#endregion

		#region Initialization & Dispose

		public ServiceTestPropertiesUserControl(ServiceTestViewItemManager viewItemManager)
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

		#region Event handling

		#endregion

	}

}
