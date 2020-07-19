using System;
using System.Collections.Generic;
using System.Text;

namespace BaseWallet.Data
{
    public class Wallet
    {
        public string Name { get; set; }
        public int Index { get; set; }
        public decimal Balance { get; set; }
        public bool WatchOnly { get; set; }
    }
}
