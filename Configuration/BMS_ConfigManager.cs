﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
namespace BMS.Core
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
    using System.Runtime.Serialization;
    using System.Xml;
    using System.Xml.Schema;
    
	/// <summary>
	/// Central manager for all configuration files and settings
	/// </summary>
	public class BMS_ConfigManager : BMS_ConfigObject
    {
        #region Sub-System/Class ID
        /// <summary>
        /// BMS_ConfigManager Class ID
        /// </summary>
        public override byte CLASS_ID
        {
            get { return 0x02; }
        }
        #endregion

        #region Private Attributes

        /// <summary>
        /// Singleton instance of BMS_ConfigManager
        /// </summary>
        private static BMS_ConfigManager m_instance = null;

        #endregion

        #region Protected Attributes
        
        /// <summary>
		/// The collection of loaded configs accessible at O(1) relative performance
		/// </summary>
        protected virtual Dictionary<String, BMS_Config> m_configs
        {
            get;
            set;
        }

        /// <summary>
        /// The config sub-system logger
        /// </summary>
        protected virtual BMS_Logger m_logger
        {
            get;
            set;
        }

        /// <summary>
        /// Collection of all schemas used for validating XML configuration files
        /// </summary>
        protected virtual Dictionary<String, XmlSchema> m_schemas
        {
            get;
            set;
        }

        /// <summary>
        /// The current schema used for config file validation
        /// </summary>
        protected virtual XmlSchema m_curSchema
        {
            get;
            set;
        }

        #endregion

        #region Public Attributes

        /// <summary>
        /// Enumeration of BMS_Configs in the manager
        /// </summary>
        public virtual IEnumerable<BMS_Config> BMS_Config
		{
            get;
			set;
		}

        #endregion

        #region Public Methods

        /// <summary>
		/// Loads a configuration from a specific config file.
		/// </summary>
		/// <param name="in_configName">The internal name of the new configuration</param>
		/// <param name="in_fileName">The file name (including full path) of the configuration file to load</param>
        /// <returns>The loaded BMS_Config, or null if there was a problem</returns>
		public virtual BMS_Config loadConfigFile(string in_configName, string in_fileName)
		{
            //  Validate good parameters
            if (string.IsNullOrWhiteSpace(in_configName))
            {
                throw new ArgumentException("in_configName cannot be null, empty, or only whitespace");
            }

            //  Obtain BMS_Config with which to work
            m_logger.log(this, eLogLevel.INFO, "Loading configuration " + in_configName);

            BMS_Config newConfig = null;
            if (m_configs.ContainsKey(in_configName))
            {
                m_logger.log(this, eLogLevel.INFO, "Config found in manager. Returning.");
                newConfig = m_configs[in_configName];
                return newConfig;
            }
            else
            {
                m_logger.log(this, eLogLevel.INFO, "Config not found in manager. Loading from file " + in_fileName + ".");
                newConfig = new BMS_Config();
            }

            XmlDocument configFile = new XmlDocument();

            try
            {
                configFile.Load(in_fileName);
            }
            catch (Exception ex)
            {
                string outString = "Configuration file " + in_configName + " is bad or inaccessible. " + ex.Message;
                m_logger.log(this, eLogLevel.ERROR, outString);
                throw new BadConfigFileException(outString, ex);
            }

            //  Obtain and validate root node
            XmlNode root = configFile.SelectSingleNode("/bmsconfig");
            if (null == root)
            {
                throw new BadConfigFileException("Configuration file " + in_configName + " is not a BMS configuration file.");
            }

            newConfig.load(root);
            m_configs.Add(newConfig.getName(), newConfig);

            return newConfig;
		}

		/// <summary>
		/// Stores a given BMS_Config to the filesystem as a configuration file.
		/// </summary>
		/// <param name="in_fileName">The filename (including full path) within which to store the configuration</param>
		/// <param name="in_settings">The BMS_Config object to be stored.</param>
		public virtual void saveConfigFile(string in_fileName, BMS_Config in_settings)
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// Accesses the configuration of the provided name.
		/// </summary>
		/// <param name="in_configName">The name of the configuration to query</param>
		public virtual BMS_Config getConfig(string in_configName)
		{
            if (m_configs.ContainsKey(in_configName))
            {
                return m_configs[in_configName];
            }
            else
            {
                return null;
            }
		}

        /// <summary>
        /// Sets the schema with which to validate subsequent config loads
        /// </summary>
        /// <param name="in_schemaName">The name of this schema for filling in the Manager</param>
        /// <param name="in_fileName">The file name (including full path) to the schema definition or null if accessing a previously stored configuration.</param>
        /// <remarks>This schema is used to validate all subsequent loads.  No validation is performed
        /// on previously loaded configurations.</remarks>
        public virtual void setConfigSchemaFile(string in_schemaName, string in_fileName)
        {
            //  Validate parameters
            if (string.IsNullOrWhiteSpace(in_schemaName))
            {
                throw new ArgumentException("in_schemaName cannot be null, empty, or only whitespace");
            }

            m_logger.log(this, eLogLevel.INFO, "Setting new schema for config validation...");

            if (m_schemas.ContainsKey(in_schemaName))
            {
                m_logger.log(this, eLogLevel.INFO, "Schema found within Manager. Making current.");
                m_curSchema = m_schemas[in_schemaName];
            }
            else
            {
                m_logger.log(this, eLogLevel.INFO, "Schema not found in Manager. Loading from " + in_fileName + "...");
                try
                {
                    XmlTextReader read = new XmlTextReader(in_fileName);
                    XmlSchema newSchema = XmlSchema.Read(read, schemaReadCallback);
                    m_curSchema = newSchema;
                    m_logger.log(this, eLogLevel.INFO, "Schema " + in_schemaName + " loaded.");
                }
                catch (Exception ex)
                {
                    string outString = "Error loading config schema " + in_schemaName;
                    m_logger.log(this, eLogLevel.ERROR, outString + " : " + ex.Message);
                    throw new BadConfigDefException(outString, ex);
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
		/// Hidden singleton constructor
		/// </summary>
		private BMS_ConfigManager()
		{
            m_configs = new Dictionary<string, BMS_Config>();
            m_schemas = new Dictionary<string, XmlSchema>();
            m_curSchema = null;
            m_instance = null;
            m_logger = BMS_Logger.getLogger("BMS.Core", new BMS_MultiLogFactory());
            
            ((BMS_MultiLog)m_logger).addLogger(m_logger.getName() + ".file", new BMS_FileLogFactory());
#if DEBUG
            ((BMS_MultiLog)m_logger).addLogger(m_logger.getName() + ".console", new BMS_ConsoleLogFactory());
#endif
            //  All configuration messages are logged at TRACE Level during config loading
            m_logger.setLogLevel(eLogLevel.TRACE);

            m_logger.log(this, eLogLevel.INFO, "Configuration Manager initialized successfully!");
		}

        /// <summary>
        /// Schema validation call back
        /// </summary>
        /// <param name="sender">The object sending the validation information.</param>
        /// <param name="args">The arguments regarding this particular validation event.</param>
        private void schemaReadCallback (object sender, ValidationEventArgs args)
        {
            switch (args.Severity)
            {
                case XmlSeverityType.Warning:
                    m_logger.log(this, eLogLevel.WARN, "Validation warning in configuration schema: " + args.Message);
                    break;

                case XmlSeverityType.Error:
                    m_logger.log(this, eLogLevel.ERROR, "Validation error in configuration schema: " + args.Message);
                    break;
            }

            m_logger.log(this, eLogLevel.INFO, "Configuration schema load completed." + args.Message);
        }

        #endregion

        #region Static Methods

        /// <summary>
		/// Provides access to the singleton BMS_ConfigManager instance
		/// </summary>
		public static BMS_ConfigManager GetInstance()
		{
            if (null == m_instance)
            {
                m_instance = new BMS_ConfigManager();
            }

            return m_instance;
        }

        #endregion

    }

    /// <summary>
    /// Exception wrapping a mal-formed configuration file
    /// </summary>
    [Serializable()]
    public class BadConfigFileException : Exception
    {
        public BadConfigFileException() : base(){}
        public BadConfigFileException(string in_message) : base(in_message){}
        public BadConfigFileException(string in_message, Exception in_innerEx) : base (in_message, in_innerEx){}
        protected BadConfigFileException(SerializationInfo in_serialInfo, StreamingContext in_context){}
    }

    /// <summary>
    /// Exception wrapping a mal-formed configuration file definition
    /// </summary>
    [Serializable()]
    public class BadConfigDefException : Exception
    {
        public BadConfigDefException() : base() { }
        public BadConfigDefException(string in_message) : base(in_message) { }
        public BadConfigDefException(string in_message, Exception in_innerEx) : base(in_message, in_innerEx) { }
        protected BadConfigDefException(SerializationInfo in_serialInfo, StreamingContext in_context) { }
    }

}
