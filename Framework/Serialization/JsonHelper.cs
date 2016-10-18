using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using Newtonsoft.Json;

namespace Mn.Framework.Serialization
{
    public class JsonHelper
    {

        /// <summary>
        /// JSON Serialization
        /// </summary>
        public static string JsonSerializer<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        public static string JsonSerializer(dynamic obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        public static string JsonComplexSerializer<T>(T obj)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            return JsonSerializer<T>(obj, settings);
        }
        public static string JsonSerializer<T>(T obj, JsonSerializerSettings settings)
        {
            var jsonString = JsonConvert.SerializeObject(obj, settings);
            return jsonString;
        }
        /// <summary>
        /// JSON Deserialization
        /// </summary>       
        public static T JsonDeserialize<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
        public static T JsonComplexDeserialize<T>(string jsonString)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            return JsonDeserialize<T>(jsonString, settings);
        }
        public static T JsonDeserialize<T>(string jsonString, JsonSerializerSettings settings)
        {
            var obj = JsonConvert.DeserializeObject<T>(jsonString, settings);
            return obj;
        }



        /// <summary>
        ///Microsoft JSON Serialization
        /// </summary>
        public static string MicrosoftJsonSerializer<T>(T t)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, t);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return jsonString;
        }
        /// <summary>
        ///Microsoft JSON Deserialization
        /// </summary>
        public static T MicrosoftJsonDeserialize<T>(string jsonString)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            T obj = (T)ser.ReadObject(ms);
            return obj;
        }

        public static string EnumToJson<EnumType>() where EnumType : struct, IConvertible
        {
            return "\"" + typeof(EnumType).Name + "\"" +
                   ":[" +
                   string.Join(",", Enum.GetValues(typeof(EnumType)).Cast<EnumType>().Select(e => "\"" + e.ToString() + "\"")) +
                   "]";
        }

        //public static string JsonSerializer(T t)
        //{
        //    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
        //    MemoryStream ms = new MemoryStream();
        //    ser.WriteObject(ms, t);
        //    string jsonString = Encoding.UTF8.GetString(ms.ToArray());
        //    ms.Close();
        //    //Replace Json Date String                                         
        //    string p = @"\\/Date\((\d+)\+\d+\)\\/";
        //    MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertJsonDateToDateString);
        //    Regex reg = new Regex(p);
        //    jsonString = reg.Replace(jsonString, matchEvaluator);
        //    return jsonString;
        //}

        ///// <summary>
        ///// JSON Deserialization
        ///// </summary>
        //public static T JsonDeserialize(string jsonString)
        //{
        //    //Convert "yyyy-MM-dd HH:mm:ss" String as "\/Date(1319266795390+0800)\/"
        //    string p = @"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}";
        //    MatchEvaluator matchEvaluator = new MatchEvaluator(
        //        ConvertDateStringToJsonDate);
        //    Regex reg = new Regex(p);
        //    jsonString = reg.Replace(jsonString, matchEvaluator);
        //    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
        //    MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
        //    T obj = (T)ser.ReadObject(ms);
        //    return obj;
        //}

        ///// <summary>
        ///// Convert Serialization Time /Date(1319266795390+0800) as String
        ///// </summary>
        //private static string ConvertJsonDateToDateString(Match m)
        //{
        //    string result = string.Empty;
        //    DateTime dt = new DateTime(1970, 1, 1);
        //    dt = dt.AddMilliseconds(long.Parse(m.Groups[1].Value));
        //    dt = dt.ToLocalTime();
        //    result = dt.ToString("yyyy-MM-dd HH:mm:ss");
        //    return result;
        //}

        ///// <summary>
        ///// Convert Date String as Json Time
        ///// </summary>
        //private static string ConvertDateStringToJsonDate(Match m)
        //{
        //    string result = string.Empty;
        //    DateTime dt = DateTime.Parse(m.Groups[0].Value);
        //    dt = dt.ToUniversalTime();
        //    TimeSpan ts = dt - DateTime.Parse("1970-01-01");
        //    result = string.Format("\\/Date({0}+0800)\\/", ts.TotalMilliseconds);
        //    return result;
        //}
    }
}
