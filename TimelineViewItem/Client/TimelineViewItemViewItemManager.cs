using System.Collections.Generic;
using VideoOS.Platform.Client;

namespace TimelineViewItem.Client
{
    /// <summary>
    /// The ViewItemManager contains the configuration for the ViewItem.
    /// When the class is initiated it will automatically recreate relevant ViewItem configuration saved in the properties collection from earlier.
    /// Also, when the viewlayout is saved the ViewItemManager will supply current configuration to the SmartClient to be saved on the server.
    /// This class is only relevant when executing in the Smart Client.
    /// </summary>
    public class TimelineViewItemViewItemManager : ViewItemManager
    {
        private List<TimelineSequenceSource> _timelineSequenceSources = new List<TimelineSequenceSource> { new TimelineRibbonSource(), new TimelineMarkerSource() };

        public TimelineViewItemViewItemManager() : base("TimelineViewItemViewItemManager")
        {
        }

        #region Methods overridden 

        /// <summary>
        /// Generate the UserControl containing the Actual ViewItem Content
        /// </summary>
        public override ViewItemWpfUserControl GenerateViewItemWpfUserControl()
        {
            return new TimelineViewItemViewItemWpfUserControl(this);
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
                // This will be shown as a header for the individual timeline so provide a name that describes the source
                return "Timeline sample";
            }
        }

        #endregion

    }
}
