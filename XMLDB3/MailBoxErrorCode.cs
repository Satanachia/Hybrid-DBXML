namespace XMLDB3
{
    using System;

    internal enum MailBoxErrorCode : byte
    {
        ErrorCharacterIsPet = 5,
        ErrorCharacterNotExist = 4,
        ErrorMailNotExist = 3,
        ErrorReceiverMailBoxFull = 1,
        ErrorSenderMailBoxFull = 2,
        ErrorUnknown = 0
    }
}

