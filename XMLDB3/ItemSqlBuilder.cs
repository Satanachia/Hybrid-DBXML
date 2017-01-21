namespace XMLDB3
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Runtime.CompilerServices;

    public sealed class ItemSqlBuilder
    {
        private static readonly string[] bankDeleteProc = new string[] { null, "BankItemDelete_Large", "BankItemDelete_Small", "BankItemDelete_Huge", "BankItemDelete_Quest", null };
        private static readonly string[] bankSelfUpdateProc = new string[] { null, "BankItemSelfUpdate_Large", "BankItemSelfUpdate_Small", "BankItemSelfUpdate_Huge", "BankItemSelfUpdate_Quest", null };
        private static readonly string[] bankUpdateProc = new string[] { null, "BankItemUpdate_Large", "BankItemUpdate_Small", "BankItemUpdate_Huge", "BankItemUpdate_Quest", null };
        private static readonly string[] charCheckedUpdateProc = new string[] { null, "CharItemCheckedUpdate_Large", "CharItemCheckedUpdate_Small", "CharItemCheckedUpdate_Huge", "CharItemCheckedUpdate_Quest", "CharItemCheckedUpdate_Ego" };
        private static readonly string[] charDeleteProc = new string[] { null, "CharItemDelete_Large", "CharItemDelete_Small", "CharItemDelete_Huge", "CharItemDelete_Quest", "CharItemDelete_Ego" };
        private static readonly string[] charSelfUpdateProc = new string[] { null, "CharItemSelfUpdate_Large", "CharItemSelfUpdate_Small", "CharItemSelfUpdate_Huge", "CharItemSelfUpdate_Quest", "CharItemSelfUpdate_Ego" };
        private static readonly string[] charUpdateProc = new string[] { null, "CharItemUpdate_Large", "CharItemUpdate_Small", "CharItemUpdate_Huge", "CharItemUpdate_Quest", "CharItemUpdate_Ego" };
        private static readonly string[] houseDeleteProc = new string[] { null, "HouseItemDelete_Large", "HouseItemDelete_Small", "HouseItemDelete_Huge", "HouseItemDelete_Quest", null };
        private static readonly string[] houseItemSelectProc = new string[] { null, "SelectHouseItemLarge", "SelectHouseItemSmall", "SelectHouseItemHuge", "SelectHouseItemQuest", null };
        private static readonly string[] houseSelfUpdateProc = new string[] { null, "HouseItemSelfUpdate_Large", "HouseItemSelfUpdate_Small", "HouseItemSelfUpdate_Huge", "HouseItemSelfUpdate_Quest", null };
        private static readonly ObjectBuilder[] objBuilders = new ObjectBuilder[] { null, new ObjectBuilder(ItemObjectBuilder.BuildLargeItem), new ObjectBuilder(ItemObjectBuilder.BuildSmallItem), new ObjectBuilder(ItemObjectBuilder.BuildHugeItem), new ObjectBuilder(ItemObjectBuilder.BuildQuestItem), new ObjectBuilder(ItemObjectBuilder.BuildEgoItem) };
        private static readonly ParameterBuilder[] paramBuilders = new ParameterBuilder[] { null, new ParameterBuilder(ItemParameterBuilder.BuildLargeItem), new ParameterBuilder(ItemParameterBuilder.BuildSmallItem), new ParameterBuilder(ItemParameterBuilder.BuildHugeItem), new ParameterBuilder(ItemParameterBuilder.BuildQuestItem), new ParameterBuilder(ItemParameterBuilder.BuildEgoItem) };
        private static readonly UpdateChecker[] updateCheckers = new UpdateChecker[] { null, new UpdateChecker(ItemUpdateChecker.CheckLargeItem), new UpdateChecker(ItemUpdateChecker.CheckSmallItem), new UpdateChecker(ItemUpdateChecker.CheckHugeItem), new UpdateChecker(ItemUpdateChecker.CheckQuestItem), new UpdateChecker(ItemUpdateChecker.CheckEgoItem) };

        public static string BankDeleteItem(string _account, BankItem _item)
        {
            return string.Concat(new object[] { "exec dbo.", bankDeleteProc[_item.item.storedtype], " @account=", UpdateUtility.BuildString(_account), ",@itemID=", _item.item.id, "\n" });
        }

        public static string BankSelfUpdateItem(string _account, string _slotName, BankRace _race, BankItem _item)
        {
            return string.Concat(new object[] { "exec dbo.", bankSelfUpdateProc[_item.item.storedtype], " @account=", UpdateUtility.BuildString(_account), ",@slotname=", UpdateUtility.BuildString(_slotName), ",@race=", (byte) _race, ",@location=", UpdateUtility.BuildString(_item.location), ",@extratime=", _item.extraTime, ",@time=", _item.time, paramBuilders[_item.item.storedtype](_item.item), "\n" });
        }

        public static string BankUpdateItem(BankSlotInfo _newSlot, BankSlotInfo _oldSlot, BankItem _new, BankItem _old)
        {
            if (((!(_newSlot.name != _oldSlot.name) && (_newSlot.race == _oldSlot.race)) && (!(_new.location != _old.location) && (_new.extraTime == _old.extraTime))) && ((_new.time == _old.time) && !updateCheckers[_new.item.storedtype](_new.item, _old.item)))
            {
                return string.Empty;
            }
            return string.Concat(new object[] { "exec dbo.", bankUpdateProc[_new.item.storedtype], " @slotname=", UpdateUtility.BuildString(_newSlot.name), ",@race=", _newSlot.race, ",@location=", UpdateUtility.BuildString(_new.location), ",@extraTime=", _new.extraTime, ",@time=", _new.time, paramBuilders[_new.item.storedtype](_new.item), "\n" });
        }

        public static string DeleteItem(long _gameID, long _itemID, byte _storedtype)
        {
            return string.Concat(new object[] { "exec dbo.", charDeleteProc[_storedtype], " @id=", _gameID, ",@itemID=", _itemID, "\n" });
        }

        public static bool ForceUpdateRetry(SqlException _ex)
        {
            if (_ex.Number == 0xa43)
            {
                foreach (string str in charCheckedUpdateProc)
                {
                    if (_ex.Procedure == str)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static BankItem GetBankItem(Item.StoredType _storedType, DataRow _row)
        {
            BankItem item = new BankItem();
            item.location = (string) _row["location"];
            item.extraTime = (long) _row["extraTime"];
            item.time = (long) _row["time"];
            item.item = GetCharItem(_storedType, _row);
            return item;
        }

        public static Item GetCharItem(Item.StoredType _storedType, DataRow _row)
        {
            return objBuilders[(int) _storedType](_row);
        }

        public static HouseItem GetHouseItem(Item.StoredType _storedType, DataRow _row)
        {
            HouseItem item = new HouseItem();
            item.posX = (byte) _row["posX"];
            item.posY = (byte) _row["posY"];
            item.direction = (byte) _row["direction"];
            item.userprice = (int) _row["userprice"];
            item.pocket = (byte) _row["pocket"];
            item.item = GetCharItem(_storedType, _row);
            return item;
        }

        public static string GetHouseItemSelectProc(Item.StoredType _type)
        {
            return houseItemSelectProc[(int) _type];
        }

        public static string HouseDeleteItem(string _account, long _houseID, Item _item, int _money)
        {
            return string.Concat(new object[] { "exec dbo.", houseDeleteProc[_item.storedtype], " @strAccount=", UpdateUtility.BuildString(_account), ",@idHouse=", _houseID, ",@itemID=", _item.id, ",@money=", _money, "\n" });
        }

        public static string HouseSelfUpdateItem(string _account, HouseItem _item)
        {
            return string.Concat(new object[] { "exec dbo.", houseSelfUpdateProc[_item.item.storedtype], " @account=", UpdateUtility.BuildString(_account), ",@posX=", _item.posX, ",@posY=", _item.posY, ",@direction=", _item.direction, ",@userprice=", _item.userprice, ",@pocket=", _item.pocket, paramBuilders[_item.item.storedtype](_item.item), "\n" });
        }

        public static string SelfUpdateItem(long _gameID, Item _item, bool _bForceUpdate)
        {
            return string.Concat(new object[] { "exec dbo.", _bForceUpdate ? charSelfUpdateProc[_item.storedtype] : charCheckedUpdateProc[_item.storedtype], " @id=", _gameID, paramBuilders[_item.storedtype](_item), "\n" });
        }

        public static string UpdateItem(long _gameID, Item _new, Item _old)
        {
            if (updateCheckers[_new.storedtype](_new, _old))
            {
                return string.Concat(new object[] { "exec dbo.", charUpdateProc[_new.storedtype], " @id=", _gameID, paramBuilders[_new.storedtype](_new), "\n" });
            }
            return string.Empty;
        }

        private delegate Item ObjectBuilder(DataRow _row);

        private delegate string ParameterBuilder(Item _item);

        private delegate bool UpdateChecker(Item _new, Item _old);
    }
}

