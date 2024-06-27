using System;
using VideoOS.Platform.Search;
using VideoOS.Platform.Search.FilterCategories;
using VideoOS.Platform.UI.Controls;

namespace SCSearchAgent.SCAnimalsSearchAgent.SearchAgent
{
    /// <summary>
    /// Defines the search agent plugin.
    /// </summary>
    public class SCAnimalsSearchAgentPlugin : SearchAgentPlugin
    {
        internal static Guid PluginId = new Guid("575d0760-d653-4555-aba9-807a918e816f");

        /// <summary>
        /// Unique id of this plugin.
        /// </summary>
        public override Guid Id { get; protected set; } = PluginId;

        /// <summary>
        /// Name of the plugin.
        /// </summary>
        public override string Name { get; protected set; } = "AnimalsSearchAgentPlugin";

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
            return new SCAnimalsSearchDefinition(searchScope);
        }

        /// <summary>
        /// This method is called when the user has logged in and configuration is accessible.
        /// If a user logs out and in again, this method will be called at every login. This
        /// should be used if the plugin is accessing configuration items.
        /// </summary>
        public override void Init()
        {
            // By creating an instance of OtherSearchFilterCategory, we add a new search category, instead of 
            // using one of the existing ones (like Person or Vehicle). We do that because "Animals search agent" 
            // does not logically fit into any of the existing search categories.
            // However, it is strongly recommended to use the existing categories whenever it is posssible.
            SearchFilterCategory = new OtherSearchFilterCategory("Animals", (VideoOSIconSourceBase)null,
                    new SearchFilter[]
                    {
                        SCAnimalsSearchDefinition.ActivityFilter,
                        SCAnimalsSearchDefinition.FamilyFilter,
                        SCAnimalsSearchDefinition.SpeciesFilter,
                        SCAnimalsSearchDefinition.AreaFilter,
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
