namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class CastleBuildSerializer
    {
        public static void Deserialize(CastleBuild _build, Message _message)
        {
            if (_build == null)
            {
                _build = new CastleBuild();
            }
            _message.WriteS32(_build.durability);
            _message.WriteS32(_build.maxDurability);
            _message.WriteU8(_build.buildState);
            _message.WriteS64(_build.buildNextTime.Ticks);
            _message.WriteU8(_build.buildStep);
            if (_build.resource != null)
            {
                _message.WriteS32(_build.resource.Length);
                foreach (CastleBuildResource resource in _build.resource)
                {
                    CastleBuildResourceSerializer.Deserialize(resource, _message);
                }
            }
            else
            {
                _message.WriteS32(0);
            }
        }

        public static CastleBuild Serialize(Message _message)
        {
            CastleBuild build = new CastleBuild();
            build.durability = _message.ReadS32();
            build.maxDurability = _message.ReadS32();
            build.buildState = _message.ReadU8();
            build.buildNextTime = new DateTime(_message.ReadS64());
            build.buildStep = _message.ReadU8();
            int num = _message.ReadS32();
            if (num > 0)
            {
                build.resource = new CastleBuildResource[num];
                for (int i = 0; i < num; i++)
                {
                    build.resource[i] = CastleBuildResourceSerializer.Serialize(_message);
                }
                return build;
            }
            build.resource = null;
            return build;
        }
    }
}

