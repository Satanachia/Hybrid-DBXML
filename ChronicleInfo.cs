using System;

public class ChronicleInfo : ChronicleInfoBase
{
    public bool ContentEquals(ChronicleInfo _info)
    {
        return (((((base.questName == _info.questName) && (base.keyword == _info.keyword)) && ((base.localtext == _info.localtext) && (base.sort == _info.sort))) && (((base.group == _info.group) && (base.source == _info.source)) && (base.width == _info.width))) && (base.height == _info.height));
    }

    public bool IsRankingChronicle
    {
        get
        {
            return (base.sort != string.Empty);
        }
    }
}

