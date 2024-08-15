using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceTelemetryDLL.Models
{
    // Class contains telemetry parameters of Watt-And_Well parameters group
    public class WattAndWellParameters
    {
        public string ChargingStateOfCarBattery { get; set; }
        public string Tarrif { get; set; }
        public string BillAmount { get; set; }
        public string ChargerAvailability { get; set; }
        public string CCSCableTemperature_1 { get; set; }
        public string CCSCableTemperature_2 { get; set; }
        public string ErrorCode { get; set; }
        public string SessionNumber { get; set; }
        public string SessionEndReason { get; set; }
        public string FirmwareVersion { get; set; }
        public string PMVersion { get; set; }
        public string ChipSetVersion { get; set; }
        public string MPUVersion { get; set; }
        public string SupervisorVersion { get; set; }
        public string BoardSerialNumber { get; set; }
        public string CellularConnectivityStatus { get; set; }
        public string CellularRSSI { get; set; }
        public string EthernetConnectivityStatus { get; set; }
        public string WiFiRSSI { get; set; }
        public string WiFiConnectivityStatus { get; set; }
        public string PrimaryConnectivitySource { get; set; }
        public string SupervisorState { get; set; }
        public string ChipSetState { get; set; }
        public string LCDConnectionStatus { get; set; }
        public string SupervisorActive { get; set; }
        public string ChipSetActive { get; set; }
        public string MPUActive { get; set; }
        public string CarConnectionStatus { get; set; }
        public string ChipSet_SubState { get; set; }
    }// End of the class: WattAndWellParameters
}
