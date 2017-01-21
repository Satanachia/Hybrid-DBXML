namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class CharacterNameUsableCommand : BasicCommand
    {
        private string m_Account;
        private string m_Name;
        private bool m_Result = false;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("CharacterNameUsableCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("CharacterNameUsableCommand.DoProcess() : 캐릭터 이름 [" + this.m_Name + "] 을 중복체크합니다");
            this.m_Result = QueryManager.Character.IsUsableName(this.m_Name, this.m_Account);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("CharacterNameUsableCommand.DoProcess() : 캐릭터 이름 [" + this.m_Name + "] 가 사용가능합니다");
            }
            else
            {
                WorkSession.WriteStatus("CharacterNameUsableCommand.DoProcess() : 캐릭터 이름 [" + this.m_Name + "] 가 사용불가합니다");
            }
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("CharacterNameUsableCommand.MakeMessage() : 함수에 진입하였습니다");
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

        protected override void ReceiveData(Message _Msg)
        {
            this.m_Name = _Msg.ReadString();
            this.m_Account = _Msg.ReadString();
        }
    }
}

