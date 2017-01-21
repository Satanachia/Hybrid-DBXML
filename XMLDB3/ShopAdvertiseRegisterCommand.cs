namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class ShopAdvertiseRegisterCommand : BasicCommand
    {
        private ShopAdvertise m_Advertise = null;
        private bool m_Result = false;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("ShopAdvertiseRegisterCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("ShopAdvertiseRegisterCommand.DoProcess() : 상점 광고를 등록합니다.");
            this.m_Result = QueryManager.ShopAdvertise.Register(this.m_Advertise);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("ShopAdvertiseRegisterCommand.DoProcess() : 상점 광고를 등록하는데 성공하였습니다.");
            }
            else
            {
                WorkSession.WriteStatus("ShopAdvertiseRegisterCommand.DoProcess() : 상점 광고를 등록하는데 실패하였습니다");
            }
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("ShopAdvertiseRegisterCommand.MakeMessage() : 함수에 진입하였습니다");
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

        protected override void ReceiveData(Message _message)
        {
            this.m_Advertise = ShopAdvertiseSerializer.Serialize(_message);
        }
    }
}

