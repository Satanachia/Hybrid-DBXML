﻿namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class BidRemoveCommand : BasicCommand
    {
        private long m_BidID = 0L;
        private byte m_errorCode = 0;
        private REPLY_RESULT m_Result = REPLY_RESULT.FAIL;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("BidRemoveCommand.DoProcess() : 함수에 진입하였습니다");
            this.m_Result = QueryManager.Bid.Remove(this.m_BidID, ref this.m_errorCode);
            if (this.m_Result == REPLY_RESULT.SUCCESS)
            {
                WorkSession.WriteStatus("BidRemoveCommand.DoProcess() : 경매 데이터를 성공적으로 삭제했습니다.");
            }
            else
            {
                WorkSession.WriteStatus("BidRemoveCommand.DoProcess() : 경매 데이터를 삭제하는데 실패하였습니다.");
            }
            return (this.m_Result == REPLY_RESULT.SUCCESS);
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("BidRemoveCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            message.WriteU8((byte) this.m_Result);
            if (this.m_Result == REPLY_RESULT.FAIL_EX)
            {
                message.WriteU8(this.m_errorCode);
            }
            return message;
        }

        protected override void ReceiveData(Message _message)
        {
            this.m_BidID = _message.ReadS64();
        }
    }
}

