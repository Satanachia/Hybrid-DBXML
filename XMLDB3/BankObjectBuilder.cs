namespace XMLDB3
{
    using System;
    using System.Data;

    public class BankObjectBuilder
    {
        public static Bank Build(DataSet ds)
        {
            DataTable table = ds.Tables["bank"];
            if (table == null)
            {
                throw new Exception("은행 테이블이 없습니다.");
            }
            if (table.Rows.Count != 1)
            {
                throw new Exception("은행 테이블 열이 하나 이상입니다.");
            }
            Bank bank = new Bank();
            bank.account = (string) table.Rows[0]["account"];
            bank.data = new BankData();
            bank.data.deposit = (int) table.Rows[0]["deposit"];
            if (table.Rows[0].IsNull("password"))
            {
                bank.data.password = string.Empty;
            }
            else
            {
                bank.data.password = (string) table.Rows[0]["password"];
            }
            for (int i = 0; i < 3; i++)
            {
                BankRace race = (BankRace) ((byte) i);
                bank.SetWealth(race, (int) table.Rows[0][BankSqlAdapter.GetWealthColumn(race)]);
            }
            bank.data.updatetime = (DateTime) table.Rows[0]["update_time"];
            return bank;
        }
    }
}

