namespace XMLDB3.ItemMarket
{
    using System;

    public enum IMMessage : byte
    {
        CheckBalance = 0x12,
        CheckEnterance = 0x11,
        GetItem = 0x81,
        GetItemCommit = 130,
        GetItemRollback = 0x83,
        Heartbeat = 0xff,
        Initialize = 1,
        InquiryMyPage = 0x24,
        InquirySaleItem = 0x21,
        InquiryStorage = 0x22,
        ItemList = 0x31,
        ItemSearch = 50,
        None = 0,
        Purchase = 0x44,
        SaleCancel = 0x43,
        SaleRequest = 0x41,
        SaleRequestCommit = 0x42,
        SaleRequestRollback = 0x45
    }
}

