namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class WorldMetaUpdateCommand : BasicCommand
    {
        private byte m_errorCode = 0;
        private string[] m_removeKeys = null;
        private REPLY_RESULT m_Result = REPLY_RESULT.FAIL;
        private WorldMetaList m_WorldMetaUpdateList = null;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("WorldMetaUpdateCommand.DoProcess() : 함수에 진입하였습니다");
            if ((((this.m_WorldMetaUpdateList == null) || (this.m_WorldMetaUpdateList.metas == null)) || (this.m_WorldMetaUpdateList.metas.Length == 0)) && ((this.m_removeKeys == null) || (this.m_removeKeys.Length == 0)))
            {
                WorkSession.WriteStatus("WorldMetaUpdateCommand.DoProcess() : 월드메타 업데이트할 항목이 없습니다.");
                this.m_Result = REPLY_RESULT.SUCCESS;
                return true;
            }
            this.m_Result = QueryManager.WorldMeta.UpdateList(this.m_WorldMetaUpdateList, this.m_removeKeys, ref this.m_errorCode);
            if (this.m_Result == REPLY_RESULT.SUCCESS)
            {
                WorkSession.WriteStatus("WorldMetaUpdateCommand.DoProcess() : 월드메타 데이터를 성공적으로 업데이트했습니다.");
            }
            else
            {
                WorkSession.WriteStatus("WorldMetaUpdateCommand.DoProcess() : 월드메타 데이터를 업데이트하는데 실패하였습니다.");
            }
            return (this.m_Result == REPLY_RESULT.SUCCESS);
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("WorldMetaUpdateListSerializer.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            message.WriteU8((byte) this.m_Result);
            if (this.m_Result == REPLY_RESULT.FAIL_EX)
            {
                message.WriteU8(this.m_errorCode);
            }
            return message;
        }

        protected override void ReceiveData(Message _message)
        {
            WorldMetaUpdateListSerializer.Serialize(_message, out this.m_WorldMetaUpdateList, out this.m_removeKeys);
        }
    }
}

