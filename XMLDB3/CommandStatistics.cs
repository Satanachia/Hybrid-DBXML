namespace XMLDB3
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Data.SqlClient;
    using System.Timers;

    public class CommandStatistics
    {
        private static bool active = false;
        private static Hashtable commands;
        private static string connStr = ConfigManager.StatisticsConnection;
        private static Hashtable sessions;
        private static Timer timer;

        static CommandStatistics()
        {
            int statisticsPeriod = ConfigManager.StatisticsPeriod;
            if (((connStr != null) && (connStr != string.Empty)) && (statisticsPeriod != -1))
            {
                InitCommands();
                InitSessions();
                timer = new Timer((double) statisticsPeriod);
                timer.Elapsed += new ElapsedEventHandler(CommandStatistics.WriteToDB);
                timer.Enabled = true;
                timer.AutoReset = true;
                timer.Start();
                active = true;
            }
        }

        private static void InitCommands()
        {
            commands = new Hashtable();
            commands.Add(CommandType.cctCharacterRead, new CommandStat());
            commands.Add(CommandType.cctCharacterWrite, new CommandStat());
            commands.Add(CommandType.cctPetRead, new CommandStat());
            commands.Add(CommandType.cctPetWrite, new CommandStat());
            commands.Add(CommandType.cctBankRead, new CommandStat());
            commands.Add(CommandType.cctBankWrite, new CommandStat());
            commands.Add(CommandType.cctBankWriteEx, new CommandStat());
            commands.Add(CommandType.cctCharItemDelete, new CommandStat());
        }

        private static void InitSessions()
        {
            sessions = new Hashtable();
            sessions.Add("CharacterReadCommand", new CommandStat());
            sessions.Add("CharacterWriteCommand", new CommandStat());
            sessions.Add("PetReadCommand", new CommandStat());
            sessions.Add("PetWriteCommand", new CommandStat());
            sessions.Add("BankReadCommand", new CommandStat());
            sessions.Add("BankUpdateCommand", new CommandStat());
            sessions.Add("BankUpdateExCommand", new CommandStat());
        }

        public static void RegisterCommandTime(CommandType _type, long _time)
        {
            if (active)
            {
                CommandStat stat = (CommandStat) commands[_type];
                if (stat != null)
                {
                    lock (stat)
                    {
                        stat.Update(_time);
                    }
                }
            }
        }

        public static void RegisterSessionTime(string _command, long _time)
        {
            if (active)
            {
                CommandStat stat = (CommandStat) sessions[_command];
                if (stat != null)
                {
                    lock (stat)
                    {
                        stat.Update(_time);
                    }
                }
            }
        }

        private static void WriteToDB(object sender, ElapsedEventArgs e)
        {
            Hashtable commands;
            Hashtable sessions;
            Hashtable hashtable3;
            lock ((hashtable3 = CommandStatistics.commands))
            {
                commands = CommandStatistics.commands;
                InitCommands();
            }
            lock ((hashtable3 = CommandStatistics.sessions))
            {
                sessions = CommandStatistics.sessions;
                InitSessions();
            }
            SqlConnection connection = new SqlConnection(connStr);
            string cmdText = string.Empty;
            try
            {
                CacheStatistics statistics = ObjectCache.Character.Statistics;
                object obj2 = cmdText;
                cmdText = string.Concat(new object[] { obj2, "exec InsertCacheHit  @name=", UpdateUtility.BuildString(statistics.Name), ",@total=", statistics.Total, ",@hit=", statistics.Hit, ",@size=", statistics.Size, "\n" });
                statistics = ObjectCache.Bank.Statistics;
                obj2 = cmdText;
                cmdText = string.Concat(new object[] { obj2, "exec InsertCacheHit  @name=", UpdateUtility.BuildString(statistics.Name), ",@total=", statistics.Total, ",@hit=", statistics.Hit, ",@size=", statistics.Size, "\n" });
                IDictionaryEnumerator enumerator = commands.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    obj2 = cmdText;
                    cmdText = string.Concat(new object[] { obj2, "exec InsertCommandStat  @command=", UpdateUtility.BuildString(enumerator.Key.ToString()), ",@avg=", ((CommandStat) enumerator.Value).avg, ",@min=", ((CommandStat) enumerator.Value).min, ",@max=", ((CommandStat) enumerator.Value).max, ",@count=", ((CommandStat) enumerator.Value).count, "\n" });
                }
                enumerator = sessions.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    obj2 = cmdText;
                    cmdText = string.Concat(new object[] { obj2, "exec InsertCommandStat  @command=", UpdateUtility.BuildString(enumerator.Key.ToString()), ",@avg=", ((CommandStat) enumerator.Value).avg, ",@min=", ((CommandStat) enumerator.Value).min, ",@max=", ((CommandStat) enumerator.Value).max, ",@count=", ((CommandStat) enumerator.Value).count, "\n" });
                }
                connection.Open();
                SqlCommand command = new SqlCommand(cmdText, connection);
                command.CommandType = System.Data.CommandType.Text;
                command.ExecuteNonQuery();
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, cmdText);
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, cmdText);
            }
            finally
            {
                connection.Close();
            }
        }

        private class CommandStat
        {
            public float avg = 0f;
            public int count = 0;
            public long max = 0L;
            public long min = 0L;

            public void Update(long _time)
            {
                this.avg = ((this.avg * this.count) / ((float) (this.count + 1))) + (((float) _time) / ((float) (this.count + 1)));
                this.count++;
                if (_time > this.max)
                {
                    this.max = _time;
                }
                if ((this.min == 0L) || (_time < this.min))
                {
                    this.min = _time;
                }
            }
        }

        public enum CommandType
        {
            cctCharacterRead,
            cctCharacterWrite,
            cctPetRead,
            cctPetWrite,
            cctBankRead,
            cctBankWrite,
            cctBankWriteEx,
            cctCharItemDelete
        }
    }
}

