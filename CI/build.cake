#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
#addin "Cake.FileHelpers"
#addin "Cake.IIS"


//////////////////////////////////////////////////////////////////////
// CONSTANTS
//////////////////////////////////////////////////////////////////////
const string appNameStr = "LogR";
const string commandStr = "command";
const string buildStr = "build";
const string publishStr = "publish";
const string testStr = "test";
const string pocoGeneratorStr = "pocogenerator";
const string cleanStr = "clean";
const string appPoolNameStr = "NetCoreAppApool";
const string customCommandStr = "Command";
const string webAppCodePathStr = "../Source/LogR/App/Web";
const string iisPublishFolderStr = "..//..//IISPublishedFiles";
const string iisApplicationPathStr = "/logr";
const string iisVirtualPathStr = "/";
const string sourcePathStr = "../Source";
const string stopIISStr = "stopIIS";
const string publishToIISStr = "iis";
const string servernameStr = "localhost";
var currentWebsiteNameStr = "Default Web Site";

class CommandProcess
{
    public CommandProcess(string commandName, string shortcut1, string shortcut2,string shortcut3, string description)
    {
        this.CommandName = commandName.Trim();
        this.Shortcut1 = shortcut1.Trim();
        this.Shortcut2 = shortcut2.Trim();
        this.Shortcut3 = shortcut3.Trim();        
        this.Description = description.Trim();
    }

    public string CommandName {get; private set;}
    public string Shortcut1 {get; private set;}
    public string Shortcut2 {get; private set;}
    public string Shortcut3 {get; private set;}
    public string Description {get; private set;}

    public string Shortcuts
    {
        get 
        {
            return Shortcut1 + (Shortcut2 == "" ? "" : "," + Shortcut2) +(Shortcut3 == "" ?  "" : "," + Shortcut3);
        }
    }
}

List<CommandProcess> commandList = new List<CommandProcess>()
{
    new CommandProcess(buildStr,"b", "01" , "1","build the project"),
    new CommandProcess(publishStr,"p", "02" , "2","publish the project"),
    new CommandProcess(testStr,"t", "03" , "3","test the project"),
	new CommandProcess(cleanStr,"c", "" , "","clean obj/bin file of the project"),
    new CommandProcess(pocoGeneratorStr,"pg", "" , "","generate pocos and sp function"),
	new CommandProcess(publishToIISStr,"", "" , "","publish to IIS"),
};

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var command = Argument(commandStr, "");


//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn(customCommandStr);

Task(buildStr)
    .Does(() =>
{
    Warning($"Restoring {appNameStr}:");
    DotNetCoreRestore(webAppCodePathStr);
    Warning($"Building {appNameStr}:");
    DotNetCoreBuild(webAppCodePathStr);
});

Task(publishStr)
	.IsDependentOn(stopIISStr)
    .Does(() =>
{
    Warning($"Publishing {appNameStr}:");
    DotNetCorePublish(webAppCodePathStr, new DotNetCorePublishSettings
     {
         Configuration = "Debug",
         OutputDirectory = iisPublishFolderStr,
         Framework = "net461",
     });

});

Task(testStr)
    .IsDependentOn(buildStr)
    .Does(() =>
{
    Warning($"Running {appNameStr} Tests:");
    DotNetCoreTest("../Source/LogR/Test");
});

Task(cleanStr)
    .Does(() =>
{
   
    var path = MakeAbsolute(Directory(sourcePathStr)).FullPath;
    Warning("Cleaning bin/obj folders from Path " + path);
    CleanDirectories(path + "/**/bin/" + "Debug");
    CleanDirectories(path + "/**/bin/" + "Release");
    CleanDirectories(path + "/**/obj/" + "Debug");   
    CleanDirectories(path + "/**/obj/" + "Release");
});

Task(stopIISStr)
    .Does(() =>
{
    if (PoolExists(appPoolNameStr))
    {
        Warning("Stopping IIS App Pool ...");
        StopPool(appPoolNameStr);
    }
});


