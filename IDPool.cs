using System;
using System.Xml.Serialization;

[XmlInclude(typeof(ItemIDPool))]
public abstract class IDPool
{
    [XmlAttribute]
    public long count;

    protected IDPool()
    {
    }
}

