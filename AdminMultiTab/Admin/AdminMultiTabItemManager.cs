using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using VideoOS.Platform;
using VideoOS.Platform.Admin;

namespace AdminMultiTab.Admin
{
    /// <summary>
    /// The ItemManager creates and handles the usercontrols for the plugin.
    /// </summary>
    public class AdminMultiTabItemManager : ItemManager
    {        
        private FQID _fqid = new FQID(
            new ServerId("Tab", "nohost", 0, AdminMultiTabDefinition.AdminMultiTabPluginId),
            Guid.Empty,
            AdminMultiTabDefinition.AdminMultiTabItemId,
            FolderType.No,
            AdminMultiTabDefinition.AdminMultiTabKind);
        
        private List<IPropertyTab> _myControls;

        #region Constructors       

        public AdminMultiTabItemManager() : base()
        {    
        }

        /// <summary>
        /// Is called when the Environment is initializing, if the item for this plugin
        /// does not exist we create it.
        /// </summary>
        public override void Init()
        {
            if (CurrentItem == null)
            {
                Item item = Configuration.Instance.GetItemConfiguration(
                _fqid.ServerId.Id,
                _fqid.Kind,
                _fqid.ObjectId);
                if (item == null)
                {
                    item = new Item(_fqid, "Admin Multi Tab");
                }
                CurrentItem = item;
            }
        }

        /// <summary>
        /// Close is called when server is changing or application is closing down.
        /// </summary>
        public override void Close()
        {
        }
        
        #endregion

        #region UserControl Methods

        /// <summary>
        /// Don't generate just one UserControl, return null.
        /// </summary>
        /// <returns></returns>
        public override UserControl GenerateDetailUserControl()
        {            
            return null;
        }

        /// <summary>
        /// Generate multiple user controls, setup event handler
        /// </summary>
        /// <returns></returns>
        public override List<DetailedUserControl> GenerateDetailUserControlList()
        {
            _myControls = new List<IPropertyTab>();

            _myControls.Add(new AdminFirstTab());
            _myControls.Add(new AdminSecondTab());

            foreach (var control in _myControls)
            {
                control.ConfigurationChangedByUser += new EventHandler(ConfigurationChangedByUserHandler);
            }

            return _myControls.Cast<DetailedUserControl>().ToList();
        }


        /// <summary>
        /// UserControls are no longer being used, and related resources can be released.
        /// </summary>
        public override void ReleaseUserControl()
        {
            foreach (var control in _myControls)
            {
                control.ConfigurationChangedByUser -= new EventHandler(ConfigurationChangedByUserHandler); 
                ((DetailedUserControl)control).Dispose();
            }
            
        }

        /// <summary>
        /// Clear the contents of the user controls
        /// </summary>
        public override void ClearUserControl()
        {
            foreach (var control in _myControls)
            {
                control.ClearContent();
            }
        }

        /// <summary>
        /// Fill user controls with the content of the Item or the data it represent iterating through the controls used.
        /// </summary>
        /// <param name="item">The Item to work with</param>
        public override void FillUserControl(Item item)
        {
            foreach (var control in _myControls)
            {
                control.FillContent(item);
            }
        }

        #endregion

        #region Working with currentItem

        /// <summary>
        /// Validate the user entry, here we return true for OK always.<br/>        
        /// </summary>
        /// <returns>Indicates error in user entry.  True is a valid entry</returns>
        public override bool ValidateAndSaveUserControl()
        {
            foreach (var control in _myControls)
            {
                control.StorePropertiesInItem(CurrentItem);
            }
            Configuration.Instance.SaveItemConfiguration(AdminMultiTabDefinition.AdminMultiTabPluginId, CurrentItem);        
            return true;
        }
        #endregion      
    }
}

