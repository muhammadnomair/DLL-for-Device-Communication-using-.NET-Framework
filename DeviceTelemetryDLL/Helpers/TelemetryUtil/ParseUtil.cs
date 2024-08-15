
namespace DeviceTelemetryDLL.Helpers.TelemetryUtil
{
    /// <summary>
    /// Creating a class: Util, which contains all functions (methods) which are used frequetly in this project
    /// </summary>
    public unsafe class ParseUtil
    {
        // Get oly one Byte from the byte-list
        public static byte GetOneByteValue(byte[] DataFile, ref uint ulIndex)
        {
            byte x = DataFile[ulIndex];
            ulIndex += sizeof(byte); // Jump to 1 byte
            return x;
        }// End of the function: GetOneByteValue

        // based on little endian (LE) mechanism, parsing the consecutive 4-bytes of data from byte-array and returning it into Uinit32
        // (In little endian machines, last byte of binary representation of the multibyte data-type is stored first.)
        public static UInt32 Parseuint32LittleEndian(byte[] DataFile, ref uint ulIndex)
        {
            UInt32 x = (UInt32)((DataFile[ulIndex + 3] << 24) | (DataFile[ulIndex + 2] << 16) | (DataFile[ulIndex + 1] << 8) | DataFile[ulIndex]); // little endian format
            ulIndex += sizeof(UInt32); // Jump to 4- Bytes

            return x;
        }// End of function: Parseuint32LittleEndian

        // based on little endian (LE) mechanism, parsing the consecutive 2-bytes of data from byte-array and returning it into Uinit16
        // (In little endian machines, last byte of binary representation of the multibyte data-type is stored first.)
        public static UInt16 Parseuint16LittleEndian(byte[] DataFile, ref uint ulIndex)
        {
            UInt16 x = (UInt16)((DataFile[ulIndex + 1] << 8) | DataFile[ulIndex]); // little endian format
            ulIndex += sizeof(UInt16); // Jump to 2- Bytes

            return x;
        }// End of function: Parseuint32LittleEndian

        // Parse and return the consecutive 4-bytes of data from byte-array
        public static UInt32 Parseuint32(byte[] DataFile, ref uint ulIndex)
        {
            UInt32 x = (UInt32)((DataFile[ulIndex + 0] << 24) | (DataFile[ulIndex + 1] << 16) | (DataFile[ulIndex + 2] << 8) | DataFile[ulIndex + 3]);
            // UInt32 x = (UInt32)( (DataFile[ulIndex + 3] << 24) | (DataFile[ulIndex + 2] << 16) | (DataFile[ulIndex + 1] << 8) | DataFile[ulIndex] ); // little endian format
            ulIndex += sizeof(UInt32); // Jump to 4- Bytes

            return x;
        }// End of function: Parseuint32

        // Parse and return the consecutive 4-bytes of data from byte-array and return it into float data-type.
        public static float Parsefloat32(byte[] DataFile, ref uint ulIndex)
        {
            UInt32 x = (UInt32)((DataFile[ulIndex] << 24) | (DataFile[ulIndex + 1] << 16) | (DataFile[ulIndex + 2] << 8) | DataFile[ulIndex + 3]);
            //UInt32 x = (UInt32)((DataFile[ulIndex + 3] << 24) | (DataFile[ulIndex + 2] << 16) | (DataFile[ulIndex + 1] << 8) | DataFile[ulIndex]);

            float x_float = *(float*)(&x); // assigning the float value
            ulIndex += sizeof(UInt32); // Jump to 4-Bytes

            return x_float;
        }// End of function: Parsefloat32

        // Parse and return the consecutive 2-bytes of data from byte-array
        public static UInt16 Parseuint16(byte[] DataFile, ref uint ulIndex)
        {
            UInt16 x = (UInt16)((DataFile[ulIndex + 0] << 8) | (DataFile[ulIndex + 1]));
            ulIndex += sizeof(UInt16); // Jump to 2-Bytes

            return x;
        }// End of function: Parseuint16

