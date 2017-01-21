using System;
using System.Xml.Serialization;

[XmlRoot(Namespace="", IsNullable=false)]
public class ShopAdvertise
{
    public ShopAdvertiseItem[] items;
    public ShopAdvertisebase shopInfo;

    protected bool IsValid()
    {
        return (((this.shopInfo != null) && (this.shopInfo.account != null)) && (this.shopInfo.server != null));
    }

    public override string ToString()
    {
        return (base.ToString() + ":" + (this.IsValid() ? (this.shopInfo.server + ":" + this.shopInfo.account) : "invalid"));
    }
}

