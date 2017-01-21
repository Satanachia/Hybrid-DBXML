namespace XMLDB3
{
    using Mabinogi;
    using System;
    using XMLDB3.ItemMarket;

    public class SaleRequestCommand : BasicCommand
    {
        private bool m_bReplyEnable = true;
        private string m_CharacterName = string.Empty;
        private Item m_Item = null;
        private int m_ItemFee = 0;
        private string m_ItemName = string.Empty;
        private int m_ItemRegistFee = 0;
        private int m_Price = 0;
        private byte m_SalePeriod = 0;

        public override bool DoProcess()
        {
            try
            {
                ItemMarketCommand command = new IMSaleRequestCommand(ConfigManager.ItemMarketServerNo, this.m_CharacterName, this.m_Item, this.m_ItemName, this.m_Price, this.m_ItemFee, this.m_ItemRegistFee, this.m_SalePeriod);
                ItemMarketHandler handler = ItemMarketManager.GetHandler();
                if ((handler != null) && handler.Send(command, base.ID, base.QueryID, 0, base.Target))
                {
                    this.m_bReplyEnable = false;
                }
            }
            catch (Exception exception)
            {
                ExceptionMonitor.ExceptionRaised(exception);
                this.m_bReplyEnable = true;
            }
            return true;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("SaleRequestCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            message.WriteU8(0);
            return message;
        }

        protected override void ReceiveData(Message _message)
        {
            this.m_CharacterName = _message.ReadString();
            this.m_Item = ItemSerializer.Serialize(_message);
            this.m_ItemName = _message.ReadString();
            this.m_Price = _message.ReadS32();
            this.m_ItemFee = _message.ReadS32();
            this.m_ItemRegistFee = _message.ReadS32();
            this.m_SalePeriod = _message.ReadU8();
        }

        public override bool ReplyEnable
        {
            get
            {
                return this.m_bReplyEnable;
            }
        }
    }
}

