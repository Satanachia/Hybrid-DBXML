namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class ShopAdvertiseItemPriceUpdateCommand : BasicCommand
    {
        private string m_Account = string.Empty;
        private long m_ItemID = 0L;
        private int m_Price = 0;
        private bool m_Result = false;
        private string m_Server = string.Empty;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("ShopAdvertiseItemPriceUpdateCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("ShopAdvertiseItemPriceUpdateCommand.DoProcess() : 상점 광고에 아이템 가격을 업데이트합니다.");
            this.m_Result = QueryManager.ShopAdvertise.SetItemPrice(this.m_Account, this.m_Server, this.m_ItemID, this.m_Price);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("ShopAdvertiseItemPriceUpdateCommand.DoProcess() : 상점 광고에 아이템 가격을 업데이트하는데 성공하였습니다.");
            }
            else
            {
                WorkSession.WriteStatus("ShopAdvertiseItemPriceUpdateCommand.DoProcess() : 점 광고에 아이템 가격을 업데이트하는데 실패하였습니다");
            }
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("ShopAdvertiseItemPriceUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
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
            this.m_ItemID = _message.ReadS64();
            this.m_Price = _message.ReadS32();
        }
    }
}

