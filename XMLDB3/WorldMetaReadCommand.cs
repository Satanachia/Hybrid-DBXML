namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class WorldMetaReadCommand : BasicCommand
    {
        private WorldMetaList m_WorldMetaList = null;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("WorldMetaReadCommand.DoProcess() : 함수에 진입하였습니다");
            this.m_WorldMetaList = QueryManager.WorldMeta.Read();
            if (this.m_WorldMetaList != null)
            {
                WorkSession.WriteStatus("WorldMetaReadCommand.DoProcess() : 월드메타 데이터를 성공적으로 읽었습니다");
            }
            else
            {
                WorkSession.WriteStatus("WorldMetaReadCommand.DoProcess() : 월드메타 데이터를 읽는데 실패하였습니다.");
            }
            return (this.m_WorldMetaList != null);
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("WorldMetaReadCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_WorldMetaList != null)
            {
                message.WriteU8(1);
                WorldMetaListSerializer.Deserialize(this.m_WorldMetaList, message);
                return message;
            }
            message.WriteU8(0);
            return message;
        }

        protected override void ReceiveData(Message _message)
        {
        }

        public override bool IsPrimeCommand
        {
            get
            {
                return true;
            }
        }
    }
}

