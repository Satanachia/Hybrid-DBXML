namespace XMLDB3
{
    using System;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    public class ConfigManager
    {
        private static configration config = null;
        private const string configFile = "config.xml";
        private const int defBankCacheSize = 0x5dc;
        private const int defCharacterCacheSize = 0x7d0;
        private const int defMainPort = 0x32c9;
        private const int defMaxChronicleFirst = 5;
        private const int defMaxChronicleLatest = 5;
        private const int defMonitorPort = 0x3c29;
        private const int defProfilerPort = 0x3c28;

        public static string GetConnectionString(string _Index)
        {
            if (((config != null) && (config.sql != null)) && (config.sql.connections != null))
            {
                switch (_Index)
                {
                    case "account":
                        if (config.sql.connections.account == null)
                        {
                            return null;
                        }
                        return MakeConnectionString(config.sql.connections.account.server, config.sql.connections.account.database, config.sql.connections.account.user, config.sql.connections.account.password);

                    case "accountref":
                        if (config.sql.connections.accountref == null)
                        {
                            return null;
                        }
                        return MakeConnectionString(config.sql.connections.accountref.server, config.sql.connections.accountref.database, config.sql.connections.accountref.user, config.sql.connections.accountref.password);

                    case "character":
                        if (config.sql.connections.character == null)
                        {
                            return null;
                        }
                        return MakeConnectionString(config.sql.connections.character.server, config.sql.connections.character.database, config.sql.connections.character.user, config.sql.connections.character.password);

                    case "bank":
                        if (config.sql.connections.bank == null)
                        {
                            return null;
                        }
                        return MakeConnectionString(config.sql.connections.bank.server, config.sql.connections.bank.database, config.sql.connections.bank.user, config.sql.connections.bank.password);

                    case "prop":
                        if (config.sql.connections.prop == null)
                        {
                            return null;
                        }
                        return MakeConnectionString(config.sql.connections.prop.server, config.sql.connections.prop.database, config.sql.connections.prop.user, config.sql.connections.prop.password);

                    case "guild":
                        if (config.sql.connections.guild == null)
                        {
                            return null;
                        }
                        return MakeConnectionString(config.sql.connections.guild.server, config.sql.connections.guild.database, config.sql.connections.guild.user, config.sql.connections.guild.password);

                    case "websynch":
                        if (config.sql.connections.websynch == null)
                        {
                            return null;
                        }
                        return MakeConnectionString(config.sql.connections.websynch.server, config.sql.connections.websynch.database, config.sql.connections.websynch.user, config.sql.connections.websynch.password);

                    case "itemidpool":
                        if (config.sql.connections.itemidpool == null)
                        {
                            return null;
                        }
                        return MakeConnectionString(config.sql.connections.itemidpool.server, config.sql.connections.itemidpool.database, config.sql.connections.itemidpool.user, config.sql.connections.itemidpool.password);

                    case "charidpool":
                        if (config.sql.connections.charidpool == null)
                        {
                            return null;
                        }
                        return MakeConnectionString(config.sql.connections.charidpool.server, config.sql.connections.charidpool.database, config.sql.connections.charidpool.user, config.sql.connections.charidpool.password);

                    case "propidpool":
                        if (config.sql.connections.propidpool == null)
                        {
                            return null;
                        }
                        return MakeConnectionString(config.sql.connections.propidpool.server, config.sql.connections.propidpool.database, config.sql.connections.propidpool.user, config.sql.connections.propidpool.password);

                    case "guildidpool":
                        if (config.sql.connections.guildidpool == null)
                        {
                            return null;
                        }
                        return MakeConnectionString(config.sql.connections.guildidpool.server, config.sql.connections.guildidpool.database, config.sql.connections.guildidpool.user, config.sql.connections.guildidpool.password);

                    case "bididpool":
                        if (config.sql.connections.bididpool == null)
                        {
                            return null;
                        }
                        return MakeConnectionString(config.sql.connections.bididpool.server, config.sql.connections.bididpool.database, config.sql.connections.bididpool.user, config.sql.connections.bididpool.password);

                    case "castle":
                        if (config.sql.connections.house == null)
                        {
                            return null;
                        }
                        return MakeConnectionString(config.sql.connections.castle.server, config.sql.connections.castle.database, config.sql.connections.castle.user, config.sql.connections.castle.password);

                    case "house":
                        if (config.sql.connections.house == null)
                        {
                            return null;
                        }
                        return MakeConnectionString(config.sql.connections.house.server, config.sql.connections.house.database, config.sql.connections.house.user, config.sql.connections.house.password);

                    case "memo":
                        if (config.sql.connections.memo == null)
                        {
                            return null;
                        }
                        return MakeConnectionString(config.sql.connections.memo.server, config.sql.connections.memo.database, config.sql.connections.memo.user, config.sql.connections.memo.password);

                    case "chronicle":
                        if (config.sql.connections.chronicle == null)
                        {
                            return null;
                        }
                        return MakeConnectionString(config.sql.connections.chronicle.server, config.sql.connections.chronicle.database, config.sql.connections.chronicle.user, config.sql.connections.chronicle.password);

                    case "ruin":
                        if (config.sql.connections.ruin == null)
                        {
                            return null;
                        }
                        return MakeConnectionString(config.sql.connections.ruin.server, config.sql.connections.ruin.database, config.sql.connections.ruin.user, config.sql.connections.ruin.password);

                    case "shopadvertise":
                        if (config.sql.connections.shopadvertise == null)
                        {
                            return null;
                        }
                        return MakeConnectionString(config.sql.connections.shopadvertise.server, config.sql.connections.shopadvertise.database, config.sql.connections.shopadvertise.user, config.sql.connections.shopadvertise.password);

                    case "houseguestbook":
                        if (config.sql.connections.houseguestbook == null)
                        {
                            return null;
                        }
                        return MakeConnectionString(config.sql.connections.houseguestbook.server, config.sql.connections.houseguestbook.database, config.sql.connections.houseguestbook.user, config.sql.connections.houseguestbook.password);

                    case "dungeonrank":
                        if (config.sql.connections.dungeonrank == null)
                        {
                            return null;
                        }
                        return MakeConnectionString(config.sql.connections.dungeonrank.server, config.sql.connections.dungeonrank.database, config.sql.connections.dungeonrank.user, config.sql.connections.dungeonrank.password);

                    case "channelingkeypool":
                        if (config.sql.connections.channelingkeypool == null)
                        {
                            return null;
                        }
                        return MakeConnectionString(config.sql.connections.channelingkeypool.server, config.sql.connections.channelingkeypool.database, config.sql.connections.channelingkeypool.user, config.sql.connections.channelingkeypool.password);

                    case "promotionrank":
                        if (config.sql.connections.promotionrank == null)
                        {
                            return null;
                        }
                        return MakeConnectionString(config.sql.connections.promotionrank.server, config.sql.connections.promotionrank.database, config.sql.connections.promotionrank.user, config.sql.connections.promotionrank.password);

                    case "mailbox":
                        if (config.sql.connections.mailbox == null)
                        {
                            return null;
                        }
                        return MakeConnectionString(config.sql.connections.mailbox.server, config.sql.connections.mailbox.database, config.sql.connections.mailbox.user, config.sql.connections.mailbox.password);

                    case "farm":
                        if (config.sql.connections.farm == null)
                        {
                            return null;
                        }
                        return MakeConnectionString(config.sql.connections.farm.server, config.sql.connections.farm.database, config.sql.connections.farm.user, config.sql.connections.farm.password);

                    case "bid":
                        if (config.sql.connections.bid == null)
                        {
                            return null;
                        }
                        return MakeConnectionString(config.sql.connections.bid.server, config.sql.connections.bid.database, config.sql.connections.bid.user, config.sql.connections.bid.password);

                    case "event":
                        if (config.sql.connections.@event == null)
                        {
                            return null;
                        }
                        return MakeConnectionString(config.sql.connections.@event.server, config.sql.connections.@event.database, config.sql.connections.@event.user, config.sql.connections.@event.password);

                    case "worldmeta":
                        if (config.sql.connections.worldmeta == null)
                        {
                            return null;
                        }
                        return MakeConnectionString(config.sql.connections.worldmeta.server, config.sql.connections.worldmeta.database, config.sql.connections.worldmeta.user, config.sql.connections.worldmeta.password);

                    case "wine":
                        if (config.sql.connections.wine == null)
                        {
                            return null;
                        }
                        return MakeConnectionString(config.sql.connections.wine.server, config.sql.connections.wine.database, config.sql.connections.wine.user, config.sql.connections.wine.password);

                    case "countryreport":
                        if (config.sql.connections.countryreport == null)
                        {
                            return null;
                        }
                        return MakeConnectionString(config.sql.connections.countryreport.server, config.sql.connections.countryreport.database, config.sql.connections.countryreport.user, config.sql.connections.countryreport.password);

                    case "loginoutreport":
                        if (config.sql.connections.loginoutreport == null)
                        {
                            return null;
                        }
                        return MakeConnectionString(config.sql.connections.loginoutreport.server, config.sql.connections.loginoutreport.database, config.sql.connections.loginoutreport.user, config.sql.connections.loginoutreport.password);

                    case "husky":
                        if (config.sql.connections.husky == null)
                        {
                            return null;
                        }
                        return MakeConnectionString(config.sql.connections.husky.server, config.sql.connections.husky.database, config.sql.connections.husky.user, config.sql.connections.husky.password);

                    case "loginidpool":
                        if (config.sql.connections.loginidpool == null)
                        {
                            return null;
                        }
                        return MakeConnectionString(config.sql.connections.loginidpool.server, config.sql.connections.loginidpool.database, config.sql.connections.loginidpool.user, config.sql.connections.loginidpool.password);
                }
            }
            return null;
        }

        public static string GetFileDBPath(string _Index)
        {
            if (((config == null) || (config.file == null)) || (config.file.paths == null))
            {
                if (_Index.ToLower().IndexOf("idpool") >= 0)
                {
                    return "idpool/";
                }
                return (_Index + "/");
            }
            index_info[] _infoArray = new index_info[] { 
                new index_info("account", ref config.file.paths.account), new index_info("accountref", ref config.file.paths.accountref), new index_info("character", ref config.file.paths.character), new index_info("pet", ref config.file.paths.pet), new index_info("bank", ref config.file.paths.bank), new index_info("prop", ref config.file.paths.prop), new index_info("guild", ref config.file.paths.guild), new index_info("websynch", ref config.file.paths.websynch), new index_info("itemidpool", ref config.file.paths.itemidpool), new index_info("charidpool", ref config.file.paths.charidpool), new index_info("propidpool", ref config.file.paths.propidpool), new index_info("guildidpool", ref config.file.paths.guildidpool), new index_info("bididpool", ref config.file.paths.bididpool), new index_info("castle", ref config.file.paths.castle), new index_info("house", ref config.file.paths.house), new index_info("memo", ref config.file.paths.memo), 
                new index_info("chronicle", ref config.file.paths.chronicle), new index_info("ruin", ref config.file.paths.ruin), new index_info("shopadvertise", ref config.file.paths.shopadvertise), new index_info("houseguestbook", ref config.file.paths.houseguestbook), new index_info("dungeonrank", ref config.file.paths.dungeonrank), new index_info("channelingkey", ref config.file.paths.channelingkeypool), new index_info("promotionrank", ref config.file.paths.promotionrank), new index_info("mailbox", ref config.file.paths.mailbox), new index_info("farm", ref config.file.paths.farm), new index_info("bid", ref config.file.paths.bid), new index_info("event", ref config.file.paths.@event), new index_info("worldmeta", ref config.file.paths.worldmeta), new index_info("wine", ref config.file.paths.wine), new index_info("countryreport", ref config.file.paths.countryreport), new index_info("loginoutreport", ref config.file.paths.loginoutreport), new index_info("husky", ref config.file.paths.husky), 
                new index_info("royalalchemist", ref config.file.paths.royalalchemist), new index_info("family", ref config.file.paths.family), new index_info("loginidpool", ref config.file.paths.loginidpool)
             };
            for (int i = 0; i < _infoArray.Length; i++)
            {
                if (_Index.ToLower() == _infoArray[i].index.ToLower())
                {
                    if (((_infoArray[i].path != null) && (_infoArray[i].path.path != null)) && (_infoArray[i].path.path.Length > 0))
                    {
                        return _infoArray[i].path.path;
                    }
                    if (_Index.ToLower().IndexOf("idpool") >= 0)
                    {
                        return "idpool/";
                    }
                    return (_infoArray[i].index + "/");
                }
            }
            return null;
        }

        public static void Load()
        {
            try
            {
                if (File.Exists("config.xml"))
                {
                    StreamReader input = new StreamReader("config.xml");
                    XmlTextReader xmlReader = new XmlTextReader(input);
                    try
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(configration));
                        if (serializer.CanDeserialize(xmlReader))
                        {
                            config = (configration) serializer.Deserialize(xmlReader);
                        }
                        else
                        {
                            ExceptionMonitor.ExceptionRaised(new Exception("컨피그 파일의 형식이 잘못되었습니다. 초기화 할 수 없습니다"));
                            Process.GetCurrentProcess().Close();
                        }
                    }
                    finally
                    {
                        input.Close();
                        xmlReader.Close();
                    }
                }
                else
                {
                    StreamWriter writer = new StreamWriter("config.xml", false, Encoding.Unicode);
                    try
                    {
                        config = new configration();
                        config.sql = null;
                        new XmlSerializer(typeof(configration)).Serialize((TextWriter) writer, config);
                    }
                    finally
                    {
                        writer.Close();
                    }
                }
                if (config.chronicleRank == null)
                {
                    config.chronicleRank = new configrationChronicleRank();
                    config.chronicleRank.maxFirstRank = 5;
                    config.chronicleRank.maxLatestRank = 5;
                }
                if (config.cache == null)
                {
                    config.cache = new configrationCache();
                    config.cache.characterSize = 0x7d0;
                    config.cache.bankSize = 0x5dc;
                }
                if (config.feature == null)
                {
                    config.feature = new configrationFeature();
                }
            }
            catch (Exception exception)
            {
                ExceptionMonitor.ExceptionRaised(exception);
                Process.GetCurrentProcess().Close();
            }
        }

        private static string MakeConnectionString(string _server, string _database, string _user_id, string _pwd)
        {
            if ((_user_id == null) || (_user_id == string.Empty))
            {
                return string.Format("Persist Security Info=True;Integrated Security=true;Initial Catalog={0};server={1};", _database, _server);
            }
            return string.Format("server={0}; database={1}; user id={2}; pwd={3};", new object[] { _server, _database, _user_id, _pwd });
        }

        public static bool TestSqlConnection()
        {
            string[] strArray = new string[] { "account", "accountref", "character", "prop", "guild", "websynch", "itemidpool", "charidpool", "propidpool", "guildidpool", "castle", "house" };
            foreach (string str in strArray)
            {
                string connectionString = GetConnectionString(str);
                if (connectionString == null)
                {
                    throw new Exception(str + " 설정이 없습니다.");
                }
                SqlConnection connection = new SqlConnection(connectionString);
                try
                {
                    connection.Open();
                }
                finally
                {
                    connection.Close();
                }
            }
            return true;
        }

        public static int BankCacheSize
        {
            get
            {
                return config.cache.bankSize;
            }
        }

        public static uint CacheKeyTimeOut
        {
            get
            {
                if ((config != null) && (config.encryption != null))
                {
                    return config.encryption.cacheKeyTimeout;
                }
                return 0;
            }
        }

        public static int CharacterCacheSize
        {
            get
            {
                return config.cache.characterSize;
            }
        }

        public static EncryptionColumn[] EncryptionColumn
        {
            get
            {
                if (((config != null) && (config.encryption != null)) && ((config.encryption.columns != null) && (config.encryption.columns.Length > 0)))
                {
                    return (EncryptionColumn[]) config.encryption.columns.Clone();
                }
                return null;
            }
        }

        public static string EncryptionDll
        {
            get
            {
                if ((config != null) && (config.encryption != null))
                {
                    return config.encryption.dll;
                }
                return string.Empty;
            }
        }

        public static string EncryptionUser
        {
            get
            {
                if ((config != null) && (config.encryption != null))
                {
                    return config.encryption.userName;
                }
                return string.Empty;
            }
        }

        public static bool IsEncryptionEnabled
        {
            get
            {
                return (((config != null) && (config.encryption != null)) && ((config.encryption.columns != null) && (config.encryption.columns.Length > 0)));
            }
        }

        public static bool IsPVPable
        {
            get
            {
                return (((config != null) && (config.feature != null)) && config.feature.pvp);
            }
        }

        public static bool IsRedirectionEnabled
        {
            get
            {
                return (((config != null) && (config.redirection != null)) && config.redirection.enable);
            }
        }

        public static bool IsTestMode
        {
            get
            {
                return (config.sql == null);
            }
        }

        public static int ItemMarketCodePage
        {
            get
            {
                if ((config != null) && (config.itemMarket != null))
                {
                    return config.itemMarket.codePage;
                }
                return 0;
            }
        }

        public static int ItemMarketConnectionPoolNo
        {
            get
            {
                if ((config != null) && (config.itemMarket != null))
                {
                    return config.itemMarket.connectionPool;
                }
                return 0;
            }
        }

        public static bool ItemMarketEnabled
        {
            get
            {
                return ((config != null) && (config.itemMarket != null));
            }
        }

        public static int ItemMarketGameNo
        {
            get
            {
                if ((config != null) && (config.itemMarket != null))
                {
                    return config.itemMarket.gameNo;
                }
                return 0;
            }
        }

        public static string ItemMarketIP
        {
            get
            {
                if ((config != null) && (config.itemMarket != null))
                {
                    return config.itemMarket.ip;
                }
                return string.Empty;
            }
        }

        public static short ItemMarketPort
        {
            get
            {
                if ((config != null) && (config.itemMarket != null))
                {
                    return config.itemMarket.port;
                }
                return 0;
            }
        }

        public static int ItemMarketServerNo
        {
            get
            {
                if ((config != null) && (config.itemMarket != null))
                {
                    return (config.itemMarket.gameNo + config.itemMarket.serverNo);
                }
                return 0;
            }
        }

        public static int MainPort
        {
            get
            {
                if (((config != null) && (config.server != null)) && (config.server.main != null))
                {
                    return config.server.main.port;
                }
                return 0x32c9;
            }
        }

        public static int MaxChronicleFirst
        {
            get
            {
                return config.chronicleRank.maxFirstRank;
            }
        }

        public static int MaxChronicleLatest
        {
            get
            {
                return config.chronicleRank.maxLatestRank;
            }
        }

        public static int MonitorPort
        {
            get
            {
                if (((config != null) && (config.server != null)) && (config.server.monitor != null))
                {
                    return config.server.monitor.port;
                }
                return 0x3c29;
            }
        }

        public static int ProfilerPort
        {
            get
            {
                if (((config != null) && (config.server != null)) && (config.server.profiler != null))
                {
                    return config.server.profiler.port;
                }
                return 0x3c28;
            }
        }

        public static int RedirectionPort
        {
            get
            {
                if ((config != null) && (config.redirection != null))
                {
                    return config.redirection.port;
                }
                return 0;
            }
        }

        public static string RedirectionServer
        {
            get
            {
                if ((config != null) && (config.redirection != null))
                {
                    return config.redirection.server;
                }
                return string.Empty;
            }
        }

        public static string ReportReceiver
        {
            get
            {
                if ((config != null) && (config.report != null))
                {
                    return config.report.receiver;
                }
                return null;
            }
        }

        public static string ReportSender
        {
            get
            {
                if ((config != null) && (config.report != null))
                {
                    return config.report.sender;
                }
                return null;
            }
        }

        public static string ReportServer
        {
            get
            {
                if ((config != null) && (config.report != null))
                {
                    return config.report.server;
                }
                return null;
            }
        }

        public static string StatisticsConnection
        {
            get
            {
                if (((config != null) && (config.statistics != null)) && (config.statistics.database != null))
                {
                    return MakeConnectionString(config.statistics.database.server, config.statistics.database.database, config.statistics.database.user, config.statistics.database.password);
                }
                return null;
            }
        }

        public static int StatisticsPeriod
        {
            get
            {
                if ((config != null) && (config.statistics != null))
                {
                    return (config.statistics.period * 0x3e8);
                }
                return -1;
            }
        }

        private class index_info
        {
            public string index;
            public filedbpath path;

            public index_info(string index, ref filedbpath path)
            {
                this.index = index;
                this.path = path;
            }
        }
    }
}

