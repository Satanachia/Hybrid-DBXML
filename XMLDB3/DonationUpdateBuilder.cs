namespace XMLDB3
{
    using System;

    public class DonationUpdateBuilder
    {
        public static string Build(Character _new, Character _old)
        {
            if ((_new.donation == null) || (_old.donation == null))
            {
                return string.Empty;
            }
            string str = string.Empty;
            if (_new.donation.donationValue != _old.donation.donationValue)
            {
                str = str + ",[donationValue]=" + _new.donation.donationValue;
            }
            if (_new.donation.donationUpdate != _old.donation.donationUpdate)
            {
                str = str + ",[donationUpdate]=" + _new.donation.donationUpdate;
            }
            return str;
        }
    }
}

