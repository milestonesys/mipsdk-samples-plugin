using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Platform.Search;
using VideoOS.Platform.Search.FilterConfigurations;
using VideoOS.Platform.Search.FilterValues;

namespace SCSearchAgent.SCPeopleWithAccessoriesSearchAgent.SearchAgent
{
    /// <summary>
    /// Represents a single searchable filter.
    /// </summary>
    public class SCPeopleWithAccessoriesSearchFilter : SearchFilter
    {
        /// <summary>
        /// A dictionary that contains {key,value} pairs of a unique id and an accessory type that
        /// is used in the search filter.
        /// </summary>
        public readonly Dictionary<Guid, string> AccessoryTypes = new Dictionary<Guid, string>()
        {
            { new Guid("9BAD18B4-CCBF-446C-8E01-82044266753D"), "Umbrella" },
            { new Guid("767C37E8-063E-4D2E-9FB1-21165DFD397A"), "Cane" },
            { new Guid("D5D3E00F-F735-4196-A761-A58E694F1D13"), "Purse" },
            { new Guid("0BA8DD4E-2887-4586-87E8-F6AAEF75D8DE"), "Glasses" },
        };

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SCPeopleWithAccessoriesSearchFilter()
        {
            Id = new Guid("19EC6D07-DA15-477C-8AA0-B6B13E7D7C09");
        }

        /// <summary>
        /// Creates a filter value for the search filter.
        /// </summary>
        /// <returns>Search filter value for the search filter.</returns>
        public override FilterValueBase CreateValue()
        {
            FilterValueBase value = new SelectionFilterValue();
            ResetValue(value);
            return value;
        }

        /// <summary>
        /// Resets a filter value for the search filter by applying default values.
        /// </summary>
        /// <param name="value">The filter value to reset</param>
        public override void ResetValue(FilterValueBase value)
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
            var cfg = new ListSelectionFilterConfiguration();
            foreach (var accessoryType in AccessoryTypes)
                cfg.Items.Add(accessoryType.Key, accessoryType.Value);

            return cfg;
        }
    }
}
