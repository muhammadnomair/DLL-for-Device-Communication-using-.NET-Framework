using System;
using DeviceTelemetryDLL;

class TestDLL
{
    public static void Main(string[] args)
    {
        DeviceHBTelemetryService testObj = new DeviceHBTelemetryService();
        testObj.GetNetworkDeviceList();

        //Console.Write("Enter network interface index: ");
        //int interfaceIndex = Convert.ToInt32( Console.ReadLine() );

        //Console.Write("Enter HeartBeat Telemetry Time-interval: ");
        //int telemetryTimeInterval = Convert.ToInt32(Console.ReadLine());

        testObj.GetTelemetryResposneData(3, 100);

    }
}// End of the class: TestDLL



