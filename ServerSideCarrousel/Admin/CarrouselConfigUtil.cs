using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using VideoOS.Platform;

namespace ServerSideCarrousel.Admin
{
    internal delegate void CarrouselHandlerDelegate(List<CarrouselTreeNode> list);

    internal class CarrouselConfigUtil
    {
        /// <summary>
        /// </summary>
        /// <param name="node"></param>
        /// <param name="carrouselHandler"></param>
        internal static void BuildCarrouselList(Item item, CarrouselHandlerDelegate carrouselHandler)
        {
            if (item.Properties.ContainsKey("SelectedDevices"))
            {
                string selectedList = item.Properties["SelectedDevices"];
                XmlDocument doc = new XmlDocument();
                doc.InnerXml = selectedList;
                List<CarrouselTreeNode> list = new List<CarrouselTreeNode>();
                XmlNode selectedNode = doc.SelectSingleNode("SelectedDevices");
                foreach (XmlNode itemNode in selectedNode.SelectNodes("Item"))
                {
                    FQID fqiditem = new FQID(itemNode.SelectSingleNode("FQID"));
                    int seconds = 10;
                    int sortix = 1;
                    int.TryParse(itemNode.SelectSingleNode("Seconds").InnerText, out seconds);
                    int.TryParse(itemNode.SelectSingleNode("Sort").InnerText, out sortix);

                    //Call will communicate with service, this should be called on another thread than the UI thread
                    Item cameraItem = Configuration.Instance.GetItem(fqiditem);

                    if (cameraItem != null) // Ignore items the user has no access to, or has been disabled/deleted
                    {
                        int listIndex = 0;
                        foreach (var existingItem in list)
                        {
                            if (existingItem.Sortix > sortix)
                                break;
                            listIndex++;
                        }
                        list.Insert(listIndex, new CarrouselTreeNode(cameraItem, seconds, sortix));
                    }
                    
                }
                carrouselHandler(list);
            }
        }
    }
}
