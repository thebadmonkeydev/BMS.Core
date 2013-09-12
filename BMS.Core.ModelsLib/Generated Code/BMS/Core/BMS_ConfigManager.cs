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

	/// <summary>
	/// Central manager for all configuration files and settings
	/// </summary>
	public class BMS_ConfigManager
	{
		/// <summary>
		/// The collection of loaded configs accessible at O(1) relative performance
		/// </summary>
		protected virtual Dictionary<String, BMS_Config> m_configs
		{
			get;
			set;
		}

		/// <summary>
		/// Singleton instance of BMS_ConfigManager
		/// </summary>
		private static BMS_ConfigManager m_instance
		{
			get;
			set;
		}

		protected virtual BMS_Logger m_logger
		{
			get;
			set;
		}

		protected virtual Dictionary<String,XMLSchema> m_schemas
		{
			get;
			set;
		}

		public virtual IEnumerable<BMS_Config> BMS_Config
		{
			get;
			set;
		}

		/// <summary>
		/// Loads a configuration from a specific config file.
		/// </summary>
		/// <param name="in_configName">The internal name of the new configuration</param>
		/// <param name="in_fileName">The file name (including full path) of the configuration file to load</param>
		public virtual BMS_Config loadConfigFile(string in_configName, string in_fileName)
		{
			throw new System.NotImplementedException();
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
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// Hidden singleton constructor
		/// </summary>
		private BMS_ConfigManager()
		{
		}

		/// <summary>
		/// Provides access to the singleton BMS_ConfigManager instance
		/// </summary>
		public static BMS_ConfigManager GetInstance()
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// Sets the schema with which to validate subsequent config loads
		/// </summary>
		/// <param name="in_schemaName">The name of this schema for filling in the Manager</param>
		/// <param name="in_fileName">The file name (including full path) to the schema definition.</param>
		public virtual void setConfigSchemaFile(string in_schemaName, string in_fileName)
		{
			throw new System.NotImplementedException();
		}

	}
}

