namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class HouseUpdateCommand : SerializedCommand
    {
        private House m_House = null;
        private bool m_Result = false;

        protected override bool _DoProces()
        {
            WorkSession.WriteStatus("HouseUpdateCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("HouseUpdateCommand.DoProcess() : 집 상태를 업데이트합니다.");
            this.m_Result = QueryManager.House.Write(this.m_House);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("HouseUpdateCommand.DoProcess() : 집 상태를 업데이트 하였습니다.");
            }
            else
            {
                WorkSession.WriteStatus("HouseUpdateCommand.DoProcess() : 집 상태를 업데이트 하는데 실패하였습니다");
            }
            return this.m_Result;
        }

        protected override void _ReceiveData(Message _Msg)
        {
            this.m_House = HouseAppearSerializer.Serialize(_Msg);
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("HouseUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
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
            _helper.ObjectIDRegistant(this.m_House.houseID);
        }
    }
}

