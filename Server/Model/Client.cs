using Common.Interfaces;
using libsignal.state;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model
{
    public class Client
    {
        public string SessionID { get; set; }
        public string Username { get; set; }
        public IChatCallback ChatCallback { get; set; }
        public PreKeyBundle PreKeyBundle { get; set; }
        
    }
}
