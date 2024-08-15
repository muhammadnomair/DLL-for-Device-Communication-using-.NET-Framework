using DeviceTelemetryDLL.DeviceCommunication.CommunicationConstants;
using DeviceTelemetryDLL.DeviceCommunication.MacLayer;
using DeviceTelemetryDLL.Enums.ApduRequestResponse;
using DeviceTelemetryDLL.Helpers.TimerUtil;

namespace DeviceTelemetryDLL.DeviceCommunication.ApduLayer
{
    // Creating a class: ApduCommunicator
    static class ApduCommunicator
    {
        // Defining fields of Command-based REQUEST packet structure
        public struct APDU_CMD_REQ_STRUCT
        {
            public UInt32 ulApduCmdCode;
            public MAC_PACKET_BUILD stApduParamters;
            public SendRequestStates enSendReqStates;
            public UInt32 ulSendReqWaitRespTick;
            public byte unApduRetriesMade;
            public byte unMaxCmdReqRetry;
            public UInt16 udApduCmdRespTimeout;
            public bool bApduCmdScheduled;
            public bool bApduCmdPayloadPacketBuilt;
            public bool bApduCmdRespReceived;
            public Int32 lApduCmdStatus;
        }

        // Creating a list of objects of structure: APDU_CMD_REQ_STRUCT
        static public APDU_CMD_REQ_STRUCT[] stApduCmdReqPkt = new APDU_CMD_REQ_STRUCT[FDRConstants.TotalApduCommands];

        static public void ApduInitCommandStructure()
        {
            UInt32[] aulApduOpcode = { 
                                        MacCmdReqOpcode.ETH_REQ_GET_HEARTBEAT_OPCODE,
                                     };

            // List of APDUC Cmmand Code indexes
            UInt32[] aulApduOpcodeIndex = {
                                            MacReqCmdCodeIndex.ETH_REQ_GET_HEARTBEAT_INDEX,
                                          };

            byte[] aulApduCmdRetries = {
                                        MacCmdReqRetries.ETH_REQ_GET_HEARTBEAT_RETRY
                                        };

            UInt16[] audApduCmdRespTimeout = {
                                               MacCmdResTimeout.ETH_REQ_GET_HEARTBEAT_TIMEOUT,
                                             };

            // designing initial version of APDU request packet (structure) for each APDU command
            for (byte unIter = 0; unIter < FDRConstants.TotalApduCommands; unIter++)
            {
                stApduCmdReqPkt[aulApduOpcodeIndex[unIter]].ulApduCmdCode = aulApduOpcode[unIter]; // Coammand Code value
                stApduCmdReqPkt[aulApduOpcodeIndex[unIter]].stApduParamters.aunMacPopulateBuff = new byte[FDRConstants.ApduMaxEthPktSize]; // Maximum Ethernet Packet size
                stApduCmdReqPkt[aulApduOpcodeIndex[unIter]].enSendReqStates = SendRequestStates.SendReqIdle; // Send Request state => Idle
                
                //stApduCmdReqPkt[aulApduOpcodeIndex[unIter]].ulSendReqWaitRespTick = BaseTickTimer.ulDllApiTTmrRead1ms();
                stApduCmdReqPkt[aulApduOpcodeIndex[unIter]].ulSendReqWaitRespTick = 1000; // hard-coded
                
                stApduCmdReqPkt[aulApduOpcodeIndex[unIter]].unApduRetriesMade = 0;
                stApduCmdReqPkt[aulApduOpcodeIndex[unIter]].unMaxCmdReqRetry = aulApduCmdRetries[unIter];
                stApduCmdReqPkt[aulApduOpcodeIndex[unIter]].udApduCmdRespTimeout = audApduCmdRespTimeout[unIter];
                stApduCmdReqPkt[aulApduOpcodeIndex[unIter]].bApduCmdScheduled = false; // Command schedule FALASE (by-default)
                stApduCmdReqPkt[aulApduOpcodeIndex[unIter]].bApduCmdPayloadPacketBuilt = false;
                stApduCmdReqPkt[aulApduOpcodeIndex[unIter]].bApduCmdRespReceived = false; // Response received FALSE (by-default)
                stApduCmdReqPkt[aulApduOpcodeIndex[unIter]].lApduCmdStatus = 0;
            }
        }// End of the function: ApduInitCommandStructure
    }// End of the class: ApduCommunicator
}
