using System;
using System.Collections.Generic;
using SCClientAction.Client;
using VideoOS.Platform;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI.Controls;

namespace SCClientAction
{
    public class SCClientActionDefinition : PluginDefinition
    {
        #region Private fields

        // Note that all the plugins are constructed during application start, and the constructors
        // should only contain code that references their own dll, e.g. resource load.

        private List<ClientActionGroup> _clientActionGroups = new List<ClientActionGroup>();

        #endregion

        #region Initialization

        /// <summary>
        /// This method is called when the environment is up and running.
        /// This method sets the colors for View Item and Work Space toolbar plugins.
        /// Registration of Messages via RegisterReceiver can be done at this point.
        /// </summary>
        public override void Init()
        {
            if (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.SmartClient)
            {
                ImageViewerHelper.Init();

                //Create client action groups. Note that in a production plug-in the group names should be localized.

                ClientActionGroup cameraToolsGroup = new ClientActionGroup(new Guid("2CA0965A-5DA0-4AFE-8E19-47E9FBBEC54A"), "Camera Tools", new VideoOSIconBuiltInSource() { Icon = VideoOSIconBuiltInSource.Icons.Camera })
                {
                    Actions = { new Rewind15SecondsClientAction(), new OpenInWindowClientAction() }
                };
                _clientActionGroups.Add(cameraToolsGroup);

                ClientActionGroup windowToolsGroup = new ClientActionGroup(new Guid("925A60D7-27D4-4197-BE57-67094797999A"), "Window Tools", new VideoOSIconBuiltInSource() { Icon = VideoOSIconBuiltInSource.Icons.Window })
                {
                    Actions = { new CloseAllWindowsClientAction() }
                };
                _clientActionGroups.Add(windowToolsGroup);
            }
        }

        #endregion

        /// <summary>
        /// The main application is about to be in an undetermined state, either logging off or exiting.
        /// You can release resources at this point, it should match what you acquired during Init, so additional call to Init() will work.
        /// </summary>
        public override void Close()
        {
            ImageViewerHelper.Close();
            _clientActionGroups.Clear();
        }

        #region Identification Properties

        /// <summary>
        /// Gets the unique id identifying this plugin component
        /// </summary>
        public override Guid Id
        {
            get { return new Guid("30319518-479A-44E1-BD9B-28DA878E28E1"); }
        }

        public override Guid SharedNodeId
        {
            get { return PluginSamples.Common.SampleTopNode; }
        }

        /// <summary>
        /// Define name of top level Tree node - e.g. A product name
        /// </summary>
        public override string Name
        {
            get { return "SCClientAction"; }
        }

        /// <summary>
        /// Top level name
        /// </summary>
        public override string SharedNodeName
        {
            get { return PluginSamples.Common.SampleNodeName; }
        }

        public override System.Drawing.Image Icon
        {
            get { return null; }
        }

        /// <summary>
        /// Your company name
        /// </summary>
        public override string Manufacturer
        {
            get
            {
                return PluginSamples.Common.ManufacturerName;
            }
        }

        /// <summary>
        /// Version of this plugin.
        /// </summary>
        public override string VersionString
        {
            get
            {
                return "1.0.0.0";
            }
        }

        #endregion

        #region Client related methods and properties

        public override List<ClientActionGroup> ClientActionGroups
        {
            get { return _clientActionGroups; }
        }

        #endregion
    }
}
