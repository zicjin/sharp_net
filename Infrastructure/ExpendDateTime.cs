using System;
using System.Linq;
using System.Collections.Generic;
using sharp_net.Mvc;

namespace sharp_net {

    public static class ExpendDateTime {

        public static DateTime OffsetCN(this DateTime input) {
            return input.AddHours(8);
        }

    }
}