using System;
using System.Diagnostics;
using System.IO;

namespace PlatformFileServer
{
    public class FileService : IFileService
    {
        public string[] GetFolders(String path)
        {
			try
			{
				FormMain.Display("Entry: GetFolders(" + path + ")");
				if (Directory.Exists(path))
				{
					string[] directories = Directory.GetDirectories(path);
					FormMain.Display("... Exit with " + directories.Length + " folder names");
					return directories;
				}
			} catch (Exception ex)
			{
				FormMain.Display("... Exception: "+ex.Message);
				Debug.WriteLine("GetFolders exception:" + ex.Message);
			}
        	FormMain.Display("... Exit with null");
        	return null;    // Invalid path
        }

        public string[] GetFiles(String path)
        {
			try
			{
				FormMain.Display("Entry: GetFiles(" + path + ")");
				if (Directory.Exists(path))
				{
					string [] files = Directory.GetFiles(path);
					FormMain.Display("... Exit with " + files.Length + " file names");
					return files;
				}
			} catch (Exception ex)
			{
				FormMain.Display("... Exception: " + ex.Message);
				Debug.WriteLine("GetFiles exception:" + ex.Message);				
			}
			FormMain.Display("... Exit with null");
			return null;    //Invalid path
        }

    }
}
