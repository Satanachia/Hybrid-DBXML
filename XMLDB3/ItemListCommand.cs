namespace XMLDB3
{
    using Mabinogi;
    using System;
    using XMLDB3.ItemMarket;

    public class ItemListCommand : BasicCommand
    {
        private bool m_bReplyEnable = true;
        private string m_CharacterName = string.Empty;
        private int m_PageItemCount = 0;
        private int m_PageNo = 0;
        private bool m_SortingAsc = false;
        private IMSortingType m_SortingType = IMSortingType.ExpireDate;

        public override bool DoProcess()
        {
            try
            {
                ItemMarketCommand command = new IMItemListCommand(ConfigManager.ItemMarketServerNo, this.m_CharacterName, this.m_PageNo, this.m_PageItemCount, this.m_SortingType, this.m_SortingAsc);
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
            WorkSession.WriteStatus("ItemListCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            message.WriteU8(0);
            return message;
        }

        protected override void ReceiveData(Message _message)
        {
            this.m_CharacterName = _message.ReadString();
            this.m_PageNo = _message.ReadS32();
            this.m_PageItemCount = _message.ReadS32();
            int sortType = _message.ReadS32();
            this.m_SortingType = SortTypeHelper.GetSortingType(sortType);
            this.m_SortingAsc = SortTypeHelper.GetAscendingType(sortType);
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

