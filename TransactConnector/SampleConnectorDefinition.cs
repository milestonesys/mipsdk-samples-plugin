using System;
using System.Collections.Generic;
using VideoOS.Platform.Transact.Connector;
using VideoOS.Platform.Transact.Connector.Property;

namespace TransactConnector
{
    public class SampleConnectorDefinition : ConnectorDefinition
    {
        internal const string UseTimestampingProperty = "USE_TIMESTAMPING";
        internal const string TimestampOffsetProperty = "TIMESTAMP_OFFSET";
        internal const string ExtralineProperty = "EXTRALINEPROPERTY";
        internal const string UriProperty = "URI";
        internal const string PasswordProperty = "PASSWORD";
        internal const string NumberProperty = "NUMBER";
        internal const string FlagProperty = "FLAG";
        internal const string OptionsProperty = "OPTIONS";
        internal const string Option1Name = "OPTION1";
        internal const string Option2Name = "OPTION2";
        internal const string Option3Name = "OPTION3";
        
        private static readonly Version ConnectorVersion = new Version(1, 0);

        // Remember to use your own GUID for your own connector. Do not reuse the one below
        private static readonly Guid ProviderId = new Guid("CD56C850-34BA-40C2-81DA-8628E30DB40F");

        public override Guid Id
        {
            get { return ProviderId; }
        }

        public override string Name
        {
            get { return "Sample Connector"; }
        }

        public override string DisplayName
        {
            get { return Name; }
        }

        public override string VersionText
        {
            get { return ConnectorVersion.ToString(); }
        }

        public override string Manufacturer
        {
            get { return PluginSamples.Common.ManufacturerName; }
        }

        public override void Init()
        {
            Util.Log(false, GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, "Initialized sample connector definition!");
        }

        public override void Close()
        {
            Util.Log(false, GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, "Closed sample connector definition!");
        }

        public override ConnectorInstance CreateConnectorInstance()
        {
            return new SampleConnectorInstance();
        }

        public override IEnumerable<ConnectorPropertyDefinition> GetPropertyDefinitions()
        {
            return new List<ConnectorPropertyDefinition>
                {
                    new ConnectorStringPropertyDefinition(ExtralineProperty, Resources.ExtraLine, string.Empty, Resources.ExtraLineTooltip),
                    new ConnectorStringPropertyDefinition(UriProperty, Resources.TestURI, "http://localhost", Resources.DummyProperty),
                    new ConnectorPasswordPropertyDefinition(PasswordProperty, Resources.TestPassword, "pass1234", Resources.DummyProperty) { MinLength = 8, MaxLength = 16 },
                    new ConnectorIntegerPropertyDefinition(NumberProperty, Resources.TestNumber, 0, Resources.DummyProperty) { MinValue = -10, MaxValue = 10 },
                    new ConnectorBooleanPropertyDefinition(FlagProperty, Resources.TestFlag, true, Resources.DummyProperty),
                    new ConnectorOptionsPropertyDefinition(OptionsProperty, Resources.TestOptions, Option2Name, Resources.DummyProperty, new List<ConnectorPropertyValueOption>
                    {
                        new ConnectorPropertyValueOption(Option1Name, Resources.Option1),
                        new ConnectorPropertyValueOption(Option2Name, Resources.Option2),
                        new ConnectorPropertyValueOption(Option3Name, Resources.Option3),
                    }),
                    new ConnectorBooleanPropertyDefinition(UseTimestampingProperty, Resources.TimestampOffsetToggle, false, Resources.TimestampOffsetToggleTooltip),
                    new ConnectorIntegerPropertyDefinition(TimestampOffsetProperty, Resources.TimestampOffset, 0, Resources.TimestampOffsetTooltip)
                };
        }
    }
}
