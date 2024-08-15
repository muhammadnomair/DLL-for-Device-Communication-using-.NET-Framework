using DeviceTelemetryDLL.DeviceCommunication.CommunicationConstants;
using DeviceTelemetryDLL.DeviceCommunication.MacLayer;
using SharpPcap;
using Buffer = System.Buffer;

namespace DeviceTelemetryDLL.DeviceCommunication.PcapLayer
{
    #region: Declaring fields in packets structure 
    // Structure fields of Receiving (RX) Packet stucture at Pcap level (Network interface level)
    public struct PCAP_RX_PACKET_STRUCT
    {
        public byte[] aunPcapRxBuff; // Receive packet buffer at PCAP level
        public int lRxPacketLen; // Received packet length
    }

    // Defining fields of Ethernet Layer structure
    public struct EthernetLayer
    {
        public byte[] Source;
        public byte[] Destination;
        public UInt16 udEtherType;

        // array of bytes for UDP packet header values
        public byte[] UDP_PacketHeader; // 28 bytes
    }

    // structure fields of PCAP Packet (Network Interface level)
    public struct PCAP_PACKET_STRUCT
    {
        public byte[] aunDestination;
        public byte[] aunSource;
        public UInt16 udEtherType;

        // array of bytes for UDP packet header values
        public byte[] UDP_PacketHeader; // 28 bytes

        public UInt32 ulCmdCode;
        public UInt16 udPcapPayloadLen;
        public byte[] aunPcapPayloadBuff;
    }
    #endregion: Declaring Packet Structures

    public delegate void CbFunction(object sender, CaptureEventArgs objEvent);

    /// <summary>
    /// Creating a new class: PcapInterface (Lower Level network interface class)
    /// This will receive all packet from the comunicating device (Embedding device) at network interface and pre-process them and sends them to MAC layer above
    /// </summary>
    public class PcapInterface
    {
        // declaring an object of structure
        PCAP_RX_PACKET_STRUCT stPcapRxPacket;

        PCAP_PACKET_STRUCT stPcapPacket;

        // declaring an object of structure
        EthernetLayer EthernetLayer;

        // Check where the response contains valid UDP packet header value or not (return true/false)
        private bool bPcapIsValid_UDPHeader_InResponse(ref byte[] aunRxPacket, UInt16 udOffset)
        {
            bool bResult = false;
            try
            {
                // getting 28 bytes (UDP packet header value) from response packet
                byte[] responseUDP_Packet_Header = new byte[28];
                Buffer.BlockCopy(aunRxPacket, udOffset, responseUDP_Packet_Header, 0, 28); // copying 28 bytes

                // are two UDP header packets (two byte arrays) are equal?
                if (responseUDP_Packet_Header.SequenceEqual(FDRConstants.ResponseUdpHeader))
                    bResult = true;
            }
            catch (Exception ex)
            {
                // show exception message
                Console.WriteLine($"An Error occured while checking the validity of UDP packet-header value in bPcapIsValid_UDPHeader_InResponse method:\n{ex.Message}");
            }
            return bResult;
        }// End of the function: bPcapIsValid_UDPHeader_InResponse

        // This function returns true/false based on the validity of an Ethernet type from the received packet.
        private bool bPcapIsValidEtherType(ref byte[] aunRxPacket, UInt16 udOffset)
        {
            bool bResult = false;
            try
            {
                UInt16 udEtherType = (UInt16)(aunRxPacket[udOffset] << 8 | (aunRxPacket[udOffset + 1]));

                // check wheter resposne contains valid Ethernet Type value
                if (udEtherType == EthernetLayer.udEtherType)
                    bResult = true;
            }
            catch (Exception ex)
            {
                // show exception message
                Console.WriteLine($"An Error occurred while checking the validity of EthernetType in bPcapIsValidEtherType method:\n{ex.Message}");

                //ExceptionsLogger.WriteLogMessageInFile();
            }
            return bResult;
        }// End of the function: bPcapIsValidEtherType

        // copying UDP packet header from constant in Ethernet Layer's UDP_PacketHeader (byte array)
        private void vPcapSetupUDP_Packetheader(byte[] udp_packet_header_values)
        {
            // travering each byte value in UDP Packet header array 
            for (int byteIndex = 0; byteIndex < FDRConstants.UdpPacketHeaderSize; byteIndex++)
            {
                EthernetLayer.UDP_PacketHeader[byteIndex] = udp_packet_header_values[byteIndex];
            }
        }// End of the function: vPcapSetupUDP_Packetheader

