using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace BaseWallet
{
    public static class Utils
    {
        public static string ToJson(this object obj) => Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        public static T FromJson<T>(this string json) => Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        public static dynamic FromJson(this string json) => Newtonsoft.Json.JsonConvert.DeserializeObject(json);
        public static byte[] ToUtf8Bytes(this string text) => System.Text.Encoding.UTF8.GetBytes(text);
        public static string FromUtf8Byte(this byte[] bytes) => System.Text.Encoding.UTF8.GetString(bytes);
        public static byte[] ToBase64Bytes(this string text) => Convert.FromBase64String(text);
        public static string FromBase64Byte(this byte[] bytes) => Convert.ToBase64String(bytes);
    }
}