Task(publishToIISStr)
    .IsDependentOn(publishStr)
    .IsDependentOn(stopIISStr)    
    .Does(() =>
{
    try
    {
        Warning("Publishing to  IIS :");

        Warning("Checking IIS Configuration ...");

        Warning("Checking if App Pool with the Name "+appPoolNameStr+" exists :");
        if (PoolExists(appPoolNameStr) == false)
        {
            Warning("App Pool "+appPoolNameStr+" does not exists. Creating..");

            CreatePool(servernameStr, new ApplicationPoolSettings()
            {
                Name = appPoolNameStr,
                Autostart = true,
                ManagedRuntimeVersion = "",
            });
        }
        else
        {
            Warning("App Pool " +appPoolNameStr+ " already exists.");
        }

        StartPool(servernameStr, appPoolNameStr);
        
        var path = MakeAbsolute(Directory(iisPublishFolderStr)).FullPath;
        Warning("Site will be mapped to Path " + path);

		Warning("Checking if Virtual Directory exists in IIS..");

        var appSetings = new ApplicationSettings(){
            SiteName = currentWebsiteNameStr,
            ApplicationPath = iisApplicationPathStr,
            ApplicationPool = appPoolNameStr,
            PhysicalDirectory = path,
            VirtualDirectory = iisVirtualPathStr
        };

        if (SiteApplicationExists(servernameStr,appSetings ) == false)
        {
            Warning("Application does not exists in IIS.");
        }
        else
        {
            Warning("Application exists in IIS..");
            Warning("Removing before creating a new one..");
            RemoveSiteApplication(appSetings);
        }

        Warning("Creating a new Application");
        AddSiteApplication(appSetings);
        Warning("Application created");
    }
    catch(AggregateException aex)
    {
        Warning("Error when creating Application");
        aex.Handle(ex => { Warning(ex.ToString()); return false; });
    }
    catch(Exception ex)
    {
        Warning("Error when creating Application");
        Warning(" Exception - " + ex.ToString());
    }
});



//////////////////////////////////////////////////////////////////////
// POCO GENERATOR RELATED TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task(pocoGeneratorStr)
    .Does(() =>
{
    Warning($"Generating {appNameStr} Pocos...");
    //DotNetCoreExecute(".\\PocoGenerator\\PocoGenerator.dll", "-config ..\\Source\\LogR\\Common\Models\\DB\\PocoTemplate\\generator_settings.json");    
});



//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////


Task(customCommandStr)
    .Does(() =>
{
    var command = Argument<string>(commandStr);
    
    if(command == null || command.Trim() == "")
    {
        if (command.Trim().Length > 0)
            Error("Invalid command");
        else
            Error("No Parameter specified");
        ShowUsage();        
        return;
    }

    command = command.Trim().ToLower();

    var item = commandList.FirstOrDefault(x=> x.CommandName.Trim().ToLower() == command || x.Shortcut1.Trim().ToLower() == command || x.Shortcut2.Trim().ToLower() == command|| x.Shortcut3.Trim().ToLower() == command);
    if (item == null)
    {
        Error("Invalid command - " + command);
        ShowUsage();
        return;
    }

    RunTarget(item.CommandName);

});


void ShowUsage()
{
    Warning("=====");
    Warning("Usage");
    Warning("=====");
    Warning("ci.bat <Command>");
    int commandPadding = 20;
    int shortcutPadding = 10;

    var header = "Command".PadRight(commandPadding,' ')+" - "+"Shortcut".PadRight(shortcutPadding,' ')+" - "+"Description";
    var line = "=".PadRight(header.Length + 30,'=');

    Warning(line);
    Warning(header);
    Warning(line);
    foreach(var item in commandList)
    {
        Warning(item.CommandName.PadRight(commandPadding,' ')+" - "+item.Shortcuts.PadRight(shortcutPadding,' ')+" - "+item.Description);
    }
    Warning(line);
}

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);