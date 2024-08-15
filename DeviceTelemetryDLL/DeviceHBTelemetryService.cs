using System.Text;
using System.Timers;
using DeviceTelemetryDLL.DeviceCommunication.ApduLayer;
using DeviceTelemetryDLL.DeviceCommunication.MacLayer;
using SharpPcap;

namespace DeviceTelemetryDLL
{
    // creating a new class: DeviceHBTelemetryService
    public class DeviceHBTelemetryService
    {
        private System.Timers.Timer _heartBeatTeleReqTimer;
        private List<string> networkInterfaceDescription = new List<string>();

        // creating an object of class: Apdu
        Apdu objApdu = new Apdu();

        // class constructor
        public DeviceHBTelemetryService()
        {
        }// End of the class constructor

        // Call Back function, scheduling "REQUEST HeartBeat Telemetry" 
        private void CbHeartBeatTimer(object source, ElapsedEventArgs e)
        {
            try
            {
                objApdu.stHeartbeatReq.systemDateTime = new byte[36];

                DateTime dateTime = DateTime.Now;
                string dateString = dateTime.ToString($"dd MM yy HH:mm:ss.fff.ffffff.fffffff"); // precision upto nano seconds
                byte[] byteArray = Encoding.UTF8.GetBytes(dateString);

                // save date-time byte-array in structure
                objApdu.stHeartbeatReq.systemDateTime = byteArray;

                if (ApduCommunicator.stApduCmdReqPkt[MacReqCmdCodeIndex.ETH_REQ_GET_HEARTBEAT_INDEX].bApduCmdScheduled == false)
                {
                    ApduCommunicator.stApduCmdReqPkt[MacReqCmdCodeIndex.ETH_REQ_GET_HEARTBEAT_INDEX].bApduCmdPayloadPacketBuilt = true;
                }
            }
            catch(Exception ex) 
            {
                Console.WriteLine($"Exception occurred in the CbHeartBeatTimer method: {ex.Message}");
            }
        }// End of the function: CbHeartBeatTimer

        private void DigitizerTimerInit(double interval, ElapsedEventHandler callback)
        {
            _heartBeatTeleReqTimer = new System.Timers.Timer(interval);
            _heartBeatTeleReqTimer.Elapsed += callback;
            _heartBeatTeleReqTimer.AutoReset = true; // If you want the timer to repeat
            _heartBeatTeleReqTimer.Enabled = true;
        }// End of the function: InitializeTimer

        // print out attached network interfaces on console
        public void GetNetworkDeviceList()
        {
            try
            {
                if (networkInterfaceDescription.Count != 0)
                    networkInterfaceDescription.Clear();

                var objDetectedDeviceList = CaptureDeviceList.Instance;
                // If no any attached device detected (count =0)
                if (objDetectedDeviceList.Count < 1)
                {
                    Console.WriteLine("No any attached netowrk interfaces are found on this machine. You can not proceed further.");
                    return;
                }
                
                // get interface description
                foreach (var dev in objDetectedDeviceList)
                {
                    networkInterfaceDescription.Add(dev.Description);
                }

                // print interface name
                int interfaceId = 0;
                foreach (string interfaceDes in networkInterfaceDescription)
                {
                    Console.WriteLine($"{interfaceId} - {interfaceDes}");
                    interfaceId++;
                }
                Console.WriteLine("In order to select network interface, select the mentioned index above.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while getting attached network interfaces name:\n{ex.Message}");
            }
        }// End of the function: GetNetworkDeviceList

        // Entry point (function) to be exposed
        public void GetTelemetryResposneData(int selectedInterfaceIndex, int heartBeatTimer = 100) 
        {
            try
            {
                // is the provided network interface valid ?
                int interfacesCount = networkInterfaceDescription.Count;
                if ( (selectedInterfaceIndex < 0) || (selectedInterfaceIndex > interfacesCount) )
                {
                    Console.WriteLine($"In-valid networtk interface index {selectedInterfaceIndex}. Index should be between (0 and {interfacesCount - 1})");
                    return;
                }

                // is the provided HeartBeat Timer valid ? (should be between 100ms to 1000ms)
                if ( (heartBeatTimer < 100) || (heartBeatTimer > 1000) )
                {
                    Console.WriteLine($"In-valid time-interval value {heartBeatTimer}.\nHeartBeat Time-Interval value should between 100 and 1000.");
                    return;
                }

                // initializing BaseTick Timer
                //BaseTickTimer.lDllApiTimerInit();

                // enable timer for scheduling 'Device Heartbeat Telemetry'
                DigitizerTimerInit(heartBeatTimer, CbHeartBeatTimer);

                // initializing the APDU layer
                objApdu.ApduInit(selectedInterfaceIndex);

                //Console.WriteLine($"Device UpTime: {objApdu.telemetryParametersObj.generalParams.UpTime}, PCB Temperature: {objApdu.telemetryParametersObj.generalParams.PCBTemperature}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Later: {ex.Message}");
            }
        }

        public void test_function()
        {
            Console.WriteLine("This is a modified test message.");
        }
    }// End of the function: DeviceHBTelemetryService
}
