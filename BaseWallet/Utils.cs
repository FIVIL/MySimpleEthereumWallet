using System;
using System.Collections.Generic;
using System.Text;

namespace BaseWallet
{
    public static class Utils
    {
        public static string ToJson(this object obj) => Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        public static T FromJson<T>(this string json) => Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        public static dynamic FromJson(this string json) => Newtonsoft.Json.JsonConvert.DeserializeObject(json);
    }
}
