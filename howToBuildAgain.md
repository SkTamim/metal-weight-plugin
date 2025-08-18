

## 4. Final Source Code

Here is the complete and final code for the project.

### `Properties/AssemblyInfo.cs`
```csharp
using System.Reflection;
using System.Runtime.InteropServices;
using Rhino.PlugIns;

// Plug-in Description Attributes
[assembly: PlugInDescription(DescriptionType.WebSite, "")]
[assembly: PlugInDescription(DescriptionType.Email, "your.email@example.com")]
[assembly: PlugInDescription(DescriptionType.Organization, "Your Name or Company")]
[assembly.PlugInDescription(DescriptionType.Country, "")]

// This is the unique GUID for your plugin
[assembly: Guid("A8B8E6F2-8A7B-4A1E-9C8A-3E3B6E1D1F2A")]

// Assembly Info
[assembly: AssemblyTitle("MetalWeight")]
[assembly: AssemblyDescription("Metal Weight Calculator Plug-In")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Your Name or Company")]
[assembly: AssemblyProduct("MetalWeight")]
[assembly: AssemblyCopyright("Copyright © 2025 Your Name")]
[assembly: ComVisible(false)]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]







MetalWeightPlugIn.cs

C#

using Rhino.PlugIns;

namespace MetalWeight
{
    public class MetalWeightPlugIn : PlugIn
    {
        public static MetalWeightPlugIn Instance { get; private set; }

        public MetalWeightPlugIn()
        {
            Instance = this;
        }
    }
}

MetalWeightCommand.cs

C#

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;

namespace MetalWeight
{
    public class MetalWeightCommand : Command
    {
        public override string EnglishName => "MetalWeight";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // --- Step 1: Select Objects ---
            var selectedObjects = doc.Objects.GetSelectedObjects(false, false).ToList();

            if (selectedObjects.Count == 0)
            {
                var go = new GetObject();
                go.SetCommandPrompt("Select solids or meshes");
                go.GeometryFilter = ObjectType.Brep | ObjectType.Mesh;
                go.GroupSelect = true;
                go.SubObjectSelect = false;
                go.GetMultiple(1, 0);

                if (go.CommandResult() != Result.Success)
                    return go.CommandResult();

                foreach (var objRef in go.Objects())
                    selectedObjects.Add(objRef.Object());
            }

            if (selectedObjects.Count == 0)
            {
                RhinoApp.WriteLine("❌ No objects selected.");
                return Result.Cancel;
            }

            // --- Step 2: Get Density from User ---
            var gn = new GetNumber();
            gn.SetCommandPrompt($"Enter density in grams per cubic {doc.ModelUnitSystem.ToString().ToLower()}");
            gn.SetDefaultNumber(17.0); 
            gn.SetLowerLimit(0.0, true); 
            gn.Get();

            if (gn.CommandResult() != Result.Success)
                return gn.CommandResult();
            
            double density = gn.Number();

            RhinoApp.WriteLine("Analyzing {0} selected objects using parallel processing...", selectedObjects.Count);

            // --- Step 3: Parallel Validation and Calculation ---
            var badObjects = new ConcurrentBag<Guid>();
            var openSurfaces = new ConcurrentBag<Guid>();
            var openMeshes = new ConcurrentBag<Guid>();
            double totalVolume = 0;
            object lockObject = new object();

            Rhino.UI.StatusBar.ShowProgressMeter(0, selectedObjects.Count, "Calculating...", false, true);
            doc.Views.Redraw();

            try
            {
                using (new Rhino.UI.WaitCursor())
                {
                    Parallel.ForEach(selectedObjects, rhObj =>
                    {
                        if (rhObj == null) return;

                        var geo = rhObj.Geometry;
                        if (!geo.IsValid) { badObjects.Add(rhObj.Id); }
                        else if (geo is Brep brep)
                        {
                            if (brep.IsSolid)
                            {
                                var props = VolumeMassProperties.Compute(brep);
                                if (props != null)
                                {
                                    lock (lockObject) { totalVolume += props.Volume; }
                                }
                            }
                            else { openSurfaces.Add(rhObj.Id); }
                        }
                        else if (geo is Mesh mesh)
                        {
                            if (mesh.IsClosed)
                            {
                                var props = VolumeMassProperties.Compute(mesh);
                                if (props != null)
                                {
                                    lock (lockObject) { totalVolume += props.Volume; }
                                }
                            }
                            else { openMeshes.Add(rhObj.Id); }
                        }
                    });
                }
            }
            finally
            {
                Rhino.UI.StatusBar.HideProgressMeter();
            }

            int validObjectCount = selectedObjects.Count - badObjects.Count - openSurfaces.Count - openMeshes.Count;

            // --- Step 4: Handle Problem Objects ---
            if (!badObjects.IsEmpty || !openSurfaces.IsEmpty || !openMeshes.IsEmpty)
            {
                var allIssues = new HashSet<Guid>();
                foreach (var id in badObjects) allIssues.Add(id);
                foreach (var id in openSurfaces) allIssues.Add(id);
                foreach (var id in openMeshes) allIssues.Add(id);

                doc.Objects.UnselectAll();
                foreach (var id in allIssues)
                    doc.Objects.Select(id);
                doc.Views.Redraw();
                RhinoApp.RunScript("_Zoom Selected", false);

                string msg = "Some selected objects may cause issues:\n\n";
                if (!badObjects.IsEmpty) msg += $"❌ {badObjects.Count} bad object(s)\n";
                if (!openSurfaces.IsEmpty) msg += $"📄 {openSurfaces.Count} open surface(s)\n";
                if (!openMeshes.IsEmpty) msg += $"🕳️ {openMeshes.Count} open mesh(es)\n";
                msg += "\nDo you want to continue with the calculation?";

                DialogResult result = MessageBox.Show(msg, "Warning: Problem Detected", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    RhinoApp.WriteLine("🚫 Operation cancelled by user.");
                    return Result.Cancel;
                }
            }

            if (validObjectCount == 0)
            {
                RhinoApp.WriteLine("No valid solid or closed objects were found to calculate.");
                return Result.Nothing;
            }

            // --- Step 5: Display Final Results ---
            double estimatedWeight = totalVolume * density; 

            RhinoApp.WriteLine("--- Weight Calculation Results ---");
            RhinoApp.WriteLine($"Processed {validObjectCount} valid object(s).");
            RhinoApp.WriteLine($"Total Volume: {Math.Round(totalVolume, 2)} cubic {doc.ModelUnitSystem.ToString().ToLower()}s");
            RhinoApp.WriteLine($"Estimated Weight (at {density} g/cubic {doc.ModelUnitSystem.ToString().ToLower()}): {Math.Round(estimatedWeight, 2)} grams");

            return Result.Success;
        }
    }
}

5. How to Build the Project from Source

To compile this project yourself, you will need:

    Visual Studio 2017 (Community Edition is fine).

    .NET Framework 4.5 Developer Pack.

    Rhino 5 (64-bit) installed on your machine.

Steps:

    Create a new project in Visual Studio named MetalWeight using the Class Library (.NET Framework) template.

    Configure the project properties as discussed (Platform to x64, .NET 4.5, add RhinoCommon reference, add post-build event).

    Create the three C# files (AssemblyInfo.cs, MetalWeightPlugIn.cs, MetalWeightCommand.cs) and paste the code from this document.

    Select Build > Rebuild Solution.