namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class PlayinCommand : BasicCommand
    {
        private string m_Account;
        private bool m_bResult = false;
        private int m_RemainTime;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("PlayinCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("PlayinCommand.DoProcess() : [" + this.m_Account + "] 가 게임접속을 기록합니다");
            this.m_bResult = QueryManager.Accountref.PlayIn(this.m_Account, this.m_RemainTime);
            if (this.m_bResult)
            {
                WorkSession.WriteStatus("PlayinCommand.DoProcess() : [" + this.m_Account + "] 가 게임접속을 기록합니다");
                return true;
            }
            WorkSession.WriteStatus("PlayinCommand.DoProcess() : [" + this.m_Account + "] 가 게임접속을 기록하지 못하였습니다");
            return false;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("PlayinCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_bResult)
            {
                message.WriteU8(1);
                return message;
            }
            message.WriteU8(0);
            return message;
        }

        protected override void ReceiveData(Message _Msg)
        {
            this.m_Account = _Msg.ReadString();
            this.m_RemainTime = _Msg.ReadS32();
        }
    }
}

