///////////////////////////////////////////////////////////////////////////////
// TOOLS
///////////////////////////////////////////////////////////////////////////////
#addin "Cake.WebDeploy"
#tool "nuget:?package=NUnit.ConsoleRunner"


///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument<string>("target", "Default");
var configuration = Argument<string>("configuration", "Release");


///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////

var solutions = GetFiles("./**/*.sln");
var solutionPaths = solutions.Select(solution => solution.GetDirectory());

var buildResultDirRelative = Directory("./.build-output"); //+ Directory("v" + semVersion) + Directory("bin");
var buildResultDir = System.IO.Path.GetFullPath(buildResultDirRelative.ToString());


///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup((context) =>
{
    // Executed BEFORE the first task.
    Information("Running tasks...");
});

Teardown((context) =>
{
    // Executed AFTER the last task.
    Information("Finished running tasks.");
});


///////////////////////////////////////////////////////////////////////////////
// TASK DEFINITIONS
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
    .Description("Cleans all directories that are used during the build process.")
    .Does(() =>
{
    // Clean solution directories.
    foreach(var path in solutionPaths)
    {
        Information("Cleaning {0}", path);
        CleanDirectories(path + "/**/bin/" + configuration);
        CleanDirectories(path + "/**/obj/" + configuration);
    }
});

Task("Restore")
    .Description("Restores all the NuGet packages that are used by the specified solution.")
    .Does(() =>
{
    // Restore all NuGet packages.
    foreach(var solution in solutions)
    {
        Information("Restoring {0}...", solution);
        NuGetRestore(solution);
    }
});

Task("Build")
    .Description("Builds all the different parts of the project.")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .Does(() =>
{
    // Build all solutions.
    foreach(var solution in solutions)
    {
        Information("Building {0}", solution);

		//You need to include the 'DeployOnBuild' property to create the publish zip
        MSBuild(solution, settings =>
            settings.SetPlatformTarget(PlatformTarget.MSIL)
				//.WithProperty("WebPublishMethod", "Package")
				//.WithProperty("SkipInvalidConfigurations", "true"));
				//.WithProperty("OutDir", buildResultDir)
				.WithProperty("DeployOnBuild", "true") 
                .WithProperty("TreatWarningsAsErrors","true")
				.WithProperty("PackageAsSingleFile", "true")
                .WithTarget("Build")
                .SetConfiguration(configuration));
    }
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    foreach(var path in solutionPaths)
    {
		NUnit3(path + "/**/bin/" + configuration + "/*Tests.dll");
        //MSTest(path + "/**/bin/" + configuration + "/*Tests.dll");
    }
});

Task("Deploy")
    .IsDependentOn("Build")
    .Description("Deploy an example website")
    .Does(() =>
    {
        DeployWebsite(new DeploySettings()
        {
            SourcePath = "./AmpedBiz.Service.Host/obj/Release/Package/AmpedBiz.Service.Host.zip",
			PublishUrl = "https://publish.gear.host:8172/msdeploy.axd",
			SiteName = "ampbiz-api",
            Username = "$ampbiz-api",
            Password = "ZHPsTLr8JReX0G8g0TwPm06AfGfpm4cX2t4kPh0ofAl5LTg3xiGn1B9FLSwM"
        });
    });


///////////////////////////////////////////////////////////////////////////////
// TARGETS
///////////////////////////////////////////////////////////////////////////////

Task("Default")
    .Description("This is the default task which will be ran if no specific target is passed in.")
	.IsDependentOn("Run-Unit-Tests");


///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////

RunTarget(target);