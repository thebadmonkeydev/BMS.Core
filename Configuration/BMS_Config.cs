using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace BMS.Core
{
    /// <summary>
    /// Wraps a single instance of a configuration (analogous to a config file in memory)
    /// </summary>
    public class BMS_Config
    {
        /// <summary>
        /// The configuration in it's raw XML form
        /// </summary>
        protected virtual XmlDocument m_raw
        {
            get;
            set;
        }

        /// <summary>
        /// Container for the settings within this configuration
        /// </summary>
        protected virtual Dictionary<string, BMS_Setting> m_settings
        {
            get;
            set;
        }

        /// <summary>
        /// Flag indicating if the configuration is loaded
        /// </summary>
        protected virtual bool m_isLoaded
        {
            get;
            set;
        }

        /// <summary>
        /// This configuration's name
        /// </summary>
        protected virtual string m_name
        {
            get;
            set;
        }

        /// <summary>
        /// THe configuration version
        /// </summary>
        protected virtual int m_version
        {
            get;
            set;
        }

        /// <summary>
        /// Retrieves a specific setting
        /// </summary>
        /// <param name="in_setName">The name of the setting to retrieve.</param>
        /// <returns>The <seealso cref="BMS_Setting"/> with the provided name or null.</returns>
        public virtual BMS_Setting getSetting(string in_setName)
        {
            if (m_isLoaded)
            {
                if (m_settings.ContainsKey(in_setName))
                {
                    return m_settings[in_setName];
                }
            }

            return null;
        }

        /// <summary>
        /// Loads a configuration from the given XML
        /// </summary>
        /// <param name="in_configDoc">The XmlDocument representing the desired configuration.</param>
        public virtual void load(XmlDocument in_configDoc)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Saves the configuration to a Stream
        /// </summary>
        /// <param name="in_output">The writer for the target save stream.</param>
        public virtual void save(StreamWriter in_output)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Determines if the configuration is loaded
        /// </summary>
        /// <returns>True if the configuration is properly loaded, false otherwise.</returns>
        public virtual bool isLoaded()
        {
            return m_isLoaded;
        }
    }
}

