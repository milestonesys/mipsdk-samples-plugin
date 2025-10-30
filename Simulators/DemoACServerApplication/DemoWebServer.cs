using DemoServerApplication.Configuration;
using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace DemoServerApplication
{
    internal class DemoWebServer
    {
        private CancellationTokenSource _tcs;
        private Task _runTask;

        public void Run()
        {
            _tcs = new CancellationTokenSource();
            _runTask = RunAsync(_tcs.Token);
        }

        public void Stop()
        {
            _tcs.Cancel();
            _runTask.Wait();
        }

        private static async Task RunAsync(CancellationToken cancellationToken)
        {
            using (var listener = new HttpListener())
            {
                listener.Prefixes.Add("http://localhost:8732/DemoACServerApplication/CardholderManagement/");
                listener.Prefixes.Add("http://localhost:8732/DemoACServerApplication/Reporting/");

                // Stop listener on cancellation (GetContextAsync doesn't take a cancellation token)
                cancellationToken.Register(() => listener.Stop());
                listener.Start();
                try
                {
                    while (listener.IsListening && !cancellationToken.IsCancellationRequested)
                    {
                        var ctx = await listener.GetContextAsync().ConfigureAwait(false);
                        try
                        {
                            var response = Encoding.UTF8.GetBytes(HandleRequest(ctx.Request));
                            ctx.Response.ContentLength64 = response.Length;
                            await ctx.Response.OutputStream.WriteAsync(response, 0, response.Length, cancellationToken);
                        }
                        catch { }
                        finally
                        {
                            ctx.Response.OutputStream.Close();
                        }
                    }
                }
                catch { }
            }
        }

        private static string HandleRequest(HttpListenerRequest request)
        {
            var responseBuilder = new StringBuilder();
            responseBuilder.AppendLine("<html>");
            responseBuilder.AppendLine("  <head>");
            responseBuilder.AppendLine("    <style>");
            responseBuilder.AppendLine("      body { font-family: sans-serif; }");
            responseBuilder.AppendLine("      table, th, td { border: 1px solid black; border-collapse: collapse; }");
            responseBuilder.AppendLine("      th, td { padding: 5px; text-align: left; }");
            responseBuilder.AppendLine("    </style>");
            responseBuilder.AppendLine("  </head>");
            responseBuilder.AppendLine("  <body>");
            responseBuilder.AppendLine("    <h1>Demo Access Control System</h1>");

            if (request.Url.LocalPath.StartsWith("/DemoACServerApplication/CardholderManagement", StringComparison.OrdinalIgnoreCase))
            {
                responseBuilder.AppendLine("    <h2>Cardholder Management</h2>");

                var match = Regex.Match(request.Url.Query, @"(\?|&)id=(?<id>[^&]*)", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    Guid cardholderId;
                    CredentialHolder cardholder;
                    if (Guid.TryParse(match.Groups["id"].Value, out cardholderId) &&
                        (cardholder = ConfigurationManager.Instance.LookupCredentialHolder(cardholderId)) != null)
                    {
                        responseBuilder.AppendLine("    <table>");
                        responseBuilder.AppendLine(string.Format("      <tr><td>Name</td><td>{0}</td></tr>", cardholder.Name));
                        responseBuilder.AppendLine(string.Format("      <tr><td>Department</td><td>{0}</td></tr>", cardholder.Department));
                        responseBuilder.AppendLine(string.Format("      <tr><td>Card id</td><td>{0}</td></tr>", cardholder.CardId));
                        responseBuilder.AppendLine(string.Format("      <tr><td>Expiry date</td><td>{0}</td></tr>", cardholder.ExpiryDate));
                        responseBuilder.AppendLine("    </table>");
                    }
                    else
                    {
                        responseBuilder.AppendLine("    <p>Cardholder not found.</p>");
                    }
                }
                else
                {
                    foreach (var cardholder in ConfigurationManager.Instance.CredentialHolders)
                    {
                        responseBuilder.AppendLine(string.Format("    <p><a href=\"?id={0}\">{1}</a></p>", cardholder.Id, cardholder.Name));
                    }
                }
            }
            else if (request.Url.LocalPath.StartsWith("/DemoACServerApplication/Reporting", StringComparison.OrdinalIgnoreCase))
            {
                responseBuilder.AppendLine("    <h2>Reporting</h2>");
                responseBuilder.AppendLine("    <p>Nothing to report...</p>");
            }
            responseBuilder.AppendLine("  </body>");
            responseBuilder.AppendLine("</html>");
            return responseBuilder.ToString();
        }
    }
}