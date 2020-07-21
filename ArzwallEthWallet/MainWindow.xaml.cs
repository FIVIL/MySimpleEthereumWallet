using BaseWallet;
using BaseWallet.Services;
using NBitcoin;
using System;
using System.Collections.Generic;
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

namespace ArzwallEthWallet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var s = new Startup();
            await s.Start();
            var w = s.Resolve<IWalletManagerService>();
            var seed = w.GenerateWallet("test");
            var expub = w.ExportExtendedPublicKey();
            var add1 = w.GetAddress(0);
            var s2 = new Startup();
            await s2.Start("w2");
            var w2 = s2.Resolve<IWalletManagerService>();
            w2.GenerateWatchOnlyWallet(expub);
            Address.Items.Add(new TextBox
            {
                Text = string.Join(' ', seed)
            });
            for (int i = 0; i < 20; i++)
            {
                Address.Items.Add(w.GetAddress(i));
            }
        }
    }
}
