using System;
using System.Xml.Serialization;

[XmlRoot(Namespace="", IsNullable=false)]
public class PromotionRecordTable
{
    [XmlElement("records")]
    public PromotionRecord[] records;
}

