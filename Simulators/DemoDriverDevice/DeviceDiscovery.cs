using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using System.Net.NetworkInformation;

namespace MulticastV2
{
    internal class DeviceDiscovery
    {
        UdpClient _clientIp4;
        UdpClient _clientIp6;
        IPAddress _multicastAddr;
        IPAddress _multicastIp6;
        int _port;
        bool _enabled;
        int _servicePort;
        string _scheme;

        //Use port 3702 and multicastGroupAddress "[ff02::c]" or "239.255.255.250"
        public DeviceDiscovery(int port, string multicastGroupAddressIp4, string multicastGroupAddressIp6,
            int servicePort, string scheme)
        {
            _port = port;
            _multicastAddr = IPAddress.Parse(multicastGroupAddressIp4);
            _multicastIp6 = IPAddress.Parse(multicastGroupAddressIp6);
            _servicePort = servicePort;
            _scheme = scheme;
        }

        public void JoinMulticastGroup()
        {
            var validIp4Nics = GetMulticastEnabledNICs();
            _clientIp4 = new UdpClient(_port, AddressFamily.InterNetwork);
            _clientIp4.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            foreach (IPAddress ip in validIp4Nics)
            {
                _clientIp4.JoinMulticastGroup(_multicastAddr, ip);
            }
            _clientIp6 = new UdpClient(_port, AddressFamily.InterNetworkV6);
            _clientIp6.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _clientIp6.JoinMulticastGroup(_multicastIp6);
            _enabled = true;
        }

        public void UDPListener()
        {
            Task.Run(async () =>
            {
                while (_enabled)
                {
                    try
                    {
                        var receivedResults = await _clientIp4.ReceiveAsync();
                        var message = Encoding.ASCII.GetString(receivedResults.Buffer);

                        var xdoc = XElement.Parse(message);
                        XNamespace wsadis = "http://schemas.xmlsoap.org/ws/2004/08/addressing";
                        var messageID = xdoc.Descendants().Where(node => node.Name == (wsadis + "MessageID")).FirstOrDefault()?.Value;
                        XNamespace wsdisc = "http://schemas.xmlsoap.org/ws/2005/04/discovery";
                        var msgNumber = xdoc.Descendants().Where(node => node.Name == (wsdisc + "AppSequence")).FirstOrDefault()?.Attribute("MessageNumber")?.Value;
                        if (!string.IsNullOrEmpty(messageID) && messageID.ToLower().Contains("0673c10c-36c7-480e-9283"))
                        {
                            var response = Encoding.ASCII.GetBytes(CreateResponse(messageID, msgNumber));
                            var sendResult = await _clientIp4.SendAsync(response, response.Length, receivedResults.RemoteEndPoint);
                        }
                    }
                    catch (Exception)
                    {
                        if (_enabled)
                        {
                            LeaveMulticastGroup();
                            JoinMulticastGroup();
                        }
                    }
                }
            });
            Task.Run(async () =>
            {
                while (_enabled)
                {
                    try
                    {
                        var receivedResults = await _clientIp6.ReceiveAsync();
                        var message = Encoding.ASCII.GetString(receivedResults.Buffer);

                        var xdoc = XElement.Parse(message);
                        XNamespace wsadis = "http://schemas.xmlsoap.org/ws/2004/08/addressing";
                        var messageID = xdoc.Descendants().Where(node => node.Name == (wsadis + "MessageID")).FirstOrDefault()?.Value;
                        XNamespace wsdisc = "http://schemas.xmlsoap.org/ws/2005/04/discovery";
                        var msgNumber = xdoc.Descendants().Where(node => node.Name == (wsdisc + "AppSequence")).FirstOrDefault()?.Attribute("MessageNumber")?.Value;
                        if (!string.IsNullOrEmpty(messageID) && messageID.ToLower().Contains("0673c10c-36c7-480e-9283"))
                        {
                            var response = Encoding.ASCII.GetBytes(CreateResponse(messageID, msgNumber, true));
                            var sendResult = await _clientIp6.SendAsync(response, response.Length, receivedResults.RemoteEndPoint);
                        }
                    }
                    catch (Exception)
                    {
                        if (_enabled)
                        {
                            LeaveMulticastGroup();
                            JoinMulticastGroup();
                        }
                    }
                }
            });
        }

        public void LeaveMulticastGroup()
        {
            if (_enabled)
            {
                _enabled = false;
                _clientIp4.DropMulticastGroup(_multicastAddr);
                _clientIp4.Close();
                _clientIp4.Dispose();
                _clientIp6.DropMulticastGroup(_multicastIp6);
                _clientIp6.Close();
                _clientIp6.Dispose();
            }
        }

        private static List<IPAddress> GetMulticastEnabledNICs()
        {
            var list = new List<IPAddress>();
            var nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach(var nic in nics)
            {
                if(nic.OperationalStatus == OperationalStatus.Up && nic.SupportsMulticast)
                {
                    var addresses = nic.GetIPProperties().UnicastAddresses.Where(x => x.Address.AddressFamily == AddressFamily.InterNetwork);
                    if(addresses.Any())
                    {
                        list.AddRange(addresses.Select(x => x.Address));
                    }
                }
            }
            return list;
        }

        private string CreateResponse(string relatesTo, string msgNumber, bool isIp6 = false)
        {
            Guid messageId = Guid.NewGuid();
            var hostName = Dns.GetHostName();
            var addressList = Dns.GetHostEntry(hostName).AddressList.Where(x => x.IsIPv6LinkLocal == isIp6);
            var ip = addressList.First().ToString();
            if(isIp6 && ip != null)
            {
                if(ip.Contains("%"))
                {
                    ip = ip.Substring(0, ip.IndexOf("%"));
                }
                ip = "[" + ip + "]";
            }
            var response = 
                $@"<SOAP:Envelope xmlns:SOAP=""http://www.w3.org/2003/05/SOAPelope"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
                                  xmlns:a=""http://schemas.xmlsoap.org/ws/2004/08/addressing"" xmlns:d=""http://schemas.xmlsoap.org/ws/2005/04/discovery"" 
                                  xmlns:dn=""http://www.onvif.org/ver10/network/wsdl"" xmlns:tds=""http://www.onvif.org/ver10/device/wsdl"">
                     <SOAP:Header>
                        <a:Action>http://schemas.xmlsoap.org/ws/2005/04/discovery/ProbeMatches</a:Action>
                        <a:MessageID>urn:uuid:{messageId}</a:MessageID>
                        <a:RelatesTo>{relatesTo}</a:RelatesTo>
                        <a:To>http://schemas.xmlsoap.org/ws/2004/08/addressing/role/anonymous</a:To>
                        <d:AppSequence MessageNumber=""{msgNumber}"" InstanceId=""1""></d:AppSequence>
                     </SOAP:Header>
                     <SOAP:Body>
                        <d:ProbeMatches>
                            <d:ProbeMatch>
                                <d:Types>dn:NetworkVideoTransmitter tds:Device</d:Types>
                                <d:Scopes>onvif://www.onvif.org/hardware/DemoDriverDevice onvif://www.onvif.org/name/Demo</d:Scopes>
                                <d:XAddrs>{_scheme}://{ip}:{_servicePort}/onvif/device_service</d:XAddrs>
                                <d:MetadataVersion>2</d:MetadataVersion>
                            </d:ProbeMatch>
                        </d:ProbeMatches>
                     </SOAP:Body>
                   </SOAP:Envelope>";

            return response;
        }
    }
}
