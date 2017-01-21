namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class ShopAdvertiseItemAddCommand : BasicCommand
    {
        private string m_Account = string.Empty;
        private ShopAdvertiseItem m_Item = null;
        private bool m_Result = false;
        private string m_Server = string.Empty;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("ShopAdvertiseItemAddCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("ShopAdvertiseItemAddCommand.DoProcess() : 상점 광고에 아이템을 더합니다.");
            this.m_Result = QueryManager.ShopAdvertise.AddItem(this.m_Account, this.m_Server, this.m_Item);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("ShopAdvertiseItemAddCommand.DoProcess() : 상점 광고에 아이템을 더하는데 성공하였습니다.");
            }
            else
            {
                WorkSession.WriteStatus("ShopAdvertiseItemAddCommand.DoProcess() : 점 광고에 아이템을 더하는데 실패하였습니다");
            }
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("ShopAdvertiseItemAddCommand.MakeMessage() : 함수에 진입하였습니다");
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
            this.m_Item = ShopAdvertiseItemSerializer.Serialize(_message);
        }
    }
}

