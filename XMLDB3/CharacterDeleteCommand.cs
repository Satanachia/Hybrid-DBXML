namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class CharacterDeleteCommand : BasicCommand
    {
        private string m_Account;
        private long m_Id;
        private bool m_Result = false;
        private string m_Server;
        private int m_SupportLastChangeTime;
        private byte m_SupportRace;
        private byte m_SupportRewardState;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("CharacterDeleteCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus(string.Concat(new object[] { "CharacterDeleteCommand.DoProcess() : [", this.m_Id, "/알수없음@", this.m_Server, "] 캐릭터를 삭제합니다" }));
            this.m_Result = QueryManager.Character.DeleteEx(this.m_Account, this.m_SupportRace, this.m_SupportRewardState, this.m_SupportLastChangeTime, this.m_Server, this.m_Id, QueryManager.Accountref, QueryManager.Bank, QueryManager.Guild, QueryManager.WebSynch);
            if (this.m_Result)
            {
                WorkSession.WriteStatus(string.Concat(new object[] { "CharacterDeleteCommand.DoProcess() : [", this.m_Id, "/알수없음@", this.m_Server, "] 캐릭터를 삭제하였습니다" }));
            }
            else
            {
                WorkSession.WriteStatus(string.Concat(new object[] { "CharacterDeleteCommand.DoProcess() : [", this.m_Id, "/알수없음@", this.m_Server, "] 캐릭터 삭제에 실패하였습니다" }));
            }
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("CharacterDeleteCommand.MakeMessage() : 함수에 진입하였습니다");
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

        protected override void ReceiveData(Message _message)
        {
            this.m_Account = _message.ReadString();
            this.m_SupportLastChangeTime = _message.ReadS32();
            this.m_SupportRace = _message.ReadU8();
            this.m_SupportRewardState = _message.ReadU8();
            this.m_Server = _message.ReadString();
            this.m_Id = (long) _message.ReadU64();
        }
    }
}

