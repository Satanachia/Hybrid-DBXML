namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class ItemMarketInfoCommand : BasicCommand
    {
        public override bool DoProcess()
        {
            return ConfigManager.ItemMarketEnabled;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("ItemMarketInfoCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (ConfigManager.ItemMarketEnabled)
            {
                message.WriteU8(1);
                message.WriteU32((uint) ConfigManager.ItemMarketGameNo);
                message.WriteU32((uint) ConfigManager.ItemMarketServerNo);
                message.WriteString(ConfigManager.ItemMarketIP);
                message.WriteU16((ushort) ConfigManager.ItemMarketPort);
                return message;
            }
            message.WriteU8(0);
            return message;
        }

        protected override void ReceiveData(Message _Msg)
        {
        }
    }
}

