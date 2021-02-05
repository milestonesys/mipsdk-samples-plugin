using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Platform.Transact.Connector;
using VideoOS.Platform.Transact.Connector.Property;

namespace TransactConnector
{
    public class SampleConnectorInstance : ConnectorInstance
    {
        private ITransactionDataReceiver _transactionDataReceiver;
        private CancellationTokenSource _cancellationTokenSource;

        private readonly object _cancellationTokenLock = new object();
        private readonly SemaphoreSlim _gate = new SemaphoreSlim(1, 1);

        public override void Init(ITransactionDataReceiver transactionDataReceiver, IEnumerable<ConnectorProperty> properties)
        {
            _transactionDataReceiver = transactionDataReceiver;
            UpdateProperties(properties);
            Util.Log(false, GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, "Initializing sample connector instance");
        }

        public override void Close()
        {
            Util.Log(false, GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, "Closing sample connector instance");
            StopDataProducer();
        }

        public override ConnectorPropertyValidationResult ValidateProperties(IEnumerable<ConnectorProperty> properties)
        {
            foreach (ConnectorProperty property in properties)
            {
                switch (property.Key)
                {
                    case SampleConnectorDefinition.UriProperty:
                        Uri uri;
                        if (!Uri.TryCreate(((ConnectorStringProperty)property).Value, UriKind.Absolute, out uri))
                        {
                            return ConnectorPropertyValidationResult.CreateInvalidResult(property.Key, Resources.InvalidURI);
                        }
                        break;
                    case SampleConnectorDefinition.PasswordProperty:
                        string password = ((ConnectorPasswordProperty)property).Value;
                        if (!password.Any(c => Char.IsLetter(c)) || !password.Any(c => Char.IsDigit(c)))
                        {
                            return ConnectorPropertyValidationResult.CreateInvalidResult(property.Key, Resources.InvalidPassword);
                        }
                        break;
                }
            }

            return ConnectorPropertyValidationResult.ValidResult;
        }

        public override async void UpdateProperties(IEnumerable<ConnectorProperty> properties)
        {
            string extraLine = string.Empty;
            bool enableTimestampOffset = false;
            int timestampOffsetInSeconds = 0;

            foreach (var property in properties)
            {
                // Cast the ConnectorProperty to retrieve the strongly typed value
                switch (property.Key)
                {
                    case SampleConnectorDefinition.ExtralineProperty:
                        extraLine = ((ConnectorStringProperty)property).Value;
                        break;
                    case SampleConnectorDefinition.UriProperty:
                        Uri uri = new Uri(((ConnectorStringProperty)property).Value);
                        break;
                    case SampleConnectorDefinition.PasswordProperty:
                        string password = ((ConnectorPasswordProperty)property).Value;
                        break;
                    case SampleConnectorDefinition.NumberProperty:
                        int offset = ((ConnectorIntegerProperty)property).Value;
                        break;
                    case SampleConnectorDefinition.FlagProperty:
                        bool flag = ((ConnectorBooleanProperty)property).Value;
                        break;
                    case SampleConnectorDefinition.OptionsProperty:
                        string optionKey = ((ConnectorOptionProperty)property).Value;
                        break;
                    case SampleConnectorDefinition.UseTimestampingProperty:
                        enableTimestampOffset = ((ConnectorBooleanProperty)property).Value;
                        break;
                    case SampleConnectorDefinition.TimestampOffsetProperty:
                        timestampOffsetInSeconds = ((ConnectorIntegerProperty)property).Value;
                        break;
                }
            }

            await RestartDataProducer(extraLine, enableTimestampOffset, timestampOffsetInSeconds);
        }

