using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BaseWallet.Services
{
    public interface IFileEncryptionService
    {
        Task EncryptWriteAsync(string privateKey, Guid walletPassword, string path, string password);
        Task<(string privateKey, Guid password)> DecryptReadAsync(string path, string password);
    }
    public class FileEncryptionService: IFileEncryptionService
    {
        class DataModel
        {
            public byte[] Cipher { get; set; }
            public byte[] Tag { get; set; }
            public byte[] Nonce { get; set; }
        }
        public async Task EncryptWriteAsync(string privateKey, Guid walletPassword, string path, string password)
        {
            var clear = $"{privateKey};{walletPassword}";
            var clearpBytes = clear.ToUtf8Bytes();
            var (cipher, tag, Nonce) = Encrypt(clearpBytes, password);
            var d = new DataModel
            {
                Cipher = cipher,
                Nonce = Nonce,
                Tag = tag
            };
            var ds = d.ToJson().ToUtf8Bytes();
            await File.WriteAllBytesAsync(path, ds);
        }

        public async Task<(string privateKey, Guid password)> DecryptReadAsync(string path, string password)
        {
            var data = (await File.ReadAllBytesAsync(path)).FromUtf8Byte().FromJson<DataModel>();
            var dec = DecryptByte(data.Cipher, data.Tag, data.Nonce, password).FromUtf8Byte().Split(';');
            return (dec[0], Guid.Parse(dec[1]));
        }


        private int saltSize = 12;
        private int passWordSize = 32;
        private int iterationCount = 50_000;
        private int tagSize = 16;
        private (byte[] cipher, byte[] tag, byte[] Nonce) Encrypt(byte[] plainBytes, string rawPassword)
        {
            var (Key, Nonce) = GeneratePassword(rawPassword);
            byte[] tag = new byte[tagSize];
            byte[] cipher = new byte[plainBytes.Length];
            using var aesGcm = new AesGcm(Key);
            aesGcm.Encrypt(Nonce, plainBytes, cipher, tag);
            return (cipher, tag, Nonce);
        }

        private byte[] DecryptByte(byte[] cipher, byte[] tag, byte[] Nonce, string rawPassword)
        {
            var Key = GeneratePassword(rawPassword, Nonce);
            byte[] plain = new byte[cipher.Length];
            using var aesGcm = new AesGcm(Key);
            aesGcm.Decrypt(Nonce, cipher, tag, plain);
            return plain;
        }

        private (byte[] Key, byte[] Nonce) GeneratePassword(string rawPass)
        {
            using var rng = new RNGCryptoServiceProvider();
            byte[] s = new byte[saltSize];
            byte[] r;
            rng.GetBytes(s);
            using var t = new Rfc2898DeriveBytes(rawPass, s, iterationCount, HashAlgorithmName.SHA256);
            r = t.GetBytes(passWordSize);
            return (r, s);
        }
        private byte[] GeneratePassword(string rawPass, byte[] salt)
        {
            byte[] r;
            using var t = new Rfc2898DeriveBytes(rawPass, salt, iterationCount, HashAlgorithmName.SHA256);
            r = t.GetBytes(passWordSize);
            return (r);
        }
    }
}
