﻿namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class RoyalAlchemistAddCommand : BasicCommand
    {
        private byte m_errorCode = 0;
        private REPLY_RESULT m_Result = REPLY_RESULT.FAIL;
        private RoyalAlchemist m_RoyalAlchemist = null;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("RoyalAlchemistUpdateCommand.DoProcess() : 함수에 진입하였습니다");
            this.m_Result = QueryManager.RoyalAlchemist.Add(this.m_RoyalAlchemist, ref this.m_errorCode);
            if (this.m_Result == REPLY_RESULT.SUCCESS)
            {
                WorkSession.WriteStatus("RoyalAlchemistAddCommand.DoProcess() : 왕궁 연금술사 데이터를 성공적으로 추가했습니다.");
            }
            else
            {
                WorkSession.WriteStatus("RoyalAlchemistAddCommand.DoProcess() : 왕궁 연금술사 데이터를 추가하는데 실패하였습니다.");
            }
            return (this.m_Result == REPLY_RESULT.SUCCESS);
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("RoyalAlchemistAddCommand.MakeMessage() : 함수에 진입하였습니다");
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
            this.m_RoyalAlchemist = RoyalAlchemistSerializer.Serialize(_message);
        }
    }
}

