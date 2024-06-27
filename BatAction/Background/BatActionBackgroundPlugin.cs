using System;
using System.Collections.Generic;
using VideoOS.Platform;
using VideoOS.Platform.Background;

namespace BatAction.Background
{
	/// <summary>
	/// This class is just here, to have the Event Server accepting the Plugin !
	/// </summary>
	public class BatActionBackgroundPlugin : BackgroundPlugin
	{
		/// <summary>
		/// </summary>
		public override Guid Id
		{
			get { return BatActionDefinition.BatActionBackgroundPlugin; }
		}

		/// <summary>
		/// </summary>
		public override String Name
		{
			get { return "BatAction BackgroundPlugin"; }
		}

		/// <summary>
		/// </summary>
		public override void Init()
		{
		}

		/// <summary>
		/// </summary>
		public override void Close()
		{
		}

		/// <summary>
		/// Define in what Environments the current background task should be started.
		/// </summary>
		public override List<EnvironmentType> TargetEnvironments
		{
			get { return new List<EnvironmentType>() { EnvironmentType.Service }; }	// Default will run in the Event Server
		}

	}
}
