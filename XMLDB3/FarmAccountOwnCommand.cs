namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class FarmAccountOwnCommand : BasicCommand
    {
        private string m_Account = "";
        private long m_FarmId = 0L;
        private long m_OwnerCharID = 0L;
        private string m_OwnerCharName = "";
        private bool m_Result = false;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("FarmAccountOwnCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("FarmAccountOwnCommand.DoProcess() : 소유한 농장을 읽어옵니다.");
            this.m_Result = QueryManager.Farm.GetOwnerInfo(this.m_Account, ref this.m_FarmId, ref this.m_OwnerCharID, ref this.m_OwnerCharName);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("FarmAccountOwnCommand.DoProcess() :  농장을 읽어왔습니다");
                return true;
            }
            WorkSession.WriteStatus("FarmAccountOwnCommand.DoProcess() : 농장을 얻는데 실패하였습니다");
            return false;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("FarmAccountOwnCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_Result)
            {
                message.WriteU8(1);
                message.WriteS64(this.m_FarmId);
                message.WriteS64(this.m_OwnerCharID);
                message.WriteString(this.m_OwnerCharName);
                return message;
            }
            message.WriteU8(0);
            return message;
        }

        protected override void ReceiveData(Message _Msg)
        {
            this.m_Account = _Msg.ReadString();
        }
    }
}

