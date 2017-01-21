using System;
using System.Xml.Serialization;

public class Item : Itembase
{
    [XmlIgnore]
    public StoredType Type
    {
        get
        {
            return (StoredType) base.storedtype;
        }
        set
        {
            base.storedtype = (byte) value;
        }
    }

    public enum StoredType : byte
    {
        IstEgo = 5,
        IstHuge = 3,
        IstLarge = 1,
        IstMax = 6,
        IstMin = 0,
        IstQuest = 4,
        IstSmall = 2
    }
}

