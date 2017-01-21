namespace XMLDB3
{
    using Mabinogi;
    using System;

    public abstract class BasicCommand
    {
        private BasicCommand m_ChainCommand = null;
        private uint m_ID = 0;
        private bool m_IsValid = false;
        private uint m_QueryId = 0;
        private int m_TargetClient = 0;

        protected BasicCommand()
        {
        }

        public abstract bool DoProcess();
        public abstract Message MakeMessage();
        public virtual void OnError()
        {
            if (this.m_IsValid && this.ReplyEnable)
            {
                Message message = this.MakeMessage();
                MainProcedure.ServerSend(this.Target, message);
            }
        }

        public static BasicCommand Parse(int _ClientId, Message _Msg)
        {
            uint num = _Msg.ReadU32();
            BasicCommand command = null;
            switch (((NETWORKMSG) _Msg.ID))
            {
                case NETWORKMSG.MC_DB_ACCOUNT_CREATE:
                    command = new AccountCreateCommand();
                    break;

                case NETWORKMSG.MC_DB_ACCOUNT_READ:
                    command = new AccountReadCommand();
                    break;

                case NETWORKMSG.MC_DB_CHARACTER_CREATE:
                    command = new CharacterCreateCommand();
                    break;

                case NETWORKMSG.MC_DB_CHARACTER_UPDATE:
                    command = new CharacterUpdateCommand();
                    break;

                case NETWORKMSG.MC_DB_CHARACTER_READ:
                    command = new CharacterReadCommand();
                    break;

                case NETWORKMSG.MC_DB_CHARACTER_DELETE:
                    command = new CharacterDeleteCommand();
                    break;

                case NETWORKMSG.MC_DB_CHARACTER_DELETE_RESERVE:
                    command = new CharacterDeleteRsvCommand();
                    break;

                case NETWORKMSG.MC_DB_CHARACTER_REVIVE:
                    command = new CharacterReviveCommand();
                    break;

                case NETWORKMSG.MC_DB_ACCOUNT_REF_CREATE:
                    command = new AccountrefCreateCommand();
                    break;

                case NETWORKMSG.MC_DB_ACCOUNT_REF_READ:
                    command = new AccountrefReadCommand();
                    break;

                case NETWORKMSG.MC_DB_ACCOUNT_REF_LOBBY_OPTION_UPDATE:
                    command = new AccountrefLobbyUpdateCommand();
                    break;

                case NETWORKMSG.NET_DB_BANK_ACCOUNT_UPDATE:
                    command = new BankUpdateCommand();
                    break;

                case NETWORKMSG.NET_DB_BANK_ACCOUNT_READ:
                    command = new BankReadCommand();
                    break;

                case NETWORKMSG.MC_DB_CHARACTER_READ_APPEAR:
                    command = new CharacterReadCommand();
                    break;

                case NETWORKMSG.NET_DB_BANK_ACCOUNT_UPDATE_EX:
                    command = new BankUpdateExCommand();
                    break;

                case NETWORKMSG.NET_DB_CHARACTER_NAME_USABLE:
                    command = new CharacterNameUsableCommand();
                    break;

                case NETWORKMSG.NET_DB_CHARACTER_READ_WRITE_COUNTER:
                    command = new CharacterGetWriteCounterCommand();
                    break;

                case NETWORKMSG.MC_DB_ACCOUNT_READ_SMS_AUTH:
                    command = new AccountReadSMSCommand();
                    break;

                case NETWORKMSG.NET_DB_PROP_CREATE:
                    command = new PropCreateCommand();
                    break;

                case NETWORKMSG.NET_DB_PROP_UPDATE:
                    command = new PropUpdateCommand();
                    break;

                case NETWORKMSG.NET_DB_PROP_READ:
                    command = new PropReadCommand();
                    break;

                case NETWORKMSG.NET_DB_PROP_DELETE:
                    command = new PropDeleteCommand();
                    break;

                case NETWORKMSG.NET_DB_PROPLIST_READ:
                    command = new PropListGetCommand();
                    break;

                case NETWORKMSG.NET_DB_GUILD_CREATE:
                    command = new GuildCreateCommand();
                    break;

                case NETWORKMSG.NET_DB_GUILD_UPDATE:
                    command = new GuildUpdateCommand();
                    break;

                case NETWORKMSG.NET_DB_GUILD_READ:
                    command = new GuildReadCommand();
                    break;

                case NETWORKMSG.NET_DB_GUILD_DELETE:
                    command = new GuildDeleteCommand();
                    break;

                case NETWORKMSG.NET_DB_GUILDLIST_READ:
                    command = new GuildListGetCommand();
                    break;

                case NETWORKMSG.NET_DB_GUILD_MEMBER_ADD:
                    command = new GuildMemberAddCommand();
                    break;

                case NETWORKMSG.NET_DB_GUILD_POINT_ADD:
                    command = new GuildPointAddCommand();
                    break;

                case NETWORKMSG.NET_DB_GUILD_MONEY_ADD:
                    command = new GuildMoneyAddCommand();
                    break;

                case NETWORKMSG.NET_DB_GUILD_STONE_ADD:
                    command = new GuildStoneAddCommand();
                    break;

                case NETWORKMSG.NET_DB_GUILD_TRANSFER_MASTER:
                    command = new GuildTransferMasterCommand();
                    break;

                case NETWORKMSG.NET_DB_GUILD_NAME_USABLE:
                    command = new GuildNameUsableCommand();
                    break;

                case NETWORKMSG.NET_DB_GUILD_DESTROY_GUILDSTONE:
                    command = new GuildStoneDeleteCommand();
                    break;

                case NETWORKMSG.NET_DB_GUILD_MEMEBER_CHECK_JOINTIME:
                    command = new GuildMemberCheckJointimeCommand();
                    break;

                case NETWORKMSG.NET_DB_GUILD_DRAW_MONEY:
                    command = new GuildDrawMoneyCommand();
                    break;

                case NETWORKMSG.NET_DB_GUILD_CHANGE_GUILDSTONE:
                    command = new GuildStoneChangeCommand();
                    break;

                case NETWORKMSG.NET_DB_ITEM_ID_POOL:
                    command = new ItemIdPoolCommand();
                    break;

                case NETWORKMSG.NET_DB_CHARACTER_ID_POOL:
                    command = new CharacterIdPoolCommand();
                    break;

                case NETWORKMSG.NET_DB_GUILD_ID_POOL:
                    command = new GuildIdPoolCommand();
                    break;

                case NETWORKMSG.NET_DB_PROP_ID_POOL:
                    command = new PropIdPoolCommand();
                    break;

                case NETWORKMSG.NET_DB_GUILD_UPDATE_ROBE:
                    command = new GuildRobeUpdateCommand();
                    break;

                case NETWORKMSG.NET_DB_GUILD_DESTROY_ROBE:
                    command = new GuildRobeDeleteCommand();
                    break;

                case NETWORKMSG.NET_DB_GUILD_UPDATE_TITLE:
                    command = new GuildUpdateTitleCommand();
                    break;

                case NETWORKMSG.NET_DB_SIGNAL_LOGIN:
                    command = new LoginCommand();
                    break;

                case NETWORKMSG.NET_DB_SIGNAL_LOGOUT:
                    command = new LogoutCommand();
                    break;

                case NETWORKMSG.NET_DB_SIGNAL_PLAYIN:
                    command = new PlayinCommand();
                    break;

                case NETWORKMSG.NET_DB_SIGNAL_PLAYOUT:
                    command = new PlayoutCommand();
                    break;

                case NETWORKMSG.NET_DB_CHECK_CACHEKEY:
                    command = new CheckCacheKeyCommand();
                    break;

                case NETWORKMSG.NET_DB_GUILD_UPDATE_BATTLEGROUND_TYPE:
                    command = new GuildBattleGroundTypeUpdateCommand();
                    break;

                case NETWORKMSG.NET_DB_GUILD_CLEAR_BATTLEGROUND_TYPE:
                    command = new GuildBattleGroundTypeClearCommand();
                    break;

                case NETWORKMSG.NET_DB_GUILD_UPDATE_BATTLEGROUND_WINNER_TYPE:
                    command = new GuildBattleGroundWinnerTypeUpdateCommand();
                    break;

                case NETWORKMSG.NET_DB_GUILD_UPDATE_STATUS_FLAG:
                    command = new GuildStatusUpdateCommand();
                    break;

                case NETWORKMSG.NET_DB_GUILD_JOINED_MEMBER_COUNT:
                    command = new GuildGetJoinedMemberCountCommand();
                    break;

                case NETWORKMSG.NET_DB_PET_CREATE:
                    command = new PetCreateCommand();
                    break;

                case NETWORKMSG.NET_DB_PET_UPDATE:
                    command = new PetUpdateCommand();
                    break;

                case NETWORKMSG.NET_DB_PET_READ:
                    command = new PetReadCommand();
                    break;

                case NETWORKMSG.NET_DB_PET_DELETE:
                    command = new PetDeleteCommand();
                    break;

                case NETWORKMSG.NET_DB_PET_DELETE_RESERVE:
                    command = new PetDeleteRsvCommand();
                    break;

                case NETWORKMSG.NET_DB_PET_REVIVE:
                    command = new PetReviveCommand();
                    break;

                case NETWORKMSG.NET_DB_PET_READ_APPEAR:
                    command = new PetReadCommand();
                    break;

                case NETWORKMSG.NET_DB_PET_READ_WRITE_COUNTER:
                    command = new PetGetWriteCounterCommand();
                    break;

                case NETWORKMSG.NET_DB_CASTLE_LIST_READ:
                    command = new CastleListReadCommand();
                    break;

                case NETWORKMSG.NET_DB_CASTLE_BID_START:
                    command = new CastleBidStartCommand();
                    break;

                case NETWORKMSG.NET_DB_CASTLE_BIDDER_CREATE:
                    command = new CastleBidderCreateCommand();
                    break;

                case NETWORKMSG.NET_DB_CASTLE_BIDDER_UPDATE:
                    command = new CastleBidderUpdateCommand();
                    break;

                case NETWORKMSG.NET_DB_CASTLE_BIDDER_DELETE:
                    command = new CastleBidderDeleteCommand();
                    break;

                case NETWORKMSG.NET_DB_CASTLE_BID_END:
                    command = new CastleBidEndCommand();
                    break;

                case NETWORKMSG.NET_DB_CASTLE_UPDATE:
                    command = new CastleUpdateCommand();
                    break;

                case NETWORKMSG.NET_DB_CASTLE_BUILD_UPDATE:
                    command = new CastleBuildUpdateCommand();
                    break;

                case NETWORKMSG.NET_DB_CASTLE_BUILD_ITEM_UPDATE:
                    command = new CastleBuildItemUpdateCommand();
                    break;

                case NETWORKMSG.NET_DB_CASTLE_GUILD_MONEY_TAKE:
                    command = new CastleGuildMoneyTakeCommand();
                    break;

                case NETWORKMSG.NET_DB_CASTLE_GUILD_MONEY_GIVE:
                    command = new CastleGuildMoneyGiveCommand();
                    break;

                case NETWORKMSG.NET_DB_CASTLE_BLOCK_UPDATE:
                    command = new CastleBlockUpdateCommand();
                    break;

                case NETWORKMSG.NET_DB_HOUSE_READ:
                    command = new HouseReadCommand();
                    break;

                case NETWORKMSG.NET_DB_HOUSE_BID_START:
                    command = new HouseBidStartCommand();
                    break;

                case NETWORKMSG.NET_DB_HOUSE_BIDDER_CREATE:
                    command = new HouseBidderCreateCommand();
                    break;

                case NETWORKMSG.NET_DB_HOUSE_BIDDER_DELETE:
                    command = new HouseBidderDeleteCommand();
                    break;

                case NETWORKMSG.NET_DB_HOUSE_BID_END:
                    command = new HouseBidEndCommand();
                    break;

                case NETWORKMSG.NET_DB_HOUSE_BID_REPAY_END:
                    command = new HouseBidRepayEndCommand();
                    break;

                case NETWORKMSG.NET_DB_HOUSE_UPDATE:
                    command = new HouseUpdateCommand();
                    break;

                case NETWORKMSG.NET_DB_HOUSE_ITEM_READ:
                    command = new HouseItemReadCommand();
                    break;

                case NETWORKMSG.NET_DB_HOUSE_ITEM_UPDATE:
                    command = new HouseItemUpdateCommand();
                    break;

                case NETWORKMSG.NET_DB_HOUSE_ITEM_DELETE:
                    command = new HouseItemDeleteCommand();
                    break;

                case NETWORKMSG.NET_DB_HOUSE_OWNER_DELETE:
                    command = new HouseOwnerDeleteCommand();
                    break;

                case NETWORKMSG.NET_DB_HOUSE_BLOCK_READ:
                    command = new HouseBlockReadCommand();
                    break;

                case NETWORKMSG.NET_DB_HOUSE_BLOCK_UPDATE:
                    command = new HouseBlockUpdateCommand();
                    break;

                case NETWORKMSG.NET_DB_HOUSE_BIDDER_AUTOREPAY:
                    command = new HouseBidderAutoRepayCommand();
                    break;

                case NETWORKMSG.NET_DB_HOUSE_BLOCK_DELETE:
                    command = new HouseBlockDeleteCommand();
                    break;

                case NETWORKMSG.NET_DB_MEMO_SEND:
                    command = new MemoSendCommand();
                    break;

                case NETWORKMSG.NET_DB_ITEM_DELETE:
                    command = new CharacterItemDeleteCommand();
                    break;

                case NETWORKMSG.NET_DB_PET_ITEM_DELETE:
                    command = new PetItemDeleteCommand();
                    break;

                case NETWORKMSG.NET_DB_CHRONICLE_INSERT_LOG:
                    command = new ChronicleCreateCommand();
                    break;

                case NETWORKMSG.NET_DB_CHRONICLE_INFO_LIST_INIT:
                    command = new ChronicleInfoInitCommand();
                    break;

                case NETWORKMSG.NET_DB_RUIN_READ:
                    command = new RuinReadCommand();
                    break;

                case NETWORKMSG.NET_DB_RUIN_UPDATE:
                    command = new RuinUpdateCommand();
                    break;

                case NETWORKMSG.NET_DB_RELIC_READ:
                    command = new RelicReadCommand();
                    break;

                case NETWORKMSG.NET_DB_RELIC_UPDATE:
                    command = new RelicUpdateCommand();
                    break;

                case NETWORKMSG.NET_DB_ADVERTISE_QUERY:
                    command = new ShopAdvertiseReadCommand();
                    break;

                case NETWORKMSG.NET_DB_ADVERTISE_REGISTER:
                    command = new ShopAdvertiseRegisterCommand();
                    break;

                case NETWORKMSG.NET_DB_ADVERTISE_UNREGISTER:
                    command = new ShopAdvertiseUnregisterCommand();
                    break;

                case NETWORKMSG.NET_DB_ADVERTISE_ITEM_ADD:
                    command = new ShopAdvertiseItemAddCommand();
                    break;

                case NETWORKMSG.NET_DB_ADVERTISE_ITEM_REMOVE:
                    command = new ShopAdvertiseItemDeleteCommand();
                    break;

                case NETWORKMSG.NET_DB_ADVERTISE_ITEM_SET_PRICE:
                    command = new ShopAdvertiseItemPriceUpdateCommand();
                    break;

                case NETWORKMSG.NET_DB_ADVERTISE_UPDATE:
                    command = new ShopAdvertiseUpdateCommand();
                    break;

                case NETWORKMSG.NET_DB_DUNGEON_RANKING_UPDATE:
                    command = new DungeonRankUpdateCommand();
                    break;

                case NETWORKMSG.NET_DB_CHANNELING_CHECK_AND_INSERT:
                    command = new ChannelingKeyPoolCheckAndInsertComand();
                    break;

                case NETWORKMSG.MC_DB_ACCOUNT_CREATE_ACTIVATE:
                    command = new AccountActivationCommand();
                    break;

                case NETWORKMSG.NET_DB_PROMOTION_BEGIN_TEST:
                    command = new PromotionBeginCommand();
                    break;

                case NETWORKMSG.NET_DB_PROMOTION_RECORD_POINT:
                    command = new PromotionRecordScoreCommand();
                    break;

                case NETWORKMSG.NET_DB_PROMOTION_END_TEST:
                    command = new PromotionEndCommand();
                    break;

                case NETWORKMSG.NET_DB_MAIL_READ:
                    command = new MailReadCommand();
                    break;

                case NETWORKMSG.NET_DB_MAIL_SEND:
                    command = new MailSendCommand();
                    break;

                case NETWORKMSG.NET_DB_MAIL_DELETE:
                    command = new MailDeleteCommand();
                    break;

                case NETWORKMSG.NET_DB_MAIL_UPDATE_STATUS:
                    command = new MailUpdateStatusCommand();
                    break;

                case NETWORKMSG.NET_DB_MAIL_CHECK_CHARACTER:
                    command = new MailCheckCharacterComamnd();
                    break;

                case NETWORKMSG.NET_DB_MAIL_GET_UNREAD_COUNT:
                    command = new MailGetUnreadCountCommand();
                    break;

                case NETWORKMSG.NET_DB_FARM_READ:
                    command = new FarmReadCommand();
                    break;

                case NETWORKMSG.NET_DB_FARM_LEASE:
                    command = new FarmLeaseCommand();
                    break;

                case NETWORKMSG.NET_DB_FARM_EXPIRE:
                    command = new FarmExpireCommand();
                    break;

                case NETWORKMSG.NET_DB_FARM_UPDATE:
                    command = new FarmUpdateCommand();
                    break;

                case NETWORKMSG.NET_DB_FARM_ACCOUNT_OWN:
                    command = new FarmAccountOwnCommand();
                    break;

                case NETWORKMSG.NET_DB_AUCTION_BID_ID_POOL:
                    command = new BidIdPoolCommand();
                    break;

                case NETWORKMSG.NET_DB_AUCTION_BID_READ:
                    command = new BidReadCommand();
                    break;

                case NETWORKMSG.NET_DB_AUCTION_BID_ADD:
                    command = new BidAddCommand();
                    break;

                case NETWORKMSG.NET_DB_AUCTION_BID_UPDATE:
                    command = new BidUpdateCommand();
                    break;

                case NETWORKMSG.NET_DB_AUCTION_BID_REMOVE:
                    command = new BidRemoveCommand();
                    break;

                case NETWORKMSG.NET_DB_WORLDMETA_READ:
                    command = new WorldMetaReadCommand();
                    break;

                case NETWORKMSG.NET_DB_WORLDMETA_UPDATE:
                    command = new WorldMetaUpdateCommand();
                    break;

                case NETWORKMSG.NET_DB_EVENT_UPDATE:
                    command = new EventUpdateCommand();
                    break;

                case NETWORKMSG.NET_DB_WINE_AGING_READ:
                    command = new WineReadCommand();
                    break;

                case NETWORKMSG.NET_DB_WINE_AGING_UPDATE:
                    command = new WineUpdateCommand();
                    break;

                case NETWORKMSG.NET_DB_WINE_AGING_REMOVE:
                    command = new WineDeleteCommand();
                    break;

                case NETWORKMSG.NET_DB_BAN_ACCOUNT:
                    command = new AccountBanCommand();
                    break;

                case NETWORKMSG.NET_DB_UNBAN_ACCOUNT:
                    command = new AccountUnbanCommand();
                    break;

                case NETWORKMSG.NET_DB_QUERY_BANK_PASSWORD:
                    command = new BankQueryPasswordCommand();
                    break;

                case NETWORKMSG.NET_DB_QUERY_ACCUM_LEVEL:
                    command = new CharacterQueryAccumLevelCommand();
                    break;

                case NETWORKMSG.NET_DB_ROYALALCHEMIST_READ:
                    command = new RoyalAlchemistReadCommand();
                    break;

                case NETWORKMSG.NET_DB_ROYALALCHEMIST_LIST:
                    command = new RoyalAlchemistReadListCommand();
                    break;

                case NETWORKMSG.NET_DB_ROYALALCHEMIST_ADD:
                    command = new RoyalAlchemistAddCommand();
                    break;

                case NETWORKMSG.NET_DB_ROYALALCHEMIST_REMOVE:
                    command = new RoyalAlchemistRemoveCommand();
                    break;

                case NETWORKMSG.NET_DB_ROYALALCHEMIST_UPDATE:
                    command = new RoyalAlchemistUpdateCommand();
                    break;

                case NETWORKMSG.NET_DB_COUNTRY_CCU_REPORT:
                    command = new CountryCCUReportCommand();
                    break;

                case NETWORKMSG.NET_DB_COUNTRY_LOGINOUT_REPORT:
                    command = new LogInOutReportCommand();
                    break;

                case NETWORKMSG.NET_DB_FAMILY_LIST:
                    command = new FamilyReadListCommand();
                    break;

                case NETWORKMSG.NET_DB_FAMILY_READ:
                    command = new FamilyReadCommand();
                    break;

                case NETWORKMSG.NET_DB_FAMILY_ADD:
                    command = new FamilyAddCommand();
                    break;

                case NETWORKMSG.NET_DB_FAMILY_REMOVE:
                    command = new FamilyRemoveCommand();
                    break;

                case NETWORKMSG.NET_DB_FAMILY_UPDATE:
                    command = new FamilyUpdateCommand();
                    break;

                case NETWORKMSG.NET_DB_FAMILY_ADD_MEMBER:
                    command = new FamilyAddMemberCommand();
                    break;

                case NETWORKMSG.NET_DB_FAMILY_REMOVE_MEMBER:
                    command = new FamilyRemoveMemberCommand();
                    break;

                case NETWORKMSG.NET_DB_FAMILY_UPDATE_MEMBER:
                    command = new FamilyUpdateMemberCommand();
                    break;

                case NETWORKMSG.NET_DB_HUSKY_CALLPROCEDURE:
                    command = new HuskyCallProcedureCommand();
                    break;

                case NETWORKMSG.NET_DB_REMOVE_RESERVED_CHARNAME_INFO:
                    command = new RemoveReservedCharNameCommand();
                    break;

                case NETWORKMSG.NET_DB_UPDATE_ACCOUNT_REF_FLAG:
                    command = new AccountRefUpdateFlagCommand();
                    break;

                case NETWORKMSG.NET_DB_ITEMMARKET_INFO:
                    command = new ItemMarketInfoCommand();
                    break;

                case NETWORKMSG.NET_DB_ITEMMARKET_CHECKENTRANCE:
                    command = new CheckEntranceCommand();
                    break;

                case NETWORKMSG.NET_DB_ITEMMARKET_GETITEM:
                    command = new GetItemCommand();
                    break;

                case NETWORKMSG.NET_DB_ITEMMARKET_GETITEM_COMMIT:
                    command = new GetItemCommitCommand();
                    break;

                case NETWORKMSG.NET_DB_ITEMMARKET_GETITEM_ROLLBACK:
                    command = new GetItemRollbackCommand();
                    break;

                case NETWORKMSG.NET_DB_ITEMMARKET_INQUIRY_MYPAGE:
                    command = new InquiryMyPageCommand();
                    break;

                case NETWORKMSG.NET_DB_ITEMMARKET_INQUIRY_SALEITEM:
                    command = new InquirySaleItemCommand();
                    break;

                case NETWORKMSG.NET_DB_ITEMMARKET_INQUIRY_STORAGE:
                    command = new InquiryStorageCommand();
                    break;

                case NETWORKMSG.NET_DB_ITEMMARKET_ITEMLIST:
                    command = new ItemListCommand();
                    break;

                case NETWORKMSG.NET_DB_ITEMMARKET_ITEMPURCHASE:
                    command = new ItemPurchaseCommand();
                    break;

                case NETWORKMSG.NET_DB_ITEMMARKET_ITEMSEARCH:
                    command = new ItemSearchCommand();
                    break;

                case NETWORKMSG.NET_DB_ITEMMARKET_SALE_REQUEST:
                    command = new SaleRequestCommand();
                    break;

                case NETWORKMSG.NET_DB_ITEMMARKET_SALE_CANCEL:
                    command = new SaleCancelCommand();
                    break;

                case NETWORKMSG.NET_DB_ITEMMARKET_SALE_REQUEST_COMMIT:
                    command = new SaleRequestCommitCommmand();
                    break;

                case NETWORKMSG.NET_DB_ITEMMARKET_SALE_REQUEST_ROLLBACK:
                    command = new SaleRequestRollbackCommand();
                    break;

                case NETWORKMSG.NET_DB_ITEMMARKET_CHECKBALANCE:
                    command = new CheckBalanceCommand();
                    break;

                case NETWORKMSG.NET_DB_LOGIN_ID_POOL:
                    command = new LoginIdPoolCommand();
                    break;

                default:
                    throw new Exception(string.Concat(new object[] { "connection ", _ClientId, " send invalid message ", _Msg.ID }));
            }
            if (command != null)
            {
                command.m_TargetClient = _ClientId;
                command.m_ID = _Msg.ID;
                command.m_QueryId = num;
            }
            try
            {
                command.ReceiveData(_Msg);
            }
            catch (Exception exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, command);
                command.OnError();
                return null;
            }
            command.m_IsValid = true;
            return command;
        }

        public virtual bool Prepare()
        {
            return true;
        }

        protected abstract void ReceiveData(Message _Msg);
        public override string ToString()
        {
            return (base.GetType().Name + ":" + this.m_ID.ToString() + ":" + (this.m_IsValid ? "Valid" : "Invalid"));
        }

        public uint ID
        {
            get
            {
                return this.m_ID;
            }
        }

        public virtual bool IsPrimeCommand
        {
            get
            {
                return false;
            }
        }

        public bool IsValid
        {
            get
            {
                return this.m_IsValid;
            }
        }

        public BasicCommand Next
        {
            get
            {
                return this.m_ChainCommand;
            }
            set
            {
                this.m_ChainCommand = value;
            }
        }

        public uint QueryID
        {
            get
            {
                return this.m_QueryId;
            }
        }

        public virtual bool ReplyEnable
        {
            get
            {
                return true;
            }
        }

        public int Target
        {
            get
            {
                return this.m_TargetClient;
            }
        }
    }
}

