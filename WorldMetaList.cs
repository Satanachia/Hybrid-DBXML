﻿using System;
using System.Xml.Serialization;

[XmlRoot(Namespace="", IsNullable=false)]
public class WorldMetaList
{
    [XmlElement("metas")]
    public WorldMeta[] metas;
}

