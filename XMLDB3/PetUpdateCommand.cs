namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class PetUpdateCommand : SerializedCommand
    {
        private string m_Account = string.Empty;
        private byte m_ChannelGroupId = 0;
        private long m_Id = 0L;
        private string m_Name = string.Empty;
        private bool m_Result = false;
        private string m_Server = string.Empty;
        private PetInfo m_WritePet = null;

        protected override bool _DoProces()
        {
            WorkSession.WriteStatus(string.Concat(new object[] { "PetUpdateCommand.DoProcess() : [", this.m_Id, "/", this.m_Name, "] 의 데이터를 캐쉬에서 읽습니다" }));
            PetInfo info = (PetInfo) ObjectCache.Character.Extract(this.m_Id);
            WorkSession.WriteStatus(string.Concat(new object[] { "PetUpdateCommand.DoProcess() : [", this.m_Id, "/", this.m_Name, "] 의 데이터를 업데이트합니다" }));
            this.m_Result = QueryManager.Pet.Write(this.m_Account, this.m_Server, this.m_ChannelGroupId, this.m_WritePet, info, QueryManager.Accountref);
            if (!this.m_Result)
            {
                WorkSession.WriteStatus(string.Concat(new object[] { "PetUpdateCommand.DoProcess() : [", this.m_Id, "/", this.m_Name, "] 의 데이터 저장에 실패하였습니다. 다시 시도합니다" }));
                this.m_Result = QueryManager.Pet.Write(this.m_Account, this.m_Server, this.m_ChannelGroupId, this.m_WritePet, null, QueryManager.Accountref);
            }
            if (this.m_Result)
            {
                WorkSession.WriteStatus(string.Concat(new object[] { "PetUpdateCommand.DoProcess() : [", this.m_Id, "/", this.m_Name, "] 의 데이터를 업데이트 하였습니다" }));
                ObjectCache.Character.Push(this.m_Id, this.m_WritePet);
            }
            else
            {
                WorkSession.WriteStatus(string.Concat(new object[] { "PetUpdateCommand.DoProcess() : [", this.m_Id, "/", this.m_Name, "] 의 데이터 저장에 실패하였습니다" }));
                ExceptionMonitor.ExceptionRaised(new Exception(string.Concat(new object[] { "[", this.m_Id, "/", this.m_Name, "] 의 데이터 저장에 실패하였습니다" })));
            }
            return this.m_Result;
        }

        protected override void _ReceiveData(Message _Msg)
        {
            this.m_Account = _Msg.ReadString();
            this.m_Server = _Msg.ReadString();
            this.m_ChannelGroupId = _Msg.ReadU8();
            this.m_WritePet = PetSerializer.Serialize(_Msg);
            this.m_Id = this.m_WritePet.id;
            this.m_Name = this.m_WritePet.name;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("PetUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_Result)
            {
                message.WriteU8(1);
                message.WriteU64((ulong) this.m_Id);
                return message;
            }
            message.WriteU8(0);
            return message;
        }

        public override void OnSerialize(IObjLockRegistHelper _helper, bool bBegin)
        {
            _helper.ObjectIDRegistant(this.m_Id);
            if (this.m_WritePet.inventory != null)
            {
                foreach (Item item in this.m_WritePet.inventory.Values)
                {
                    _helper.ObjectIDRegistant(item.id);
                }
            }
        }
    }
}

