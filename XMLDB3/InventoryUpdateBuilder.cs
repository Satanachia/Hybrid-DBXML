namespace XMLDB3
{
    using System;
    using System.Collections;
    using System.Runtime.InteropServices;
    using System.Text;

    public class InventoryUpdateBuilder
    {
        public static string Build(long _id, Hashtable _new, Hashtable _cache, bool _forceUpdate)
        {
            string str = string.Empty;
            string str2 = string.Empty;
            Build(_id, _new, _cache, _forceUpdate, out str, out str2);
            return (str + str2);
        }

        public static void Build(long _id, Hashtable _new, Hashtable _cache, bool _forceUpdate, out string _deleteSql, out string _updateSql)
        {
            StringBuilder builder = new StringBuilder();
            StringBuilder builder2 = new StringBuilder();
            if (_new != null)
            {
                if (_cache != null)
                {
                    foreach (Item item in _new.Values)
                    {
                        Item item2 = (Item) _cache[item.id];
                        if (item2 == null)
                        {
                            builder.Append(ItemSqlBuilder.SelfUpdateItem(_id, item, _forceUpdate));
                        }
                        else
                        {
                            builder.Append(ItemSqlBuilder.UpdateItem(_id, item, item2));
                            _cache.Remove(item2.id);
                        }
                    }
                }
                else
                {
                    foreach (Item item3 in _new.Values)
                    {
                        builder.Append(ItemSqlBuilder.SelfUpdateItem(_id, item3, _forceUpdate));
                    }
                }
            }
            if (_cache != null)
            {
                foreach (Item item4 in _cache.Values)
                {
                    builder2.Append(ItemSqlBuilder.DeleteItem(_id, item4.id, item4.storedtype));
                }
            }
            _updateSql = builder.ToString();
            _deleteSql = builder2.ToString();
        }
    }
}

