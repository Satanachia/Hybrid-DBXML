namespace XMLDB3
{
    using System;

    internal enum BidState
    {
        Biddable = -1,
        HouseBidderExist = 1,
        HouseItemExist = 2,
        NotEnoughMoney = 0,
        Unknown = 3
    }
}

