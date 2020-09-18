using Common.Interfaces;
using libsignal.protocol;
using libsignal.state;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server.WCF
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class ChatService : IChat
    {
        public string Connect(string Username, PreKeyBundle preKeyBundle)
        {
            if (ServerData.Clients.Count >= 2)
                return "";

            var callbackInstance = OperationContext.Current.GetCallbackChannel<IChatCallback>();

            string sessionID = OperationContext.Current.SessionId;
            Console.WriteLine("Konektovan klijent {0}", Username);

            ServerData.Clients[sessionID] = new Model.Client()
            {
                SessionID = sessionID,
                ChatCallback = callbackInstance,
                Username = Username,
                PreKeyBundle = preKeyBundle,
            };
                       
            if(ServerData.Clients.Count >= 2)
            {
                string[] usernames = new string[] { "Alisa", "Bobo" };
                int index = 0;
                //start chat
                foreach (var client in ServerData.Clients.Values)
                {
                    try
                    {
                        Task.Factory.StartNew(() => { client.ChatCallback.SignalStart(usernames[index++]); });
                    }
                    catch
                    {
                    }
                }
            }

            return "Registration Successful";
        }

        public PreKeyBundle FetchBundle(string BobsUsername)
        {
            //var bundle = ServerData.Clients.FirstOrDefault(x =>
            //    x.Value.Username.ToLower().Equals(BobsUsername.ToLower())).Value.PreKeyBundle;

            //simplification (2 clients)
            var sessionId = OperationContext.Current.SessionId;
            var bundle = ServerData.Clients.Values.FirstOrDefault(x => x.SessionID.Equals(ServerData.GetPartnerClientID(sessionId))).PreKeyBundle;

            return bundle;
        }

        
        public void SendMessage(CiphertextMessage cypherMsg)
        {
            var sendingClient = OperationContext.Current.SessionId;
            cypherMsg.serialize();

            /*
             * Forward to other user.
             * There is a simplification here, I assume there are only 2 clients who talk only to eachother.
             */
            var receivingClient = ServerData.GetPartnerClientID(sendingClient);

            if (receivingClient == null)
                throw new NullReferenceException("Receiving client not found.");


            //forward msg to receiving client
            try
            {
                ServerData.Clients[receivingClient].ChatCallback.ForwardMsg(cypherMsg);
            }
            catch (Exception ex)
            {
            }
            
        }
    }
}
