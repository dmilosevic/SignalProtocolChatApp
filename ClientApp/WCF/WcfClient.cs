using Common.Interfaces;
using libsignal.protocol;
using libsignal.state;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClientApp.WCF
{
    public class WcfClient : DuplexChannelFactory<IChat>, IChat, IDisposable
    {
        IChat proxy;

        public WcfClient(object callbackInstance, NetTcpBinding binding, EndpointAddress ep) : base(callbackInstance, binding, ep)
        {
            // serviceHost.Description.Behaviors.Find<ServiceBehaviorAttribute>().MaxItemsInObjectGraph = int.MaxValue;
           
            proxy = this.CreateChannel();
            
        }

        public string Connect(string Username, PreKeyBundle preKeyBundle)
        {
            try
            {
                return proxy.Connect(Username, preKeyBundle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            
        }

        public PreKeyBundle FetchBundle(string BobsUsername)
        {
            try
            {
                return proxy.FetchBundle(BobsUsername);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public void SendMessage(CiphertextMessage cypherMsg)
        {
            try
            {
                proxy.SendMessage(cypherMsg);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }
    }
}
