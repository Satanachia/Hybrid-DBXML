namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class HouseItemUpdateCommand : BasicCommand
    {
        private string m_Account = string.Empty;
        private HouseItem m_Item = null;
        private bool m_Result = false;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("HouseItemUpdateCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("HouseItemUpdateCommand.DoProcess() : 집 아이템을 업데이트합니다.");
            this.m_Result = QueryManager.House.UpdateItem(this.m_Account, this.m_Item);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("HouseItemUpdateCommand.DoProcess() : 집 아이템을 업데이트하였습니다.");
            }
            else
            {
                WorkSession.WriteStatus("HouseItemUpdateCommand.DoProcess() : 집 아이템을 업데이트하는데 실패하였습니다.");
            }
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("HouseItemUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
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

        protected override void ReceiveData(Message _Msg)
        {
            this.m_Account = _Msg.ReadString();
            this.m_Item = HouseItemSerializer.Serialize(_Msg);
        }
    }
}

