namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class PetReadCommand : SerializedCommand
    {
        private string m_Account = string.Empty;
        private long m_Id;
        private PetInfo m_ReadPet = null;
        private string m_Server = string.Empty;

        protected override bool _DoProces()
        {
            WorkSession.WriteStatus("PetReadCommand.DoProcess() : [" + this.m_Id + "] 를  캐쉬에서 읽기를 시도합니다");
            PetInfo info = (PetInfo) ObjectCache.Character.Extract(this.m_Id);
            this.m_ReadPet = QueryManager.Pet.Read(this.m_Account, this.m_Server, this.m_Id, info, QueryManager.Accountref);
            if (this.m_ReadPet != null)
            {
                WorkSession.WriteStatus("PetReadCommand.DoProcess() : [" + this.m_Id + "] 를 데이터베이스에서 읽었습니다");
                ObjectCache.Character.Push(this.m_Id, this.m_ReadPet);
            }
            else
            {
                WorkSession.WriteStatus("PetReadCommand.DoProcess() : [" + this.m_Id + "] 를 데이터베이스에 쿼리하는데 실패하였습니다");
            }
            return (this.m_ReadPet != null);
        }

        protected override void _ReceiveData(Message _Msg)
        {
            this.m_Account = _Msg.ReadString();
            this.m_Server = _Msg.ReadString();
            this.m_Id = (long) _Msg.ReadU64();
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("PetReadCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_ReadPet != null)
            {
                message.WriteU8(1);
                PetSerializer.Deserialize(this.m_ReadPet, message);
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

