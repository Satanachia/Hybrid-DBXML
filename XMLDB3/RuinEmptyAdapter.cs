namespace XMLDB3
{
    using System;

    public class RuinEmptyAdapter : RuinAdapter
    {
        public void Initialize(string _argument)
        {
        }

        public RuinList Read(RuinType _type)
        {
            return null;
        }

        public bool Write(Ruin _ruin, RuinType _type)
        {
            return false;
        }

        public bool Write(RuinList _ruinlist, RuinType _type)
        {
            return false;
        }
    }
}

