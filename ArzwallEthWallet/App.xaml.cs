using BaseWallet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ArzwallEthWallet
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Startup Startup { get; set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Startup = new Startup();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Startup?.Dispose();
            base.OnExit(e);
        }
    }
}
