using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Platform;
using VideoOS.Platform.Search;
using VideoOS.Platform.Search.FilterValues;
using VideoOS.Platform.Search.Results;
using VideoOS.Platform.Util;

namespace SCSearchAgent.SCPeopleWithAccessoriesSearchAgent.SearchAgent
{
    /// <summary>
    /// Represents an independent search definition.
    /// </summary>
    public class SCPeopleWithAccessoriesSearchDefinition : SearchDefinition
    {
        internal static readonly SCPeopleWithAccessoriesSearchFilter AccessoryTypeFilter = new SCPeopleWithAccessoriesSearchFilter { Name = "Accessory type" };

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="searchScope">Search scope.</param>
        public SCPeopleWithAccessoriesSearchDefinition(SearchScope searchScope) : base(searchScope)
        { }

        /// <summary>
        /// Implementation of search. This method is called when running in a pre 2020 R1 Smart Client; sort order is not provided here.
        /// NOTE: You must override this method if you want your plugin to work in a pre 2020 R1 Smart Client.
        /// </summary>
        /// <param name="sessionId">A unique session id identifying the started search.</param>
        /// <param name="from">The start of the time span in which to search.</param>
        /// <param name="to">The end of the time span in which to search.</param>
        /// <param name="items">The items (i.e. cameras) to include in the search.</param>
        /// <param name="cancellationToken">
        /// Cancellation token which can be used to cancel an ongoing search (if the implementation allows it).
        /// </param>
        protected override void Search(Guid sessionId, DateTime from, DateTime to, IEnumerable<Item> items, CancellationToken cancellationToken)
        {
            // It is important NOT to use the SearchInput type in this branch of the code. Doing so will cause a runtime exception because the
            // VideoOS.Platform.dll shipped with the Smart Client does not include this type (i.e. the version is older than 2020 R1)
            SearchInternal(sessionId, from, to, items, true, cancellationToken);
        }

        /// <summary>
        /// Implementation of search. This method is called when running in a 2020 R1 Smart Client or newer; sort order is included in the searchInput.
        /// </summary>
        /// <param name="searchInput">Search input containing the input to the search such as (SessionId, From, To, Items, and SortOrder).</param>
        /// <param name="cancellationToken">Cancellation token which can be used to cancel an ongoing search (if the implementation allows it).</param>
        protected override void Search(SearchInput searchInput, CancellationToken cancellationToken)
        {
            var ascendingSortOrder = searchInput.SortOrder == SearchSortOrder.Unspecified ||
                                     searchInput.SortOrder == SearchSortOrder.OldestStartTime ||
                                     searchInput.SortOrder == SearchSortOrder.OldestEventTime;
            SearchInternal(searchInput.SessionId, searchInput.From, searchInput.To, searchInput.Items, ascendingSortOrder, cancellationToken);
        }

        private void SearchInternal(Guid sessionId, DateTime from, DateTime to, IEnumerable<Item> items,
            bool ascendingSortOrder, CancellationToken cancellationToken)
        {
            // This implementation is for demo purposes only: creates dummy search result per camera and selected accessory type.
            // Respecting the provided sort order(i.e.delivering results in that order) can help reduce "flickering results" in the results
            // list in Smart Client. Since this sample is based on random trigger times, respecting the sort order does not make much sense.
            // If however you retrieve results from a database or similar, it is very much encouraged to respect the provided sort order.

            var totalSpan = (to - from).TotalMilliseconds;
            var rnd = new Random(37);
            var accessoryTypeValue = SearchCriteria.GetFilterValues(AccessoryTypeFilter).OfType<SelectionFilterValue>().FirstOrDefault();

            if (accessoryTypeValue == null)
                return;

            foreach (var selectedAccessoryTypeId in accessoryTypeValue.SelectedIds)
            {
                const string title = "A person with an accessory found!";
                foreach (Item camera in items)
                {
                    var triggerTime = totalSpan * rnd.NextDouble();
                    var endTime = from.AddMilliseconds(triggerTime + totalSpan / 5);
                    var id = GuidUtil.Create(SCPeopleWithAccessoriesSearchAgentPlugin.PluginId,
                        title,
                        camera.FQID.ObjectId.ToString(),
                        from.AddMilliseconds(triggerTime).Ticks.ToString(CultureInfo.InvariantCulture),
                        endTime.Ticks.ToString(CultureInfo.InvariantCulture));
                    var result = new SCPeopleWithAccessoriesSearchResultData(id)
                    {
                        Title = title,
                        Item = camera,
                        RelatedItems = null,
                        BeginTime = from.AddMilliseconds(triggerTime - totalSpan / 10),
                        TriggerTime = from.AddMilliseconds(triggerTime),
                        EndTime = endTime,
                        Accessory = AccessoryTypeFilter.AccessoryTypes[selectedAccessoryTypeId]
                    };
                    FireSearchResultsReadyEvent(sessionId, new[] { result });
                }
            }
        }
    }

    /// <summary>
    /// Represents a single search result. A search performed on a SCPeopleWithAccessoriesSearchResultData
    /// instance may yield many instances of derivatives of this class.
    /// </summary>
    public class SCPeopleWithAccessoriesSearchResultData : SearchResultData
    {
        /// <summary>
        /// Unique id for the SearchUserControlType to use for representing SCPeopleWithAccessoriesSearchResultData.
        /// </summary>
        public static Guid PeopleWithAccessoriesResultType = new Guid("797BB070-2A41-486C-AF4C-46C92A9D28D0");

        /// <summary>
        /// The accessory that the person has in the search result.
        /// </summary>
        public string Accessory { get; set; }

        /// <summary>
        /// Create instance of the SCPeopleWithAccessoriesSearchResultData. Id property
        /// is used to identify the search result between searches: it is desired that a
        /// search repeated with same filters produce results with same ids. It is allowed
        /// to generate results with new id's, but that will disable certain functionality.
        /// For example, result items selection may not function for such items.
        /// </summary>
        /// <param name="id">The unique identifier of the search result.</param>
        public SCPeopleWithAccessoriesSearchResultData(Guid id) : base(id)
        {
            SearchResultUserControlType = PeopleWithAccessoriesResultType;
        }        

        protected override Task<ICollection<ResultProperty>> GetPropertiesAsync(CancellationToken token)
        {
            var props = new Collection<ResultProperty>()
            {
                new ResultProperty("Accessory", Accessory)
            };
            return Task.FromResult<ICollection<ResultProperty>>(props);
        }
    }
}
