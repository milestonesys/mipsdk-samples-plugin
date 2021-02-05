using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Messaging;

namespace SCViewAndWindow.Client
{
    public partial class MapSelectForm : Form
    {
        private TreeNode _selectedNode;
        private object _msgObject;
        private MessageCommunication _client;

        public String SelectedMapId
        {
            get { if (_selectedNode != null) return _selectedNode.Name;
                return "";
            }
        }
        public String SelectedMapName
        {
            get
            {
                if (_selectedNode != null) return _selectedNode.Text;
                return "";
            }
        }

        public MapSelectForm()
        {
            InitializeComponent();

            treeView1.Nodes.Clear();
            _selectedNode = treeView1.Nodes.Add("", "Top");

            try
            {
                MessageCommunicationManager.Start(EnvironmentManager.Instance.MasterSite.ServerId);
                _client = MessageCommunicationManager.Get(EnvironmentManager.Instance.MasterSite.ServerId);

                //System.Threading.Thread.Sleep(500);
                bool x = _client.IsConnected;
                _msgObject = _client.RegisterCommunicationFilter(MapResponseHandler,
                    new CommunicationIdFilter(MessageId.Server.GetMapResponse));

                MapRequestData data = new MapRequestData()
                {
                    MapGuid = "",
                };
                _client.TransmitMessage(new VideoOS.Platform.Messaging.Message(MessageId.Server.GetMapRequest, data),
                    null, null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Map Select issue:" + ex.Message);
            }
        }

        private object MapResponseHandler(VideoOS.Platform.Messaging.Message message, FQID to, FQID from)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MessageReceiver(MapResponseHandler), new object[] {message, to, from});
                return null;
            } else
            {
                MapResponseData data = message.Data as MapResponseData;
                if (data != null && _selectedNode != null)
                {
                    _selectedNode.Nodes.Clear();
                    if (String.IsNullOrEmpty(data.ErrorText))
                    {
                        foreach (var entry in data.MapCollection)
                        {
                            if (!entry.RecursiveMap)
                            {
                                TreeNode node = _selectedNode.Nodes.Add(entry.Id, entry.DisplayName);
                                node.Nodes.Add("Empty");
                            }
                        }
                        treeView1.Invalidate();
                    }
                    else
                    {
                        _selectedNode.Nodes.Add(data.ErrorText);
                    }
                }
                return null;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            _client.UnRegisterCommunicationFilter(_msgObject);
            Close();
        }

        private void OnBeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            _selectedNode = e.Node;
            if (_selectedNode != null)
            {
                if (_selectedNode.Nodes.Count == 1 && _selectedNode.Nodes[0].Text == "Empty")
                {
                    try
                    {
                        MapRequestData data = new MapRequestData()
                        {
                            MapGuid = SelectedMapId,
                        };
                        _client.TransmitMessage(
                            new VideoOS.Platform.Messaging.Message(MessageId.Server.GetMapRequest, data), null, null,
                            null);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Map Select issue:" + ex.Message);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            _client.UnRegisterCommunicationFilter(_msgObject);
            Close();
        }

        private void OnAfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
                _selectedNode = e.Node;
        }

    }
}
