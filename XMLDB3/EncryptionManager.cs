namespace XMLDB3
{
    using System;
    using System.Collections;

    public class EncryptionManager
    {
        private static Hashtable encryptions = new Hashtable();

        private EncryptionManager()
        {
        }

        public static void Close()
        {
            foreach (IEncryptionMethod method in encryptions.Values)
            {
                method.Close();
            }
        }

        public static string Decrypt(string _columnName, byte[] _in)
        {
            if (encryptions.ContainsKey(_columnName))
            {
                IEncryptionMethod method = encryptions[_columnName] as IEncryptionMethod;
                if (method != null)
                {
                    try
                    {
                        return method.DecryptColumn(_in);
                    }
                    catch (Exception exception)
                    {
                        ExceptionMonitor.ExceptionRaised(exception);
                    }
                }
            }
            return null;
        }

        public static byte[] Encrypt(string _columnName, string _in)
        {
            if (encryptions.ContainsKey(_columnName))
            {
                IEncryptionMethod method = encryptions[_columnName] as IEncryptionMethod;
                if (method != null)
                {
                    try
                    {
                        return method.EncryptColumn(_in);
                    }
                    catch (Exception exception)
                    {
                        ExceptionMonitor.ExceptionRaised(exception);
                    }
                }
            }
            return null;
        }

        public static bool Init(EncryptionColumn[] columns)
        {
            if (ConfigManager.IsEncryptionEnabled)
            {
                foreach (EncryptionColumn column in columns)
                {
                    IEncryptionMethod method = new NEDEncryption(column.name, column.passCode);
                    try
                    {
                        method.Init();
                        encryptions.Add(column.name, method);
                    }
                    catch (Exception exception)
                    {
                        if (Console.Out != null)
                        {
                            Console.WriteLine(exception.ToString());
                        }
                        ExceptionMonitor.ExceptionRaised(exception);
                        return false;
                    }
                }
            }
            return true;
        }
    }
}

