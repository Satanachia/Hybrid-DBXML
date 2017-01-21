namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class MemoSendCommand : BasicCommand
    {
        private Memo m_Memo = null;
        private bool m_Result = false;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("MemoSendCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("MemoSendCommand.DoProcess() : 쪽지를 보냅니다");
            this.m_Result = QueryManager.Memo.SendMemo(this.m_Memo);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("MemoSendCommand.DoProcess() : 쪽지를 보냈습니다.");
            }
            else
            {
                WorkSession.WriteStatus("MemoSendCommand.DoProcess() : 쪽지를 보내는데 실패하였습니다.");
            }
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("MemoSendCommand.MakeMessage() : 함수에 진입하였습니다");
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
            this.m_Memo = MemoSerializer.Serialize(_Msg);
        }
    }
}

