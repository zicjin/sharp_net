using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace zic_dotnet {

    public static class EFLogProvider {
        static ILog log;

        public static ILog Log {
            get { return log ?? (log = log4net.LogManager.GetLogger("zicjin")); }
        }
    }
}