using System;
using System.Collections.Generic;
using System.Xml;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace TimelineViewItem.Client
{
    /// <summary>
    /// The ViewItemManager contains the configuration for the ViewItem. <br/>
    /// When the class is initiated it will automatically recreate relevant ViewItem configuration saved in the properties collection from earlier.
    /// Also, when the viewlayout is saved the ViewItemManager will supply current configuration to the SmartClient to be saved on the server.<br/>
    /// This class is only relevant when executing in the Smart Client.
    /// </summary>
    public class TimelineViewItemViewItemManager : ViewItemManager
    {
        private Guid _someid;
        private string _someName;
        private List<Item> _configItems;
        private List<TimelineSequenceSource> _timelineSequenceSources = new List<TimelineSequenceSource> { new TimelineRibbonSource(), new TimelineMarkerSource() };

        public TimelineViewItemViewItemManager() : base("TimelineViewItemViewItemManager")
        {
        }

        #region Methods overridden 
        /// <summary>
        /// The properties for this ViewItem is now loaded into the base class and can be accessed via 
        /// GetProperty(key) and SetProperty(key,value) methods
        /// </summary>
        public override void PropertiesLoaded()
        {
            String someid = GetProperty("SelectedGUID");
            _configItems = Configuration.Instance.GetItemConfigurations(TimelineViewItemDefinition.TimelineViewItemPluginId, null, TimelineViewItemDefinition.TimelineViewItemKind);
            if (someid != null && _configItems != null)
            {
                SomeId = new Guid(someid);  // Set as last selected
            }
        }

        /// <summary>
        /// Generate the UserControl containing the Actual ViewItem Content
        /// </summary>
        /// <returns></returns>
        public override ViewItemUserControl GenerateViewItemUserControl()
        {
            return new TimelineViewItemViewItemUserControl(this);
        }

        /// <summary>
        /// Generate the UserControl containing the property configuration.
        /// </summary>
        /// <returns></returns>
        public override PropertiesUserControl GeneratePropertiesUserControl()
        {
            return new TimelineViewItemPropertiesUserControl(this);
        }

        public override IEnumerable<TimelineSequenceSource> TimelineSequenceSources
        {
            get
            {
                return _timelineSequenceSources;
            }
        }

        public override string DisplayName
        {
            get
            {
                // This is what will be shown as the header for the individual timeline so if possible provide a name that describes the source
                return "Timeline sample";
            }
        }

        #endregion

        public List<Item> ConfigItems
        {
            get { return _configItems; }
        }

        public Guid SomeId
        {
            get { return _someid; }
            set
            {
                _someid = value;
                SetProperty("SelectedGUID", _someid.ToString());
                if (_configItems != null)
                {
                    foreach (Item item in _configItems)
                    {
                        if (item.FQID.ObjectId == _someid)
                        {
                            SomeName = item.Name;
                        }
                    }
                }
                SaveProperties();
            }
        }

        public String SomeName
        {
            get { return _someName; }
            set { _someName = value; }
        }
    }
}
