using DemoAccessControlPlugin.Client;
using DemoAccessControlPlugin.Constants;
using System;
using VideoOS.Platform.AccessControl.Elements;
using VideoOS.Platform.AccessControl.Plugin;

namespace DemoAccessControlPlugin.Managers
{
    /// <summary>
    /// The EventManager is responsible for calling FireEventsOccurred.
    /// </summary>
    internal class EventManager : ACEventManager
    {
        private readonly DemoClient _client;

        public EventManager(DemoClient client)
        {
            _client = client;
            _client.EventTriggered += _client_EventTriggered;
            _client.Connected += _client_Connected;
            _client.Disconnected += _client_Disconnected;
        }

        public void Close()
        {
            _client.EventTriggered -= _client_EventTriggered;
            _client.Connected -= _client_Connected;
            _client.Disconnected -= _client_Disconnected;
        }

        private void _client_EventTriggered(object sender, EventTriggeredEventArgs e)
        {
            FireEventsOccurred(new[] { e.Event });
        }

        private void _client_Connected(object sender, EventArgs e)
        {
            FireServerEvent(EventTypes.ServerConnected);
        }

        private void _client_Disconnected(object sender, EventArgs e)
        {
            FireServerEvent(EventTypes.ServerDisconnected);
        }

        private void FireServerEvent(ACEventType eventType)
        {
            var @event = new ACEvent(Guid.NewGuid().ToString(), eventType.Id, _client.ServerId, DateTime.UtcNow, eventType.Name, string.Empty, null, null, null);
            FireEventsOccurred(new[] { @event });
        }
    }
}
