namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class AccountrefLobbyUpdateCommand : BasicCommand
    {
        private string m_Account;
        private bool m_bResult = false;
        private LobbyTabList m_CharLobbyTabList;
        private int m_LobbyOption;
        private LobbyTabList m_PetLobbyTabList;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("AccountrefLobbyUpdateCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("AccountrefLobbyUpdateCommand.DoProcess() : [" + this.m_Account + "] 가 로비설정을 기록합니다");
            this.m_bResult = QueryManager.Accountref.SetLobbyOption(this.m_Account, this.m_LobbyOption, this.m_CharLobbyTabList, this.m_PetLobbyTabList);
            if (this.m_bResult)
            {
                WorkSession.WriteStatus("AccountrefLobbyUpdateCommand.DoProcess() : [" + this.m_Account + "] 가 로비설정을 기록합니다");
                return true;
            }
            WorkSession.WriteStatus("AccountrefLobbyUpdateCommand.DoProcess() : [" + this.m_Account + "] 가 로비설정을 기록하지 못하였습니다");
            return false;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("AccountrefLobbyUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
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
            this.m_LobbyOption = _Msg.ReadS32();
            this.m_CharLobbyTabList = LobbyTabListSerializer.Serialize(_Msg);
            this.m_PetLobbyTabList = LobbyTabListSerializer.Serialize(_Msg);
        }
    }
}

