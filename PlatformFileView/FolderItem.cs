using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using PlatformFileView.WebService;
using VideoOS.Platform;

namespace PlatformFileView
{
	public class FolderItem : Item
	{
		/// <summary>
		/// Contains the ServerId and the path of a specific file.
		/// the ObjectIdString contains the full filename.
		/// </summary>
		public FolderItem(FQID fqid, String name) : base(fqid, name)
		{
		    String path = fqid.ObjectIdString;
			base.FQID.FolderType = fqid.Kind == PlatformFileViewDefinition.PlatformFolderKind ? FolderType.SystemDefined: FolderType.No;
		    if (path == null || path == "C:\\")
		    {
		        base.FQID.ObjectIdString = "C:\\";
		    }
		    else
		    {
                base.FQID.ObjectIdString = path;
                base.Name = System.IO.Path.GetFileName(base.FQID.ObjectIdString);		        
		    }
		}

		/// <summary>
		/// Place a request to get child items on a work thread.
		/// </summary>
		/// <param name="asyncItemsHandler"></param>
		/// <param name="control"></param>
		/// <param name="callerReference"></param>
		public override void GetChildrenAsync(AsyncItemsHandler asyncItemsHandler, Control control, object callerReference)
		{
			ThreadPool.UnsafeQueueUserWorkItem(RespondAsyncItems, new ThreadPoolContainer(asyncItemsHandler, control, callerReference));
		}

		/// <summary>
		/// The work thread will execute this method.
		/// Will call the getChildren from this thread and perform a call back with the result.
		/// </summary>
        /// <param name="threadPoolContainerParm"></param>
		private void RespondAsyncItems(object threadPoolContainerParm)
		{
			try
			{
				ThreadPoolContainer threadPoolContainer = (ThreadPoolContainer) threadPoolContainerParm;
				List<Item> items = GetChildren();

				if (threadPoolContainer.Control != null && threadPoolContainer.Control.IsHandleCreated)
					threadPoolContainer.Control.BeginInvoke(threadPoolContainer.AsyncItemsHandler,
					                                         new object[] {items, threadPoolContainer.CallerReference});
				else
					threadPoolContainer.AsyncItemsHandler(items, threadPoolContainer.CallerReference);
			}
			catch (Exception e)
			{
				EnvironmentManager.Instance.ExceptionDialog("GetChildrenAsync", e);
			}
		}

		/// <summary>
		/// Get all children from a specific Folder.
		/// </summary>
		/// <returns></returns>
		public override List<Item> GetChildren()
		{
			List<Item> children = new List<Item>();
			if (base.FQID.ObjectIdString!=null)
			{
				string[] folders = WebServiceManager.Instance.ClientProxy.GetFolders(base.FQID.ObjectIdString);
				if (folders!=null)
				foreach (string name in folders)
				{
					children.Add(new FolderItem(new FQID(FQID.ServerId, FQID.ParentId, name, FolderType.SystemDefined, PlatformFileViewDefinition.PlatformFolderKind),name));
				}
				string[] names = WebServiceManager.Instance.ClientProxy.GetFiles(base.FQID.ObjectIdString);
				if (names!=null)
				foreach (string name in names)
				{
					children.Add(new FolderItem(new FQID(FQID.ServerId, FQID.ParentId, name, FolderType.No, PlatformFileViewDefinition.PlatformFileKind), name));
				}
			}
			return children;
		}

		/// <summary>
		/// When the Kind is a PlatformFileViewKind, we have a folder that may contain files.
		/// Otherwise it is a single file with no children.
		/// </summary>
		public override HasChildren HasChildren
		{
			get
			{
				if (base.FQID.Kind == PlatformFileViewDefinition.PlatformFolderKind)
					return HasChildren.Maybe;
				return HasChildren.No;
			}
			set
			{
				base.HasChildren = value;
			}
		}
	}

	/// <summary>
	/// Contains information for executing the getChildren method.
	/// </summary>
	internal class ThreadPoolContainer
	{
		private Thread _callerThread;
		private object _callerReference;
		private AsyncItemsHandler _handler;
		private Control _control;

		internal ThreadPoolContainer(AsyncItemsHandler handler, Control control, object callerReference)
		{
			_callerThread = Thread.CurrentThread;
			_handler = handler;
			_callerReference = callerReference;
			_control = control;
		}

		internal Thread CallerThread { get { return _callerThread; } }
		internal object CallerReference { get { return _callerReference; } }
		internal AsyncItemsHandler AsyncItemsHandler { get { return _handler; } }
		internal Control Control { get { return _control; } }
	}
}
