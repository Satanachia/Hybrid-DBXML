namespace XMLDB3
{
    using System;

    public class PetMacroCheckerUpdateBuilder
    {
        public static string Build(Pet _new, Pet _old)
        {
            if ((_new.macroChecker == null) || (_old.macroChecker == null))
            {
                return string.Empty;
            }
            string str = string.Empty;
            if (_new.macroChecker.macroPoint != _old.macroChecker.macroPoint)
            {
                str = str + ",[macroPoint]=" + _new.macroChecker.macroPoint;
            }
            return str;
        }
    }
}

