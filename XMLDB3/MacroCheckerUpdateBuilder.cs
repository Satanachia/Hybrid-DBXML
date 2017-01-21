namespace XMLDB3
{
    using System;

    public class MacroCheckerUpdateBuilder
    {
        public static string Build(Character _new, Character _old)
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

