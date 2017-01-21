namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class HouseOwnerDeleteCommand : SerializedCommand
    {
        private string m_Account = string.Empty;
        private long m_HouseID = 0L;
        private bool m_Result = false;
        private string m_Server = string.Empty;

        protected override bool _DoProces()
        {
            WorkSession.WriteStatus("HouseOwnerDeleteCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("HouseOwnerDeleteCommand.DoProcess() : 집 주인을 삭제합니다.");
            this.m_Result = QueryManager.House.DeleteOwner(this.m_HouseID, this.m_Account, this.m_Server);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("HouseOwnerDeleteCommand.DoProcess() :집 주인을 삭제하였습니다.");
            }
            else
            {
                WorkSession.WriteStatus("HouseOwnerDeleteCommand.DoProcess() :집 주인을 삭제하는데 실패하였습니다.");
            }
            return this.m_Result;
        }

        protected override void _ReceiveData(Message _Msg)
        {
            this.m_HouseID = _Msg.ReadS64();
            this.m_Account = _Msg.ReadString();
            this.m_Server = _Msg.ReadString();
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("HouseOwnerDeleteCommand.MakeMessage() : 함수에 진입하였습니다");
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

        public override void OnSerialize(IObjLockRegistHelper _helper, bool bBegin)
        {
            _helper.ObjectIDRegistant(this.m_HouseID);
        }
    }
}

