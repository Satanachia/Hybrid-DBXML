namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class CharacterQueryAccumLevelCommand : BasicCommand
    {
        private string m_Account;
        private string m_Name;
        private uint m_Result = 0;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("CharacterQueryAccumLevelCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("CharacterQueryAccumLevelCommand.DoProcess() : 캐릭터 [" + this.m_Account + " / " + this.m_Name + "] 의 누적레벨을 쿼리합니다");
            this.m_Result = QueryManager.Character.GetAccumLevel(this.m_Account, this.m_Name);
            WorkSession.WriteStatus(string.Concat(new object[] { "CharacterQueryAccumLevelCommand.DoProcess() : 캐릭터 [", this.m_Account, " / ", this.m_Name, "] 의 누적레벨은 ", this.m_Result, " 입니다" }));
            return (this.m_Result > 0);
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("CharacterNameUsableCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            message.WriteU32(this.m_Result);
            return message;
        }

        protected override void ReceiveData(Message _Msg)
        {
            this.m_Account = _Msg.ReadString();
            this.m_Name = _Msg.ReadString();
        }
    }
}

