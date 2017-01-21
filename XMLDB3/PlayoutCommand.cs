namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class PlayoutCommand : BasicCommand
    {
        private string m_Account;
        private bool m_beginnerFlag = false;
        private bool m_bResult = false;
        private GroupIDList m_CharGroupID = null;
        private byte m_macroCheckFailure = 0;
        private byte m_macroCheckSuccess = 0;
        private GroupIDList m_PetGroupID = null;
        private int m_RemainTime;
        private string m_Server;
        private int m_supportLastChangeTime = 0;
        private byte m_supportRace = 0;
        private byte m_supportRewardState = 0;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("PlayoutCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("PlayoutCommand.DoProcess() : [" + this.m_Account + "] 가 게임접속종료를 기록합니다");
            this.m_bResult = QueryManager.Accountref.PlayOut(this.m_Account, this.m_RemainTime, this.m_Server, this.m_CharGroupID, this.m_PetGroupID, this.m_supportRace, this.m_supportRewardState, this.m_supportLastChangeTime, this.m_macroCheckFailure, this.m_macroCheckSuccess, this.m_beginnerFlag);
            if (this.m_bResult)
            {
                WorkSession.WriteStatus("PlayoutCommand.DoProcess() : [" + this.m_Account + "] 가 게임접속종료를 기록합니다");
                return true;
            }
            WorkSession.WriteStatus("PlayoutCommand.DoProcess() : [" + this.m_Account + "] 가 게임접속종료를 기록하지 못하였습니다");
            return false;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("PlayoutCommand.MakeMessage() : 함수에 진입하였습니다");
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

        protected override void ReceiveData(Message _message)
        {
            this.m_Account = _message.ReadString();
            this.m_RemainTime = _message.ReadS32();
            this.m_Server = _message.ReadString();
            this.m_supportLastChangeTime = _message.ReadS32();
            this.m_supportRace = _message.ReadU8();
            this.m_supportRewardState = _message.ReadU8();
            this.m_CharGroupID = GroupIDListSerializer.Serialize(_message);
            this.m_PetGroupID = GroupIDListSerializer.Serialize(_message);
            this.m_macroCheckFailure = _message.ReadU8();
            this.m_macroCheckSuccess = _message.ReadU8();
            this.m_beginnerFlag = _message.ReadU8() != 0;
        }
    }
}

