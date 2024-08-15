using CsvHelper.Configuration;
using DeviceTelemetryDLL.Models;

namespace DeviceTelemetryDLL.Helpers
{
    public class DeviceTelemetryMapper : ClassMap<DeviceTelemetry>
    {
        public DeviceTelemetryMapper()
        {
            //General Params Group
            Map(m => m.generalParams.DeviceSerialNumber).Index(0).Name("GeneralParam_DeviceSerialNumber");
            Map(m => m.generalParams.UpTime).Index(1).Name("GeneralParam_UpTime");
            Map(m => m.generalParams.PCBTemperature).Index(2).Name("GeneralParam_PCBTemperature");
            Map(m => m.generalParams.Humidity).Index(3).Name("GeneralParam_Humidity");
            Map(m => m.generalParams.LCSStatus).Index(4).Name("GeneralParam_LCSStatus");
            Map(m => m.generalParams.FirmwareVersion).Index(5).Name("GeneralParam_FirmwareVersion");
            Map(m => m.generalParams.CommunicationLinkStatus).Index(6).Name("GeneralParam_CommunicationLinkStatus");
            Map(m => m.generalParams.Price).Index(7).Name("GeneralParam_Price");
            Map(m => m.generalParams.CommunicationStatus).Index(8).Name("GeneralParam_CommunicationStatus");
            Map(m => m.generalParams.ConnectorStatus).Index(9).Name("GeneralParam_ConnectorStatus");
            Map(m => m.generalParams.PLLFrequency).Index(10).Name("GeneralParam_PLLFrequency");
            Map(m => m.generalParams.ErrorCodes).Index(11).Name("GeneralParam_ErrorCodes");
            Map(m => m.generalParams.MPUState).Index(12).Name("GeneralParam_MPUState");
            Map(m => m.generalParams.SupervisorState).Index(13).Name("GeneralParam_SupervisorState");
            Map(m => m.generalParams.SupervisorSubState).Index(14).Name("GeneralParam_SupervisorSubState");
            Map(m => m.generalParams.CalculatedEfficiency).Index(15).Name("GeneralParam_CalculatedEfficiency");
            Map(m => m.generalParams.DCVoltageRequested).Index(16).Name("GeneralParam_DCVoltageRequested");
            Map(m => m.generalParams.DCCurrentRequested).Index(17).Name("GeneralParam_DCCurrentRequested");
            Map(m => m.generalParams.SessionPeakDCVoltage).Index(18).Name("GeneralParam_SessionPeakDCVoltage");
            Map(m => m.generalParams.SessionPeakDCCurrent).Index(19).Name("GeneralParam_SessionPeakDCCurrent");
            Map(m => m.generalParams.MaxVoltageLimit).Index(20).Name("GeneralParam_MaxVoltageLimit");
            Map(m => m.generalParams.MaxCurentLimit).Index(21).Name("GeneralParam_MaxCurentLimit");
            Map(m => m.generalParams.RailVoltage).Index(22).Name("GeneralParam_RailVoltage");
            Map(m => m.generalParams.FaultType).Index(23).Name("GeneralParam_FaultType");
            Map(m => m.generalParams.FDRCount).Index(24).Name("GeneralParam_FDRCount");

            //Three Pahse params group
            Map(m => m.threePhasePortParams.UpTime).Index(25).Name("ThreePhaseParam_UpTime");
            Map(m => m.threePhasePortParams.PCBTemperature).Index(26).Name("ThreePhaseParam_PCBTemperature");
            Map(m => m.threePhasePortParams.FirmwareVersion).Index(27).Name("ThreePhaseParam_FirmwareVersion");
            Map(m => m.threePhasePortParams.Humidity).Index(28).Name("ThreePhaseParam_Humidity");
            Map(m => m.threePhasePortParams.PowerModuleTemperatures).Index(29).Name("ThreePhaseParam_PowerModuleTemperatures");
            Map(m => m.threePhasePortParams.MainCurrent_1).Index(30).Name("ThreePhaseParam_MainCurrent_1");
            Map(m => m.threePhasePortParams.MainCurrent_2).Index(31).Name("ThreePhaseParam_MainCurrent_2");
            Map(m => m.threePhasePortParams.MainCurrent_3).Index(32).Name("ThreePhaseParam_MainCurrent_3");
            Map(m => m.threePhasePortParams.MainVoltage_1).Index(33).Name("ThreePhaseParam_MainVoltage_1");
            Map(m => m.threePhasePortParams.MainVoltage_2).Index(34).Name("ThreePhaseParam_MainVoltage_2");
            Map(m => m.threePhasePortParams.MainVoltage_3).Index(35).Name("ThreePhaseParam_MainVoltage_3");
            Map(m => m.threePhasePortParams.OTAStatus).Index(36).Name("ThreePhaseParam_OTAStatus");
            Map(m => m.threePhasePortParams.ACGDDState).Index(37).Name("ThreePhaseParam_ACGDDState");
            Map(m => m.threePhasePortParams.RailVoltages).Index(38).Name("ThreePhaseParam_RailVoltages");
            Map(m => m.threePhasePortParams.FDRCount).Index(39).Name("ThreePhaseParam_FDRCount");
            Map(m => m.threePhasePortParams.FaultType).Index(40).Name("ThreePhaseParam_FaultType");
            Map(m => m.threePhasePortParams.InstantaneousPower).Index(41).Name("ThreePhaseParam_InstantaneousPower");

            //EV Port Params Group
            Map(m => m.evPortParams.UpTime).Index(42).Name("EVPortParam_UpTime");
            Map(m => m.evPortParams.PCBTemperature).Index(43).Name("EVPortParam_PCBTemperature");
            Map(m => m.evPortParams.FirmwareVersion).Index(44).Name("EVPortParam_FirmwareVersion");
            Map(m => m.evPortParams.Humidity).Index(45).Name("EVPortParam_Humidity");
            Map(m => m.evPortParams.PowerModuleTemperatures).Index(46).Name("EVPortParam_PowerModuleTemperatures");
            Map(m => m.evPortParams.Current).Index(47).Name("EVPortParam_Current");
            Map(m => m.evPortParams.Voltage_1).Index(48).Name("EVPortParam_Voltage_1");
            Map(m => m.evPortParams.Voltage_2).Index(49).Name("EVPortParam_Voltage_2");
            Map(m => m.evPortParams.OTAStatus).Index(50).Name("EVPortParam_OTAStatus");
            Map(m => m.evPortParams.DCGDDState).Index(51).Name("EVPortParam_DCGDDState");
            Map(m => m.evPortParams.RailVoltages).Index(52).Name("EVPortParam_RailVoltages");
            Map(m => m.evPortParams.FDRCount).Index(53).Name("EVPortParam_FDRCount");
            Map(m => m.evPortParams.FaultType).Index(54).Name("EVPortParam_FaultType");
            Map(m => m.evPortParams.DispensedEnergy).Index(55).Name("EVPortParam_DispensedEnergy");
            Map(m => m.evPortParams.PeakDCVoltage).Index(56).Name("EVPortParam_PeakDCVoltage");
            Map(m => m.evPortParams.InstantaneousPower).Index(57).Name("EVPortParam_InstantaneousPower");

            //LCS Port Params Group
            Map(m => m.lcsPortParams.UpTime).Index(57).Name("LCSPortParam_UpTime");
            Map(m => m.lcsPortParams.PCBTemperature).Index(58).Name("LCSPortParam_PCBTemperature");
            Map(m => m.lcsPortParams.FirmwareVersion).Index(59).Name("LCSPortParam_FirmwareVersion");
            Map(m => m.lcsPortParams.Humidity).Index(60).Name("LCSPortParam_Humidity");
            Map(m => m.lcsPortParams.PumpAndFanStatus).Index(61).Name("LCSPortParam_PumpAndFanStatus");
            Map(m => m.lcsPortParams.ContactorStatus).Index(62).Name("LCSPortParam_ContactorStatus");
            Map(m => m.lcsPortParams.IOExpanderOutStatus).Index(63).Name("LCSPortParam_IOExpanderOutStatus");
            Map(m => m.lcsPortParams.IOExpanderInStatus).Index(64).Name("LCSPortParam_IOExpanderInStatus");
            Map(m => m.lcsPortParams.PumpState).Index(65).Name("LCSPortParam_PumpState");
            Map(m => m.lcsPortParams.FanRPM).Index(66).Name("LCSPortParam_FanRPM");
            Map(m => m.lcsPortParams.OTAStatus).Index(67).Name("LCSPortParam_OTAStatus");
            Map(m => m.lcsPortParams.GDDState).Index(68).Name("LCSPortParam_GDDState");
            Map(m => m.lcsPortParams.RailVoltages).Index(69).Name("LCSPortParam_RailVoltages");
            Map(m => m.lcsPortParams.FDRCount).Index(70).Name("LCSPortParam_FDRCount");
            Map(m => m.lcsPortParams.FaultType).Index(71).Name("LCSPortParam_FaultType");
            Map(m => m.lcsPortParams.PeakNTCTemperature).Index(72).Name("LCSPortParam_PeakNTCTemperature");
            Map(m => m.lcsPortParams.ContactorSwitchCount).Index(73).Name("LCSPortParam_ContactorSwitchCount");

            //Watt And Wall Params Group
            Map(m => m.wattAndWellParams.ChargingStateOfCarBattery).Index(74).Name("WattWellParam_ChargingStateOfCarBattery");
            Map(m => m.wattAndWellParams.Tarrif).Index(75).Name("WattWellParam_Tarrif");
            Map(m => m.wattAndWellParams.BillAmount).Index(76).Name("WattWellParam_BillAmount");
            Map(m => m.wattAndWellParams.ChargerAvailability).Index(77).Name("WattWellParam_ChargerAvailability");
            Map(m => m.wattAndWellParams.CCSCableTemperature_1).Index(78).Name("WattWellParam_CCSCableTemperature_1");
            Map(m => m.wattAndWellParams.CCSCableTemperature_2).Index(79).Name("WattWellParam_CCSCableTemperature_2");
            Map(m => m.wattAndWellParams.ErrorCode).Index(80).Name("WattWellParam_ErrorCode");
            Map(m => m.wattAndWellParams.SessionNumber).Index(81).Name("WattWellParam_SessionNumber");
            Map(m => m.wattAndWellParams.SessionEndReason).Index(82).Name("WattWellParam_SessionEndReason");
            Map(m => m.wattAndWellParams.FirmwareVersion).Index(83).Name("WattWellParam_FirmwareVersion");
            Map(m => m.wattAndWellParams.PMVersion).Index(84).Name("WattWellParam_PMVersion");
            Map(m => m.wattAndWellParams.ChipSetVersion).Index(85).Name("WattWellParam_ChipSetVersion");
            Map(m => m.wattAndWellParams.MPUVersion).Index(86).Name("WattWellParam_MPUVersion");
            Map(m => m.wattAndWellParams.SupervisorVersion).Index(87).Name("WattWellParam_SupervisorVersion");
            Map(m => m.wattAndWellParams.BoardSerialNumber).Index(88).Name("WattWellParam_BoardSerialNumber");
            Map(m => m.wattAndWellParams.CellularConnectivityStatus).Index(89).Name("WattWellParam_CellularConnectivityStatus");
            Map(m => m.wattAndWellParams.CellularRSSI).Index(90).Name("WattWellParam_CellularRSSI");
            Map(m => m.wattAndWellParams.EthernetConnectivityStatus).Index(91).Name("WattWellParam_EthernetConnectivityStatus");
            Map(m => m.wattAndWellParams.WiFiRSSI).Index(92).Name("WattWellParam_WiFiRSSI");
            Map(m => m.wattAndWellParams.WiFiConnectivityStatus).Index(93).Name("WattWellParam_WiFiConnectivityStatus");
            Map(m => m.wattAndWellParams.PrimaryConnectivitySource).Index(94).Name("WattWellParam_PrimaryConnectivitySource");
            Map(m => m.wattAndWellParams.SupervisorState).Index(95).Name("WattWellParam_SupervisorState");
            Map(m => m.wattAndWellParams.ChipSetState).Index(96).Name("WattWellParam_ChipSetState");
            Map(m => m.wattAndWellParams.LCDConnectionStatus).Index(97).Name("WattWellParam_LCDConnectionStatus");
            Map(m => m.wattAndWellParams.SupervisorActive).Index(98).Name("WattWellParam_SupervisorActive");
            Map(m => m.wattAndWellParams.ChipSetActive).Index(99).Name("WattWellParam_ChipSetActive");
            Map(m => m.wattAndWellParams.MPUActive).Index(100).Name("WattWellParam_MPUActive");
            Map(m => m.wattAndWellParams.CarConnectionStatus).Index(101).Name("WattWellParam_CarConnectionStatus");
            Map(m => m.wattAndWellParams.ChipSet_SubState).Index(102).Name("WattWellParam_ChipSet_SubState");
        }
    }
}
