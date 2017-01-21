namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class FarmReadCommand : BasicCommand
    {
        private Farm m_Farm = null;
        private long m_FarmId = 0L;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("FarmReadCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("FarmReadCommand.DoProcess() : 농장을 읽어옵니다.");
            this.m_Farm = QueryManager.Farm.Read(this.m_FarmId);
            if (this.m_Farm != null)
            {
                WorkSession.WriteStatus("FarmReadCommand.DoProcess() :  농장을 읽어왔습니다");
                return true;
            }
            WorkSession.WriteStatus("FarmReadCommand.DoProcess() : 농장을 얻는데 실패하였습니다");
            return false;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("FarmReadCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_Farm != null)
            {
                message.WriteU8(1);
                FarmSerializer.Deserialize(this.m_Farm, message);
                return message;
            }
            message.WriteU8(0);
            return message;
        }

        protected override void ReceiveData(Message _Msg)
        {
            this.m_FarmId = _Msg.ReadS64();
        }

        public override bool IsPrimeCommand
        {
            get
            {
                return true;
            }
        }
    }
}

