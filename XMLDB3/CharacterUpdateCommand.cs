namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class CharacterUpdateCommand : SerializedCommand
    {
        private string desc = string.Empty;
        private bool m_Result = false;
        private CharacterInfo m_WriteCharacter = null;

        protected override bool _DoProces()
        {
            WorkSession.WriteStatus("CharacterUpdateCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("CharacterUpdateCommand.DoProcess() : [" + this.desc + "] 의 데이터를 캐쉬에서 읽습니다");
            CharacterInfo info = (CharacterInfo) ObjectCache.Character.Extract(this.m_WriteCharacter.id);
            WorkSession.WriteStatus("CharacterUpdateCommand.DoProcess() : [" + this.desc + "] 의 데이터를 업데이트합니다");
            this.m_Result = QueryManager.Character.Write(this.m_WriteCharacter, info);
            if (!this.m_Result)
            {
                WorkSession.WriteStatus("CharacterUpdateCommand.DoProcess() : [" + this.desc + "] 의 데이터 저장에 실패하였습니다. 다시 시도합니다");
                this.m_Result = QueryManager.Character.Write(this.m_WriteCharacter, null);
            }
            if (this.m_Result)
            {
                WorkSession.WriteStatus("CharacterUpdateCommand.DoProcess() : [" + this.desc + "] 의 데이터를 업데이트 하였습니다");
                ObjectCache.Character.Push(this.m_WriteCharacter.id, this.m_WriteCharacter);
            }
            else
            {
                WorkSession.WriteStatus("CharacterUpdateCommand.DoProcess() : [" + this.desc + "] 의 데이터 저장에 실패하였습니다");
            }
            return this.m_Result;
        }

        protected override void _ReceiveData(Message _Msg)
        {
            this.m_WriteCharacter = new CharacterInfo();
            this.m_WriteCharacter = CharacterSerializer.Serialize(_Msg);
            this.desc = this.m_WriteCharacter.id + "/" + this.m_WriteCharacter.name;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("CharacterUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
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

        public override void OnSerialize(IObjLockRegistHelper _helper, bool bBegin)
        {
            _helper.ObjectIDRegistant(this.m_WriteCharacter.id);
            if (this.m_WriteCharacter.inventory != null)
            {
                foreach (Item item in this.m_WriteCharacter.inventory.Values)
                {
                    _helper.ObjectIDRegistant(item.id);
                }
            }
        }
    }
}

