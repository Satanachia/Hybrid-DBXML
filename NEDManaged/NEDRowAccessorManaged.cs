namespace NEDManaged
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal class NEDRowAccessorManaged : IDisposable
    {
        private int m_uAccessorID;

        private NEDRowAccessorManaged()
        {
            this.m_uAccessorID = 0;
        }

        public NEDRowAccessorManaged(string strUserName, ulong uPasscode, string strRowID)
        {
            this.m_uAccessorID = 0;
            this.m_uAccessorID = NEDManaged.NEDGetRowAccessor(strUserName, uPasscode, strRowID, null, null, null, null, null, null, null, null);
        }

        public NEDRowAccessorManaged(string strUserName, ulong uPasscode, string strRowID, string strColumnName1)
        {
            this.m_uAccessorID = 0;
            this.m_uAccessorID = NEDManaged.NEDGetRowAccessor(strUserName, uPasscode, strRowID, strColumnName1, null, null, null, null, null, null, null);
        }

        public NEDRowAccessorManaged(string strUserName, ulong uPasscode, string strRowID, string strColumnName1, string strColumnName2)
        {
            this.m_uAccessorID = 0;
            this.m_uAccessorID = NEDManaged.NEDGetRowAccessor(strUserName, uPasscode, strRowID, strColumnName1, strColumnName2, null, null, null, null, null, null);
        }

        public NEDRowAccessorManaged(string strUserName, ulong uPasscode, string strRowID, string strColumnName1, string strColumnName2, string strColumnName3)
        {
            this.m_uAccessorID = 0;
            this.m_uAccessorID = NEDManaged.NEDGetRowAccessor(strUserName, uPasscode, strRowID, strColumnName1, strColumnName2, strColumnName3, null, null, null, null, null);
        }

        public NEDRowAccessorManaged(string strUserName, ulong uPasscode, string strRowID, string strColumnName1, string strColumnName2, string strColumnName3, string strColumnName4)
        {
            this.m_uAccessorID = 0;
            this.m_uAccessorID = NEDManaged.NEDGetRowAccessor(strUserName, uPasscode, strRowID, strColumnName1, strColumnName2, strColumnName3, strColumnName4, null, null, null, null);
        }

        public NEDRowAccessorManaged(string strUserName, ulong uPasscode, string strRowID, string strColumnName1, string strColumnName2, string strColumnName3, string strColumnName4, string strColumnName5)
        {
            this.m_uAccessorID = 0;
            this.m_uAccessorID = NEDManaged.NEDGetRowAccessor(strUserName, uPasscode, strRowID, strColumnName1, strColumnName2, strColumnName3, strColumnName4, strColumnName5, null, null, null);
        }

        public NEDRowAccessorManaged(string strUserName, ulong uPasscode, string strRowID, string strColumnName1, string strColumnName2, string strColumnName3, string strColumnName4, string strColumnName5, string strColumnName6)
        {
            this.m_uAccessorID = 0;
            this.m_uAccessorID = NEDManaged.NEDGetRowAccessor(strUserName, uPasscode, strRowID, strColumnName1, strColumnName2, strColumnName3, strColumnName4, strColumnName5, strColumnName6, null, null);
        }

        public NEDRowAccessorManaged(string strUserName, ulong uPasscode, string strRowID, string strColumnName1, string strColumnName2, string strColumnName3, string strColumnName4, string strColumnName5, string strColumnName6, string strColumnName7)
        {
            this.m_uAccessorID = 0;
            this.m_uAccessorID = NEDManaged.NEDGetRowAccessor(strUserName, uPasscode, strRowID, strColumnName1, strColumnName2, strColumnName3, strColumnName4, strColumnName5, strColumnName6, strColumnName7, null);
        }

        public NEDRowAccessorManaged(string strUserName, ulong uPasscode, string strRowID, string strColumnName1, string strColumnName2, string strColumnName3, string strColumnName4, string strColumnName5, string strColumnName6, string strColumnName7, string strColumnName8)
        {
            this.m_uAccessorID = 0;
            this.m_uAccessorID = NEDManaged.NEDGetRowAccessor(strUserName, uPasscode, strRowID, strColumnName1, strColumnName2, strColumnName3, strColumnName4, strColumnName5, strColumnName6, strColumnName7, strColumnName8);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public int AddColumn(string strColumnName)
        {
            if (this.m_uAccessorID > 0)
            {
                return NEDManaged.NEDAddColumn((uint) this.m_uAccessorID, strColumnName);
            }
            return -1;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public int DecryptColumn(string strColumnName, byte[] aInBuf, out string strOut)
        {
            strOut = null;
            if (this.m_uAccessorID > 0)
            {
                return NEDManaged.DecryptColumn((uint) this.m_uAccessorID, strColumnName, aInBuf, out strOut);
            }
            return -1;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (this.m_uAccessorID > 0)
            {
                NEDManaged.NEDCloseRowAccessor((uint) this.m_uAccessorID);
                this.m_uAccessorID = 0;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public int EncryptColumn(string strColumnName, string strInput, out byte[] aOutBuf)
        {
            aOutBuf = null;
            if (this.m_uAccessorID > 0)
            {
                return NEDManaged.EncryptColumn((uint) this.m_uAccessorID, strColumnName, strInput, out aOutBuf);
            }
            return -1;
        }

        ~NEDRowAccessorManaged()
        {
            this.Dispose(false);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public int GetAuthorization()
        {
            if (this.m_uAccessorID > 0)
            {
                return NEDManaged.NEDGetAuthorization((uint) this.m_uAccessorID);
            }
            return -1;
        }

        public static NEDRowAccessorManaged GetCachedRA(string strUserName, ulong uPasscode, string strRowID, string strColumnName1)
        {
            NEDRowAccessorManaged managed = new NEDRowAccessorManaged();
            managed.m_uAccessorID = NEDManaged.NEDGetRACached(strUserName, uPasscode, strRowID, strColumnName1, null, null, null, null, null, null, null);
            return managed;
        }

        public static NEDRowAccessorManaged GetCachedRA(string strUserName, ulong uPasscode, string strRowID, string strColumnName1, string strColumnName2)
        {
            NEDRowAccessorManaged managed = new NEDRowAccessorManaged();
            managed.m_uAccessorID = NEDManaged.NEDGetRACached(strUserName, uPasscode, strRowID, strColumnName1, strColumnName2, null, null, null, null, null, null);
            return managed;
        }

        public static NEDRowAccessorManaged GetCachedRA(string strUserName, ulong uPasscode, string strRowID, string strColumnName1, string strColumnName2, string strColumnName3)
        {
            NEDRowAccessorManaged managed = new NEDRowAccessorManaged();
            managed.m_uAccessorID = NEDManaged.NEDGetRACached(strUserName, uPasscode, strRowID, strColumnName1, strColumnName2, strColumnName3, null, null, null, null, null);
            return managed;
        }

        public static NEDRowAccessorManaged GetCachedRA(string strUserName, ulong uPasscode, string strRowID, string strColumnName1, string strColumnName2, string strColumnName3, string strColumnName4)
        {
            NEDRowAccessorManaged managed = new NEDRowAccessorManaged();
            managed.m_uAccessorID = NEDManaged.NEDGetRACached(strUserName, uPasscode, strRowID, strColumnName1, strColumnName2, strColumnName3, strColumnName4, null, null, null, null);
            return managed;
        }

        public static NEDRowAccessorManaged GetCachedRA(string strUserName, ulong uPasscode, string strRowID, string strColumnName1, string strColumnName2, string strColumnName3, string strColumnName4, string strColumnName5)
        {
            NEDRowAccessorManaged managed = new NEDRowAccessorManaged();
            managed.m_uAccessorID = NEDManaged.NEDGetRACached(strUserName, uPasscode, strRowID, strColumnName1, strColumnName2, strColumnName3, strColumnName4, strColumnName5, null, null, null);
            return managed;
        }

        public static NEDRowAccessorManaged GetCachedRA(string strUserName, ulong uPasscode, string strRowID, string strColumnName1, string strColumnName2, string strColumnName3, string strColumnName4, string strColumnName5, string strColumnName6)
        {
            NEDRowAccessorManaged managed = new NEDRowAccessorManaged();
            managed.m_uAccessorID = NEDManaged.NEDGetRACached(strUserName, uPasscode, strRowID, strColumnName1, strColumnName2, strColumnName3, strColumnName4, strColumnName5, strColumnName6, null, null);
            return managed;
        }

        public static NEDRowAccessorManaged GetCachedRA(string strUserName, ulong uPasscode, string strRowID, string strColumnName1, string strColumnName2, string strColumnName3, string strColumnName4, string strColumnName5, string strColumnName6, string strColumnName7)
        {
            NEDRowAccessorManaged managed = new NEDRowAccessorManaged();
            managed.m_uAccessorID = NEDManaged.NEDGetRACached(strUserName, uPasscode, strRowID, strColumnName1, strColumnName2, strColumnName3, strColumnName4, strColumnName5, strColumnName6, strColumnName7, null);
            return managed;
        }

        public static NEDRowAccessorManaged GetCachedRA(string strUserName, ulong uPasscode, string strRowID, string strColumnName1, string strColumnName2, string strColumnName3, string strColumnName4, string strColumnName5, string strColumnName6, string strColumnName7, string strColumnName8)
        {
            NEDRowAccessorManaged managed = new NEDRowAccessorManaged();
            managed.m_uAccessorID = NEDManaged.NEDGetRACached(strUserName, uPasscode, strRowID, strColumnName1, strColumnName2, strColumnName3, strColumnName4, strColumnName5, strColumnName6, strColumnName7, strColumnName8);
            return managed;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public string GetCachedString(string strColumnName)
        {
            string strOut = null;
            if (this.m_uAccessorID > 0)
            {
                NEDManaged.GetCachedString((uint) this.m_uAccessorID, strColumnName, ref strOut);
                return strOut;
            }
            return string.Format("Invalid RowAccessor ID : {0}", this.m_uAccessorID);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public int GetColumnSize(string strColumnName)
        {
            if (this.m_uAccessorID > 0)
            {
                return NEDManaged.NEDGetColumnSize((uint) this.m_uAccessorID, strColumnName);
            }
            return -1;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public int GetLastErrorCode()
        {
            if (this.m_uAccessorID > 0)
            {
                return NEDManaged.NEDGetLastErrorCode((uint) this.m_uAccessorID);
            }
            return -1;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public string GetLastErrorMessage()
        {
            if (this.m_uAccessorID > 0)
            {
                return NEDManaged.GetLastErrorMessage((uint) this.m_uAccessorID);
            }
            return string.Format("Invalid Row Accessor ID : {0}", this.m_uAccessorID);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public int SetCacheTimeout(uint uTimeout)
        {
            if (this.m_uAccessorID > 0)
            {
                return NEDManaged.NEDSetLocalCacheTimeout((uint) this.m_uAccessorID, uTimeout);
            }
            return -1;
        }
    }
}

