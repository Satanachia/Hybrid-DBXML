namespace XMLDB3
{
    using System;

    public class ServiceUpdateBuilder
    {
        public static string Build(Character _new, Character _old)
        {
            if ((_new.service == null) || (_old.service == null))
            {
                return string.Empty;
            }
            string str = string.Empty;
            if (_new.service.nsrespawncount != _old.service.nsrespawncount)
            {
                str = str + ",[nsrespawncount]=" + _new.service.nsrespawncount;
            }
            if (_new.service.nslastrespawnday != _old.service.nslastrespawnday)
            {
                str = str + ",[nslastrespawnday]=" + _new.service.nslastrespawnday;
            }
            if (_new.service.nsgiftreceiveday != _old.service.nsgiftreceiveday)
            {
                str = str + ",[nsgiftreceiveday]=" + _new.service.nsgiftreceiveday;
            }
            if (_new.service.apgiftreceiveday != _old.service.apgiftreceiveday)
            {
                str = str + ",[apgiftreceiveday]=" + _new.service.apgiftreceiveday;
            }
            if (_new.service.nsbombcount != _old.service.nsbombcount)
            {
                str = str + ",[nsbombcount]=" + _new.service.nsbombcount;
            }
            if (_new.service.nsbombday != _old.service.nsbombday)
            {
                str = str + ",[nsbombday]=" + _new.service.nsbombday;
            }
            return str;
        }
    }
}

