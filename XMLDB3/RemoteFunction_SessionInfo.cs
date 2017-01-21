namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class RemoteFunction_SessionInfo : RemoteFunction
    {
        protected override string BuildName(Message _input)
        {
            return "Session Information & Statistics";
        }

        public override Message Process()
        {
            Message newReply = base.GetNewReply();
            WorkSession.WriteStatus("RemoteFunction_SessionInfo.Process() : 세션 정보를 얻습니다");
            SessionStatistics[] statisticsArray = WorkSession.Statistics;
            WorkSession.WriteStatus("RemoteFunction_SessionInfo.Process() : 총 " + statisticsArray.Length + " 개의 세션이 있습니다");
            newReply.WriteS32(statisticsArray.Length);
            foreach (SessionStatistics statistics in statisticsArray)
            {
                WorkSession.WriteStatus("RemoteFunction_SessionInfo.Process() : " + statistics.Name + "세션정보를 메시지에 적습니다");
                newReply += statistics.ToMessage();
            }
            WorkSession.WriteStatus("RemoteFunction_SessionInfo.Process() : 세션정보를 요청을 완료했습니다");
            base.Reply(newReply);
            return newReply;
        }
    }
}

