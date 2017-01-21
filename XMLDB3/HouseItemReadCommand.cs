namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class HouseItemReadCommand : BasicCommand
    {
        private string m_Account = string.Empty;
        private HouseInventory m_HouseInventory = null;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("HouseItemReadCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("HouseItemReadCommand.DoProcess() : 집 아이템을 읽습니다.");
            this.m_HouseInventory = QueryManager.House.ReadItem(this.m_Account);
            if (this.m_HouseInventory != null)
            {
                WorkSession.WriteStatus("HouseItemReadCommand.DoProcess() : 집 아이템을 읽었습니다.");
            }
            else
            {
                WorkSession.WriteStatus("HouseItemReadCommand.DoProcess() : 집 아이템을 읽는데 실패하였습니다.");
            }
            return (this.m_HouseInventory != null);
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("HouseItemReadCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_HouseInventory != null)
            {
                message.WriteU8(1);
                HouseInventorySerializer.Deserialize(this.m_HouseInventory, message);
                return message;
            }
            message.WriteU8(0);
            return message;
        }

        protected override void ReceiveData(Message _Msg)
        {
            this.m_Account = _Msg.ReadString();
        }
    }
}

