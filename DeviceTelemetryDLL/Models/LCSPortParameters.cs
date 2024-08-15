﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceTelemetryDLL.Models
{
    // Class contains telemetry parameters of LCS Port group
    public class LCSPortParameters
    {
        public string UpTime { get; set; }
        public string PCBTemperature { get; set; }
        public string FirmwareVersion { get; set; }
        public string Humidity { get; set; }
        public string PumpAndFanStatus { get; set; }
        public string ContactorStatus { get; set; }
        public string IOExpanderOutStatus { get; set; }
        public string IOExpanderInStatus { get; set; }
        public string PumpState { get; set; }
        public string FanRPM { get; set; }
        public string OTAStatus { get; set; }
        public string GDDState { get; set; }
        public string RailVoltages { get; set; }
        public string FDRCount { get; set; }
        public string FaultType { get; set; }
        public string PeakNTCTemperature { get; set; }
        public string ContactorSwitchCount { get; set; }
    }// End of the class: LCSPortParameters
}
