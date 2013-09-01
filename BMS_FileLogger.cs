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
        public class BMS_FileLogFactory : BMS_LogFactory 
        {
            public override BMS_Logger create(string in_fileName)
            {
                BMS_FileLogger ret = new BMS_FileLogger();
                ret.setTarget(in_fileName);

                return ret;
            }
        }

        public class BMS_FileLogger : BMS_Logger
        {
            protected string m_fileURI;

            /// <summary>
            /// Default constructor.  Initializes log level and file location to default values (INFO and ./log.log)
            /// </summary>
            public BMS_FileLogger()
            {   //  Set defaults for a file logger
                m_curLogLevel = eLogLevel.INFO;
                m_fileURI = "./log.log";
            }

            /// <summary>
            /// Constructs a BMS_FileLogger using the provided log file name and sets the default log level to INFO
            /// </summary>
            /// <param name="in_logFileName">The path to the intended log output file.</param>
            public BMS_FileLogger(string in_logFileName)
            {
                m_curLogLevel = eLogLevel.INFO;
                m_fileURI = in_logFileName;
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
            }

            /// <summary>
            /// Logs a message to the file log
            /// </summary>
            /// <param name="in_logLvl">The log level of this message.</param>
            /// <param name="in_message">The message.</param>
            public override void log(eLogLevel in_logLvl, string in_message)
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
                    logWriter.WriteLine(timeStamp + "\t" + BMS_Logger.getLevelTag(in_logLvl) + "\t" + in_message);
                    logWriter.Flush();
                    logWriter.Close();
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    if (logWriter != null)
                    {
                        logWriter.Dispose();
                    }
                }
            }

            public override void logBroadcast(eLogLevel in_logLvl, string in_message)
            {
                StreamWriter logWriter = null;
                string timeStamp = BMS_Logger.getTimeStamp();
                try
                {
                    logWriter = new StreamWriter(m_fileURI, true);
                    logWriter.WriteLine(timeStamp + "\t" + BMS_Logger.getLevelTag(in_logLvl) + "\t" + in_message);
                    logWriter.Flush();
                    logWriter.Close();
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    if (logWriter != null)
                    {
                        logWriter.Dispose();
                    }
                }
            }

            /// <summary>
            /// Sets the file location for this logger.
            /// </summary>
            /// <param name="in_logTarget">The new log target as a file path.</param>
            public override void setTarget(string in_logTarget)
            {
                m_fileURI = in_logTarget;
            }
        }
    }
}
