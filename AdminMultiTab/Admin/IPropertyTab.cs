using System;
using VideoOS.Platform;

namespace AdminMultiTab.Admin
{
   public  interface IPropertyTab
    {
        event EventHandler ConfigurationChangedByUser;
        void StorePropertiesInItem(Item item);
        void FillContent(Item item);
        void ClearContent();
    }
}
