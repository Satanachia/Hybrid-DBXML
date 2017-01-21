namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class BankQueryPasswordCommand : SerializedCommand
    {
        private string m_Account = string.Empty;
        private Bank m_Bank = null;
        private string m_CharName = string.Empty;

        protected override bool _DoProces()
        {
            WorkSession.WriteStatus("BankQueryPasswordCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("BankQueryPasswordCommand.DoProcess() : [" + this.m_Account + "] 를 캐쉬에서 읽도록 시도합니다.");
            BankCache cache = (BankCache) ObjectCache.Bank.Extract(this.m_Account);
            if (cache == null)
            {
                cache = new BankCache();
            }
            this.m_Bank = QueryManager.Bank.Read(this.m_Account, this.m_CharName, BankRace.None, cache);
            if (this.m_Bank != null)
            {
                WorkSession.WriteStatus("BankQueryPasswordCommand.DoProcess() : [" + this.m_Account + "] 의 정보를 데이터베이스에서 읽었습니다");
                ObjectCache.Bank.Push(this.m_Account, cache);
            }
            else
            {
                WorkSession.WriteStatus("BankQueryPasswordCommand.DoProcess() : [" + this.m_Account + "] 의 정보를 데이터베이스에 쿼리하는데 실패하였습니다");
            }
            return (this.m_Bank != null);
        }

        protected override void _ReceiveData(Message _Msg)
        {
            this.m_Account = _Msg.ReadString();
            this.m_CharName = _Msg.ReadString();
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("BankQueryPasswordCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_Bank != null)
            {
                message.WriteString(this.m_Bank.data.password);
                return message;
            }
            string str = "";
            message.WriteString(str);
            return message;
        }

        public override void OnSerialize(IObjLockRegistHelper _helper, bool bBegin)
        {
            _helper.StringIDRegistant(this.m_Account);
        }
    }
}

