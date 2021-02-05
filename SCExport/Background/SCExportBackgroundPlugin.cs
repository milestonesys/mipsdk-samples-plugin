using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml;
using SCExport.Client;
using VideoOS.Platform;
using VideoOS.Platform.Background;
using VideoOS.Platform.Client;

namespace SCExport.Background
{
	/// <summary>
	/// </summary>
	public class SCExportBackgroundPlugin : BackgroundPlugin
	{
		private bool _stop = false;
		private Thread _thread;


		VideoOS.Platform.Data.IExporter _exporter;
		private SCExportJob _currentJob;

		internal static SCExportSidePanelUserControl UserControl { get; set; }

		/// <summary>
		/// Gets the unique id identifying this plugin component
		/// </summary>
		public override Guid Id
		{
			get { return SCExportDefinition.SCExportBackgroundPlugin; }
		}

		/// <summary>
		/// The name of this background plugin
		/// </summary>
		public override String Name
		{
			get { return "SCExport BackgroundPlugin"; }
		}

		/// <summary>
		/// Called by the Environment when the user has logged in.
		/// </summary>
		public override void Init()
		{
			_stop = false;
			_thread = new Thread(new ThreadStart(Run));
			_thread.Start();
		}

		/// <summary>
		/// Called by the Environment when the user log's out.
		/// You should close all remote sessions and flush cache information, as the
		/// user might logon to another server next time.
		/// </summary>
		public override void Close()
		{
			_stop = true;
			if (_thread!=null)
			{
				_thread.Join();
			}
		}

		/// <summary>
		/// Define in what Environments the current background task should be started.
		/// </summary>
		public override List<EnvironmentType> TargetEnvironments
		{
			get { return new List<EnvironmentType>() { EnvironmentType.SmartClient }; }	
		}


		/// <summary>
		/// the thread doing the work
		/// </summary>
		private void Run()
		{
			EnvironmentManager.Instance.Log(false, "SCExport background thread", "Now starting...", null);
			while (!_stop)
			{
				_currentJob = null;
				string queueLength = "";
				lock (_exportJobs)
				{
					queueLength = "Queue: "+_exportJobs.Count;
					if (_exportJobs.Count>0)
					{
						_currentJob = _exportJobs[0];
						_exportJobs.Remove(_currentJob);
					}
				}
				if (_currentJob != null)
				{
					ShowStatus(queueLength + " - " + _currentJob, 0);

                    if (_currentJob.AVIexport)
					{
                        VideoOS.Platform.Data.AVIExporter aviExporter = new VideoOS.Platform.Data.AVIExporter() { Width = 320, Height = 240, Filename = _currentJob.FileName, AutoSplitExportFile = true, MaxAVIFileSize = 512 * 1024 * 1024 };   // 512 MB

                        if (_currentJob.OverlayImage != null)
                        {
                            if (aviExporter.SetOverlayImage(_currentJob.OverlayImage,
                                                        _currentJob.VerticalOverlayPosition,
                                                        _currentJob.HorizontalOverlayPosition,
                                                        _currentJob.ScaleFactor,
                                                        _currentJob.IgnoreAspect) == false)
                            {
                                ShowStatus("Error: " + aviExporter.LastErrorString, 0);
                            }
                        }

                        _exporter = aviExporter;
                        _exporter.AudioList = new List<Item>();
                        _exporter.Path = _currentJob.Path;
                        // comment the following if you want to avoid audio from a microphone _not_ connected to the camera.
						if (_currentJob.Item.HasRelated != HasRelated.No)
						{
						    List<Item> related = _currentJob.Item.GetRelated();
						    foreach (Item item in related)
						    {
						        if (item.FQID.Kind == Kind.Microphone)
						            _exporter.AudioList.Add(item);
						    }
						}
					} else
                    if (_currentJob.MKVexport)
                    {
                        _exporter = new VideoOS.Platform.Data.MKVExporter() { Filename = _currentJob.FileName };
                        _exporter.AudioList = new List<Item>();
                        _exporter.Path = _currentJob.Path;
                        // comment the following if you want to avoid audio from a microphone _not_ connected to the camera.
                        if (_currentJob.Item.HasRelated != HasRelated.No)
                        {
                            List<Item> related = _currentJob.Item.GetRelated();
                            foreach (Item item in related)
                            {
                                if (item.FQID.Kind == Kind.Microphone)
                                    _exporter.AudioList.Add(item);
                            }
                        }
                    }
                    else
					{
						_exporter = new VideoOS.Platform.Data.DBExporter() {Encryption = false};
                        _exporter.AudioList = new List<Item>();
                        _exporter.Path = Path.Combine(_currentJob.Path, _currentJob.FileName);
                        ((VideoOS.Platform.Data.DBExporter)_exporter).PreventReExport = _currentJob.PreventReExport;
                        ((VideoOS.Platform.Data.DBExporter)_exporter).SignExport = _currentJob.SignExport;

                        // If there is recorded audio from a microphone on the camera it will be in the export, as the Smart Client will 
                        // add audio spurces automatically for DB Exports
                    }
					_exporter.Init();
					_exporter.CameraList = new List<Item>() { _currentJob.Item };
					bool started = _exporter.StartExport(_currentJob.StartTime, _currentJob.EndTime);
					if (started)
					{
						while (_exporter.Progress == 0)
						{
							Thread.Sleep(100);
						}

						// Perhaps consider some cancel mechanism
						while (_exporter.Progress < 100 && _exporter.Progress > 0)
						{
							lock (_exportJobs)
							{
								queueLength = "Queue: " + _exportJobs.Count;
							}
							ShowStatus(queueLength + " - " + _currentJob, _exporter.Progress);
							Thread.Sleep(300);
						}
					}
					if (_exporter.LastError != 0)
					{
						ShowStatus("Error:" + _exporter.LastErrorString, 0);
						Thread.Sleep(5000);
					}

					_exporter.EndExport();
					_exporter.Close();
				}
                ShowStatus(queueLength, 0);
				Thread.Sleep(2000);
			}
			EnvironmentManager.Instance.Log(false, "SCExport background thread", "Now stopping...", null);
			_thread = null;
		}

		private void ShowStatus(string text, int percent)
		{
			if (UserControl!=null)
			{
				UserControl.ShowStatus(text, percent);
			}
		}

		private static List<SCExportJob> _exportJobs = new List<SCExportJob>();
		internal static void AddJob(SCExportJob job)
		{
			lock (_exportJobs)
			{
				_exportJobs.Add(job);
			}
		}
	}
}
