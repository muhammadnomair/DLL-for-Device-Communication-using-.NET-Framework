namespace DeviceTelemetryDLL.Models
{
    // Class contains all the parametres of the device heartbeat telemetry
    public  class DeviceTelemetry
    {
        public GeneralParameters generalParams { get; set; }
        public ThreePhasePortParameters threePhasePortParams { get; set;}
        public EVPortParameters evPortParams { get; set;}
        public LCSPortParameters lcsPortParams { get; set;}
        public WattAndWellParameters wattAndWellParams { get; set;}
    }// End of the class: TelemetryParameters
}
