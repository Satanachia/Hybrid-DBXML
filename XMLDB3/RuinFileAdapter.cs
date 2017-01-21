namespace XMLDB3
{
    using System;

    public class RuinFileAdapter : FileAdapter, RuinAdapter
    {
        private const string relicFileName = "relic";
        private const string ruinFileName = "ruin";

        private string GetName(RuinType _type)
        {
            switch (_type)
            {
                case RuinType.rtRuin:
                    return "ruin";

                case RuinType.rtRelic:
                    return "relic";
            }
            throw new Exception("잘못된 유적 타입입니다.");
        }

        public void Initialize(string _argument)
        {
            base.Initialize(typeof(RuinList), ConfigManager.GetFileDBPath("ruin"), ".xml");
        }

        public RuinList Read(RuinType _type)
        {
            string name = this.GetName(_type);
            if (base.IsExistData(name))
            {
                return (RuinList) base.ReadFromDB(name);
            }
            return new RuinList();
        }

        public bool Write(Ruin _ruin, RuinType _type)
        {
            string name = this.GetName(_type);
            RuinList list = null;
            if (base.IsExistData(name))
            {
                list = (RuinList) base.ReadFromDB(name);
            }
            if (list != null)
            {
                if ((list.ruins != null) && (list.ruins.Length > 0))
                {
                    for (int i = 0; i < list.ruins.Length; i++)
                    {
                        if (list.ruins[i].ruinID == _ruin.ruinID)
                        {
                            list.ruins[i] = _ruin;
                            base.WriteToDB(list, name);
                            return true;
                        }
                    }
                }
                Ruin[] array = new Ruin[list.ruins.Length + 1];
                if ((list.ruins != null) && (list.ruins.Length > 0))
                {
                    list.ruins.CopyTo(array, 0);
                }
                array[list.ruins.Length] = _ruin;
                base.WriteToDB(list, name);
                return true;
            }
            list = new RuinList();
            list.ruins = new Ruin[] { _ruin };
            base.WriteToDB(list, name);
            return true;
        }

        public bool Write(RuinList _ruinList, RuinType _type)
        {
            base.WriteToDB(_ruinList, this.GetName(_type));
            return true;
        }
    }
}

