namespace XMLDB3
{
    using System;

    public class UpdateUtility
    {
        public static string BuildDateTime(DateTime _data)
        {
            return string.Concat(new object[] { "'", _data.Year, '-', _data.Month, '-', _data.Day, ' ', _data.Hour, ':', _data.Minute, ":", _data.Second, ".", _data.Millisecond, "'" });
        }

        public static string BuildString(string _data)
        {
            if (_data != null)
            {
                _data = _data.Replace("'", "''");
                return ("'" + _data + "'");
            }
            return "''";
        }
    }
}

