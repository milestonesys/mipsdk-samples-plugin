using System.Collections.Generic;
using System.Linq;
using System.Windows;
using VideoOS.Platform.Search;

namespace SCSearchAgent.SCAnimalsSearchAgent.SearchToolbar
{
    /// <summary>
    /// Instance of a SearchToolbarPlugin. An instance is needed
    /// for every place in the UI where the toolbar action is shown.
    /// </summary>
    public class SCAnimalsSearchToolbarPluginInstance : SearchToolbarPluginInstance
    {
        /// <summary>
        /// This method is called when the instance is created. This should be used if the
        /// plugin is accessing configuration items. 
        /// </summary>
        public override void Init() { }

        /// <summary>
        /// This method is called when the containing view is disposed.
        /// </summary>
        public override void Close() { }

        /// <summary>
        /// Activates the plugin with the given set of search results.
        /// </summary>
        /// <param name="searchResults">The search results to activate the plugin with.</param>
        public override ActivateResult Activate(IEnumerable<SearchResultData> searchResults)
        {
            MessageBox.Show($"Register animals in the database for {searchResults.Count()} results.",
                "Register animals",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            return ActivateResult.Default;
        }
    }
}
