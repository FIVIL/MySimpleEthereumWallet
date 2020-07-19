using System;
using System.Collections.Generic;
using System.Text;

namespace BaseWallet.Data
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public double Value { get; set; }
        public string Hash { get; set; }
        public long TimeStamp { get; set; }
        public string Raw { get; set; }
        public long Block { get; set; }
        public DateTime Time { get; set; }
    }
}
