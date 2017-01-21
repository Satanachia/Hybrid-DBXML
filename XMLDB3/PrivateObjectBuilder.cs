namespace XMLDB3
{
    using System;
    using System.Collections;
    using System.Data;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    public class PrivateObjectBuilder
    {
        private static XmlSerializer serializer = new XmlSerializer(typeof(CharacterPrivate));

        public static CharacterPrivate Build(DataRow _characterRow, DataTable _questTable)
        {
            StringReader input = new StringReader("<CharacterPrivate>" + ((string) _characterRow["reserved"]) + "<registereds />" + ((string) _characterRow["book"]) + "</CharacterPrivate>");
            XmlTextReader xmlReader = new XmlTextReader(input);
            CharacterPrivate @private = (CharacterPrivate) serializer.Deserialize(xmlReader);
            @private.npc_event_daycount = (int) _characterRow["npc_event_daycount"];
            @private.npc_event_bitflag = (long) _characterRow["npc_event_bitflag"];
            if (((_questTable != null) && (_questTable.Rows != null)) && (_questTable.Rows.Count > 0))
            {
                ArrayList list = new ArrayList(_questTable.Rows.Count);
                foreach (DataRow row in _questTable.Rows)
                {
                    CharacterPrivateRegistered registered = new CharacterPrivateRegistered();
                    registered.id = (int) row["questID"];
                    registered.start = (long) row["start"];
                    registered.end = (long) row["end"];
                    registered.extra = (int) row["extra"];
                    list.Add(registered);
                }
                @private.registereds = (CharacterPrivateRegistered[]) list.ToArray(typeof(CharacterPrivateRegistered));
            }
            return @private;
        }
    }
}

