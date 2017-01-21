namespace XMLDB3
{
    using System;

    public interface RuinAdapter
    {
        void Initialize(string _argument);
        RuinList Read(RuinType _type);
        bool Write(Ruin _ruin, RuinType _type);
        bool Write(RuinList _ruinList, RuinType _type);
    }
}

