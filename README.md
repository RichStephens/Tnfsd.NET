# Tnfsd.NET

Program to easily install and maintain tnfsd.exe (See https://github.com/FujiNetWIFI/fujinet-firmware/wiki/Setting-up-a-TNFS-Server) on Windows 10/11.
The program has the ability to download tnfsd.exe and install it into a folder.  You can then specify what folder you want to share with TNFS
and create a Windows Task Scheduler task to run it automatically whenever Windows starts.  You can also set up a firewall exception for TNFS automatically
when creating the service.  When running, Tnfsd.NET will allow you to delete and recreate tnfsd at any time, as well as start/stop tnfsd.

To run:  Download the executable and the appsettings.json file from [h](https://github.com/RichStephens/Tnfsd.NET/releases/latest) and place them in any folder, 
then click on the executable to run it.

NOTE: This application requires .NET 8.0 https://dotnet.microsoft.com/en-us/download/dotnet/8.0
