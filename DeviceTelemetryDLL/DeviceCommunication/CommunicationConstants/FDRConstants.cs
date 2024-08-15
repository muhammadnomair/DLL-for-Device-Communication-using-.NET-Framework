using System;
using System.Collections.Generic;

namespace DeviceTelemetryDLL.DeviceCommunication.CommunicationConstants
{
    public static class FDRConstants
    {
        // --- Constant values of PCAP_Functions class
        // Source and Destination MAC addresses and Ethernet packet-type
        public static UInt64 SrcMacAddress = 0x001122334455;
        public static UInt64 DestMacAddress = 0xFFFFFFFFFFFF;
        public static UInt16 EthernetPacketType = 0x0800; // changed from 0x6058 to 0x0008 

        // --- Constant values of MacPacket
        public const int UdpPacketHeaderSize = 28;  // UDP packet header length (31 bytes array)

        public const UInt16 PayLoadSize = 1024;
        public const UInt16 MaxEthPktSize = 1500;
        public const byte MacBufferSize = 5;
        public const byte MaxMacDeviceSupported = 5;
        public const byte MaxDeviceMacLength = 6;
        public const byte MacHeaderLength = 31;

        public static byte[] BroadCastMacAddress = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }; // MAC address for message broadcasting

        // --- Constant values of ApduCommunicator class
        // Total Commands, Ethernet Packet size 
        public const byte TotalApduCommands = 1; // one CommadCode based REQ (HEARTBEAT TELEMETRY) 
        public const UInt16 ApduMaxEthPktSize = 1500;

        // Limit of FDR Files in device's drive.
        public const UInt16 FdrFilesLimitInDrive = 255;

        // Declaring list of Board Names for OTA file transferring
        public static List<string> DeviceBoardNames = new List<string>
        {
            "CB",
            "DCGDD",
            "ACGDD",
            "PxLCS"
        };
        
