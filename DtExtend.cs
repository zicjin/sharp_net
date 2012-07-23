using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zic_dotnet {
    public static class DtExpend {

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
}
