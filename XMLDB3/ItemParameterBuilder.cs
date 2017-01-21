namespace XMLDB3
{
    using System;

    public class ItemParameterBuilder
    {
        public static string BuildEgoItem(Item _item)
        {
            return string.Concat(new object[] { 
                ",@itemID=", _item.id, ",@pocketid=", _item.pocket, ",@class=", _item.@class, ",@pos_x=", _item.pos_x, ",@pos_y=", _item.pos_y, ",@varint=", _item.varint, ",@color_01=", _item.color_01, ",@color_02=", _item.color_02, 
                ",@color_03=", _item.color_03, ",@price=", _item.price, ",@sellingprice=", _item.sellingprice, ",@bundle=", _item.bundle, ",@linked_pocket=", _item.linked_pocket, ",@figure=", _item.figure, ",@flag=", _item.flag, ",@durability=", _item.durability, 
                ",@durability_max=", _item.durability_max, ",@origin_durability_max=", _item.origin_durability_max, ",@attack_min=", _item.attack_min, ",@attack_max=", _item.attack_max, ",@wattack_min=", _item.wattack_min, ",@wattack_max=", _item.wattack_max, ",@balance=", _item.balance, ",@critical=", _item.critical, 
                ",@defence=", _item.defence, ",@protect=", _item.protect, ",@effective_range=", _item.effective_range, ",@attack_speed=", _item.attack_speed, ",@down_hit_count=", _item.down_hit_count, ",@experience=", _item.experience, ",@exp_point=", _item.exp_point, ",@upgraded=", _item.upgraded, 
                ",@upgrade_max=", _item.upgrade_max, ",@grade=", _item.grade, ",@prefix=", _item.prefix, ",@suffix=", _item.suffix, ",@data=", UpdateUtility.BuildString(_item.data), ",@expiration=", _item.expiration, ",@egoname=", UpdateUtility.BuildString(_item.ego.egoName), ",@egotype=", _item.ego.egoType, 
                ",@egodesire=", _item.ego.egoDesire, ",@egosociallevel=", _item.ego.egoSocialLevel, ",@egosocialexp=", _item.ego.egoSocialExp, ",@egostrlevel=", _item.ego.egoStrLevel, ",@egostrexp=", _item.ego.egoStrExp, ",@egointlevel=", _item.ego.egoIntLevel, ",@egointexp=", _item.ego.egoIntExp, ",@egodexlevel=", _item.ego.egoDexLevel, 
                ",@egodexexp=", _item.ego.egoDexExp, ",@egowilllevel=", _item.ego.egoWillLevel, ",@egowillexp=", _item.ego.egoWillExp, ",@egolucklevel=", _item.ego.egoLuckLevel, ",@egoluckexp=", _item.ego.egoLuckExp, ",@egoskillcount=", _item.ego.egoSkillCount, ",@egoskillgauge=", _item.ego.egoSkillGauge, ",@egoskillcooltime=", _item.ego.egoSkillCoolTime
             });
        }

        public static string BuildHugeItem(Item _item)
        {
            return string.Concat(new object[] { 
                ",@itemID=", _item.id, ",@pocketid=", _item.pocket, ",@class=", _item.@class, ",@pos_x=", _item.pos_x, ",@pos_y=", _item.pos_y, ",@varint=", _item.varint, ",@color_01=", _item.color_01, ",@color_02=", _item.color_02, 
                ",@color_03=", _item.color_03, ",@price=", _item.price, ",@sellingprice=", _item.sellingprice, ",@bundle=", _item.bundle, ",@linked_pocket=", _item.linked_pocket, ",@flag=", _item.flag, ",@durability=", _item.durability, ",@durability_max=", _item.durability_max, 
                ",@origin_durability_max=", _item.origin_durability_max, ",@data=", UpdateUtility.BuildString(_item.data), ",@expiration=", _item.expiration
             });
        }

        public static string BuildLargeItem(Item _item)
        {
            return string.Concat(new object[] { 
                ",@itemID=", _item.id, ",@pocketid=", _item.pocket, ",@class=", _item.@class, ",@pos_x=", _item.pos_x, ",@pos_y=", _item.pos_y, ",@varint=", _item.varint, ",@color_01=", _item.color_01, ",@color_02=", _item.color_02, 
                ",@color_03=", _item.color_03, ",@price=", _item.price, ",@sellingprice=", _item.sellingprice, ",@bundle=", _item.bundle, ",@linked_pocket=", _item.linked_pocket, ",@figure=", _item.figure, ",@flag=", _item.flag, ",@durability=", _item.durability, 
                ",@durability_max=", _item.durability_max, ",@origin_durability_max=", _item.origin_durability_max, ",@attack_min=", _item.attack_min, ",@attack_max=", _item.attack_max, ",@wattack_min=", _item.wattack_min, ",@wattack_max=", _item.wattack_max, ",@balance=", _item.balance, ",@critical=", _item.critical, 
                ",@defence=", _item.defence, ",@protect=", _item.protect, ",@effective_range=", _item.effective_range, ",@attack_speed=", _item.attack_speed, ",@down_hit_count=", _item.down_hit_count, ",@experience=", _item.experience, ",@exp_point=", _item.exp_point, ",@upgraded=", _item.upgraded, 
                ",@upgrade_max=", _item.upgrade_max, ",@grade=", _item.grade, ",@prefix=", _item.prefix, ",@suffix=", _item.suffix, ",@data=", UpdateUtility.BuildString(_item.data), ",@option=", UpdateUtility.BuildString(ItemXmlFieldHelper.BuildItemOptionXml(_item.options)), ",@expiration=", _item.expiration
             });
        }

        public static string BuildQuestItem(Item _item)
        {
            return string.Concat(new object[] { 
                ",@itemID=", _item.id, ",@pocketid=", _item.pocket, ",@quest=", _item.quest.id, ",@class=", _item.@class, ",@pos_x=", _item.pos_x, ",@pos_y=", _item.pos_y, ",@varint=", _item.varint, ",@color_01=", _item.color_01, 
                ",@color_02=", _item.color_02, ",@color_03=", _item.color_03, ",@price=", _item.price, ",@sellingprice=", _item.sellingprice, ",@bundle=", _item.bundle, ",@linked_pocket=", _item.linked_pocket, ",@flag=", _item.flag, ",@durability=", _item.durability, 
                ",@expiration=", _item.expiration, ",@templateid=", _item.quest.templateid, ",@complete=", _item.quest.complete, ",@start_time=", _item.quest.start_time, ",@data=", UpdateUtility.BuildString(_item.quest.data), ",@objective=", UpdateUtility.BuildString(ItemXmlFieldHelper.BuildQuestObjectiveXml(_item.quest.objectives))
             });
        }

        public static string BuildSmallItem(Item _item)
        {
            return string.Concat(new object[] { 
                ",@itemID=", _item.id, ",@pocketid=", _item.pocket, ",@class=", _item.@class, ",@pos_x=", _item.pos_x, ",@pos_y=", _item.pos_y, ",@varint=", _item.varint, ",@color_01=", _item.color_01, ",@color_02=", _item.color_02, 
                ",@color_03=", _item.color_03, ",@price=", _item.price, ",@sellingprice=", _item.sellingprice, ",@bundle=", _item.bundle, ",@linked_pocket=", _item.linked_pocket, ",@flag=", _item.flag, ",@durability=", _item.durability, ",@expiration=", _item.expiration
             });
        }
    }
}

