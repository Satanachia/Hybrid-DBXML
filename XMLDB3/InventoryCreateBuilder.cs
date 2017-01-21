namespace XMLDB3
{
    using System;
    using System.Collections;
    using System.Text;

    public class InventoryCreateBuilder
    {
        public static string Build(long _id, Hashtable _inventory)
        {
            if (_inventory == null)
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder();
            foreach (Item item in _inventory.Values)
            {
                if ((item.storedtype > 0) && (item.storedtype < 6))
                {
                    builder.Append(ItemSqlBuilder.SelfUpdateItem(_id, item, true));
                }
            }
            return builder.ToString();
        }
    }
}

