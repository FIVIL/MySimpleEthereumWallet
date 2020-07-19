using BaseWallet.Data;
using NBitcoin;
using Nethereum.HdWallet;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wallet = Nethereum.HdWallet.Wallet;

namespace BaseWallet.Services
{
    public interface IWalletManagerService
    {

    }
    public class WalletManagerService : IWalletManagerService, IDisposable
    {
        private DatabaseContext DatabaseContext { get; }
        private Configuration Configuration { get; }

        public WalletManagerService(DatabaseContext databaseContext, Configuration configuration)
        {
            DatabaseContext = databaseContext;
            Configuration = configuration;
        }

        private Wallet Wallet { get; set; }
        private PublicWallet PublicWallet { get; set; }

        public string[] GenerateWallet(string password)
        {
            var mnemo = new Mnemonic(Wordlist.English, WordCount.Twelve);
            Wallet = new Wallet(string.Join(' ', mnemo.Words), password);
            return mnemo.Words;
        }
        public void GenerateWallet(string[] seed, string password)
        {
            Wallet = new Wallet(string.Join(' ', seed), password);
        }

        public void GenerateWatchOnlyWallet(string extPubKey)
        {
            PublicWallet = new PublicWallet(extPubKey);
        }
        public string ExportExtendedPublicKey() => Wallet?.GetMasterExtPubKey()?.ToBytes()?.ToHex() ?? throw new NullReferenceException();

        public string GetAddress(int index)
        {
            if (!(Wallet is null))
            {
                return Wallet.GetAccount(index).Address;
            }
            else if (!(PublicWallet is null))
            {
                return PublicWallet.GetAddress(index);
            }
            else throw new NullReferenceException();
        }
        public string GenerateNewAddress()
        {
            var w = GetWalletFromDatabase();
            w.Index++;
            DatabaseContext.Wallets.Update(w);
            return GetAddress(w.Index);
        }
        public async Task<SendTransaction> CreateUnsignedTransaction(int index, string toAddress, decimal value)
        {
            var web3 = new Web3(Wallet.GetAccount(index), Configuration.InfuraLink);
            var res = await web3.Eth.GetEtherTransferService()
                 .TransferEtherAndWaitForReceiptAsync(toAddress, value);
            var tx = new SendTransaction
            {
                Id = Guid.NewGuid(),
                Hash = res.TransactionHash,
                Success = res.Succeeded(),
                TransactionTime = DateTime.UtcNow,
            };
            DatabaseContext.SentTransactions.Insert(tx);
            return tx;
        }
        public async Task<decimal> GetBalance(int index)
        {
            var web3 = new Web3(Wallet.GetAccount(index), Configuration.InfuraLink);
            var balance = await web3.Eth.GetBalance.SendRequestAsync(GetAddress(index));
            var etherAmount = Web3.Convert.FromWei(balance.Value);
            var w = GetWalletFromDatabase();
            w.Balance = etherAmount;
            DatabaseContext.Wallets.Update(w);
            return etherAmount;
        }

        public Data.Wallet GetWalletFromDatabase()
        {
            var w = DatabaseContext.Wallets.FindOne(x => x.Name == Configuration.WalletName);
            if (w is null) w = new Data.Wallet
            {
                Balance = 0,
                Index = 0,
                Name = Configuration.WalletName,
                WatchOnly = !(PublicWallet is null) && (Wallet is null)
            };
            DatabaseContext.Wallets.Insert(w);
            return w;
        }

        public long GetCurrentIndex() => GetWalletFromDatabase().Index;
        public void Dispose()
        {
            Wallet = null;
            PublicWallet = null;
        }
    }
}
