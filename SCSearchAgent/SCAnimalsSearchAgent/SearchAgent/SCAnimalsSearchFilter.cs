using SCSearchAgent.SCAnimalsSearchAgent.SearchUserControl;
using System;
using System.Linq;
using VideoOS.Platform.Search;
using VideoOS.Platform.Search.FilterConfigurations;
using VideoOS.Platform.Search.FilterValues;

namespace SCSearchAgent.SCAnimalsSearchAgent.SearchAgent
{
    /// <summary>
    /// Represents a single searchable filter.
    /// </summary>
    public class SCAnimalsSearchFilter : SearchFilter
    {
        public static readonly Guid GuidFilterMammals = Guid.NewGuid();
        public static readonly Guid GuidFilterReptiles = Guid.NewGuid();

        public SCAnimalsSearchFilter()
        {
            Id = new Guid("6745AF55-8232-4858-B6F9-5C5360651C96"); ;
        }

        /// <summary>
        /// Creates a filter value for the search filter.
        /// </summary>
        /// <returns>Search filter value for the search filter.</returns>
        public override FilterValueBase CreateValue()
        {
            FilterValueBase value = null;

            if (this == SCAnimalsSearchDefinition.FamilyFilter)
            {
                value = new SelectionFilterValue();
            }
            else if (this == SCAnimalsSearchDefinition.SpeciesFilter)
            {
                value = new StringFilterValue();
            }
            else if(this == SCAnimalsSearchDefinition.AreaFilter)
            {
                value = new StringFilterValue();
            }

            if (value != null)
            {
                ResetValue(value);
            }

            return value;
        }

        /// <summary>
        /// Resets a filter value for the search filter by applying default values.
        /// </summary>
        /// <param name="value">The filter value to reset</param>
        public override void ResetValue(FilterValueBase value)
        {
            if (this == SCAnimalsSearchDefinition.FamilyFilter)
            {
                SelectionFilterValue selectionFilterValue = value as SelectionFilterValue;
                if (selectionFilterValue == null)
                {
                    throw new ArgumentException("Does not match expected type: " + 
                                                typeof(SelectionFilterValue).Name, 
                                                nameof(value));
                }
                selectionFilterValue.SetSelectedIds(Enumerable.Empty<Guid>());
            }
            else if (this == SCAnimalsSearchDefinition.SpeciesFilter)
            {
                StringFilterValue stringFilterValue = value as StringFilterValue;
                if (stringFilterValue == null)
                {
                    throw new ArgumentException("Does not match expected type: " + 
                                                typeof(StringFilterValue).Name, 
                                                nameof(value));
                }
                ((StringFilterValue)value).Text = "Any";
            }
            else if(this == SCAnimalsSearchDefinition.AreaFilter)
            {
                ((StringFilterValue)value).Text = "Custom area";
            }
        }

        /// <summary>
        /// Must return a boolean value indicating whether the provided value is "empty" or not. Empty values are still shown in the UI
        /// but are not included in the returned value from a call to <see cref="SearchCriteria.GetFilterValues(VideoOS.Platform.Search.SearchFilter)"/>.
        /// </summary>
        /// <param name="value">The filter value to check whether it is "empty" or not.</param>
        /// <returns>True if the filter value is "empty", false otherwise.</returns>
        public override bool IsEmptyValue(FilterValueBase value)
        {
            if (this == SCAnimalsSearchDefinition.SpeciesFilter)
            {
                StringFilterValue stringFilterValue = value as StringFilterValue;
                if (stringFilterValue != null)
                    return stringFilterValue.Text == "Any"; // Interpret the "Any" value as an empty value
            }
            // The base implementation handles all the built-in types, so for all filter values you don't explicitly handle, you should return the
            // return value from the base implementation.
            return base.IsEmptyValue(value);
        }
        
        /// <summary>
        /// Gets the configuration for the search filter. This is used to instantiate
        /// an appropriate edit control to edit the value of the filter.
        /// </summary>
        /// <returns>
        /// Filter configuration specifying details for the edit control used to edit
        /// the value of the filter.
        /// </returns>
        public override FilterConfigurationBase GetFilterConfiguration()
        {
            if (this == SCAnimalsSearchDefinition.FamilyFilter)
            {
                var cfg = new ListSelectionFilterConfiguration();
                // items could be added using .Add method...
                cfg.Items.Add(GuidFilterMammals, "Mammals");
                // ... or using indexer
                cfg.Items[GuidFilterReptiles] = "Reptiles";
                return cfg;
            }
            else if (this == SCAnimalsSearchDefinition.SpeciesFilter)
            {
                return new SCAnimalsFilterConfiguration() { DisplayMode = EditControlDisplayMode.SnapToParentWidth };
            }
            else if (this == SCAnimalsSearchDefinition.AreaFilter)
            {
                return new SCAnimalsFilterConfiguration() { DisplayMode = EditControlDisplayMode.InDialog };
            }

            return default(FilterConfigurationBase);
        }
    }
}
