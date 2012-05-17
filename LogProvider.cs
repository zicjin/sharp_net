using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace zic_dotnet {

    public static class LogProvider {
        static ILog log;

        public static ILog Log {
            get { return log ?? (log = log4net.LogManager.GetLogger("zicjin")); }
        }
    }
}