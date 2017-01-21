namespace XMLDB3
{
    using System;

    public class PetParameterUpdateBuilder
    {
        public static string Build(Pet _new, Pet _old)
        {
            if ((_new.parameter == null) || (_old.parameter == null))
            {
                return string.Empty;
            }
            string str = string.Empty;
            if (_new.parameter.life != _old.parameter.life)
            {
                if (float.IsNaN(_new.parameter.life))
                {
                    _new.parameter.life = -9999f;
                }
                str = str + ",[life]=" + _new.parameter.life;
            }
            if (_new.parameter.life_damage != _old.parameter.life_damage)
            {
                str = str + ",[life_damage]=" + _new.parameter.life_damage;
            }
            if (_new.parameter.life_max != _old.parameter.life_max)
            {
                str = str + ",[life_max]=" + _new.parameter.life_max;
            }
            if (_new.parameter.mana != _old.parameter.mana)
            {
                str = str + ",[mana]=" + _new.parameter.mana;
            }
            if (_new.parameter.mana_max != _old.parameter.mana_max)
            {
                str = str + ",[mana_max]=" + _new.parameter.mana_max;
            }
            if (_new.parameter.stamina != _old.parameter.stamina)
            {
                str = str + ",[stamina]=" + _new.parameter.stamina;
            }
            if (_new.parameter.stamina_max != _old.parameter.stamina_max)
            {
                str = str + ",[stamina_max]=" + _new.parameter.stamina_max;
            }
            if (_new.parameter.food != _old.parameter.food)
            {
                str = str + ",[food]=" + _new.parameter.food;
            }
            if (_new.parameter.level != _old.parameter.level)
            {
                str = str + ",[level]=" + _new.parameter.level;
            }
            if (_new.parameter.cumulatedlevel != _old.parameter.cumulatedlevel)
            {
                str = str + ",[cumulatedlevel]=" + _new.parameter.cumulatedlevel;
            }
            if (_new.parameter.maxlevel != _old.parameter.maxlevel)
            {
                str = str + ",[maxlevel]=" + _new.parameter.maxlevel;
            }
            if (_new.parameter.rebirthcount != _old.parameter.rebirthcount)
            {
                str = str + ",[rebirthcount]=" + _new.parameter.rebirthcount;
            }
            if (_new.parameter.experience != _old.parameter.experience)
            {
                str = str + ",[experience]=" + _new.parameter.experience;
            }
            if (_new.parameter.age != _old.parameter.age)
            {
                str = str + ",[age]=" + _new.parameter.age;
            }
            if (_new.parameter.strength != _old.parameter.strength)
            {
                str = str + ",[strength]=" + _new.parameter.strength;
            }
            if (_new.parameter.dexterity != _old.parameter.dexterity)
            {
                str = str + ",[dexterity]=" + _new.parameter.dexterity;
            }
            if (_new.parameter.intelligence != _old.parameter.intelligence)
            {
                str = str + ",[intelligence]=" + _new.parameter.intelligence;
            }
            if (_new.parameter.will != _old.parameter.will)
            {
                str = str + ",[will]=" + _new.parameter.will;
            }
            if (_new.parameter.luck != _old.parameter.luck)
            {
                str = str + ",[luck]=" + _new.parameter.luck;
            }
            if (_new.parameter.attack_min != _old.parameter.attack_min)
            {
                str = str + ",[attack_min]=" + _new.parameter.attack_min;
            }
            if (_new.parameter.attack_max != _old.parameter.attack_max)
            {
                str = str + ",[attack_max]=" + _new.parameter.attack_max;
            }
            if (_new.parameter.wattack_min != _old.parameter.wattack_min)
            {
                str = str + ",[wattack_min]=" + _new.parameter.wattack_min;
            }
            if (_new.parameter.wattack_max != _old.parameter.wattack_max)
            {
                str = str + ",[wattack_max]=" + _new.parameter.wattack_max;
            }
            if (_new.parameter.critical != _old.parameter.critical)
            {
                str = str + ",[critical]=" + _new.parameter.critical;
            }
            if (_new.parameter.protect != _old.parameter.protect)
            {
                str = str + ",[protect]=" + _new.parameter.protect;
            }
            if (_new.parameter.defense != _old.parameter.defense)
            {
                str = str + ",[defense]=" + _new.parameter.defense;
            }
            if (_new.parameter.rate != _old.parameter.rate)
            {
                str = str + ",[rate]=" + _new.parameter.rate;
            }
            return str;
        }
    }
}

