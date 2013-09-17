using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Diagnostics;
using System.Reflection;

//  Comments section for each module containing configurable elements
//  Log file must be written to and closed after each message
//  Possibley use worker thread for message writting for performance (Test with POC)
//  Client component multi threaded with download and print seperate
//  look into using CSOM or web services to get document so we can use the browser session (no need for extra authentication)
//  true log factory pattern, create classes to create each log type
//  use hash table/dictionary for list of logs in use
//  on filelog creation check for log in dictionary, then check for log on filesystem (create if needed), only then create new)
//  configurable log file roll over
//  configurable log recycle
//  configurable naming convention
//  configurable location
//  configurable log level
//  configuration propegation from server side
//  possible script in client components to update file from master (GPO deployment)
//  configurable server logging (net logger), must use web service and log to [service]application database
//  more granularity on return codes from client side, possible extension of API for multiple method calls

/* Configurable Elements
 *      1.  log location
 *      2.  naming convention
 *      3.  log file roll over interval
 *      4.  log recycle interval
 *      5.  file log level
 *      6.  net log level
 *      7.  console log level
 *      8.  global log level
 *      9.  debug output (console logger)
 *      10. master log location ??
 *      11.  
 * */

//  TODO:   re-write with true factory pattern and associated classes
//  TODO:   Factory classes in sperate file

namespace BMS
{
    namespace Core
    {
        /// <summary>
        /// Log filter level value enum
        /// </summary>
        public enum eLogLevel
        {
            /// <summary>
            /// All messages are written (usually development only setting)
            /// </summary>
            TRACE = 1,

            /// <summary>
            /// Provides Debug level output
            /// </summary>
            DEBUG,

            /// <summary>
            /// Provides informative output
            /// </summary>
            INFO,

            /// <summary>
            /// Provides warning output (unlikely or possibly bad scenarios)
            /// </summary>
            WARN,

            /// <summary>
            /// Error level output, only critical messages
            /// </summary>
            ERROR,

            /// <summary>
            /// System level messages used for broadcasts and product level messages
            /// </summary>
            SYSTEM,

            /// <summary>
            /// No log messages are logged
            /// </summary>
            NONE
        };

        /// <summary>
        /// Abstract base class for all BMS_LogFactory implementations.  Provides interface methods.
        /// </summary>
        public abstract class BMS_LogFactory : BMS_LoggingObject
        {
            #region Sub-System/Class ID
            /// <summary>
            /// BMS_LogFactory Class ID
            /// </summary>
            public override byte CLASS_ID
            {
                get { return 0x03; }
            }
            #endregion

            /// <summary>
            /// Interface method for BMS_Logger object creation
            /// </summary>
            /// <returns>A specific BMS_Logger subtype as a generic BMS_Logger.</returns>
            public abstract BMS_Logger create(string in_fileName);
        }

        /// <summary>
        /// Abstract base class for all BMS_Logger implementations.  Provides interface details and common members as well as current logger containers.
        /// </summary>
        public abstract class BMS_Logger : BMS_LoggingObject
        {
            #region Sub-System/Class ID
            /// <summary>
            /// BMS_Logger class ID
            /// </summary>
            public override byte CLASS_ID
            {
                get { return 0x02; }
            }
            #endregion


            #region Private Members
            /// <summary>
            /// Maintains the collection of created loggers
            /// </summary>
            /// <remarks>Represented as Hashtable to provide ~O(1) access based on logger name (hash key)</remarks>
            private static Hashtable m_loggers = new Hashtable();
            #endregion

            #region Protected Members
            /// <summary>
            /// The current logger's message filter level
            /// </summary>
            protected eLogLevel m_curLogLevel;

            /// <summary>
            /// The current logger's name
            /// </summary>
            protected string m_logName;
            #endregion

            #region Static Methods
            /// <summary>
            /// Returns a properly formatted time stamp for each log message
            /// </summary>
            /// <returns>String representation of the current time stamp.</returns>
            public static string getTimeStamp()
            {
                return DateTime.Now.Date.ToShortDateString() + " " + DateTime.Now.TimeOfDay.Hours + ":" + DateTime.Now.TimeOfDay.Minutes + ":" + DateTime.Now.TimeOfDay.Seconds + ":" + DateTime.Now.TimeOfDay.Milliseconds;
            }

