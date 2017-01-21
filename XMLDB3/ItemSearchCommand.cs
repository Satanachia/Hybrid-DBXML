namespace XMLDB3
{
    using Mabinogi;
    using System;
    using XMLDB3.ItemMarket;

    public class ItemSearchCommand : BasicCommand
    {
        private bool m_bReplyEnable = true;
        private string m_CharacterName = string.Empty;
        private int m_ItemGroup = -1;
        private string m_ItemName = string.Empty;
        private int m_PageItemCount = 0;
        private int m_PageNo = 0;
        private bool m_SortingAsc = false;
        private IMSortingType m_SortingType = IMSortingType.ExpireDate;

        public override bool DoProcess()
        {
            try
            {
                ItemMarketCommand command = new IMItemSearchCommand(ConfigManager.ItemMarketServerNo, this.m_CharacterName, this.m_PageNo, this.m_PageItemCount, this.m_ItemName, this.m_SortingType, this.m_SortingAsc, this.m_ItemGroup);
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
            WorkSession.WriteStatus("ItemSearchCommand.MakeMessage() : 함수에 진입하였습니다");
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
            this.m_ItemName = _message.ReadString();
            int sortType = _message.ReadS32();
            this.m_SortingType = SortTypeHelper.GetSortingType(sortType);
            this.m_SortingAsc = SortTypeHelper.GetAscendingType(sortType);
            int num2 = _message.ReadS32();
            switch (num2)
            {
                case 0:
                    this.m_ItemGroup = -1;
                    return;

                case 10:
                    this.m_ItemGroup = 0;
                    return;
            }
            this.m_ItemGroup = num2;
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

