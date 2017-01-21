namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class QuestSerializer
    {
        public static void Deserialize(Quest _quest, Message _Msg)
        {
            if (_quest == null)
            {
                _Msg.WriteS32(0);
            }
            else
            {
                _Msg.WriteS32(1);
                _Msg.WriteS64(_quest.id);
                _Msg.WriteS32(_quest.templateid);
                _Msg.WriteU8(_quest.complete);
                _Msg.WriteS64(_quest.start_time);
                _Msg.WriteString(_quest.data);
                if (_quest.objectives != null)
                {
                    int length = _quest.objectives.Length;
                    _Msg.WriteS32(length);
                    foreach (QuestObjective objective in _quest.objectives)
                    {
                        WriteQuestObjectiveToMessage(objective, _Msg);
                    }
                }
                else
                {
                    _Msg.WriteS32(0);
                }
            }
        }

        private static QuestObjective ReadQuestObjectiveFromMessage(Message _Msg)
        {
            QuestObjective objective = new QuestObjective();
            objective.complete = _Msg.ReadU8();
            objective.active = _Msg.ReadU8();
            objective.data = _Msg.ReadString();
            return objective;
        }

        public static Quest Serialize(Message _Msg)
        {
            Quest quest = new Quest();
            if (_Msg.ReadS32() == 0)
            {
                return null;
            }
            quest.id = _Msg.ReadS64();
            quest.templateid = _Msg.ReadS32();
            quest.complete = _Msg.ReadU8();
            quest.start_time = _Msg.ReadS64();
            quest.data = _Msg.ReadString();
            int num = _Msg.ReadS32();
            if (num > 0)
            {
                quest.objectives = new QuestObjective[num];
                for (int i = 0; i < num; i++)
                {
                    quest.objectives[i] = ReadQuestObjectiveFromMessage(_Msg);
                }
                return quest;
            }
            quest.objectives = null;
            return quest;
        }

        private static void WriteQuestObjectiveToMessage(QuestObjective _questObj, Message _Msg)
        {
            if (_questObj == null)
            {
                _questObj = new QuestObjective();
                _questObj.data = string.Empty;
            }
            _Msg.WriteU8(_questObj.complete);
            _Msg.WriteU8(_questObj.active);
            _Msg.WriteString(_questObj.data);
        }
    }
}

