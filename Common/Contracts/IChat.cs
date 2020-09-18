using libsignal.protocol;
using libsignal.state;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    [ServiceContract(CallbackContract = typeof(IChatCallback))]
    public interface IChat
    {
        /// <summary>
        /// Registers on the server
        /// </summary>
        /// <param name="MyUsername">Human readable ID. (Not relevant in simplified 2-client version)</param>
        /// <param name="preKeyBundle">Keys to be stored on server. Provided on request to users who want to chat with you</param>
        /// <returns></returns>
        [OperationContract]
        string Connect(string MyUsername, PreKeyBundle preKeyBundle);

        /// <summary>
        /// Chat message intended for the other party
        /// </summary>
        /// <param name="cypherMsg"></param>
        [OperationContract]
        void SendMessage(CiphertextMessage cypherMsg);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BobsUsername">(Not relevant in simplified 2-client version)</param>
        /// <returns>Other party's keys needed for initiating protocol</returns>
        [OperationContract]
        PreKeyBundle FetchBundle(string BobsUsername);
    }

    public interface IChatCallback
    {
        /// <summary>
        /// Forwards msg to the receiving client
        /// </summary>
        /// <param name="cypherMsg"></param>
        [OperationContract]
        void ForwardMsg(CiphertextMessage cypherMsg);

        /// <summary>
        /// Signals to both users that the other party has connected and the chat session may begin
        /// </summary>
        [OperationContract]
        void SignalStart(string username);
    }
}
