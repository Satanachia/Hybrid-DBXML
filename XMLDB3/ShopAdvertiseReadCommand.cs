namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class ShopAdvertiseReadCommand : BasicCommand
    {
        private ShopAdvertiseList m_List = null;
        private string m_Server = string.Empty;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("ShopAdvertiseReadCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("ShopAdvertiseReadCommand.DoProcess() : 상점 광고를 읽습니다.");
            this.m_List = QueryManager.ShopAdvertise.Read(this.m_Server, QueryManager.House);
            if (this.m_List != null)
            {
                WorkSession.WriteStatus("ShopAdvertiseReadCommand.DoProcess() : 상점 광고를 읽는데 성공했습니다.");
                return true;
            }
            WorkSession.WriteStatus("ShopAdvertiseReadCommand.DoProcess() : 상점 광고를 읽는데 실패하였습니다");
            return false;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("ShopAdvertiseReadCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_List != null)
            {
                message.WriteU8(1);
                ShopAdvertiseListSerializer.Deserialize(this.m_List, message);
                return message;
            }
            message.WriteU8(0);
            return message;
        }

        protected override void ReceiveData(Message _message)
        {
            this.m_Server = _message.ReadString();
        }
    }
}

