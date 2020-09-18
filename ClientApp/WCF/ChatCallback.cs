using Common.Interfaces;
using libsignal;
using libsignal.protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ClientApp.WCF
{
    public class ChatCallback : IChatCallback
    {
        ChatControl chatctrl;

        public MainWindow mw;

        public ChatCallback(ChatControl chatctrl, MainWindow mw)
        {
            this.chatctrl = chatctrl;
            this.mw = mw;
        }
        public void ForwardMsg(CiphertextMessage cypherMsg)
        {
            byte[] data = null;
            
            //sledi grbav grbav kod...
            if (cypherMsg is SignalMessage)
            {
                //cypherMsg = new SignalMessage(cypherMsg.serialize());
                data = ClientData.SessionCipher.decrypt((SignalMessage)cypherMsg);
            }
            else if(cypherMsg is PreKeySignalMessage)
            {
                if (ClientData.SessionBuilder == null)
                {
                    ClientData.SessionBuilder = new libsignal.SessionBuilder(
                        ClientData.InMemorySignalProtocolStore, new SignalProtocolAddress(
                                                "Alice", ((PreKeySignalMessage)cypherMsg).getRegistrationId()));
                }
                if (ClientData.SessionCipher == null)
                {
                    ClientData.SessionCipher = new SessionCipher(ClientData.InMemorySignalProtocolStore,
                        new SignalProtocolAddress("Alice", ((PreKeySignalMessage)cypherMsg).getRegistrationId()));
                }

                data = ClientData.SessionCipher.decrypt(new PreKeySignalMessage(cypherMsg.serialize()));
            }

            var plainText = Encoding.UTF8.GetString(data);
            DisplayInChatWindow(plainText);
        }

        public void SendSymetricKey(string cypherKey)
        {
            MessageBox.Show("Server called back with msg: {0}", cypherKey);
        }

        public void SignalStart(string username)
        {
            mw.WaitingDisplay.Visibility = Visibility.Hidden;
            chatctrl.Visibility = Visibility.Visible;

            mw.Title = username;
        }

        private void DisplayInChatWindow(string textToDisplay)
        {
            chatctrl.rtbChat.Document.Blocks.LastBlock.TextAlignment = TextAlignment.Left;
            chatctrl.rtbChat.Document.Blocks.LastBlock.Foreground = Brushes.Red;


            chatctrl.rtbChat.AppendText(textToDisplay);

            chatctrl.rtbChat.ScrollToEnd();
        }
    }
}
