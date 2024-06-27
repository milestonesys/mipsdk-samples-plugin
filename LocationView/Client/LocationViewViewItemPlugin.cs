using System;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI.Controls;

namespace LocationView.Client
{
    public class LocationViewViewItemPlugin 
        : ViewItemPlugin
    {
        public override Guid Id
        {
            get { return new Guid("05C1DB06-792A-44F4-A4B4-55A40EA9F7C9"); }
        }

        public override VideoOSIconSourceBase IconSource { get => LocationViewDefinition.TreeNodeImage; protected set => base.IconSource = value; }

        public override string Name
        {
            get { return "LocationView"; }
        }

        public override ViewItemManager GenerateViewItemManager()
        {
            return new LocationViewViewItemManager();
        }

        public override void Init()
        {			
        }

        public override void Close()
        {
        }
    }
}
