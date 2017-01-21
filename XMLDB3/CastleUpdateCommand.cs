namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class CastleUpdateCommand : SerializedCommand
    {
        private Castle m_Castle = null;
        private bool m_Result = false;

        protected override bool _DoProces()
        {
            WorkSession.WriteStatus("CastleUpdateCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("CastleUpdateCommand.DoProcess() : 성 정보를 업데이트 합니다.");
            this.m_Result = QueryManager.Castle.Write(this.m_Castle);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("CastleUpdateCommand.DoProcess() : 성 정보를 업데이트하였습니다..");
            }
            else
            {
                WorkSession.WriteStatus("CastleUpdateCommand.DoProcess() : 성 정보를 업데이트하는데 실패하였습니다.");
            }
            return this.m_Result;
        }

        protected override void _ReceiveData(Message _Msg)
        {
            this.m_Castle = CastleSerializer.Serialize(_Msg);
            this.m_Castle.build = null;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("CastleUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
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
            _helper.ObjectIDRegistant(this.m_Castle.castleID);
        }
    }
}

