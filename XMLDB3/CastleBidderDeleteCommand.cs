namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class CastleBidderDeleteCommand : BasicCommand
    {
        private long m_CastleID = 0L;
        private long m_GuildID = 0L;
        private int m_RemainMoney = 0;
        private int m_RepayMoney = 0;
        private REPLY_RESULT m_Result = REPLY_RESULT.ERROR;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("CastleBidderDeleteCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("CastleBidderDeleteCommand.DoProcess() : 성 입찰을 취소합니다.");
            this.m_Result = QueryManager.Castle.DeleteBidder(this.m_CastleID, this.m_GuildID, this.m_RepayMoney, QueryManager.Guild, ref this.m_RemainMoney);
            if (this.m_Result == REPLY_RESULT.SUCCESS)
            {
                WorkSession.WriteStatus("CastleBidderDeleteCommand.DoProcess() : 성 입찰을 취소하였습니다.");
                return true;
            }
            WorkSession.WriteStatus("CastleBidderDeleteCommand.DoProcess() : 성 입찰을 취소하는데 실패하였습니다.");
            return false;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("CastleBidderDeleteCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            message.WriteU8((byte) this.m_Result);
            if (this.m_Result == REPLY_RESULT.SUCCESS)
            {
                message.WriteS32(this.m_RemainMoney);
            }
            return message;
        }

        protected override void ReceiveData(Message _Msg)
        {
            this.m_CastleID = _Msg.ReadS64();
            this.m_GuildID = _Msg.ReadS64();
            this.m_RepayMoney = _Msg.ReadS32();
        }
    }
}

