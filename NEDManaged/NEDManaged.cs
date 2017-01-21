namespace NEDManaged
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using XMLDB3;

    public class NEDManaged
    {
        private static Type _dynamicType = CreateDynamicType(typeof(NEDManagedOriginal), "NEDManagedDynamic");
        public static NEDAddColumnDelegate NEDAddColumn;
        public static NEDCloseRowAccessorDelegate NEDCloseRowAccessor;
        private static NEDDecryptColumnDelegate NEDDecryptColumn;
        private static NEDEncryptColumnDelegate NEDEncryptColumn;
        public static NEDGetAuthorizationDelegate NEDGetAuthorization;
        private static NEDGetCachedStringDelegate NEDGetCachedString;
        public static NEDGetColumnSizeDelegate NEDGetColumnSize;
        public static NEDGetLastErrorCodeDelegate NEDGetLastErrorCode;
        private static NEDGetLastErrorMessageDelegate NEDGetLastErrorMessage;
        public static NEDGetRACachedDelegate NEDGetRACached;
        public static NEDGetRefCountDelegate NEDGetRefCount;
        public static NEDGetRowAccessorDelegate NEDGetRowAccessor;
        public static NEDInitDelegate NEDInit;
        public static NEDInit2Delegate NEDInit2;
        public static NEDSetCacheTimeoutDelegate NEDSetCacheTimeout;
        public static NEDSetLocalCacheTimeoutDelegate NEDSetLocalCacheTimeout;
        public static NEDTerminateDelegate NEDTerminate;

        static NEDManaged()
        {
            if (_dynamicType == null)
            {
                throw new Exception();
            }
            NEDInit = Delegate.CreateDelegate(typeof(NEDInitDelegate), _dynamicType.GetMethod("NEDInit")) as NEDInitDelegate;
            NEDInit2 = Delegate.CreateDelegate(typeof(NEDInit2Delegate), _dynamicType.GetMethod("NEDInit2")) as NEDInit2Delegate;
            NEDTerminate = Delegate.CreateDelegate(typeof(NEDTerminateDelegate), _dynamicType.GetMethod("NEDTerminate")) as NEDTerminateDelegate;
            NEDGetRefCount = Delegate.CreateDelegate(typeof(NEDGetRefCountDelegate), _dynamicType.GetMethod("NEDGetRefCount")) as NEDGetRefCountDelegate;
            NEDSetCacheTimeout = Delegate.CreateDelegate(typeof(NEDSetCacheTimeoutDelegate), _dynamicType.GetMethod("NEDSetCacheTimeout")) as NEDSetCacheTimeoutDelegate;
            NEDGetRACached = Delegate.CreateDelegate(typeof(NEDGetRACachedDelegate), _dynamicType.GetMethod("NEDGetRACached")) as NEDGetRACachedDelegate;
            NEDGetRowAccessor = Delegate.CreateDelegate(typeof(NEDGetRowAccessorDelegate), _dynamicType.GetMethod("NEDGetRowAccessor")) as NEDGetRowAccessorDelegate;
            NEDSetLocalCacheTimeout = Delegate.CreateDelegate(typeof(NEDSetLocalCacheTimeoutDelegate), _dynamicType.GetMethod("NEDSetLocalCacheTimeout")) as NEDSetLocalCacheTimeoutDelegate;
            NEDAddColumn = Delegate.CreateDelegate(typeof(NEDAddColumnDelegate), _dynamicType.GetMethod("NEDAddColumn")) as NEDAddColumnDelegate;
            NEDGetAuthorization = Delegate.CreateDelegate(typeof(NEDGetAuthorizationDelegate), _dynamicType.GetMethod("NEDGetAuthorization")) as NEDGetAuthorizationDelegate;
            NEDEncryptColumn = Delegate.CreateDelegate(typeof(NEDEncryptColumnDelegate), _dynamicType.GetMethod("NEDEncryptColumn")) as NEDEncryptColumnDelegate;
            NEDDecryptColumn = Delegate.CreateDelegate(typeof(NEDDecryptColumnDelegate), _dynamicType.GetMethod("NEDDecryptColumn")) as NEDDecryptColumnDelegate;
            NEDGetColumnSize = Delegate.CreateDelegate(typeof(NEDGetColumnSizeDelegate), _dynamicType.GetMethod("NEDGetColumnSize")) as NEDGetColumnSizeDelegate;
            NEDGetCachedString = Delegate.CreateDelegate(typeof(NEDGetCachedStringDelegate), _dynamicType.GetMethod("NEDGetCachedString")) as NEDGetCachedStringDelegate;
            NEDGetLastErrorCode = Delegate.CreateDelegate(typeof(NEDGetLastErrorCodeDelegate), _dynamicType.GetMethod("NEDGetLastErrorCode")) as NEDGetLastErrorCodeDelegate;
            NEDGetLastErrorMessage = Delegate.CreateDelegate(typeof(NEDGetLastErrorMessageDelegate), _dynamicType.GetMethod("NEDGetLastErrorMessage")) as NEDGetLastErrorMessageDelegate;
            NEDCloseRowAccessor = Delegate.CreateDelegate(typeof(NEDCloseRowAccessorDelegate), _dynamicType.GetMethod("NEDCloseRowAccessor")) as NEDCloseRowAccessorDelegate;
        }

        private NEDManaged()
        {
        }

        private static Type CreateDynamicType(Type originalType, string dynamicBaseName)
        {
            AssemblyName name = new AssemblyName();
            name.Name = dynamicBaseName + "Assembly";
            AssemblyBuilder builder = AppDomain.CurrentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.RunAndSave);
            TypeBuilder builder3 = builder.DefineDynamicModule(dynamicBaseName + "Module", dynamicBaseName + ".dll").DefineType(dynamicBaseName + "Type", TypeAttributes.AnsiClass);
            MethodInfo[] methods = originalType.GetMethods(BindingFlags.Public | BindingFlags.Static);
            for (int i = 0; i < methods.GetLength(0); i++)
            {
                MethodInfo info = methods[i];
                ParameterInfo[] parameters = info.GetParameters();
                int length = parameters.GetLength(0);
                Type[] parameterTypes = new Type[length];
                ParameterAttributes[] attributesArray = new ParameterAttributes[length];
                for (int j = 0; j < length; j++)
                {
                    parameterTypes[j] = parameters[j].ParameterType;
                    attributesArray[j] = parameters[j].Attributes;
                }
                MethodBuilder builder4 = builder3.DefinePInvokeMethod(info.Name, GetDynamicDllPath(), info.Attributes, info.CallingConvention, info.ReturnType, parameterTypes, CallingConvention.StdCall, CharSet.Unicode);
                for (int k = 0; k < length; k++)
                {
                    builder4.DefineParameter(k + 1, attributesArray[k], parameters[k].Name);
                }
                builder4.SetImplementationFlags(info.GetMethodImplementationFlags());
            }
            Type type = builder3.CreateType();
            builder.Save(dynamicBaseName + ".dll");
            return type;
        }

        public static unsafe int DecryptColumn(uint uRowAccssorID, string strColumnName, byte[] aInBuf, out string strOut)
        {
            int num;
            strOut = null;
            fixed (byte* numRef = aInBuf)
            {
                char* chPtr;
                num = NEDDecryptColumn(uRowAccssorID, strColumnName, numRef, (uint) aInBuf.Length, &chPtr);
                if ((num == 0) && (chPtr != null))
                {
                    strOut = new string(chPtr);
                }
            }
            return num;
        }

        public static unsafe int EncryptColumn(uint uRowAccessorID, string strColumnName, string strInput, out byte[] aOutBuf)
        {
            aOutBuf = null;
            int num = NEDGetColumnSize(uRowAccessorID, strColumnName);
            if (num > 0)
            {
                byte[] sourceArray = new byte[num];
                uint pOutLen = 0;
                fixed (byte* numRef = sourceArray)
                {
                    num = NEDEncryptColumn(uRowAccessorID, strColumnName, strInput, numRef, (uint) sourceArray.Length, &pOutLen);
                }
                if (num == 0)
                {
                    aOutBuf = new byte[pOutLen];
                    Array.Copy(sourceArray, aOutBuf, (long) pOutLen);
                }
            }
            return num;
        }

        public static unsafe int GetCachedString(uint uRowAccessorID, string strColumnName, ref string strOut)
        {
            char* chPtr;
            int num = NEDGetCachedString(uRowAccessorID, strColumnName, &chPtr);
            if (num == 0)
            {
                if (chPtr == null)
                {
                    strOut = null;
                    return num;
                }
                strOut = new string(chPtr);
            }
            return num;
        }

        private static string GetDynamicDllPath()
        {
            return Path.Combine(Environment.CurrentDirectory, ConfigManager.EncryptionDll);
        }

        public static unsafe string GetLastErrorMessage(uint uRowAccessorID)
        {
            return new string(NEDGetLastErrorMessage(uRowAccessorID));
        }

        public delegate int NEDAddColumnDelegate(uint uRowAccessorID, string strColumnName);

        public delegate void NEDCloseRowAccessorDelegate(uint uRowAccessorID);

        private unsafe delegate int NEDDecryptColumnDelegate(uint uRowAccessorID, string strColumnName, byte* pInData, uint uDataLen, char** pOutString);

        private unsafe delegate int NEDEncryptColumnDelegate(uint uRowAccessorID, string strColumnName, string strInput, byte* pOutData, uint uDataLen, uint* pOutLen);

        public enum NEDErrorCode
        {
            NED_ERROR_ACCESS_DENIED = -200,
            NED_ERROR_DECODEFAIL = -102,
            NED_ERROR_INITIALIZED_NEEDED = -10,
            NED_ERROR_INVALID_ARGUMENT = -2,
            NED_ERROR_INVALID_DATABLOCK = -202,
            NED_ERROR_INVALID_ROWACCESSORID = -1,
            NED_ERROR_NO_MORE_COLUMN = -3,
            NED_ERROR_SOAPERROR = -101,
            NED_ERROR_SOAPFAIL = -100,
            NED_ERROR_SUCCESS = 0,
            NED_ERROR_UNREGISTERED_COLUMN = -201
        }

        public delegate int NEDGetAuthorizationDelegate(uint uRowAccessorID);

        private unsafe delegate int NEDGetCachedStringDelegate(uint uRowAccessorID, string strColumnName, char** pOutString);

        public delegate int NEDGetColumnSizeDelegate(uint uRowAccessorID, string strColumnName);

        public delegate int NEDGetLastErrorCodeDelegate(uint uRowAccessorID);

        private unsafe delegate char* NEDGetLastErrorMessageDelegate(uint uRowAccessorID);

        public delegate int NEDGetRACachedDelegate(string strUserName, ulong uPasscode, string strRowID, string strColumnName1, string strColumnName2, string strColumnName3, string strColumnName4, string strColumnName5, string strColumnName6, string strColumnName7, string strColumnName8);

        public delegate uint NEDGetRefCountDelegate();

        public delegate int NEDGetRowAccessorDelegate(string strUserName, ulong uPasscode, string strRowID, string strColumnName1, string strColumnName2, string strColumnName3, string strColumnName4, string strColumnName5, string strColumnName6, string strColumnName7, string strColumnName8);

        public delegate bool NEDInit2Delegate(string strURL);

        public delegate bool NEDInitDelegate();

        public delegate int NEDSetCacheTimeoutDelegate(uint uTimeout);

        public delegate int NEDSetLocalCacheTimeoutDelegate(uint uRowAccessorID, uint uTimeout);

        public delegate void NEDTerminateDelegate(bool bForceTerminate);
    }
}

