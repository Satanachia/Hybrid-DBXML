namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class PetDeleteRsvCommand : BasicCommand
    {
        private string m_Account;
        private long m_Id;
        private bool m_Result = false;
        private string m_Server;
        private long m_Time;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("PetDeleteRsvCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus(string.Concat(new object[] { "PetDeleteRsvCommand.DoProcess() : [", this.m_Id, "-", this.m_Account, "@", this.m_Server, "] 펫을 삭제 예약합니다" }));
            this.m_Result = QueryManager.Accountref.SetPetSlotFlag(this.m_Account, this.m_Id, this.m_Server, this.m_Time);
            if (this.m_Result)
            {
                WorkSession.WriteStatus(string.Concat(new object[] { "PetDeleteRsvCommand.DoProcess() : [", this.m_Id, "-", this.m_Account, "@", this.m_Server, "] 펫을 삭제 예약하였습니다" }));
            }
            else
            {
                WorkSession.WriteStatus(string.Concat(new object[] { "PetDeleteRsvCommand.DoProcess() : [", this.m_Id, "-", this.m_Account, "@", this.m_Server, "] 펫을 삭제 예약에 실패하였습니다" }));
            }
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("PetDeleteRsvCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_Result)
            {
                message.WriteU8(1);
                message.WriteU64((ulong) this.m_Id);
                return message;
            }
            message.WriteU8(0);
            message.WriteU64((ulong) this.m_Id);
            return message;
        }

        protected override void ReceiveData(Message _Msg)
        {
            this.m_Account = _Msg.ReadString();
            this.m_Id = (long) _Msg.ReadU64();
            this.m_Time = (long) _Msg.ReadU64();
            this.m_Server = _Msg.ReadString();
        }
    }
}

