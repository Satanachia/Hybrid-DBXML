namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class RuinUpdateCommand : BasicCommand
    {
        private bool m_Result = false;
        private RuinList m_RuinList = null;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("RuinUpdateCommand.DoProcess() : 함수에 진입하였습니다");
            this.m_Result = QueryManager.Ruin.Write(this.m_RuinList, RuinType.rtRuin);
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("RuinUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
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
            this.m_RuinList = RuinListSerializer.Serialize(_Msg);
        }
    }
}

