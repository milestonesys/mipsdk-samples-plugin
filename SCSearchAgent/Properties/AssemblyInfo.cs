using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using SCSearchAgent;

[assembly: AssemblyCompany(AssemblyInfo.CompanyName)]
[assembly: AssemblyProduct(AssemblyInfo.ProductName)]
[assembly: AssemblyCopyright(AssemblyInfo.Copyright)]
[assembly: AssemblyTrademark(AssemblyInfo.Trademark)]

// Assembly version is part of the strong name for the assembly.
[assembly: AssemblyVersion(AssemblyInfo.AssemblyVersion)]

// The information below is just for informational 
[assembly: AssemblyInformationalVersion(AssemblyInfo.Version)]    // Win32 Product Version (same as AssemblyFileVersion is not specified)
[assembly: AssemblyFileVersion(AssemblyInfo.FileVersion)]         // Win32 File Version (same as AssemblyVersion if not specified)
[assembly: AssemblyTitle(AssemblyInfo.FullDescription)]           // Win32 Description (The assembly title is a friendly name, which can include spaces)
[assembly: AssemblyConfiguration(AssemblyInfo.Configuration)]     // Specifies whether this is a debug or release version
[assembly: AssemblyDescription("")]                               // Win32 Comments
[assembly: AssemblyCulture("")]                                   // This must always be "" = neutral for main assemblies (only satellite assemblies must have a culture)
[assembly: NeutralResourcesLanguage("en-US")]

// It is good practice that developers consider and explicitly set these atttributes
[assembly: CLSCompliant(false)]
// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

namespace SCSearchAgent
{
    /// <summary>
    /// Information specific for this component.
    /// </summary>
    internal abstract class AssemblyInfo
    {
        // This is the version info that will be used for the assembly strong name (in case the assembly is signed) 
        internal const string Major = "1";          // Update these to reflect your version
        internal const string Minor = "0";
        internal const string Build = "0";
        internal const string Revision = "0";
        internal const string BuildDate = "";

        internal const string Version = Major + "." + Minor + "." + Build + "." + Revision;
        internal const string FileVersion = Major + "." + Minor + "." + Build + "." + Revision;
        internal const string AssemblyVersion = "1.0.0.0"; // This should not be changed.

        internal const string CompanyName = PluginSamples.Common.ManufacturerName;
        internal const string Copyright = "© " + CompanyName + ". All rights reserved";
        internal const string Trademark = "";

        internal const string ProductName = "SCSearchAgent integration to XProtect";
        internal const string ProductNameShort = "SCSearchAgent";
        internal const string Description = "SCSearchAgent integration";

#if DEBUG
        internal const string Configuration = "Debug";
        internal const string FullDescription = Description + " (Debug)";
#else
        internal const string Configuration = "Release";
        internal const string FullDescription = Description;
#endif
    }
}
