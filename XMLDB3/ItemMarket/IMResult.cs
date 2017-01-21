namespace XMLDB3.ItemMarket
{
    using System;

    public enum IMResult
    {
        Fail = 0,
        Maintenance = 0xff,
        Success = 1,
        SuccessWithNotify = 0x11
    }
}

