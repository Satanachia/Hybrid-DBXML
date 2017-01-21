namespace XMLDB3
{
    using System;
    using System.Collections;
    using System.Threading;

    public class CommandSerializer
    {
        private static bool bActive;
        private ISerializableCommand command = null;
        private const int DefaultTimeout = -1;
        private ILock lockInst = null;
        private static ObjectLock objectLock;
        private static Queue registerQueue;
        private LockState state = LockState.Invalid;
        private static int timeout;
        private static AutoResetEvent wakeupEvent;
        private static Thread workThread;

        public CommandSerializer(ISerializableCommand _command)
        {
            this.command = _command;
            this.state = LockState.Created;
            lock (registerQueue.SyncRoot)
            {
                registerQueue.Enqueue(this);
                wakeupEvent.Set();
            }
        }

        public bool Close()
        {
            lock (this)
            {
                switch (this.state)
                {
                    case LockState.Invalid:
                        Close(this);
                        break;

                    case LockState.Registered:
                        Close(this);
                        break;

                    case LockState.Closed:
                        throw new Exception("이미 자원이 해재되었습니다.");
                }
                this.state = LockState.Closed;
            }
            return true;
        }

        private static void Close(CommandSerializer _cs)
        {
            lock (objectLock.SyncRoot)
            {
                WorkSession.WriteStatus(_cs.command.ToString() + "에 락을 해제합니다.");
                if ((_cs != null) && (_cs.lockInst != null))
                {
                    try
                    {
                        _cs.command.OnSerialize(_cs.lockInst.EndHelper, false);
                    }
                    catch (Exception exception)
                    {
                        MailSender.Send(exception.ToString());
                        ExceptionMonitor.ExceptionRaised(exception, _cs.command);
                        _cs.lockInst.ForceUnregist();
                    }
                    _cs.lockInst.Close();
                    if (objectLock.Available > 0)
                    {
                        Monitor.Pulse(objectLock.SyncRoot);
                    }
                }
            }
        }

        private static ILock CreateLock()
        {
            ILock lock2;
            Monitor.Enter(objectLock.SyncRoot);
            try
            {
                while (objectLock.Available == 0)
                {
                    WorkSession.WriteStatus("여유 락이 생길때 까지 대기합니다.");
                    if (!Monitor.Wait(objectLock.SyncRoot, timeout))
                    {
                        throw new Exception("락 생성 타임아웃");
                    }
                }
                lock2 = objectLock.Create();
            }
            catch (Exception exception)
            {
                ExceptionMonitor.ExceptionRaised(exception);
                lock2 = null;
            }
            finally
            {
                Monitor.Exit(objectLock.SyncRoot);
            }
            return lock2;
        }

        public static void Initialize()
        {
            Initialize(-1);
        }

        public static void Initialize(int _timeout)
        {
            timeout = _timeout;
            bActive = true;
            objectLock = new ObjectLock();
            wakeupEvent = new AutoResetEvent(false);
            registerQueue = new Queue();
            workThread = new Thread(new ThreadStart(CommandSerializer.MainLoop));
            workThread.Start();
        }

        private static void MainLoop()
        {
            CommandSerializer serializer;
        Label_0000:
            serializer = null;
            lock (registerQueue)
            {
                if (registerQueue.Count > 0)
                {
                    serializer = (CommandSerializer) registerQueue.Dequeue();
                }
            }
            if (serializer != null)
            {
                WorkSession.Begin("CommandSerializer", null, 0);
                ILock @lock = CreateLock();
                Monitor.Enter(serializer);
                try
                {
                    try
                    {
                        if (@lock == null)
                        {
                            WorkSession.WriteStatus(serializer.command.ToString() + "의 락을 얻어오는데 실패했습니다.");
                            serializer.state = LockState.Invalid;
                        }
                        else
                        {
                            WorkSession.WriteStatus(serializer.command.ToString() + "의 락을 생성하였습니다.");
                            if (serializer.state == LockState.Created)
                            {
                                WorkSession.WriteStatus(serializer.command.ToString() + "에 락[" + @lock.ToString() + "]을 할당합니다.");
                                serializer.lockInst = @lock;
                                lock (objectLock.SyncRoot)
                                {
                                    WorkSession.WriteStatus(serializer.command.ToString() + "에 락을 겁니다.");
                                    serializer.command.OnSerialize(serializer.lockInst.BeginHelper, true);
                                }
                                serializer.state = LockState.Registered;
                            }
                            else
                            {
                                serializer.state = LockState.Invalid;
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        ExceptionMonitor.ExceptionRaised(exception);
                        serializer.state = LockState.Invalid;
                    }
                    goto Label_0000;
                }
                finally
                {
                    WorkSession.WriteStatus(serializer.command.ToString() + "에 할당신호를 보냅니다.");
                    Monitor.Pulse(serializer);
                    Monitor.Exit(serializer);
                    WorkSession.End();
                }
            }
            if (!bActive)
            {
                return;
            }
            wakeupEvent.WaitOne();
            goto Label_0000;
        }

        public static void Shutdown()
        {
            bActive = false;
            wakeupEvent.Set();
            workThread.Join();
        }

        public void Wait()
        {
            lock (this)
            {
                while (this.state != LockState.Registered)
                {
                    if (this.state != LockState.Created)
                    {
                        throw new Exception("상태 값이 잘못 되었습니다.");
                    }
                    WorkSession.WriteStatus(this.command.ToString() + "의 락이 할당되기를 대기합니다.");
                    if (!Monitor.Wait(this, timeout))
                    {
                        throw new Exception("락 대기 타임 아웃");
                    }
                }
                WorkSession.WriteStatus(this.command.ToString() + "의 명령순서를 대기합니다.");
                this.lockInst.Wait();
            }
        }

        private enum LockState
        {
            Closed = 3,
            Created = 0,
            Entered = 2,
            Invalid = -1,
            Registered = 1
        }
    }
}

