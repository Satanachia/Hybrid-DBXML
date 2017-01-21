namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class CastleGuildMoneyGiveCommand : BasicCommand
    {
        private long m_CastleID = 0L;
        private long m_GuildID = 0L;
        private int m_Money = 0;
        private REPLY_RESULT m_Result = REPLY_RESULT.ERROR;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("CastleGuildMoneyGiveCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("CastleGuildMoneyGiveCommand.DoProcess() : 성의 돈을 길드에 줍니다.");
            this.m_Result = QueryManager.Castle.GiveGuildMoney(this.m_CastleID, this.m_GuildID, this.m_Money, QueryManager.Guild);
            if (this.m_Result == REPLY_RESULT.SUCCESS)
            {
                WorkSession.WriteStatus("CastleGuildMoneyGiveCommand.DoProcess() : 성의 돈을 길드에 줬습니다.");
                return true;
            }
            WorkSession.WriteStatus("CastleGuildMoneyGiveCommand.DoProcess() : 성의 돈을 길드에 주는데 실패하였습니다.");
            return false;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("CastleGuildMoneyGiveCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            message.WriteU8((byte) this.m_Result);
            return message;
        }

        protected override void ReceiveData(Message _Msg)
        {
            this.m_CastleID = _Msg.ReadS64();
            this.m_GuildID = _Msg.ReadS64();
            this.m_Money = _Msg.ReadS32();
        }
    }
}

