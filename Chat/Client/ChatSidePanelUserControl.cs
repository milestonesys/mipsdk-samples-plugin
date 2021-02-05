using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Login;
using VideoOS.Platform.Messaging;

namespace Chat.Client
{
	/// <summary>
	/// </summary>
	public partial class ChatSidePanelUserControl : SidePanelUserControl
	{
		private object _obj1, _obj2, _obj3, _obj4;
		private string _myName = "";

		internal static string MessageIdChatLine = "Chat.Line";
		internal Collection<EndPointIdentityData> AllEndPoints = null;
		private MessageCommunication _messageCommunication;

		public ChatSidePanelUserControl()
		{
			InitializeComponent();
		}

		public override void Init()
		{
			try
			{
				MessageCommunicationManager.Start(EnvironmentManager.Instance.MasterSite.ServerId);
				_messageCommunication = MessageCommunicationManager.Get(EnvironmentManager.Instance.MasterSite.ServerId);

				_messageCommunication.ConnectionStateChangedEvent += new EventHandler(_messageCommunication_ConnectionStateChangedEvent);
				_obj1 = _messageCommunication.RegisterCommunicationFilter(WhoAreOnlineResponseHandler, new VideoOS.Platform.Messaging.CommunicationIdFilter(MessageCommunication.WhoAreOnlineResponse));
				_obj2 = _messageCommunication.RegisterCommunicationFilter(NewEndPointHandler, new VideoOS.Platform.Messaging.CommunicationIdFilter(MessageCommunication.NewEndPointIndication));
				_obj3 = _messageCommunication.RegisterCommunicationFilter(EndPointTableChangedHandler, new VideoOS.Platform.Messaging.CommunicationIdFilter(MessageCommunication.EndPointTableChangedIndication));
				_obj4 = _messageCommunication.RegisterCommunicationFilter(ChatLineHandler, new VideoOS.Platform.Messaging.CommunicationIdFilter(MessageIdChatLine));

				LoginSettings ls = LoginSettingsCache.GetLoginSettings(EnvironmentManager.Instance.MasterSite);
				_myName = ls.UserName;
				if (_myName.Contains("\\"))
				{
					_myName = _myName.Split('\\')[1];
				}
			    if (_messageCommunication.IsConnected)
			    {
                    _messageCommunication.TransmitMessage(
                        new VideoOS.Platform.Messaging.Message(MessageCommunication.WhoAreOnlineRequest), null, null,
                        null);			        
			    }
			} catch (Exception ex)
			{
				EnvironmentManager.Instance.ExceptionDialog("Chat.Init", ex);
			}
		}

		void _messageCommunication_ConnectionStateChangedEvent(object sender, EventArgs e)
		{
			if (_messageCommunication.IsConnected)
			{
                try
                {
                    _messageCommunication.TransmitMessage(
                        new VideoOS.Platform.Messaging.Message(MessageCommunication.WhoAreOnlineRequest), null, null,
                        null);
                } catch (Exception ex)
                {
                    MessageBox.Show("Chat: " + ex.Message);
                    //TODO: Retry later?
                }
			} else
			{
                BeginInvoke(new MethodInvoker(delegate() { this.Enabled = false; }));				
			}
		}

		public override void Close()
		{
            _messageCommunication.ConnectionStateChangedEvent -= new EventHandler(_messageCommunication_ConnectionStateChangedEvent);
            _messageCommunication.UnRegisterCommunicationFilter(_obj1);
			_messageCommunication.UnRegisterCommunicationFilter(_obj2);
			_messageCommunication.UnRegisterCommunicationFilter(_obj3);
			_messageCommunication.UnRegisterCommunicationFilter(_obj4);
			_obj1 = _obj2 = _obj3 = _obj4 = null;
		}

		private object ChatLineHandler(VideoOS.Platform.Messaging.Message message, FQID dest, FQID source)
		{
			if (InvokeRequired)
			{
				BeginInvoke(new MessageReceiver(ChatLineHandler), message, dest, source);
			}
			else
			{
				ChatData chatData = message.Data as ChatData;
				if (chatData != null)
				{
					listBoxChat.Items.Add(chatData.Source.IdentityName + ": " + chatData.Entry);
				}
			}
			return null;
		}

		private object WhoAreOnlineResponseHandler(VideoOS.Platform.Messaging.Message message, FQID dest, FQID source)
		{
			if (InvokeRequired)
			{
				BeginInvoke(new MessageReceiver(WhoAreOnlineResponseHandler), message, dest, source);
			}
			else
			{
				this.Enabled = true;
				Collection<EndPointIdentityData> result = message.Data as Collection<EndPointIdentityData>;
				AllEndPoints = result;
				if (result != null)
				{
					listBoxUsers.Items.Clear();
					foreach (EndPointIdentityData user in result)
					{
						AddUser(user);
					}
				}
			}
			return null;
		}

		private void AddUser(EndPointIdentityData endPointIdentityData)
		{
			string shortName = endPointIdentityData.IdentityName;
			if (shortName.Contains("\\"))
			{
				shortName = shortName.Split('\\')[1];
			}
			foreach (String name in listBoxUsers.Items)
			{
				// test for duplicate FQIDs ?
			}
			shortName += " (" + endPointIdentityData.EndPointFQID.ServerId.ServerType + ") ";
			listBoxUsers.Items.Add(shortName);
		}

		private object NewEndPointHandler(VideoOS.Platform.Messaging.Message message, FQID dest, FQID source)
		{
			EndPointIdentityData user = message.Data as EndPointIdentityData;
			if (user != null)
			{
                if (InvokeRequired)
    				BeginInvoke(new MethodInvoker(delegate() { AddUser(user); }));
                else
                    AddUser(user);
            }
			return null;
		}

		private object EndPointTableChangedHandler(VideoOS.Platform.Messaging.Message message, FQID dest, FQID source)
		{
			_messageCommunication.TransmitMessage(
				new VideoOS.Platform.Messaging.Message(MessageCommunication.WhoAreOnlineRequest), null, null, null);
			return null;
		}

		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				OnClickSend(sender, e);
				e.Handled = true;
				e.SuppressKeyPress = true;
			}
		}

		private void OnClickSend(object sender, EventArgs e)
		{
			ChatData data = new ChatData()
			                	{
			                		Source = new EndPointIdentityData()
			                		         	{
			                		         		IdentityName = _myName,
			                		         		EndPointFQID =MessageCommunicationManager.EndPointFQID
				},
				Entry = textBoxEntry.Text
			};
            try
            {
                _messageCommunication.TransmitMessage(new VideoOS.Platform.Messaging.Message(MessageIdChatLine, data), null, null, null);
            }
            catch (Exception)
            {
                textBoxEntry.Text += " (unable to send)";
            }
			textBoxEntry.Text = "";
		}

		private void OnPreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			e.IsInputKey = true;
		}
	}

	[Serializable]
	public class ChatData
	{
		public EndPointIdentityData Source;
		public String Entry;
	}
}
