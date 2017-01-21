namespace XMLDB3
{
    using System;

    public class WebSynchSqlAdapter : SqlAdapter, WebSynchAdapter
    {
        public void Initialize(string _argument)
        {
            this.Initialize(typeof(int), _argument);
        }
    }
}

