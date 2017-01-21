namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class CharacterCreateCommand : BasicCommand
    {
        private string desc = string.Empty;
        private string m_Account = string.Empty;
        private byte m_Race = 0;
        private bool m_Result = false;
        private string m_Server = string.Empty;
        private bool m_SupportCharacter = false;
        private byte m_SupportRewardState = 0;
        private CharacterInfo m_WriteCharacter = null;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("CharacterCreateCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("CharacterCreateCommand.DoProcess() : [" + this.desc + "] 캐릭터를 생성합니다");
            this.m_Result = QueryManager.Character.CreateEx(this.m_Account, this.m_SupportRewardState, this.m_Server, this.m_Race, this.m_SupportCharacter, this.m_WriteCharacter, QueryManager.Accountref, QueryManager.Bank, QueryManager.WebSynch);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("CharacterCreateCommand.DoProcess() : [" + this.desc + "] 캐릭터를 생성하였습니다");
                return true;
            }
            WorkSession.WriteStatus("CharacterCreateCommand.DoProcess() : [" + this.desc + "] 캐릭터를 생성하는데 실패하였습니다");
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("CharacterCreateCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_Result)
            {
                message.WriteU8(1);
                message.WriteU64((ulong) this.m_WriteCharacter.id);
                return message;
            }
            message.WriteU8(0);
            return message;
        }

        protected override void ReceiveData(Message _message)
        {
            this.m_Account = _message.ReadString();
            this.m_SupportRewardState = _message.ReadU8();
            this.m_Server = _message.ReadString();
            this.m_Race = _message.ReadU8();
            this.m_SupportCharacter = _message.ReadU8() != 0;
            this.m_WriteCharacter = CharacterSerializer.Serialize(_message);
            this.desc = string.Concat(new object[] { this.m_WriteCharacter.id, "/", this.m_WriteCharacter.name, "@", this.m_Server });
        }
    }
}

