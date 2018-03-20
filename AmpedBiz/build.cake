///////////////////////////////////////////////////////////////////////////////
// ADDINS & TOOLS
///////////////////////////////////////////////////////////////////////////////
//#addin "Cake.IISExpress"
#addin "Cake.FileHelpers"
#tool "nuget:?package=OpenCover"
#tool "nuget:?package=ReportGenerator"
#tool "nuget:?package=NUnit.ConsoleRunner"

//using Cake.IISExpress;
using System;
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
	Solutions = GetFiles("./**/AmpedBiz.sln"),
	SolutionPaths = GetFiles("./**/AmpedBiz.sln").Select(solution => solution.GetDirectory()),
    //BuildResultDirRelative = Directory("./.build-output"), //+ Directory("v" + semVersion) + Directory("bin");
	BuildResultDir = System.IO.Path.GetFullPath(Directory("./.build-output").ToString()),
	//IisExpressProcess = (IAdvProcess)null;
};


///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup((context) =>
{
    // Executed BEFORE the first task.
    Information("Running tasks...");
	//Information(AppDomain.CurrentDomain.BaseDirectory);
});

Teardown((context) =>
{
	//if (gbl.IisExpressProcess == null)
    //{
    //    return;
    //}
	//
    //try
    //{
    //    gbl.IisExpressProcess.Kill();
    //}
    //finally
    //{
    //    gbl.IisExpressProcess.Dispose();
    //    Context.Log.Information("Disposed IIS Express process");
	//}

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
    .Does(() => 
{
	if (!string.IsNullOrWhiteSpace(env.DatabaseConfig))
	{
		CopyFile("./.settings/DatabaseConfigs/" + env.DatabaseConfig, "./.settings/DatabaseConfigs/database.config.json");
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

        MSBuild(solution, settings => settings
			.SetPlatformTarget(PlatformTarget.MSIL)
			.SetVerbosity(Verbosity.Minimal)
            .WithProperty("TreatWarningsAsErrors", "true")
            .WithTarget("Build")
            .SetConfiguration(args.BuildConfiguration)
		);
    }
});

Task("Run-Test")
    .IsDependentOn("Build")
    .Does(() =>
{
	
    foreach(var path in gbl.SolutionPaths)
    {
		var test = new 
		{
			AssemblyPath = path + "/**/bin/" + args.BuildConfiguration + "/AmpedBiz.Test*.dll",
			ResultFile = "./result.xml",
			CoverageOutput = ".coverage"
		};
 
		//var testFile = path + "/**/bin/" + args.BuildConfiguration + "/*Tests.dll";
		//var testResultFile = "./result.xml";

		OpenCover(context => 
			context.NUnit3(test.AssemblyPath),
			new FilePath(test.ResultFile),
			new OpenCoverSettings() { ReturnTargetCodeOffset = 0 }
				.WithFilter("+[AmpedBiz.*]*")
				.WithFilter("-[AmpedBiz.Test*]*")
		);

		ReportGenerator(test.ResultFile, test.CoverageOutput);

		//NUnit3(path + "/**/bin/" + args.BuildConfiguration + "/*Tests.dll");
        //MSTest(path + "/**/bin/" + args.BuildConfiguration + "/*Tests.dll");
    }
});

//Task("Run-App-Service.Host")
//    .IsDependentOn("Build")
//    .Does(context =>
//{
//	var appPath = Argument<string>("sitePath", @".\AmpedBiz.Service.Host");
//	var portNumber = Argument<int?>("portNumber", 49561);
//
//    var settings = new AppPathBasedIISExpressSettings(appPath)
//    {
//        PortNumber  = portNumber,
//    };
//
//    Verbose("AppPathBasedIISExpressSettings.AppPath: {0}", settings.AppPath);
//    Verbose("AppPathBasedIISExpressSettings.PortNumber: {0}", settings.PortNumber);
//
//    iisExpressProcess = StartIISExpress(settings);
//
//    iisExpressProcess.Exited += (sender, args1) =>
//    {
//        Information("IIS Express exited with code: {0}", args1.ExitCode);
//    };
//
//    iisExpressProcess.OutputDataReceived += (sender, args1) =>
//    {
//        if (args1.Output != null)
//        {
//            Information("IIS Express output: {0}", args1.Output);
//        }
//    };
//
//    iisExpressProcess.ErrorDataReceived += (sender, args1) => 
//	{ 
//		Error("IIS Express error: {0}", args1.Output); 
//	};
//});

Task("Seed-Data")
    .Description("Seed data")
    .IsDependentOn("Build")
	.Does(() =>
{
	var projectName = "AmpedBiz.Data.Initializer";

	var seedingCommand = gbl.SolutionPaths.FirstOrDefault() + "/" + projectName + "/bin/" + args.BuildConfiguration + "/" + projectName + ".exe";
	
	Information("Seeding Data Started: {0}", seedingCommand);

	var process = StartProcess(seedingCommand);
	
	Information("Seeding Data Finished: {0}", process);
});

Task("Deploy")
    .Description("Deploy website")
	.IsDependentOn("Prepare Config")
    .IsDependentOn("Run-Test")
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