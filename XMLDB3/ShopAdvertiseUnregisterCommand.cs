namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class ShopAdvertiseUnregisterCommand : BasicCommand
    {
        private string m_Account = string.Empty;
        private bool m_Result = false;
        private string m_Server = string.Empty;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("ShopAdvertiseUnregisterCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("ShopAdvertiseUnregisterCommand.DoProcess() : 상점 광고를 삭제합니다.");
            this.m_Result = QueryManager.ShopAdvertise.Unregister(this.m_Account, this.m_Server);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("ShopAdvertiseUnregisterCommand.DoProcess() : 상점 광고를 삭제하는데 성공하였습니다.");
            }
            else
            {
                WorkSession.WriteStatus("ShopAdvertiseUnregisterCommand.DoProcess() : 상점 광고를 삭제하는데 실패하였습니다");
            }
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("ShopAdvertiseUnregisterCommand.MakeMessage() : 함수에 진입하였습니다");
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
            this.m_Account = _message.ReadString();
            this.m_Server = _message.ReadString();
        }
    }
}

