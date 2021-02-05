using System;
using System.Collections.Generic;
using System.Drawing;
using SCSearchAgent.SCAnimalsSearchAgent.SearchAgent;
using SCSearchAgent.SCAnimalsSearchAgent.SearchToolbar;
using SCSearchAgent.SCAnimalsSearchAgent.SearchUserControl;
using SCSearchAgent.SCPeopleWithAccessoriesSearchAgent.SearchAgent;
using SCSearchAgent.SCPeopleWithAccessoriesSearchAgent.SearchUserControl;
using VideoOS.Platform;
using VideoOS.Platform.Search;

namespace SCSearchAgent
{
    /// <summary>
    /// The PluginDefinition is the ‘entry’ point to any plugin.  
    /// This is the starting point for any plugin development and the class MUST be available for a plugin to be loaded.
    /// Several PluginDefinitions are allowed to be available within one DLL.
    /// Here the references to all other plugin known objects and classes are defined.
    /// The class is constructed when the environment is loading the DLL.
    /// </summary>
    public class SCSearchAgentPluginDefinition : PluginDefinition
    {
        private readonly List<SearchAgentPlugin> _searchAgentPlugins = new List<SearchAgentPlugin>();
        private readonly List<SearchUserControlsPlugin> _searchUserControlsPlugins = new List<SearchUserControlsPlugin>();
        private readonly List<SearchToolbarPlugin> _searchToolbarPlugins = new List<SearchToolbarPlugin>();

        /// <summary>
        /// This method is called when the environment is up and running. Registration of
        //  Messages via RegisterReceiver can be done at this point.
        /// </summary>
        public override void Init()
        {
            // Add search agent plugins
            _searchAgentPlugins.Add(new SCAnimalsSearchAgentPlugin());
            _searchAgentPlugins.Add(new SCPeopleWithAccessoriesSearchAgentPlugin());

            // Add search user control plugins
            _searchUserControlsPlugins.Add(new SCAnimalsSearchUserControlsPlugin());
            _searchUserControlsPlugins.Add(new SCPeopleWithAccessoriesSearchUserControlsPlugin());

            // Add search toolbar plugins
            _searchToolbarPlugins.Add(new SCAnimalsSearchToolbarPlugin());
        }

        /// <summary>
        /// The main application is about to be in an undetermined state, either logging
        /// off or exiting. You can release resources at this point, it should match what
        /// you acquired during Init, so additional call to Init() work.
        /// </summary>
        public override void Close()
        {
            _searchAgentPlugins.Clear();
            _searchUserControlsPlugins.Clear();
            _searchToolbarPlugins.Clear();
        }

        /// <summary>
        /// The Guid identifying this PluginDefinition.
        /// </summary>
        public override Guid Id
        {
            get { return new Guid("0c51d4b0-7b67-4d0b-95e5-b1b29e73e366"); }
        }

        /// <summary>
        /// Name of the plugin.
        /// </summary>
        public override string Name
        {
            get { return AssemblyInfo.ProductNameShort; }
        }

        /// <summary>
        /// Company owning the plugin.
        /// </summary>
        public override string Manufacturer
        {
            get { return AssemblyInfo.CompanyName; }
        }

        /// <summary>
        /// Icon to be used on top level - e.g. a product or company logo.
        /// </summary>
        public override Image Icon => VideoOS.Platform.UI.Util.ImageList.Images[VideoOS.Platform.UI.Util.SDK_VSIx];

        /// <summary>
        /// Extension plug-ins running in the Smart Client for adding custom search agents.
        /// </summary>
        public override IEnumerable<SearchAgentPlugin> SearchAgentPlugins => _searchAgentPlugins;

        /// <summary>
        /// Extension plug-ins running in the Smart Client for adding/modifying custom UI for
        /// search results and/or criteria filters.
        /// </summary>
        public override IEnumerable<SearchUserControlsPlugin> SearchUserControlsPlugins => _searchUserControlsPlugins;

        /// <summary>
        /// Extension plug-ins to add to the search toolbar in the Smart Client.
        /// </summary>
        public override IEnumerable<SearchToolbarPlugin> SearchToolbarPlugins => _searchToolbarPlugins;
    }
}
