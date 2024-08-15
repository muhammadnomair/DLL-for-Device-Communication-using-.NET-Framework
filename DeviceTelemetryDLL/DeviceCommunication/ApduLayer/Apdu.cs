using CsvHelper.Configuration;
using CsvHelper;
using DeviceTelemetryDLL.DeviceCommunication.CommunicationConstants;
using DeviceTelemetryDLL.DeviceCommunication.MacLayer;
using DeviceTelemetryDLL.Enums.ApduRequestResponse;
using DeviceTelemetryDLL.Helpers.TelemetryUtil;
using DeviceTelemetryDLL.Helpers.TimerUtil;
using DeviceTelemetryDLL.Models;
using System.Globalization;
using DeviceTelemetryDLL.Helpers;

namespace DeviceTelemetryDLL.DeviceCommunication.ApduLayer
{
    #region: --- Section for declaring STRUCTURES
    // structure for HeartBeat Telemetry REQUEST
    public unsafe struct ST_HEARTBEAT_REQ
    {
        public byte commandCode;
        public byte[] systemDateTime;
        public fixed byte dummyData[21]; // 21 bytes for dummy data
    }

    // Command-based RESPONSE Packet structure
    public struct APDU_CMD_RES_PACKET_STRUCT
    {
        public byte[] aunDestination; // destination address
        public byte[] aunSource; // source address
        public UInt16 udEtherType; // Ethernet type

        public byte[] UDP_PacketHeaderValue; // array of 28 bytes 
        public byte ulApduCmdCode; // Command Code value (byte)
        public UInt16 udApduPayloadLen; // Payload (Buffer) length
        public byte[] aunApduPayloadBuff; // payload buffer (packet body)
    }

    // Device 'Heartbeat Telemetry Fields' fields
    public struct ST_DEVICE_HEARTBEAT_TELEMETRY
    {
        // --- List of General parameters
        public byte[] SerialNumber; // 12 bytes (byte array of 12 elements (1 x 12) )
        public UInt32 GeneralParams_UpTime;
        public float GeneralParams_PCBTemperature;
        public float GeneralParams_Humidity;
        public byte[] LCSStatus; // 4 bytes (byte array of 4 elementrs (1 x 4) )
        public float[] PowerModuleTemperatures; // 32 Bytes (float array of 8 elemens (4 x 8) )
        public byte[] GeneralParams_FirmwareVersion; // 4 bytes (byte array of 4 elements (1 x 4) )
        public byte[] CommunicationLinkStatus; // 8 Bytes (byte array of 8 elements (1 x 8) )
        public float Price;
        public byte CommunicationStatus;
        public byte ConnectorStatus;
        public float PLLFrequency;
        public byte[] ErrorCodes; // 4 bytes (byte array of 4 elements)
        public byte MPUState;
        public byte SupervisorState; // one byte
        public byte SupervisorSubState; // one byte
        public byte CalculatedEfficiency;
        public UInt16 DC_VoltageRequested;
        public UInt16 DC_CurrentRequested;
        public UInt16 SessionPeakDC_Voltage;
        public UInt16 SessionPeakDC_Current;
        public UInt16 MaxVoltageLimit;
        public UInt16 MaxCurentLimit;
        public float GeneralParams_RailVoltage; // Rail-Voltage value in General-parameter
        public byte GeneralParams_FaultType; // Fault-Type value in General-parameter
        public byte GeneralParams_FDRCount; // FDR Count value in General-parameter

        // --- List of Three Phase Port Parameters
        public UInt32 ThreePhase_UpTime;
        public float ThreePhase_PCBTemperature;
        public byte[] ThreePhase_FirmwareVersion; // 4 bytes (1 x 4)
        public float ThreePhase_Humidity;
        public float ThreePhase_MainCurrent_1;
        public float ThreePhase_MainCurrent_2;
        public float ThreePhase_MainCurrent_3;
        public float ThreePhase_MainVoltage_1;
        public float ThreePhase_MainVoltage_2;
        public float ThreePhase_MainVoltage_3;
        public byte ThreePhase_OTAStatus;
        public byte ThreePhase_ACGDDState;
        public byte[] ThreePhase_RailVoltages; // 2 bytes (byte array of 2 elements)
        public byte ThreePhase_FDRCount;
        public byte ThreePhase_FaultType;
        public float ThreePhase_InstantaneousPower; // 4 bytes (float)

        // --- List of EV Port Parameters
        public UInt32 EVPort_UpTime;
        public float EVPort_PCBTemperature;
        public byte[] EVPort_FirmwareVersion; // 4 bytes (1 x 4)
        public float EVPort_Humidity;
        public float EVPort_Current;
        public float EVPort_Voltage_1;
        public float EVPort_Voltage_2;
        public byte EVPort_OTAStatus;
        public byte EVPort_DCGDDState;
        public byte[] EVPort_RailVoltages; // 2 bytes (byte array of 2 elements)
        public byte EVPort_FDRCount;
        public byte EVPort_FaultType;
        public float EVPort_DispensedEnergy; // 4 bytes (float)
        public UInt16 EVPort_PeakDCVoltage;
        public float EVPort_InstantaneousPower; // 4 bytes (float)

        // --- List of LCS Port Parameters
        public UInt32 LCSPort_UpTime;
        public float LCSPort_PCBTemperature;
        public byte[] LCSPort_FirmwareVersion; // 4 bytes (1 x 4)
        public float LCSPort_Humidity;
        public UInt32 LCSPort_PumpAndFanStatus;
        public UInt32 LCSPort_ContactorStatus;
        public UInt16 LCSPort_IOExpanderOutStatus;
        public UInt16 LCSPort_IOExpanderInStatus;
        public byte[] LCSPort_PumpState; // 2 bytes (byte array of 2 elements)
        public UInt16[] LCSPort_FanRPM; // 8 bytes (array of Uint16 having 4 elements)
        public byte LCSPort_OTAStatus;
        public byte LCSPort_GDDState;
        public byte[] LCSPort_RailVoltages; // 2 bytes (byte array of 2 elements)
        public byte LCSPort_FDRCount;
        public byte LCSPort_FaultType;
        public byte LCSPort_PeakNTCTemperature;
        public byte LCSPort_ContactorSwitchCount;

        // Formatting telemetry parameters for saving in csv file
        public string formattedSerialNumber;
    }
    // Structure for Watt and Well Telemetry parameters
    public struct ST_WATT_AND_WELL_TELEMETRY
    {
        public byte ChargingStateOfCarBattery;
        public float Tarrif; // 4 bytes (float)
        public float BillAmount; // 4 bytes (float)
        public byte ChargerAvailability;
        public byte CCSCableTemperature_1;
        public byte CCSCableTemperature_2;
        public UInt32 ErrorCode;
        public UInt32 SessionNumber;
        public byte SessionEndReason;

        public byte[] WattWellFirmwareVersion;  // array of 4 bytes (1 x 4)
        public byte[] PMVersion; // array of 4 bytes (1 x 4)
        public byte[] ChipSetVersion; // array of 4 bytes (1 x 4)
        public byte[] MPUVersion; // array of 4 bytes (1 x 4)
        public byte[] SupervisorVersion; // array of 4 bytes (1 x 4)
        public byte[] WattWellBoardSerialNumber; // array of 4 bytes (1 x 4)

        public byte CellularConnectivityStatus;
        public byte CellularRSSI;
        public byte EthernetConnectivityStatus;
        public byte WiFiRSSI;
        public byte WiFiConnectivityStatus;
        public byte PrimaryConnectivitySource;
        public byte SupervisorState;
        public byte ChipSetState;
        public byte LCDConnectionStatus;
        public byte SupervisorActive;
        public byte ChipSetActive;
        public byte MPUActive;
        public byte CarConnectionStatus;
        public byte ChipSet_SubState;
    }
    #endregion: --- Declaring STRUCTURES

    /// <summary>
    /// Creating a new class: Apdu
    /// This class represents application layer (top layer), which generates Command Packet structure and recives MAC packet from the underlying layer (MAC layer)
    /// </summary>
    public class Apdu
    {
        #region: --- CONSTANT VARIABLES section
        public Thread thApduHandlerThread = null; // thread for APDU handler
        const UInt16 MaxEthPktSize = 1500; // Max Ethernet packet size
        private UInt16 udCmdLoopIndex2 = 0;
        #endregion: --- CONSTANT VARIABLES

