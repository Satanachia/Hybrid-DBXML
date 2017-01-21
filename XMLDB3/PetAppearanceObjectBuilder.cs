namespace XMLDB3
{
    using System;
    using System.Data;

    public class PetAppearanceObjectBuilder
    {
        public static PetAppearance Build(DataRow _pet_row)
        {
            PetAppearance appearance = new PetAppearance();
            appearance.type = (int) _pet_row["type"];
            appearance.skin_color = (byte) _pet_row["skin_color"];
            appearance.eye_type = (byte) _pet_row["eye_type"];
            appearance.eye_color = (byte) _pet_row["eye_color"];
            appearance.mouth_type = (byte) _pet_row["mouth_type"];
            appearance.status = (int) _pet_row["status"];
            appearance.height = (float) _pet_row["height"];
            appearance.fatness = (float) _pet_row["fatness"];
            appearance.upper = (float) _pet_row["upper"];
            appearance.lower = (float) _pet_row["lower"];
            appearance.region = (int) _pet_row["region"];
            appearance.x = (int) _pet_row["x"];
            appearance.y = (int) _pet_row["y"];
            appearance.direction = (byte) _pet_row["direction"];
            appearance.battle_state = (int) _pet_row["battle_state"];
            appearance.extra_01 = (int) _pet_row["extra_01"];
            appearance.extra_02 = (int) _pet_row["extra_02"];
            appearance.extra_03 = (int) _pet_row["extra_03"];
            return appearance;
        }
    }
}

