namespace XMLDB3
{
    using System;
    using System.Runtime.InteropServices;

    public class PetFileAdapter : FileAdapter, PetAdapter
    {
        public bool CreateEx(string _account, string _server, PetInfo _pet, AccountRefAdapter _accountref, WebSynchAdapter _websynch)
        {
            if (!base.IsExistData(_pet.id))
            {
                base.WriteToDB(_pet, _pet.id);
                ((AccountrefFileAdapter) _accountref).AddPetSlot(_account, _pet.id, _pet.name, _server, _pet.summon.remaintime, _pet.summon.lasttime, _pet.summon.expiretime);
                return true;
            }
            return false;
        }

        public bool Delete(string _name)
        {
            return base.DeleteDB(_name);
        }

        public bool DeleteEx(string _account, string _server, long _idpet, AccountRefAdapter _accountref, WebSynchAdapter _websynch)
        {
            ((AccountrefFileAdapter) _accountref).RemovePetSlot(_account, _idpet, _server);
            return true;
        }

        public bool DeleteItem(long _id, ItemList[] _list)
        {
            if (!base.IsExistData(_id))
            {
                return false;
            }
            PetInfo info = (PetInfo) base.ReadFromDB(_id);
            if (info == null)
            {
                return false;
            }
            if ((info.inventory != null) && (info.inventory.Count > 0))
            {
                foreach (ItemList list in _list)
                {
                    info.inventory.Remove(list.itemID);
                }
                this.Write(info);
            }
            return true;
        }

        public bool GetWriteCounter(long _id, out byte _counter)
        {
            if (base.IsExistData(_id))
            {
                PetInfo info = (PetInfo) base.ReadFromDB(_id);
                _counter = (info.data != null) ? info.data.writeCounter : ((byte) 0);
                return true;
            }
            _counter = 0;
            return false;
        }

        public void Initialize(string _argument)
        {
            base.Initialize(typeof(PetInfo), ConfigManager.GetFileDBPath("pet"), ".xml");
        }

        public bool IsUsableName(string _name)
        {
            return true;
        }

        public PetInfo Read(string _account, string _server, long _id, PetInfo _cache, AccountRefAdapter _accountref)
        {
            AccountrefPet pet = null;
            Accountref accountref = _accountref.Read(_account);
            if (accountref != null)
            {
                foreach (AccountrefPet pet2 in accountref.pet)
                {
                    if ((pet2.id == _id) && (pet2.server == _server))
                    {
                        pet = pet2;
                        break;
                    }
                }
                if (pet == null)
                {
                    return null;
                }
                if (base.IsExistData(_id))
                {
                    PetInfo info = (PetInfo) base.ReadFromDB(_id);
                    info.summon.remaintime = pet.remaintime;
                    info.summon.lasttime = pet.lasttime;
                    return info;
                }
            }
            return null;
        }

        public bool Write(PetInfo _data)
        {
            if (base.IsExistData(_data.id))
            {
                base.WriteToDB(_data, _data.id);
                return true;
            }
            return false;
        }

        public bool Write(string _account, string _server, byte _channelgroupid, PetInfo _pet, PetInfo _cache, AccountRefAdapter _accountref)
        {
            AccountrefPet pet = null;
            Accountref accountref = _accountref.Read(_account);
            if (accountref == null)
            {
                return false;
            }
            foreach (AccountrefPet pet2 in accountref.pet)
            {
                if ((pet2.id == _pet.id) && (pet2.server == _server))
                {
                    pet = pet2;
                    break;
                }
            }
            if (pet == null)
            {
                return false;
            }
            pet.remaintime = _pet.summon.remaintime;
            pet.lasttime = _pet.summon.lasttime;
            pet.groupID = _channelgroupid;
            ((AccountrefFileAdapter) _accountref).WriteToDB(accountref, _account);
            return this.Write(_pet);
        }
    }
}