        #region: --- Section for declaring STRUCTURES OBJECT
        public APDU_CMD_RES_PACKET_STRUCT stApduCmdResPkt = new APDU_CMD_RES_PACKET_STRUCT();
        public ST_HEARTBEAT_REQ stHeartbeatReq;
        public ST_DEVICE_HEARTBEAT_TELEMETRY stDeviceHeartBeatTelemetry;
        public ST_WATT_AND_WELL_TELEMETRY stWattAndWallTelemetry;
        #endregion: --- declaring STRUCTURES OBJECT

        // creating object of class: MacPacket
        public MacPacket objMacPkt = new MacPacket();

        // creating an object of model
        public DeviceTelemetry telemetryParametersObj;

        // Save Telemetry Response data in model
        public void SaveParmetersdataInModel(DeviceTelemetry teleParamsObj)
        {
            try
            {
                // -- For General Parameters
                GeneralParameters generalParamsObj = new GeneralParameters()
                {
                    DeviceSerialNumber = ParseUtil.formateDeviceSerailNumber(stDeviceHeartBeatTelemetry.SerialNumber), // formatted in string
                    UpTime = stDeviceHeartBeatTelemetry.GeneralParams_UpTime.ToString(),
                    PCBTemperature = stDeviceHeartBeatTelemetry.GeneralParams_PCBTemperature.ToString(),
                    FirmwareVersion = ParseUtil.formateFirmwareVersion(stDeviceHeartBeatTelemetry.GeneralParams_FirmwareVersion), // formatted in string
                    Humidity = stDeviceHeartBeatTelemetry.GeneralParams_Humidity.ToString(),
                    LCSStatus = ParseUtil.formateLCSStatus(stDeviceHeartBeatTelemetry.LCSStatus), // formatted in string
                    CommunicationLinkStatus = ParseUtil.formateCommunicationLinkStatus(stDeviceHeartBeatTelemetry.CommunicationLinkStatus), // formatted in string
                    Price = stDeviceHeartBeatTelemetry.Price.ToString(),
                    CommunicationStatus = stDeviceHeartBeatTelemetry.CommunicationStatus.ToString(),
                    ConnectorStatus = stDeviceHeartBeatTelemetry.ConnectorStatus.ToString(),
                    PLLFrequency = stDeviceHeartBeatTelemetry.PLLFrequency.ToString(),
                    ErrorCodes = ParseUtil.formateErrorCodesList(stDeviceHeartBeatTelemetry.ErrorCodes), // formatted in string
                    MPUState = stDeviceHeartBeatTelemetry.MPUState.ToString(),
                    SupervisorState = stDeviceHeartBeatTelemetry.SupervisorState.ToString(),
                    SupervisorSubState = stDeviceHeartBeatTelemetry.SupervisorSubState.ToString(),
                    CalculatedEfficiency = stDeviceHeartBeatTelemetry.CalculatedEfficiency.ToString(),
                    DCVoltageRequested = stDeviceHeartBeatTelemetry.DC_VoltageRequested.ToString(),
                    DCCurrentRequested = stDeviceHeartBeatTelemetry.DC_CurrentRequested.ToString(),
                    SessionPeakDCVoltage = stDeviceHeartBeatTelemetry.SessionPeakDC_Voltage.ToString(),
                    SessionPeakDCCurrent = stDeviceHeartBeatTelemetry.SessionPeakDC_Current.ToString(),
                    MaxVoltageLimit = stDeviceHeartBeatTelemetry.MaxVoltageLimit.ToString(),
                    MaxCurentLimit = stDeviceHeartBeatTelemetry.MaxCurentLimit.ToString(),
                    RailVoltage = stDeviceHeartBeatTelemetry.GeneralParams_RailVoltage.ToString(),
                    FaultType = stDeviceHeartBeatTelemetry.GeneralParams_FaultType.ToString(),
                    FDRCount = stDeviceHeartBeatTelemetry.GeneralParams_FDRCount.ToString(),
                };
                teleParamsObj.generalParams = generalParamsObj;

                // -- For Three Phase Port Parameters
                ThreePhasePortParameters threePhaseParamsObj = new ThreePhasePortParameters()
                {
                    UpTime = stDeviceHeartBeatTelemetry.ThreePhase_UpTime.ToString(),
                    PCBTemperature = stDeviceHeartBeatTelemetry.ThreePhase_PCBTemperature.ToString(),
                    FirmwareVersion = ParseUtil.formateFirmwareVersion(stDeviceHeartBeatTelemetry.ThreePhase_FirmwareVersion), // formatted in string
                    Humidity = stDeviceHeartBeatTelemetry.ThreePhase_Humidity.ToString(),
                    PowerModuleTemperatures = ParseUtil.formateAndGetPowerModuleTemperature(stDeviceHeartBeatTelemetry.PowerModuleTemperatures, temperatureIndex: 0), // sensors (1, 2) temperatures
                    MainCurrent_1 = stDeviceHeartBeatTelemetry.ThreePhase_MainCurrent_1.ToString(),
                    MainCurrent_2 = stDeviceHeartBeatTelemetry.ThreePhase_MainCurrent_2.ToString(),
                    MainCurrent_3 = stDeviceHeartBeatTelemetry.ThreePhase_MainCurrent_3.ToString(),
                    MainVoltage_1 = stDeviceHeartBeatTelemetry.ThreePhase_MainVoltage_1.ToString(),
                    MainVoltage_2 = stDeviceHeartBeatTelemetry.ThreePhase_MainVoltage_2.ToString(),
                    MainVoltage_3 = stDeviceHeartBeatTelemetry.ThreePhase_MainVoltage_3.ToString(),
                    OTAStatus = stDeviceHeartBeatTelemetry.ThreePhase_OTAStatus.ToString(),
                    ACGDDState = stDeviceHeartBeatTelemetry.ThreePhase_ACGDDState.ToString(),
                    RailVoltages = ParseUtil.formateRailVoltagesList(stDeviceHeartBeatTelemetry.ThreePhase_RailVoltages), // formatted in string
                    FDRCount = stDeviceHeartBeatTelemetry.ThreePhase_FDRCount.ToString(),
                    FaultType = stDeviceHeartBeatTelemetry.ThreePhase_FaultType.ToString(),
                    InstantaneousPower = stDeviceHeartBeatTelemetry.ThreePhase_InstantaneousPower.ToString(),
                };
                teleParamsObj.threePhasePortParams = threePhaseParamsObj;

                // -- For EV Port Parameters
                EVPortParameters evPortParamsObj = new EVPortParameters()
                {
                    UpTime = stDeviceHeartBeatTelemetry.EVPort_UpTime.ToString(),
                    PCBTemperature = stDeviceHeartBeatTelemetry.EVPort_PCBTemperature.ToString(),
                    FirmwareVersion = ParseUtil.formateFirmwareVersion(stDeviceHeartBeatTelemetry.EVPort_FirmwareVersion),
                    Humidity = stDeviceHeartBeatTelemetry.EVPort_Humidity.ToString(),
                    PowerModuleTemperatures = ParseUtil.formateAndGetPowerModuleTemperature(stDeviceHeartBeatTelemetry.PowerModuleTemperatures, temperatureIndex: 4), // sensors (5, 6) temperatures
                    Current = stDeviceHeartBeatTelemetry.EVPort_Current.ToString(),
                    Voltage_1 = stDeviceHeartBeatTelemetry.EVPort_Voltage_1.ToString(),
                    Voltage_2 = stDeviceHeartBeatTelemetry.EVPort_Voltage_2.ToString(),
                    OTAStatus = stDeviceHeartBeatTelemetry.EVPort_OTAStatus.ToString(),
                    DCGDDState = stDeviceHeartBeatTelemetry.EVPort_DCGDDState.ToString(),
                    RailVoltages = ParseUtil.formateRailVoltagesList(stDeviceHeartBeatTelemetry.EVPort_RailVoltages),
                    FDRCount = stDeviceHeartBeatTelemetry.EVPort_FDRCount.ToString(),
                    FaultType = stDeviceHeartBeatTelemetry.EVPort_FaultType.ToString(),
                    DispensedEnergy = stDeviceHeartBeatTelemetry.EVPort_DispensedEnergy.ToString(),
                    PeakDCVoltage = stDeviceHeartBeatTelemetry.EVPort_PeakDCVoltage.ToString(),
                    InstantaneousPower = stDeviceHeartBeatTelemetry.EVPort_InstantaneousPower.ToString(),
                };
                teleParamsObj.evPortParams = evPortParamsObj;

                // -- LCS Port Parameters
                LCSPortParameters lcsPortParamsObj = new LCSPortParameters()
                {
                    UpTime = stDeviceHeartBeatTelemetry.LCSPort_UpTime.ToString(),
                    PCBTemperature = stDeviceHeartBeatTelemetry.LCSPort_PCBTemperature.ToString(),
                    FirmwareVersion = ParseUtil.formateFirmwareVersion(stDeviceHeartBeatTelemetry.LCSPort_FirmwareVersion),
                    Humidity = stDeviceHeartBeatTelemetry.LCSPort_Humidity.ToString(),
                    PumpAndFanStatus = stDeviceHeartBeatTelemetry.LCSPort_PumpAndFanStatus.ToString(),
                    ContactorStatus = stDeviceHeartBeatTelemetry.LCSPort_ContactorStatus.ToString(),
                    IOExpanderOutStatus = stDeviceHeartBeatTelemetry.LCSPort_IOExpanderOutStatus.ToString(),
                    IOExpanderInStatus = stDeviceHeartBeatTelemetry.LCSPort_IOExpanderInStatus.ToString(),
                    PumpState = ParseUtil.formateEachPumpActualStateValue(stDeviceHeartBeatTelemetry.LCSPort_PumpState),
                    FanRPM = ParseUtil.formateFanRPMList(stDeviceHeartBeatTelemetry.LCSPort_FanRPM),
                    OTAStatus = stDeviceHeartBeatTelemetry.LCSPort_OTAStatus.ToString(),
                    GDDState = stDeviceHeartBeatTelemetry.LCSPort_GDDState.ToString(),
                    RailVoltages = ParseUtil.formateRailVoltagesList(stDeviceHeartBeatTelemetry.LCSPort_RailVoltages),
                    FDRCount = stDeviceHeartBeatTelemetry.LCSPort_FDRCount.ToString(),
                    FaultType = stDeviceHeartBeatTelemetry.LCSPort_FaultType.ToString(),
                    PeakNTCTemperature = stDeviceHeartBeatTelemetry.LCSPort_PeakNTCTemperature.ToString(),
                    ContactorSwitchCount = stDeviceHeartBeatTelemetry.LCSPort_ContactorSwitchCount.ToString(),
                };
                teleParamsObj.lcsPortParams = lcsPortParamsObj;

                // For Watt and Well Parameters
                WattAndWellParameters wattWellParamsObj = new WattAndWellParameters()
                {
                    ChargingStateOfCarBattery = stWattAndWallTelemetry.ChargingStateOfCarBattery.ToString(),
                    Tarrif = stWattAndWallTelemetry.Tarrif.ToString(),
                    BillAmount = stWattAndWallTelemetry.BillAmount.ToString(),
                    ChargerAvailability = stWattAndWallTelemetry.ChargerAvailability.ToString(),
                    CCSCableTemperature_1 = stWattAndWallTelemetry.CCSCableTemperature_1.ToString(),
                    CCSCableTemperature_2 = stWattAndWallTelemetry.CCSCableTemperature_2.ToString(),
                    ErrorCode = stWattAndWallTelemetry.ErrorCode.ToString(),
                    SessionNumber = stWattAndWallTelemetry.SessionNumber.ToString(),
                    SessionEndReason = stWattAndWallTelemetry.SessionEndReason.ToString(),
                    FirmwareVersion = ParseUtil.formateFirmwareVersion(stWattAndWallTelemetry.WattWellFirmwareVersion),
                    PMVersion = ParseUtil.formateFirmwareVersion(stWattAndWallTelemetry.PMVersion),
                    ChipSetVersion = ParseUtil.formateFirmwareVersion(stWattAndWallTelemetry.ChipSetVersion),
                    MPUVersion = ParseUtil.formateFirmwareVersion(stWattAndWallTelemetry.MPUVersion),
                    SupervisorVersion = ParseUtil.formateFirmwareVersion(stWattAndWallTelemetry.SupervisorVersion),
                    BoardSerialNumber = ParseUtil.formateFirmwareVersion(stWattAndWallTelemetry.WattWellBoardSerialNumber),
                    CellularConnectivityStatus = stWattAndWallTelemetry.CellularConnectivityStatus.ToString(),
                    CellularRSSI = stWattAndWallTelemetry.CellularRSSI.ToString(),
                    EthernetConnectivityStatus = stWattAndWallTelemetry.EthernetConnectivityStatus.ToString(),
                    WiFiRSSI = stWattAndWallTelemetry.WiFiRSSI.ToString(),
                    WiFiConnectivityStatus = stWattAndWallTelemetry.WiFiConnectivityStatus.ToString(),
                    PrimaryConnectivitySource = stWattAndWallTelemetry.PrimaryConnectivitySource.ToString(),
                    SupervisorState = stWattAndWallTelemetry.SupervisorState.ToString(),
                    ChipSetState = stWattAndWallTelemetry.ChipSetState.ToString(),
                    LCDConnectionStatus = stWattAndWallTelemetry.LCDConnectionStatus.ToString(),
                    SupervisorActive = stWattAndWallTelemetry.SupervisorActive.ToString(),
                    ChipSetActive = stWattAndWallTelemetry.ChipSetActive.ToString(),
                    MPUActive = stWattAndWallTelemetry.MPUActive.ToString(),
                    CarConnectionStatus = stWattAndWallTelemetry.CarConnectionStatus.ToString(),
                    ChipSet_SubState = stWattAndWallTelemetry.ChipSet_SubState.ToString(),
                };
                teleParamsObj.wattAndWellParams = wattWellParamsObj;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred while saving telemetry parameters data in model:\n{ex.Message}");
            }
        }// End of the function: SaveParmetersdataInModel

