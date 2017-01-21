namespace XMLDB3
{
    using System;
    using System.Data;

    public class ItemObjectBuilder
    {
        public static Item BuildEgoItem(DataRow _row)
        {
            Item item = new Item();
            item.ego = new Ego();
            item.storedtype = 5;
            item.id = (long) _row["itemID"];
            item.pocket = (byte) _row["pocketid"];
            item.@class = (int) _row["class"];
            item.pos_x = (int) _row["pos_x"];
            item.pos_y = (int) _row["pos_y"];
            item.color_01 = (int) _row["color_01"];
            item.color_02 = (int) _row["color_02"];
            item.color_03 = (int) _row["color_03"];
            item.price = (int) _row["price"];
            item.bundle = (short) _row["bundle"];
            item.linked_pocket = (byte) _row["linked_pocket"];
            item.figure = (int) _row["figure"];
            item.flag = (byte) _row["flag"];
            item.durability = (int) _row["durability"];
            item.durability_max = (int) _row["durability_max"];
            item.origin_durability_max = (int) _row["origin_durability_max"];
            item.attack_min = (short) _row["attack_min"];
            item.attack_max = (short) _row["attack_max"];
            item.wattack_min = (short) _row["wattack_min"];
            item.wattack_max = (short) _row["wattack_max"];
            item.balance = (byte) _row["balance"];
            item.critical = (byte) _row["critical"];
            item.defence = (int) _row["defence"];
            item.protect = (short) _row["protect"];
            item.effective_range = (short) _row["effective_range"];
            item.attack_speed = (byte) _row["attack_speed"];
            item.down_hit_count = (byte) _row["down_hit_count"];
            item.experience = (short) _row["experience"];
            item.exp_point = (byte) _row["exp_point"];
            item.upgraded = (byte) _row["upgraded"];
            item.upgrade_max = (byte) _row["upgrade_max"];
            item.grade = (byte) _row["grade"];
            item.prefix = (short) _row["prefix"];
            item.suffix = (short) _row["suffix"];
            item.data = (string) _row["data"];
            item.sellingprice = (int) _row["sellingprice"];
            item.expiration = (int) _row["expiration"];
            item.ego.egoName = (string) _row["egoname"];
            item.ego.egoType = (byte) _row["egotype"];
            item.ego.egoDesire = (byte) _row["egodesire"];
            item.ego.egoSocialLevel = (byte) _row["egosociallevel"];
            item.ego.egoSocialExp = (int) _row["egosocialexp"];
            item.ego.egoStrLevel = (byte) _row["egostrlevel"];
            item.ego.egoStrExp = (int) _row["egostrexp"];
            item.ego.egoIntLevel = (byte) _row["egointlevel"];
            item.ego.egoIntExp = (int) _row["egointexp"];
            item.ego.egoDexLevel = (byte) _row["egodexlevel"];
            item.ego.egoDexExp = (int) _row["egodexexp"];
            item.ego.egoWillLevel = (byte) _row["egowilllevel"];
            item.ego.egoWillExp = (int) _row["egowillexp"];
            item.ego.egoLuckLevel = (byte) _row["egolucklevel"];
            item.ego.egoLuckExp = (int) _row["egoluckexp"];
            item.ego.egoSkillCount = (byte) _row["egoskillcount"];
            item.ego.egoSkillGauge = (int) _row["egoskillgauge"];
            item.ego.egoSkillCoolTime = (long) _row["egoskillcooltime"];
            item.varint = (int) _row["varint"];
            return item;
        }

        public static Item BuildHugeItem(DataRow _row)
        {
            Item item = new Item();
            item.storedtype = 3;
            item.id = (long) _row["itemID"];
            item.pocket = (byte) _row["pocketid"];
            item.@class = (int) _row["class"];
            item.pos_x = (int) _row["pos_x"];
            item.pos_y = (int) _row["pos_y"];
            item.color_01 = (int) _row["color_01"];
            item.color_02 = (int) _row["color_02"];
            item.color_03 = (int) _row["color_03"];
            item.price = (int) _row["price"];
            item.bundle = (short) _row["bundle"];
            item.linked_pocket = (byte) _row["linked_pocket"];
            item.flag = (byte) _row["flag"];
            item.durability = (int) _row["durability"];
            item.durability_max = (int) _row["durability_max"];
            item.origin_durability_max = (int) _row["origin_durability_max"];
            item.sellingprice = (int) _row["sellingprice"];
            item.data = (string) _row["data"];
            item.expiration = (int) _row["expiration"];
            item.varint = (int) _row["varint"];
            return item;
        }

        public static Item BuildLargeItem(DataRow _row)
        {
            Item item = new Item();
            item.storedtype = 1;
            item.id = (long) _row["itemID"];
            item.pocket = (byte) _row["pocketid"];
            item.@class = (int) _row["class"];
            item.pos_x = (int) _row["pos_x"];
            item.pos_y = (int) _row["pos_y"];
            item.color_01 = (int) _row["color_01"];
            item.color_02 = (int) _row["color_02"];
            item.color_03 = (int) _row["color_03"];
            item.price = (int) _row["price"];
            item.bundle = (short) _row["bundle"];
            item.linked_pocket = (byte) _row["linked_pocket"];
            item.figure = (int) _row["figure"];
            item.flag = (byte) _row["flag"];
            item.durability = (int) _row["durability"];
            item.durability_max = (int) _row["durability_max"];
            item.origin_durability_max = (int) _row["origin_durability_max"];
            item.attack_min = (short) _row["attack_min"];
            item.attack_max = (short) _row["attack_max"];
            item.wattack_min = (short) _row["wattack_min"];
            item.wattack_max = (short) _row["wattack_max"];
            item.balance = (byte) _row["balance"];
            item.critical = (byte) _row["critical"];
            item.defence = (int) _row["defence"];
            item.protect = (short) _row["protect"];
            item.effective_range = (short) _row["effective_range"];
            item.attack_speed = (byte) _row["attack_speed"];
            item.down_hit_count = (byte) _row["down_hit_count"];
            item.experience = (short) _row["experience"];
            item.exp_point = (byte) _row["exp_point"];
            item.upgraded = (byte) _row["upgraded"];
            item.upgrade_max = (byte) _row["upgrade_max"];
            item.grade = (byte) _row["grade"];
            item.prefix = (short) _row["prefix"];
            item.suffix = (short) _row["suffix"];
            item.data = (string) _row["data"];
            item.options = ItemXmlFieldHelper.BuildItemOption((string) _row["option"]);
            item.sellingprice = (int) _row["sellingprice"];
            item.expiration = (int) _row["expiration"];
            item.varint = (int) _row["varint"];
            return item;
        }

        public static Item BuildQuestItem(DataRow _row)
        {
            Item item = new Item();
            item.quest = new Quest();
            item.storedtype = 4;
            item.id = (long) _row["itemID"];
            item.quest.id = (long) _row["quest"];
            item.pocket = (byte) _row["pocketid"];
            item.@class = (int) _row["class"];
            item.pos_x = (int) _row["pos_x"];
            item.pos_y = (int) _row["pos_y"];
            item.color_01 = (int) _row["color_01"];
            item.color_02 = (int) _row["color_02"];
            item.color_03 = (int) _row["color_03"];
            item.price = (int) _row["price"];
            item.bundle = (short) _row["bundle"];
            item.linked_pocket = (byte) _row["linked_pocket"];
            item.flag = (byte) _row["flag"];
            item.durability = (int) _row["durability"];
            item.sellingprice = (int) _row["sellingprice"];
            item.quest.templateid = (int) _row["templateid"];
            item.quest.complete = (byte) _row["complete"];
            item.quest.start_time = (long) _row["start_time"];
            item.quest.data = (string) _row["data"];
            item.quest.objectives = ItemXmlFieldHelper.BuildQuestObjective((string) _row["objective"]);
            item.expiration = (int) _row["expiration"];
            item.varint = (int) _row["varint"];
            item.data = string.Empty;
            return item;
        }

        public static Item BuildSmallItem(DataRow _row)
        {
            Item item = new Item();
            item.storedtype = 2;
            item.id = (long) _row["itemID"];
            item.pocket = (byte) _row["pocketid"];
            item.@class = (int) _row["class"];
            item.pos_x = (int) _row["pos_x"];
            item.pos_y = (int) _row["pos_y"];
            item.color_01 = (int) _row["color_01"];
            item.color_02 = (int) _row["color_02"];
            item.color_03 = (int) _row["color_03"];
            item.price = (int) _row["price"];
            item.bundle = (short) _row["bundle"];
            item.linked_pocket = (byte) _row["linked_pocket"];
            item.flag = (byte) _row["flag"];
            item.durability = (int) _row["durability"];
            item.sellingprice = (int) _row["sellingprice"];
            item.expiration = (int) _row["expiration"];
            item.varint = (int) _row["varint"];
            item.data = string.Empty;
            return item;
        }
    }
}

