namespace XMLDB3
{
    using System;
    using System.IO;

    public class FarmFileAdapter : FileAdapter, FarmAdapter
    {
        public REPLY_RESULT Expire(long _farmID, ref byte _errorCode)
        {
            Farm farm = this.Read(_farmID);
            if (farm == null)
            {
                return REPLY_RESULT.FAIL;
            }
            if ((farm.ownerAccount != null) && (farm.ownerAccount.Length != 0))
            {
                this.InitFarmData(farm);
                base.WriteToDB(farm, _farmID);
                return REPLY_RESULT.SUCCESS;
            }
            _errorCode = 0;
            return REPLY_RESULT.FAIL_EX;
        }

        public bool GetOwnerInfo(string _account, ref long _farmID, ref long _ownerCharID, ref string _ownerCharName)
        {
            string[] files = System.IO.Directory.GetFiles(base.Directory);
            if (files != null)
            {
                foreach (string str in files)
                {
                    Farm farm = (Farm) base.ReadFromDB(Path.GetFileNameWithoutExtension(str));
                    if ((farm != null) && (farm.ownerAccount == _account))
                    {
                        _farmID = farm.farmID;
                        _ownerCharID = farm.ownerCharID;
                        _ownerCharName = farm.ownerCharName;
                        return true;
                    }
                }
            }
            return false;
        }

        private void InitFarmData(Farm _farm)
        {
            _farm.ownerAccount = "";
            _farm.ownerCharID = 0L;
            _farm.ownerCharName = "";
            _farm.expireTime = 0L;
            _farm.crop = 0;
            _farm.plantTime = 0L;
            _farm.waterWork = 0;
            _farm.nutrientWork = 0;
            _farm.insectWork = 0;
            _farm.water = 0;
            _farm.nutrient = 0;
            _farm.insect = 0;
            _farm.growth = 0;
            _farm.currentWork = 0;
            _farm.workCompleteTime = 0L;
            _farm.todayWorkCount = 0;
            _farm.lastWorkTime = 0L;
        }

        public void Initialize(string _Argument)
        {
            base.Initialize(typeof(Farm), ConfigManager.GetFileDBPath("farm"), ".xml");
        }

        public REPLY_RESULT Lease(long _farmID, string _account, long _charID, string _charName, long _expireTime, ref byte _errorCode)
        {
            Farm farm = this.Read(_farmID);
            if (farm == null)
            {
                return REPLY_RESULT.FAIL;
            }
            if ((farm.ownerAccount == null) || (farm.ownerAccount.Length == 0))
            {
                farm.ownerAccount = _account;
                farm.ownerCharID = _charID;
                farm.ownerCharName = _charName;
                farm.expireTime = _expireTime;
                base.WriteToDB(farm, _farmID);
                return REPLY_RESULT.SUCCESS;
            }
            _errorCode = 0;
            return REPLY_RESULT.FAIL_EX;
        }

        public Farm Read(long _farmID)
        {
            if (base.IsExistData(_farmID))
            {
                return (Farm) base.ReadFromDB(_farmID.ToString());
            }
            Farm farm = new Farm();
            farm.farmID = _farmID;
            this.InitFarmData(farm);
            base.WriteToDB(farm, _farmID);
            if (base.IsExistData(_farmID))
            {
                return farm;
            }
            return null;
        }

        public REPLY_RESULT Update(Farm _farm, ref byte _errorCode)
        {
            Farm farm = this.Read(_farm.farmID);
            if (farm != null)
            {
                farm.crop = _farm.crop;
                farm.plantTime = _farm.plantTime;
                farm.waterWork = _farm.waterWork;
                farm.nutrientWork = _farm.nutrientWork;
                farm.insectWork = _farm.insectWork;
                farm.water = _farm.water;
                farm.nutrient = _farm.nutrient;
                farm.insect = _farm.insect;
                farm.growth = _farm.growth;
                farm.currentWork = _farm.currentWork;
                farm.workCompleteTime = _farm.workCompleteTime;
                farm.todayWorkCount = _farm.todayWorkCount;
                farm.lastWorkTime = _farm.lastWorkTime;
                base.WriteToDB(farm, farm.farmID);
                return REPLY_RESULT.SUCCESS;
            }
            return REPLY_RESULT.FAIL;
        }
    }
}

