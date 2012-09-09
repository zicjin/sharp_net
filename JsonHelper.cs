using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace sharp_net {

    public class JsonHelper {

        /// <summary>
        /// JSON序列化
        /// </summary>
        public static string JsonSerializer<T>(T t) {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, t);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return jsonString;
        }

        /// <summary>
        /// JSON反序列化
        /// </summary>
        public static T JsonDeserialize<T>(string jsonString) {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            T obj = (T)ser.ReadObject(ms);
            return obj;
        }
    }
}