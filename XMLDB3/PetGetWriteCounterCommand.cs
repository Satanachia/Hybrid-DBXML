namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class PetGetWriteCounterCommand : SerializedCommand
    {
        private byte m_counter = 0;
        private long m_Id = 0L;
        private bool m_Result = false;

        protected override bool _DoProces()
        {
            WorkSession.WriteStatus("PetGetWriteCounterCommand.DoProcess() : 함수에 진입하였습니다");
            this.m_Result = QueryManager.Pet.GetWriteCounter(this.m_Id, out this.m_counter);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("PetGetWriteCounterCommand.DoProcess() : 저장 카운터를 읽었습니다.");
            }
            else
            {
                WorkSession.WriteStatus("PetGetWriteCounterCommand.DoProcess() : 저장 카운터를 읽는데 실패하였습니다.");
            }
            return this.m_Result;
        }

        protected override void _ReceiveData(Message _Msg)
        {
            this.m_Id = _Msg.ReadS64();
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("PetGetWriteCounterCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_Result)
            {
                message.WriteU8(1);
                message.WriteU8(this.m_counter);
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

