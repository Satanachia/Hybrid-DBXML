namespace XMLDB3
{
    using System;
    using System.Data;

    public class ServiceObjectBuilder
    {
        public static CharacterService Build(DataRow _character_row)
        {
            CharacterService service = new CharacterService();
            service.nsrespawncount = (byte) _character_row["nsrespawncount"];
            service.nslastrespawnday = (int) _character_row["nslastrespawnday"];
            service.nsgiftreceiveday = (int) _character_row["nsgiftreceiveday"];
            service.apgiftreceiveday = (int) _character_row["apgiftreceiveday"];
            service.nsbombcount = (byte) _character_row["nsbombcount"];
            service.nsbombday = (int) _character_row["nsbombday"];
            return service;
        }
    }
}

