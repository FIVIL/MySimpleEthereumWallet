using System;
using System.Collections.Generic;
using System.Text;

namespace BaseWallet.Data
{
    public class SendTransaction
    {
        public Guid Id { get; set; }
        public string Hash { get; set; }
        public bool Success { get; set; }
        public DateTime TransactionTime { get; set; }
    }
}
