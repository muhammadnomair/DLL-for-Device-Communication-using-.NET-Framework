
namespace DeviceTelemetryDLL.Models
{
    // Class contains telemetry parameters of Three-Phase Port group
    public class ThreePhasePortParameters
    {
        public string UpTime { get; set; }
        public string PCBTemperature { get; set; }
        public string FirmwareVersion { get; set; }
        public string Humidity { get; set; }
        public string PowerModuleTemperatures { get; set; }
        public string MainCurrent_1 { get; set; }
        public string MainCurrent_2 { get; set; }
        public string MainCurrent_3 { get; set; }
        public string MainVoltage_1 { get; set; }
        public string MainVoltage_2 { get; set; }
        public string MainVoltage_3 { get; set; }
        public string OTAStatus { get; set; }
        public string ACGDDState { get; set; }
        public string RailVoltages { get; set; }
        public string FDRCount { get; set; }
        public string FaultType { get; set; }
        public string InstantaneousPower { get; set; }
    }// End of the class: ThreePhasePortParameters
}
