///////////////////////////////////////////////////////////////////////////////
// TOOLS
///////////////////////////////////////////////////////////////////////////////
//#addin "Cake.WebDeploy"
//#addin "Cake.MsDeploy"
#tool "nuget:?package=NUnit.ConsoleRunner"


//using Cake.MsDeploy;
//using Cake.MsDeploy.Providers;
using System.Diagnostics;
using System.IO;


///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////
var args = new 
{
	Target = Argument<string>("target", "Default"),
	BuildConfiguration = Argument<string>("build_configuration", "Release"),
};


///////////////////////////////////////////////////////////////////////////////
// ENVIRONMENT VARIABLES
///////////////////////////////////////////////////////////////////////////////
var env = new
{
	PublishProfile = EnvironmentVariable("publish_profile"),
	DatabaseConfig = EnvironmentVariable("database_config")
};


///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////
var gbl = new 
{
	Solutions = GetFiles("./**/*.sln"),
	SolutionPaths = GetFiles("./**/*.sln").Select(solution => solution.GetDirectory()),
    //BuildResultDirRelative = Directory("./.build-output"), //+ Directory("v" + semVersion) + Directory("bin");
	BuildResultDir = System.IO.Path.GetFullPath(Directory("./.build-output").ToString()),
};


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

    foreach(var path in gbl.SolutionPaths)
    {
        Information("Cleaning {0}", path);
        CleanDirectories(path + "/**/bin/" + args.BuildConfiguration);
        CleanDirectories(path + "/**/obj/" + args.BuildConfiguration);
    }
});

Task("Restore")
    .Description("Restores all the NuGet packages that are used by the specified solution.")
    .Does(() =>
{
    // Restore all NuGet packages.
    foreach(var solution in gbl.Solutions)
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
    foreach(var solution in gbl.Solutions)
    {
        Information("Building {0}", solution);

		//You need to include the 'DeployOnBuild' property to create the publish zip
        MSBuild(solution, settings =>
            settings.SetPlatformTarget(PlatformTarget.MSIL)
                .WithProperty("TreatWarningsAsErrors", "true")
                .WithTarget("Build")
                .SetConfiguration(args.BuildConfiguration));


        Information("Building Conifiguration");
		CopyFile("./.settings/DatabaseConfigs/" + env.DatabaseConfig, "./.settings/DatabaseConfigs/database.config.json");
    }
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    foreach(var path in gbl.SolutionPaths)
    {
		NUnit3(path + "/**/bin/" + args.BuildConfiguration + "/*Tests.dll");
        //MSTest(path + "/**/bin/" + args.BuildConfiguration + "/*Tests.dll");
    }
});

Task("Deploy")
    .Description("Deploy website")
    .IsDependentOn("Run-Unit-Tests")
    .Does(() =>
{
	// Build all solutions.
    foreach(var solution in gbl.Solutions)
    {
        Information("Deploying {0}", solution);

        MSBuild(solution, settings =>
            settings.SetPlatformTarget(PlatformTarget.MSIL)
				.WithProperty("PackageAsSingleFile", "true")
				.WithProperty("PublishProfile", env.PublishProfile)
				.WithProperty("WebPublishMethod", "Package")
				.WithProperty("DeployOnBuild", "true") 
				.WithProperty("DeployTarget", "MSDeployPublish")
                .WithProperty("TreatWarningsAsErrors","true")
                .WithTarget("Build")
                .SetConfiguration(args.BuildConfiguration));
    }

});


///////////////////////////////////////////////////////////////////////////////
// TARGETS
///////////////////////////////////////////////////////////////////////////////

Task("Default")
    .Description("This is the default task which will be ran if no specific target is passed in.")
	.IsDependentOn("Build");


///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////

RunTarget(args.Target);