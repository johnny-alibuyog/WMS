///////////////////////////////////////////////////////////////////////////////
// TOOLS
///////////////////////////////////////////////////////////////////////////////
#tool "nuget:?package=NUnit.ConsoleRunner"

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
	Username = EnvironmentVariable("username"),
	Password = EnvironmentVariable("password"),
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

Task("Prepare Config")
    .Description("Builds all the different parts of the project.")
    .Does(() => CopyFile("./.settings/DatabaseConfigs/" + env.DatabaseConfig, "./.settings/DatabaseConfigs/database.config.json"));

Task("Build")
    .Description("Builds all the different parts of the project.")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
	.IsDependentOn("Prepare Config")
    .Does(() =>
{
    // Build all solutions.
    foreach(var solution in gbl.Solutions)
    {
        Information("Building {0}", solution);

        MSBuild(solution, settings =>
            settings.SetPlatformTarget(PlatformTarget.MSIL)
				.SetVerbosity(Verbosity.Minimal)
                .WithProperty("TreatWarningsAsErrors", "true")
                .WithTarget("Build")
                .SetConfiguration(args.BuildConfiguration));


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
				.SetVerbosity(Verbosity.Minimal)
				.WithProperty("PublishProfile", env.PublishProfile)
                .WithProperty("UserName", env.Username)
                .WithProperty("Password", env.Password)
				.WithProperty("PackageAsSingleFile", "true")
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