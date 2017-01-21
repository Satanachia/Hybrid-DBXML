namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class HouseBidderAutoRepayCommand : BasicCommand
    {
        private string m_Account = string.Empty;
        private long m_HouseID = 0L;
        private HouseInventory m_Inventory = null;
        private bool m_Result = false;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("HouseBidderAutoRepayCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("HouseBidderAutoRepayCommand.DoProcess() : 입찰자를 삭제합니다.");
            this.m_Result = QueryManager.House.AutoRepay(this.m_HouseID, this.m_Account, this.m_Inventory);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("HouseBidderAutoRepayCommand.DoProcess() : 입찰자를 삭제하였습니다.");
            }
            else
            {
                WorkSession.WriteStatus("HouseBidderAutoRepayCommand.DoProcess() : 입찰자를 삭제하는데 실패하였습니다.");
            }
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("HouseBidderAutoRepayCommand.MakeMessage() : 함수에 진입하였습니다");
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
            this.m_HouseID = _Msg.ReadS64();
            this.m_Account = _Msg.ReadString();
            this.m_Inventory = HouseInventorySerializer.Serialize(_Msg);
        }
    }
}

