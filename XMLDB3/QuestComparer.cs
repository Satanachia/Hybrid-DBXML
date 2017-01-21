namespace XMLDB3
{
    using System;
    using System.Collections;

    public class QuestComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            if ((x != null) && (y != null))
            {
                CharacterPrivateRegistered registered = x as CharacterPrivateRegistered;
                CharacterPrivateRegistered registered2 = y as CharacterPrivateRegistered;
                if (registered == null)
                {
                    throw new ArgumentException("Argument Type is not CharacterPrivateRegistered", "x");
                }
                if (registered2 == null)
                {
                    throw new ArgumentException("Argument Type is not CharacterPrivateRegistered", "y");
                }
                if (registered.id == registered2.id)
                {
                    return 0;
                }
                if (registered.id <= registered2.id)
                {
                    return -1;
                }
                return 1;
            }
            if ((x == null) && (y == null))
            {
                return 0;
            }
            if (x != null)
            {
                return 1;
            }
            return -1;
        }
    }
}

