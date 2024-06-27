using System;
using VideoOS.Platform.Search;
using VideoOS.Platform.Search.FilterCategories;
using VideoOS.Platform.UI.Controls;

namespace SCSearchAgent.SCPeopleWithAccessoriesSearchAgent.SearchAgent
{
    /// <summary>
    /// Defines the search agent plugin.
    /// </summary>
    public class SCPeopleWithAccessoriesSearchAgentPlugin : SearchAgentPlugin
    {
        internal static Guid PluginId = new Guid("A3CCAEC3-F928-4E05-AA3F-7439C11B52E0");

        /// <summary>
        /// Unique id of this plugin.
        /// </summary>
        public override Guid Id { get; protected set; } = PluginId;

        /// <summary>
        /// Name of the plugin.
        /// </summary>
        public override string Name { get; protected set; } = "PeopleWithAccessoriesSearchAgentPlugin";

        /// <summary>
        /// The filter container which this agent can search in.
        /// </summary>
        public override SearchFilterCategory SearchFilterCategory { get; protected set; }

        /// <summary>
        /// Creates a new search definition.
        /// </summary>
        /// <param name="searchScope">The search scope.</param>
        /// <returns>A new search definition.</returns>
        public override SearchDefinition CreateSearchDefinition(SearchScope searchScope)
        {
            return new SCPeopleWithAccessoriesSearchDefinition(searchScope);
        }

        /// <summary>
        /// This method is called when the user has logged in and configuration is accessible.
        /// If a user logs out and in again, this method will be called at every login. This
        /// should be used if the plugin is accessing configuration items.
        /// </summary>
        public override void Init()
        {
            // We use the existing "Person" category for this search agent. 
            // It is strongly recommended to use the existing categories whenever it is posssible.
            SearchFilterCategory = new PersonSearchFilterCategory("People with accessories", (VideoOSIconSourceBase)null,
                PersonSearchFilterCategory.StandardSearchFilters.None,
                new SearchFilter[]
                {
                        SCPeopleWithAccessoriesSearchDefinition.AccessoryTypeFilter
                });
        }

        /// <summary>
        /// Called by the Environment when the user logs off. Disposes of any resources.
        /// </summary>
        public override void Close()
        {
            //TODO: dispose resources here
        }
    }
}
