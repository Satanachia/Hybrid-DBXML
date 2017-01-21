namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class PropListGetCommand : BasicCommand
    {
        private PropIDList m_PropList = null;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("PropListGetCommand.DoProcess() : 함수에 진입하였습니다");
            this.m_PropList = QueryManager.Prop.LoadPropList();
            if (this.m_PropList == null)
            {
                return false;
            }
            return true;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("PropListGetCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_PropList != null)
            {
                message.WriteU8(1);
                PropListSerializer.Deserialize(this.m_PropList, message);
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

