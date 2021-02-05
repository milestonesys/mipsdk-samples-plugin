using SCSearchAgent.SCPeopleWithAccessoriesSearchAgent.SearchAgent;
using System;
using System.Collections.Generic;
using VideoOS.Platform.Search;

namespace SCSearchAgent.SCPeopleWithAccessoriesSearchAgent.SearchUserControl
{
    /// <summary>
    /// Defines the custom search user controls used for the search agent.
    /// </summary>
    public class SCPeopleWithAccessoriesSearchUserControlsPlugin : SearchUserControlsPlugin
    {
        /// <summary>
        /// Unique id of this plugin.
        /// </summary>
        public override Guid Id { get; protected set; } = new Guid("2F5E7F85-D698-4A3A-A010-5F8C730FDF22");

        /// <summary>
        /// Name of the plugin.
        /// </summary>
        public override string Name { get; protected set; } = "PeopleWithAccessoriesSearchUserControlsPlugin";

        /// <summary>
        /// Gets an enumerable of search result user control types indicating which search
        /// results you wish to add a (or override the default) user control for.
        /// </summary>
        public override IEnumerable<Guid> SearchResultUserControlTypes { get; protected set; } = new[] { SCPeopleWithAccessoriesSearchResultData.PeopleWithAccessoriesResultType };

        /// <summary>
        /// Gets an enumerable of search filter configuration types (i.e. types deriving
        /// from VideoOS.Platform.Search.Configuration.Criteria.FilterConfigurationBase)
        /// indicating which search criteria filter types you wish to add a (or override
        /// the default) user control for.
        /// </summary>
        public override IEnumerable<Type> SearchFilterConfigurationTypes { get; protected set; }

        /// <summary>
        /// Creates a VideoOS.Platform.Search.SearchResultUserControl which can visualize
        /// the given search result type.
        /// </summary>
        /// <param name="searchResultUserControlType"></param>
        /// <returns></returns>
        public override SearchResultUserControl CreateSearchResultUserControl(Guid searchResultUserControlType)
        {
            if (searchResultUserControlType == SCPeopleWithAccessoriesSearchResultData.PeopleWithAccessoriesResultType)
            {
                return new SCPeopleWithAccessoriesSearchResultUserControl();
            }
            return base.CreateSearchResultUserControl(searchResultUserControlType);
        }
    }
}
