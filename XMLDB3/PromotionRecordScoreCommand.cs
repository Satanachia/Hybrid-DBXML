namespace XMLDB3
{
    using Mabinogi;
    using System;
    using System.Collections;

    public class PromotionRecordScoreCommand : BasicCommand
    {
        private string m_channelName;
        private ArrayList m_characterID = new ArrayList();
        private ArrayList m_characterName = new ArrayList();
        private ArrayList m_level = new ArrayList();
        private ArrayList m_point = new ArrayList();
        private ArrayList m_race = new ArrayList();
        private bool m_Result;
        private string m_serverName;
        private string m_skillCategory;
        private ushort m_skillid;
        private string m_skillName;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("PromotionRecordScoreCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("PromotionRecordScoreCommand.DoProcess() : 점수를 기록합니다.");
            for (int i = 0; i < this.m_characterID.Count; i++)
            {
                this.m_Result = QueryManager.PromotionRank.RecordScore(this.m_serverName, this.m_channelName, this.m_skillid, this.m_skillCategory, this.m_skillName, (ulong) this.m_characterID[i], (string) this.m_characterName[i], (byte) this.m_race[i], (ushort) this.m_level[i], (uint) this.m_point[i]);
                if (this.m_Result)
                {
                    WorkSession.WriteStatus("PromotionRecordScoreCommand.DoProcess() :점수를 기록하였습니다..");
                }
                else
                {
                    WorkSession.WriteStatus("PromotionRecordScoreCommand.DoProcess() : 점수를 기록하지 못했습니다.");
                    break;
                }
            }
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("PromotionRecordScoreCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_Result)
            {
                message.WriteU8(1);
                return message;
            }
            message.WriteU8(0);
            return message;
        }

        protected override void ReceiveData(Message _Msg)
        {
            this.m_serverName = _Msg.ReadString();
            this.m_channelName = _Msg.ReadString();
            this.m_skillCategory = _Msg.ReadString();
            this.m_skillid = _Msg.ReadU16();
            this.m_skillName = _Msg.ReadString();
            while (true)
            {
                byte num = _Msg.ReadU8();
                if (num == 0xff)
                {
                    return;
                }
                this.m_race.Add(num);
                this.m_characterID.Add(_Msg.ReadU64());
                this.m_characterName.Add(_Msg.ReadString());
                this.m_level.Add(_Msg.ReadU16());
                this.m_point.Add(_Msg.ReadU32());
            }
        }
    }
}

