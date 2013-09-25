using System.Collections.Generic;

namespace BMS.Core
{
    /// <summary>
	/// Wraps a configuration attribute
	/// </summary>
	public class BMS_Setting : BMS_ConfigObject
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
		/// The name of this setting
		/// </summary>
		protected virtual string m_name
		{
			get;
			set;
		}

		/// <summary>
		/// Name-corrolated container for the attributes of this setting
		/// </summary>
		protected virtual Dictionary<string,BMS_Attr> m_attrs
		{
			get;
			set;
		}

		/// <summary>
		/// Name corrolated container for the child settings of this setting
		/// </summary>
		protected virtual Dictionary<string, BMS_Setting> m_children
		{
			get;
			set;
		}

		/// <summary>
		/// The value of this setting
		/// </summary>
		protected virtual string m_value
		{
			get;
			set;
		}

		/// <summary>
		/// Determines if the given attribute exists on this setting
		/// </summary>
		/// <param name="in_attrName">The name of the attribute to search for</param>
		public virtual bool attrExists(string in_attrName)
		{
            return m_attrs.ContainsKey(in_attrName);
		}

		/// <summary>
		/// Retrieves an attribute given the attribute name
		/// </summary>
		/// <param name="in_attrName">The name of the attribute to retrieve</param>
		public virtual BMS_Attr getAttr(string in_attrName)
		{
            return m_attrs[in_attrName];
		}

        /// <summary>
        /// Sets a new attribute
        /// </summary>
        /// <param name="in_attr">The new attribute.</param>
		public virtual void addAttr(BMS_Attr in_attr)
		{
            if (m_attrs.ContainsKey(in_attr.Name))
            {
                m_attrs[in_attr.Name].Value = in_attr.Value;
            }
            else
            {
                m_attrs.Add(in_attr.Name, in_attr);
            }
		}

        /// <summary>
        /// Modifies the named attribute.
        /// </summary>
        /// <param name="in_attrName">The name of the attribute to modify</param>
        /// <param name="in_value">The value of the attribute.</param>
		public virtual void setAttr(string in_attrName, string in_value)
		{
            if (m_attrs.ContainsKey(in_attrName))
            {
                m_attrs[in_attrName].Value = in_value;
            }
		}

        /// <summary>
        /// Retrieves the value of this setting
        /// </summary>
        /// <returns>The setting's value</returns>
		public virtual string getValue()
		{
            return m_value;
		}

        /// <summary>
        /// Modifies this setting's value
        /// </summary>
        /// <param name="in_value">The new value.</param>
		public virtual void setValue(string in_value)
		{
            m_value = in_value;
		}

        /// <summary>
        /// Retrieves the name of this setting
        /// </summary>
        /// <returns>The setting's name</returns>
		public virtual string getName()
		{
            return m_name;
		}

		/// <summary>
		/// Retrieves a specific child setting
		/// </summary>
		/// <param name="in_childName">The local name of the child setting to retrieve</param>
		public virtual BMS_Setting getChild(string in_childName)
        {
            string qualName = m_name + "." + in_childName;
            if (m_children.ContainsKey(qualName))
            {
                return m_children[qualName];
            }
            else
            {
                return null;
            }
		}

        /// <summary>
        /// Retrives the list of this setting's children
        /// </summary>
        /// <returns>An array of BMS_Setting objects referencing this setting's children.</returns>
		public virtual BMS_Setting[] getChildren()
		{
            BMS_Setting[] ret = new BMS_Setting[m_children.Count];
            m_children.Values.CopyTo(ret, 0);
            return ret;
		}

        /// <summary>
        /// Adds a child setting
        /// </summary>
        /// <param name="in_child">The new child</param>
        public virtual void addChild(BMS_Setting in_child)
        {
            if (!m_children.ContainsKey(in_child.getName()))
            {
                m_children.Add(in_child.getName(), in_child);
            }
        }

		/// <summary>
		/// Retrieves all attributes of this setting
		/// </summary>
		public virtual BMS_Attr[] getAttrs()
		{
            BMS_Attr[] ret = new BMS_Attr[m_attrs.Count];
            m_attrs.Values.CopyTo(ret, 0);
            return ret;
		}

        /// <summary>
        /// Default Constructor
        /// </summary>
        public BMS_Setting()
        {
            m_attrs = new Dictionary<string, BMS_Attr>();
            m_children = new Dictionary<string, BMS_Setting>();
            m_name = "";
            m_value = "";
        }

        /// <summary>
        /// Name initialized Constructor
        /// </summary>
        /// <param name="in_name">The name of the new BMS_Setting</param>
        public BMS_Setting(string in_name)
        {
            m_attrs = new Dictionary<string, BMS_Attr>();
            m_children = new Dictionary<string, BMS_Setting>();
            m_name = in_name;
            m_value = "";
        }

        /// <summary>
        /// Name and Value initialized constructor
        /// </summary>
        /// <param name="in_name">The name of the new BMS_Setting.</param>
        /// <param name="in_value">The value of the new BMS_Setting.</param>
        public BMS_Setting(string in_name, string in_value)
        {
            m_attrs = new Dictionary<string, BMS_Attr>();
            m_children = new Dictionary<string, BMS_Setting>();
            m_name = in_name;
            m_value = in_value;
        }
	}
}

