namespace XMLDB3
{
    using System;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    public class ItemXmlFieldHelper
    {
        private static XmlSerializer itemOptionSerializer = new XmlSerializer(typeof(ItemOptionContainer));
        private static string LATEST_ITEMOPTION_VER = "2";
        private static XmlSerializer questObjectiveSerializer = new XmlSerializer(typeof(QuestObjectiveContainer));

        private static ItemOption[] _BuildItemOption(string _options)
        {
            StringReader input = new StringReader("<ItemOptionContainer>" + _options + "</ItemOptionContainer>");
            XmlTextReader xmlReader = new XmlTextReader(input);
            ItemOptionContainer container = null;
            ItemOption[] options = null;
            try
            {
                xmlReader.MoveToContent();
                if (xmlReader.Read() && xmlReader.Name.ToLower().Equals("options"))
                {
                    string attribute = xmlReader.GetAttribute("ver");
                    if (((attribute != null) && attribute.Equals(LATEST_ITEMOPTION_VER)) && (xmlReader.Read() && xmlReader.Name.ToLower().Equals("option")))
                    {
                        return ItemOptionDeserializer(xmlReader.GetAttribute("data"));
                    }
                }
                if (!_options.Equals(""))
                {
                    input = new StringReader("<ItemOptionContainer>" + _options + "</ItemOptionContainer>");
                    xmlReader = new XmlTextReader(input);
                    container = (ItemOptionContainer) itemOptionSerializer.Deserialize(xmlReader);
                    if (container != null)
                    {
                        options = container.options;
                    }
                }
            }
            finally
            {
                if (xmlReader != null)
                {
                    xmlReader.Close();
                }
            }
            return options;
        }

        public static ItemOption[] BuildItemOption(string _options)
        {
            string str = _options;
            try
            {
                return _BuildItemOption(str);
            }
            catch (XmlException)
            {
                int length = str.LastIndexOf('$');
                if (length >= 0)
                {
                    str = str.Substring(0, length) + "$\" /></options>";
                }
            }
            return _BuildItemOption(str);
        }

        public static string BuildItemOptionXml(ItemOption[] _options)
        {
            if ((_options == null) || (_options.Length <= 0))
            {
                return string.Empty;
            }
            string str = "<options ver=\"" + LATEST_ITEMOPTION_VER + "\"><option data=\"";
            foreach (ItemOption option in _options)
            {
                object obj2 = str;
                str = string.Concat(new object[] { 
                    obj2, option.type, "|", option.flag, "|", option.execute, "|", option.execdata, "|", option.open, "|", option.opendata, "|", option.enable, "|", option.enabledata, 
                    "|$"
                 });
            }
            return (str + "\"/></options>");
        }

        public static QuestObjective[] BuildQuestObjective(string _objectives)
        {
            StringReader input = new StringReader("<QuestObjectiveContainer>" + _objectives + "</QuestObjectiveContainer>");
            XmlTextReader xmlReader = new XmlTextReader(input);
            QuestObjectiveContainer container = (QuestObjectiveContainer) questObjectiveSerializer.Deserialize(xmlReader);
            return container.objectives;
        }

        public static string BuildQuestObjectiveXml(QuestObjective[] _objectives)
        {
            if ((_objectives == null) || (_objectives.Length <= 0))
            {
                return string.Empty;
            }
            string str = "<objectives>";
            foreach (QuestObjective objective in _objectives)
            {
                object obj2 = str;
                str = string.Concat(new object[] { obj2, "<objective complete=\"", objective.complete, "\" active=\"", objective.active, "\" data=\"", objective.data, "\"/>" });
            }
            return (str + "</objectives>");
        }

        private static ItemOption[] ItemOptionDeserializer(string _optionsData)
        {
            string[] strArray = null;
            string[] strArray2 = null;
            strArray = _optionsData.Split(new char[] { '$' });
            int num = strArray.GetLength(0) - 1;
            ItemOption[] optionArray = null;
            optionArray = new ItemOption[num];
            for (int i = 0; i < num; i++)
            {
                optionArray[i] = new ItemOption();
                strArray2 = strArray[i].Split(new char[] { '|' });
                int num3 = strArray2.GetLength(0) - 1;
                if (num3 != 8)
                {
                    throw new Exception(string.Concat(new object[] { "Item option이 ", 8, "개가 아님 : (", num3, "개)" }));
                }
                for (int j = 0; j < 8; j++)
                {
                    optionArray[i].type = Convert.ToByte(strArray2[j].Trim());
                    optionArray[i].flag = Convert.ToInt32(strArray2[++j].Trim());
                    optionArray[i].execute = Convert.ToInt16(strArray2[++j].Trim());
                    optionArray[i].execdata = Convert.ToInt32(strArray2[++j].Trim());
                    optionArray[i].open = Convert.ToByte(strArray2[++j].Trim());
                    optionArray[i].opendata = Convert.ToInt32(strArray2[++j].Trim());
                    optionArray[i].enable = Convert.ToByte(strArray2[++j].Trim());
                    optionArray[i].enabledata = Convert.ToInt32(strArray2[++j].Trim());
                }
            }
            return optionArray;
        }

        public class ItemOptionContainer
        {
            [XmlArrayItem("option", IsNullable=false)]
            public ItemOption[] options;
        }

        public class QuestObjectiveContainer
        {
            [XmlArrayItem("objective", IsNullable=false)]
            public QuestObjective[] objectives;
        }
    }
}

