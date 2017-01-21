namespace XMLDB3
{
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class ObjectLock
    {
        public const int DefaultLockTimeout = 0x1d4c0;
        private Queue lockPool;
        private Hashtable longHash;
        public const int MaxConcurrentLock = 0x20;
        private Hashtable stringHash;
        private int timeout;
        private LockInstance[] waitingInstants;
        private uint waitingLockIDs;

        public ObjectLock() : this(0x1d4c0)
        {
        }

        public ObjectLock(int _timeout)
        {
            this.waitingLockIDs = 0;
            this.lockPool = new Queue(0x21);
            for (int i = 0; i < 0x20; i++)
            {
                LockInstance instance = new LockInstance(i, this);
                this.lockPool.Enqueue(instance);
            }
            this.waitingInstants = new LockInstance[0x20];
            this.timeout = _timeout;
            this.stringHash = new Hashtable(0x40);
            this.longHash = new Hashtable(0xc80);
        }

        private Hashtable _ForceUnlock(Hashtable _lockTable, LockInstance _inst)
        {
            uint num = ((uint) 1) << _inst.LockID;
            Hashtable hashtable = (Hashtable) _lockTable.Clone();
            IDictionaryEnumerator enumerator = _lockTable.GetEnumerator();
            while (enumerator.MoveNext())
            {
                uint num2 = (uint) enumerator.Value;
                if (num2 == num)
                {
                    hashtable.Remove(enumerator.Key);
                }
                else if ((num2 & num) != 0)
                {
                    hashtable[enumerator.Key] = num2 & ~num;
                }
            }
            _lockTable = null;
            return hashtable;
        }

        private void Close(LockInstance _lockInst)
        {
            for (int i = 0; i < 0x20; i++)
            {
                if ((this.waitingInstants[i] != null) && this.waitingInstants[i].WakeUp(_lockInst.LockID))
                {
                    this.waitingInstants[i] = null;
                }
            }
            this.waitingLockIDs &= (uint) ~(((int) 1) << _lockInst.LockID);
            this.lockPool.Enqueue(_lockInst);
        }

        public ILock Create()
        {
            if (this.lockPool.Count > 0)
            {
                LockInstance instance = (LockInstance) this.lockPool.Dequeue();
                try
                {
                    instance.Init();
                    this.waitingInstants[instance.LockID] = null;
                    return instance;
                }
                catch (Exception exception)
                {
                    this.lockPool.Enqueue(instance);
                    throw exception;
                }
            }
            throw new Exception("더 이상 락을 사용할 수 없습니다.");
        }

        private void ForceUnlock(LockInstance _inst)
        {
            this.stringHash = this._ForceUnlock(this.stringHash, _inst);
            this.longHash = this._ForceUnlock(this.longHash, _inst);
        }

        private uint Lock(object _object, Hashtable _lockTable, LockInstance _inst)
        {
            uint num = ((uint) 1) << _inst.LockID;
            if (_lockTable.ContainsKey(_object))
            {
                uint num2 = (uint) _lockTable[_object];
                if ((num2 & num) != 0)
                {
                    throw new Exception("락 테이블에 이상이 있습니다.");
                }
                WorkSession.WriteStatus("ObjectLock.Lock() : 이미 락 개체[" + num2.ToString() + "]가 아이디를 점유하고 있습니다.");
                _lockTable[_object] = num2 | num;
                if (this.waitingInstants[_inst.LockID] == null)
                {
                    if ((num & this.waitingLockIDs) != 0)
                    {
                        throw new Exception("데드락 가능 상황입니다. [" + this.waitingLockIDs.ToString() + "|" + num.ToString() + "]");
                    }
                    this.waitingLockIDs |= num2;
                    this.waitingInstants[_inst.LockID] = _inst;
                }
                return num2;
            }
            _lockTable[_object] = num;
            return 0;
        }

        private uint LockObjectID(long _objectID, LockInstance _inst)
        {
            return this.Lock(_objectID, this.longHash, _inst);
        }

        private uint LockStringID(string _stringID, LockInstance _inst)
        {
            return this.Lock(_stringID, this.stringHash, _inst);
        }

        private bool Unlock(object _object, Hashtable _lockTable, LockInstance _inst)
        {
            if (_lockTable.Contains(_object))
            {
                uint num = ((uint) 1) << _inst.LockID;
                uint num2 = (uint) _lockTable[_object];
                if (num == num2)
                {
                    _lockTable.Remove(_object);
                    return true;
                }
                if ((num & num2) != 0)
                {
                    _lockTable[_object] = num2 & ~num;
                    return false;
                }
                if (_inst.bIsValidLock)
                {
                    ExceptionMonitor.ExceptionRaised(new Exception("락이 재대로 풀리지 않았습니다."), _inst.LockID, _object);
                    return false;
                }
                return true;
            }
            ExceptionMonitor.ExceptionRaised(new Exception("락이 재대로 풀리지 않았습니다."), _inst.LockID, _object);
            return true;
        }

        private void UnlockObjectID(long _objectID, LockInstance _inst)
        {
            this.Unlock(_objectID, this.longHash, _inst);
        }

        private void UnlockStringID(string _stringID, LockInstance _inst)
        {
            this.Unlock(_stringID, this.stringHash, _inst);
        }

        public int Available
        {
            get
            {
                return this.lockPool.Count;
            }
        }

        public object SyncRoot
        {
            get
            {
                return this;
            }
        }

        public int Timeout
        {
            get
            {
                return this.timeout;
            }
        }

        private class LockInstance : ILock
        {
            private ObjectLock.ObjLockRegistHelper beginHelper;
            public bool bIsValidLock;
            private ObjectLock creator;
            private ObjectLock.ObjLockRegistHelper endHelper;
            private int lockID;
            private AutoResetEvent onLock;
            protected uint waitingLockID;

            public LockInstance(int _lockID, ObjectLock _creator)
            {
                this.lockID = _lockID;
                this.creator = _creator;
                this.beginHelper = new ObjectLock.ObjLockRegistHelper(new ObjectLock.ObjectIDHelper(this.RegisterObjectID), new ObjectLock.StringIDHelper(this.RegisterStringID));
                this.endHelper = new ObjectLock.ObjLockRegistHelper(new ObjectLock.ObjectIDHelper(this.CloseObjectID), new ObjectLock.StringIDHelper(this.CloseStringID));
                this.onLock = new AutoResetEvent(false);
            }

            private void CloseObjectID(long _id)
            {
                this.creator.UnlockObjectID(_id, this);
            }

            private void CloseStringID(string _id)
            {
                this.creator.UnlockStringID(_id, this);
            }

            public void Init()
            {
                this.bIsValidLock = true;
                this.waitingLockID = 0;
                this.onLock.Reset();
            }

            private void RegisterObjectID(long _id)
            {
                this.waitingLockID |= this.creator.LockObjectID(_id, this);
            }

            private void RegisterStringID(string _id)
            {
                this.waitingLockID |= this.creator.LockStringID(_id, this);
            }

            public override string ToString()
            {
                return ((this.lockID.ToString() + ":" + this.waitingLockID.ToString()));
            }

            public bool WakeUp(int _lockID)
            {
                this.waitingLockID &= (uint) ~(((int) 1) << _lockID);
                if (this.waitingLockID == 0)
                {
                    this.onLock.Set();
                    return true;
                }
                return false;
            }

            void ILock.Close()
            {
                this.creator.Close(this);
            }

            void ILock.ForceUnregist()
            {
                this.creator.ForceUnlock(this);
            }

            void ILock.Wait()
            {
                if (this.waitingLockID != 0)
                {
                    if (this.creator.Timeout == -1)
                    {
                        this.onLock.WaitOne();
                    }
                    else if (!this.onLock.WaitOne(this.creator.Timeout, false))
                    {
                        throw new Exception("락 대기 타임아웃:[" + this.ToString() + "]");
                    }
                }
            }

            public int LockID
            {
                get
                {
                    return this.lockID;
                }
            }

            IObjLockRegistHelper ILock.BeginHelper
            {
                get
                {
                    return this.beginHelper;
                }
            }

            IObjLockRegistHelper ILock.EndHelper
            {
                get
                {
                    return this.endHelper;
                }
            }
        }

        private delegate void ObjectIDHelper(long _id);

        private class ObjLockRegistHelper : IObjLockRegistHelper
        {
            private ObjectLock.ObjectIDHelper objHelper;
            private ObjectLock.StringIDHelper strHelper;

            public ObjLockRegistHelper(ObjectLock.ObjectIDHelper _objHelper, ObjectLock.StringIDHelper _strHelper)
            {
                this.objHelper = _objHelper;
                this.strHelper = _strHelper;
            }

            public void ObjectIDRegistant(long _id)
            {
                this.objHelper(_id);
            }

            public void StringIDRegistant(string _id)
            {
                this.strHelper(_id);
            }
        }

        private delegate void StringIDHelper(string _id);
    }
}

