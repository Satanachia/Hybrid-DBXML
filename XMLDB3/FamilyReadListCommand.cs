﻿namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class FamilyReadListCommand : BasicCommand
    {
        private FamilyList m_familyList = null;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("FamilyReadListCommand.DoProcess() : 함수에 진입하였습니다");
            this.m_familyList = QueryManager.Family.ReadList();
            if (this.m_familyList != null)
            {
                WorkSession.WriteStatus("FamilyReadListCommand.DoProcess() : 가문 데이터를 성공적으로 읽었습니다");
            }
            else
            {
                WorkSession.WriteStatus("FamilyReadListCommand.DoProcess() : 가문 데이터를 읽는데 실패하였습니다.");
            }
            return (this.m_familyList != null);
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("FamilyReadListCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_familyList != null)
            {
                message.WriteU8(1);
                FamilyListSerializer.Deserialize(this.m_familyList, message);
                return message;
            }
            message.WriteU8(0);
            return message;
        }

        protected override void ReceiveData(Message _message)
        {
        }
    }
}

