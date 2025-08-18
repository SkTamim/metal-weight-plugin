using System.Reflection;
using System.Runtime.InteropServices;
using Rhino.PlugIns;

// Plug-in Description Attributes
[assembly: PlugInDescription(DescriptionType.WebSite, "")]
[assembly: PlugInDescription(DescriptionType.Email, "mdsk44415@gmail.com")] // <-- ADD YOUR EMAIL HERE
[assembly: PlugInDescription(DescriptionType.Organization, "SK Tamim")] // <-- ADD YOUR NAME/COMPANY HERE
[assembly: PlugInDescription(DescriptionType.Country, "")]

// This is the unique GUID for your plugin
[assembly: Guid("B202ACB3-6752-4F62-BAD9-5F8809472407")]

// Assembly Info
[assembly: AssemblyTitle("MetalWeight")] // <-- RENAMED
[assembly: AssemblyDescription("Metal Weight Calculator Plug-In")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Sk Tamim")]
[assembly: AssemblyProduct("MetalWeight")] // <-- RENAMED
[assembly: AssemblyCopyright("Copyright © 2025 Sk Tamim")] // <-- ADD YOUR NAME HERE
[assembly: ComVisible(false)]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]