            /// <summary>
            /// COnstructs a formated log string to be written to the log
            /// </summary>
            /// <param name="in_sender">The BMS_Object sending the message.</param>
            /// <param name="in_logLvl">The level of this message.</param>
            /// <param name="in_message">The message.</param>
            /// <returns>String constructed as a log message line</returns>
            /// <remarks>If in_sender is null, the message tagging is omitted.</remarks>
            public static string makeLogString(BMS_Object in_sender, eLogLevel in_logLvl, string in_message)
            {
                string ret = "";

                lock (sync)
                {
                    string messageID = Guid.NewGuid().ToString();

                    if (null == in_sender)
                    {
                        ret = getTimeStamp() + "\t" + messageID + "\tApplication\t0x0000\t" + BMS_Logger.getLevelTag(in_logLvl) + "\t" + in_message;
                    }
                    else
                    {
                        ret = getTimeStamp() + "\t" + messageID + "\t" + BMS_Object.getSubSysName(in_sender.SUBSYS_ID) + "\t0x" + in_sender.SUBSYS_ID.ToString("X2") + in_sender.CLASS_ID.ToString("X2") + "\t" + BMS_Logger.getLevelTag(in_logLvl) + "\t" + in_message;
                    }
                }

                return ret;
            }

            /// <summary>
            /// Provides the string representation of the provided log level
            /// </summary>
            /// <param name="in_logLvl">The log message filter level value to convert.</param>
            /// <returns>String representation of the filter level value</returns>
            public static string getLevelTag(eLogLevel in_logLvl)
            {
                switch (in_logLvl)
                {
                    case eLogLevel.DEBUG:
                        return "DEBUG";
                    case eLogLevel.ERROR:
                        return "ERROR";
                    case eLogLevel.INFO:
                        return "INFO";
                    case eLogLevel.TRACE:
                        return "TRACE";
                    case eLogLevel.WARN:
                        return "WARN";
                    case eLogLevel.NONE:
                        return "NONE";
                    case eLogLevel.SYSTEM:
                        return "SYSTEM";
                    default:
                        return "UNKNOWN";
                }
            }

            /// <summary>
            /// Gets the named file logger or creates it if it does not exist
            /// </summary>
            /// <param name="in_logName">The name of the desired log (access key)</param>
            /// <returns>The requested file logger</returns>
            public static BMS_Logger getLogger(string in_logName)
            {
                BMS_Logger ret = null;

                lock (sync)
                {
                    ret = (BMS_Logger)m_loggers[in_logName];

                    if (ret == null)
                    {
                        ret = new BMS_FileLogFactory().create(in_logName);
                        m_loggers.Add(in_logName, ret);
                    }
                }
                return ret;
            }

            /// <summary>
            /// Gets the named logger if it exists or creates the logger with the provided log factory.
            /// </summary>
            /// <param name="in_logName">The name of the desired log</param>
            /// <param name="in_factory">The instance of the BMS_LogFactory to use for creation if necessary</param>
            /// <returns>The requested Logger.</returns>
            public static BMS_Logger getLogger(string in_logName, BMS_LogFactory in_factory)
            {
                BMS_Logger ret = null;

                lock (sync)
                {
                    ret = (BMS_Logger)m_loggers[in_logName];

                    if (ret == null)
                    {
                        ret = in_factory.create(in_logName);
                        m_loggers.Add(in_logName, ret);
                    }
                }

                return ret;
            }

            /// <summary>
            /// Global log write method.  writes the message to all contained (previously created) logs
            /// </summary>
            /// <param name="in_logLvl">The level of this message.</param>
            /// <param name="in_message">The message to log.</param>
            /// <remarks>Does not filter log messages.  Global log messages are considered system level messages and will always be logged</remarks>
            public static void broadcast(eLogLevel in_logLvl, string in_message)
            {
                lock (sync)
                {
                    foreach (DictionaryEntry it in m_loggers)
                    {
                        ((BMS_Logger)it.Value).logBroadcast(eLogLevel.SYSTEM, in_message);
                    }
                }
            }

