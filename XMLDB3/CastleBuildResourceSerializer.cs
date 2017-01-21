namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class CastleBuildResourceSerializer
    {
        public static void Deserialize(CastleBuildResource _resource, Message _message)
        {
            _message.WriteS32(_resource.classID);
            _message.WriteS32(_resource.curAmount);
            _message.WriteS32(_resource.maxAmount);
        }

        public static CastleBuildResource Serialize(Message _message)
        {
            CastleBuildResource resource = new CastleBuildResource();
            resource.classID = _message.ReadS32();
            resource.curAmount = _message.ReadS32();
            resource.maxAmount = _message.ReadS32();
            return resource;
        }
    }
}

