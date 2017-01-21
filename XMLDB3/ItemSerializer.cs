namespace XMLDB3
{
    using Mabinogi;
    using System;
    using System.IO;
    using System.Xml.Serialization;

    public class ItemSerializer
    {
        public static Message Deserialize(Item _item, Message _message)
        {
            if (_item == null)
            {
                _item = new Item();
            }
            _message.WriteS64(_item.id);
            _message.WriteU8(_item.storedtype);
            _message.WriteU8(_item.pocket);
            _message.WriteS32(_item.@class);
            _message.WriteS32(_item.pos_x);
            _message.WriteS32(_item.pos_y);
            _message.WriteS32(_item.color_01);
            _message.WriteS32(_item.color_02);
            _message.WriteS32(_item.color_03);
            _message.WriteS32(_item.price);
            _message.WriteS32(_item.sellingprice);
            _message.WriteS16(_item.bundle);
            _message.WriteU8(_item.linked_pocket);
            _message.WriteS32(_item.figure);
            _message.WriteU8(_item.flag);
            _message.WriteS32(_item.durability);
            _message.WriteS32(_item.durability_max);
            _message.WriteS32(_item.origin_durability_max);
            _message.WriteS16(_item.attack_min);
            _message.WriteS16(_item.attack_max);
            _message.WriteS16(_item.wattack_min);
            _message.WriteS16(_item.wattack_max);
            _message.WriteU8(_item.balance);
            _message.WriteU8(_item.critical);
            _message.WriteS32(_item.defence);
            _message.WriteS16(_item.protect);
            _message.WriteS16(_item.effective_range);
            _message.WriteU8(_item.attack_speed);
            _message.WriteU8(_item.down_hit_count);
            _message.WriteS16(_item.experience);
            _message.WriteU8(_item.exp_point);
            _message.WriteU8(_item.upgraded);
            _message.WriteU8(_item.upgrade_max);
            _message.WriteU8(_item.grade);
            _message.WriteS16(_item.prefix);
            _message.WriteS16(_item.suffix);
            _message.WriteString(_item.data);
            if (_item.options != null)
            {
                _message.WriteU32((uint) _item.options.Length);
                foreach (ItemOption option in _item.options)
                {
                    _message.WriteU8(option.type);
                    _message.WriteS32(option.flag);
                    _message.WriteS16(option.execute);
                    _message.WriteS32(option.execdata);
                    _message.WriteU8(option.open);
                    _message.WriteS32(option.opendata);
                    _message.WriteU8(option.enable);
                    _message.WriteS32(option.enabledata);
                }
            }
            else
            {
                _message.WriteU32(0);
            }
            _message.WriteS32(_item.expiration);
            _message.WriteS32(_item.varint);
            if (_item.Type == Item.StoredType.IstEgo)
            {
                WriteEgoToMessage(_item.ego, _message);
            }
            QuestSerializer.Deserialize(_item.quest, _message);
            return _message;
        }

        private static Ego ReadEgoFromMessage(Message _message)
        {
            Ego ego = new Ego();
            ego.egoName = _message.ReadString();
            ego.egoType = _message.ReadU8();
            ego.egoDesire = _message.ReadU8();
            ego.egoSocialLevel = _message.ReadU8();
            ego.egoSocialExp = _message.ReadS32();
            ego.egoStrLevel = _message.ReadU8();
            ego.egoStrExp = _message.ReadS32();
            ego.egoIntLevel = _message.ReadU8();
            ego.egoIntExp = _message.ReadS32();
            ego.egoDexLevel = _message.ReadU8();
            ego.egoDexExp = _message.ReadS32();
            ego.egoWillLevel = _message.ReadU8();
            ego.egoWillExp = _message.ReadS32();
            ego.egoLuckLevel = _message.ReadU8();
            ego.egoLuckExp = _message.ReadS32();
            ego.egoSkillCount = _message.ReadU8();
            ego.egoSkillGauge = _message.ReadS32();
            ego.egoSkillCoolTime = _message.ReadS64();
            return ego;
        }

        public static Item Serialize(Message _message)
        {
            Item o = new Item();
            o.id = _message.ReadS64();
            o.storedtype = _message.ReadU8();
            if ((o.storedtype <= 0) || (o.storedtype >= 6))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Item));
                StringWriter writer = new StringWriter();
                serializer.Serialize((TextWriter) writer, o);
                MailSender.Send("Item 의 storedtype이 잘못되었습니다", writer.ToString());
                throw new Exception("Invalid item storedtype\n");
            }
            o.pocket = _message.ReadU8();
            o.@class = _message.ReadS32();
            o.pos_x = _message.ReadS32();
            o.pos_y = _message.ReadS32();
            o.color_01 = _message.ReadS32();
            o.color_02 = _message.ReadS32();
            o.color_03 = _message.ReadS32();
            o.price = _message.ReadS32();
            o.sellingprice = _message.ReadS32();
            o.bundle = _message.ReadS16();
            o.linked_pocket = _message.ReadU8();
            o.figure = _message.ReadS32();
            o.flag = _message.ReadU8();
            o.durability = _message.ReadS32();
            o.durability_max = _message.ReadS32();
            o.origin_durability_max = _message.ReadS32();
            o.attack_min = _message.ReadS16();
            o.attack_max = _message.ReadS16();
            o.wattack_min = _message.ReadS16();
            o.wattack_max = _message.ReadS16();
            o.balance = _message.ReadU8();
            o.critical = _message.ReadU8();
            o.defence = _message.ReadS32();
            o.protect = _message.ReadS16();
            o.effective_range = _message.ReadS16();
            o.attack_speed = _message.ReadU8();
            o.down_hit_count = _message.ReadU8();
            o.experience = _message.ReadS16();
            o.exp_point = _message.ReadU8();
            o.upgraded = _message.ReadU8();
            o.upgrade_max = _message.ReadU8();
            o.grade = _message.ReadU8();
            o.prefix = _message.ReadS16();
            o.suffix = _message.ReadS16();
            o.data = _message.ReadString();
            uint num = _message.ReadU32();
            o.options = new ItemOption[num];
            for (int i = 0; i < num; i++)
            {
                ItemOption option = o.options[i] = new ItemOption();
                option.type = _message.ReadU8();
                option.flag = _message.ReadS32();
                option.execute = _message.ReadS16();
                option.execdata = _message.ReadS32();
                option.open = _message.ReadU8();
                option.opendata = _message.ReadS32();
                option.enable = _message.ReadU8();
                option.enabledata = _message.ReadS32();
            }
            o.expiration = _message.ReadS32();
            o.varint = _message.ReadS32();
            if (o.Type == Item.StoredType.IstEgo)
            {
                o.ego = ReadEgoFromMessage(_message);
            }
            else
            {
                o.ego = null;
            }
            o.quest = QuestSerializer.Serialize(_message);
            return o;
        }

        private static void WriteEgoToMessage(Ego _item, Message _message)
        {
            if (_item == null)
            {
                _item = new Ego();
            }
            _message.WriteString(_item.egoName);
            _message.WriteU8(_item.egoType);
            _message.WriteU8(_item.egoDesire);
            _message.WriteU8(_item.egoSocialLevel);
            _message.WriteS32(_item.egoSocialExp);
            _message.WriteU8(_item.egoStrLevel);
            _message.WriteS32(_item.egoStrExp);
            _message.WriteU8(_item.egoIntLevel);
            _message.WriteS32(_item.egoIntExp);
            _message.WriteU8(_item.egoDexLevel);
            _message.WriteS32(_item.egoDexExp);
            _message.WriteU8(_item.egoWillLevel);
            _message.WriteS32(_item.egoWillExp);
            _message.WriteU8(_item.egoLuckLevel);
            _message.WriteS32(_item.egoLuckExp);
            _message.WriteU8(_item.egoSkillCount);
            _message.WriteS32(_item.egoSkillGauge);
            _message.WriteS64(_item.egoSkillCoolTime);
        }
    }
}

