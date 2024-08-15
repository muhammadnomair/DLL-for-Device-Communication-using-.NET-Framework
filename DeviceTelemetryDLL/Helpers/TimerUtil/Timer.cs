using System.Runtime.InteropServices;     // DLL support
using System.Timers;

namespace DeviceTelemetryDLL.Helpers.TimerUtil
{
    // Creating a class: BaseTickTimer
    static class BaseTickTimer
    {
        [DllImport("TestTimer.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 lDllApiTimerInit();

        [DllImport("TestTimer.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 llDllApiTimerReadUsEpoxy();

        [DllImport("TestTimer.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 llDllApiTTmrDiff1UsEpoxy(Int64 llRefTime);

        [DllImport("TestTimer.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 ulDllApiTimerReadUs();

        [DllImport("TestTimer.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 ulDllApiTTmrDiff1Us(UInt32 ulRefTime);

        [DllImport("TestTimer.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 ulDllApiTTmrRead1ms();

        [DllImport("TestTimer.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 ulDllApiTTmrDiff1ms(UInt32 ulRefTime);

        [DllImport("TestTimer.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 ulDllApiTTmrRead1s();

        // TODO:
        //static UInt32 ulMsecTicTime = 0;
        //static Timer objTimer1Ms = new Timer();

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
        //
        // vTimer1MsInit()
        //
        //! This function De-Init the 1 Milli sec Timer.
        //
        //! \param [in] NONE -
        //
        //! \return void -
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
        static public void vTimer1MsInit()
        {
            //             objTimer1Ms.Interval = 2; //>! 1 Ms
            //             objTimer1Ms.Enabled = true;
            //             objTimer1Ms.AutoReset = true;
            //             objTimer1Ms.Elapsed += CbTTmr1MsTick;
            //
            //             objTimer1Ms.Start();

            lDllApiTimerInit();
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
        //
        // vTimer1MsDeInit()
        //
        //! This function De-Init the 1 Milli sec Timer.
        //
        //! \param [in] NONE -
        //
        //! \return void -
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
        static public void vTimer1MsDeInit()
        {
            //             objTimer1Ms.AutoReset = false;
            //             objTimer1Ms.Stop();
            //             objTimer1Ms.Dispose();
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
        //
        // ulTTmrRead1ms()
        //
        //! This function returns the time in Milli-seconds.
        //
        //! \param [in] NONE -
        //
        //! \return UInt32 -
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
        static public UInt32 ulTTmrRead1ms()
        {
            //             //>! Way 1 [Increment 'ulMsecTicTime' via Callback]
            //             return (ulMsecTicTime);

            return (ulDllApiTTmrRead1ms());
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
        //
        // ulTTmrDiff1ms()
        //
        //! This function returns the difference in time w.r.t Reference tick passed as parameter.
        //
        //! \param [in] uint32_t ulRefTime -
        //
        //! \return uint32_t -
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
        static public UInt32 ulTTmrDiff1ms(UInt32 ulRefTime)
        {
            UInt32 ulDiff = 0;

            //             if (ulMsecTicTime >= ulRefTime)
            //             {
            //                 ulDiff = (UInt32)(ulMsecTicTime - ulRefTime);
            //             }
            //             else
            //             {
            //                 ulDiff = (UInt32)((0xFFFFFFFF - ulRefTime) + ulMsecTicTime + (UInt32)1);    // Handle Wrap-around
            //             }

            ulDiff = ulDllApiTTmrDiff1ms(ulRefTime);

            return (ulDiff);
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
        //
        // CbTTmr1MsTick()
        //
        //! This function implements the Callback for 1 Milli-sec timer.
        //
        //! \param [in] NONE -
        //
        //! \return void -
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
        static private void CbTTmr1MsTick(object sender, EventArgs e)
        {
            //             //  Console.WriteLine("DEBUG: 1 m sec Elapsed!\n");
            //
            //             Console.WriteLine("{0}", ulMsecTicTime);
            //             ulMsecTicTime++;
        }

    }// End of the class: BaseTickTimer
}
