using System;
using System.Xml.Serialization;

[XmlRoot(Namespace="", IsNullable=false)]
public class BankSlotBase
{
    [XmlElement("item")]
    public BankItem[] item;
    public BankSlotInfo slot;
}

