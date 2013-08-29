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
            /// <summary>
            /// Creates a new BMS_ConsoleLogger instance.
            /// </summary>
            /// <param name="in_fileName">The name associated with this log</param>
            /// <returns>The newly created console log</returns>
            public override BMS_Logger create(string in_fileName)
            {
                return BMS_ConsoleLogger.get();
            }
        }

        public class BMS_ConsoleLogger : BMS_Logger
        {
        #region Console Setup
            [DllImport("kernel32.dll",
            EntryPoint = "GetStdHandle",
            SetLastError = true,
            CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
            private static extern IntPtr GetStdHandle(int nStdHandle);
            [DllImport("kernel32.dll",
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
            public static BMS_ConsoleLogger get()
            {
                if (sm_instance == null)
                {
                    sm_instance = new BMS_ConsoleLogger();
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
            }

            /// <summary>
            /// Constructs a BMS_ConsoleLogger.
            /// </summary>
            /// <param name="in_logLvl"></param>
            private BMS_ConsoleLogger(eLogLevel in_logLvl)
            {
                m_curLogLevel = in_logLvl;

                AllocConsole();
                IntPtr stdHandle = GetStdHandle(STD_OUTPUT_HANDLE);
                SafeFileHandle safeFileHandle = new SafeFileHandle(stdHandle, true);
                FileStream fileStream = new FileStream(safeFileHandle, FileAccess.Write);
                Encoding encoding = System.Text.Encoding.GetEncoding(MY_CODE_PAGE);
                StreamWriter standardOutput = new StreamWriter(fileStream, encoding);
                standardOutput.AutoFlush = true;
                Console.SetOut(standardOutput);
            }

            /// <summary>
            /// Logs a message to the console log
            /// </summary>
            /// <param name="in_logLvl">The log level of this message.</param>
            /// <param name="in_message">The message.</param>
            public override void log(eLogLevel in_logLvl, string in_message)
            {
                //  return early if not logging message
                //if (in_logLvl < m_curLogLevel)
                //{
                //    return;
                //}

                Console.WriteLine(BMS_Logger.getTimeStamp() + "\t" + BMS_Logger.getLevelTag(in_logLvl) + "\t" + in_message);
            }

            /// <summary>
            /// Has no Effect for the Console Logger instance
            /// </summary>
            /// <param name="in_logTarget"></param>
            public override void setTarget(string in_logTarget)
            {
                
            }
        }
    }
}
