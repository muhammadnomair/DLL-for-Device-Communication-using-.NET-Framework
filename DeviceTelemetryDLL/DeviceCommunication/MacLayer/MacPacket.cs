using DeviceTelemetryDLL.DeviceCommunication.CommunicationConstants;
using DeviceTelemetryDLL.DeviceCommunication.PcapLayer;
using DeviceTelemetryDLL.Helpers;
using SharpPcap;

namespace DeviceTelemetryDLL.DeviceCommunication.MacLayer
{
    #region: Declaring different C# structures (data-type) related to MAC and Ethernet layer
    // Declaring fields of Ethernet Layer Structure
    public struct EthernetLayer
    {
        public byte[] Source;
        public byte[] Destination;
        public UInt16 udEtherType;

        // array of bytes for UDP packet header values
        public byte[] UDP_PacketHeader; // 28 bytes
        public byte[] CRC;
    }

    // Declaring fields of the Receive Packet Structure at MAC Level
    public struct MAC_RX_PACKET_STRUCT
    {
        public byte[] aunMacRxPacketBuff;
        public int lMacRxPacketLen;
    }

    // Declaring structure fields of actual MAC Packet
    public struct MAC_PACKET_STRUCT
    {
        public byte[] aunDestination; // 6 bytes
        public byte[] aunSource; // 6 bytes
        public UInt16 udEtherType; // 2 bytes

        public byte[] UDP_PacketHeader; // 28 bytes
        public byte[] CRC; //4 bytes
        public byte ulCmdCode; // 1 byte
        public UInt16 udMacPayloadLen; // 2 bytes
        public byte[] aunMacPayloadBuff;
    }

    // Declaring fields of Ethernet layer MAC Packet structure
    public struct MAC_PACKET
    {
        public EthernetLayer EthernetLayerParam;
        //public UInt32 ulCmdCode;
        public byte ulCmdCode;
        public UInt16 udPayloadLength;
        public byte[] aunPayload;
    }

    // Declaring fields of building (populating) MAC Packet structure
    public struct MAC_PACKET_BUILD
    {
        public byte[] aunMacPopulateBuff;
        public UInt16 udMacPopulatedDataLen;
    }
    #endregion: Declaring different C# structures

    public delegate void CbFunction(object sender, CaptureEventArgs objEvent);

    /// <summary>
    /// Creating a new class: MacPacket (Intermediate layer, between pcap(network interface layer) and apdu layer (application layer))
    /// This class will handles and preprocess  
    /// </summary>
    public class MacPacket
    {
        public UInt32 ulEthMacTransId;

        // Declaring the object of Receiving packet and actual MAC packet structures
        private MAC_RX_PACKET_STRUCT stMacRxPacket;
        private MAC_PACKET_STRUCT stMacPacket;

        // Create a new PCAP Interface
        public PcapInterface objPcapInterface = new PcapInterface();

        private bool bMacProcessingCompleted = false;
        public bool bMacIsProcessingCompleted
        {
            get
            {
                return bMacProcessingCompleted;    // get method
            }
            set
            {
                bMacProcessingCompleted = value;    // set method
            }
        }

        // Displaying the encided MAC packet fields
        private void vMacPrintParsedPacket(ref MAC_PACKET_STRUCT stMacPacket)
        {
            try
            {
                Console.WriteLine("------------------- IN MAC PACKET Layer: Response Packets properties -------------------");

                // Dusplaying all paraeters parsed for debugging
                Console.WriteLine($"Destination Address: {stMacPacket.aunDestination}");

                Console.WriteLine($"Source Address: {stMacPacket.aunSource}");

                Console.WriteLine($"Ethernet Type: {stMacPacket.udEtherType}");

                Console.WriteLine($"UDP Packet Header Values in Resposne (in string formate): {stMacPacket.UDP_PacketHeader}");

                Console.WriteLine($"Command Code: {stMacPacket.ulCmdCode}");

                Console.WriteLine($"Payload Length: {stMacPacket.udMacPayloadLen}");
            }
            catch (Exception ex)
            {
                // save log message
                Console.WriteLine($"An Error Occured during vMacPrintParsedPacket :{ex.Message}");
            }
        }// End of the function: vMacPrintParsedPacket

        // This function print the encoded packet.
        private void vMacPrintEncodedPacket(ref MAC_PACKET pstMacPacketParam)
        {
            /* Dusplaying all paraeters parsed for debugging */
        }// End of the function: vMacPrintEncodedPacket

