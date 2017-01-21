namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class BankReadCommand : SerializedCommand
    {
        private string m_Account = string.Empty;
        private Bank m_Bank = null;
        private string m_CharName = string.Empty;
        private BankRace m_Race = BankRace.None;

        protected override bool _DoProces()
        {
            WorkSession.WriteStatus("BankReadCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("BankReadCommand.DoProcess() : [" + this.m_Account + "] 를 캐쉬에서 읽도록 시도합니다.");
            BankCache cache = (BankCache) ObjectCache.Bank.Extract(this.m_Account);
            if (cache == null)
            {
                cache = new BankCache();
            }
            this.m_Bank = QueryManager.Bank.Read(this.m_Account, this.m_CharName, this.m_Race, cache);
            if (this.m_Bank != null)
            {
                WorkSession.WriteStatus("BankReadCommand.DoProcess() : [" + this.m_Account + "] 의 정보를 데이터베이스에서 읽었습니다");
                ObjectCache.Bank.Push(this.m_Account, cache);
            }
            else
            {
                WorkSession.WriteStatus("BankReadCommand.DoProcess() : [" + this.m_Account + "] 의 정보를 데이터베이스에 쿼리하는데 실패하였습니다");
            }
            return (this.m_Bank != null);
        }

        protected override void _ReceiveData(Message _Msg)
        {
            this.m_Account = _Msg.ReadString();
            this.m_CharName = _Msg.ReadString();
            this.m_Race = (BankRace) _Msg.ReadU8();
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("BankReadCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_Bank != null)
            {
                message.WriteU8(1);
                BankSerializer.Deserialize(this.m_Bank, this.m_Race, message);
                return message;
            }
            message.WriteU8(0);
            return message;
        }

        public override void OnSerialize(IObjLockRegistHelper _helper, bool bBegin)
        {
            _helper.StringIDRegistant(this.m_Account);
        }
    }
}