        // This function will parse (decode) general parameters group
        public void ParseHeartBeatGeneralParams(ref UInt32 ulIndex, ref byte[] aunApduBytes)
        {
            try
            {
                // Serail Number, list of 12 bytes
                stDeviceHeartBeatTelemetry.SerialNumber = new byte[12];
                for (int index = 0; index < 12; index++)
                {
                    stDeviceHeartBeatTelemetry.SerialNumber[index] = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);
                }
                // Device Up Time (4 bytes)
                stDeviceHeartBeatTelemetry.GeneralParams_UpTime = ParseUtil.Parseuint32(aunApduBytes, ref ulIndex);

                // PCB Temperature float (4 bytes)
                stDeviceHeartBeatTelemetry.GeneralParams_PCBTemperature = ParseUtil.Parsefloat32(aunApduBytes, ref ulIndex);

                // Humidity (4 byte)
                stDeviceHeartBeatTelemetry.GeneralParams_Humidity = ParseUtil.Parsefloat32(aunApduBytes, ref ulIndex);

                // LCS Status, list of 4 elements and each of 1 byte
                stDeviceHeartBeatTelemetry.LCSStatus = new byte[4];
                for (int index = 0; index < 4; index++)
                {
                    stDeviceHeartBeatTelemetry.LCSStatus[index] = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);
                }

                // Power Module Temperatures, list of size 8 elements (array of 8 elements) and each of float (4 bytes)
                stDeviceHeartBeatTelemetry.PowerModuleTemperatures = new float[8];
                for (int index = 0; index < 8; index++)
                {
                    stDeviceHeartBeatTelemetry.PowerModuleTemperatures[index] = ParseUtil.Parsefloat32(aunApduBytes, ref ulIndex);
                }

