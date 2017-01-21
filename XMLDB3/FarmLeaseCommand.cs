namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class FarmLeaseCommand : BasicCommand
    {
        private byte m_ErrorCode = 0;
        private long m_ExpireTime = 0L;
        private long m_FarmId = 0L;
        private string m_OwnerAccount = null;
        private long m_OwnerCharId = 0L;
        private string m_OwnerCharName = null;
        private REPLY_RESULT m_Result = REPLY_RESULT.FAIL;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("FarmLeaseCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("FarmLeaseCommand.DoProcess() : 농장을 임대합니다.");
            this.m_Result = QueryManager.Farm.Lease(this.m_FarmId, this.m_OwnerAccount, this.m_OwnerCharId, this.m_OwnerCharName, this.m_ExpireTime, ref this.m_ErrorCode);
            if (this.m_Result == REPLY_RESULT.SUCCESS)
            {
                WorkSession.WriteStatus("FarmLeaseCommand.DoProcess() :  농장을 임대했습니다");
                return true;
            }
            WorkSession.WriteStatus("FarmLeaseCommand.DoProcess() : 농장을 임대하는데 실패하였습니다");
            return false;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("FarmLeaseCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_Result != REPLY_RESULT.FAIL_EX)
            {
                message.WriteU8((byte) this.m_Result);
                return message;
            }
            message.WriteU8((byte) this.m_Result);
            message.WriteU8(this.m_ErrorCode);
            return message;
        }

        protected override void ReceiveData(Message _Msg)
        {
            this.m_FarmId = _Msg.ReadS64();
            this.m_OwnerAccount = _Msg.ReadString();
            this.m_OwnerCharId = _Msg.ReadS64();
            this.m_OwnerCharName = _Msg.ReadString();
            this.m_ExpireTime = _Msg.ReadS64();
        }
    }
}

