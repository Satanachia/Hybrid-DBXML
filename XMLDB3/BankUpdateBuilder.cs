namespace XMLDB3
{
    using System;

    public class BankUpdateBuilder
    {
        public static string Build(Bank _bank, BankCache _cache)
        {
            if (!(_bank.account.ToLower() == _cache.Account.ToLower()))
            {
                throw new Exception(string.Format("Bank account [{0}] is different from {1}", _bank.account, _cache.Account));
            }
            string str = string.Empty;
            if (_bank.data.deposit != _cache.bank.deposit)
            {
                str = str + ",[deposit]=" + _bank.data.deposit;
            }
            if (_bank.data.password != _cache.bank.password)
            {
                str = str + ",[password]='" + _bank.data.password + "'";
            }
            for (int i = 0; i < 3; i++)
            {
                BankRace race = (BankRace) ((byte) i);
                int wealth = _bank.GetWealth(race);
                if (_bank.IsBankLoaded(race) && (wealth != _cache.GetWealth(race)))
                {
                    object obj2 = str;
                    str = string.Concat(new object[] { obj2, ",[", BankSqlAdapter.GetWealthColumn(race), "]=", wealth });
                    _cache.SetWealth(race, wealth);
                }
            }
            _cache.bank = _bank.data;
            return ("update bank set [update_time]=getdate()" + str + (" where account=" + UpdateUtility.BuildString(_bank.account) + "\n"));
        }
    }
}

