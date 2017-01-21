namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class RemoteFunction_ExceptionInfo : RemoteFunction
    {
        protected override string BuildName(Message _input)
        {
            return "Exception Information";
        }

        public override Message Process()
        {
            int num = base.Input.ReadS32();
            int num2 = base.Input.ReadS32();
            Message newReply = base.GetNewReply();
            WorkSession.WriteStatus("RemoteFunction_ExceptionInfo.Process() : 처리된 예외 정보를 얻습니다");
            newReply += ExceptionMonitor.ToMessage(num, num2);
            WorkSession.WriteStatus("RemoteFunction_ExceptionInfo.Process() : 처리된 예외 정보 요청을 완료했습니다 ");
            base.Reply(newReply);
            return newReply;
        }
    }
}

