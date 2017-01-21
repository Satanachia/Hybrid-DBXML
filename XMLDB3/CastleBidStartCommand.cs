namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class CastleBidStartCommand : BasicCommand
    {
        private CastleBid m_CastleBid = null;
        private bool m_Result = false;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("CastleBidStartCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("CastleBidStartCommand.DoProcess() : 성 경매를 시작.");
            this.m_Result = QueryManager.Castle.CreateBid(this.m_CastleBid);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("CastleBidStartCommand.DoProcess() : 성 경매를 시작하였습니다..");
                return true;
            }
            WorkSession.WriteStatus("CastleBidStartCommand.DoProcess() : 성 경매를 시작하는데 실패하였습니다.");
            return false;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("CastleBidStartCommand.MakeMessage() : 함수에 진입하였습니다");
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

        protected override void ReceiveData(Message _Msg)
        {
            this.m_CastleBid = CastleBidSerializer.Serialize(_Msg);
        }
    }
}

