using System;
using System.Xml.Serialization;

[XmlRoot(Namespace="", IsNullable=false)]
public class House
{
    [XmlAttribute]
    public string account;
    public HouseBid bid;
    [XmlElement("bidders")]
    public HouseBidder[] bidders;
    [XmlAttribute]
    public DateTime bidSuccessDate;
    [XmlElement("block")]
    public HouseBlock[] block;
    [XmlAttribute]
    public string charName;
    [XmlAttribute]
    public byte constructed;
    [XmlAttribute]
    public int deposit;
    [XmlAttribute]
    public long flag;
    [XmlAttribute]
    public int height;
    [XmlAttribute]
    public int houseClass;
    [XmlAttribute]
    public long houseID;
    [XmlAttribute]
    public int houseMoney;
    [XmlAttribute]
    public string houseName;
    [XmlAttribute]
    public byte innerColor1;
    [XmlAttribute]
    public byte innerColor2;
    [XmlAttribute]
    public byte innerColor3;
    [XmlAttribute]
    public byte innerSkin;
    [XmlAttribute]
    public byte roofColor1;
    [XmlAttribute]
    public byte roofColor2;
    [XmlAttribute]
    public byte roofColor3;
    [XmlAttribute]
    public byte roofSkin;
    [XmlAttribute]
    public byte taxAutopay;
    [XmlAttribute]
    public DateTime taxNextDate;
    [XmlAttribute]
    public DateTime taxPrevDate;
    [XmlAttribute]
    public int taxPrice;
    [XmlAttribute]
    public DateTime updateTime;
    [XmlAttribute]
    public byte wallColor1;
    [XmlAttribute]
    public byte wallColor2;
    [XmlAttribute]
    public byte wallColor3;
    [XmlAttribute]
    public byte wallSkin;
    [XmlAttribute]
    public int width;
}

