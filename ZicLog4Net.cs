using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Filter;
using log4net.Layout;
using log4net.Repository;

namespace zic_dotnet {

    public sealed class ZicLog4Net {
        private static PatternLayout ZicLayout = new PatternLayout("时间：%date 线程ID：[%thread] 级别：%-5level 触发源：%logger property:[%property{NDC}] - " + Environment.NewLine + "Message：%message%newline");
        private static IDictionary<string, LevelRangeFilter> Filters = new Dictionary<string, LevelRangeFilter>();
        private static LevelRangeFilter FatalErrorFilter = new LevelRangeFilter();
        private static LevelRangeFilter FatalFilter = new LevelRangeFilter();
        private static LevelRangeFilter ErrorFilter = new LevelRangeFilter();
        private static LevelRangeFilter WarnFilter = new LevelRangeFilter();
        private static LevelRangeFilter InfoFilter = new LevelRangeFilter();
        private static LevelRangeFilter DebugFilter = new LevelRangeFilter();

        private ZicLog4Net() {
            ZicLayout.Header = "New Log ------------------------------" + Environment.NewLine;
            ZicLayout.Footer = "By ZIC" + Environment.NewLine;

            FatalFilter.LevelMax = log4net.Core.Level.Fatal;
            FatalFilter.LevelMin = log4net.Core.Level.Fatal;
            FatalFilter.ActivateOptions();
            Filters.Add("FATAL", FatalFilter);

            ErrorFilter.LevelMax = log4net.Core.Level.Error;
            ErrorFilter.LevelMin = log4net.Core.Level.Error;
            ErrorFilter.ActivateOptions();
            Filters.Add("ERROR", ErrorFilter);

            WarnFilter.LevelMax = log4net.Core.Level.Warn;
            WarnFilter.LevelMin = log4net.Core.Level.Warn;
            WarnFilter.ActivateOptions();
            Filters.Add("WARN", FatalFilter);

            InfoFilter.LevelMax = log4net.Core.Level.Info;
            InfoFilter.LevelMin = log4net.Core.Level.Info;
            InfoFilter.ActivateOptions();
            Filters.Add("INFO", FatalFilter);

            DebugFilter.LevelMax = log4net.Core.Level.Debug;
            DebugFilter.LevelMin = log4net.Core.Level.Debug;
            DebugFilter.ActivateOptions();
            Filters.Add("DEBUG", FatalFilter);

            FatalErrorFilter.LevelMax = Level.Fatal;
            FatalErrorFilter.LevelMin = Level.Error;
            FatalErrorFilter.ActivateOptions();
        }

        private static ZicLog4Net instance;

        public static ZicLog4Net Instance {
            get { return instance ?? (instance = new ZicLog4Net()); }
        }

        public void Config(string[] domains) {
            foreach (string domain in domains) {
                ILoggerRepository repository = LogManager.CreateRepository(domain);

                //FileLog
                foreach (KeyValuePair<string, LevelRangeFilter> Filter in Filters) {
                    RollingFileAppender ZicFileAppender = new RollingFileAppender();
                    ZicFileAppender.Name = domain + "_" + Filter.Key + "_FileAppender";
                    ZicFileAppender.File = "Log_" + domain + "\\" + Filter.Key + "\\";
                    ZicFileAppender.AppendToFile = true;
                    ZicFileAppender.RollingStyle = RollingFileAppender.RollingMode.Date;
                    ZicFileAppender.DatePattern = "yyyy-MM-dd'.log'";
                    ZicFileAppender.StaticLogFileName = false;
                    ZicFileAppender.Layout = ZicLayout;
                    ZicFileAppender.AddFilter(Filter.Value);
                    ZicFileAppender.ActivateOptions();
                    BasicConfigurator.Configure(repository, ZicFileAppender);
                }

                //SmtpLog
                SmtpAppender ZicSmtpAppender = new SmtpAppender();
                ZicSmtpAppender.Name = domain + "_SmtpAppender";
                ZicSmtpAppender.Authentication = SmtpAppender.SmtpAuthentication.Basic;
                ZicSmtpAppender.To = "zicjin@gmail.com";
                ZicSmtpAppender.From = "zicjin@gmail.com";
                ZicSmtpAppender.Username = "zicjin";
                ZicSmtpAppender.Password = "rWcsarpi2#gg";
                ZicSmtpAppender.Subject = domain + "_logging message";
                ZicSmtpAppender.SmtpHost = "smtp.gmail.com";
                ZicSmtpAppender.BufferSize = 512;
                ZicSmtpAppender.Lossy = true;
                ZicSmtpAppender.Layout = ZicLayout;
                ZicSmtpAppender.AddFilter(FatalErrorFilter);
                ZicSmtpAppender.ActivateOptions();
                BasicConfigurator.Configure(repository, ZicSmtpAppender);

                //TraceLog
                TraceAppender ZicTranceAppender = new TraceAppender();
                ZicTranceAppender.Name = domain + "_TranceAppender";
                ZicTranceAppender.Layout = ZicLayout;
                ZicTranceAppender.AddFilter(FatalFilter);
                ZicTranceAppender.ActivateOptions();
                BasicConfigurator.Configure(repository, ZicTranceAppender);
            }
        }
    }
}