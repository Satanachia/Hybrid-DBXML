namespace NEDManaged
{
    using System;
    using System.Runtime.InteropServices;

    public class NEDManagedOriginal
    {
        private const string s_DllPath = "NED_TEST.dll";

        [DllImport("NED_TEST.dll", CharSet=CharSet.Unicode)]
        public static extern int NEDAddColumn(uint uRowAccessorID, string strColumnName);
        [DllImport("NED_TEST.dll", CharSet=CharSet.Unicode)]
        public static extern void NEDCloseRowAccessor(uint uRowAccessorID);
        [DllImport("NED_TEST.dll", CharSet=CharSet.Unicode)]
        private static extern unsafe int NEDDecryptColumn(uint uRowAccessorID, string strColumnName, byte* pInData, uint uDataLen, char** pOutString);
        [DllImport("NED_TEST.dll", CharSet=CharSet.Unicode)]
        private static extern unsafe int NEDEncryptColumn(uint uRowAccessorID, string strColumnName, string strInput, byte* pOutData, uint uDataLen, uint* pOutLen);
        [DllImport("NED_TEST.dll", CharSet=CharSet.Unicode)]
        public static extern int NEDGetAuthorization(uint uRowAccessorID);
        [DllImport("NED_TEST.dll", CharSet=CharSet.Unicode)]
        private static extern unsafe int NEDGetCachedString(uint uRowAccessorID, string strColumnName, char** pOutString);
        [DllImport("NED_TEST.dll", CharSet=CharSet.Unicode)]
        public static extern int NEDGetColumnSize(uint uRowAccessorID, string strColumnName);
        [DllImport("NED_TEST.dll", CharSet=CharSet.Unicode)]
        public static extern int NEDGetLastErrorCode(uint uRowAccessorID);
        [DllImport("NED_TEST.dll", CharSet=CharSet.Unicode)]
        private static extern unsafe char* NEDGetLastErrorMessage(uint uRowAccessorID);
        [DllImport("NED_TEST.dll", CharSet=CharSet.Unicode)]
        public static extern uint NEDGetModuleVersion();
        [DllImport("NED_TEST.dll", CharSet=CharSet.Unicode)]
        public static extern int NEDGetRACached(string strUserName, ulong uPasscode, string strRowID, string strColumnName1, string strColumnName2, string strColumnName3, string strColumnName4, string strColumnName5, string strColumnName6, string strColumnName7, string strColumnName8);
        [DllImport("NED_TEST.dll", CharSet=CharSet.Unicode)]
        public static extern uint NEDGetRefCount();
        [DllImport("NED_TEST.dll", CharSet=CharSet.Unicode)]
        public static extern int NEDGetRowAccessor(string strUserName, ulong uPasscode, string strRowID, string strColumnName1, string strColumnName2, string strColumnName3, string strColumnName4, string strColumnName5, string strColumnName6, string strColumnName7, string strColumnName8);
        [DllImport("NED_TEST.dll", CharSet=CharSet.Unicode)]
        private static extern unsafe char* NEDGetSoapURL();
        [DllImport("NED_TEST.dll", CharSet=CharSet.Unicode)]
        public static extern bool NEDInit();
        [DllImport("NED_TEST.dll", CharSet=CharSet.Unicode)]
        public static extern bool NEDInit2(string strURL);
        [DllImport("NED_TEST.dll", CharSet=CharSet.Unicode)]
        public static extern int NEDSetCacheTimeout(uint uTimeout);
        [DllImport("NED_TEST.dll", CharSet=CharSet.Unicode)]
        public static extern int NEDSetLocalCacheTimeout(uint uRowAccessorID, uint uTimeout);
        [DllImport("NED_TEST.dll", CharSet=CharSet.Unicode)]
        public static extern int NEDSetLockTimeout(uint uTimeout);
        [DllImport("NED_TEST.dll", CharSet=CharSet.Unicode)]
        public static extern void NEDSetLogDirectory(string strLogDir);
        [DllImport("NED_TEST.dll", CharSet=CharSet.Unicode)]
        private static extern void NEDSetSoapURL(string strURL);
        [DllImport("NED_TEST.dll", CharSet=CharSet.Unicode)]
        public static extern void NEDTerminate(bool bForceTerminate);
    }
}