        // This function will properly formate serial number from the given bytes list
        public static string formateDeviceSerailNumber(byte[] snList)
        {
            string formatedSN = "";
            try
            {
                for (int index = 0; index < snList.Length; index++)
                {
                    // treating the last byte differently
                    if (index != snList.Length - 1)
                        formatedSN += $"{snList[index]}-";
                    else
                        formatedSN += $"{snList[index]}";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An Error occured while formatting device serial numebr in DeviceSerailNumber method:\n{ex.Message}");
            }
            return formatedSN;
        }// End of the function: formateDeviceSerailNumber

        // Getting each byte from LCS status list for LCS Status
        public static string formateLCSStatus(byte[] lcsStatusList)
        {
            string formatedLCSStatus = "";
            try
            {
                // Concatenating all status values in single line (row)
                for (int byteIndex = 0; byteIndex < lcsStatusList.Length; byteIndex++)
                {
                    // treating the last byte differently
                    if (byteIndex != lcsStatusList.Length - 1)
                        formatedLCSStatus += $"{byteIndex + 1}: {lcsStatusList[byteIndex]}, "; // comma (,) seperated values
                    else
                        formatedLCSStatus += $"{byteIndex + 1}: {lcsStatusList[byteIndex]}";
                }

                /*
                int byteIndex = 0;
                for (int status_row = 0; status_row < (lcsStatusList.Length / 2); status_row++)
                {
                    if (status_row != (lcsStatusList.Length / 2) - 1)
                    {
                        // one row has two status values
                        formatedLCSStatus += $"{byteIndex + 1}: {lcsStatusList[byteIndex]}, {byteIndex + 2}: {lcsStatusList[byteIndex + 1]}\n"; 
                        byteIndex += 2;
                    }
                    else
                    {
                        // not using /n (new line) at the last status row
                        formatedLCSStatus += $"{byteIndex + 1}: {lcsStatusList[byteIndex]}, {byteIndex + 2}: {lcsStatusList[byteIndex + 1]}"; 
                    }
                }
                */
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while formatting LCSStatus in formateLCSStatus method:\n{ex.Message}");
            }
            return formatedLCSStatus;
        }// End of the function: formateLCSStatus

        // Formatting device firmware version in string-format properly
        public static string formateFirmwareVersion(byte[] firmwareList)
        {
            string formattedFirmwareVer = "";
            try
            {
                for (int index = 0; index < firmwareList.Length; index++)
                {
                    // treating the last byte differently
                    if (index != firmwareList.Length - 1)
                        formattedFirmwareVer += $"{firmwareList[index]}."; // dot (period) seperated values
                    else
                        formattedFirmwareVer += $"{firmwareList[index]}";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while formatting device-firmware version in formateFirmwareVersion method:\n{ex.Message}");
            }
            return formattedFirmwareVer;
        }// End of the function: formateFirmwareVersion

        // Getting each byte from communication link status list for CommunicationLinkStatus
        public static string formateCommunicationLinkStatus(byte[] commLinkStatusList)
        {
            string formattedCommLinkStatus = "";
            try
            {
                // Concatenating all status values in single line (row)
                for (int byteIndex = 0; byteIndex < commLinkStatusList.Length; byteIndex++)
                {
                    // treating the last byte differently
                    if (byteIndex != commLinkStatusList.Length - 1)
                        formattedCommLinkStatus += $"{byteIndex + 1}: {commLinkStatusList[byteIndex]}, "; // comma (,) seperated values
                    else
                        formattedCommLinkStatus += $"{byteIndex + 1}: {commLinkStatusList[byteIndex]}";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while formatting communication-link status in formateCommunicationLinkStatus method:\n{ex.Message}");
            }
            return formattedCommLinkStatus;
        }// End of the funcion: formateCommunicationLinkStatus

        // Getting each byte of Error Codes from Errorcodes List and format them in string representation
        public static string formateErrorCodesList(byte[] errorCodesList)
        {
            string formattedErrorCodes = "";
            try
            {
                // Concatenating all status values in single line (row)
                for (int byteIndex = 0; byteIndex < errorCodesList.Length; byteIndex++)
                {
                    // treating the last byte differently
                    if (byteIndex != errorCodesList.Length - 1)
                        formattedErrorCodes += $"{byteIndex + 1}: {errorCodesList[byteIndex]}, "; // comma (,) seperated values
                    else
                        formattedErrorCodes += $"{byteIndex + 1}: {errorCodesList[byteIndex]}";
                }

                /*
                int byteIndex = 0;

                // one row contains two Error-Code values.
                for (int status_row = 0; status_row < (errorCodesList.Length / 2); status_row++)
                {
                    // treating the last string line (status row) differently
                    if (status_row != (errorCodesList.Length / 2) - 1)
                    {
                        formattedErrorCodes += $"{byteIndex + 1}: {errorCodesList[byteIndex]}, {byteIndex + 2}: {errorCodesList[byteIndex + 1]}\n"; // one row has two status values
                        byteIndex += 2;
                    }
                    else
                    {
                        formattedErrorCodes += $"{byteIndex + 1}: {errorCodesList[byteIndex]}, {byteIndex + 2}: {errorCodesList[byteIndex + 1]}"; // not using /n (new line) at the last status row
                    }
                }
                */
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while formatting ErrorCodes values in formateErrorCodesList method:\n{ex.Message}");
            }
            return formattedErrorCodes;
        }// End of the function: formateErrorCodesList

        // Getting n number of power-module temperatures
        public static string formateAndGetPowerModuleTemperature(float[] powerModuleTempList, int temperatureIndex)
        {
            string formatedPowerModuleTemp = "";
            try
            {
                formatedPowerModuleTemp += $"1: {powerModuleTempList[temperatureIndex]} °C, 2: {powerModuleTempList[temperatureIndex + 1]} °C"; // one row has two status values
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while formatting and getting two power-module temperatures values in formateAndGetPowerModuleTemperature method:\n{ex.Message}");
            }
            return formatedPowerModuleTemp;
        }// End of the function: formateAndGetPowerModuleTemperature

        // Getting each byte of Rail-Voltages from railVoltages List and format them in string representation
        public static string formateRailVoltagesList(byte[] railVoltageslist)
        {
            string formattedRailVoltages = "";
            try
            {
                // Concatenating all status values in single line (row)
                for (int index = 0; index < railVoltageslist.Length; index++)
                {
                    // treating the last byte differently
                    if (index != railVoltageslist.Length - 1)
                        formattedRailVoltages += $"{index + 1}: {railVoltageslist[index]} V, "; // commaa (,) seperated values
                    else
                        formattedRailVoltages += $"{index + 1}: {railVoltageslist[index]} V";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while formatting Rail-Voltages values in formateRailVoltagesList method:\n{ex.Message}");
            }
            return formattedRailVoltages;
        }// End of the function: formateRailVoltagesList

        // Getting each byte of Fan RPM from fanRPM List and format them in string representation
        public static string formateFanRPMList(UInt16[] fanRPMList)
        {
            string formattedFanRPM = "";
            try
            {
                // Concatenating all status values in single line (row)
                for (int index = 0; index < fanRPMList.Length; index++)
                {
                    // treating the last byte differently
                    if (index != fanRPMList.Length - 1)
                        formattedFanRPM += $"{index + 1}: {fanRPMList[index]}, "; // comma (,) seperated values
                    else
                        formattedFanRPM += $"{index + 1}: {fanRPMList[index]}";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while formatting Fan RPM status values in formateFanRPMList method:\n{ex.Message}");
            }
            return formattedFanRPM;
        }// End of the function: formateFanRPMList

        // Getting each byte of Pump-State from pumpstate List and format them in string representation
        public static string formateEachPumpActualStateValue(byte[] pumpStatelist)
        {
            string formattedPumpStateActualValues = "";
            try
            {
                // Concatenating all status values in single line (row)
                for (int index = 0; index < pumpStatelist.Length; index++)
                {
                    string pumpState = "";

                    // pump state value at current byte
                    byte sumpStateValue = pumpStatelist[index];

                    // getting actual state value
                    switch (sumpStateValue)
                    {
                        case 0:
                            pumpState = "Normal Working";
                            break;

                        case 1:
                            pumpState = "Dry Running";
                            break;

                        case 2:
                            pumpState = "No Speed Control";
                            break;

                        case 3:
                            pumpState = "Rotor-Locked/Current-Overload";
                            break;

                        default:
                            pumpState = "State Invalid";
                            break;
                    }

                    // formatting pump status value in string (treating the last byte differently)
                    if (index != pumpStatelist.Length - 1)
                        formattedPumpStateActualValues += $"{index + 1}: {pumpState}, "; // comma (,) seperated values
                    else
                        formattedPumpStateActualValues += $"{index + 1}: {pumpState}";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while formatting Pump state actual values in formateEachPumpActualStateValue method:\n{ex.Message}");
            }
            return formattedPumpStateActualValues;
        }// End of the function: formatePumpStateList
    }// End of the class: ParseUtil
}
