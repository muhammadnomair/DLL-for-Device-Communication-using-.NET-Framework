using System;
using System.Runtime.CompilerServices;

namespace DeviceTelemetryDLL.Helpers
{
    // [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public unsafe struct ST_TEST
    {
        public UInt32 Data1;
        public UInt32 Data2;
        public byte Data3;
    }

    // Creating a class: PureUnsafeMethods
    static unsafe class PureUnsafeMethods
    {
        public static void Copy(void* source, void* destination, int lengthSrc, int lengthDest)
        {
            Buffer.MemoryCopy(source, destination, lengthDest, lengthSrc);
        }

        public static void vmemcpy(void* source, void* destination, int length)
        {
            Unsafe.CopyBlock(destination, source, (uint)length);
        }

        public static unsafe Int32 SizeOf<T>() where T : unmanaged
        {
            return sizeof(T);
        }

        /*
        public static unsafe Int32 SizeOf<T>() where T : struct
        {
            return Marshal.SizeOf(default(T));
        }
        */

        public static bool Equals(byte* source1, byte* source2, int length)
        {
            byte* t_x = source1, t_y = source2;

            for (int i = (length / 8) - 1; i >= 0; i--, t_x += 8, t_y += 8)
            {
                if (*((long*)t_x) != *((long*)t_y))
                    return false;
            }

            if ((length & 4) != 0)
            {
                if (*((int*)t_x) != *((int*)t_y))
                    return false;
                t_x += 4;
                t_y += 4;
            }

            if ((length & 2) != 0)
            {
                if (*((short*)t_x) != *((short*)t_y))
                    return false;
                t_x += 2;
                t_y += 2;
            }

            if ((length & 1) != 0)
            {
                if (*((byte*)t_x) != *((byte*)t_y))
                    return false;
            }

            return true;
        }

        public static int Compare(byte* source1, byte* source2, int length)
        {
            byte* t_x = source1, t_y = source2;

            int len = length;
            int c = 0;

            for (; len > 0; len--)
            {
                c = (int)*t_x++ - (int)*t_y++;
                if (c != 0)
                    return c;
            }

            return 0;
        }

        public static void And(byte* source1, byte* source2, byte* result, int length)
        {
            byte* t_x = source1, t_y = source2;
            var t_buffer = result;

            for (int i = (length / 8) - 1; i >= 0; i--, t_x += 8, t_y += 8, t_buffer += 8)
            {
                *((long*)t_buffer) = *((long*)t_x) & *((long*)t_y);
            }

            if ((length & 4) != 0)
            {
                *((int*)t_buffer) = *((int*)t_x) & *((int*)t_y);
                t_x += 4;
                t_y += 4;
                t_buffer += 4;
            }

            if ((length & 2) != 0)
            {
                *((short*)t_buffer) = (short)(*((short*)t_x) & *((short*)t_y));
                t_x += 2;
                t_y += 2;
                t_buffer += 2;
            }

            if ((length & 1) != 0)
            {
                *((byte*)t_buffer) = (byte)(*((byte*)t_x) & *((byte*)t_y));
            }
        }

        public static void Or(byte* source1, byte* source2, byte* result, int length)
        {
            byte* t_x = source1, t_y = source2;
            var t_buffer = result;

            for (int i = (length / 8) - 1; i >= 0; i--, t_x += 8, t_y += 8, t_buffer += 8)
            {
                *((long*)t_buffer) = *((long*)t_x) | *((long*)t_y);
            }

            if ((length & 4) != 0)
            {
                *((int*)t_buffer) = *((int*)t_x) | *((int*)t_y);
                t_x += 4;
                t_y += 4;
                t_buffer += 4;
            }

            if ((length & 2) != 0)
            {
                *((short*)t_buffer) = (short)(*((short*)t_x) | *((short*)t_y));
                t_x += 2;
                t_y += 2;
                t_buffer += 2;
            }

            if ((length & 1) != 0)
            {
                *((byte*)t_buffer) = (byte)(*((byte*)t_x) | *((byte*)t_y));
            }
        }

        public static void Xor(byte* source1, byte* source2, byte* result, int length)
        {
            byte* t_x = source1, t_y = source2;
            var t_buffer = result;

            for (int i = (length / 8) - 1; i >= 0; i--, t_x += 8, t_y += 8, t_buffer += 8)
            {
                *((long*)t_buffer) = *((long*)t_x) ^ *((long*)t_y);
            }

            if ((length & 4) != 0)
            {
                *((int*)t_buffer) = *((int*)t_x) ^ *((int*)t_y);
                t_x += 4;
                t_y += 4;
                t_buffer += 4;
            }

            if ((length & 2) != 0)
            {
                *((short*)t_buffer) = (short)(*((short*)t_x) ^ *((short*)t_y));
                t_x += 2;
                t_y += 2;
                t_buffer += 2;
            }

            if ((length & 1) != 0)
            {
                *((byte*)t_buffer) = (byte)(*((byte*)t_x) ^ *((byte*)t_y));
            }
        }

        public static void UnSafeCopyTestRoutine()
        {
            byte x = 10;
            byte y;

            byte[] aunArrayBuffSrc = new byte[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            byte[] aunArrayBuffDest = new byte[10];

            unsafe // Variable Test
            {
                PureUnsafeMethods.vmemcpy((byte*)&x, (byte*)&y, 1);
            }

            unsafe // Array Test
            {
                fixed (byte* pinnedSource = aunArrayBuffSrc)
                fixed (byte* pinnedDestination = aunArrayBuffDest)
                {
                    PureUnsafeMethods.vmemcpy((byte*)pinnedSource, (byte*)pinnedDestination, 10);
                }

                for (int lIter = 0; lIter < 10; lIter++)
                {
                    Console.WriteLine("{0}", aunArrayBuffDest[lIter]);
                }
            }

            ST_TEST stTest;
            byte[] aunPayloadBuff = new byte[100];

            unsafe
            {
                //  fixed (void* pinnedSource = &stTest) //>! Use this if any error is thrown by compiler
                fixed (byte* pinnedDestination = aunPayloadBuff)
                {
                    PureUnsafeMethods.vmemcpy((byte*)&stTest, (byte*)pinnedDestination, PureUnsafeMethods.SizeOf<ST_TEST>());
                }

                Console.WriteLine("Sizeof Structure: {0}", PureUnsafeMethods.SizeOf<ST_TEST>());
            }
        }
    }// End of the function: PureUnsafeMethods
}