        // This function sends packet on the Network Interface.
        private byte[] pktMacEncodePacket(ref MAC_PACKET pstMacPacketParam, ref MAC_PACKET_BUILD pstMacBuildPacket)
        {
            UInt16 udIndex = 0;
            try
            {
                Crc32 objCRC32 = new Crc32();
                // Fill up the LSB bytes first as index gets incremented i.e each LSB should be at lower index count
                CharArrayUtility.vMemSet(pstMacBuildPacket.aunMacPopulateBuff, 0);

                // Destination Address (first 6 bytes)
                pstMacBuildPacket.aunMacPopulateBuff[udIndex++] = pstMacPacketParam.EthernetLayerParam.Destination[0];
                pstMacBuildPacket.aunMacPopulateBuff[udIndex++] = pstMacPacketParam.EthernetLayerParam.Destination[1];
                pstMacBuildPacket.aunMacPopulateBuff[udIndex++] = pstMacPacketParam.EthernetLayerParam.Destination[2];
                pstMacBuildPacket.aunMacPopulateBuff[udIndex++] = pstMacPacketParam.EthernetLayerParam.Destination[3];
                pstMacBuildPacket.aunMacPopulateBuff[udIndex++] = pstMacPacketParam.EthernetLayerParam.Destination[4];
                pstMacBuildPacket.aunMacPopulateBuff[udIndex++] = pstMacPacketParam.EthernetLayerParam.Destination[5];

                // Source Address (next 6 bytes)
                pstMacBuildPacket.aunMacPopulateBuff[udIndex++] = pstMacPacketParam.EthernetLayerParam.Source[0];
                pstMacBuildPacket.aunMacPopulateBuff[udIndex++] = pstMacPacketParam.EthernetLayerParam.Source[1];
                pstMacBuildPacket.aunMacPopulateBuff[udIndex++] = pstMacPacketParam.EthernetLayerParam.Source[2];
                pstMacBuildPacket.aunMacPopulateBuff[udIndex++] = pstMacPacketParam.EthernetLayerParam.Source[3];
                pstMacBuildPacket.aunMacPopulateBuff[udIndex++] = pstMacPacketParam.EthernetLayerParam.Source[4];
                pstMacBuildPacket.aunMacPopulateBuff[udIndex++] = pstMacPacketParam.EthernetLayerParam.Source[5];

                // Ethernet types (next 2 bytes)
                pstMacBuildPacket.aunMacPopulateBuff[udIndex++] = (byte)(pstMacPacketParam.EthernetLayerParam.udEtherType >> 8); // getting MSB (second byte)
                pstMacBuildPacket.aunMacPopulateBuff[udIndex++] = (byte)(pstMacPacketParam.EthernetLayerParam.udEtherType); // getting LSB (first byte)

                // UDP packet header values (28 bytes) from MAC Packet's "UDP_PacketHeader" field
                for (UInt16 byteIndex = 0; byteIndex < FDRConstants.UdpPacketHeaderSize; byteIndex++)
                {
                    pstMacBuildPacket.aunMacPopulateBuff[udIndex++] = pstMacPacketParam.EthernetLayerParam.UDP_PacketHeader[byteIndex];
                }

                // patload (next bytes upto 'udPayloadLength')
                Buffer.BlockCopy(pstMacPacketParam.aunPayload, 0, pstMacBuildPacket.aunMacPopulateBuff, udIndex, pstMacPacketParam.udPayloadLength);
                udIndex += pstMacPacketParam.udPayloadLength;

                //Calculate CRC
                uint calculatedCRC = objCRC32.CalculateCRC(pstMacBuildPacket.aunMacPopulateBuff, udIndex);
                pstMacPacketParam.EthernetLayerParam.CRC = BitConverter.GetBytes(calculatedCRC);

                for (UInt16 byteIndex = 0; byteIndex < 4; byteIndex++)
                {
                    pstMacBuildPacket.aunMacPopulateBuff[udIndex++] = pstMacPacketParam.EthernetLayerParam.CRC[byteIndex];
                }

                // current index shows populated-data-length 
                pstMacBuildPacket.udMacPopulatedDataLen = udIndex;

                // Debug Prints
                //vMacPrintEncodedPacket(ref pstMacPacketParam);
            }
            catch (Exception ex)
            {
                // save log message
                Console.WriteLine($"An error occured while encoding (constructing) MAC-packet at MAC layer in the pktMacEncodePacket method:\n{ex.Message}");
            }
            // Return the populated buffer
            return pstMacBuildPacket.aunMacPopulateBuff;
        }// End of the function: pktMacEncodePacket

