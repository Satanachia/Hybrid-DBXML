namespace XMLDB3
{
    using Mabinogi;
    using System;
    using System.Threading;

    public class CacheStatistics
    {
        private int hit;
        private string name;
        private int size;
        private int total;

        public CacheStatistics(string _name)
        {
            this.name = string.Empty;
            this.name = _name;
        }

        public CacheStatistics(string _name, int _size)
        {
            this.name = string.Empty;
            this.name = _name;
            this.size = _size;
        }

        public void CacheHit()
        {
            Interlocked.Increment(ref this.total);
            Interlocked.Increment(ref this.hit);
        }

        public void CacheMiss()
        {
            Interlocked.Increment(ref this.total);
        }

        public Message FromMessage(Message _input)
        {
            this.name = _input.ReadString();
            this.size = _input.ReadS32();
            this.total = _input.ReadS32();
            this.hit = _input.ReadS32();
            return _input;
        }

        public void InitHitCount()
        {
            Interlocked.Exchange(ref this.total, 0);
            Interlocked.Exchange(ref this.hit, 0);
        }

        public Message ToMessage()
        {
            Message message = new Message(0, 0L);
            message.WriteString(this.name);
            message.WriteS32(this.size);
            message.WriteS32(this.total);
            message.WriteS32(this.hit);
            return message;
        }

        public int Hit
        {
            get
            {
                return this.hit;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public int Size
        {
            get
            {
                return this.size;
            }
            set
            {
                this.size = value;
            }
        }

        public int Total
        {
            get
            {
                return this.total;
            }
        }
    }
}

