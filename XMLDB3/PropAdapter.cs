namespace XMLDB3
{
    using System;

    public interface PropAdapter
    {
        bool Create(Prop _data);
        bool Delete(long _id);
        void Initialize(string _argument);
        PropIDList LoadPropList();
        Prop Read(long _id);
        bool Write(Prop _data);
    }
}

