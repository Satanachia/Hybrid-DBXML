namespace XMLDB3
{
    using System;
    using System.Data;

    public class DonationObjectBuilder
    {
        public static CharacterDonation Build(DataRow _character_row)
        {
            CharacterDonation donation = new CharacterDonation();
            donation.donationValue = (int) _character_row["donationValue"];
            donation.donationUpdate = (long) _character_row["donationUpdate"];
            return donation;
        }
    }
}

