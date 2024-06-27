using System;
using VideoOS.Platform.Search;

namespace SCSearchAgent.SCAnimalsSearchAgent.SearchToolbar
{
    /// <summary>
    /// Defines a plugin that resides in the action/preview area
    /// of the Search tab in Smart Client 
    /// </summary>
    public class SCAnimalsSearchToolbarPlugin : SearchToolbarPlugin
    {
        /// <summary>
        /// Gets the unique id identifying this plugin component
        /// </summary>
        public override Guid Id { get; protected set; } = new Guid("81ed29df-c463-4875-9380-abc1667d9a26");

        /// <summary>
        /// Plugin name
        /// </summary>
        public override string Name { get; protected set; } = "Register animals";
        
        /// <summary>
        /// This method is called when the user has logged in and configuration is accessible.
        /// If a user logs out and in again, this method will be called at every login. This
        /// should be used if the plugin is accessing configuration items. 
        /// </summary>
        public override void Init() { }

        /// <summary>
        /// This method is called by the Environment when the user logs off.
        /// </summary>
        public override void Close() { }

        /// <summary>
        /// Called for every instance of this toolbar plugin that is added to the UI.
        /// </summary>
        /// <returns>A search toolbar plugin instance</returns>
        public override SearchToolbarPluginInstance GenerateSearchToolbarPluginInstance()
        {
            // Sample code
            return new SCAnimalsSearchToolbarPluginInstance
            {
                Title = "Register animals",
                Tooltip = "Click to register animals in the database."
            };
        }
    }
}
