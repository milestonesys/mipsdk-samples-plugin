using AlarmPreview.Client;
using System;
using System.Drawing;
using System.Reflection;
using VideoOS.Platform;

namespace AlarmPreview
{
    public class AlarmPreviewDefinition : PluginDefinition
    {
        private static System.Drawing.Image _treeNodeImage;
        private static System.Drawing.Image _topTreeNodeImage;

        internal static Guid AlarmPreviewPluginId = new Guid("c306c510-82d2-4bb6-99a8-937ebc3307e4");

        #region Initialization
        /// <summary>
        /// Load resources 
        /// </summary>
        static AlarmPreviewDefinition()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string name = assembly.GetName().Name;

            System.IO.Stream pluginStream = assembly.GetManifestResourceStream(name + ".Resources.AlarmPreview.bmp");
            if (pluginStream != null)
                _treeNodeImage = System.Drawing.Image.FromStream(pluginStream);
            System.IO.Stream configStream = assembly.GetManifestResourceStream(name + ".Resources.Server.png");
            if (configStream != null)
                _topTreeNodeImage = System.Drawing.Image.FromStream(configStream);
        }

        /// <summary>
        /// Get the icon for the plugin
        /// </summary>
        internal static Image TreeNodeImage
        {
            get { return _treeNodeImage; }
        }

        #endregion

        /// <summary>
        /// This method is called when the environment is up and running.
        /// Registration of Messages via RegisterReceiver can be done at this point.
        /// </summary>
        public override void Init()
        {

        }

        /// <summary>
        /// The main application is about to be in an undetermined state, either logging off or exiting.
        /// You can release resources at this point, it should match what you acquired during Init, so additional call to Init() will work.
        /// </summary>
        public override void Close()
        {
        
        }
        
        public override Guid Id
        {
            get { return AlarmPreviewPluginId; }
        }

        public override Guid SharedNodeId
        {
            get { return Guid.Empty; }
        }
        
        public override string Name
        {
            get { return "AlarmPreview"; }
        }

        public override string Manufacturer
        {
            get { return PluginSamples.Common.ManufacturerName; }
        }

        public override string VersionString
        {
            get { return "2.0.0.0"; }
        }

        public override System.Drawing.Image Icon
        {
            get { return _topTreeNodeImage; }
        }

        /// <summary>
        /// Override GenerateAlarmPreviewUserControl causes the usercontrol to be called when
        /// the Smart Client user marks an Alarm line in the AlarmList control
        /// The usercontrol is then automatically included in the AlarmPreview control
        /// </summary>
        /// <param name="alarmOrBaseEvent"></param>
        /// <returns></returns>
        public override System.Windows.Controls.UserControl GenerateAlarmPreviewWpfUserControl(object alarmOrBaseEvent)
        {
            return new AlarmPreviewWpfUserControl(alarmOrBaseEvent);
        }
    }
}
