namespace XMLDB3
{
    using System;
    using System.Data;

    public class PetMacroCheckerObjectBuilder
    {
        public static PetMacroChecker Build(DataRow _pet_row)
        {
            PetMacroChecker checker = new PetMacroChecker();
            checker.macroPoint = (int) _pet_row["macroPoint"];
            return checker;
        }
    }
}