                // Firmware Version, list of 4 elements and each of 1 byte
                stDeviceHeartBeatTelemetry.GeneralParams_FirmwareVersion = new byte[4];
                for (int index = 0; index < 4; index++)
                {
                    stDeviceHeartBeatTelemetry.GeneralParams_FirmwareVersion[index] = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);
                }

                // Communication Status Link, list of 8 elements and each of 1 byte
                stDeviceHeartBeatTelemetry.CommunicationLinkStatus = new byte[8];
                for (int index = 0; index < 8; index++)
                {
                    stDeviceHeartBeatTelemetry.CommunicationLinkStatus[index] = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);
                }

                // Price value, float (4 byte)
                stDeviceHeartBeatTelemetry.Price = ParseUtil.Parsefloat32(aunApduBytes, ref ulIndex);

                // Communication Status (1 byte)
                stDeviceHeartBeatTelemetry.CommunicationStatus = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);

                // Status of Device Connector (1 byte)
                stDeviceHeartBeatTelemetry.ConnectorStatus = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);

                // PLL Frequency value, float (4 bytes)
                stDeviceHeartBeatTelemetry.PLLFrequency = ParseUtil.Parsefloat32(aunApduBytes, ref ulIndex);

                // Error Codes, list of 4 elements and each of 1 byte size
                stDeviceHeartBeatTelemetry.ErrorCodes = new byte[4];
                for (int index = 0; index < 4; index++)
                {
                    // decode and save each error code
                    stDeviceHeartBeatTelemetry.ErrorCodes[index] = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);
                }

                // MPU State value (1 byte)
                stDeviceHeartBeatTelemetry.MPUState = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);

                // Supervisor State value (1 byte)
                stDeviceHeartBeatTelemetry.SupervisorState = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);

                // Supervisor Sub-State value (1 byte)
                stDeviceHeartBeatTelemetry.SupervisorSubState = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);

                // Calculated Efficiency value (1 byte)
                stDeviceHeartBeatTelemetry.CalculatedEfficiency = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);

                // value of DC Voltage Requested, Uint16 (2 bytes)
                stDeviceHeartBeatTelemetry.DC_VoltageRequested = ParseUtil.Parseuint16(aunApduBytes, ref ulIndex);

                // value of DC Current Requested, Uint16 (2 bytes)
                stDeviceHeartBeatTelemetry.DC_CurrentRequested = ParseUtil.Parseuint16(aunApduBytes, ref ulIndex);

                // value of Session Peak DC Voltage, Uint16 (2 bytes)
                stDeviceHeartBeatTelemetry.SessionPeakDC_Voltage = ParseUtil.Parseuint16(aunApduBytes, ref ulIndex);

                // value of Session Peak DC Current, Uint16 (2 bytes)
                stDeviceHeartBeatTelemetry.SessionPeakDC_Current = ParseUtil.Parseuint16(aunApduBytes, ref ulIndex);

                // Max. Voltage limit, Uint16 (2 bytes)
                stDeviceHeartBeatTelemetry.MaxVoltageLimit = ParseUtil.Parseuint16(aunApduBytes, ref ulIndex);

                // Max. Current Limit, Uint16 (2 bytes)
                stDeviceHeartBeatTelemetry.MaxCurentLimit = ParseUtil.Parseuint16(aunApduBytes, ref ulIndex);

                // General Parameter Rail-Voltage value, float (4 bytes)
                stDeviceHeartBeatTelemetry.GeneralParams_RailVoltage = ParseUtil.Parsefloat32(aunApduBytes, ref ulIndex);

                // General Parameter Fault-Type value, (1 byte)
                stDeviceHeartBeatTelemetry.GeneralParams_FaultType = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);

                // General Parameter FDR-Count value, (1 byte)
                stDeviceHeartBeatTelemetry.GeneralParams_FDRCount = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An Error Occured while Parsing General Parameters in HeartBeat Teleemtry:\n{ex.Message}");
            }
        }// End of the function: ParseHeartBeatGeneralParams

        // This function will parse (decode) three phase port parameters group
        public void ParseHeartBeatThreePhasePortParams(ref UInt32 ulIndex, ref byte[] aunApduBytes)
        {
            try
            {
                // Up-Time value in Three-phase port, Uint32 (4 bytes)
                stDeviceHeartBeatTelemetry.ThreePhase_UpTime = ParseUtil.Parseuint32(aunApduBytes, ref ulIndex);

                // PCB temperature value in Three-phase port, float (4 bytes)
                stDeviceHeartBeatTelemetry.ThreePhase_PCBTemperature = ParseUtil.Parsefloat32(aunApduBytes, ref ulIndex);

                // Firmware Version value in Three-phase port, list of 4 elements and each of 1 byte
                stDeviceHeartBeatTelemetry.ThreePhase_FirmwareVersion = new byte[4];
                for (int index = 0; index < 4; index++)
                {
                    stDeviceHeartBeatTelemetry.ThreePhase_FirmwareVersion[index] = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);
                }

                // Humidity value in Three-phase port, float (4 bytes)
                stDeviceHeartBeatTelemetry.ThreePhase_Humidity = ParseUtil.Parsefloat32(aunApduBytes, ref ulIndex);

                // Main Current 1, float type (4 bytes)
                stDeviceHeartBeatTelemetry.ThreePhase_MainCurrent_1 = ParseUtil.Parsefloat32(aunApduBytes, ref ulIndex);

                // Main Current 2, float type (4 bytes)
                stDeviceHeartBeatTelemetry.ThreePhase_MainCurrent_2 = ParseUtil.Parsefloat32(aunApduBytes, ref ulIndex);

                // Main Current 3, float type (4 bytes)
                stDeviceHeartBeatTelemetry.ThreePhase_MainCurrent_3 = ParseUtil.Parsefloat32(aunApduBytes, ref ulIndex);

                // Main Voltage 1, float type (4 bytes)
                stDeviceHeartBeatTelemetry.ThreePhase_MainVoltage_1 = ParseUtil.Parsefloat32(aunApduBytes, ref ulIndex);

                // Main Voltage 2, float type (4 bytes)
                stDeviceHeartBeatTelemetry.ThreePhase_MainVoltage_2 = ParseUtil.Parsefloat32(aunApduBytes, ref ulIndex);

                // Main Voltage 3, float type (4 bytes)
                stDeviceHeartBeatTelemetry.ThreePhase_MainVoltage_3 = ParseUtil.Parsefloat32(aunApduBytes, ref ulIndex);

                // OTA Status in Three-phase Port, (1 byte)
                stDeviceHeartBeatTelemetry.ThreePhase_OTAStatus = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);

                // AC GDD status value in Three-phase port, (1 byte)
                stDeviceHeartBeatTelemetry.ThreePhase_ACGDDState = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);

                // Rail Voltages value in Three-phase port, list of 2 elements and each of 1 byte
                stDeviceHeartBeatTelemetry.ThreePhase_RailVoltages = new byte[2];
                for (int index = 0; index < 2; index++)
                {
                    stDeviceHeartBeatTelemetry.ThreePhase_RailVoltages[index] = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);
                }

                // FDR Count value in Three-phase Port, (1 byte)
                stDeviceHeartBeatTelemetry.ThreePhase_FDRCount = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);

                //Fault Type value in Three-phase Port, (1 byte)
                stDeviceHeartBeatTelemetry.ThreePhase_FaultType = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);

                // Instantaneous Power value in three-phase Port, float (4 bytes)
                stDeviceHeartBeatTelemetry.ThreePhase_InstantaneousPower = ParseUtil.Parsefloat32(aunApduBytes, ref ulIndex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An Error Occured while parsing Three-Phase Port Parameters in heartbeat Telemetry: {ex.Message}");
            }
        }// End of the function: ParseHeartBeatThreePhasePortParams

        // This function will parse (decode) EV Port parameters group
        public void ParseHeartBeatEVPortParams(ref UInt32 ulIndex, ref byte[] aunApduBytes)
        {
            try
            {
                // Up-Time value in EV-Port, Uint32 (4 bytes)
                stDeviceHeartBeatTelemetry.EVPort_UpTime = ParseUtil.Parseuint32(aunApduBytes, ref ulIndex);

                // PCB Temperature value in EV-Port, float (4 bytes)
                stDeviceHeartBeatTelemetry.EVPort_PCBTemperature = ParseUtil.Parsefloat32(aunApduBytes, ref ulIndex);

                // Firmware Version value in EV-Port, list of 4 elements and each of 1 byte
                stDeviceHeartBeatTelemetry.EVPort_FirmwareVersion = new byte[4];
                for (int index = 0; index < 4; index++)
                {
                    stDeviceHeartBeatTelemetry.EVPort_FirmwareVersion[index] = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);
                }

                // Humidity value in EV-Port, float (4 bytes)
                stDeviceHeartBeatTelemetry.EVPort_Humidity = ParseUtil.Parsefloat32(aunApduBytes, ref ulIndex);

                // Current value in EV-Port, float (4 bytes)
                stDeviceHeartBeatTelemetry.EVPort_Current = ParseUtil.Parsefloat32(aunApduBytes, ref ulIndex);

                // Voltage value in EV-Port, float type (4 bytes)
                stDeviceHeartBeatTelemetry.EVPort_Voltage_1 = ParseUtil.Parsefloat32(aunApduBytes, ref ulIndex);

                // Voltage value in EV-Port, float type (4 bytes)
                stDeviceHeartBeatTelemetry.EVPort_Voltage_2 = ParseUtil.Parsefloat32(aunApduBytes, ref ulIndex);

                // OTA Status in EV-Port, (1 byte)
                stDeviceHeartBeatTelemetry.EVPort_OTAStatus = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);

                // DC GDD status value in EV-Port, (1 byte)
                stDeviceHeartBeatTelemetry.EVPort_DCGDDState = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);

                // Rail Voltages value in EV-Port, list of 2 elements and each of 1 byte
                stDeviceHeartBeatTelemetry.EVPort_RailVoltages = new byte[2];
                for (int index = 0; index < 2; index++)
                {
                    stDeviceHeartBeatTelemetry.EVPort_RailVoltages[index] = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);
                }

                // FDR Count value in EV-Port, (1 byte)
                stDeviceHeartBeatTelemetry.EVPort_FDRCount = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);

                //Fault Type value in EV-Port, (1 byte)
                stDeviceHeartBeatTelemetry.EVPort_FaultType = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);

                // Dispensed Energy value in EV-Port, float (4 bytes) 
                stDeviceHeartBeatTelemetry.EVPort_DispensedEnergy = ParseUtil.Parsefloat32(aunApduBytes, ref ulIndex);

                //Peak DC Voltage value in EV-Port, Uint16 (2 bytes)
                stDeviceHeartBeatTelemetry.EVPort_PeakDCVoltage = ParseUtil.Parseuint16(aunApduBytes, ref ulIndex);

                // Instantenous Power value in EV-Port, float (4 bytes) 
                stDeviceHeartBeatTelemetry.EVPort_InstantaneousPower = ParseUtil.Parsefloat32(aunApduBytes, ref ulIndex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An Error Occured while parsing EV-Port parameters in HeartBeat Telemetry: {ex.Message}");
            }
        }// End of the function: ParseHeartBeatEVPortParams

        // This function will parse (decode) LCS Port parameters group in HeartBeat telemetry
        public void ParseHeartBeatLCSPortParams(ref UInt32 ulIndex, ref byte[] aunApduBytes)
        {
            try
            {
                // Up-Time value in LCS-Port, Uint32 (4 bytes)
                stDeviceHeartBeatTelemetry.LCSPort_UpTime = ParseUtil.Parseuint32(aunApduBytes, ref ulIndex);

                // PCB Temperature value in LCS-Port, float (4 bytes)
                stDeviceHeartBeatTelemetry.LCSPort_PCBTemperature = ParseUtil.Parsefloat32(aunApduBytes, ref ulIndex);

                // Firmware Version value in LCS-Port, list of 4 elements and each of 1 byte
                stDeviceHeartBeatTelemetry.LCSPort_FirmwareVersion = new byte[4];
                for (int index = 0; index < 4; index++)
                {
                    stDeviceHeartBeatTelemetry.LCSPort_FirmwareVersion[index] = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);
                }

                // Humidity value in LCS-Port, float (4 bytes)
                stDeviceHeartBeatTelemetry.LCSPort_Humidity = ParseUtil.Parsefloat32(aunApduBytes, ref ulIndex);

                // Pump and Fan Status value in LCS-Port, Uint32 (4 bytes)
                stDeviceHeartBeatTelemetry.LCSPort_PumpAndFanStatus = ParseUtil.Parseuint32(aunApduBytes, ref ulIndex);

                // Contactor Status value in LCS-Port, Uint32 (4 bytes)
                stDeviceHeartBeatTelemetry.LCSPort_ContactorStatus = ParseUtil.Parseuint32(aunApduBytes, ref ulIndex);

                // IO-Expander-Out Status value in LCS-Port, Uint16 (2 bytes)
                stDeviceHeartBeatTelemetry.LCSPort_IOExpanderOutStatus = ParseUtil.Parseuint16(aunApduBytes, ref ulIndex);

                // IO-Expander-In Status value in LCS-Port, Uint16 (2 bytes)
                stDeviceHeartBeatTelemetry.LCSPort_IOExpanderInStatus = ParseUtil.Parseuint16(aunApduBytes, ref ulIndex);

                // Pump State value in LCS-Port, list of 2 elements and each of 1 byte
                stDeviceHeartBeatTelemetry.LCSPort_PumpState = new byte[2];
                for (int index = 0; index < 2; index++)
                {
                    stDeviceHeartBeatTelemetry.LCSPort_PumpState[index] = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);
                }

                // Fan RPM value in LCS-Port, list of 4 elements and each of (Uint16 = 2 bytes)
                stDeviceHeartBeatTelemetry.LCSPort_FanRPM = new UInt16[4];
                for (int index = 0; index < 4; index++)
                {
                    stDeviceHeartBeatTelemetry.LCSPort_FanRPM[index] = ParseUtil.Parseuint16(aunApduBytes, ref ulIndex);
                }

                // OTA Status in LCS-Port, (1 byte)
                stDeviceHeartBeatTelemetry.LCSPort_OTAStatus = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);

                // GDD status value in LCS-Port, (1 byte)
                stDeviceHeartBeatTelemetry.LCSPort_GDDState = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);

                // Rail Voltages value in LCS-Port, list of 2 elements and each of 1 byte
                stDeviceHeartBeatTelemetry.LCSPort_RailVoltages = new byte[2];
                for (int index = 0; index < 2; index++)
                {
                    stDeviceHeartBeatTelemetry.LCSPort_RailVoltages[index] = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);
                }

                // FDR Count value in LCS-Port, (1 byte)
                stDeviceHeartBeatTelemetry.LCSPort_FDRCount = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);

                // Fault Type value in LCS-Port, (1 byte)
                stDeviceHeartBeatTelemetry.LCSPort_FaultType = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);

                // Peak NTC Temperature value in LCS-Port, (1 byte)
                stDeviceHeartBeatTelemetry.LCSPort_PeakNTCTemperature = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);

                // Contactor Switch Count value in LCS-Port, (1 byte)
                stDeviceHeartBeatTelemetry.LCSPort_ContactorSwitchCount = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An Error Occured while parsing LCS Port Parameters in HeartBeat Telemetry: {ex.Message}");
            }
        }// End of the function: ParseHeartBeatLCSPortParams

        // This function will parse (decode) Watt and Well parameters group in HeartBeat telemetry
        public void ParseHeartBeatWattAndWellParams(ref UInt32 ulIndex, ref byte[] aunApduBytes)
        {
            try
            {
                // current charging state (&) of car-battery
                stWattAndWallTelemetry.ChargingStateOfCarBattery = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);

                //Tarrif value, float(4 bytes)
                stWattAndWallTelemetry.Tarrif = ParseUtil.Parsefloat32(aunApduBytes, ref ulIndex);

                // Bill Amount value, float(4 bytes)
                stWattAndWallTelemetry.BillAmount = ParseUtil.Parsefloat32(aunApduBytes, ref ulIndex);

                // Charger Availability value
                stWattAndWallTelemetry.ChargerAvailability = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);

                // CCS Cable temperature 1 value
                stWattAndWallTelemetry.CCSCableTemperature_1 = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);

                // CCS Cable temperature 2 value
                stWattAndWallTelemetry.CCSCableTemperature_2 = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);

                // Error Code value, Uint32 (4 bytes)
                stWattAndWallTelemetry.ErrorCode = ParseUtil.Parseuint32(aunApduBytes, ref ulIndex);

                // Sessin Number value, Uint32 (4 bytes)
                stWattAndWallTelemetry.SessionNumber = ParseUtil.Parseuint32(aunApduBytes, ref ulIndex);

                // Session End Reason value
                stWattAndWallTelemetry.SessionEndReason = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);

                // Watt & Well Firmware Version, byte array (4 elements having onr byte each)
                stWattAndWallTelemetry.WattWellFirmwareVersion = new byte[4];
                for (int index = 0; index < stWattAndWallTelemetry.WattWellFirmwareVersion.Length; index++)
                {
                    stWattAndWallTelemetry.WattWellFirmwareVersion[index] = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);
                }

                // PM Version, byte array (4 elements having onr byte each)
                stWattAndWallTelemetry.PMVersion = new byte[4];
                for (int index = 0; index < stWattAndWallTelemetry.PMVersion.Length; index++)
                {
                    stWattAndWallTelemetry.PMVersion[index] = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);
                }

                // Chipset Version, byte array (4 elements having one byte each)
                stWattAndWallTelemetry.ChipSetVersion = new byte[4];
                for (int index = 0; index < stWattAndWallTelemetry.ChipSetVersion.Length; index++)
                {
                    stWattAndWallTelemetry.ChipSetVersion[index] = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);
                }

                // MPU Version, byte array (4 elements having one byte each)
                stWattAndWallTelemetry.MPUVersion = new byte[4];
                for (int index = 0; index < stWattAndWallTelemetry.MPUVersion.Length; index++)
                {
                    stWattAndWallTelemetry.MPUVersion[index] = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);
                }

                // Supervisor Version, byte array (4 elements having one byte each)
                stWattAndWallTelemetry.SupervisorVersion = new byte[4];
                for (int index = 0; index < stWattAndWallTelemetry.SupervisorVersion.Length; index++)
                {
                    stWattAndWallTelemetry.SupervisorVersion[index] = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);
                }

                // Watt & Well Board SerailNumber, byte array (4 elements having one byte each)
                stWattAndWallTelemetry.WattWellBoardSerialNumber = new byte[4];
                for (int index = 0; index < stWattAndWallTelemetry.WattWellBoardSerialNumber.Length; index++)
                {
                    stWattAndWallTelemetry.WattWellBoardSerialNumber[index] = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);
                }

                // Cellular Connectivity Status value
                stWattAndWallTelemetry.CellularConnectivityStatus = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);

                // Cellular RSSI value
                stWattAndWallTelemetry.CellularRSSI = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);

                // Ethernet Connectivity Status value
                stWattAndWallTelemetry.EthernetConnectivityStatus = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);

                // WiFi RSSI value
                stWattAndWallTelemetry.WiFiRSSI = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);

                // WiFi Connectivity Status value
                stWattAndWallTelemetry.WiFiConnectivityStatus = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);

                // Primary Connectivity Source value
                stWattAndWallTelemetry.PrimaryConnectivitySource = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);

                // Supervisor State value
                stWattAndWallTelemetry.SupervisorState = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);

                // Chipset State value
                stWattAndWallTelemetry.ChipSetState = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);

                // LCD Connection Status value
                stWattAndWallTelemetry.LCDConnectionStatus = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);

                // Supervisor Active value
                stWattAndWallTelemetry.SupervisorActive = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);

                // Chipset Active value
                stWattAndWallTelemetry.ChipSetActive = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);

                // MPU Active value
                stWattAndWallTelemetry.MPUActive = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);

                // Car Connection Status value
                stWattAndWallTelemetry.CarConnectionStatus = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);

                // Shipset Sub-State Value
                stWattAndWallTelemetry.ChipSet_SubState = ParseUtil.GetOneByteValue(aunApduBytes, ref ulIndex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while parsing Watt & Well Parameters group in HeartBeat Telemetry: {ex.Message}");
            }
        }// End of the function: ParseHeartBeatWattAndWellParams

        // Parsing the Payload section of the HeartBeat Resposne Message.
        public void ParseHeartBeatRespPayload(byte[] aunApduBytes)
        {
            try
            {
                UInt32 ulIndex = 0;
                // General Parameters of HeartBeat Telemetry
                ParseHeartBeatGeneralParams(ref ulIndex, ref aunApduBytes);

                // Three Phase Port Parameters of HeartBeat Telemetry
                ParseHeartBeatThreePhasePortParams(ref ulIndex, ref aunApduBytes);

                // EV Port Parameters of HeartBeat Telemetry
                ParseHeartBeatEVPortParams(ref ulIndex, ref aunApduBytes);

                // LCS Port Parameters of HeartBeat Telemetry
                ParseHeartBeatLCSPortParams(ref ulIndex, ref aunApduBytes);

                // Parsing Watt & Well parameters from response packet
                ParseHeartBeatWattAndWellParams(ref ulIndex, ref aunApduBytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An Error Occured during ParseHeartBeatRespPayload:\n{ex.Message}");
            }
        }// End of the function: ParseHeartBeatRespPayload

        // This function designs the request and populates the payload buffer of the REQUEST packet (for sending) based on Command Code value.
        private Int32 ApduBuiltCmdPayloadHandler(UInt32 ulCmdCode, UInt16 udCmdLoopIndex)
        {
            Int32 lResult = 0;
            try
            {
                // Populate the aunPayloadBuff and pass it to pktMacBuildCmdReq()
                byte[] aunPayloadBuff = new byte[1200];
                UInt16 udPayloadLen = 0;

                // ladder of different switch cases (based on Command Code)
                switch (ulCmdCode)
                {
                    // For Requesting HeartBeat Device Telemetry
                    case MacCmdCodes.ETH_REQ_GET_HEARTBEAT:
                        {
                            try
                            {
                                stHeartbeatReq.commandCode = (byte)ulCmdCode;

                                // populating the payload
                                UInt32 udIndex = 0;
                                aunPayloadBuff[udIndex] = stHeartbeatReq.commandCode;
                                udIndex += 1;
                                Buffer.BlockCopy(stHeartbeatReq.systemDateTime, 0, aunPayloadBuff, 1, stHeartbeatReq.systemDateTime.Length);
                                udIndex += 36;
                                udPayloadLen = 1 + 36;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"An error occurred while building (populating payload) request packet of HEARTBEAT TELEMETRY in the vMacApduResponseHandler method:\n{ex.Message}");
                            }
                        }
                        break;
                    // default case
                    default:
                        break;
                }// End of the switch statement

                // if request packet has a valid Command Code value (Other than ETH_NO_COMMAND)
                if (ulCmdCode != MacCmdCodes.ETH_NO_COMMAND)
                {
                    try
                    {
                        // Building MAC request packet
                        ApduCommunicator.stApduCmdReqPkt[udCmdLoopIndex].stApduParamters = objMacPkt.pktMacBuildCmdReq(ulCmdCode, aunPayloadBuff, udPayloadLen);
                        ApduCommunicator.stApduCmdReqPkt[udCmdLoopIndex].bApduCmdScheduled = true;
                        lResult = 1;
                    }
                    catch (Exception)
                    {
                        ApduCommunicator.stApduCmdReqPkt[udCmdLoopIndex].bApduCmdPayloadPacketBuilt = false;
                        ApduCommunicator.stApduCmdReqPkt[udCmdLoopIndex].bApduCmdScheduled = false;
                        lResult = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An Error Occured during lApduBuiltCmdPayloadHandler\n:{ex.Message}");
            }
            return lResult;
        }// End of the function: ApduBuiltCmdPayloadHandler

        // This function handles (initialize) APDU interface
        public void ApduInit(int selectedInterfaceIndex)
        {
            bool test = false;
            try
            {
                // MAC and PCAP interfaces (structures) initialization
                objMacPkt.vMacInterfaceInit(selectedInterfaceIndex);

                // initializing the request commands structure
                ApduCommunicator.ApduInitCommandStructure();

                // initializing APDU command response packet structure (Response Packet at APDU interface)
                stApduCmdResPkt.aunSource = new byte[6];
                stApduCmdResPkt.aunDestination = new byte[6];
                stApduCmdResPkt.UDP_PacketHeaderValue = new byte[FDRConstants.UdpPacketHeaderSize]; // array of 28 bytes for UDP packet header 
                stApduCmdResPkt.aunApduPayloadBuff = new byte[MaxEthPktSize];

                // starting Apdu thread
                ApduStartThread();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while initializing parameters in APDU layer in the vApduInit method:\n{ex.Message}");
            }
        }// End of the function: ApduInit
        #region: Srtarting APDU Thread Section
        // This function start the APDU Handler Thread.
        public void ApduStartThread()
        {
            try
            {
                if (thApduHandlerThread == null)
                {
                    thApduHandlerThread = new Thread(ApduHandler);
                    thApduHandlerThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An Error Occured during vApduStartThread:\n{ex.Message}");
            }
        }// End of the function: ApduStartThread

        // This function handles the APDU Commands Transmission (Tx) to Device & Response message (Rx) from device handling.
        private void ApduHandler()
        {
            try
            {
                while (true)
                {
                    // Designing APDU request with request command code value
                    MacApduRequestHandler();

                    // MAC Layer Packet Reception
                    objMacPkt.lMacRespReceivedHandler();

                    // Handling the Resposne Message Handler at APDU layer (application layer)
                    MacApduResponseHandler();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An Error Occured during vApduHandler:\n{ex.Message}");
            }
        }// End of the function: ApduHandler

        // This function handles the built-up & transmission logic for APDU Command.
        private void MacApduRequestHandler()
        {
            try
            {
                for (byte unIter = 0; unIter < FDRConstants.TotalApduCommands; unIter++)
                {
                    if ((ApduCommunicator.stApduCmdReqPkt[unIter].bApduCmdPayloadPacketBuilt == true))
                    {
                        UInt32 ulCmdCode = ApduCommunicator.stApduCmdReqPkt[unIter].ulApduCmdCode; // getting Command Code value, which is initialized earlier in ApduCommunicator class
                        ApduBuiltCmdPayloadHandler(ulCmdCode, unIter);
                    }

                    // sending the command-based request
                    ApduSendCmdRequestHandler(unIter);
                }

                udCmdLoopIndex2 += 1;
                if (udCmdLoopIndex2 >= FDRConstants.TotalApduCommands)
                {
                    udCmdLoopIndex2 = 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An Error Ocuured during vMacApduRequestHandler :{ex.Message}");
            }
        }// End of the function: MacApduRequestHandler

        // This function handles the APDU Command request for sending.
        private int ApduSendCmdRequestHandler(UInt16 udCmdLoopIndex)
        {
            try
            {
                if ((ApduCommunicator.stApduCmdReqPkt[udCmdLoopIndex].bApduCmdScheduled) == true && (ApduCommunicator.stApduCmdReqPkt[udCmdLoopIndex].bApduCmdPayloadPacketBuilt == true))
                {
                    ApduCommunicator.stApduCmdReqPkt[udCmdLoopIndex].bApduCmdPayloadPacketBuilt = false;
                    ApduCommunicator.stApduCmdReqPkt[udCmdLoopIndex].enSendReqStates = SendRequestStates.SendCmdReq;
                }
                switch (ApduCommunicator.stApduCmdReqPkt[udCmdLoopIndex].enSendReqStates)
                {
                    case SendRequestStates.SendCmdReq:
                        try
                        {
                            ApduCommunicator.stApduCmdReqPkt[udCmdLoopIndex].lApduCmdStatus = objMacPkt.lMacSendCmdReq(ApduCommunicator.stApduCmdReqPkt[udCmdLoopIndex].stApduParamters.aunMacPopulateBuff,
                                ApduCommunicator.stApduCmdReqPkt[udCmdLoopIndex].stApduParamters.udMacPopulatedDataLen);

                            //ApduCommunicator.stApduCmdReqPkt[udCmdLoopIndex].ulSendReqWaitRespTick = BaseTickTimer.ulDllApiTTmrRead1ms();
                            ApduCommunicator.stApduCmdReqPkt[udCmdLoopIndex].ulSendReqWaitRespTick = 1000; // hard-coded

                            ApduCommunicator.stApduCmdReqPkt[udCmdLoopIndex].enSendReqStates = SendRequestStates.WaitCmdRes;
                        }
                        catch (Exception)
                        {
                            ApduCommunicator.stApduCmdReqPkt[udCmdLoopIndex].enSendReqStates = SendRequestStates.SendReqAbort;
                            ApduCommunicator.stApduCmdReqPkt[udCmdLoopIndex].bApduCmdScheduled = false;

                            ApduCommunicator.stApduCmdReqPkt[udCmdLoopIndex].lApduCmdStatus = -2;
                        }
                        break;

                    case SendRequestStates.WaitCmdRes:

                        //>! Check if retry is need for a APDU Command or Not
                        if (ApduCommunicator.stApduCmdReqPkt[udCmdLoopIndex].unMaxCmdReqRetry == 0)
                        {
                            ApduCommunicator.stApduCmdReqPkt[udCmdLoopIndex].enSendReqStates = SendRequestStates.SendReqIdle;
                            ApduCommunicator.stApduCmdReqPkt[udCmdLoopIndex].bApduCmdScheduled = false;
                        }

                        //>! Check if APDU command response received
                        if (ApduCommunicator.stApduCmdReqPkt[udCmdLoopIndex].bApduCmdRespReceived == false)
                        {
                            if (BaseTickTimer.ulDllApiTTmrDiff1ms(ApduCommunicator.stApduCmdReqPkt[udCmdLoopIndex].ulSendReqWaitRespTick) >= ApduCommunicator.stApduCmdReqPkt[udCmdLoopIndex].udApduCmdRespTimeout)
                            {
                                //ApduCommunicator.stApduCmdReqPkt[udCmdLoopIndex].ulSendReqWaitRespTick = BaseTickTimer.ulDllApiTTmrRead1ms();
                                ApduCommunicator.stApduCmdReqPkt[udCmdLoopIndex].ulSendReqWaitRespTick = 1000; // hard-coded

                                ApduCommunicator.stApduCmdReqPkt[udCmdLoopIndex].unApduRetriesMade += 1; // update the retries...

                                if (ApduCommunicator.stApduCmdReqPkt[udCmdLoopIndex].unApduRetriesMade <= ApduCommunicator.stApduCmdReqPkt[udCmdLoopIndex].unMaxCmdReqRetry) // Make a retry
                                {
                                    ApduCommunicator.stApduCmdReqPkt[udCmdLoopIndex].enSendReqStates = SendRequestStates.SendCmdReq;
                                }
                                else // Retries exhausted
                                {
                                    ApduCommunicator.stApduCmdReqPkt[udCmdLoopIndex].unApduRetriesMade = 0; // Reset
                                    ApduCommunicator.stApduCmdReqPkt[udCmdLoopIndex].enSendReqStates = SendRequestStates.SendReqAbort;
                                    ApduCommunicator.stApduCmdReqPkt[udCmdLoopIndex].bApduCmdScheduled = false;

                                    ApduCommunicator.stApduCmdReqPkt[udCmdLoopIndex].lApduCmdStatus = -1;
                                }
                            }
                        }
                        else if (ApduCommunicator.stApduCmdReqPkt[udCmdLoopIndex].bApduCmdRespReceived == true) // Resp Received
                        {
                            ApduCommunicator.stApduCmdReqPkt[udCmdLoopIndex].bApduCmdRespReceived = false;

                            ApduCommunicator.stApduCmdReqPkt[udCmdLoopIndex].bApduCmdScheduled = false;
                            ApduCommunicator.stApduCmdReqPkt[udCmdLoopIndex].enSendReqStates = SendRequestStates.SendReqIdle;
                        }
                        break;

                    case SendRequestStates.SendReqAbort:

                        break;

                    case SendRequestStates.SendReqIdle:

                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An Error Occured during lApduSendCmdRequestHandler:\n{ex.Message}");
            }
            return ApduCommunicator.stApduCmdReqPkt[udCmdLoopIndex].lApduCmdStatus;
        }// End of the function: ApduSendCmdRequestHandler

        // function to handle (process) response message of MAC Packet received earlier
        private void MacApduResponseHandler()
        {
            byte[] aunApduBytes = null; // local bytes array for MAC packet buffer

            try
            {
                if (objMacPkt.bMacIsProcessingCompleted == true)
                {
                    objMacPkt.bMacIsProcessingCompleted = false;

                    // getting response buffer from MAC layer
                    aunApduBytes = objMacPkt.aunMacReceivedBytes();

                    // decoding (copying) data from bytes array (MAC Bytes) into APDU_CMD_RES_PACKET_STRUCT structure
                    ApduParseReceivedResponse(ref aunApduBytes, ref stApduCmdResPkt);

                    // ladder of Cases in Switch statement: Handle the Command Response.
                    switch ((UInt32)stApduCmdResPkt.ulApduCmdCode)
                    {
                        // Getting Response Message of HeartBeat Packet.
                        case MacCmdCodes.ETH_RES_GET_HEARTBEAT:
                            {
                                try
                                {
                                    // creating a new object of the model
                                    telemetryParametersObj = new DeviceTelemetry();

                                    // Calling function to parse Payload Buffer data (section) of the response message
                                    ParseHeartBeatRespPayload(stApduCmdResPkt.aunApduPayloadBuff);

                                    // saving the formatted serial number (formatted string will be used in logging)
                                    stDeviceHeartBeatTelemetry.formattedSerialNumber = ParseUtil.formateDeviceSerailNumber(stDeviceHeartBeatTelemetry.SerialNumber);

                                    // save parameters data in model
                                    SaveParmetersdataInModel(telemetryParametersObj);

                                    // save telemetry in csv

                                    AppendDataToCsv(telemetryParametersObj);

                                    // response received
                                    ApduCommunicator.stApduCmdReqPkt[MacReqCmdCodeIndex.ETH_REQ_GET_HEARTBEAT_INDEX].bApduCmdRespReceived = true;
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"An error occured while handling (parsing) the response of HEARTBEAT TELEMETRY in the vMacApduResponseHandler method:\n{ex.Message}");
                                }
                            }
                            break;
                    }// End of the switch statement
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An Error occured during vMacApduResponseHandler:\n{ex.Message}");
            }
        }// End of the function: MacApduResponseHandler

        private void AppendDataToCsv(DeviceTelemetry telemetryParametersObj)
        {
            try
            {
                // creating a 'FDR' folder, if not exist (FDR directory in MyDocuments Folder)
                if (!Directory.Exists($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\FDR"))
                    Directory.CreateDirectory($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\FDR");

                // if 'TelemetryFiles' folder, in base path, does not exist, careting it
                if (!Directory.Exists($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\FDR\\DLLDeviceTelemetry"))
                    Directory.CreateDirectory($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\FDR\\DLLDeviceTelemetry");

                // getting current date value (year, month and day)
                string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

                // if 'currentDate value' folder, in /TelemetryFiles/, does not exist, careting a new folder with current-date value
                if (!Directory.Exists($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\FDR\\DLLDeviceTelemetry\\{currentDate}"))
                    Directory.CreateDirectory($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\FDR\\DLLDeviceTelemetry\\{currentDate}");

                // extracting only Hour value (24-hours format) from datetime
                string currentDateWithTime = DateTime.Now.ToString("yyyy-MM-dd_HH-00-00");

                // csv filename for device telemtry data (logs) (file name consists of current date and current Hour )
                string fileName = $"DeviceTelemetryDLL_{currentDateWithTime}.csv";
                string csvFilePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\FDR\\DLLDeviceTelemetry\\{currentDate}\\{fileName}";

                bool fileExists = File.Exists(csvFilePath);

                if(fileExists == true)
                {
                    // if File does not exit already
                    if (IsFileOpen(csvFilePath) == false)
                    {
                        using (var writer = new StreamWriter(csvFilePath, append: true))
                        using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
                        {
                            NewLine = Environment.NewLine
                        }))
                        {
                            csv.Context.RegisterClassMap<DeviceTelemetryMapper>();
                            if (!fileExists)
                            {
                                csv.WriteHeader<DeviceTelemetry>();
                                csv.NextRecord();
                            }
                            csv.WriteRecord(telemetryParametersObj);
                            csv.NextRecord();
                        }
                    }
                }
                // file has been created already.
                else
                {
                    using (var writer = new StreamWriter(csvFilePath, append: true))
                    using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        NewLine = Environment.NewLine
                    }))
                    {
                        csv.Context.RegisterClassMap<DeviceTelemetryMapper>();
                        if (!fileExists)
                        {
                            csv.WriteHeader<DeviceTelemetry>();
                            csv.NextRecord();
                        }
                        csv.WriteRecord(telemetryParametersObj);
                        csv.NextRecord();
                    }
                }                     
            }
            catch (Exception ex)
            {
                Console.WriteLine();
            }
        }
        private bool IsFileOpen(string filePath)
        {
            FileStream stream = null;
            try
            {
                // Attempt to open the file with exclusive access
                stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None);
                return false; // File is not open by another process
            }
            catch (IOException)
            {
                // IOException indicates the file is in use by another process
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                // Ensure the file stream is properly closed if it was successfully opened
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }
        // Parsing the received bytes (from the MAC packet/structure) and populating values in the APDU response packet structure fields
        private void ApduParseReceivedResponse(ref byte[] aunReceivedBytes, ref APDU_CMD_RES_PACKET_STRUCT pstApduPacketParam)
        {
            try
            {
                UInt16 udIndex = 0;

                // Extracting Destination value and saving it in structure's field
                Buffer.BlockCopy(aunReceivedBytes, udIndex, pstApduPacketParam.aunDestination, 0, pstApduPacketParam.aunDestination.Length);
                udIndex += (UInt16)(pstApduPacketParam.aunDestination.Length);

                // Getting Source value and saving it in structure's field
                Buffer.BlockCopy(aunReceivedBytes, udIndex, pstApduPacketParam.aunSource, 0, pstApduPacketParam.aunSource.Length);
                udIndex += (UInt16)(pstApduPacketParam.aunSource.Length);

                // Getting Ethernet packet Type value 
                pstApduPacketParam.udEtherType = (UInt16)((aunReceivedBytes[udIndex] << 8) | (aunReceivedBytes[udIndex + 1]));
                udIndex += 2;

                // Getting UDP Packet Header value (array of 28 bytes) from MAC layer response
                Buffer.BlockCopy(aunReceivedBytes, udIndex, pstApduPacketParam.UDP_PacketHeaderValue, 0, pstApduPacketParam.UDP_PacketHeaderValue.Length);
                udIndex += (UInt16)(pstApduPacketParam.UDP_PacketHeaderValue.Length);

                // Getting Command Code value -> TODO: (CommandCode -> 1 bytes)
                pstApduPacketParam.ulApduCmdCode = aunReceivedBytes[udIndex];
                udIndex += 1;

                pstApduPacketParam.udApduPayloadLen = (UInt16)((aunReceivedBytes[udIndex] << 8) | (aunReceivedBytes[udIndex + 1]));// Getting Payload Length value
                udIndex += 2;

                // Getting payload Buffer value (Data) (Telemetry-Payload data)
                Buffer.BlockCopy(aunReceivedBytes, udIndex, pstApduPacketParam.aunApduPayloadBuff, 0, pstApduPacketParam.udApduPayloadLen);
                udIndex += pstApduPacketParam.udApduPayloadLen;

                //ApduPrintParsedPacket(ref pstApduPacketParam); // For testing purpose
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An Error Occured during vApduParseReceivedResponse:\n{ex.Message}");
            }
        }// End of the function: ApduParseReceivedResponse
        #endregion: APDU Starting Thread
    }// End of the class: Apdu
}
