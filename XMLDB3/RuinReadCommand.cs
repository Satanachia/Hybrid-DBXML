namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class RuinReadCommand : BasicCommand
    {
        private RuinList m_RuinList = null;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("RuinReadCommand.DoProcess() : 함수에 진입하였습니다");
            this.m_RuinList = QueryManager.Ruin.Read(RuinType.rtRuin);
            if (this.m_RuinList == null)
            {
                return false;
            }
            return true;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("RuinReadCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_RuinList != null)
            {
                message.WriteU8(1);
                return RuinListSerializer.Deserialize(this.m_RuinList, message);
            }
            message.WriteU8(0);
            return message;
        }

        protected override void ReceiveData(Message _Msg)
        {
        }
    }
}