        // Setting Destination address for PCAP
        private void vPcapSetupDestinationAddress(UInt64 ullDestAddress)
        {
            //>! Ref: 0x0000ABCDEFFF;
            EthernetLayer.Destination[0] = (byte)((ullDestAddress >> 40) & 0xFF);
            EthernetLayer.Destination[1] = (byte)((ullDestAddress >> 32) & 0xFF);
            EthernetLayer.Destination[2] = (byte)((ullDestAddress >> 24) & 0xFF);
            EthernetLayer.Destination[3] = (byte)((ullDestAddress >> 16) & 0xFF);
            EthernetLayer.Destination[4] = (byte)((ullDestAddress >> 8) & 0xFF);
            EthernetLayer.Destination[5] = (byte)(ullDestAddress & 0xFF);
        }// End of the function: vPcapSetupDestinationAddress

        // Setting Source address for PCAP
        private void vPcapSetupSourceAddress(UInt64 ullSrcAddress)
        {
            //>! Ref: 0x0000ABCDEFFF;
            EthernetLayer.Source[0] = (byte)((ullSrcAddress >> 40) & 0xFF);
            EthernetLayer.Source[1] = (byte)((ullSrcAddress >> 32) & 0xFF);
            EthernetLayer.Source[2] = (byte)((ullSrcAddress >> 24) & 0xFF);
            EthernetLayer.Source[3] = (byte)((ullSrcAddress >> 16) & 0xFF);
            EthernetLayer.Source[4] = (byte)((ullSrcAddress >> 8) & 0xFF);
            EthernetLayer.Source[5] = (byte)(ullSrcAddress & 0xFF);
        }// End of the function: vPcapSetupSourceAddress

        // This function displays the available Network Interface and returns the selected Network Interface.
        private ICaptureDevice objPcapGetAvailableNetworkAdapter(int selectedInterfaceIndex)
        {
            ICaptureDevice objSelectedInterface = null;
            try
            {
                // capturing attached network interface list
                var objDetectedDeviceList = CaptureDeviceList.Instance;

                // getting one network interface based on the provided index
                objSelectedInterface = objDetectedDeviceList[ selectedInterfaceIndex ];
            }
            catch (Exception ex)
            {
                // save log message
                Console.WriteLine($"An error occurred while getting available network adaptors in the objPcapGetAvailableNetworkAdapter method:\n{ex.Message}");
            }
            return objSelectedInterface;
        }// End of the function: objPcapGetAvailableNetworkAdapter

        // This function selects and initializes the PCAP Interface and opens PCAP stream for communication.
        public void vPcapInterfaceInit(int selectedInterfaceIndex)
        {
            try
            {
                // Take the selected adapter
                PcapCommunicator.Communicator = objPcapGetAvailableNetworkAdapter(selectedInterfaceIndex);

                // selected network interface is found successfully
                if (PcapCommunicator.Communicator != null)
                {
                    // save information logs in file
                    Console.WriteLine($"*** The application has successfully made connection with device through the selected network interface ({PcapCommunicator.Communicator.Description}) ***.");
                }
                else
                {
                    Console.WriteLine("ERROR: No Network Interface Found!");
                    Console.WriteLine($"An error occured in the vPcapInterfaceInit method: No network interface found!");
                    //Environment.Exit(0);
                    return;
                }

                // initializing Ethernet layer structure (packet)
                EthernetLayer.Source = new byte[6];
                EthernetLayer.Destination = new byte[6];
                EthernetLayer.UDP_PacketHeader = new byte[FDRConstants.UdpPacketHeaderSize]; // initializing 28 bytes array for UDP_Packet header

                // setting up source and destination MAC addresses
                vPcapSetupDestinationAddress(FDRConstants.DestMacAddress); // 255:255:255:255:255:255
                vPcapSetupSourceAddress(FDRConstants.SrcMacAddress); // ); 0:17:34:51:68:85

                // setting (copying) UDP packet header bytes
                vPcapSetupUDP_Packetheader(FDRConstants.RequestUdpHeader);

                EthernetLayer.udEtherType = FDRConstants.EthernetPacketType; // Ether Packet Type (2 bytes)

                // initializing array for receive packet at PCAP layer
                stPcapRxPacket.aunPcapRxBuff = new byte[2048];

                // initializing PCAP packet structure
                stPcapPacket.aunDestination = new byte[6];
                stPcapPacket.aunSource = new byte[6];
                stPcapPacket.UDP_PacketHeader = new byte[FDRConstants.UdpPacketHeaderSize]; // 28 bytes for storing UDP packet header values

                stPcapPacket.aunPcapPayloadBuff = new byte[2048];

                PcapCommunicator.Communicator.Open(DeviceMode.Promiscuous, 1);
            }
            catch (Exception ex)
            {
                // save log message
                Console.WriteLine($"An error occured during PCAP interface initialization in the vPcapInterfaceInit method:\n{ex.Message}");
            }
        }// End of the function: vPcapInterfaceInit

