using CsvHelper.Configuration.Attributes;

namespace DeviceTelemetryDLL.Models
{
    // Class contains telemetry parameters of General parameters group
    public  class GeneralParameters
    {
        public string DeviceSerialNumber { get; set; }
        public string UpTime { get; set; }
        public string PCBTemperature { get; set; }
        public string Humidity { get; set; }
        public string LCSStatus { get; set; }
        public string FirmwareVersion { get; set; }
        public string CommunicationLinkStatus { get; set; }
        public string Price { get; set; }
        public string CommunicationStatus { get; set; }
        public string ConnectorStatus { get; set; }
        public string PLLFrequency { get; set; }
        public string ErrorCodes { get; set; }
        public string MPUState { get; set; }
        public string SupervisorState { get; set; }
        public string SupervisorSubState { get; set; }
        public string CalculatedEfficiency { get; set; }
        public string DCVoltageRequested { get; set; }
        public string DCCurrentRequested { get; set; }
        public string SessionPeakDCVoltage { get; set; }
        public string SessionPeakDCCurrent { get; set; }
        public string MaxVoltageLimit { get; set; }
        public string MaxCurentLimit { get; set; }
        public string RailVoltage { get; set; }
        public string FaultType { get; set; }
        public string FDRCount { get; set; }
    }// End of the class: GeneralParameters
}
