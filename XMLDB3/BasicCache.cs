namespace XMLDB3
{
    using System;
    using System.Collections;

    public class BasicCache
    {
        private const int DefaultTableInitSize = 0xc350;
        private bool m_AutoElevation;
        private int m_ElevationCutline;
        protected string m_Name;
        private Hashtable m_Objects;
        private Hashtable m_ObjectsLv2;
        private CacheStatistics m_statistics;
        private int m_TableInitSize;

        public BasicCache() : this(0xc350, "DefaultCache")
        {
            this.m_Name = "DefaultCache";
        }

        public BasicCache(int _size, string _name)
        {
            this.m_AutoElevation = true;
            this.m_TableInitSize = _size;
            this.m_ElevationCutline = _size / 2;
            this.m_Name = _name;
            this.m_Objects = new Hashtable(_size);
            this.m_ObjectsLv2 = new Hashtable(0);
            this.m_statistics = new CacheStatistics(this.m_Name);
        }

        public void Elevation()
        {
            this.m_ObjectsLv2 = this.m_Objects;
            this.m_Objects = new Hashtable(this.m_TableInitSize);
            WorkSession.WriteStatus("BasicCache.Elevation() : 캐쉬 에레베이션이 일어났습니다");
        }

        public object Extract(object _Key)
        {
            lock (this)
            {
                object obj2 = this.m_Objects[_Key];
                if (obj2 == null)
                {
                    obj2 = this.m_ObjectsLv2[_Key];
                    if (obj2 != null)
                    {
                        this.m_ObjectsLv2.Remove(_Key);
                        this.m_statistics.CacheHit();
                    }
                    else
                    {
                        this.m_statistics.CacheMiss();
                    }
                }
                else
                {
                    this.m_Objects.Remove(_Key);
                    this.m_statistics.CacheHit();
                }
                return obj2;
            }
        }

        public object Find(object _Key)
        {
            lock (this)
            {
                object obj2 = this.m_Objects[_Key];
                if (obj2 == null)
                {
                    obj2 = this.m_ObjectsLv2[_Key];
                }
                return obj2;
            }
        }

        public void Pop(object _Key)
        {
            lock (this)
            {
                this.m_Objects.Remove(_Key);
                this.m_ObjectsLv2.Remove(_Key);
            }
        }

        public bool Push(object _Id, object _Data)
        {
            if (_Data == null)
            {
                return false;
            }
            lock (this)
            {
                if (this.m_AutoElevation && (this.m_Objects.Count >= this.m_ElevationCutline))
                {
                    this.Elevation();
                }
                this.m_Objects[_Id] = _Data;
                return true;
            }
        }

        public bool AutoElevation
        {
            get
            {
                return this.m_AutoElevation;
            }
            set
            {
                this.m_AutoElevation = value;
            }
        }

        public CacheStatistics Statistics
        {
            get
            {
                this.m_statistics.Size = this.m_Objects.Count + this.m_ObjectsLv2.Count;
                return this.m_statistics;
            }
        }
    }
}

