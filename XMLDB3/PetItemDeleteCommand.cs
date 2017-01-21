namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class PetItemDeleteCommand : SerializedCommand
    {
        private long m_ID;
        private ItemList[] m_ItemList = null;
        private bool m_Result = false;

        protected override bool _DoProces()
        {
            WorkSession.WriteStatus("PetItemDeleteCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("PetItemDeleteCommand.DoProcess() : [" + this.m_ID + "] 의 데이터를 캐쉬에서 읽습니다");
            PetInfo info = (PetInfo) ObjectCache.Character.Extract(this.m_ID);
            WorkSession.WriteStatus("PetItemDeleteCommand.DoProcess() : [" + this.m_ID + "] 의 아이템을 삭제합니다.");
            this.m_Result = QueryManager.Pet.DeleteItem(this.m_ID, this.m_ItemList);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("PetItemDeleteCommand.DoProcess() : [" + this.m_ID + "] 의 아이템을 삭제하였습니다");
                if (((info != null) && (info.inventory != null)) && ((this.m_ItemList != null) && (this.m_ItemList.Length > 0)))
                {
                    foreach (ItemList list in this.m_ItemList)
                    {
                        info.inventory.Remove(list.itemID);
                    }
                    ObjectCache.Character.Push(this.m_ID, info);
                }
            }
            else
            {
                WorkSession.WriteStatus("PetItemDeleteCommand.DoProcess() : [" + this.m_ID + "] 의 아이템을 삭제에 실패하였습니다");
            }
            return this.m_Result;
        }

        protected override void _ReceiveData(Message _Msg)
        {
            this.m_ID = _Msg.ReadS64();
            this.m_ItemList = ItemListSerializer.Serialize(_Msg);
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("PetItemDeleteCommand.MakeMessage() : 함수에 진입하였습니다");
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
            _helper.ObjectIDRegistant(this.m_ID);
            if (this.m_ItemList != null)
            {
                foreach (ItemList list in this.m_ItemList)
                {
                    _helper.ObjectIDRegistant(list.itemID);
                }
            }
        }

        public override bool ReplyEnable
        {
            get
            {
                return false;
            }
        }
    }
}

