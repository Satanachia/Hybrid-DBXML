namespace XMLDB3
{
    using System;
    using System.Collections;
    using System.IO;

    public class CastleFileAdapter : FileAdapter, CastleAdapter
    {
        public bool CreateBid(CastleBid _bid)
        {
            CastleList biddingCastle = this.GetBiddingCastle(_bid.castleID);
            if (biddingCastle != null)
            {
                return false;
            }
            if (base.IsExistData(_bid.castleID.ToString()))
            {
                biddingCastle = (CastleList) base.ReadFromDB(_bid.castleID);
                if ((biddingCastle.bidders != null) && (biddingCastle.bidders.Length > 0))
                {
                    return false;
                }
            }
            if (biddingCastle == null)
            {
                biddingCastle = new CastleList();
            }
            biddingCastle.bids = new CastleBid[1];
            _bid.bidStartTime = DateTime.Now;
            biddingCastle.bids[0] = _bid;
            this.InitializeCastleBlock(biddingCastle, _bid.castleID);
            base.WriteToDB(biddingCastle, _bid.castleID);
            return true;
        }

        public REPLY_RESULT CreateBidder(CastleBidder _bidder, GuildAdapter _guildAdapter, ref int _remainMoney)
        {
            CastleList biddingCastle = this.GetBiddingCastle(_bidder.castleID);
            if (biddingCastle == null)
            {
                return REPLY_RESULT.ERROR;
            }
            if (!((GuildFileAdapter) _guildAdapter).WithdrawMoney(_bidder.bidGuildID, _bidder.bidPrice, ref _remainMoney))
            {
                return REPLY_RESULT.ERROR;
            }
            if (_remainMoney >= 0)
            {
                int length;
                CastleBidder[] bidderArray;
                if (biddingCastle.bidders == null)
                {
                    bidderArray = new CastleBidder[1];
                    length = 0;
                }
                else
                {
                    length = biddingCastle.bidders.Length;
                    bidderArray = new CastleBidder[length + 1];
                    biddingCastle.bidders.CopyTo(bidderArray, 0);
                }
                _bidder.bidTime = DateTime.Now;
                _bidder.bidUpdateTime = DateTime.Now;
                bidderArray[length] = _bidder;
                biddingCastle.bidders = bidderArray;
                base.WriteToDB(biddingCastle, _bidder.castleID);
                return REPLY_RESULT.SUCCESS;
            }
            _remainMoney += _bidder.bidPrice;
            return REPLY_RESULT.FAIL_EX;
        }

        public REPLY_RESULT DeleteBidder(long _castleID, long _guildID, int _repayMoney, GuildAdapter _guildAdapter, ref int _remainMoney)
        {
            CastleList biddingCastle = this.GetBiddingCastle(_castleID);
            if (biddingCastle == null)
            {
                return REPLY_RESULT.ERROR;
            }
            ArrayList list2 = new ArrayList();
            bool flag = false;
            foreach (CastleBidder bidder in biddingCastle.bidders)
            {
                if (bidder.bidGuildID == _guildID)
                {
                    flag = true;
                }
                else
                {
                    list2.Add(bidder);
                }
            }
            if (flag)
            {
                ((GuildFileAdapter) _guildAdapter).AddMoney(_guildID, _repayMoney, ref _remainMoney);
                biddingCastle.bidders = (CastleBidder[]) list2.ToArray(typeof(CastleBidder));
                base.WriteToDB(biddingCastle, _castleID);
                return REPLY_RESULT.SUCCESS;
            }
            return REPLY_RESULT.FAIL;
        }

        public bool EndBid(Castle _castle, GuildAdapter _guildAdapter)
        {
            CastleList biddingCastle = this.GetBiddingCastle(_castle.castleID);
            if (biddingCastle == null)
            {
                return false;
            }
            if (biddingCastle.bidders != null)
            {
                foreach (CastleBidder bidder in biddingCastle.bidders)
                {
                    if (bidder.bidGuildID != _castle.guildID)
                    {
                        _guildAdapter.AddMoney(bidder.bidGuildID, bidder.bidPrice);
                    }
                }
            }
            biddingCastle.castles = new Castle[] { _castle };
            biddingCastle.bidders = null;
            biddingCastle.bids = null;
            base.WriteToDB(biddingCastle, _castle.castleID);
            return true;
        }

        private CastleList GetBiddingCastle(long _castleID)
        {
            if (base.IsExistData(_castleID))
            {
                CastleList list = (CastleList) base.ReadFromDB(_castleID);
                if (((list != null) && (list.bids != null)) && (list.bids.Length == 1))
                {
                    return list;
                }
            }
            return null;
        }

        private CastleList GetCastleOfGuild(long _castleID, long _guildID)
        {
            if (!base.IsExistData(_castleID))
            {
                return null;
            }
            CastleList list = (CastleList) base.ReadFromDB(_castleID);
            if (!this.IsValidCastle(list))
            {
                return null;
            }
            if ((_guildID != 0L) && (list.castles[0].guildID != _guildID))
            {
                return null;
            }
            return list;
        }

        public REPLY_RESULT GiveGuildMoney(long _castleID, long _guildID, int _money, GuildAdapter _guildAdapter)
        {
            CastleList castleOfGuild = this.GetCastleOfGuild(_castleID, _guildID);
            if (castleOfGuild != null)
            {
                if (castleOfGuild.castles[0].castleMoney < _money)
                {
                    return REPLY_RESULT.FAIL;
                }
                if (_guildAdapter.AddMoney(_guildID, _money))
                {
                    Castle castle1 = castleOfGuild.castles[0];
                    castle1.castleMoney -= _money;
                    base.WriteToDB(castleOfGuild, _castleID);
                    return REPLY_RESULT.SUCCESS;
                }
            }
            return REPLY_RESULT.ERROR;
        }

        public void Initialize(string _argument)
        {
            base.Initialize(typeof(CastleList), ConfigManager.GetFileDBPath("castle"), ".xml");
        }

        private void InitializeCastleBlock(CastleList _list, long _castleID)
        {
            _list.blocks = new CastleBlockList[] { new CastleBlockList() };
            _list.blocks[0].block = null;
            _list.blocks[0].castleID = _castleID;
        }

        private bool IsValidCastle(CastleList _list)
        {
            return (((_list != null) && (_list.castles != null)) && (_list.castles.Length == 1));
        }

        public CastleList ReadList()
        {
            ArrayList list = new ArrayList();
            ArrayList list2 = new ArrayList();
            ArrayList list3 = new ArrayList();
            ArrayList list4 = new ArrayList();
            string[] files = System.IO.Directory.GetFiles(base.Directory);
            if (files == null)
            {
                return new CastleList();
            }
            foreach (string str in files)
            {
                CastleList list5 = (CastleList) base.ReadFromDB(Path.GetFileNameWithoutExtension(str));
                if (list5 != null)
                {
                    if (list5.bids != null)
                    {
                        foreach (CastleBid bid in list5.bids)
                        {
                            list.Add(bid);
                        }
                    }
                    if (list5.bidders != null)
                    {
                        foreach (CastleBidder bidder in list5.bidders)
                        {
                            list2.Add(bidder);
                        }
                    }
                    if (list5.castles != null)
                    {
                        foreach (Castle castle in list5.castles)
                        {
                            list3.Add(castle);
                        }
                    }
                    if (list5.blocks != null)
                    {
                        foreach (CastleBlockList list6 in list5.blocks)
                        {
                            list4.Add(list6);
                        }
                    }
                }
            }
            CastleList list7 = new CastleList();
            list7.bids = (CastleBid[]) list.ToArray(typeof(CastleBid));
            list7.bidders = (CastleBidder[]) list2.ToArray(typeof(CastleBidder));
            list7.castles = (Castle[]) list3.ToArray(typeof(Castle));
            list7.blocks = (CastleBlockList[]) list4.ToArray(typeof(CastleBlockList));
            return list7;
        }

        public REPLY_RESULT TakeGuildMoney(long _castleID, long _guildID, int _money, ref int _remainMoney, GuildAdapter _guildAdapter)
        {
            int num = 0;
            CastleList castleOfGuild = this.GetCastleOfGuild(_castleID, _guildID);
            if (castleOfGuild == null)
            {
                return REPLY_RESULT.ERROR;
            }
            if (!((GuildFileAdapter) _guildAdapter).WithdrawMoney(_guildID, _money, ref num))
            {
                return REPLY_RESULT.ERROR;
            }
            if (num >= 0)
            {
                Castle castle1 = castleOfGuild.castles[0];
                castle1.castleMoney += _money;
                base.WriteToDB(castleOfGuild, _castleID);
                return REPLY_RESULT.SUCCESS;
            }
            _remainMoney = num + _money;
            return REPLY_RESULT.FAIL_EX;
        }

        public REPLY_RESULT UpdateBidder(long _castleID, long _guildID, int _bidPrice, int _bidDiffPrice, int _bidOrder, GuildAdapter _guildAdapter, ref int _remainMoney)
        {
            CastleList biddingCastle = this.GetBiddingCastle(_castleID);
            if (biddingCastle == null)
            {
                return REPLY_RESULT.ERROR;
            }
            CastleBidder bidder = null;
            foreach (CastleBidder bidder2 in biddingCastle.bidders)
            {
                if (bidder2.bidGuildID == _guildID)
                {
                    bidder = bidder2;
                }
            }
            if (bidder == null)
            {
                return REPLY_RESULT.FAIL;
            }
            if (!((GuildFileAdapter) _guildAdapter).WithdrawMoney(_guildID, _bidDiffPrice, ref _remainMoney))
            {
                return REPLY_RESULT.ERROR;
            }
            if (_remainMoney >= 0)
            {
                bidder.bidPrice += _bidDiffPrice;
                bidder.bidUpdateTime = DateTime.Now;
                base.WriteToDB(biddingCastle, _castleID);
                return REPLY_RESULT.SUCCESS;
            }
            _remainMoney += _bidDiffPrice;
            return REPLY_RESULT.FAIL_EX;
        }

        public bool UpdateBlock(long _castleID, CastleBlock[] _added, CastleBlock[] _deleted)
        {
            if (base.IsExistData(_castleID.ToString()))
            {
                CastleList list = (CastleList) base.ReadFromDB(_castleID);
                if (this.IsValidCastle(list))
                {
                    ArrayList list2 = new ArrayList();
                    if (list.blocks == null)
                    {
                        this.InitializeCastleBlock(list, _castleID);
                    }
                    if (list.blocks[0].block != null)
                    {
                        Hashtable hashtable = new Hashtable();
                        if (_deleted != null)
                        {
                            foreach (CastleBlock block in _deleted)
                            {
                                string key = block.gameName + block.flag + block.entry;
                                if (!hashtable.ContainsKey(key))
                                {
                                    hashtable.Add(key, block);
                                }
                            }
                        }
                        foreach (CastleBlock block2 in list.blocks[0].block)
                        {
                            if (!hashtable.ContainsKey(block2.gameName + block2.flag + block2.entry))
                            {
                                list2.Add(block2);
                            }
                        }
                    }
                    if (_added != null)
                    {
                        foreach (CastleBlock block3 in _added)
                        {
                            list2.Add(block3);
                        }
                    }
                    list.blocks[0].block = (CastleBlock[]) list2.ToArray(typeof(CastleBlock));
                    base.WriteToDB(list, _castleID);
                    return true;
                }
            }
            return false;
        }

        public bool UpdateBuild(long _castleID, CastleBuild _build)
        {
            if (base.IsExistData(_castleID.ToString()))
            {
                CastleList list = (CastleList) base.ReadFromDB(_castleID);
                if (this.IsValidCastle(list))
                {
                    list.castles[0].build = _build;
                    base.WriteToDB(list, _castleID);
                    return true;
                }
            }
            return false;
        }

        public bool UpdateBuildResource(long _castleID, CastleBuildResource _resource)
        {
            if (base.IsExistData(_castleID.ToString()))
            {
                CastleList list = (CastleList) base.ReadFromDB(_castleID);
                if ((this.IsValidCastle(list) && (list.castles[0].build != null)) && (list.castles[0].build.resource != null))
                {
                    foreach (CastleBuildResource resource in list.castles[0].build.resource)
                    {
                        if (resource.classID == _resource.classID)
                        {
                            resource.curAmount = _resource.curAmount;
                            resource.maxAmount = _resource.maxAmount;
                            base.WriteToDB(list, _castleID);
                            return true;
                        }
                    }
                    return false;
                }
            }
            return false;
        }

        public bool Write(Castle _castle)
        {
            if (base.IsExistData(_castle.castleID.ToString()))
            {
                CastleList list = (CastleList) base.ReadFromDB(_castle.castleID);
                if (this.IsValidCastle(list))
                {
                    _castle.build = list.castles[0].build;
                    list.castles[0] = _castle;
                    base.WriteToDB(list, _castle.castleID);
                    return true;
                }
            }
            return false;
        }
    }
}

