namespace XMLDB3
{
    using System;

    public class JobUpdateBuilder
    {
        public static string Build(Character _new, Character _old)
        {
            if ((_new.job == null) || (_old.job == null))
            {
                return string.Empty;
            }
            string str = string.Empty;
            if (_new.job.jobId != _old.job.jobId)
            {
                str = str + ",[jobId]=" + _new.job.jobId;
            }
            return str;
        }
    }
}

