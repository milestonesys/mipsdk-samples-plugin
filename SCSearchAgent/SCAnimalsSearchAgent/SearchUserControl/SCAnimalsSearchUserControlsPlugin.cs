using System;
using System.Collections.Generic;
using SCSearchAgent.SCAnimalsSearchAgent.SearchAgent;
using SCSearchAgent.SCAnimalsSearchAgent.SearchUserControl.WinForms;
using VideoOS.Platform.Search;
using VideoOS.Platform.Search.FilterConfigurations;

namespace SCSearchAgent.SCAnimalsSearchAgent.SearchUserControl
{
    /// <summary>
    /// Defines the search user controls plugin interface. Can be used to create
    /// custom UI controls to visualize VideoOS.Platform.Search.SearchResultData and/or
    /// VideoOS.Platform.Search.Configuration.Criteria.FilterConfigurationBase instances.
    /// </summary>
    public class SCAnimalsSearchUserControlsPlugin : SearchUserControlsPlugin
    {
        /// <summary>
        /// Unique id of this plugin.
        /// </summary>
        public override Guid Id { get; protected set; } = new Guid("7748f609-6634-4d8d-9f87-bb765a501779");

        /// <summary>
        /// Name of the plugin.
        /// </summary>
        public override string Name { get; protected set; } = "AnimalsSearchUserControlsPlugin";

        /// <summary>
        /// Gets an enumerable of search result user control types indicating which search
        /// results you wish to add a (or override the default) user control for.
        /// </summary>
        public override IEnumerable<Guid> SearchResultUserControlTypes { get; protected set; } = new [] { AnimalsSearchResultData.AnimalsResultType };

        /// <summary>
        /// Gets an enumerable of search filter configuration types (i.e. types deriving
        /// from VideoOS.Platform.Search.Configuration.Criteria.FilterConfigurationBase)
        /// indicating which search criteria filter types you wish to add a (or override
        /// the default) user control for.
        /// </summary>
        public override IEnumerable<Type> SearchFilterConfigurationTypes { get; protected set; } = new[] { typeof(SCAnimalsFilterConfiguration) };

        /// <summary>
        /// Creates a VideoOS.Platform.Search.SearchFilterEditControl used to allow the
        /// user to edit a search criteria filter based on the given filter configuration.
        /// </summary>
        /// <param name="filterConfiguration">
        /// The configuration of the search filter with which to create a
        /// VideoOS.Platform.Search.SearchFilterEditControl.
        /// </param>
        /// <returns>
        /// User control to display the search filter edit control based on the provided
        /// filter configuration.
        /// </returns>
        public override SearchFilterEditControl CreateSearchFilterEditControl(FilterConfigurationBase filterConfiguration)
        {
            if (filterConfiguration is SCAnimalsFilterConfiguration)
            {
                return (filterConfiguration.DisplayMode == EditControlDisplayMode.InDialog)
                    ? (SearchFilterEditControl)new SCAnimalsSearchAreaFilterEditHostControl()
                    : (SearchFilterEditControl)new SCAnimalsSearchFilterEditControl();
            }
            
            return base.CreateSearchFilterEditControl(filterConfiguration);
        }

        public override SearchResultUserControl CreateSearchResultUserControl(Guid searchResultUserControlType)
        {
            if (searchResultUserControlType == AnimalsSearchResultData.AnimalsResultType)
            {
                return new AnimalsResultControl();
            }
            return base.CreateSearchResultUserControl(searchResultUserControlType);
        }
    }
}
