namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class PetCreateCommand : BasicCommand
    {
        private string desc = string.Empty;
        private string m_Account = string.Empty;
        private bool m_Result = false;
        private string m_Server = string.Empty;
        private PetInfo m_WritePet = null;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("PetCreateCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("PetCreateCommand.DoProcess() : [" + this.desc + "] 펫을 생성합니다");
            this.m_Result = QueryManager.Pet.CreateEx(this.m_Account, this.m_Server, this.m_WritePet, QueryManager.Accountref, QueryManager.WebSynch);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("PetCreateCommand.DoProcess() : [" + this.desc + "] 캐릭터를 생성하였습니다");
                return true;
            }
            WorkSession.WriteStatus("PetCreateCommand.DoProcess() : [" + this.desc + "] 캐릭터를 생성하는데 실패하였습니다");
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("PetCreateCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_Result)
            {
                message.WriteU8(1);
                message.WriteU64((ulong) this.m_WritePet.id);
                return message;
            }
            message.WriteU8(0);
            return message;
        }

        protected override void ReceiveData(Message _Msg)
        {
            this.m_Account = _Msg.ReadString();
            this.m_Server = _Msg.ReadString();
            this.m_WritePet = PetSerializer.Serialize(_Msg);
            this.desc = string.Concat(new object[] { this.m_WritePet.id, "/", this.m_WritePet.name, "@", this.m_Server });
        }
    }
}