            /// <summary>
            /// Global log write method.  writes the message to all contained (previously created) logs
            /// </summary>
            /// <param name="in_sender">The BMS_Object sending this message.</param>
            /// <param name="in_logLvl">The level of this message.</param>
            /// <param name="in_message">The message to log.</param>
            /// <remarks>Does not filter log messages.  Global log messages are considered system level messages and will always be logged</remarks>
            public static void broadcast(BMS_Object in_sender, eLogLevel in_logLvl, string in_message)
            {
                lock (sync)
                {
                    foreach (DictionaryEntry it in m_loggers)
                    {
                        ((BMS_Logger)it.Value).logBroadcast(in_sender, eLogLevel.SYSTEM, in_message);
                    }
                }
            }

            /// <summary>
            /// Logs a message to the windows event logs
            /// </summary>
            /// <param name="in_logLvl">The level of this message.</param>
            /// <param name="in_message">The message to log.</param>
            /// <remarks>This method will not log messages of type TRACE or DEBUG, these types of messages will simply spam the logs.</remarks>
            public static void logToWindows(eLogLevel in_logLvl, string in_message)
            {
                if (!EventLog.SourceExists("BMS.Core"))
                    EventLog.CreateEventSource("BMS.Core", "Application");

                EventLogEntryType messageType = EventLogEntryType.Information;
                switch (in_logLvl)
                {
                    case eLogLevel.ERROR:
                        messageType = EventLogEntryType.Error;
                        break;
                    case eLogLevel.INFO:
                        messageType = EventLogEntryType.Information;
                        break;
                    case eLogLevel.WARN:
                        messageType = EventLogEntryType.Warning;
                        break;
                    case eLogLevel.SYSTEM:
                        messageType = EventLogEntryType.Error;
                        break;
                    default:
                        return;
                }

                EventLog.WriteEntry("BMS.Core", "[BMS_Logger.getLevelTag (in_logLvl)] " + in_message, messageType, 0);
            }

            #endregion

            #region Public Methods
            /// <summary>
            /// Set the current log message filter level for this log
            /// </summary>
            /// <param name="in_logLvl">The new log level.</param>
            public void setLogLevel(eLogLevel in_logLvl)
            {
                lock (sync)
                {
                    m_curLogLevel = in_logLvl;
                }
            }

            /// <summary>
            /// Retrieves the log instance's name
            /// </summary>
            /// <returns>The log name.</returns>
            public string getName()
            {
                return m_logName;
            }

            /// <summary>
            /// Gets the current log message filter level for this logger
            /// </summary>
            /// <returns>The current log filter level.</returns>
            public eLogLevel getLogLevel()
            {
                return m_curLogLevel;
            }
            #endregion

            #region Interface Method Definitions
            /// <summary>
            /// Interface for writting a message to the log instance
            /// </summary>
            /// <param name="in_logLvl">The level of this message.</param>
            /// <param name="in_message">The message to log.</param>
            abstract public void log(eLogLevel in_logLvl, string in_message);

            /// <summary>
            /// Interface for writing a message to the log instance using sender information for log tagging
            /// </summary>
            /// <param name="in_sender">The class object logging the message.</param>
            /// <param name="in_logLvl">THe level of this message.</param>
            /// <param name="in_message">The message to log.</param>
            abstract public void log(BMS_Object in_sender, eLogLevel in_logLvl, string in_message);

            /// <summary>
            /// Interface to set the log instance's target (specific per log type)
            /// </summary>
            /// <param name="in_logTarget">The new log target.</param>
            abstract public void setTarget(string in_logTarget);

            /// <summary>
            /// Interface for writing broadcast (system) messages to the log
            /// </summary>
            /// <param name="in_logLvl">The level of this message.</param>
            /// <param name="in_message">The message to log.</param>
            abstract public void logBroadcast(eLogLevel in_logLvl, string in_message);

            /// <summary>
            /// Interface for writing broadcast (system) messages to the log
            /// </summary>
            /// <param name="in_sender">The BMS_Object sending this message.</param>
            /// <param name="in_logLvl">The level of this message.</param>
            /// <param name="in_message">The message to log.</param>
            abstract public void logBroadcast(BMS_Object in_sender, eLogLevel in_logLvl, string in_message);

            /// <summary>
            /// Interface for ensuring the shutdown of the log instance
            /// </summary>
            abstract public void shutdown();
            #endregion
        }
    }
}
