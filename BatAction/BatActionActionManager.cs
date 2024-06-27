using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using VideoOS.Platform;
using VideoOS.Platform.Data;
using VideoOS.Platform.RuleAction;

namespace BatAction
{
	public class BatActionActionManager : ActionManager
	{
		private Guid ExecuteActionItemId = new Guid("E12278C6-B0C0-4F51-BF1F-544DD7EE2F87");
        private Guid ExecuteActionItemId2 = new Guid("E12278C6-B0C0-4F51-BF1F-544DD7EE2F98");

        public override Collection<ActionDefinition> GetActionDefinitions()
		{
			// Expose supported actions here
			return new Collection<ActionDefinition>()
			{
				new ActionDefinition()
				{
					Id = ExecuteActionItemId,
					Name = "Execute a BAT file",
					SelectionText = "Execute <*.BAT>",
					DescriptionText = "Execute {0}",
					ActionItemKind = new ActionElement()
					{
						DefaultText = "BAT File",
						ItemKinds = new Collection<Guid>()
						{
							BatActionDefinition.BatActionKind
						}
					}
				},
                new ActionDefinition()
                {
                    Id = ExecuteActionItemId2,
                    Name = "Send camera id to bat file as parameter.",
                    SelectionText = "Execute <*.BAT> with camera as parameter",
                    DescriptionText = "Use {0} as a parameter to execute {1}",
                    ActionItemKind = new ActionElement()
                    {
                        DefaultText = "Camera",
                        ItemKinds = new Collection<Guid>()
                        {
                            Kind.Camera
                        }
                    },
                    ActionItemKind2 = new ActionElement()
                    {
                        DefaultText = "BAT File",
                        ItemKinds = new Collection<Guid>()
                        {
                            BatActionDefinition.BatActionKind
                        }
                    }

                },
            };

		}

		public override void ExecuteAction(Guid actionId, Collection<FQID> actionItems, BaseEvent sourceEvent)
		{
			if (actionId == ExecuteActionItemId)
			{
				String dllFileName = Assembly.GetExecutingAssembly().Location;
				String path = Path.GetDirectoryName(dllFileName);
				String batFolder = Path.Combine(path, "BatFiles");

				string filename = "";
				string parms = "";
				try
				{
					if (sourceEvent != null && sourceEvent.EventHeader!=null && sourceEvent.EventHeader.Source!=null && sourceEvent.EventHeader.Source.FQID!=null)
					{
						String sourceType = (Kind.DefaultTypeToNameTable[sourceEvent.EventHeader.Source.FQID.Kind] as String) ?? "";
						EnvironmentManager.Instance.Log(false,"BatAction.Kind",""+sourceType);
						parms = sourceEvent.EventHeader.Source.FQID.ObjectId + " \"" + sourceEvent.EventHeader.Source.Name + "\" " + sourceType;
					}
					if (actionItems == null)
					{
						EnvironmentManager.Instance.Log(false, "BatAction:ExecuteAction", "ActionItems is null ??:");
						return;
					}
					foreach (FQID fqid in actionItems)
					{
						if (String.IsNullOrEmpty(fqid.ObjectIdString))
						{
							foreach (KeyValuePair<String, Guid> entry in BatActionDefinition.FileGuidIndex)
							{
								if (entry.Value == fqid.ObjectId)
								{
									filename = entry.Key;
									break;
								}
							}
						}
						else
						{
							filename = fqid.ObjectIdString;
						}
						if (String.IsNullOrEmpty(filename))
						{
							EnvironmentManager.Instance.Log(true, "BatAction.Execute","Command not executed, filename not found");
							return;
						}
						String fullname = Path.Combine(batFolder, filename);
						EnvironmentManager.Instance.Log(false,"BatAction","Starting: "+fullname);
						Process process = new Process();
						process.StartInfo = new ProcessStartInfo(fullname, parms);
						process.Start();
					} 
				}
				catch (Exception ex)
				{
					EnvironmentManager.Instance.Log(true, "BatAction:ActionManager",
						"Unable to execute " + filename + " - " + ex.Message);
				}
			}
		}

