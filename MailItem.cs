using System;
using System.Xml.Serialization;

public class MailItem
{
    public Item item;
    [XmlAttribute]
    public int itemCharge;
    [XmlAttribute]
    public string location;
    [XmlAttribute]
    public long postID;
    [XmlAttribute]
    public byte postType;
    [XmlAttribute]
    public long receiverCharID;
    [XmlAttribute]
    public string receiverCharName;
    [XmlAttribute]
    public DateTime sendDate;
    [XmlAttribute]
    public long senderCharID;
    [XmlAttribute]
    public string senderCharName;
    [XmlAttribute]
    public string senderMsg;
    [XmlAttribute]
    public byte status;
}