        private async Task RestartDataProducer(string extraLine, bool enableTimestampOffset, int timestampOffsetInSeconds)
        {
            CancellationToken cancellationToken;
            lock (_cancellationTokenLock)
            {
                if (_cancellationTokenSource != null)
                {
                    _cancellationTokenSource.Cancel();
                }

                _cancellationTokenSource = new CancellationTokenSource();
                cancellationToken = _cancellationTokenSource.Token;
            }

            await ProduceData(extraLine, enableTimestampOffset, timestampOffsetInSeconds, cancellationToken);
        }

        private void StopDataProducer()
        {
            lock (_cancellationTokenLock)
            {
                if (_cancellationTokenSource != null)
                {
                    _cancellationTokenSource.Cancel();
                    _cancellationTokenSource = null;
                }
            }
        }

        private async Task ProduceData(string extraLine, bool enableTimestampOffset, int timestampOffsetInSeconds, CancellationToken cancellationToken)
        {
            try
            {
                await _gate.WaitAsync(cancellationToken);
                try
                {
                    while (cancellationToken.IsCancellationRequested == false)
                    {
                        // NOTE: This is where the logic for producing data goes!
                        await CreateAndWriteData(extraLine, enableTimestampOffset, timestampOffsetInSeconds, cancellationToken);
                    }
                }
                finally
                {
                    _gate.Release();
                    Util.Log(false, GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, "Closed sample connector instance");
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception e)
            {
                Util.Log(false, GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, "Unexpected error in Sample connector instance. Shutting it down: " + e);
            }
        }

        private async Task CreateAndWriteData(string extraLine, bool enableTimestampOffset, int timestampOffsetInSeconds, CancellationToken cancellationToken)
        {
            byte[] data = CreateData(extraLine);

            var minimalAcceptedTimestamp = _transactionDataReceiver.MinimalAcceptedTimestamp;
            if (enableTimestampOffset)
            {
                var requestedTimestamp = DateTime.UtcNow.AddSeconds(timestampOffsetInSeconds);
                if (requestedTimestamp < minimalAcceptedTimestamp)
                {
                    await DelayNextWriteAttempt(requestedTimestamp, minimalAcceptedTimestamp, cancellationToken);
                    return;
                }

                _transactionDataReceiver.WriteRawData(data, requestedTimestamp);
            }
            else
            {
                if (DateTime.UtcNow < minimalAcceptedTimestamp)
                {
                    await DelayNextWriteAttempt(DateTime.UtcNow, minimalAcceptedTimestamp, cancellationToken);
                    return;
                }

                // Note: If an offset has been used and it was set in the future (which makes no sense in a real life scenario),
                // this call can fail with an ArgumentException if the usage of offset is disabled again. Hence the check above,
                // which would normally not be required.
                _transactionDataReceiver.WriteRawData(data);
            }

            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
        }

        private async Task DelayNextWriteAttempt(DateTime requestedTimestamp, DateTime minimalAcceptedTimestamp, CancellationToken cancellationToken)
        {
            var waitTime = minimalAcceptedTimestamp - requestedTimestamp;
            Util.Log(false, GetType().FullName + "." + MethodBase.GetCurrentMethod().Name,
                string.Format(
                    "Timestamp offset will attempt to write data before the latest acceptable date. Sleeping for {0}",
                    waitTime));
            await Task.Delay(waitTime, cancellationToken);
        }

        private byte[] CreateData(string extraLine)
        {
            var builder = new StringBuilder();
            builder.AppendFormat("Starting transaction on {0}", DateTime.Now);
            builder.AppendLine();

            builder.AppendFormat("Transaction line 1 with data {0}", Guid.NewGuid());
            builder.AppendLine();

            builder.AppendLine("Transaction line 2");
            
            builder.AppendFormat("Another line with data {0}", Guid.NewGuid());
            builder.AppendLine();

            if (!string.IsNullOrEmpty(extraLine))
            {
                builder.AppendLine(extraLine);
            }

            builder.AppendLine("End of transaction. Have a nice day!");

            return Encoding.UTF8.GetBytes(builder.ToString());
        }
    }
}