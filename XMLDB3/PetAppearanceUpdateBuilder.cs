namespace XMLDB3
{
    using System;

    public class PetAppearanceUpdateBuilder
    {
        public static string Build(Pet _new, Pet _old)
        {
            if ((_new.appearance == null) || (_old.appearance == null))
            {
                return string.Empty;
            }
            string str = string.Empty;
            if (_new.appearance.type != _old.appearance.type)
            {
                str = str + ",[type]=" + _new.appearance.type;
            }
            if (_new.appearance.skin_color != _old.appearance.skin_color)
            {
                str = str + ",[skin_color]=" + _new.appearance.skin_color;
            }
            if (_new.appearance.eye_color != _old.appearance.eye_color)
            {
                str = str + ",[eye_color]=" + _new.appearance.eye_color;
            }
            if (_new.appearance.eye_type != _old.appearance.eye_type)
            {
                str = str + ",[eye_type]=" + _new.appearance.eye_type;
            }
            if (_new.appearance.mouth_type != _old.appearance.mouth_type)
            {
                str = str + ",[mouth_type]=" + _new.appearance.mouth_type;
            }
            if (_new.appearance.status != _old.appearance.status)
            {
                str = str + ",[status]=" + _new.appearance.status;
            }
            if (_new.appearance.height != _old.appearance.height)
            {
                str = str + ",[height]=" + _new.appearance.height;
            }
            if (_new.appearance.fatness != _old.appearance.fatness)
            {
                str = str + ",[fatness]=" + _new.appearance.fatness;
            }
            if (_new.appearance.upper != _old.appearance.upper)
            {
                str = str + ",[upper]=" + _new.appearance.upper;
            }
            if (_new.appearance.lower != _old.appearance.lower)
            {
                str = str + ",[lower]=" + _new.appearance.lower;
            }
            if (_new.appearance.region != _old.appearance.region)
            {
                str = str + ",[region]=" + _new.appearance.region;
            }
            if (_new.appearance.x != _old.appearance.x)
            {
                str = str + ",[x]=" + _new.appearance.x;
            }
            if (_new.appearance.y != _old.appearance.y)
            {
                str = str + ",[y]=" + _new.appearance.y;
            }
            if (_new.appearance.direction != _old.appearance.direction)
            {
                str = str + ",[direction]=" + _new.appearance.direction;
            }
            if (_new.appearance.battle_state != _old.appearance.battle_state)
            {
                str = str + ",[battle_state]=" + _new.appearance.battle_state;
            }
            if (_new.appearance.extra_01 != _old.appearance.extra_01)
            {
                str = str + ",[extra_01]=" + _new.appearance.extra_01;
            }
            if (_new.appearance.extra_02 != _old.appearance.extra_02)
            {
                str = str + ",[extra_02]=" + _new.appearance.extra_02;
            }
            if (_new.appearance.extra_03 != _old.appearance.extra_03)
            {
                str = str + ",[extra_03]=" + _new.appearance.extra_03;
            }
            return str;
        }
    }
}

