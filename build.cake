var target = Argument("target", "Run");

Task("Build")
    .Does(() =>
{
	DotNetCoreBuild("./IreckonuShop.sln");
});

Task("Run_JsonStorage")
	.IsDependentOn("Build")
    .Does(() =>
{
	DotNetCoreRun("./src/IreckonuShop.API/IreckonuShop.API.csproj", "--urls=http://localhost:5001/ --environment Json");
});

Task("Run_SqlStorage")
	.IsDependentOn("Build")
    .Does(() =>
{
	DotNetCoreRun("./src/IreckonuShop.API/IreckonuShop.API.csproj", "--urls=http://localhost:5001/ --environment Sql");
});

Task("BuildUnitTests")	
    .Does(() =>
{
	DotNetCoreBuild("./tests/Unit/IreckonuShop.PersistenceLayer.FileSystem.UnitTests/IreckonuShop.PersistenceLayer.FileSystem.UnitTests.csproj");	
	DotNetCoreBuild("./tests/Unit/IreckonuShop.PersistenceLayer.RelationalDb.UnitTests/IreckonuShop.PersistenceLayer.RelationalDb.UnitTests.csproj");	
});

Task("RunUnitTests")
	.IsDependentOn("BuildUnitTests")
    .Does(() =>
{
	DotNetCoreTest("./tests/Unit/IreckonuShop.PersistenceLayer.FileSystem.UnitTests/IreckonuShop.PersistenceLayer.FileSystem.UnitTests.csproj");	
	DotNetCoreTest("./tests/Unit/IreckonuShop.PersistenceLayer.RelationalDb.UnitTests/IreckonuShop.PersistenceLayer.RelationalDb.UnitTests.csproj");
});


RunTarget(target);