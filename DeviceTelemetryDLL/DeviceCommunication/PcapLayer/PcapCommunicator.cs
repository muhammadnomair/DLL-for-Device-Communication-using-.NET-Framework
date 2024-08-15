using SharpPcap;

namespace DeviceTelemetryDLL.DeviceCommunication.PcapLayer
{
    public static class PcapCommunicator
    {
        public static ICaptureDevice Communicator = null;

        public const int MaxEthPktsCount = 1000;

        public const int PacketbufferSize = 1500;

        public static byte[,] PacketBuffer = new byte[MaxEthPktsCount, PacketbufferSize];

        public static int BufferCounter = 0;

        public static int ReadbuffCounter = 0;

        public static int ProcessCounter = 0;

        public static int[] PacketLenght = new int[1000];

        public static int PlotBufferCounter = 0;
    }// End of the class: PcapCommunicator
}
