namespace XMLDB3
{
    using System;
    using System.Collections;

    public class ObjectCache
    {
        private static BasicCache bankCache = new BasicCache(ConfigManager.BankCacheSize, "BankCache");
        private static BasicCache characterCache = new BasicCache(ConfigManager.CharacterCacheSize, "CharacterCache");
        private static ChronicleCache chronicleCache = null;

        private ObjectCache()
        {
        }

        public static void DeleteChronicleCache()
        {
            chronicleCache = null;
        }

        public static void Init()
        {
        }

        public static void InitChronicleCache(string _serverName)
        {
            chronicleCache = new ChronicleCache(_serverName);
        }

        public static void InitChronicleCache(string _serverName, IDictionary _dic)
        {
            chronicleCache = new ChronicleCache(_serverName, _dic);
        }

        public static BasicCache Bank
        {
            get
            {
                return bankCache;
            }
        }

        public static BasicCache Character
        {
            get
            {
                return characterCache;
            }
        }

        public static ChronicleCache Chronicle
        {
            get
            {
                if (chronicleCache == null)
                {
                    throw new Exception("Chronicle cache 초기화 안됨");
                }
                return chronicleCache;
            }
        }
    }
}

