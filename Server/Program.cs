using Server.WCF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            WcfServer wcfServer = new WcfServer();
            ServerData serverData = new ServerData();

            Console.WriteLine("Press ENTER to shut down the server.");
            Console.ReadLine();
            wcfServer.StopServer();
        }
    }
}
