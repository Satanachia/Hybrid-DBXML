namespace XMLDB3
{
    using System;

    public interface BankAdapter
    {
        void Initialize(string _argument);
        Bank Read(string _id, string _charName, BankRace _race, BankCache _cache);
        bool Write(string _charName, Bank _data, BankCache _cache);
        bool WriteEx(Bank _bank, CharacterInfo _character, BankCache _bankCache, CharacterInfo _charCache, CharacterAdapter _charAdapter);
    }
}

