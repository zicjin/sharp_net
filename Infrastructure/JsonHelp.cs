using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharp_net {
    public static class JsonHelp {

        // 处理 Date(1245398693390)/
        // 通常情况下，应该在Js端使用Json.js处理，更为流畅。http://goo.gl/VEff8。这里是为了正巧处理Highstock的特殊数据需求。
        // 来源 http://goo.gl/R8h25
        public static double UnixTicks(this DateTime dt) {
            DateTime tempDt = dt.AddHours(TimeZoneOffset);
            DateTime d1 = new DateTime(1970, 1, 1);
            DateTime d2 = tempDt.ToUniversalTime();
            TimeSpan ts = new TimeSpan(d2.Ticks - d1.Ticks);
            return ts.TotalMilliseconds;
        }
        private static int TimeZoneOffset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours;

    }

    public abstract class JsonCreationConverter<T> : JsonConverter {
        protected abstract T Create(Type objectType, JObject jsonObject);

        public override bool CanConvert(Type objectType) {
            return typeof(T).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            var jsonObject = JObject.Load(reader);
            T target = Create(objectType, jsonObject);
            serializer.Populate(jsonObject.CreateReader(), target);
            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            throw new NotImplementedException();
        }
    }
}
