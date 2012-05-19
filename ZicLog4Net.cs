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
        private static PatternLayout ZicLayout = new PatternLayout(Environment.NewLine + "时间：%date 线程ID：[%thread] 级别：%-5level 触发源：%logger property:[%property{NDC}] - " + Environment.NewLine + "Message：%message%newline");
        private static IDictionary<string, LevelRangeFilter> Filters = new Dictionary<string, LevelRangeFilter>();
        private static LevelRangeFilter FatalErrorFilter = new LevelRangeFilter();
        private static LevelRangeFilter FatalFilter = new LevelRangeFilter();
        private static LevelRangeFilter ErrorFilter = new LevelRangeFilter();
        private static LevelRangeFilter WarnFilter = new LevelRangeFilter();
        private static LevelRangeFilter InfoFilter = new LevelRangeFilter();
        private static LevelRangeFilter DebugFilter = new LevelRangeFilter();

        private ZicLog4Net() {
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
                    RollingFileAppender fileAppender = new RollingFileAppender();
                    fileAppender.Name = domain + "_" + Filter.Key + "_FileAppender";
                    fileAppender.File = "Log_" + domain + "\\" + Filter.Key + "\\";
                    fileAppender.AppendToFile = true;
                    fileAppender.RollingStyle = RollingFileAppender.RollingMode.Date;
                    fileAppender.DatePattern = "yyyy-MM-dd'.log'";
                    fileAppender.StaticLogFileName = false;
                    fileAppender.Layout = ZicLayout;
                    fileAppender.AddFilter(Filter.Value);
                    fileAppender.ActivateOptions();
                    BasicConfigurator.Configure(repository, fileAppender);
                }

                //SmtpLog
                ZicSmtpAppender smtpAppender = new ZicSmtpAppender();
                smtpAppender.Name = domain + "_SmtpAppender";
                smtpAppender.Authentication = ZicSmtpAppender.SmtpAuthentication.Basic;
                smtpAppender.To = "zicjin@gmail.com";
                smtpAppender.From = "zicjin@gmail.com";
                smtpAppender.Username = "zicjin";
                smtpAppender.Password = "rWcsarpi2#gg";
                smtpAppender.EnableSSL = true;
                smtpAppender.Subject = domain + "_logging message";
                smtpAppender.SmtpHost = "smtp.gmail.com";
                smtpAppender.Port = 25;
                smtpAppender.BufferSize = 1;
                smtpAppender.Lossy = true;
                smtpAppender.Layout = ZicLayout;
                //smtpAppender.AddFilter(FatalErrorFilter);
                smtpAppender.ActivateOptions();
                BasicConfigurator.Configure(repository, smtpAppender);

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