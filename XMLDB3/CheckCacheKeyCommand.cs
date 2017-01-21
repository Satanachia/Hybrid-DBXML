namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class CheckCacheKeyCommand : BasicCommand
    {
        private string m_Account;
        private bool m_bResult = false;
        private int m_CacheKey;
        private int m_OldCacheKey;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("UpdateCacheKeyCommand.DoProcess() : 함수에 진입하였습니다");
            this.m_bResult = QueryManager.Accountref.CacheKeyCheck(this.m_Account, this.m_CacheKey, out this.m_OldCacheKey);
            if (this.m_bResult)
            {
                WorkSession.WriteStatus(string.Concat(new object[] { "UpdateCacheKeyCommand.DoProcess() : [", this.m_Account, ",", this.m_CacheKey, "] 캐쉬 키를 업데이트 하여습니다" }));
                return true;
            }
            WorkSession.WriteStatus(string.Concat(new object[] { "UpdateCacheKeyCommand.DoProcess() : [", this.m_Account, ",", this.m_CacheKey, "] 캐쉬 키를 업데이트 하지 못하였습니다" }));
            return false;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("UpdateCacheKeyCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_bResult)
            {
                message.WriteU8(1);
                message.WriteS32(this.m_OldCacheKey);
                return message;
            }
            message.WriteU8(0);
            return message;
        }

        protected override void ReceiveData(Message _Msg)
        {
            this.m_Account = _Msg.ReadString();
            this.m_CacheKey = _Msg.ReadS32();
        }
    }
}

