namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class PetDeleteCommand : BasicCommand
    {
        private string m_Account;
        private long m_Id;
        private bool m_Result = false;
        private string m_Server;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("PetDeleteCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus(string.Concat(new object[] { "PetDeleteCommand.DoProcess() : [", this.m_Id, "/알수없음@", this.m_Server, "] 펫을 삭제합니다" }));
            this.m_Result = QueryManager.Pet.DeleteEx(this.m_Account, this.m_Server, this.m_Id, QueryManager.Accountref, QueryManager.WebSynch);
            if (this.m_Result)
            {
                WorkSession.WriteStatus(string.Concat(new object[] { "PetDeleteCommand.DoProcess() : [", this.m_Id, "/알수없음@", this.m_Server, "] 펫을 삭제하였습니다" }));
            }
            else
            {
                WorkSession.WriteStatus(string.Concat(new object[] { "PetDeleteCommand.DoProcess() : [", this.m_Id, "/알수없음@", this.m_Server, "] 펫 삭제에 실패하였습니다" }));
            }
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("PetDeleteCommand.MakeMessage() : 함수에 진입하였습니다");
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

