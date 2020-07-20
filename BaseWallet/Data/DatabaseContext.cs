using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BaseWallet.Data
{
    public class DatabaseContext : IDisposable
    {

        private LiteDatabase Database { get; }
        public DatabaseContext(Configuration configuration)
        {
            if (!Directory.Exists(configuration.DbPath)) Directory.CreateDirectory(configuration.DbPath);
            Database = new LiteDatabase(configuration.GetDbPath(configuration.WalletName));
            CreateCollections();

        }
        public ILiteCollection<Wallet> Wallets { get; private set; }
        public ILiteCollection<Transaction> Transactions { get; private set; }
        public ILiteCollection<SendTransaction> SentTransactions { get; private set; }
        private void CreateCollections()
        {
            Wallets = Database.GetCollection<Wallet>(nameof(Wallets));
            Transactions = Database.GetCollection<Transaction>(nameof(Transactions));
            SentTransactions = Database.GetCollection<SendTransaction>(nameof(SentTransactions));
        }

        public void Dispose()
        {
            Database?.Dispose();
        }
    }
}
