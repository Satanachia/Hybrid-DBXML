namespace XMLDB3
{
    using Mabinogi;
    using System;
    using XMLDB3.ItemMarket;

    public class InquirySaleItemCommand : BasicCommand
    {
        private bool m_bReplyEnable = true;
        private string m_CharacterName = string.Empty;
        private int m_PageItemCount = 0;
        private int m_PageNo = 0;

        public override bool DoProcess()
        {
            try
            {
                ItemMarketCommand command = new IMInquirySaleItemCommand(ConfigManager.ItemMarketServerNo, this.m_CharacterName, this.m_PageNo, this.m_PageItemCount);
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
            WorkSession.WriteStatus("InquirySaleItemCommand.MakeMessage() : 함수에 진입하였습니다");
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

