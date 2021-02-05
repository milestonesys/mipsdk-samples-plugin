using System.Collections.Generic;
using System.Linq;
using VideoOS.Platform.AccessControl.Plugin;

namespace DemoAccessControlPlugin.Configuration
{
    /// <summary>
    /// Class for defining, validating, and storing system properties.
    /// See also <see cref="DemoAccessControlSystem"/>.
    /// </summary>
    internal class SystemProperties
    {
        private static class Keys
        {
            public const string Address = "ACAddress";
            public const string Port = "ACPort";
            public const string User = "ACUser";
            public const string Password = "ACPassword";
            public const string EventPollingPeriod = "ACEventPollingPeriod";
            public const string EventPollingCount = "ACEventPollingCount";
            public const string ImageOverrideEnabled = "ACImageOverrideEnabled";
        }

        public string Address { get; private set; }
        public int Port { get; private set; }
        public string AdminUser { get; private set; }
        public string AdminPassword { get; private set; }
        public int EventPollingPeriodMs { get; private set; }
        public int EventPollingCount { get; private set; }
        public bool ImageOverrideEnabled { get; private set; }

        public static IEnumerable<ACPropertyDefinition> PropertyDefinitions { get { return _propertyDefinitions; } }

        private static List<ACPropertyDefinition> _propertyDefinitions = new List<ACPropertyDefinition>()
        {
            new ACPropertyDefinition(Keys.Address, "Address", "localhost", ACImportance.Required, false, "The address of the Demo server", ACValueTypes.StringType, null),
            new ACPropertyDefinition(Keys.Port, "Port", "8732", ACImportance.Required, false, "The port of the Demo server", ACValueTypes.IntType, new ACProperty[] { new ACProperty(ACValueTypeProperties.IntTypeMinValue, "1"), new ACProperty(ACValueTypeProperties.IntTypeMaxValue, "65535") }),
            new ACPropertyDefinition(Keys.User, "User", "admin", ACImportance.Required, false, "The admin user", ACValueTypes.StringType, null),
            new ACPropertyDefinition(Keys.Password, "Password", "pass", ACImportance.Required, false, "The admin password", ACValueTypes.PasswordStringType, new ACProperty[] { new ACProperty(ACValueTypeProperties.PasswordStringTypeMinLength, "4"), new ACProperty(ACValueTypeProperties.PasswordStringTypeMaxLength, "10") }),
            new ACPropertyDefinition(Keys.EventPollingPeriod, "Event polling period (ms)", "250", ACImportance.Optional, false, "Event polling period", ACValueTypes.IntType, null),
            new ACPropertyDefinition(Keys.EventPollingCount, "Event polling max count", "100", ACImportance.Optional, false, "Event polling max count", ACValueTypes.EnumType, new ACProperty[] { new ACProperty(ACValueTypeProperties.EnumTypeValueKey, "1"), new ACProperty(ACValueTypeProperties.EnumTypeValueKey, "5"), new ACProperty(ACValueTypeProperties.EnumTypeValueKey, "10"), new ACProperty(ACValueTypeProperties.EnumTypeValueKey, "100"), new ACProperty(ACValueTypeProperties.EnumTypeValueKey, "1000") }),
            new ACPropertyDefinition(Keys.ImageOverrideEnabled, "Cardholder image override enabled", bool.TrueString, ACImportance.Optional, false, "Indicates whether or not image override is enabled for cardholders", ACValueTypes.BoolType, null),
        };

        public static ACPropertyValidationResult ValidateProperties(IEnumerable<ACProperty> properties)
        {
            foreach (ACProperty property in properties)
            {
                var propertyDefinition = _propertyDefinitions.FirstOrDefault(def => def.Key == property.Key);

                if (propertyDefinition != null)
                {
                    switch (property.Key)
                    {
                        // Check that address, user, and password are not empty.
                        case Keys.Address:
                        case Keys.User:
                        case Keys.Password:
                            if (string.IsNullOrWhiteSpace(property.Value))
                            {
                                return new ACPropertyValidationResult(property.Key, propertyDefinition.DisplayName + " cannot be empty.");
                            }
                            break;
                        // For demonstration purposes check the event polling period here instead of using min/max value on the property definition
                        case Keys.EventPollingPeriod:
                            int period;
                            if (!int.TryParse(property.Value, out period) || period < 10 || period > 10000)
                                return new ACPropertyValidationResult(property.Key, propertyDefinition.DisplayName + " is invalid. Must be between 10 and 10000 ms.");
                            break;
                    }
                }
            }

            return new ACPropertyValidationResult();
        }

        public void UpdateProperties(IEnumerable<ACProperty> properties)
        {
            foreach (ACProperty property in properties)
            {
                switch (property.Key)
                {
                    case Keys.Address:
                        Address = property.Value;
                        break;
                    case Keys.Port:
                        int port;
                        if (int.TryParse(property.Value, out port))
                        {
                            Port = port;
                        }
                        break;
                    case Keys.User:
                        AdminUser = property.Value;
                        break;
                    case Keys.Password:
                        AdminPassword = property.Value;
                        break;
                    case Keys.EventPollingPeriod:
                        int eventPeriodMs;
                        if (int.TryParse(property.Value, out eventPeriodMs))
                        {
                            EventPollingPeriodMs = eventPeriodMs;
                        }
                        break;
                    case Keys.EventPollingCount:
                        int count;
                        if (int.TryParse(property.Value, out count))
                        {
                            EventPollingCount = count;
                        }
                        break;
                    case Keys.ImageOverrideEnabled:
                        bool imageOverrideEnabled;
                        if (bool.TryParse(property.Value, out imageOverrideEnabled))
                        {
                            ImageOverrideEnabled = imageOverrideEnabled;
                        }
                        break;
                }
            }
        }
    }
}