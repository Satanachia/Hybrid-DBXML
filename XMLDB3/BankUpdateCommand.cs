namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class BankUpdateCommand : SerializedCommand
    {
        private Bank m_Bank = null;
        private string m_CharName = string.Empty;
        private bool m_Result = false;

        protected override bool _DoProces()
        {
            WorkSession.WriteStatus("BankUpdateCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("BankUpdateCommand.DoProcess() : [" + this.m_Bank.account + "] 의 데이터를 캐쉬에서 읽습니다");
            BankCache cache = (BankCache) ObjectCache.Bank.Extract(this.m_Bank.account);
            if (cache == null)
            {
                cache = new BankCache();
            }
            WorkSession.WriteStatus("BankUpdateCommand.DoProcess() : [" + this.m_Bank.account + "] 의 데이터를 저장합니다");
            this.m_Result = QueryManager.Bank.Write(this.m_CharName, this.m_Bank, cache);
            if (!this.m_Result)
            {
                WorkSession.WriteStatus("BankUpdateCommand.DoProcess() : [" + this.m_Bank.account + "] 의 데이터 저장에 실패하였습니다. 다시 시도합니다");
                cache = new BankCache();
                this.m_Result = QueryManager.Bank.Write(this.m_CharName, this.m_Bank, cache);
            }
            if (this.m_Result)
            {
                WorkSession.WriteStatus("BankUpdateCommand.DoProcess() : [" + this.m_Bank.account + "] 의 데이터를 업데이트 하였습니다");
                ObjectCache.Bank.Push(this.m_Bank.account, cache);
            }
            else
            {
                WorkSession.WriteStatus("BankUpdateCommand.DoProcess() : [" + this.m_Bank.account + "] 의 데이터 저장에 실패하였습니다");
                ExceptionMonitor.ExceptionRaised(new Exception("[" + this.m_Bank.account + "] 의 데이터 저장에 실패하였습니다"));
            }
            return this.m_Result;
        }

        protected override void _ReceiveData(Message _Msg)
        {
            this.m_CharName = _Msg.ReadString();
            this.m_Bank = BankSerializer.Serialize(_Msg);
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("BankUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
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

        public override void OnSerialize(IObjLockRegistHelper _helper, bool bBegin)
        {
            _helper.StringIDRegistant(this.m_Bank.account);
            if (this.m_Bank.slot != null)
            {
                foreach (BankSlot slot in this.m_Bank.slot)
                {
                    if (slot.item != null)
                    {
                        foreach (BankItem item in slot.item)
                        {
                            _helper.ObjectIDRegistant(item.item.id);
                        }
                    }
                }
            }
        }
    }
}

