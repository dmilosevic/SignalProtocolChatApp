using Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ServerData
    {
        public static Dictionary<string, Client> Clients;

        public ServerData()
        {
            Clients = new Dictionary<string, Client>();
        }

        /// <summary>
        /// *Assumption being there are only 2 clients talking to eachother
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetPartnerClientID(string id)
        {
            foreach (var s in Clients.Keys)
            {
                if (s != id)
                    return s;
            }
            return null;
        }
    }
}
