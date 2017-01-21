using System;

public class ItemList
{
    public long itemID;
    public byte storedtype;

    public Item.StoredType Type
    {
        get
        {
            return (Item.StoredType) this.storedtype;
        }
        set
        {
            this.storedtype = (byte) value;
        }
    }
}

