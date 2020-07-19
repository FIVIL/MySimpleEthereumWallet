using Autofac;
using Autofac.Core;
using BaseWallet.Data;
using Nethereum.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BaseWallet
{
    public class Startup : IDisposable
    {
        public T Resolve<T>() => Container.Resolve<T>();
        public ContainerBuilder Builder { get; }
        private IContainer Container { get; set; }
        public Startup()
        {
            Builder = new ContainerBuilder();
        }
        private void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<DatabaseContext>().SingleInstance();
        }

        public async Task Start(string walletName = "defaultWallet", string password = "")
        {
            Configuration config;
            if (File.Exists($"{walletName}-config.json"))
            {
                var configText = await File.ReadAllTextAsync($"{walletName}-config.json");
                config = configText.FromJson<Configuration>();
            }
            else
            {
                config = new Configuration();
                config.WalletName = walletName;
                config.Password = password;
                var configtext = config.ToJson();
                await File.WriteAllTextAsync($"{walletName}-config.json", configtext);
            }
            Builder.RegisterInstance(config).SingleInstance();
            RegisterServices(Builder);
            Container = Builder.Build();
        }

        public void Dispose()
        {
            Container?.Dispose();
        }
    }
}