        // This function parse (decode) the MAC reveived packet and copies data into fields of actual MAC packet
        private void vMacParseReceivedPacket(ref MAC_RX_PACKET_STRUCT stMacRxPacket, ref MAC_PACKET_STRUCT stMacPacket)
        {
            try
            {
                UInt16 udIndex = 0;

                // Getting Destination Address data
                Buffer.BlockCopy(stMacRxPacket.aunMacRxPacketBuff, udIndex, stMacPacket.aunDestination, 0, stMacPacket.aunDestination.Length);
                udIndex += (UInt16)(stMacPacket.aunDestination.Length);

                // Getting Source Address data
                Buffer.BlockCopy(stMacRxPacket.aunMacRxPacketBuff, udIndex, stMacPacket.aunSource, 0, stMacPacket.aunSource.Length);
                udIndex += (UInt16)(stMacPacket.aunSource.Length);

                // Getting Ethernet Type from resposne
                stMacPacket.udEtherType = (UInt16)(stMacRxPacket.aunMacRxPacketBuff[udIndex] << 8 | (stMacRxPacket.aunMacRxPacketBuff[udIndex + 1]));
                udIndex += 2;

                // Getting UDP packet header (28 bytes) from response (copying it in MAC_PACKET)
                Buffer.BlockCopy(stMacRxPacket.aunMacRxPacketBuff, udIndex, stMacPacket.UDP_PacketHeader, 0, stMacPacket.UDP_PacketHeader.Length);
                udIndex += (UInt16)(stMacPacket.UDP_PacketHeader.Length);

                // Getting Command Code value (1 byte)
                stMacPacket.ulCmdCode = stMacRxPacket.aunMacRxPacketBuff[udIndex]; // converted into byte
                udIndex += 1;

                // Getting Payload Length data (only from HeartBeat Telemetry Response)
                stMacPacket.udMacPayloadLen = (UInt16)(stMacRxPacket.aunMacRxPacketBuff[udIndex] << 8 | (stMacRxPacket.aunMacRxPacketBuff[udIndex + 1]));
                udIndex += 2;

                // --- Getting MAC payload buffer data
                Buffer.BlockCopy(stMacRxPacket.aunMacRxPacketBuff, udIndex, stMacPacket.aunMacPayloadBuff, 0, stMacPacket.udMacPayloadLen);
                udIndex += stMacPacket.udMacPayloadLen;
                
                //Debug Prints
                //vMacPrintParsedPacket(ref stMacPacket);
            }
            catch (Exception ex)
            {
                // save log message
                Console.WriteLine($"An error occured while parsing fields of the received packet at MAC layer in the vMacParseReceivedPacket method:\n{ex.Message}");
            }
        }// End of the function: vMacParseReceivedPacket

        // This function receives the MAC Packet from PCAP Network Interface.
        public Int32 lMacRespReceivedHandler()
        {
            Int32 lResult = 0;
            try
            {
                // if packet received successfully at PCAP interface
                if (objPcapInterface.PcapReceivePacket() == true)
                {
                    stMacRxPacket.aunMacRxPacketBuff = objPcapInterface.aunPcapReceivedBytes(); // Getting Payload Bytes (buffer) from packet received at PCAP interface
                    stMacRxPacket.lMacRxPacketLen = objPcapInterface.aunPcapReceivedBytesLen(); // Getting Buffer length of the received bytes

                    // Parsing Received MAC packet into actual MAC packet structure
                    vMacParseReceivedPacket(ref stMacRxPacket, ref stMacPacket);

                    // if MAC packet has valid ethernet type
                    if (stMacPacket.udEtherType == objPcapInterface.aunPcapGetEthernetType())
                    {
                        bMacProcessingCompleted = true;
                    }
                }
            }
            catch (Exception ex)
            {
                // save log message
                Console.WriteLine($"An error occured handling response packet at MAC-layer in the lMacRespReceivedHandler method:\n{ex.Message}");
            }
            return lResult;
        }// End of the function: lMacRespReceivedHandler

        // This function initializes the MAC (MAC layer) & PCAP Interface (PCAP (network interface) layer).
        public void vMacInterfaceInit(int selectedInterfaceIndex)
        {
            try
            {
                // initializing PCAP / network interface (network layer)
                objPcapInterface.vPcapInterfaceInit(selectedInterfaceIndex);

                // initializing MAC Rx packet structure
                stMacRxPacket.aunMacRxPacketBuff = new byte[FDRConstants.MaxEthPktSize];

                // initializing Actual MAC packet structure
                stMacPacket.aunMacPayloadBuff = new byte[FDRConstants.MaxEthPktSize];
                stMacPacket.aunSource = new byte[6];
                stMacPacket.aunDestination = new byte[6];

                // initializing UDP packet header (byte array) 
                stMacPacket.UDP_PacketHeader = new byte[FDRConstants.UdpPacketHeaderSize];
            }
            catch (Exception ex)
            {
                // save log message
                Console.WriteLine($"An error occured while initialization of MAC-Layer in the vMacInterfaceInit method:\n{ex.Message}");
            }
        }// End of the function: vMacInterfaceInit

