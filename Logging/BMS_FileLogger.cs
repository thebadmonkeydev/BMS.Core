using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace BMS
{
    namespace Core
    {
        /// <summary>
        /// Factory to create file loggers
        /// </summary>
        public class BMS_FileLogFactory : BMS_LogFactory 
        {
            #region Sub-System/Class ID
            /// <summary>
            /// BMS_FileLogFactory Class ID
            /// </summary>
            public override byte CLASS_ID
            {
                get { return 0x05; }
            }
            #endregion

            /// <summary>
            /// Creates a new BMS_ConsoleLogger instance.
            /// </summary>
            /// <param name="in_fileName">The name associated with this log</param>
            /// <returns>The newly created console log</returns>
            public override BMS_Logger create(string in_fileName)
            {
                BMS_FileLogger ret = new BMS_FileLogger(in_fileName);
                ret.setTarget(in_fileName);

                return ret;
            }
        }

        /// <summary>
        /// File based implementation of the logger
        /// </summary>
        public class BMS_FileLogger : BMS_Logger
        {
            #region Sub-System/Class ID
            /// <summary>
            /// BMS_FileLogger Class ID
            /// </summary>
            public override byte CLASS_ID
            {
                get { return 0x04; }
            }
            #endregion
            //readonly private static string m_moduleId = System.Reflection.Assembly.GetExecutingAssembly().GetType("BMS_FileLogger").Namespace;
            
                //GetExecutingAssembly().GetName().Name;

            /// <summary>
            /// File URI for the log file
            /// </summary>
            protected string m_fileURI;

            /// <summary>
            /// Default constructor.  Initializes log level and file location to default values (INFO and ./log.log)
            /// </summary>
            public BMS_FileLogger()
            {   //  Set defaults for a file logger
                m_curLogLevel = eLogLevel.INFO;
                m_fileURI = "./log";
                m_logName = "LogFile";
            }

            /// <summary>
            /// Constructs a BMS_FileLogger using the provided log file name and sets the default log level to INFO
            /// </summary>
            /// <param name="in_logFileName">The path to the intended log output file.</param>
            public BMS_FileLogger(string in_logFileName)
            {
                m_curLogLevel = eLogLevel.INFO;
                m_fileURI = in_logFileName;
                m_logName = in_logFileName;
            }

            /// <summary>
            /// Constructs a BMS_FileLogger using the provided log level and log file name
            /// </summary>
            /// <param name="in_logLvl">The desired log level</param>
            /// <param name="in_logFileName">The path to the intended log output file.</param>
            public BMS_FileLogger(eLogLevel in_logLvl, string in_logFileName)
            {
                m_curLogLevel = in_logLvl;
                m_fileURI = in_logFileName;
                m_logName = in_logFileName;
            }

            /// <summary>
            /// Logs a message to the file log
            /// </summary>
            /// <param name="in_logLvl">The log level of this message.</param>
            /// <param name="in_message">The message.</param>
            public override void log(eLogLevel in_logLvl, string in_message)
            {
                lock (sync)
                {
                    //  Return early if not logging message
                    if (in_logLvl < m_curLogLevel)
                    {
                        return;
                    }

                    StreamWriter logWriter = null;
                    string timeStamp = BMS_Logger.getTimeStamp();
                    try
                    {
                        logWriter = new StreamWriter(m_fileURI, true);
                        logWriter.WriteLine(makeLogString(null, in_logLvl, in_message));
                        logWriter.Flush();
                    }
                    catch (Exception ex)
                    {
                        BMS_Logger.broadcast(eLogLevel.ERROR, "Could not write to log (" + m_logName + "):\t" + ex.Message + "\n" + ex.StackTrace);
                    }
                    finally
                    {
                        if (logWriter != null)
                        {
                            logWriter.Dispose();
                        }
                    }
                }
            }

            /// <summary>
            /// Logs a message to the file log using the provided sender to tag the log message
            /// </summary>
            /// <param name="in_sender">The BMS_Object sending this message.</param>
            /// <param name="in_logLvl">THe log level of this message.</param>
            /// <param name="in_message">The message.</param>
            public override void log(BMS_Object in_sender, eLogLevel in_logLvl, string in_message)
            {
                lock (sync)
                {
                    //  Return early if not logging message
                    if (in_logLvl < m_curLogLevel)
                    {
                        return;
                    }

                    StreamWriter logWriter = null;
                    string timeStamp = BMS_Logger.getTimeStamp();
                    try
                    {
                        logWriter = new StreamWriter(m_fileURI, true);
                        logWriter.WriteLine(makeLogString(in_sender, in_logLvl, in_message));
                        logWriter.Flush();
                    }
                    catch (Exception ex)
                    {
                        BMS_Logger.broadcast(this, eLogLevel.ERROR, "Could not write to log (" + m_logName + "):\t" + ex.Message + "\n" + ex.StackTrace);
                    }
                    finally
                    {
                        if (logWriter != null)
                        {
                            logWriter.Dispose();
                        }
                    }
                }
            }

            /// <summary>
            /// Logs a broadcast (system) message, ignoring level filtering
            /// </summary>
            /// <param name="in_logLvl">The level of this message.</param>
            /// <param name="in_message">The message to log.</param>
            public override void logBroadcast(eLogLevel in_logLvl, string in_message)
            {
                lock (sync)
                {
                    StreamWriter logWriter = null;
                    string timeStamp = BMS_Logger.getTimeStamp();
                    try
                    {
                        logWriter = new StreamWriter(m_fileURI, true);
                        logWriter.WriteLine(makeLogString(null, in_logLvl, in_message));
                        logWriter.Flush();
                    }
                    finally
                    {
                        if (logWriter != null)
                        {
                            logWriter.Dispose();
                        }
                    }
                }
            }

            /// <summary>
            /// Logs a broadcast (system) message, ignoring level filtering
            /// </summary>
            /// <param name="in_sender">The BMS_Object sending this message.</param>
            /// <param name="in_logLvl">The level of this message.</param>
            /// <param name="in_message">The message to log.</param>
            public override void logBroadcast(BMS_Object in_sender, eLogLevel in_logLvl, string in_message)
            {
                lock (sync)
                {
                    StreamWriter logWriter = null;
                    string timeStamp = BMS_Logger.getTimeStamp();
                    try
                    {
                        logWriter = new StreamWriter(m_fileURI, true);
                        logWriter.WriteLine(makeLogString(in_sender, in_logLvl, in_message));
                        logWriter.Flush();
                    }
                    finally
                    {
                        if (logWriter != null)
                        {
                            logWriter.Dispose();
                        }
                    }
                }
            }

            /// <summary>
            /// Sets the file location for this logger.
            /// </summary>
            /// <param name="in_logTarget">The new log target as a file path.</param>
            public override void setTarget(string in_logTarget)
            {
                lock (sync)
                {
                    m_fileURI = in_logTarget;
                }
            }

            /// <summary>
            /// Shuts down the file logger
            /// </summary>
            /// <remarks>Nothing is done here because file handles are created and destroyed for each log message</remarks>
            public override void shutdown()
            {
                //  TODO:   Find more efficient way to handle file handles and ensuring a crash leaves all messages.
            }
        }
    }
}
