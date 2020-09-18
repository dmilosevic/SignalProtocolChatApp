using ClientApp.WCF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static ChatCallback chatCallback;
        public static WcfClient wcfClient;

        public MainWindow()
        {
            InitializeComponent();

            chatCallback = new ChatCallback(this.chatic, this);
            string address = ConfigurationManager.AppSettings.Get("ChatServer");
            wcfClient = new WcfClient(chatCallback, new System.ServiceModel.NetTcpBinding(), new System.ServiceModel.EndpointAddress(address));

            this.chatic.Visibility = Visibility.Hidden;

            this.WaitingDisplay.Visibility = Visibility.Hidden;
        }
    }
}
