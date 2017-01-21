namespace XMLDB3.ItemMarket
{
    using Mabinogi;
    using System;
    using System.IO;
    using System.Net.Sockets;
    using System.Threading;
    using XMLDB3;

    public class ItemMarketHandler : ItemMarketClient
    {
        private bool bHeartBeatReceived;
        private int gameNo;
        private const long heartbeatPeriod = 0x1d4c0L;
        private string name = string.Empty;
        private int packetNo = 0;
        private XMLDB3.ItemMarket.QueryManager queryManager = new XMLDB3.ItemMarket.QueryManager();
        private int serverNo;
        private Timer timerHeartbeat = null;
        private Timer timerReconnect = null;

        public ItemMarketHandler(string _name, int _gameNo, int _serverNo)
        {
            this.name = _name;
            this.gameNo = _gameNo;
            this.serverNo = _serverNo;
            this.bHeartBeatReceived = true;
            this.timerReconnect = new Timer(new TimerCallback(this.ConnectionProcess), this, -1, -1);
            this.timerHeartbeat = new Timer(new TimerCallback(this.CheckHeartbeat), null, -1, -1);
        }

        private bool _Send(ItemMarketCommand _command, uint _ID, uint _queryID, uint _targetID, int _clientID)
        {
            lock (this)
            {
                int num = 0;
                bool flag = false;
                try
                {
                    num = this.queryManager.PushQuery(_ID, _queryID, _targetID, _clientID);
                    flag = true;
                    _command.BuildPacket(num);
                    base.Send(_command.Packet, _command.Packet.Length);
                }
                catch (Exception exception)
                {
                    ExceptionMonitor.ExceptionRaised(exception);
                    if (flag)
                    {
                        this.queryManager.PopQuery(num);
                    }
                    return false;
                }
                return true;
            }
        }

        private void CheckHeartbeat(object state)
        {
            if (this.bHeartBeatReceived)
            {
                ItemMarketCommand command = new IMHeartbeatCommand(this.gameNo, this.serverNo);
                this.Send(command, 0, 0, 0, 0);
                this.bHeartBeatReceived = false;
                this.ScheduleHeartbeat();
            }
            else
            {
                ExceptionMonitor.ExceptionRaised(new Exception(string.Format("Item Market Client [{0}] Heartbeat failed.", this.Name)));
                base.Stop();
            }
        }

        private void ConnectionProcess(object state)
        {
            switch (base.m_State)
            {
                case ItemMarketClient.ConnectionState.NotInitialized:
                    ItemMarketManager.Connect(this);
                    return;

                case ItemMarketClient.ConnectionState.JustConnected:
                {
                    Console.WriteLine("Item Market Client [{0}]'s sending Initialize Packet.", this.Name);
                    ItemMarketCommand command = new IMInitializeCommand(this.gameNo, this.serverNo);
                    this._Send(command, 0, 0, 0, 0);
                    this.timerReconnect.Change(0x7530, -1);
                    return;
                }
            }
        }

        protected override void OnConnect()
        {
            Console.WriteLine("Item Market Client [{0}]'s Connected.", this.Name);
            this.ConnectionProcess(null);
        }

        protected override int OnReceive(byte[] _buffer, int _length)
        {
            BinaryReader reader = null;
            int num;
            try
            {
                MemoryStream input = new MemoryStream(_buffer, 0, _length);
                reader = new BinaryReader(input);
                ItemMarketResponse response = ItemMarketResponse.BuildRespose(reader);
                if (response != null)
                {
                    Query query = this.queryManager.PopQuery(response.PacketNo);
                    if (query != null)
                    {
                        if (response.IsSystemMessage)
                        {
                            response.Build(reader, null);
                            this.OnSystemMessage(response);
                        }
                        else
                        {
                            Message message = new Message(query.ID, (ulong) query.targetID);
                            message.WriteU32(query.queryID);
                            response.Build(reader, message);
                            MainProcedure.ServerSend(query.clientID, message);
                        }
                    }
                    else
                    {
                        ExceptionMonitor.ExceptionRaised(new Exception("There is no query ID."), response.PacketNo);
                    }
                    return response.PacketLength;
                }
                num = 0;
            }
            catch (SocketException exception)
            {
                throw exception;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2);
                num = 0;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return num;
        }

        private void OnSystemMessage(ItemMarketResponse response)
        {
            IMMessage type = response.Type;
            if (type != IMMessage.Initialize)
            {
                if (type != IMMessage.Heartbeat)
                {
                    return;
                }
            }
            else
            {
                if (response.Result == 1)
                {
                    base.m_State = ItemMarketClient.ConnectionState.Initialized;
                    this.bHeartBeatReceived = true;
                    this.ScheduleHeartbeat();
                    if (base.OnInitialized != null)
                    {
                        base.OnInitialized(this, response.Result);
                        return;
                    }
                    return;
                }
                if (base.OnInitializeFailed != null)
                {
                    base.OnInitializeFailed(this, response.Result);
                }
                base.Stop();
                return;
            }
            if ((response.Result == 1) || (response.Result == 0x11))
            {
                this.bHeartBeatReceived = true;
            }
        }

        private void ScheduleHeartbeat()
        {
            this.timerHeartbeat.Change((long) 0x1d4c0L, (long) (-1L));
        }

        public void ScheduleReconnect()
        {
            this.timerReconnect.Change(0x2710, -1);
        }

        public bool Send(ItemMarketCommand _command, uint _ID, uint _queryID, uint _targetID, int _clientID)
        {
            if (base.m_State != ItemMarketClient.ConnectionState.Initialized)
            {
                return false;
            }
            return this._Send(_command, _ID, _queryID, _targetID, _clientID);
        }

        public bool IsWorking
        {
            get
            {
                return (base.m_State == ItemMarketClient.ConnectionState.Initialized);
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public int PacketNo
        {
            get
            {
                return Interlocked.Increment(ref this.packetNo);
            }
        }
    }
}

