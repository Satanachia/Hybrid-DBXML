namespace XMLDB3
{
    using System;

    public interface AccountActivationAdapter
    {
        bool Create(AccountActivation _data);
        void Initialize(string _argument);
    }
}