        public override void ExecuteAction2(Guid actionId, Collection<FQID> actionItems, Collection<FQID> actionItems2, BaseEvent sourceEvent)
        {
            if (actionId == ExecuteActionItemId2)
            {
                try
                {
                    EnvironmentManager.Instance.Log(false, "BatActionManager", "ExecuteAction2 Triggered");
                    foreach (FQID fqid in actionItems)
                    {
                        Item item = Configuration.Instance.GetItem(fqid);
                        if (item != null)
                        {
                            EnvironmentManager.Instance.Log(false, "BatActionManager", "--- item1: " + item.Name);
                        }
                    }
                    foreach (FQID fqid in actionItems2)
                    {
                        Item item = Configuration.Instance.GetItem(fqid);
                        if (item != null)
                        {
                            EnvironmentManager.Instance.Log(false, "BatActionManager", "--- item2: " + item.Name);
                        }
                    }
                }
                catch (Exception ex)
                {
                    EnvironmentManager.Instance.Log(true, "BatAction2:ActionManager",
                        "ExecuteAction2 Logging - " + ex.Message);
                }

                String dllFileName = Assembly.GetExecutingAssembly().Location;
                String path = Path.GetDirectoryName(dllFileName);
                String batFolder = Path.Combine(path, "BatFiles");

                string filename = "";
                string parms = "";
                string parms2 = "";
                try
                {
                    if (sourceEvent != null && sourceEvent.EventHeader != null && sourceEvent.EventHeader.Source != null && sourceEvent.EventHeader.Source.FQID != null)
                    {
                        String sourceType = (Kind.DefaultTypeToNameTable[sourceEvent.EventHeader.Source.FQID.Kind] as String) ?? "";
                        EnvironmentManager.Instance.Log(false, "BatAction2.Kind", "" + sourceType);
                        parms = sourceEvent.EventHeader.Source.FQID.ObjectId + " \"" + sourceEvent.EventHeader.Source.Name + "\" " + sourceType;
                    }
                    if (actionItems == null)
                    {
                        EnvironmentManager.Instance.Log(false, "BatAction2:ExecuteAction", "ActionItems is null ??:");
                        return;
                    }

                    foreach (FQID fqid in actionItems)
                    {
                        Item item = Configuration.Instance.GetItem(fqid);
                        if (item == null)
                            parms2 += " \"Item not found:" + fqid.ObjectId + "\"";
                        else
                            parms2 += " \"Item=" + item.FQID.ObjectId + " Name=" + item.Name + "\"" ;
                    }

                    foreach (FQID fqid in actionItems2)
                    {
                        if (String.IsNullOrEmpty(fqid.ObjectIdString))
                        {
                            foreach (KeyValuePair<String, Guid> entry in BatActionDefinition.FileGuidIndex)
                            {
                                if (entry.Value == fqid.ObjectId)
                                {
                                    filename = entry.Key;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            filename = fqid.ObjectIdString;
                        }
                        if (String.IsNullOrEmpty(filename))
                        {
                            EnvironmentManager.Instance.Log(true, "BatAction2.Execute", "Command not executed, filename not found");
                            return;
                        }
                        String fullname = Path.Combine(batFolder, filename);
                        EnvironmentManager.Instance.Log(false, "BatAction2", "Starting: " + fullname);
                        Process process = new Process();
                        process.StartInfo = new ProcessStartInfo(fullname, parms + parms2);
                        process.Start();
                    }
                }
                catch (Exception ex)
                {
                    EnvironmentManager.Instance.Log(true, "BatAction2:ActionManager",
                        "Unable to execute " + filename + " - " + ex.Message);
                }

            }
        }

    }
}
