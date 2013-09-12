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
	/// Wraps a single instance of a configuration (analogous to a config file in memory)
	/// </summary>
	public class BMS_Config
	{
		/// <summary>
		/// The raw configuration, used to provide easy read/write-back to disc.
		/// </summary>
		protected virtual XmlDocument m_rawConfig
		{
			get;
			set;
		}

		/// <summary>
		/// Attr names can contain tree-like entries to access sub attrs (i.e. LoggingConfig/DefaultLogLevel)
		/// </summary>
		protected virtual Dictionary<String, BMS_Attr> m_configAttrs
		{
			get;
			set;
		}

		/// <summary>
		/// Retrieves a string valued configuration setting.
		/// </summary>
		/// <param name="in_attrName">The name of the attribute to query</param>
		public virtual string getStringVal(string in_attrName)
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// Returns the loaded configuration's version.
		/// </summary>
		public virtual string getConfigVer()
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// Modifies a specific attribute
		/// </summary>
		/// <param name="in_attrName">The name of the attribute to modify</param>
		/// <param name="in_val">The new value for the attribute</param>
		public virtual void setAttrValue<T>(string in_attrName, T in_val)
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// Retrieves an integer valued configuration attribute.
		/// </summary>
		/// <param name="in_attrName">The name of the attribute to query</param>
		public virtual int getIntVal(string in_attrName)
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// Retrieves a boolean valued configuration attribute.
		/// </summary>
		/// <param name="in_atterName">The name of the attribute to query</param>
		public virtual bool getBoolVal(string in_atterName)
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// Determines the type of the given attribute.
		/// </summary>
		/// <param name="in_attrName">The name of the attribute to query</param>
		public virtual Type getAttrType(object in_attrName)
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// Determines is a given attribute exists in this configuration.
		/// </summary>
		/// <param name="in_attrName">The name of the attribute to query</param>
		public virtual bool doesAttrExist(object in_attrName)
		{
			throw new System.NotImplementedException();
		}

	}
}

