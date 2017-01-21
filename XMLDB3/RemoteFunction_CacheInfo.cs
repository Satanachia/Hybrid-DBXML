namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class RemoteFunction_CacheInfo : RemoteFunction
    {
        protected override string BuildName(Message _input)
        {
            return "Cache Statistics";
        }

        public override Message Process()
        {
            Message newReply = base.GetNewReply();
            WorkSession.WriteStatus("RemoteFunction_CacheInfo.Process() : 서버의 캐쉬 정보를 얻습니다");
            newReply += ProcessManager.CacheStatisticsToMessage();
            WorkSession.WriteStatus("RemoteFunction_CacheInfo.Process() : 서버의 캐쉬 정보 요청을 완료했습니다");
            base.Reply(newReply);
            return newReply;
        }
    }
}

