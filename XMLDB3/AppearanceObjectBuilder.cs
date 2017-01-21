namespace XMLDB3
{
    using System;
    using System.Data;

    public class AppearanceObjectBuilder
    {
        public static CharacterAppearance Build(DataRow _character_row)
        {
            CharacterAppearance appearance = new CharacterAppearance();
            appearance.type = (int) _character_row["type"];
            appearance.skin_color = (byte) _character_row["skin_color"];
            appearance.eye_type = (byte) _character_row["eye_type"];
            appearance.eye_color = (byte) _character_row["eye_color"];
            appearance.mouth_type = (byte) _character_row["mouth_type"];
            appearance.status = (int) _character_row["status"];
            appearance.height = (float) _character_row["height"];
            appearance.fatness = (float) _character_row["fatness"];
            appearance.upper = (float) _character_row["upper"];
            appearance.lower = (float) _character_row["lower"];
            appearance.region = (int) _character_row["region"];
            appearance.x = (int) _character_row["x"];
            appearance.y = (int) _character_row["y"];
            appearance.direction = (byte) _character_row["direction"];
            appearance.battle_state = (int) _character_row["battle_state"];
            appearance.weapon_set = (byte) _character_row["weapon_set"];
            return appearance;
        }
    }
}

