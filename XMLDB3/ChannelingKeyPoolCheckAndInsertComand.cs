namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class ChannelingKeyPoolCheckAndInsertComand : BasicCommand
    {
        private ChannelingKey m_chKey = null;
        private bool m_Result = false;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("ChannelingKeyPoolCheckAndInsertComand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("ChannelingKeyPoolCheckAndInsertComand.DoProcess() : 채널링 키풀 프로시저를 실행합니다");
            this.m_Result = QueryManager.ChannelingKeyPool.Do(this.m_chKey);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("ChannelingKeyPoolCheckAndInsertComand.DoProcess() : 채널링 키풀 프로시저 실행 성공");
            }
            else
            {
                WorkSession.WriteStatus("ChannelingKeyPoolCheckAndInsertComand.DoProcess() : 채널링 키풀 프로시저 실행에 실패하였습니다");
            }
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("ChannelingKeyPoolCheckAndInsertComand.MakeMessage() : 함수에 진입하였습니다");
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
            this.m_chKey = ChannelingKeyPoolSerializer.Serialize(_Msg);
        }
    }
}

