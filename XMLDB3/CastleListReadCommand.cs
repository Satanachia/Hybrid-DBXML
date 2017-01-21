namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class CastleListReadCommand : BasicCommand
    {
        private CastleList m_CastleList = null;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("CastleListReadCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("CastleListReadCommand.DoProcess() : 성 전체 리스트를 얻어옵니다");
            this.m_CastleList = QueryManager.Castle.ReadList();
            if (this.m_CastleList != null)
            {
                WorkSession.WriteStatus("CastleListReadCommand.DoProcess() : 성 전체 리스트를 얻어왔습니다");
                return true;
            }
            WorkSession.WriteStatus("CastleListReadCommand.DoProcess() : 성 전체 리스트를 얻는데 실패하였습니다");
            return false;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("CastleListReadCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_CastleList != null)
            {
                message.WriteU8(1);
                CastleListSerializer.Deserialize(this.m_CastleList, message);
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

