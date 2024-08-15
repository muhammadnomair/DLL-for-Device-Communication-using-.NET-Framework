using System;
using System.Collections;

namespace DeviceTelemetryDLL.Helpers
{
    // Creating a class: Utility
    class CharArrayUtility
    {
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
        //
        // vMemSet()
        //
        //! This function is clears the buffer passed as argument.
        //
        //! \param [in] byte[] aunArray -
        //! \param [in] byte unSetValue -
        //
        //! \return void -
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
        public static void vMemSet(byte[] aunArray, byte unSetValue)
        {
            if (aunArray == null)
            {
                throw new ArgumentNullException("array");
            }

            int block = 32, index = 0;
            int length = Math.Min(block, aunArray.Length);

            //Fill the initial array
            while (index < length)
            {
                aunArray[index++] = unSetValue;
            }

            length = aunArray.Length;
            while (index < length)
            {
                Buffer.BlockCopy(aunArray, 0, aunArray, index, Math.Min(block, length - index));
                index += block;
                block *= 2;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
        //
        // vMemSetCharArray()
        //
        //! This function is clears the buffer passed as argument.
        //
        //! \param [in] byte[] aunArray -
        //! \param [in] byte unSetValue -
        //
        //! \return void -
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
        public static void vMemSetCharArray(char[] aunArray, byte unSetValue)
        {
            if (aunArray == null)
            {
                throw new ArgumentNullException("array");
            }

            int block = 32, index = 0;
            int length = Math.Min(block, aunArray.Length);

            //Fill the initial array
            while (index < length)
            {
                aunArray[index++] = (char)unSetValue;
            }

            length = aunArray.Length;
            while (index < length)
            {
                Buffer.BlockCopy(aunArray, 0, aunArray, index, Math.Min(block, length - index));
                index += block;
                block *= 2;
            }
        }

        public static char[] achSkipWhiteSpace(Char[] srcString, UInt16 udSrcOffset)
        {
            char[] aunTempArray = new char[srcString.Length];
            UInt16 udIndex = 0;

            for (int i = 0; i < (srcString.Length - udSrcOffset); i++)
            {
                if (!Char.IsWhiteSpace(srcString[i + udSrcOffset]))
                {
                    aunTempArray[udIndex] = srcString[udSrcOffset + i];
                    udIndex++;
                }
            }

            return (aunTempArray);
        }

        public static char[] achSkipNonWhiteSpace(Char[] srcString, UInt16 udSrcOffset)
        {
            char[] aunTempArray = new char[srcString.Length];
            UInt16 udIndex = 0;

            for (int i = 0; i < (srcString.Length - udSrcOffset); i++)
            {
                if (Char.IsWhiteSpace(srcString[i + udSrcOffset]))
                {
                    aunTempArray[udIndex] = srcString[udSrcOffset + i];
                    udIndex++;
                }
            }

            return (aunTempArray);
        }

        public static char[] achSkipNull(Char[] srcString, UInt16 udSrcOffset)
        {
            char[] aunTempArray = new char[srcString.Length];

            for (int i = 0; i < (srcString.Length - udSrcOffset); i++)
            {
                if (srcString[i + udSrcOffset] != '\0')
                {
                    aunTempArray[i - 1] = srcString[udSrcOffset + i];
                }
            }

            return (aunTempArray);
        }

        public static string strCharArrayToString(char[] srcCharArray)
        {
            string strConverted = new string(srcCharArray);
            return (strConverted);
        }

        static bool ByteArrayCompare(byte[] a1, byte[] a2)
        {
            return StructuralComparisons.StructuralEqualityComparer.Equals(a1, a2);
        }
    }// End of the class: Utility
}
