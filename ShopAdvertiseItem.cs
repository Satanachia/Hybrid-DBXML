using System;
using System.Xml.Serialization;

public class ShopAdvertiseItem : ShopAdvertiseItembase
{
    [XmlIgnore]
    public Item.StoredType Type
    {
        get
        {
            return (Item.StoredType) base.storedtype;
        }
        set
        {
            base.storedtype = (byte) value;
        }
    }
}

