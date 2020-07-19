using System;
using System.Collections.Generic;
using System.Text;

namespace BaseWallet
{
    public class Configuration
    {
        public string DbPath { get; set; } = "db";
        public string GetDbPath(string walletName) => $"{DbPath}/{walletName}.db";
        public string PrivateKeysPath { get; set; } = "keys";
        public string GetPrivateKeyFile(string walletName) => $"{PrivateKeysPath}/{walletName}.dat";

        public string Password { get; set; }
        public string WalletName { get; set; }
    }
}
