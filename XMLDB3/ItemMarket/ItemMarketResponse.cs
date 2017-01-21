namespace XMLDB3.ItemMarket
{
    using Mabinogi;
    using System;
    using System.IO;
    using System.Net;
    using XMLDB3;

    public class ItemMarketResponse
    {
        protected Message message = null;
        protected IMMessage messageType = IMMessage.None;
        protected int packetLength = 0;
        protected int packetNo = 0;
        protected int result = 0;

        protected ItemMarketResponse()
        {
        }

        public virtual void Build(BinaryReader _br, Message _message)
        {
            _message.WriteU8(0);
        }

        public static ItemMarketResponse BuildRespose(BinaryReader _br)
        {
            ItemMarketResponse response = null;
            int num = 0;
            int num2 = 0;
            try
            {
                if (_br.BaseStream.Length <= (_br.BaseStream.Position + 5L))
                {
                    return null;
                }
                if (_br.ReadByte() != 160)
                {
                    throw new Exception("Invalid Protocol Header");
                }
                num = IPAddress.NetworkToHostOrder(_br.ReadInt32()) + 5;
                if (_br.BaseStream.Length < num)
                {
                    return null;
                }
                num2 = IPAddress.NetworkToHostOrder(_br.ReadInt32());
                byte num3 = _br.ReadByte();
                switch (num3)
                {
                    case 0x11:
                        response = new IMCheckEnteranceResponse();
                        break;

                    case 0x12:
                        response = new IMCheckBalanceResponse();
                        break;

                    case 1:
                        response = new IMInitializeResponse();
                        break;

                    case 0x21:
                        response = new IMInquirySaleItemResponse();
                        break;

                    case 0x22:
                        response = new IMInquiryStorageResponse();
                        break;

                    case 0x24:
                        response = new IMInquiryMyPageResponse();
                        break;

                    case 0x81:
                        response = new IMGetItemResponse();
                        break;

                    case 130:
                        response = new IMGetItemCommitResponse();
                        break;

                    case 0x83:
                        response = new IMGetItemRollbackResponse();
                        break;

                    case 0xff:
                        response = new IMHeartbeatResponse();
                        break;

                    case 0x31:
                        response = new IMItemListResponse();
                        break;

                    case 50:
                        response = new IMItemSearchResponse();
                        break;

                    case 0x41:
                        response = new IMSaleRequestResponse();
                        break;

                    case 0x42:
                        response = new IMSaleRequestCommitResponse();
                        break;

                    case 0x43:
                        response = new IMSaleCancelResponse();
                        break;

                    case 0x44:
                        response = new IMPurchaseResponse();
                        break;

                    case 0x45:
                        response = new IMSaleRequestRollbackResponse();
                        break;
                }
                if (response != null)
                {
                    response.messageType = (IMMessage) num3;
                }
                else
                {
                    response = new ItemMarketResponse();
                }
                response.packetLength = num;
                response.packetNo = num2;
                return response;
            }
            catch (EndOfStreamException)
            {
                return null;
            }
            catch (Exception exception)
            {
                ExceptionMonitor.ExceptionRaised(exception);
                if (response == null)
                {
                    response = new ItemMarketResponse();
                    response.packetLength = num;
                    response.packetNo = num2;
                }
                return response;
            }
        }

        public virtual bool IsSystemMessage
        {
            get
            {
                return false;
            }
        }

        public int PacketLength
        {
            get
            {
                return this.packetLength;
            }
        }

        public int PacketNo
        {
            get
            {
                return this.packetNo;
            }
        }

        public int Result
        {
            get
            {
                return this.result;
            }
        }

        public IMMessage Type
        {
            get
            {
                return this.messageType;
            }
        }
    }
}

