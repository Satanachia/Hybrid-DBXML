namespace XMLDB3
{
    using System;

    public class QueryManager
    {
        private AccountActivationAdapter m_AccountActivationAdapter;
        private AccountAdapter m_AccountAdapter;
        private AccountRefAdapter m_AccountRefAdapter;
        private BankAdapter m_BankAdapter;
        private BidAdapter m_BidAdapter;
        private BidIdPoolAdapter m_BidIdPool;
        private CastleAdapter m_CastleAdapter;
        private ChannelingKeyPoolAdapter m_ChannelingKeyPool;
        private CharacterAdapter m_CharacterAdapter;
        private CharIdPoolAdapter m_CharacterIdPool;
        private ChronicleAdapter m_ChronicleAdapter;
        private CountryReportAdapter m_CountryReportAdapter;
        private DungeonRankAdapter m_DungeonRankAdapter;
        private EventAdapter m_EventAdapter;
        private FamilyAdapter m_FamilyAdapter;
        private FarmAdapter m_FarmAdapter;
        private GuildAdapter m_GuildAdapter;
        private GuildIdPoolAdapter m_GuildIdPool;
        private HouseAdapter m_HouseAdapter;
        private HuskyAdapter m_HuskyAdapter;
        private ItemIdPoolAdapter m_ItemIdPool;
        private LoginIdPoolAdapter m_LoginIdPool;
        private LogInOutReportAdapter m_LogInOutReportAdapter;
        private MailBoxAdapter m_MailBoxAdapter;
        private MemoAdapter m_MemoAdapter;
        private PetAdapter m_PetAdapter;
        private PromotionAdapter m_PromotionAdapter;
        private PropAdapter m_PropAdapter;
        private PropIdPoolAdapter m_PropIdPool;
        private static QueryManager m_QueryManager = new QueryManager();
        private RoyalAlchemistAdapter m_RoyalAlchemistAdapter;
        private RuinAdapter m_RuinAdapter;
        private ShopAdvertiseAdapter m_ShopAdvertiseAdapter;
        private WebSynchAdapter m_WebSynchAdapter;
        private WineAdapter m_WineAdapter;
        private WorldMetaAdapter m_WorldMetaAdapter;

        public static void Initialize()
        {
            if (ConfigManager.IsTestMode)
            {
                m_QueryManager.m_AccountAdapter = new AccountFileAdapter();
                m_QueryManager.m_AccountActivationAdapter = new AccountActivationFileAdapter();
                m_QueryManager.m_AccountRefAdapter = new AccountrefFileAdapter();
                m_QueryManager.m_CharacterAdapter = new CharacterFileAdapter();
                m_QueryManager.m_PetAdapter = new PetFileAdapter();
                m_QueryManager.m_BankAdapter = new BankFileAdapter();
                m_QueryManager.m_MemoAdapter = new MemoFileAdapter();
                m_QueryManager.m_CastleAdapter = new CastleFileAdapter();
                m_QueryManager.m_HouseAdapter = new HouseFileAdapter();
                m_QueryManager.m_GuildAdapter = new GuildFileAdapter();
                m_QueryManager.m_PropAdapter = new PropFileAdapter();
                m_QueryManager.m_CharacterIdPool = new CharIdPoolFileAdapter();
                m_QueryManager.m_ItemIdPool = new ItemIdPoolFileAdapter();
                m_QueryManager.m_PropIdPool = new PropIdPoolFileAdapter();
                m_QueryManager.m_GuildIdPool = new GuildIdPoolFileAdapter();
                m_QueryManager.m_BidIdPool = new BidIdPoolFileAdapter();
                m_QueryManager.m_LoginIdPool = new LoginIdPoolFileAdapter();
                m_QueryManager.m_ChannelingKeyPool = new ChannelingKeyPoolFileAdapter();
                m_QueryManager.m_ChronicleAdapter = new ChronicleFileAdapter();
                m_QueryManager.m_RuinAdapter = new RuinFileAdapter();
                m_QueryManager.m_WebSynchAdapter = null;
                m_QueryManager.m_ShopAdvertiseAdapter = new ShopAdvertiseFileAdapter();
                m_QueryManager.m_DungeonRankAdapter = new DungeonRankFileAdapter();
                m_QueryManager.m_PromotionAdapter = new PromotionFileAdapter();
                m_QueryManager.m_MailBoxAdapter = new MailBoxFileAdapter();
                m_QueryManager.m_FarmAdapter = new FarmFileAdapter();
                m_QueryManager.m_BidAdapter = new BidFileAdapter();
                m_QueryManager.m_EventAdapter = new EventFileAdapter();
                m_QueryManager.m_WorldMetaAdapter = new WorldMetaFileAdapter();
                m_QueryManager.m_WineAdapter = new WineFileAdapter();
                m_QueryManager.m_RoyalAlchemistAdapter = new RoyalAlchemistFileAdapter();
                m_QueryManager.m_FamilyAdapter = new FamilyFileAdapter();
                m_QueryManager.m_CountryReportAdapter = new CountryReportFileAdapter();
                m_QueryManager.m_LogInOutReportAdapter = new LogInOutReportFileAdapter();
                m_QueryManager.m_HuskyAdapter = new HuskyFileAdapter();
                m_QueryManager.m_AccountAdapter.Initialize("");
                m_QueryManager.m_AccountActivationAdapter.Initialize("");
                m_QueryManager.m_AccountRefAdapter.Initialize("");
                m_QueryManager.m_CharacterAdapter.Initialize("");
                m_QueryManager.m_PetAdapter.Initialize("");
                m_QueryManager.m_BankAdapter.Initialize("");
                m_QueryManager.m_MemoAdapter.Initialize("");
                m_QueryManager.m_CastleAdapter.Initialize("");
                m_QueryManager.m_HouseAdapter.Initialize("");
                m_QueryManager.m_GuildAdapter.Initialize("");
                m_QueryManager.m_PropAdapter.Initialize("");
                m_QueryManager.m_ChannelingKeyPool.Initialize("");
                m_QueryManager.m_CharacterIdPool.Initialize("");
                m_QueryManager.m_ItemIdPool.Initialize("");
                m_QueryManager.m_PropIdPool.Initialize("");
                m_QueryManager.m_GuildIdPool.Initialize("");
                m_QueryManager.m_BidIdPool.Initialize("");
                m_QueryManager.m_LoginIdPool.Initialize("");
                m_QueryManager.m_ChronicleAdapter.Initialize("");
                m_QueryManager.m_RuinAdapter.Initialize("");
                m_QueryManager.m_ShopAdvertiseAdapter.Initialize("");
                m_QueryManager.m_DungeonRankAdapter.Initialize("");
                m_QueryManager.m_PromotionAdapter.Initialize("");
                m_QueryManager.m_MailBoxAdapter.Initialize("");
                m_QueryManager.m_FarmAdapter.Initialize("");
                m_QueryManager.m_BidAdapter.Initialize("");
                m_QueryManager.m_EventAdapter.Initialize("");
                m_QueryManager.m_WorldMetaAdapter.Initialize("");
                m_QueryManager.m_WineAdapter.Initialize("");
                m_QueryManager.m_RoyalAlchemistAdapter.Initialize("");
                m_QueryManager.m_FamilyAdapter.Initialize("");
                m_QueryManager.m_CountryReportAdapter.Initialize("");
                m_QueryManager.m_LogInOutReportAdapter.Initialize("");
                m_QueryManager.m_HuskyAdapter.Initialize("");
            }
            else
            {
                string connectionString = ConfigManager.GetConnectionString("account");
                if (connectionString != string.Empty)
                {
                    m_QueryManager.m_AccountAdapter = new AccountSqlAdapter();
                    m_QueryManager.m_AccountAdapter.Initialize(connectionString);
                    m_QueryManager.m_AccountActivationAdapter = new AccountActivationSqlAdapter();
                    m_QueryManager.m_AccountActivationAdapter.Initialize(connectionString);
                }
                else
                {
                    m_QueryManager.m_AccountAdapter = null;
                    m_QueryManager.m_AccountActivationAdapter = null;
                }
                string str2 = ConfigManager.GetConnectionString("accountref");
                if (str2 != string.Empty)
                {
                    m_QueryManager.m_AccountRefAdapter = new AccountrefSqlAdapter();
                    m_QueryManager.m_AccountRefAdapter.Initialize(str2);
                }
                else
                {
                    m_QueryManager.m_AccountRefAdapter = null;
                }
                string str3 = ConfigManager.GetConnectionString("character");
                if (str3 != string.Empty)
                {
                    m_QueryManager.m_CharacterAdapter = new CharacterSqlAdapter();
                    m_QueryManager.m_CharacterAdapter.Initialize(str3);
                    m_QueryManager.m_PetAdapter = new PetSqlAdapter();
                    m_QueryManager.m_PetAdapter.Initialize(str3);
                }
                else
                {
                    m_QueryManager.m_CharacterAdapter = null;
                    m_QueryManager.m_PetAdapter = null;
                }
                string str4 = ConfigManager.GetConnectionString("bank");
                if (str4 != string.Empty)
                {
                    m_QueryManager.m_BankAdapter = new BankSqlAdapter();
                    m_QueryManager.m_BankAdapter.Initialize(str4);
                }
                else
                {
                    m_QueryManager.m_BankAdapter = null;
                }
                string str5 = ConfigManager.GetConnectionString("guild");
                if (str5 != string.Empty)
                {
                    m_QueryManager.m_GuildAdapter = new GuildSqlAdapter();
                    m_QueryManager.m_GuildAdapter.Initialize(str5);
                }
                else
                {
                    m_QueryManager.m_GuildAdapter = null;
                }
                string str6 = ConfigManager.GetConnectionString("castle");
                if (str6 != string.Empty)
                {
                    m_QueryManager.m_CastleAdapter = new CastleSqlAdapter();
                    m_QueryManager.m_CastleAdapter.Initialize(str6);
                }
                else
                {
                    m_QueryManager.m_GuildAdapter = null;
                }
                string str7 = ConfigManager.GetConnectionString("house");
                if (str7 != string.Empty)
                {
                    string str8 = ConfigManager.GetConnectionString("houseguestbook");
                    if ((str8 == null) && (Console.Out != null))
                    {
                        Console.WriteLine("House GuestBook's DISABLED");
                    }
                    m_QueryManager.m_HouseAdapter = new HouseSqlAdapter(str8);
                    m_QueryManager.m_HouseAdapter.Initialize(str7);
                }
                else
                {
                    m_QueryManager.m_HouseAdapter = null;
                }
                string str9 = ConfigManager.GetConnectionString("prop");
                if (str9 != string.Empty)
                {
                    m_QueryManager.m_PropAdapter = new PropSqlAdapter();
                    m_QueryManager.m_PropAdapter.Initialize(str9);
                }
                else
                {
                    m_QueryManager.m_PropAdapter = null;
                }
                string str10 = ConfigManager.GetConnectionString("websynch");
                if (str10 != string.Empty)
                {
                    m_QueryManager.m_WebSynchAdapter = new WebSynchSqlAdapter();
                    m_QueryManager.m_WebSynchAdapter.Initialize(str10);
                }
                else
                {
                    m_QueryManager.m_WebSynchAdapter = null;
                }
                string str11 = ConfigManager.GetConnectionString("channelingkeypool");
                if (str11 != string.Empty)
                {
                    m_QueryManager.m_ChannelingKeyPool = new ChannelingKeyPoolSqlAdapter();
                    m_QueryManager.m_ChannelingKeyPool.Initialize(str11);
                }
                else
                {
                    m_QueryManager.m_ChannelingKeyPool = null;
                }
                string str12 = ConfigManager.GetConnectionString("charidpool");
                if (str12 != string.Empty)
                {
                    m_QueryManager.m_CharacterIdPool = new CharIdPoolSqlAdapter();
                    m_QueryManager.m_CharacterIdPool.Initialize(str12);
                }
                else
                {
                    m_QueryManager.m_CharacterIdPool = null;
                }
                string str13 = ConfigManager.GetConnectionString("itemidpool");
                if (str13 != string.Empty)
                {
                    m_QueryManager.m_ItemIdPool = new ItemIdPoolSqlAdapter();
                    m_QueryManager.m_ItemIdPool.Initialize(str13);
                }
                else
                {
                    m_QueryManager.m_ItemIdPool = null;
                }
                string str14 = ConfigManager.GetConnectionString("propidpool");
                if (str14 != string.Empty)
                {
                    m_QueryManager.m_PropIdPool = new PropIdPoolSqlAdapter();
                    m_QueryManager.m_PropIdPool.Initialize(str14);
                }
                else
                {
                    m_QueryManager.m_PropIdPool = null;
                }
                string str15 = ConfigManager.GetConnectionString("guildidpool");
                if (str15 != string.Empty)
                {
                    m_QueryManager.m_GuildIdPool = new GuildIdPoolSqlAdapter();
                    m_QueryManager.m_GuildIdPool.Initialize(str15);
                }
                else
                {
                    m_QueryManager.m_GuildIdPool = null;
                }
                string str16 = ConfigManager.GetConnectionString("loginidpool");
                if (str16 != string.Empty)
                {
                    m_QueryManager.m_LoginIdPool = new LoginIdPoolSqlAdapter();
                    m_QueryManager.m_LoginIdPool.Initialize(str16);
                }
                else
                {
                    m_QueryManager.m_LoginIdPool = null;
                }
                string str17 = ConfigManager.GetConnectionString("bididpool");
                if (str17 != string.Empty)
                {
                    m_QueryManager.m_BidIdPool = new BidIdPoolSqlAdapter();
                    m_QueryManager.m_BidIdPool.Initialize(str17);
                }
                else
                {
                    m_QueryManager.m_BidIdPool = null;
                }
                string str18 = ConfigManager.GetConnectionString("memo");
                if ((str18 != null) && (str18 != string.Empty))
                {
                    m_QueryManager.m_MemoAdapter = new MemoSqlAdapter();
                    m_QueryManager.m_MemoAdapter.Initialize(str18);
                }
                else
                {
                    m_QueryManager.m_MemoAdapter = new MemoEmptyAdapter();
                    if (Console.Out != null)
                    {
                        Console.WriteLine("Memo DB Connection string is NULL or EMPTY. All Memo related commands will return failure.");
                    }
                    ExceptionMonitor.ExceptionRaised(new Exception("Memo DB connection string is null."));
                }
                string str19 = ConfigManager.GetConnectionString("chronicle");
                if ((str19 != null) && (str19 != string.Empty))
                {
                    m_QueryManager.m_ChronicleAdapter = new ChronicleSqlAdapter();
                    m_QueryManager.m_ChronicleAdapter.Initialize(str19);
                }
                else
                {
                    m_QueryManager.m_ChronicleAdapter = new ChronicleEmptyAdapter();
                    if (Console.Out != null)
                    {
                        Console.WriteLine(@"Chronicle DB Connection string in XMLDB\Config.xml is NULL or EMPTY.");
                        Console.WriteLine("XMLDB Server Chronicle support is DISABLED");
                        Console.WriteLine("All Server Chronicle related commands will return failure.");
                        Console.WriteLine("Chronicle is important feature of Mabinogi C2G4");
                    }
                    ExceptionMonitor.ExceptionRaised(new Exception("Chronicle DB connection string is null."));
                }
                string str20 = ConfigManager.GetConnectionString("ruin");
                if ((str20 != null) && (str20 != string.Empty))
                {
                    m_QueryManager.m_RuinAdapter = new RuinSqlAdapter();
                    m_QueryManager.m_RuinAdapter.Initialize(str20);
                }
                else
                {
                    m_QueryManager.m_RuinAdapter = new RuinEmptyAdapter();
                    if (Console.Out != null)
                    {
                        Console.WriteLine(@"Ruin DB Connection string in XMLDB\Config.xml is NULL or EMPTY.");
                        Console.WriteLine("XMLDB Server Ruin support is DISABLED");
                        Console.WriteLine("All Server Ruin related commands will return failure.");
                        Console.WriteLine("Ruin is important feature of Mabinogi C2G4");
                    }
                    ExceptionMonitor.ExceptionRaised(new Exception("Ruin DB connection string is null."));
                }
                string str21 = ConfigManager.GetConnectionString("shopadvertise");
                if ((str21 != null) && (str21 != string.Empty))
                {
                    m_QueryManager.m_ShopAdvertiseAdapter = new ShopAdvertiseSqlAdapter();
                    m_QueryManager.m_ShopAdvertiseAdapter.Initialize(str21);
                }
                else
                {
                    m_QueryManager.m_ShopAdvertiseAdapter = new ShopAdvertiseEmptyAdapter();
                    if (Console.Out != null)
                    {
                        Console.WriteLine(@"Shop Advertise DB Connection string in XMLDB\Config.xml is NULL or EMPTY.");
                        Console.WriteLine("XMLDB Server Shop Advertise support is DISABLED");
                        Console.WriteLine("All Server Shop Advertise related commands will return failure.");
                        Console.WriteLine("Shop Advertise is important feature of Mabinogi C2G4S2");
                    }
                    ExceptionMonitor.ExceptionRaised(new Exception("Shop Advertise DB connection string is null."));
                }
                string str22 = ConfigManager.GetConnectionString("dungeonrank");
                if ((str22 != null) && (str22 != string.Empty))
                {
                    m_QueryManager.m_DungeonRankAdapter = new DungeonRankSqlAdapter();
                    m_QueryManager.m_DungeonRankAdapter.Initialize(str22);
                }
                else
                {
                    m_QueryManager.m_DungeonRankAdapter = new DungeonRankEmptyAdapter();
                    if (Console.Out != null)
                    {
                        Console.WriteLine(@"DungeonRank DB Connection string in XMLDB\Config.xml is NULL or EMPTY.");
                        Console.WriteLine("XMLDB Server Dungeon Rank support is DISABLED");
                        Console.WriteLine("All Server Dungeon Rank related commands will return failure.");
                        Console.WriteLine("Dungeon Rank is important feature of Mabinogi C2G5S4");
                    }
                    ExceptionMonitor.ExceptionRaised(new Exception("Dungeon Rank DB connection string is null."));
                }
                string str23 = ConfigManager.GetConnectionString("promotionrank");
                if ((str23 != null) && (str23 != string.Empty))
                {
                    m_QueryManager.m_PromotionAdapter = new PromotionSqlAdapter();
                    m_QueryManager.m_PromotionAdapter.Initialize(str23);
                }
                else
                {
                    m_QueryManager.m_PromotionAdapter = new PromotionEmptyAdapter();
                    if (Console.Out != null)
                    {
                        Console.WriteLine(@"Promotion DB Connection string in XMLDB\Config.xml is NULL or EMPTY.");
                        Console.WriteLine("XMLDB Server Promotion Rank support is DISABLED");
                        Console.WriteLine("All Server Promotion Rank related commands will return failure.");
                        Console.WriteLine("Promotion Rank is important feature of Mabinogi C2G7S3");
                    }
                    ExceptionMonitor.ExceptionRaised(new Exception("Promotion Rank DB connection string is null."));
                }
                string str24 = ConfigManager.GetConnectionString("mailbox");
                if ((str24 != null) && (str24 != string.Empty))
                {
                    m_QueryManager.m_MailBoxAdapter = new MailBoxSqlAdapter();
                    m_QueryManager.m_MailBoxAdapter.Initialize(str24);
                }
                else
                {
                    m_QueryManager.m_MailBoxAdapter = new MailBoxEmptyAdapter();
                    if (Console.Out != null)
                    {
                        Console.WriteLine(@"MailBox DB Connection string in XMLDB\Config.xml is NULL or EMPTY.");
                        Console.WriteLine("XMLDB Server MailBox support is DISABLED");
                        Console.WriteLine("All Server MailBox related commands will return failure.");
                        Console.WriteLine("MailBox is important feature of Mabinogi C2G7S4");
                    }
                    ExceptionMonitor.ExceptionRaised(new Exception("MailBox DB connection string is null."));
                }
                string str25 = ConfigManager.GetConnectionString("farm");
                if ((str25 != null) && (str25 != string.Empty))
                {
                    m_QueryManager.m_FarmAdapter = new FarmSqlAdapter();
                    m_QueryManager.m_FarmAdapter.Initialize(str25);
                }
                else
                {
                    m_QueryManager.m_FarmAdapter = new FarmEmptyAdapter();
                    if (Console.Out != null)
                    {
                        Console.WriteLine(@"Farm DB Connection string in XMLDB\Config.xml is NULL or EMPTY.");
                        Console.WriteLine("XMLDB Server Farm support is DISABLED");
                        Console.WriteLine("All Server Farm related commands will return failure.");
                        Console.WriteLine("Farm is important feature of Mabinogi C3G9S2");
                    }
                    ExceptionMonitor.ExceptionRaised(new Exception("Farm DB connection string is null."));
                }
                string str26 = ConfigManager.GetConnectionString("bid");
                if ((str26 != null) && (str26 != string.Empty))
                {
                    m_QueryManager.m_BidAdapter = new BidSqlAdapter();
                    m_QueryManager.m_BidAdapter.Initialize(str26);
                }
                else
                {
                    m_QueryManager.m_BidAdapter = new BidEmptyAdapter();
                    if (Console.Out != null)
                    {
                        Console.WriteLine(@"Bid DB Connection string in XMLDB\Config.xml is NULL or EMPTY.");
                        Console.WriteLine("XMLDB Server Bid support is DISABLED");
                        Console.WriteLine("All Server Bid related commands will return failure.");
                        Console.WriteLine("Bid is important feature of Mabinogi C3G9S2");
                    }
                    ExceptionMonitor.ExceptionRaised(new Exception("Bid DB connection string is null."));
                }
                string str27 = ConfigManager.GetConnectionString("event");
                if ((str27 != null) && (str27 != string.Empty))
                {
                    m_QueryManager.m_EventAdapter = new EventSqlAdapter();
                    m_QueryManager.m_EventAdapter.Initialize(str27);
                }
                else
                {
                    m_QueryManager.m_EventAdapter = new EventEmptyAdapter();
                    if (Console.Out != null)
                    {
                        Console.WriteLine(@"Event DB Connection string in XMLDB\Config.xml is NULL or EMPTY.");
                        Console.WriteLine("XMLDB Server Event support is DISABLED");
                        Console.WriteLine("All Server Event related commands will return failure.");
                        Console.WriteLine("Event is important feature of Mabinogi C3G10S1");
                    }
                    ExceptionMonitor.ExceptionRaised(new Exception("Event DB connection string is null."));
                }
                string str28 = ConfigManager.GetConnectionString("worldmeta");
                if ((str28 != null) && (str28 != string.Empty))
                {
                    m_QueryManager.m_WorldMetaAdapter = new WorldMetaSqlAdapter();
                    m_QueryManager.m_WorldMetaAdapter.Initialize(str28);
                }
                else
                {
                    m_QueryManager.m_WorldMetaAdapter = new WorldMetaEmptyAdapter();
                    if (Console.Out != null)
                    {
                        Console.WriteLine(@"WorldMeta DB Connection string in XMLDB\Config.xml is NULL or EMPTY.");
                        Console.WriteLine("XMLDB Server WorldMeta support is DISABLED");
                        Console.WriteLine("All Server Event related commands will return failure.");
                        Console.WriteLine("WorldMeta is important feature of Mabinogi C3G10S2");
                    }
                    ExceptionMonitor.ExceptionRaised(new Exception("WorldMeta DB connection string is null."));
                }
                string str29 = ConfigManager.GetConnectionString("wine");
                if ((str29 != null) && (str29 != string.Empty))
                {
                    m_QueryManager.m_WineAdapter = new WineSqlAdapter();
                    m_QueryManager.m_WineAdapter.Initialize(str29);
                }
                else
                {
                    m_QueryManager.m_WineAdapter = new WineEmptyAdapter();
                    if (Console.Out != null)
                    {
                        Console.WriteLine(@"Wine DB Connection string in XMLDB\Config.xml is NULL or EMPTY.");
                        Console.WriteLine("XMLDB Server wine support is DISABLED");
                        Console.WriteLine("All Server Event related commands will return failure.");
                        Console.WriteLine("Wine is important feature of Mabinogi C3G10S2");
                    }
                    ExceptionMonitor.ExceptionRaised(new Exception("wine DB connection string is null."));
                }
                string str30 = ConfigManager.GetConnectionString("wine");
                if ((str30 != null) && (str30 != string.Empty))
                {
                    m_QueryManager.m_RoyalAlchemistAdapter = new RoyalAlchemistSqlAdapter();
                    m_QueryManager.m_RoyalAlchemistAdapter.Initialize(str30);
                }
                else
                {
                    m_QueryManager.m_RoyalAlchemistAdapter = new RoyalAlchemistEmptyAdapter();
                    if (Console.Out != null)
                    {
                        Console.WriteLine(@"royalAlchemist DB Connection string in XMLDB\Config.xml is NULL or EMPTY.");
                        Console.WriteLine("XMLDB Server royalAlchemist support is DISABLED");
                        Console.WriteLine("All Server Event related commands will return failure.");
                        Console.WriteLine("royalAlchemist is important feature of Mabinogi C3G11S2");
                    }
                    ExceptionMonitor.ExceptionRaised(new Exception("royalAlchemist DB connection string is null."));
                }
                string str31 = ConfigManager.GetConnectionString("wine");
                if ((str31 != null) && (str31 != string.Empty))
                {
                    m_QueryManager.m_FamilyAdapter = new FamilySqlAdapter();
                    m_QueryManager.m_FamilyAdapter.Initialize(str31);
                }
                else
                {
                    m_QueryManager.m_FamilyAdapter = new FamilyEmptyAdapter();
                    if (Console.Out != null)
                    {
                        Console.WriteLine(@"family DB Connection string in XMLDB\Config.xml is NULL or EMPTY.");
                        Console.WriteLine("XMLDB Server family support is DISABLED");
                        Console.WriteLine("All Server Event related commands will return failure.");
                        Console.WriteLine("family is important feature of Mabinogi C3G12S1");
                    }
                    ExceptionMonitor.ExceptionRaised(new Exception("family DB connection string is null."));
                }
                string str32 = ConfigManager.GetConnectionString("countryreport");
                if (str32 != string.Empty)
                {
                    m_QueryManager.m_CountryReportAdapter = new CountryReportSqlAdapter();
                    m_QueryManager.m_CountryReportAdapter.Initialize(str32);
                }
                else
                {
                    m_QueryManager.m_CountryReportAdapter = null;
                }
                string str33 = ConfigManager.GetConnectionString("loginoutreport");
                if (str32 != string.Empty)
                {
                    m_QueryManager.m_LogInOutReportAdapter = new LogInOutReportSqlAdapter();
                    m_QueryManager.m_LogInOutReportAdapter.Initialize(str33);
                }
                else
                {
                    m_QueryManager.m_LogInOutReportAdapter = null;
                }
                string str34 = ConfigManager.GetConnectionString("husky");
                if ((str34 != null) && (str34 != string.Empty))
                {
                    m_QueryManager.m_HuskyAdapter = new HuskySqlAdapter();
                    m_QueryManager.m_HuskyAdapter.Initialize(str34);
                }
                else
                {
                    m_QueryManager.m_HuskyAdapter = null;
                }
            }
        }

        public static AccountAdapter Account
        {
            get
            {
                return m_QueryManager.m_AccountAdapter;
            }
        }

        public static AccountActivationAdapter AccountActivation
        {
            get
            {
                return m_QueryManager.m_AccountActivationAdapter;
            }
        }

        public static AccountRefAdapter Accountref
        {
            get
            {
                return m_QueryManager.m_AccountRefAdapter;
            }
        }

        public static BankAdapter Bank
        {
            get
            {
                return m_QueryManager.m_BankAdapter;
            }
        }

        public static BidAdapter Bid
        {
            get
            {
                return m_QueryManager.m_BidAdapter;
            }
        }

        public static BidIdPoolAdapter BidIdPool
        {
            get
            {
                return m_QueryManager.m_BidIdPool;
            }
        }

        public static CastleAdapter Castle
        {
            get
            {
                return m_QueryManager.m_CastleAdapter;
            }
        }

        public static ChannelingKeyPoolAdapter ChannelingKeyPool
        {
            get
            {
                return m_QueryManager.m_ChannelingKeyPool;
            }
        }

        public static CharacterAdapter Character
        {
            get
            {
                return m_QueryManager.m_CharacterAdapter;
            }
        }

        public static CharIdPoolAdapter CharacterIdPool
        {
            get
            {
                return m_QueryManager.m_CharacterIdPool;
            }
        }

        public static ChronicleAdapter Chronicle
        {
            get
            {
                return m_QueryManager.m_ChronicleAdapter;
            }
        }

        public static CountryReportAdapter CountryReport
        {
            get
            {
                return m_QueryManager.m_CountryReportAdapter;
            }
        }

        public static DungeonRankAdapter DungeonRank
        {
            get
            {
                return m_QueryManager.m_DungeonRankAdapter;
            }
        }

        public static EventAdapter Event
        {
            get
            {
                return m_QueryManager.m_EventAdapter;
            }
        }

        public static FamilyAdapter Family
        {
            get
            {
                return m_QueryManager.m_FamilyAdapter;
            }
        }

        public static FarmAdapter Farm
        {
            get
            {
                return m_QueryManager.m_FarmAdapter;
            }
        }

        public static GuildAdapter Guild
        {
            get
            {
                return m_QueryManager.m_GuildAdapter;
            }
        }

        public static GuildIdPoolAdapter GuildIdPool
        {
            get
            {
                return m_QueryManager.m_GuildIdPool;
            }
        }

        public static HouseAdapter House
        {
            get
            {
                return m_QueryManager.m_HouseAdapter;
            }
        }

        public static HuskyAdapter HuskyEvent
        {
            get
            {
                return m_QueryManager.m_HuskyAdapter;
            }
        }

        public static ItemIdPoolAdapter ItemIDPool
        {
            get
            {
                return m_QueryManager.m_ItemIdPool;
            }
        }

        public static LoginIdPoolAdapter LoginIdPool
        {
            get
            {
                return m_QueryManager.m_LoginIdPool;
            }
        }

        public static LogInOutReportAdapter LogInOutReport
        {
            get
            {
                return m_QueryManager.m_LogInOutReportAdapter;
            }
        }

        public static MailBoxAdapter MailBox
        {
            get
            {
                return m_QueryManager.m_MailBoxAdapter;
            }
        }

        public static MemoAdapter Memo
        {
            get
            {
                return m_QueryManager.m_MemoAdapter;
            }
        }

        public static PetAdapter Pet
        {
            get
            {
                return m_QueryManager.m_PetAdapter;
            }
        }

        public static PromotionAdapter PromotionRank
        {
            get
            {
                return m_QueryManager.m_PromotionAdapter;
            }
        }

        public static PropAdapter Prop
        {
            get
            {
                return m_QueryManager.m_PropAdapter;
            }
        }

        public static PropIdPoolAdapter PropIdPool
        {
            get
            {
                return m_QueryManager.m_PropIdPool;
            }
        }

        public static RoyalAlchemistAdapter RoyalAlchemist
        {
            get
            {
                return m_QueryManager.m_RoyalAlchemistAdapter;
            }
        }

        public static RuinAdapter Ruin
        {
            get
            {
                return m_QueryManager.m_RuinAdapter;
            }
        }

        public static ShopAdvertiseAdapter ShopAdvertise
        {
            get
            {
                return m_QueryManager.m_ShopAdvertiseAdapter;
            }
        }

        public static WebSynchAdapter WebSynch
        {
            get
            {
                return m_QueryManager.m_WebSynchAdapter;
            }
        }

        public static WineAdapter Wine
        {
            get
            {
                return m_QueryManager.m_WineAdapter;
            }
        }

        public static WorldMetaAdapter WorldMeta
        {
            get
            {
                return m_QueryManager.m_WorldMetaAdapter;
            }
        }
    }
}

