namespace XMLDB3
{
    using System;
    using System.Collections;

    public class LinkedHybridCache
    {
        private Hashtable dictionary;
        private Hashtable section;

        public LinkedHybridCache()
        {
            this.section = new Hashtable();
            this.dictionary = new Hashtable();
        }

        public LinkedHybridCache(int _sectionSize, int _itemSize)
        {
            if (_sectionSize != 0)
            {
                this.section = new Hashtable(_sectionSize);
            }
            else
            {
                this.section = new Hashtable();
            }
            if (_itemSize != 0)
            {
                this.dictionary = new Hashtable(_itemSize);
            }
            else
            {
                this.dictionary = new Hashtable();
            }
        }

        public ICacheItem AddItem(object _section, object _key, object _value)
        {
            LinkHeader header = (LinkHeader) this.section[_section];
            if (header == null)
            {
                throw new Exception(_section.ToString() + "에 해당하는 섹션이 없습니다.");
            }
            LinkItem item = new LinkItem(_value);
            this.dictionary.Add(_key, item);
            header.InsertItem(item);
            return item;
        }

        public ISection AddSection(object _section, object _data)
        {
            LinkHeader header = new LinkHeader(_data);
            this.section.Add(_section, header);
            return header;
        }

        public ILinkItem FindItem(object _key)
        {
            return (ILinkItem) this.dictionary[_key];
        }

        public ISection FindSection(object _section)
        {
            return (ISection) this.section[_section];
        }

        public Hashtable GetItems()
        {
            return (Hashtable) this.dictionary.Clone();
        }

        public ICollection GetSection()
        {
            return this.section.Values;
        }

        public void MoveSection(object _section, ILinkItem _item)
        {
            LinkItem item = (LinkItem) _item;
            LinkHeader header = (LinkHeader) this.section[_section];
            if (header == null)
            {
                throw new Exception(_section.ToString() + "에 해당하는 섹션이 없습니다.");
            }
            item.header.RemoveItem(item);
            header.InsertItem(item);
        }

        public void RemoveItem(object _key)
        {
            LinkItem item = (LinkItem) this.dictionary[_key];
            if (item != null)
            {
                item.header.RemoveItem(item);
                this.dictionary.Remove(_key);
            }
        }

        public void RemoveSection(object _section, IKeyFinder _keyFinder)
        {
            LinkHeader header = (LinkHeader) this.section[_section];
            LinkItem first = header.first;
            for (int i = 0; (i < header.Count) && (first != null); i++)
            {
                object key = _keyFinder.GetKey(first.Context);
                this.dictionary.Remove(key);
            }
            this.section.Remove(_section);
        }

        private class LinkHeader : ISection, ICacheItem
        {
            private object context;
            private int count = 0;
            public LinkedHybridCache.LinkItem first = null;

            public LinkHeader(object _context)
            {
                this.context = _context;
            }

            public void InsertItem(LinkedHybridCache.LinkItem _item)
            {
                _item.header = this;
                if (this.first == null)
                {
                    this.first = _item;
                    _item.next = null;
                    _item.prev = null;
                }
                else
                {
                    this.first.prev = _item;
                    _item.prev = null;
                    _item.next = this.first;
                    this.first = _item;
                }
                this.count++;
            }

            public void RemoveItem(LinkedHybridCache.LinkItem _item)
            {
                if (this.first == _item)
                {
                    this.first = _item.next;
                    if (this.first != null)
                    {
                        this.first.prev = null;
                    }
                }
                else
                {
                    _item.prev.next = _item.next;
                    if (_item.next != null)
                    {
                        _item.next.prev = _item.prev;
                    }
                }
                _item.next = null;
                _item.prev = null;
                _item.header = null;
                this.count--;
            }

            public object[] ToArray()
            {
                if (this.count <= 0)
                {
                    return null;
                }
                object[] objArray = new object[this.count];
                LinkedHybridCache.LinkItem first = this.first;
                for (int i = 0; (i < this.count) && (first != null); i++)
                {
                    objArray[i] = first.Context;
                    first = first.next;
                }
                return objArray;
            }

            public Array ToArray(Type type)
            {
                if (this.count <= 0)
                {
                    return null;
                }
                Array array = Array.CreateInstance(type, this.count);
                LinkedHybridCache.LinkItem first = this.first;
                for (int i = 0; (i < this.count) && (first != null); i++)
                {
                    array.SetValue(first.Context, i);
                    first = first.next;
                }
                return array;
            }

            public object Context
            {
                get
                {
                    return this.context;
                }
                set
                {
                    this.context = value;
                }
            }

            public int Count
            {
                get
                {
                    return this.count;
                }
            }

            public ILinkItem First
            {
                get
                {
                    return this.first;
                }
            }
        }

        private class LinkItem : ILinkItem, ICacheItem
        {
            private object context;
            public LinkedHybridCache.LinkHeader header;
            public LinkedHybridCache.LinkItem next;
            public LinkedHybridCache.LinkItem prev;

            public LinkItem(object _context)
            {
                this.context = _context;
                this.next = null;
            }

            public object Context
            {
                get
                {
                    return this.context;
                }
                set
                {
                    this.context = value;
                }
            }

            public ILinkItem Next
            {
                get
                {
                    return this.next;
                }
            }

            public ILinkItem Prev
            {
                get
                {
                    return this.prev;
                }
            }

            public ISection Section
            {
                get
                {
                    return this.header;
                }
            }
        }
    }
}

