using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BMS
{
    namespace Core
    {
        /// <summary>
        /// Factory responsible for the creation of console logs
        /// </summary>
        public class BMS_ConsoleLogFactory : BMS_LogFactory
        {
            #region Sub-System/Class ID
            /// <summary>
            /// BMS_ConsoleLogFactory CLass ID
            /// </summary>
            public override byte CLASS_ID
            {
                get { return 0x07; }
            }
            #endregion

            /// <summary>
            /// Creates a new BMS_ConsoleLogger instance.
            /// </summary>
            /// <param name="in_fileName">The name associated with this log</param>
            /// <returns>The newly created console log</returns>
            public override BMS_Logger create(string in_fileName)
            {
                return BMS_ConsoleLogger.get(in_fileName);
            }
        }

        /// <summary>
        /// Console specific implementation of a logger
        /// </summary>
        public class BMS_ConsoleLogger : BMS_Logger
        {
            #region Sub-System/Class ID
            /// <summary>
            /// BMS_ConsolLegger Class ID
            /// </summary>
            public override byte CLASS_ID
            {
                get { return 0x06; }
            }
            #endregion

        #region Console Setup
            /// <summary>
            /// Width of the console output window
            /// </summary>
            private const int m_consoleWidth = 150;

            /// <summary>
            /// Locates a console window given the console window name
            /// </summary>
            /// <param name="lpClassName">Class Name.  Usually null.</param>
            /// <param name="lpWindowName">The window name.</param>
            /// <returns>IntPtr to the window if located.  Null is it was not found.</returns>
            [DllImport("user32.dll")]
            public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

            /// <summary>
            /// Allows for the showing or hiding of a window
            /// </summary>
            /// <param name="hWnd">IntPtr to the console window.</param>
            /// <param name="nCmdShow">0-Hide; 1-Show.</param>
            /// <returns>Boolean success flag</returns>
            [DllImport("user32.dll")]
            static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass"), DllImport("kernel32.dll",
            EntryPoint = "GetStdHandle",
            SetLastError = true,
            CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
            private static extern IntPtr GetStdHandle(int nStdHandle);
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass"), DllImport("kernel32.dll",
                EntryPoint = "AllocConsole",
                SetLastError = true,
                CharSet = CharSet.Auto,
                CallingConvention = CallingConvention.StdCall)]
            private static extern int AllocConsole();
            private const int STD_OUTPUT_HANDLE = -11;
            private const int MY_CODE_PAGE = 437;
        #endregion

            /// <summary>
            /// Singleton instance.
            /// </summary>
            /// <remarks>Windows only allows a single console instance per process, thus the adoption of the singleton patern.</remarks>
            static BMS_ConsoleLogger sm_instance = null;

            /// <summary>
            /// Static singleton access method
            /// </summary>
            /// <returns>The BMS_ConsoleLogger instance</returns>
            public static BMS_ConsoleLogger get(string in_fileName)
            {
                if (sm_instance == null)
                {
                    lock (sync)
                    {
                        if (sm_instance == null)
                        {
                            sm_instance = new BMS_ConsoleLogger(in_fileName);
                        }
                    }
                }
                return sm_instance;
            }

            /// <summary>
            /// Default Constructor.  Initializes log level to default value (TRACE)
            /// </summary>
            private BMS_ConsoleLogger()
            {
                m_curLogLevel = eLogLevel.TRACE;

                AllocConsole();
                IntPtr stdHandle = GetStdHandle(STD_OUTPUT_HANDLE);
                SafeFileHandle safeFileHandle = new SafeFileHandle(stdHandle, true);
                FileStream fileStream = new FileStream(safeFileHandle, FileAccess.Write);
                Encoding encoding = System.Text.Encoding.GetEncoding(MY_CODE_PAGE);
                StreamWriter standardOutput = new StreamWriter(fileStream, encoding);
                standardOutput.AutoFlush = true;
                Console.SetOut(standardOutput);
                Console.WindowWidth = m_consoleWidth;

                m_logName = "console";
                Console.Title = m_logName;
            }

            /// <summary>
            /// Creates the console logger instance with a specified name
            /// </summary>
            /// <param name="in_fileName">the name of this logger.</param>
            private BMS_ConsoleLogger(string in_fileName)
            {
                m_curLogLevel = eLogLevel.TRACE;
                m_logName = in_fileName;

                //IntPtr testHandle = FindWindow(null, m_logName);
                //if (testHandle != IntPtr.Zero)
                //{
                //    ShowWindow(testHandle, 1);
                //}
                //else
                //{
                    AllocConsole();
                    IntPtr stdHandle = GetStdHandle(STD_OUTPUT_HANDLE);
                    SafeFileHandle safeFileHandle = new SafeFileHandle(stdHandle, true);
                    FileStream fileStream = new FileStream(safeFileHandle, FileAccess.Write);
                    Encoding encoding = System.Text.Encoding.GetEncoding(MY_CODE_PAGE);
                    StreamWriter standardOutput = new StreamWriter(fileStream, encoding);
                    standardOutput.AutoFlush = true;
                    Console.SetOut(standardOutput);
                    Console.Title = in_fileName;
                    Console.WindowWidth = m_consoleWidth;
                //}
            }

            /// <summary>
            /// Constructs a BMS_ConsoleLogger.
            /// </summary>
            /// <param name="in_logLvl"></param>
            private BMS_ConsoleLogger(eLogLevel in_logLvl)
            {
                m_curLogLevel = in_logLvl;
                m_logName = "console";

                //IntPtr testHandle = FindWindow(null, m_logName);
                //if (testHandle != IntPtr.Zero)
                //{
                //    ShowWindow(testHandle, 1);
                //}
                //else
                //{
                    AllocConsole();
                    IntPtr stdHandle = GetStdHandle(STD_OUTPUT_HANDLE);
                    SafeFileHandle safeFileHandle = new SafeFileHandle(stdHandle, true);
                    FileStream fileStream = new FileStream(safeFileHandle, FileAccess.Write);
                    Encoding encoding = System.Text.Encoding.GetEncoding(MY_CODE_PAGE);
                    StreamWriter standardOutput = new StreamWriter(fileStream, encoding);
                    standardOutput.AutoFlush = true;
                    Console.SetOut(standardOutput);
                //}
                
            }

            /// <summary>
            /// Logs a message to the console log
            /// </summary>
            /// <param name="in_logLvl">The log level of this message.</param>
            /// <param name="in_message">The message.</param>
            public override void log(eLogLevel in_logLvl, string in_message)
            {
                //  Console logging does not filter messages (used for debugging)
                Console.WriteLine(makeLogString(null, in_logLvl, in_message));
            }

            /// <summary>
            /// Logs a message to the console log using the provided sender for log tagging
            /// </summary>
            /// <param name="in_sender">The BMS_Object sending this message</param>
            /// <param name="in_logLvl">The log level of this message.</param>
            /// <param name="in_message">The message.</param>
            public override void log(BMS_Object in_sender, eLogLevel in_logLvl, string in_message)
            {
                Console.WriteLine(makeLogString(in_sender, in_logLvl, in_message));
            }

            /// <summary>
            /// Logs a broadcast (system) message, ignoring level filtering
            /// </summary>
            /// <param name="in_logLvl">The level of this message.</param>
            /// <param name="in_message">The message to log.</param>
            public override void logBroadcast(eLogLevel in_logLvl, string in_message)
            {
                log(in_logLvl, in_message);
            }

            /// <summary>
            /// Logs a broadcast (system) message, ignoring level filtering
            /// </summary>
            /// <param name="in_sender">The BMS_Object sending this message.</param>
            /// <param name="in_logLvl">The level of this message.</param>
            /// <param name="in_message">The message to log.</param>
            public override void logBroadcast(BMS_Object in_sender, eLogLevel in_logLvl, string in_message)
            {
                log(in_sender, in_logLvl, in_message);
            }

            /// <summary>
            /// Has no Effect for the Console Logger instance
            /// </summary>
            /// <param name="in_logTarget"></param>
            public override void setTarget(string in_logTarget)
            {
                
            }

            /// <summary>
            /// Shuts down the allocated console
            /// </summary>
            public override void shutdown()
            {
                lock (sync)
                {
                    IntPtr windowHandle = FindWindow(null, m_logName);

                    if (windowHandle != IntPtr.Zero)
                    {
                        ShowWindow(windowHandle, 0);
                    }
                }
            }
        }
    }
}
