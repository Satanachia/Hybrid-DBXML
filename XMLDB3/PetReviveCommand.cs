namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class PetReviveCommand : BasicCommand
    {
        private string m_Account;
        private long m_Id;
        private bool m_Result = false;
        private string m_Server;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("PetReviveCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus(string.Concat(new object[] { "PetReviveCommand.DoProcess() : [", this.m_Id, "-", this.m_Account, "@", this.m_Server, "] 펫을 삭제 취소합니다" }));
            this.m_Result = QueryManager.Accountref.SetPetSlotFlag(this.m_Account, this.m_Id, this.m_Server, 0L);
            if (this.m_Result)
            {
                WorkSession.WriteStatus(string.Concat(new object[] { "PetReviveCommand.DoProcess() : [", this.m_Id, "-", this.m_Account, "@", this.m_Server, "] 펫을 삭제 취소했습니다" }));
            }
            else
            {
                WorkSession.WriteStatus(string.Concat(new object[] { "PetReviveCommand.DoProcess() : [", this.m_Id, "-", this.m_Account, "@", this.m_Server, "] 펫을 삭제 취소에 실패하였습니다" }));
            }
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("PetReviveCommand.MakeMessage() : 함수에 진입하였습니다");
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
            this.m_Server = _Msg.ReadString();
        }
    }
}

