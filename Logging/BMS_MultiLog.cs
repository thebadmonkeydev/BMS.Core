using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS
{
    namespace Core
    {
        /// <summary>
        /// Factory to create multi log loggers
        /// </summary>
        public class BMS_MultiLogFactory : BMS_LogFactory
        {
            /// <summary>
            /// Creates a new BMS_MultiLog instance
            /// </summary>
            /// <param name="in_fileName">THe name associated with this log.</param>
            /// <returns>The newly created multi log</returns>
            public override BMS_Logger create(string in_fileName)
            {
                BMS_MultiLog ret = new BMS_MultiLog(in_fileName);
                return ret;
            }
        }


        /// <summary>
        /// Provides the ability to group logs within a single log instance
        /// </summary>
        public class BMS_MultiLog : BMS_Logger
        {
            /// <summary>
            /// Collection of loggers using hash mapping for O(1) access by name key
            /// </summary>
            protected Hashtable m_loggers;

            /// <summary>
            /// Default constructor for multilog objects
            /// </summary>
            public BMS_MultiLog(string in_logName)
            {
                m_logName = in_logName;
                m_loggers = new Hashtable();
            }

            /// <summary>
            /// Adds a logger to this multi-logger, creating a deafult file logger if necessary
            /// </summary>
            /// <param name="in_logName">The name of the logger.</param>
            public void addLogger(string in_logName)
            {
                if (m_loggers.Contains(in_logName))
                    return;

                BMS_Logger logger = BMS_Logger.getLogger(in_logName);

                m_loggers.Add(in_logName, logger);
            }

            /// <summary>
            /// Adds a logger using the provided log factory to create it if necessary
            /// </summary>
            /// <param name="in_logName">The name of the logger</param>
            /// <param name="in_logFactory">The factory for use if creation is necessary.</param>
            public void addLogger(string in_logName, BMS_LogFactory in_logFactory)
            {
                if (m_loggers.Contains(in_logName))
                    return;

                BMS_Logger logger = BMS_Logger.getLogger(in_logName, in_logFactory);

                m_loggers.Add(in_logName, logger);
            }

            /// <summary>
            /// Writes a message to all logs within this logger
            /// </summary>
            /// <param name="in_logLvl">The level of this message.</param>
            /// <param name="in_message">The message to log.</param>
            public override void log(eLogLevel in_logLvl, string in_message)
            {
                foreach (DictionaryEntry it in m_loggers)
                {
                    ((BMS_Logger)it.Value).log(in_logLvl, in_message);
                }
            }

            /// <summary>
            /// Writes a broadcast (system) message to all logs within this logger
            /// </summary>
            /// <param name="in_logLvl">The level of this message.</param>
            /// <param name="in_message">The message to log.</param>
            public override void logBroadcast(eLogLevel in_logLvl, string in_message)
            {
                foreach (DictionaryEntry it in m_loggers)
                {
                    ((BMS_Logger)it.Value).logBroadcast(in_logLvl, in_message);
                }
            }

            /// <summary>
            /// Set's the target of ALL loggers in this log
            /// </summary>
            /// <param name="in_logTarget">The new log target.</param>
            public override void setTarget(string in_logTarget)
            {
                foreach (DictionaryEntry it in m_loggers)
                {
                    ((BMS_Logger)it.Value).setTarget(in_logTarget);
                }
            }

            /// <summary>
            /// Sets the target of the named logger (if it exists in this logger)
            /// </summary>
            /// <param name="in_logName">The name of the logger to modify.</param>
            /// <param name="in_logTarget">The new log target.</param>
            public void setTarget(string in_logName, string in_logTarget)
            {
                if (m_loggers.Contains(in_logName))
                {
                    ((BMS_Logger)m_loggers[in_logName]).setTarget(in_logTarget);
                }
            }

            /// <summary>
            /// Sets the log message filter level for ALL loggers in this logger
            /// </summary>
            /// <param name="in_logLvl">The new log filter level.</param>
            new public void setLogLevel(eLogLevel in_logLvl)
            {
                foreach (DictionaryEntry it in m_loggers)
                {
                    ((BMS_Logger)it.Value).setLogLevel(in_logLvl);
                }
            }

            /// <summary>
            /// Sets the log message filter level for the specified logger
            /// </summary>
            /// <param name="in_logName">The name of the log to modify.</param>
            /// <param name="in_logLvl">The new log filter level.</param>
            public void setLogLevel(string in_logName, eLogLevel in_logLvl)
            {
                if (m_loggers.Contains(in_logName))
                {
                    ((BMS_Logger)m_loggers[in_logName]).setLogLevel(in_logLvl);
                }
            }

            /// <summary>
            /// Shuts down all contained loggers
            /// </summary>
            public override void shutdown()
            {
                foreach (DictionaryEntry it in m_loggers)
                {
                    ((BMS_Logger)it.Value).shutdown();
                }

                m_loggers.Clear();
            }
        }
    }
}