        // This function sends packet on Network Interface.
        private Int32 lMacSendPacket(byte[] aunPacketParam, Int32 lSize)
        {
            Int32 lResult = 0;

            if (aunPacketParam != null)
            {
                try
                {
                    objPcapInterface.vPcapSendPacket(aunPacketParam, lSize);
                    lResult = 1;
                }
                catch (Exception ex)
                {
                    lResult = -1;
                    // log message in ExceptionLog
                    Console.WriteLine($"Error occured while transferring packet to device at MAC layer in the lMacSendPacket method:\n{ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("ERROR: Packet is Null Referenced!");
                // save log message
                Console.WriteLine($"Error occurred in the lMacSendPacket method: The In-coming Packet is Null Referenced!");
            }
            return lResult;
        }// End of the function: lMacSendPacket

        // This function sends the MAC Packet on PCAP Network Interface.
        public Int32 lMacSendCmdReq(byte[] aunPacketParam, Int32 lSize)
        {
            return lMacSendPacket(aunPacketParam, lSize);
        }// End of the function: lMacSendCmdReq

        // This function builds the MAC packet for transmission.
        public MAC_PACKET_BUILD pktMacBuildCmdReq(UInt32 ulCmdCode, byte[] aunPayloadBuff, UInt16 udPayloadLen)
        {
            // creating an object for MAC_PACKET_BUILD structure
            MAC_PACKET_BUILD stMacBuildPacket = new MAC_PACKET_BUILD();

            MAC_PACKET stMacPacket = new MAC_PACKET();
            try
            {
                stMacBuildPacket.aunMacPopulateBuff = new byte[FDRConstants.MaxEthPktSize];

                // Get MAC source address from PCAP layer
                stMacPacket.EthernetLayerParam.Source = objPcapInterface.aunPcapGetSourceAddress();

                // Get MAC destination address from PCAP layer
                if (ulCmdCode == MacCmdCodes.ETH_REQ_FETCH_MAC)
                {
                    //check if cmd is fetch mac addr then broadcast this command
                    stMacPacket.EthernetLayerParam.Destination = FDRConstants.BroadCastMacAddress;// EthernetLayer.Destination;
                }
                else
                {
                    //EthernetLayer.Destination = digitizerform.objApdu.macdstad;
                    stMacPacket.EthernetLayerParam.Destination = objPcapInterface.aunPcapGetDestinationAddress();
                    // EthernetLayer.Destination = objPcapInterface.aunPcapGetDestinationAddress();
                }

                // Get Ethernet packet type from PCAP layer
                stMacPacket.EthernetLayerParam.udEtherType = objPcapInterface.aunPcapGetEthernetType();

                // Get UDP packet header value from PCAP layer
                stMacPacket.EthernetLayerParam.UDP_PacketHeader = objPcapInterface.PcapGetUDP_PacketHeader();

                stMacPacket.ulCmdCode = (byte)ulCmdCode; // converted to byte
                stMacPacket.aunPayload = aunPayloadBuff;
                stMacPacket.udPayloadLength = udPayloadLen; //21;


                if (udPayloadLen >= FDRConstants.PayLoadSize)
                {
                    Console.WriteLine("Error: Packet Size Out of Bound PAYLOAD_SZ = 1024 !");
                    Console.WriteLine("An error occured in the pktMacBuildCmdReq: Incoming Packet Size Out of Bound PAYLOAD_SZ = 1024 !");
                }

                pktMacEncodePacket(ref stMacPacket, ref stMacBuildPacket);
            }
            catch (Exception ex)
            {
                // save log message
                Console.WriteLine($"An error occured in while building commadn-based request at MAC Layer in the pktMacBuildCmdReq method:\n{ex.Message}");
            }
            return stMacBuildPacket;
        }// End of the function: pktMacBuildCmdReq

        // This function returns the received bytes on MAC interface.
        public byte[] aunMacReceivedBytes()
        {
            return stMacRxPacket.aunMacRxPacketBuff;
        }// End of the function: aunMacReceivedBytes
    }// End of the class: MacPacket
}