using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Platform;
using VideoOS.Platform.Search;
using VideoOS.Platform.Search.FilterConfigurations;
using VideoOS.Platform.Search.FilterValues;
using VideoOS.Platform.Search.Results;
using VideoOS.Platform.Util;

namespace SCSearchAgent.SCAnimalsSearchAgent.SearchAgent
{
    public class AnimalActivityFilter : SearchFilter
    {
        public AnimalActivityFilter()
        {
            Id = new Guid("33BCB717-6E32-4268-B4AE-76A4BE1182A3");
        } 
        
        public override FilterConfigurationBase GetFilterConfiguration()
        {
            return new CheckBoxFilterConfiguration() { CheckBoxTextValue = "Animals eating", DisplayMode = EditControlDisplayMode.SnapToParentWidth };
        }

        public override FilterValueBase CreateValue()
        {
            BoolFilterValue value = new BoolFilterValue();
            ResetValue(value);
            return value;
        }

        public override void ResetValue(FilterValueBase value)
        {
            BoolFilterValue boolFilterValue = value as BoolFilterValue;
            if (boolFilterValue == null)
            {
                throw new ArgumentException("Does not match expected type: " + 
                                            typeof(BoolFilterValue).Name, 
                                            nameof(value));
            }

            boolFilterValue.Value = false;
        }
    }

    /// <summary>
    /// Represents an independent search definition.
    /// </summary>
    public class SCAnimalsSearchDefinition : SearchDefinition
    {
        internal static readonly AnimalActivityFilter ActivityFilter = new AnimalActivityFilter() { Name = "Animal activity" };
        private static Guid IdFamilyFilter = new Guid("F602D245-1035-479A-B09A-9D9E97BE53EB");
        internal static readonly SCAnimalsSearchFilter FamilyFilter = new SCAnimalsSearchFilter { Id = IdFamilyFilter, Name = "Animal family" };
        private static Guid IdSpeciesFilter = new Guid("B3DE429B-50BC-44E0-AF21-F879D4EFDC00");
        internal static readonly SCAnimalsSearchFilter SpeciesFilter = new SCAnimalsSearchFilter { Id = IdSpeciesFilter, Name = "Animal species" };
        private static Guid IdAreaFilter = new Guid("97B9F84D-7E89-414C-8E4B-0C30EEE6FC2C");
        internal static readonly SCAnimalsSearchFilter AreaFilter = new SCAnimalsSearchFilter { Id = IdAreaFilter, Name = "Area to observe" };

        private string[] _mammals = { "Lion", "Elephant", "Giraffe", "Zebra", "Rhino" };
        private string[] _reptiles = { "Crocodile" };

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="searchScope">Search scope.</param>
        public SCAnimalsSearchDefinition(SearchScope searchScope) : base(searchScope)
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
        /// <param name="searchInput">Search input containing the input to the search such as (SessionId, From, To, Items, and SortOrder)</param>
        /// <param name="cancellationToken">
        /// Cancellation token which can be used to cancel an ongoing search (if the implementation allows it).
        /// </param>
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
            // This implementation is for demo purposes only: creates dummy search result per camera.
            // Respecting the provided sort order(i.e.delivering results in that order) can help reduce "flickering results" in the results
            // list in Smart Client. Since this sample is based on random trigger times, respecting the sort order does not make much sense.
            // If however you retrieve results from a database or similar, it is very much encouraged to respect the provided sort order.

            var totalSpan = (to - from).TotalMilliseconds;
            var ordinal = 0;
            var speciesFilterValue = (StringFilterValue)SearchCriteria.GetFilterValues(SpeciesFilter).FirstOrDefault();
            var activityFilterValue = (BoolFilterValue)SearchCriteria.GetFilterValues(ActivityFilter).FirstOrDefault();
            var familyFilterValue = (SelectionFilterValue)SearchCriteria.GetFilterValues(FamilyFilter).FirstOrDefault();

            // If family is selected, get the species in that family
            List<string> speciesList;
            if (familyFilterValue != null)
                speciesList = GetSpeciesForSelectedFamilies(familyFilterValue.SelectedIds);
            else
                speciesList = GetAllSpecies();

            // If a specific species is selected, verify that it is contained by the available species
            if (speciesFilterValue != null && !speciesList.Contains(speciesFilterValue.Text))
                return;

            var rnd = new Random(37);
            foreach (Item camera in items)
            {
                var triggerTime = totalSpan * rnd.NextDouble();
                const string title = "Animal found!";
                var endTime = from.AddMilliseconds(triggerTime + totalSpan / 5);
                var id = GuidUtil.Create(SCAnimalsSearchAgentPlugin.PluginId,
                    title,
                    camera.FQID.ObjectId.ToString(),
                    from.AddMilliseconds(triggerTime).Ticks.ToString(CultureInfo.InvariantCulture),
                    endTime.Ticks.ToString(CultureInfo.InvariantCulture));
                var result = new AnimalsSearchResultData(id)
                {
                    Title = title,
                    Item = camera,
                    RelatedItems = null,
                    BeginTime = from.AddMilliseconds(triggerTime - totalSpan / 10),
                    TriggerTime = from.AddMilliseconds(triggerTime),
                    EndTime = endTime,
                    IsAnimalEating = activityFilterValue != null ? activityFilterValue.Value : rnd.Next(0, 2) == 1,
                    Species = speciesFilterValue != null ? speciesFilterValue.Text : speciesList[rnd.Next(0, speciesList.Count)],
                    Ordinal = ordinal++,
                };
                FireSearchResultsReadyEvent(sessionId, new[] { result });
            }
        }

        private List<string> GetAllSpecies()
        {
            return GetSpeciesForSelectedFamilies(new Guid[] { SCAnimalsSearchFilter.GuidFilterMammals, SCAnimalsSearchFilter.GuidFilterReptiles });
        }

        private List<string> GetSpeciesForSelectedFamilies(IEnumerable<Guid> familyIds)
        {
            List<string> selectedSpecies = new List<string>();
            if (familyIds.Contains(SCAnimalsSearchFilter.GuidFilterMammals))
                selectedSpecies.AddRange(_mammals);

            if (familyIds.Contains(SCAnimalsSearchFilter.GuidFilterReptiles))
                selectedSpecies.AddRange(_reptiles);

            return selectedSpecies;
        }
    }

    public class AnimalsSearchResultData : SearchResultData
    {
        public static Guid AnimalsResultType = new Guid("82077A43-5DA8-4206-934A-6FDF6F90DCDB");
        public AnimalsSearchResultData(Guid id) : base(id)
        {
            SearchResultUserControlType = AnimalsResultType;
        }

        public int Ordinal { get; set; }
        public string Species { get; set; }
        public bool IsAnimalEating { get; set; }

        protected override Task<ICollection<ResultProperty>> GetPropertiesAsync(CancellationToken token)
        {
            var props = new Collection<ResultProperty>()
            {
                new ResultProperty("Ordinal", Ordinal.ToString()),
                new ResultProperty("Species", Species),
                new ResultProperty("Animal activity", IsAnimalEating ? "Eating": "Not eating")
            };
            return Task.FromResult<ICollection<ResultProperty>>(props);
        }
    }

}
