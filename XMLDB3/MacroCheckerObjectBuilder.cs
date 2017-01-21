namespace XMLDB3
{
    using System;
    using System.Data;

    public class MacroCheckerObjectBuilder
    {
        public static CharacterMacroChecker Build(DataRow _character_row)
        {
            CharacterMacroChecker checker = new CharacterMacroChecker();
            checker.macroPoint = (int) _character_row["macroPoint"];
            return checker;
        }
    }
}

