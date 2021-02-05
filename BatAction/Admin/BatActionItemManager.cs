using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.AccessControl;
using VideoOS.Platform;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Messaging;

namespace BatAction.Admin
{
	/// <summary>
	/// </summary>
	public class BatActionItemManager : ItemManager
	{
		private bool _indexUnsaved = false;
		private object _msg = null;

		#region Constructors

		public BatActionItemManager() : base()
		{
		}

		/// <summary>
		/// Is called when the Environment is initializing, and will soon call GetItem methods
		/// IF you need to establish connection to a remote server, this is a good place to initialize.
		/// </summary>
		public override void Init()
		{
			_msg = EnvironmentManager.Instance.RegisterReceiver(PluginConfigurationChangedAfter, new MessageIdFilter(MessageId.System.SystemConfigurationChangedIndication));
		}

		/// <summary>
		/// Is called when server is changing or application is closing down.
		/// You should close any remote session you may have, and flush cache.
		/// </summary>
		public override void Close()
		{
		}

		#endregion

		#region Configuration Access Methods

		/// <summary>
		/// Returns a list of all Items of this Kind
		/// </summary>
		/// <returns>A list of items.  Allowed to return null if no Items found.</returns>
		public override List<Item> GetItems()
		{
			String dllFileName = Assembly.GetExecutingAssembly().Location;
			String path = Path.GetDirectoryName(dllFileName);
			String batFolder = Path.Combine(path, "BatFiles");
			List<Item> items = new List<Item>();
			if (Directory.Exists(batFolder))
			{
				LoadIndex();
				foreach (String file in Directory.GetFiles(batFolder))
				{
					String filename = Path.GetFileName(file);
					if (!BatActionDefinition.FileGuidIndex.ContainsKey(filename))
					{
						BatActionDefinition.FileGuidIndex[filename] = Guid.NewGuid();
						_indexUnsaved = true;
					}

					Guid objectId = BatActionDefinition.FileGuidIndex[filename];
					items.Add(new Item(new FQID(EnvironmentManager.Instance.MasterSite.ServerId, Guid.Empty, filename, FolderType.No, BatActionDefinition.BatActionKind) { ObjectId = objectId }, filename));
				}
				SaveIndex();
			}

			return items;
		}

		private object PluginConfigurationChangedAfter(Message message, FQID dest, FQID source)
		{
			EnvironmentManager.Instance.PostMessage(new Message(MessageId.Server.ConfigurationChangedIndication,
				new FQID(EnvironmentManager.Instance.MasterSite.ServerId, FolderType.No, BatActionDefinition.BatActionKind)));
			return null;
		}

		/// <summary>
		/// Returns a list of all Items from a specific server.
		/// </summary>
		/// <param name="parentItem">The parent Items</param>
		/// <returns>A list of items.  Allowed to return null if no Items found.</returns>
		public override List<Item> GetItems(Item parentItem)
		{
			EnvironmentManager.Instance.Log(false,"BatAction","ItemManager.GetItems( parent )");
			List<Item> items = GetItems();
			return items;
		}

		/// <summary>
		/// Returns the Item defined by the FQID. Will return null if not found.
		/// </summary>
		/// <param name="fqid">Fully Qualified ID of an Item</param>
		/// <returns>An Item</returns>
		public override Item GetItem(FQID fqid)
		{
			if (fqid.ObjectIdString != null)
			{
				LoadIndex();
				String filename = fqid.ObjectIdString;
				if (!BatActionDefinition.FileGuidIndex.ContainsKey(filename))
				{
					BatActionDefinition.FileGuidIndex[filename] = Guid.NewGuid();
					_indexUnsaved = true;
					SaveIndex();
				}

				Guid objectId = BatActionDefinition.FileGuidIndex[filename];
				return new Item(new FQID(EnvironmentManager.Instance.MasterSite.ServerId, Guid.Empty, filename, FolderType.No, BatActionDefinition.BatActionKind) { ObjectId = objectId }, filename);
            }
            else
            {
                foreach (var name in BatActionDefinition.FileGuidIndex.Keys)
                {
                    if (BatActionDefinition.FileGuidIndex[name] == fqid.ObjectId)
                    {
                        return new Item(new FQID(EnvironmentManager.Instance.MasterSite.ServerId, Guid.Empty, name, FolderType.No, BatActionDefinition.BatActionKind) { ObjectId = fqid.ObjectId }, name);
                    }
                }

            }
            return null;
		}


		private void LoadIndex()
		{
			try
			{
				String filename = "BatFiles.txt";
				String folder = "C:\\ProgramData";
				String app = "Milestone\\XProtect Event Server\\BatAction";
				String directory = Path.Combine(folder, app);
				String fullname = Path.Combine(directory, filename);
				if (File.Exists(fullname))
				{
					BatActionDefinition.FileGuidIndex.Clear();
					StreamReader sr = new StreamReader(fullname);
					while (sr.EndOfStream == false)
					{
						String line = sr.ReadLine();
						String[] parts = line.Split(';');
						BatActionDefinition.FileGuidIndex[parts[0]] = new Guid(parts[1]);
					}
					sr.Close();
				}

			}
			catch (Exception ex)
			{
				EnvironmentManager.Instance.ExceptionHandler("BatActionItemManager", "LoadIndex",ex);
			}
		}

		private void SaveIndex()
		{
			try
			{
				if (_indexUnsaved)
				{
					String filename = "BatFiles.txt";
					String eventFolder = "C:\\ProgramData\\Milestone\\XProtect Event Server";
					String directory = Path.Combine(eventFolder, "BatAction");
					String fullname = Path.Combine(directory, filename);
					if (Directory.Exists(directory) == false)
					{
						Directory.CreateDirectory(directory);
						DirectoryInfo info = new DirectoryInfo(directory);
						DirectorySecurity security = info.GetAccessControl();
						security.AddAccessRule(new FileSystemAccessRule(".\\Network Service", FileSystemRights.Modify, AccessControlType.Allow));
						info.SetAccessControl(security);
					}
					if (File.Exists(fullname))
						File.Delete(fullname);
					StreamWriter sw = new StreamWriter(fullname);
					foreach (string name in BatActionDefinition.FileGuidIndex.Keys)
					{
						String line = name + ";" + BatActionDefinition.FileGuidIndex[name];
						sw.WriteLine(line);
					}
					sw.Flush();
					sw.Close();
					_indexUnsaved = false;
				}
			}
			catch (Exception ex)
			{
				EnvironmentManager.Instance.ExceptionHandler("BatActionItemManager", "SaveIndex", ex);
			}			
		}

		#endregion

		#region Messages and Status

		/// <summary>
		/// Just before a context menu is displayed, each line on the context menu is checked for it should be enabled or disabled.
		/// This method is called with the following command (If allowed by the ItemNode definition)<br/>
		///   "ADD" - for the "Add new ..." <br/>
		///   "DELETE" - for the "Delete ..."<br/>
		///   "RENAME" - for rename<br/>
		/// If your plugin has the configuration stored on another server, and management is not possible
		/// via the ItemManager, then this method can be used to disable all contextmenu actions.
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public override bool IsContextMenuValid(string command)
		{
			return false;	
		}
		#endregion

	}
}

