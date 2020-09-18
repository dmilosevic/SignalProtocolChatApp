using ClientApp.WCF;
using libsignal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientApp
{
    /// <summary>
    /// Interaction logic for ChatControl.xaml
    /// </summary>
    public partial class ChatControl : UserControl
    {
        public ChatControl()
        {
            InitializeComponent();

            richTextBox.Focus();

            this.Visibility = Visibility.Hidden;
        }

        private void SendMessage()
        {
            string richText = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text;

            if (string.IsNullOrEmpty(richText))
                return;

            if (IsInitialMessage())
                InitialMessagePrep();


            rtbChat.Document.Blocks.LastBlock.TextAlignment = TextAlignment.Right;
            rtbChat.Document.Blocks.LastBlock.Foreground = Brushes.Blue;
            rtbChat.AppendText(richText);

            rtbChat.ScrollToEnd();
            richTextBox.Document.Blocks.Clear();

            var cypherMsg = ClientData.SessionCipher.encrypt(Encoding.UTF8.GetBytes(richText));
            //string cypherMsg = null;
            MainWindow.wcfClient.SendMessage(cypherMsg);
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        private void RichTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage();
            }
        }

        private void InitialMessagePrep()
        {
            string name = "omitted on purpose"; //hard coded, fix it
            var bundle = MainWindow.wcfClient.FetchBundle(name);
            var signalProtocolAddress = new SignalProtocolAddress(name, bundle.getDeviceId());

            ClientData.SessionBuilder = new SessionBuilder(
                    ClientData.InMemorySignalProtocolStore, signalProtocolAddress);

            ClientData.SessionBuilder.process(bundle); //Set up session and DR parameters and store it

            ClientData.SessionCipher = new SessionCipher(ClientData.InMemorySignalProtocolStore, signalProtocolAddress);

            //session builder msm da ne mora da se cuva u ClientData, on samo na pocetku treba na napravi i pohrani sesiju
            //svaki sledeci put, koristi se samo SessionCipher
        }

        private bool IsInitialMessage()
        {
            return ClientData.SessionCipher is null; //npr
        }
    }
}
