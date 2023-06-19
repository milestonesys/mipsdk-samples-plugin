using System;
using System.Collections.Generic;

namespace LocationView.Client.Config
{
    public enum ToolTipAppearanceTypes
    {
        Always = 0,
        Never,
        OnMouse
    }

    [Flags]
    public enum ToolTipTextTypes
    {
        Name = 0x01,
        Location = 0x02,

        NameAndLocation = Name | Location,
    }

    internal class ToolTipAppearanceNames
    {
        private static readonly Dictionary<ToolTipAppearanceTypes, string> Names =
            new Dictionary<ToolTipAppearanceTypes, string>()
            {
                {ToolTipAppearanceTypes.Always, "Always"},
                {ToolTipAppearanceTypes.Never, "Never"},
                {ToolTipAppearanceTypes.OnMouse, "On mouse over"},
            };

        public static string GetName(ToolTipAppearanceTypes type)
        {
            return TypesHelper.GetName(Names, type);
        }

        public static ToolTipAppearanceTypes GetType(string name)
        {
            return TypesHelper.GetType(Names, name);
        }
    }

    internal class ToolTipTextNames
    {
        private static readonly Dictionary<ToolTipTextTypes, string> Names =
            new Dictionary<ToolTipTextTypes, string>()
            {
                {ToolTipTextTypes.Name, "Name"},
                {ToolTipTextTypes.Location, "Location"},
                {ToolTipTextTypes.NameAndLocation, "Name and Location"},
            };

        public static string GetName(ToolTipTextTypes type)
        {
            return TypesHelper.GetName(Names, type);
        }

        public static ToolTipTextTypes GetType(string name)
        {
            return TypesHelper.GetType(Names, name);
        }
    }

    internal class ToolTipTextHelper
    {
        public static bool HasValue(ToolTipTextTypes type, ToolTipTextTypes value)
        {
            return (type & value) == value;
        }
    }
}
