namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class PropUpdateCommand : BasicCommand
    {
        private Prop m_Prop = null;
        private bool m_Result = false;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("PropUpdateCommand.DoProcess() : 함수에 진입하였습니다");
            this.m_Result = QueryManager.Prop.Write(this.m_Prop);
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("PropUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
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
            this.m_Prop = PropSerializer.Serialize(_Msg);
        }
    }
}