        // This function receive packet at Network interface level from communicating device
        public bool PcapReceivePacket()
        {
            bool bResult = false;
            try
            {
                // getting next MAC packet using Pcap interface from Device
                RawCapture RxPacketResult = PcapCommunicator.Communicator.GetNextPacket(); // getting next MAC packet

                // TODO: (base packet size)
                if ((RxPacketResult != null) && (RxPacketResult.Data.Length != 0) && (RxPacketResult.Data.Length > 6 + 6 + 2)) // I have changed according to packet header length
                {
                    // getting time and length of each received packet
                    var time = RxPacketResult.Timeval.Date;
                    var len = RxPacketResult.Data.Length;

                    // if Ethernet Packet type is valid
                    bool bValidPcapPacket = bPcapIsValidEtherType(ref RxPacketResult.Data, 12);
                    bool bValidPcapUDP_PacketHeader = bPcapIsValid_UDPHeader_InResponse(ref RxPacketResult.Data, 14);

                    // if getting a valid response packet (Ethernet type and UDP packet header value)
                    if ((bValidPcapPacket == true) && (bValidPcapUDP_PacketHeader == true))
                    {
                        Buffer.BlockCopy(RxPacketResult.Data, 0, stPcapRxPacket.aunPcapRxBuff, 0, RxPacketResult.Data.Length);
                        stPcapRxPacket.lRxPacketLen = RxPacketResult.Data.Length;

                        // For getting only Comand Code (1 byte) from packet (at 42 index)
                        UInt32 commandCode = stPcapRxPacket.aunPcapRxBuff[42];

                        // --- capturing only response packets (filtering only Response packets of all REQUESTS) from device
                        if ( (commandCode == MacCmdCodes.ETH_RES_GET_HEARTBEAT) || (commandCode == MacCmdCodes.ETH_RES_FETCH_MAC) )
                        {
                            bResult = true;
                        }
                    }
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                // show exception message in console
                Console.WriteLine($"An Error occured while parsing (decoding) fields in response pakcet in PcapReceivePacket method:\n{ex.Message}");
            }
            return bResult;
        }// End of the function: PcapReceivePacket

        // This function returns the received bytes (of the received packet) on PCAP interface
        public byte[] aunPcapReceivedBytes()
        {
            return stPcapRxPacket.aunPcapRxBuff;
        }// End of the function: aunPcapReceivedBytes

        // This function returns the length of bytes of the packet received on PCAP interface
        public int aunPcapReceivedBytesLen()
        {
            return stPcapRxPacket.lRxPacketLen;
        }// End of the function: aunPcapReceivedBytesLen

        // This function sends packet on the Network Interface.
        public void vPcapSendPacket(byte[] aunPacketParam, Int32 lSize)
        {
            PcapCommunicator.Communicator.SendPacket(aunPacketParam, lSize);
        }// End of the function: vPcapSendPacket

        // Getting source address at PCAP layer
        public byte[] aunPcapGetSourceAddress()
        {
            return EthernetLayer.Source;
        }// End of the function: aunPcapGetSourceAddress

        // Getting destination address at PCAP layer
        public byte[] aunPcapGetDestinationAddress()
        {
            return EthernetLayer.Destination;
        }// End of the function: aunPcapGetDestinationAddress

        // Getting ethernet type (number) from EthernetLayer structure and returing it
        public UInt16 aunPcapGetEthernetType()
        {
            return EthernetLayer.udEtherType;
        }// End of the function: aunPcapGetEthernetType

        // Getting UDP packet header at PCAP layer
        public byte[] PcapGetUDP_PacketHeader()
        {
            return EthernetLayer.UDP_PacketHeader;
        }// End of the function: PcapGetUDP_PacketHeader
    }// End of the class: PcapInterface
}

