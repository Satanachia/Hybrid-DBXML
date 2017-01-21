namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class HouseItemDeleteCommand : SerializedCommand
    {
        private string m_Account = string.Empty;
        private long m_HouseID = 0L;
        private int m_HouseMoney = 0;
        private Item m_Item = null;
        private bool m_Result = false;

        protected override bool _DoProces()
        {
            WorkSession.WriteStatus("HouseItemDeleteCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("HouseItemDeleteCommand.DoProcess() : 집 아이템을 삭제합니다.");
            this.m_Result = QueryManager.House.DeleteItem(this.m_HouseID, this.m_Account, this.m_Item, this.m_HouseMoney);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("HouseItemDeleteCommand.DoProcess() : 집 아이템을 삭제하였습니다.");
            }
            else
            {
                WorkSession.WriteStatus("HouseItemDeleteCommand.DoProcess() : 집 아이템을 삭제하는데 실패하였습니다.");
            }
            return this.m_Result;
        }

        protected override void _ReceiveData(Message _Msg)
        {
            this.m_HouseID = _Msg.ReadS64();
            this.m_Account = _Msg.ReadString();
            this.m_HouseMoney = _Msg.ReadS32();
            this.m_Item = ItemSerializer.Serialize(_Msg);
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("HouseItemDeleteCommand.MakeMessage() : 함수에 진입하였습니다");
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

        public override void OnSerialize(IObjLockRegistHelper _helper, bool bBegin)
        {
            if (this.m_HouseID != 0L)
            {
                _helper.ObjectIDRegistant(this.m_HouseID);
            }
        }
    }
}

