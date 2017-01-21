namespace XMLDB3
{
    using System;
    using System.Xml.Serialization;
    using XMLDB3.ItemMarket;

    public class XMLDB3Main
    {
        public static void BuildXmlSerializer()
        {
            Type[] typeArray = new Type[] { typeof(configration), typeof(Item), typeof(CharacterArbeit), typeof(ConditionContainer), typeof(KeywordContainer), typeof(MemoryContainer), typeof(CharacterPrivate), typeof(TitleObjectBuilder.TitleContainer), typeof(GuildIDList), typeof(ItemXmlFieldHelper.QuestObjectiveContainer), typeof(ItemXmlFieldHelper.ItemOptionContainer), typeof(PetConditionContainer), typeof(PetMemoryContainer), typeof(PetPrivate), typeof(PropIDList) };
            foreach (Type type in typeArray)
            {
                try
                {
                    new XmlSerializer(type);
                }
                catch
                {
                }
            }
        }

        public static void ClearException()
        {
            ExceptionMonitor.Clear();
        }

        public static void Shutdown()
        {
            MainProcedure.ServerStop();
            Profiler.ServerStop();
            MonitorProcedure.ServerStop();
            ProcessManager.Shutdown();
            CommandSerializer.Shutdown();
            if (ConfigManager.ItemMarketEnabled)
            {
                ItemMarketManager.Stop();
            }
            EncryptionManager.Close();
        }

        public static void Start()
        {
            BuildXmlSerializer();
            ConfigManager.Load();
            if (EncryptionManager.Init(ConfigManager.EncryptionColumn))
            {
                ObjectCache.Init();
                XMLDB3.QueryManager.Initialize();
                CommandSerializer.Initialize();
                ProcessManager.Start();
                if (ConfigManager.IsRedirectionEnabled)
                {
                    CommandRedirection.Init(ConfigManager.RedirectionServer, ConfigManager.RedirectionPort);
                }
                if (ConfigManager.ItemMarketEnabled)
                {
                    ItemMarketManager.Init(ConfigManager.ItemMarketGameNo, ConfigManager.ItemMarketServerNo, ConfigManager.ItemMarketIP, ConfigManager.ItemMarketPort, ConfigManager.ItemMarketConnectionPoolNo, ConfigManager.ItemMarketCodePage);
                }
                MonitorProcedure.ServerStart(ConfigManager.MonitorPort);
                Profiler.ServerStart(ConfigManager.ProfilerPort);
                MainProcedure.ServerStart(ConfigManager.MainPort);
            }
        }
    }
}

