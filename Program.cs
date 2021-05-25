using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace One_Click_Server
{
    static class Program
    {
        private static bool guiOpen = false;
        private static bool firstOpen = true;
        
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GUI());   
        }

        static void OpenMenu()
        {
            
        }
        
        static void ListAllCommands()
        {
            // list all server commands
            Console.WriteLine(
                "\nInterface:" +
                "     1.  Open or close the GUI (gui)" +
                "Configuration commands: \n" +
                "     1.  Change the active game port \n" +
                "     2.  Change the maximum number of players \n" +
                "     3.  Change the maximum connection backlog \n" +
                "Player commands: \n" +
                "     1.  List all registered players (allplayers) \n" +
                "     2.  List all connected players (players) \n" +
                "     3.  List all current sessions (sessions) \n" +
                "     4.  Ban IP address (ban IP_ADDRESS) \n" +
                "     5.  Unban IP address (unban IP_ADDRESS) \n" +
                "     6.  Ban from voice (mute IP_ADDRESS) \n" +
                "     7.  Unban from voice (unmute IP_ADDRESS) \n" +
                "     8.  Ban from text (chatban IP_ADDRESS) \n" +
                "     9.  Unban from text (chatunban IP_ADDRESS) \n" +
                "     10. Promote user to server admin (promote admin USERNAME)\n" +
                "     11. Demote user from server admin (demote admin USERNAME)\n" +
                "Server commands: \n" +
                "     1.  Timed shutdown with warning to all players (shutdown s=30) \n" +
                "     2.  Exit program immediately (exit)\n");
        }
    }
}
