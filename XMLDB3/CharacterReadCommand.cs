namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class CharacterReadCommand : SerializedCommand
    {
        private long m_Id;
        private CharacterInfo m_ReadCharacter = null;

        protected override bool _DoProces()
        {
            WorkSession.WriteStatus("CharacterReadCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("CharacterReadCommand.DoProcess() : [" + this.m_Id + "] 를 캐쉬에서 읽기를 시도합니다");
            CharacterInfo info = (CharacterInfo) ObjectCache.Character.Extract(this.m_Id);
            this.m_ReadCharacter = QueryManager.Character.Read(this.m_Id, info);
            if (this.m_ReadCharacter != null)
            {
                WorkSession.WriteStatus("CharacterReadCommand.DoProcess() : [" + this.m_Id + "] 를 데이터베이스에서 읽었습니다");
                ObjectCache.Character.Push(this.m_Id, this.m_ReadCharacter);
            }
            else
            {
                WorkSession.WriteStatus("CharacterReadCommand.DoProcess() : [" + this.m_Id + "] 를 데이터베이스에 쿼리하는데 실패하였습니다");
            }
            return (this.m_ReadCharacter != null);
        }

        protected override void _ReceiveData(Message _Msg)
        {
            this.m_Id = (long) _Msg.ReadU64();
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("CharacterReadCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_ReadCharacter != null)
            {
                message.WriteU8(1);
                CharacterSerializer.Deserialize(this.m_ReadCharacter, message);
                return message;
            }
            message.WriteU8(0);
            return message;
        }

        public override void OnSerialize(IObjLockRegistHelper _helper, bool bBegin)
        {
            _helper.ObjectIDRegistant(this.m_Id);
        }
    }
}

