namespace XMLDB3
{
    using System;
    using System.Text;

    public class TitleUpdateBuilder
    {
        public static string Build(Character _new, Character _old)
        {
            if ((_new.titles != null) && (_old.titles != null))
            {
                string str = BuildTitleXmlData(_new.titles);
                string str2 = BuildTitleXmlData(_old.titles);
                if (str != str2)
                {
                    return (",[title]=" + UpdateUtility.BuildString(str));
                }
            }
            return string.Empty;
        }

        private static string BuildTitleXmlData(CharacterTitles _title)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("<titles selected=\"{0}\" appliedtime=\"{1}\" option=\"{2}\">", _title.selected, _title.appliedtime, _title.option);
            if (_title.title != null)
            {
                foreach (CharacterTitlesTitle title in _title.title)
                {
                    builder.AppendFormat("<title id=\"{0}\" state=\"{1}\" validtime=\"{2}\"/>", title.id, title.state, title.validtime);
                }
            }
            builder.Append("</titles>");
            return builder.ToString();
        }
    }
}

