namespace XMLDB3
{
    using System;
    using System.IO;

    public class PromotionFileAdapter : FileAdapter, PromotionAdapter
    {
        public bool BeginPromotion(string serverName, string channelName, ushort skillid)
        {
            try
            {
                if (File.Exists(base.GetFileName(serverName)))
                {
                    File.Delete(base.GetFileName(serverName));
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool EndPromotion(string serverName, ushort skillid)
        {
            return true;
        }

        public void Initialize(string _Argument)
        {
            base.Initialize(typeof(PromotionRecordTable), ConfigManager.GetFileDBPath("promotionrank"), ".xml");
        }

        public bool RecordScore(string serverName, string channelName, ushort skillid, string skillCategory, string skillName, ulong characterID, string characterName, byte race, ushort level, uint point)
        {
            PromotionRecord record = new PromotionRecord();
            record.serverName = serverName;
            record.skillid = skillid;
            record.skillCategory = skillCategory;
            record.skillName = skillName;
            record.characterID = characterID;
            record.characterName = characterName;
            record.race = race;
            record.level = level;
            record.point = point;
            record.channel = channelName;
            PromotionRecordTable table = new PromotionRecordTable();
            table.records = new PromotionRecord[] { record };
            try
            {
                base.WriteToDB(table, serverName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

