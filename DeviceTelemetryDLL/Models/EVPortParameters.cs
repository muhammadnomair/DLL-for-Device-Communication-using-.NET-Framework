using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceTelemetryDLL.Models
{
    // Class contains telemetry parameters of EV Port group
    public class EVPortParameters
    {
        public string UpTime { get; set; }
        public string PCBTemperature { get; set; }
        public string FirmwareVersion { get; set; }
        public string Humidity { get; set; }
        public string PowerModuleTemperatures { get; set; }
        public string Current { get; set; }
        public string Voltage_1 { get; set; }
        public string Voltage_2 { get; set; }
        public string OTAStatus { get; set; }
        public string DCGDDState { get; set; }
        public string RailVoltages { get; set; }
        public string FDRCount { get; set; }
        public string FaultType { get; set; }
        public string DispensedEnergy { get; set; }
        public string PeakDCVoltage { get; set; }
        public string InstantaneousPower { get; set; }
    }// End of the class: EVPortParameters
}
