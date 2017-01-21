namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class BankUpdateExCommand : SerializedCommand
    {
        private string desc;
        private Bank m_Bank;
        private CharacterInfo m_Character;
        private bool m_Result = false;

        protected override bool _DoProces()
        {
            WorkSession.WriteStatus("BankUpdateExCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("BankUpdateExCommand.DoProcess() : [" + this.m_Bank.account + "] 의 데이터를 캐쉬에서 읽습니다");
            BankCache cache = (BankCache) ObjectCache.Bank.Extract(this.m_Bank.account);
            if (cache == null)
            {
                cache = new BankCache();
            }
            WorkSession.WriteStatus("BankUpdateExCommand.DoProcess() : [" + this.desc + "] 의 데이터를 캐쉬에서 읽습니다");
            CharacterInfo info = (CharacterInfo) ObjectCache.Character.Extract(this.m_Character.id);
            WorkSession.WriteStatus("BankUpdateExCommand.DoProcess() : [" + this.m_Bank.account + "] 의 데이터를 저장합니다");
            this.m_Result = QueryManager.Bank.WriteEx(this.m_Bank, this.m_Character, cache, info, QueryManager.Character);
            if (!this.m_Result)
            {
                WorkSession.WriteStatus("BankUpdateExCommand.DoProcess() : [" + this.m_Bank.account + "] 의 데이터 저장에 실패하였습니다. 다시 시도합니다");
                cache = new BankCache();
                this.m_Result = QueryManager.Bank.WriteEx(this.m_Bank, this.m_Character, cache, null, QueryManager.Character);
            }
            if (this.m_Result)
            {
                WorkSession.WriteStatus("BankUpdateExCommand.DoProcess() : [" + this.m_Bank.account + "] 의 데이터를 업데이트 하였습니다");
                ObjectCache.Bank.Push(this.m_Bank.account, cache);
                ObjectCache.Character.Push(this.m_Character.id, this.m_Character);
            }
            else
            {
                WorkSession.WriteStatus("BankUpdateExCommand.DoProcess() : [" + this.m_Bank.account + "] 의 데이터 저장에 실패하였습니다");
                ExceptionMonitor.ExceptionRaised(new Exception("[" + this.m_Bank.account + "] 의 데이터 저장에 실패하였습니다"));
            }
            return this.m_Result;
        }

        protected override void _ReceiveData(Message _Msg)
        {
            this.m_Character = CharacterSerializer.Serialize(_Msg);
            this.m_Bank = BankSerializer.Serialize(_Msg);
            this.desc = this.m_Character.id + "/" + this.m_Character.name;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("BankUpdateExCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            message.WriteU8(1);
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
            _helper.ObjectIDRegistant(this.m_Character.id);
            if (this.m_Character.inventory != null)
            {
                foreach (Item item2 in this.m_Character.inventory.Values)
                {
                    _helper.ObjectIDRegistant(item2.id);
                }
            }
        }
    }
}

