namespace XMLDB3
{
    using Mabinogi;
    using System;
    using System.Collections;

    public class PromotionBeginCommand : BasicCommand
    {
        private string m_channelName = string.Empty;
        private bool m_Result;
        private string m_serverName = string.Empty;
        private ArrayList m_skillId = new ArrayList();

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("PromotionBeginCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("PromotionBeginCommand.DoProcess() : 시험을 시작합니다.");
            foreach (ushort num in this.m_skillId)
            {
                this.m_Result = QueryManager.PromotionRank.BeginPromotion(this.m_serverName, this.m_channelName, num);
                if (this.m_Result)
                {
                    WorkSession.WriteStatus("PromotionBeginCommand.DoProcess() :시험이 시작되었습니다.");
                }
                else
                {
                    WorkSession.WriteStatus("PromotionBeginCommand.DoProcess() : 시험이 시작되지 못했습니다.");
                    break;
                }
            }
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("PromotionBeginCommand.MakeMessage() : 함수에 진입하였습니다");
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
            while (true)
            {
                ushort num = _Msg.ReadU16();
                if (num == 0)
                {
                    return;
                }
                this.m_skillId.Add(num);
            }
        }
    }
}

