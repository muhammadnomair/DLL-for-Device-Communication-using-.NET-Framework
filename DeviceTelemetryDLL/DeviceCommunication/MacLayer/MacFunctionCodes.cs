namespace DeviceTelemetryDLL.DeviceCommunication.MacLayer
{
    /// <summary>
    /// Creating a class: MacCmdCodes
    /// In this class, we will define different ethernet command Codes to send to MAC layer for sending / receving particualr packet
    /// </summary>
    static class MacCmdCodes
    {
        // For 'HEARTBEAT TELEMETRY' 
        public const UInt32 ETH_REQ_GET_HEARTBEAT = 19;
        public const UInt32 ETH_RES_GET_HEARTBEAT = 15;

        // --- (REQ is not being used) For 'FETCH MAC ADDRESS'
        public const UInt32 ETH_REQ_FETCH_MAC = 270;
        public const UInt32 ETH_RES_FETCH_MAC = 275;

        // For Ethernet No-Command
        public const UInt32 ETH_NO_COMMAND = 0xFFFFFFFF;
    }// End of the function: MacCmdCodes

    // Creating a new class: MacReqCmdCodeIndex
    static class MacReqCmdCodeIndex
    {
        // Index of 'ETH_REQ_GET_HEARTBEAT'
        public const UInt32 ETH_REQ_GET_HEARTBEAT_INDEX = 0;
    }// End of the class: MacReqCmdCodeIndex

    /// <summary>
    /// Creating a new class: MacCmdReqOpcode
    /// This class contains Operation Code (Command Code) value of various Ethernet Requests
    /// </summary>
    static class MacCmdReqOpcode
    {
        public const UInt32 ETH_REQ_GET_HEARTBEAT_OPCODE = MacCmdCodes.ETH_REQ_GET_HEARTBEAT;
    }// End of the class: MacCmdReqOpcode

    /// <summary>
    /// Creating a new class: MacCmdReqRetries
    /// This class contains retries (re-attempts) values for individual Ethernet requests
    /// </summary>
    static class MacCmdReqRetries
    {
        public const byte ETH_REQ_GET_HEARTBEAT_RETRY = 0;
    }// End of the class: MacCmdReqRetries

    /// <summary>
    /// Creating a new class: MacCmdResTimeout
    /// This class contains request time-out value for individual ethernet requests
    /// </summary>
    static class MacCmdResTimeout
    {
        public const UInt16 ETH_REQ_GET_HEARTBEAT_TIMEOUT = 100;
    }// End of the class: MacCmdResTimeout
}