        // list of Device Telemetry Fields (Sequentially header fields for csv file)
        // --- If you want to add new telemetry parameters for logging in csv file, add parameter name in list.
        public static List<string> DeviceTelemetryFields = new List<string> {
            // -- General Parameters Group --
            "Serial Number",
            "Time Stamp",
            "Non-Formatted Up-Time (General-Parameter)",
            "Formatted Up-Time (General-Parameter)",
            "PCB Temperature (C) (General-Parameter)",
            "Humidity (%) (General Parameter)",
            "LCS Status",
            "Power Module Temperatures (C)",
            "Firmware Version (General-Parameter)",
            "Communication Link Status",
            "Price (USD)",
            "Communication Status",
            "Connector Status",
            "PLL Frequency (Hz)",
            "Error Codes",
            "MPU State",
            "Supervisor State (General-Parameter)",
            "Supervisor SubState (General-Parameter)",
            "Calculated Efficiency (%)",
            "DC Voltage Requested (V)",
            "DC Current Requested (A)",
            "Session Peak DC Voltage (V)",
            "Session Peak DC Current (A)",
            "Maximum Voltage Limit (V)",
            "Maximum Current Limit (A)",
            "Rail Voltage (V) (General-Parameter)",
            "Fault Type (General-Parameter)",
            "FDR Count (General-Parameter)",

             // -- Three Phase Port Parameters Group --
            "Non-Formatted Up-Time (3-Phase)",
            "Formatted Up-Time (3-Phase)",
            "PCB Temperature (C) (3-Phase)",
            "Firmware Version (3-Phase)",
            "Humidity (%) (3-Phase)",
             "Main Current 1 (A) (3-Phase)",
             "Main Current 2 (A) (3-Phase)",
             "Main Current 3 (A) (3-Phase)",
             "Main Voltage 1 (V) (3-Phase)",
             "Main Voltage 2 (V) (3-Phase)",
             "Main Voltage 3 (V) (3-Phase)",
             "OTA Status (3-Phase)",
             "AC GDD State (3-Phase)",
             "Rail Voltages (V) (3-Phase)",
             "FDR Count (3-Phase)",
             "Fault Type (3-Phase)",
             "Instantaneous Power (W) (3-Phase)",
            
            // -- EV Port Parameters --
            "Non-Formatted Up-Time (EV-Port)",
            "Formatted Up-Time (EV-Port)",
            "PCB Temperature (C) (EV-Port)",
            "Firmware Version (EV-Port)",
            "Humidity (%) (EV-Port)",
            "Current (A) (EV-Port)",
            "Voltage 1 (V) (EV-Port)",
            "Voltage 2 (V) (EV-Port)",
            "OTA Status (EV-Port)",
            "DC GDD State (EV-Port)",
            "Rail Voltages (V) (EV-Port)",
            "FDR Count (EV-Port)",
            "Fault Type (EV-Port)",
            "Dispensed Energy (kWh) (EV-Port)",
            "Peak DC Voltage (V) (EV-Port)",
            "Instantaneous Power (W) (EV-Port)",

            // -- LCS Port Parameters --
            "Non-Formatted Up-Time (LCS-Port)",
            "Formatted Up-Time (LCS-Port)",
            "PCB Temperature (C) (LCS-Port)",
            "Firmware Version (LCS-Port)",
            "Humidity (%) (LCS-Port)",
            "Pump and Fan Status (LCS-Port)",
            "Contactor Status (LCS-Port)",
            "IO Expander Out Status (LCS-Port)",
            "IO Expander In Status (LCS-Port)",
            "Pump State (LCS-Port)",
            "Fan Speed (RPM) (LCS-Port)",
            "OTA Status (LCS-Port)",
            "GDD State (LCS-Port)",
            "Rail Voltages (V) (LCS-Port)",
            "FDR Count (LCS-Port)",
            "Fault Type (LCS-Port)",
            "Peak NTC Temperature (C) (LCS-Port)",
            "Contactor Switch Count (LCS-Port)",

            // --- Watt and Well Teleemtry Parameters
            "Charging State Of Car Battery (%)",
            "Tarrif (USD)",
            "Bill Amount (USD)",
            "Charger Availability",
            "CCS-Cable Temperature 1 (C)",
            "CCS-Cable Temperature 2 (C)",
            "Error Code",
            "Session Number",
            "Session End Reason",
            "Watt-Well Firmware Version",
            "PM Version",
            "Chipset Version",
            "MPU Version",
            "Supervisor Version",
            "Watt-Well Board Serial Number",
            "Cellular Connectivity Status",
            "Cellular RSSI",
            "Ethernet Connectivity Status",
            "WiFi RSSI",
            "Wi-Fi Connectivity Status",
            "Primary Connectivity Source",
            "Supervisor State",
            "Chipset State",
            "Chipset SubState",
            "LCD Connection Status",
            "Supervisor Active",
            "Chipset Active",
            "MPU Active",
            "Car Connection Status",
        };//End of list of strings: DeviceTelemetryFields

        // array 28 bytes for UDP packet Header values
        public static readonly byte[] RequestUdpHeader =
        {
            //69, 0, 0, 28, 0, 1, 0, 0, 64, 17, 68, 155, 2, 2, 2, 2, 25, 25, 25, 25, 0, 0, 0, 0, 0, 8, 201, 168 // Decimal values
            // Hex values
            0x45, 0x00, 0x00, 0x1c, 0x00, 0x01, 0x00, 0x00, 0x40, 0x11, 0x44, 0x9b, 0x02, 0x02, 0x02, 0x02, 0x19, 0x19, 0x19, 0x19,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0xc9, 0xa8,
        };

        // UDP header values in Response packet
        public static readonly byte[] ResponseUdpHeader =
        {
            0x45, 0x00, 0x00, 0x1c, 0x00, 0x01, 0x00, 0x00, 0x40, 0x11, 0x40, 0x97, 0x04, 0x04, 0x04, 0x04, 0x19, 0x19, 0x19, 0x19, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08,
            0xfb, 0xfb,
        };
    }
}
