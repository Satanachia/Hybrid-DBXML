namespace XMLDB3
{
    using Mabinogi;
    using System;
    using System.Threading;

    public class ProcessManager
    {
        private bool bSystemActive = true;
        private CommandQueue cmdQueue = new CommandQueue();
        private const int MaxThread = 20;
        private CommandQueue primeCmdQueue = new CommandQueue();
        private static ProcessManager processManager = null;
        private Thread[] processThread = new Thread[20];
        private AutoResetEvent threadWait = new AutoResetEvent(false);

        private ProcessManager()
        {
            for (int i = 0; i < 20; i++)
            {
                this.processThread[i] = new Thread(new ThreadStart(this.ProcessThread));
                this.processThread[i].Name = i.ToString();
                this.processThread[i].Start();
            }
        }

        private void _AddCommand(BasicCommand _cmd)
        {
            lock (this.cmdQueue)
            {
                _cmd.Prepare();
                this.cmdQueue.Push(_cmd);
                this.threadWait.Set();
            }
        }

        private void _AddPrimeCommand(BasicCommand _cmd)
        {
            lock (this.primeCmdQueue)
            {
                _cmd.Prepare();
                this.primeCmdQueue.Push(_cmd);
                this.threadWait.Set();
            }
        }

        public static void AddCommand(BasicCommand _Cmd)
        {
            processManager._AddCommand(_Cmd);
        }

        public static void AddPrimeCommand(BasicCommand _Cmd)
        {
            processManager._AddPrimeCommand(_Cmd);
        }

        public static Message CacheStatisticsToMessage()
        {
            Message message = new Message(0, 0L);
            message.WriteS32(4);
            message += new CacheStatistics("").ToMessage();
            message += processManager.cmdQueue.Statistics.ToMessage();
            message += ObjectCache.Character.Statistics.ToMessage();
            return (message + ObjectCache.Bank.Statistics.ToMessage());
        }

        private void Dispose()
        {
            this.bSystemActive = false;
            this.threadWait.Set();
            for (int i = 0; i < 20; i++)
            {
                this.processThread[i].Join();
            }
        }

        private static BasicCommand GetCommandFromQueue(CommandQueue _queue)
        {
            BasicCommand command = null;
            try
            {
                lock (_queue)
                {
                    command = _queue.Pop();
                }
            }
            catch (Exception exception)
            {
                if (command != null)
                {
                    command.OnError();
                }
                throw exception;
            }
            return command;
        }

        private void ProcessThread()
        {
            BasicCommand commandFromQueue;
        Label_0000:
            commandFromQueue = null;
            try
            {
                commandFromQueue = GetCommandFromQueue(this.primeCmdQueue);
                if (commandFromQueue == null)
                {
                    commandFromQueue = GetCommandFromQueue(this.cmdQueue);
                }
                if (commandFromQueue != null)
                {
                    WorkSession.Begin(Thread.CurrentThread.Name, commandFromQueue.GetType().Name, commandFromQueue.Target);
                    try
                    {
                        try
                        {
                            WorkSession.WriteStatus("작업 세션을 시작합니다");
                            for (BasicCommand command2 = commandFromQueue; command2 != null; command2 = command2.Next)
                            {
                                try
                                {
                                    command2.DoProcess();
                                }
                                catch (Exception exception)
                                {
                                    ExceptionMonitor.ExceptionRaised(exception, command2);
                                }
                            }
                            for (BasicCommand command3 = commandFromQueue; command3 != null; command3 = command3.Next)
                            {
                                if (command3.ReplyEnable)
                                {
                                    WorkSession.WriteStatus("ProcessManager.ProcessThread() : " + command3.ToString() + " 의 응답 메시지를 클아이언트에 전송합니다");
                                    Message message = command3.MakeMessage();
                                    MainProcedure.ServerSend(command3.Target, message);
                                }
                                else
                                {
                                    WorkSession.WriteStatus("ProcessManager.ProcessThread() : " + command3.ToString() + " 의 응답 메시지가 없습니다.");
                                }
                            }
                        }
                        catch (Exception exception2)
                        {
                            ExceptionMonitor.ExceptionRaised(exception2, commandFromQueue);
                            WorkSession.WriteStatus(exception2.ToString());
                        }
                        goto Label_0000;
                    }
                    finally
                    {
                        WorkSession.WriteStatus("작업 세션을 종료합니다");
                        WorkSession.End();
                    }
                }
                if (this.bSystemActive)
                {
                    this.threadWait.WaitOne();
                }
                else
                {
                    this.threadWait.Set();
                    return;
                }
            }
            catch (Exception exception3)
            {
                if (commandFromQueue != null)
                {
                    commandFromQueue.OnError();
                }
                ExceptionMonitor.ExceptionRaised(exception3, commandFromQueue);
            }
            goto Label_0000;
        }

        public static void Shutdown()
        {
            processManager.Dispose();
        }

        public static void Start()
        {
            processManager = new ProcessManager();
        }
    }
}

