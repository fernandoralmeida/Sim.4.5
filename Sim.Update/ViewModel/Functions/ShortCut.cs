using IWshRuntimeLibrary;

namespace Sim.Update.ViewModel.Functions
{
    class ShortCut
    {
        public static void Createlnk(string shortcutName, string shortcutPath, string targetFileLocation)
        {
            string shortcutLocation = System.IO.Path.Combine(shortcutPath, shortcutName + ".lnk");
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

            shortcut.Description = "Sistema Modular"; // The description of the shortcut
            shortcut.IconLocation = Common.Folders.AppData_Sim + "\\" + "sim.apps.icon.ico"; // The icon of the shortcut
            shortcut.TargetPath = targetFileLocation; // The path of the file that will launch when the shortcut is run
            shortcut.WorkingDirectory = Common.Folders.AppData_Sim;
            shortcut.Save(); // Save the shortcut
        }
    }
}
