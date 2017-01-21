namespace XMLDB3.ItemMarket
{
    using System;

    public class SortTypeHelper
    {
        public static bool GetAscendingType(int sortType)
        {
            return ((sortType % 2) == 1);
        }

        public static IMSortingType GetSortingType(int sortType)
        {
            switch (sortType)
            {
                case 1:
                case 2:
                    return IMSortingType.ItemName;

                case 3:
                case 4:
                    return IMSortingType.Price;

                case 5:
                case 6:
                    return IMSortingType.Saler;
            }
            return IMSortingType.ExpireDate;
        }
    }
}

