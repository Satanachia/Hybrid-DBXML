namespace XMLDB3
{
    using Mabinogi;
    using System;

    public abstract class SerializedCommand : BasicCommand, ISerializableCommand
    {
        private CommandSerializer serializer = null;

        protected SerializedCommand()
        {
        }

        protected abstract bool _DoProces();
        protected abstract void _ReceiveData(Message _Msg);
        public override bool DoProcess()
        {
            bool flag;
            try
            {
                if (this.serializer == null)
                {
                    throw new Exception("시리얼라이저가 없습니다.");
                }
                this.serializer.Wait();
                flag = this._DoProces();
            }
            catch (Exception exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, this);
                WorkSession.WriteStatus(exception.Message);
                flag = false;
            }
            finally
            {
                try
                {
                    if (this.serializer != null)
                    {
                        this.serializer.Close();
                    }
                }
                catch (Exception exception2)
                {
                    ExceptionMonitor.ExceptionRaised(exception2, this);
                }
                finally
                {
                    this.serializer = null;
                }
            }
            return flag;
        }

        public override void OnError()
        {
            if (this.serializer != null)
            {
                this.serializer.Close();
            }
            this.serializer = null;
            base.OnError();
        }

        public abstract void OnSerialize(IObjLockRegistHelper _helper, bool bBegin);
        public override bool Prepare()
        {
            this.serializer = new CommandSerializer(this);
            return true;
        }

        protected override void ReceiveData(Message _Msg)
        {
            this._ReceiveData(_Msg);
        }
    }
}

