namespace XMLDB3
{
    using System;
    using System.Data;

    public class JobObjectBuilder
    {
        public static CharacterJob Build(DataRow _character_row)
        {
            CharacterJob job = new CharacterJob();
            job.jobId = (byte) _character_row["jobId"];
            return job;
        }
    }
}

