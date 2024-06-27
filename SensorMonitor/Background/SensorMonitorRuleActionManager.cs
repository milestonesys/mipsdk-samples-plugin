using System;
using System.Collections.ObjectModel;
using System.Linq;
using VideoOS.Platform;
using VideoOS.Platform.Data;
using VideoOS.Platform.RuleAction;

namespace SensorMonitor.Background
{
    /// <summary>
    /// This class exposes all available actions, and a method for executing these actions.
    /// </summary>
    public class SensorMonitorRuleActionActionManager : ActionManager
    {
        private static Guid ExecuteActionItemId = new Guid("4D3FAE1F-18E2-4917-A828-49FE3F8C0728");

        /// <summary>
        /// Executes the specified action on the specified items.
        /// </summary>
        /// <param name="actionId">Id of the action</param>
        /// <param name="actionItems">Item ids, on which to execute the action</param>
        /// <param name="sourceEvent">The event, which caused the action to be triggered</param>
        public override void ExecuteAction(Guid actionId, Collection<FQID> actionItems, BaseEvent sourceEvent)
        {
            if (actionId == ExecuteActionItemId)
            {
                foreach (FQID fqid in actionItems)
                {
                    // Execute action
                    Item item = Configuration.Instance.GetItemConfiguration(SensorMonitorDefinition.SensorMonitorPluginId, SensorMonitorDefinition.SensorMonitorCtrlKind, fqid.ObjectId);
                    EnvironmentManager.Instance.Log(false, nameof(ExecuteAction), "Start controller rule action called for: " + item.Name);
                    // either signal background plugin to send command on already established channel or simply do it here yourself
                    // SendStartSignal(item.Properties["IPAddress"]);
                }
            }
        }

        /// <summary>
        /// Lists the available actions
        /// </summary>
        /// <returns></returns>
        public override Collection<ActionDefinition> GetActionDefinitions()
        {
            // Expose supported actions here
            return new Collection<ActionDefinition>()
            {
                new ActionDefinition()
                {
                    Id = ExecuteActionItemId,
                    Name = "Start controller",
                    SelectionText = "Start <controller>",
                    DescriptionText = "Start {0}",
                    ActionItemKind = new ActionElement()
                        {
                            DefaultText = "controller",
                            ItemKinds = new Collection<Guid>() { SensorMonitorDefinition.SensorMonitorCtrlKind }
                        }
                }
            };
        }
    }
}
