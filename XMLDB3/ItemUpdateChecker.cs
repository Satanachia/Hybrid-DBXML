namespace XMLDB3
{
    using System;

    public class ItemUpdateChecker
    {
        public static bool CheckEgoItem(Item _new, Item _old)
        {
            return ((_new.pocket != _old.pocket) || ((_new.@class != _old.@class) || ((_new.pos_x != _old.pos_x) || ((_new.pos_y != _old.pos_y) || ((_new.varint != _old.varint) || ((_new.color_01 != _old.color_01) || ((_new.color_02 != _old.color_02) || ((_new.color_03 != _old.color_03) || ((_new.price != _old.price) || ((_new.sellingprice != _old.sellingprice) || ((_new.bundle != _old.bundle) || ((_new.linked_pocket != _old.linked_pocket) || ((_new.figure != _old.figure) || ((_new.flag != _old.flag) || ((_new.durability != _old.durability) || ((_new.durability_max != _old.durability_max) || ((_new.origin_durability_max != _old.origin_durability_max) || ((_new.attack_min != _old.attack_min) || ((_new.attack_max != _old.attack_max) || ((_new.wattack_min != _old.wattack_min) || ((_new.wattack_max != _old.wattack_max) || ((_new.balance != _old.balance) || ((_new.critical != _old.critical) || ((_new.defence != _old.defence) || ((_new.protect != _old.protect) || ((_new.effective_range != _old.effective_range) || ((_new.attack_speed != _old.attack_speed) || ((_new.down_hit_count != _old.down_hit_count) || ((_new.experience != _old.experience) || ((_new.exp_point != _old.exp_point) || ((_new.upgraded != _old.upgraded) || ((_new.upgrade_max != _old.upgrade_max) || ((_new.grade != _old.grade) || ((_new.prefix != _old.prefix) || ((_new.suffix != _old.suffix) || ((_new.data != _old.data) || ((_new.expiration != _old.expiration) || ((_new.ego.egoName != _old.ego.egoName) || ((_new.ego.egoType != _old.ego.egoType) || ((_new.ego.egoDesire != _old.ego.egoDesire) || ((_new.ego.egoSocialLevel != _old.ego.egoSocialLevel) || ((_new.ego.egoSocialExp != _old.ego.egoSocialExp) || ((_new.ego.egoStrLevel != _old.ego.egoStrLevel) || ((_new.ego.egoStrExp != _old.ego.egoStrExp) || ((_new.ego.egoIntLevel != _old.ego.egoIntLevel) || ((_new.ego.egoIntExp != _old.ego.egoIntExp) || ((_new.ego.egoDexLevel != _old.ego.egoDexLevel) || ((_new.ego.egoDexExp != _old.ego.egoDexExp) || ((_new.ego.egoWillLevel != _old.ego.egoWillLevel) || ((_new.ego.egoWillExp != _old.ego.egoWillExp) || ((_new.ego.egoLuckLevel != _old.ego.egoLuckLevel) || ((_new.ego.egoLuckExp != _old.ego.egoLuckExp) || ((_new.ego.egoSkillCount != _old.ego.egoSkillCount) || ((_new.ego.egoSkillGauge != _old.ego.egoSkillGauge) || (_new.ego.egoSkillCoolTime != _old.ego.egoSkillCoolTime)))))))))))))))))))))))))))))))))))))))))))))))))))))));
        }

        public static bool CheckHugeItem(Item _new, Item _old)
        {
            return ((_new.pocket != _old.pocket) || ((_new.@class != _old.@class) || ((_new.pos_x != _old.pos_x) || ((_new.pos_y != _old.pos_y) || ((_new.varint != _old.varint) || ((_new.color_01 != _old.color_01) || ((_new.color_02 != _old.color_02) || ((_new.color_03 != _old.color_03) || ((_new.price != _old.price) || ((_new.sellingprice != _old.sellingprice) || ((_new.bundle != _old.bundle) || ((_new.linked_pocket != _old.linked_pocket) || ((_new.flag != _old.flag) || ((_new.durability != _old.durability) || ((_new.durability_max != _old.durability_max) || ((_new.origin_durability_max != _old.origin_durability_max) || ((_new.data != _old.data) || (_new.expiration != _old.expiration))))))))))))))))));
        }

        public static bool CheckLargeItem(Item _new, Item _old)
        {
            if (_new.pocket != _old.pocket)
            {
                return true;
            }
            if (_new.@class != _old.@class)
            {
                return true;
            }
            if (_new.pos_x != _old.pos_x)
            {
                return true;
            }
            if (_new.pos_y != _old.pos_y)
            {
                return true;
            }
            if (_new.varint != _old.varint)
            {
                return true;
            }
            if (_new.color_01 != _old.color_01)
            {
                return true;
            }
            if (_new.color_02 != _old.color_02)
            {
                return true;
            }
            if (_new.color_03 != _old.color_03)
            {
                return true;
            }
            if (_new.price != _old.price)
            {
                return true;
            }
            if (_new.sellingprice != _old.sellingprice)
            {
                return true;
            }
            if (_new.bundle != _old.bundle)
            {
                return true;
            }
            if (_new.linked_pocket != _old.linked_pocket)
            {
                return true;
            }
            if (_new.figure != _old.figure)
            {
                return true;
            }
            if (_new.flag != _old.flag)
            {
                return true;
            }
            if (_new.durability != _old.durability)
            {
                return true;
            }
            if (_new.durability_max != _old.durability_max)
            {
                return true;
            }
            if (_new.origin_durability_max != _old.origin_durability_max)
            {
                return true;
            }
            if (_new.attack_min != _old.attack_min)
            {
                return true;
            }
            if (_new.attack_max != _old.attack_max)
            {
                return true;
            }
            if (_new.wattack_min != _old.wattack_min)
            {
                return true;
            }
            if (_new.wattack_max != _old.wattack_max)
            {
                return true;
            }
            if (_new.balance != _old.balance)
            {
                return true;
            }
            if (_new.critical != _old.critical)
            {
                return true;
            }
            if (_new.defence != _old.defence)
            {
                return true;
            }
            if (_new.protect != _old.protect)
            {
                return true;
            }
            if (_new.effective_range != _old.effective_range)
            {
                return true;
            }
            if (_new.attack_speed != _old.attack_speed)
            {
                return true;
            }
            if (_new.down_hit_count != _old.down_hit_count)
            {
                return true;
            }
            if (_new.experience != _old.experience)
            {
                return true;
            }
            if (_new.exp_point != _old.exp_point)
            {
                return true;
            }
            if (_new.upgraded != _old.upgraded)
            {
                return true;
            }
            if (_new.upgrade_max != _old.upgrade_max)
            {
                return true;
            }
            if (_new.grade != _old.grade)
            {
                return true;
            }
            if (_new.prefix != _old.prefix)
            {
                return true;
            }
            if (_new.suffix != _old.suffix)
            {
                return true;
            }
            if (_new.data != _old.data)
            {
                return true;
            }
            string str = ItemXmlFieldHelper.BuildItemOptionXml(_old.options);
            string str2 = ItemXmlFieldHelper.BuildItemOptionXml(_new.options);
            return ((str != str2) || (_new.expiration != _old.expiration));
        }

        public static bool CheckQuestItem(Item _new, Item _old)
        {
            if (_new.pocket != _old.pocket)
            {
                return true;
            }
            if (_new.@class != _old.@class)
            {
                return true;
            }
            if (_new.pos_x != _old.pos_x)
            {
                return true;
            }
            if (_new.pos_y != _old.pos_y)
            {
                return true;
            }
            if (_new.varint != _old.varint)
            {
                return true;
            }
            if (_new.color_01 != _old.color_01)
            {
                return true;
            }
            if (_new.color_02 != _old.color_02)
            {
                return true;
            }
            if (_new.color_03 != _old.color_03)
            {
                return true;
            }
            if (_new.price != _old.price)
            {
                return true;
            }
            if (_new.sellingprice != _old.sellingprice)
            {
                return true;
            }
            if (_new.bundle != _old.bundle)
            {
                return true;
            }
            if (_new.linked_pocket != _old.linked_pocket)
            {
                return true;
            }
            if (_new.flag != _old.flag)
            {
                return true;
            }
            if (_new.durability != _old.durability)
            {
                return true;
            }
            if (_new.expiration != _old.expiration)
            {
                return true;
            }
            if (_new.quest.id != _old.quest.id)
            {
                return true;
            }
            if (_new.quest.templateid != _old.quest.templateid)
            {
                return true;
            }
            if (_new.quest.complete != _old.quest.complete)
            {
                return true;
            }
            if (_new.quest.start_time != _old.quest.start_time)
            {
                return true;
            }
            if (_new.quest.data != _old.quest.data)
            {
                return true;
            }
            string str = ItemXmlFieldHelper.BuildQuestObjectiveXml(_new.quest.objectives);
            string str2 = ItemXmlFieldHelper.BuildQuestObjectiveXml(_old.quest.objectives);
            return (str != str2);
        }

        public static bool CheckSmallItem(Item _new, Item _old)
        {
            return ((_new.pocket != _old.pocket) || ((_new.@class != _old.@class) || ((_new.pos_x != _old.pos_x) || ((_new.pos_y != _old.pos_y) || ((_new.varint != _old.varint) || ((_new.color_01 != _old.color_01) || ((_new.color_02 != _old.color_02) || ((_new.color_03 != _old.color_03) || ((_new.price != _old.price) || ((_new.sellingprice != _old.sellingprice) || ((_new.bundle != _old.bundle) || ((_new.linked_pocket != _old.linked_pocket) || ((_new.flag != _old.flag) || ((_new.durability != _old.durability) || (_new.expiration != _old.expiration)))))))))))))));
        }
    }
}

