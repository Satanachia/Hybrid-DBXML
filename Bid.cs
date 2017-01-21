using System;
using System.Xml.Schema;
using System.Xml.Serialization;

public class Bid
{
    [XmlAttribute(Form=XmlSchemaForm.Qualified)]
    public int auctionItemID;
    [XmlAttribute(Form=XmlSchemaForm.Qualified)]
    public long bidID;
    [XmlAttribute(Form=XmlSchemaForm.Qualified)]
    public byte bidState;
    [XmlAttribute(Form=XmlSchemaForm.Qualified)]
    public long charID;
    [XmlAttribute(Form=XmlSchemaForm.Qualified)]
    public string charName;
    [XmlAttribute(Form=XmlSchemaForm.Qualified)]
    public int price;
    [XmlAttribute(Form=XmlSchemaForm.Qualified)]
    public DateTime time;
}

