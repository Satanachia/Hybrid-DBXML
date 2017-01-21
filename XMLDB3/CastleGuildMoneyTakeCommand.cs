namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class CastleGuildMoneyTakeCommand : BasicCommand
    {
        private long m_CastleID = 0L;
        private long m_GuildID = 0L;
        private int m_Money = 0;
        private int m_RemainMoney = 0;
        private REPLY_RESULT m_Result = REPLY_RESULT.ERROR;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("CastleGuildMoneyTakeCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("CastleGuildMoneyTakeCommand.DoProcess() : 길드 머니를 성에 추가합니다.");
            this.m_Result = QueryManager.Castle.TakeGuildMoney(this.m_CastleID, this.m_GuildID, this.m_Money, ref this.m_RemainMoney, QueryManager.Guild);
            if (this.m_Result == REPLY_RESULT.SUCCESS)
            {
                WorkSession.WriteStatus("CastleGuildMoneyTakeCommand.DoProcess() : 길드 머니를 성에 추가하였습니다.");
                return true;
            }
            WorkSession.WriteStatus("CastleGuildMoneyTakeCommand.DoProcess() : 길드 머니를 성에 추가하는데 실패하였습니다.");
            return false;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("CastleGuildMoneyTakeCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            message.WriteU8((byte) this.m_Result);
            if (this.m_Result == REPLY_RESULT.FAIL_EX)
            {
                message.WriteU8(0);
                message.WriteS32(this.m_RemainMoney);
            }
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

