using System;
using System.Collections.Generic;
using System.Text;

namespace BaseWallet.Data
{
    public class Wallet
    {
        public string Name { get; set; }
        public string PrivateKey { get; set; }
        public int KeyPath { get; set; }
        public double Balance { get; set; }
    }
}
