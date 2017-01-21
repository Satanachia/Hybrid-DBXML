namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class PropReadCommand : BasicCommand
    {
        private long m_Id = 0L;
        private Prop m_Prop = null;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("PropReadCommand.DoProcess() : 함수에 진입하였습니다");
            this.m_Prop = QueryManager.Prop.Read(this.m_Id);
            if (this.m_Prop == null)
            {
                return false;
            }
            return true;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("PropReadCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_Prop != null)
            {
                message.WriteU8(1);
                return PropSerializer.Deserialize(this.m_Prop, message);
            }
            message.WriteU8(0);
            return message;
        }

        protected override void ReceiveData(Message _Msg)
        {
            this.m_Id = (long) _Msg.ReadU64();
        }
    }
}

