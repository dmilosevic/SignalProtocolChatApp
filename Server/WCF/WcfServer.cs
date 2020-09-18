using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server.WCF
{
    public class WcfServer
    {
        ServiceHost serviceHost;
        string baseAddress;

        public WcfServer()
        {
            baseAddress = ConfigurationSettings.AppSettings.Get("ChatServer");
          
            NetTcpBinding binding = new NetTcpBinding();
            serviceHost = new ServiceHost(typeof(ChatService));
            serviceHost.AddServiceEndpoint(typeof(IChat), binding, baseAddress);

            StartServer();
            Console.WriteLine("Chat server is up and running...");
        }

        private void StartServer()
        {
            //if (serviceHost != null)
            //    serviceHost.Close();

            serviceHost.Open();
        }

        public void StopServer()
        {
            serviceHost.Close();
        }
    }
}
