using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace ServerSideCarrousel.Client
{
	public class PropertiesControl : PropertiesControlBase
    {
		private CarrouselPropertiesUserControl _uiComponent;
        private CarrouselViewItemManager _carrouselViewItemManager;

		public PropertiesControl(CarrouselViewItemManager carrouselViewItemManager, CarrouselPropertiesUserControl uiComponent) 
        {
            _uiComponent = uiComponent;
            _carrouselViewItemManager = carrouselViewItemManager;

			XmlNodeList config = Configuration.Instance.GetPluginConfiguration(ServerSideCarrouselDefinition.PluginId, ServerSideCarrouselDefinition.ConfigId);
			if (config!=null)
			{
				_uiComponent.FillContent(config, carrouselViewItemManager.SelectedCarrouselId);
			}

			//set up SamplePropertiesControlForm event listeners as last thing, to avoid events during initialization
			_uiComponent.SourceSelectionChangedEvent += new EventHandler(SelectedSourceChangedHandler);
		}


        /// <summary>
        /// Perform any cleanup stuff and event -=
        /// </summary>
        public new void Close()
        {
			_uiComponent.SourceSelectionChangedEvent -= new EventHandler(SelectedSourceChangedHandler);
		}

		/// <summary>
		/// Save the ID of the selected source.
		/// </summary>
		/// <param name="sender">Is the ComboBoxNode that is now selected</param>
		/// <param name="e">Ignored</param>
		void SelectedSourceChangedHandler(object sender, EventArgs e)
		{
			((CarrouselViewItemManager) _carrouselViewItemManager).SelectedCarrouselId = ((ComboBoxNode)sender).Id;
		}

    }
}
