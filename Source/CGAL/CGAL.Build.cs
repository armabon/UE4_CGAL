using UnrealBuildTool;
using System.IO;

public class CGAL : ModuleRules
{
    private string ModulePath
    {
        get { return ModuleDirectory; }
    }
    private string ThirdPartyPath
    {
        get { return Path.GetFullPath(Path.Combine(ModulePath, "../../ThirdParty/")); }
    }

	public CGAL(TargetInfo Target)
	{
        Definitions.Add("WIN32");
        Definitions.Add("_WINDOWS");
        Definitions.Add("_CRT_SECURE_NO_DEPRECATE");
        Definitions.Add("_SCL_SECURE_NO_DEPRECATE");
        Definitions.Add("_CRT_SECURE_NO_WARNINGS");
        Definitions.Add("_SCL_SECURE_NO_WARNINGS");
        Definitions.Add("CGAL_USE_MPFR");
        Definitions.Add("CGAL_USE_GMP");

        // Startard Module Dependencies
        PublicDependencyModuleNames.AddRange(new string[] { "Core" });
		PrivateDependencyModuleNames.AddRange(new string[] { "CoreUObject", "Engine", "Slate", "SlateCore" });

        // Start CGAL linking here!
        bool isLibrarySupported = false;

        // Create CGAL Path
        string CGALPath = Path.Combine(ThirdPartyPath, "CGAL");

        // Get Library Path
        string LibPath = "";
        bool isdebug = Target.Configuration == UnrealTargetConfiguration.Debug && BuildConfiguration.bDebugBuildsActuallyUseDebugCRT;
        if (Target.Platform == UnrealTargetPlatform.Win64)
        {
            LibPath = Path.Combine(CGALPath, "libraries", "Win64");
            isLibrarySupported = true;
        }
        else
        {
            string Err = string.Format("{0} dedicated server is made to depend on {1}. We want to avoid this, please correct module dependencies.", Target.Platform.ToString(), this.ToString()); System.Console.WriteLine(Err);
        }

        if (isLibrarySupported)
        {
            //Add Include path
            PublicIncludePaths.AddRange(new string[] { Path.Combine(CGALPath, "includes") });
            PublicIncludePaths.AddRange(new string[] { Path.Combine(CGALPath, "includes","GMP") });//Dependencie

            // Add Library Path
            PublicLibraryPaths.Add(LibPath);

            // Add Dependencies
            if (!isdebug)
            {
                //Add Static Libraries
                PublicAdditionalLibraries.Add(Path.Combine("libgmp-10.lib"));
                PublicAdditionalLibraries.Add(Path.Combine("libmpfr-4.lib"));
                PublicAdditionalLibraries.Add(Path.Combine("libCGAL-vc140-mt-4.9.lib"));
                PublicAdditionalLibraries.Add(Path.Combine("libCGAL_Core-vc140-mt-4.9.lib"));
            }
            else
            {
                //Add Static Libraries (Debug Version)
                PublicAdditionalLibraries.Add(Path.Combine("libgmp-10.lib"));
                PublicAdditionalLibraries.Add(Path.Combine("libmpfr-4.lib"));
                PublicAdditionalLibraries.Add(Path.Combine("libCGAL-vc140-mt-gd-4.9.lib"));
                PublicAdditionalLibraries.Add(Path.Combine("libCGAL_Core-vc140-mt-gd-4.9.lib"));
            }
        }

        Definitions.Add(string.Format("WITH_CGAL_BINDING={0}", isLibrarySupported ? 1 : 0));
    }
}
