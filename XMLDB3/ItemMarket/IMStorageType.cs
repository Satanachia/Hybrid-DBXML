namespace XMLDB3.ItemMarket
{
    using System;

    public enum IMStorageType
    {
        CompletedItem = 30,
        ExpiredItem = 0x1f,
        MovedItem = 0x15,
        MovingItem = 0x1d,
        WaitingItem = 20
    }
}

