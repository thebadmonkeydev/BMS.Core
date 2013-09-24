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
    public class BMS_Config : BMS_ConfigObject
    {
        #region Sub-System/Class ID
        /// <summary>
        /// BMS_FileLogFactory Class ID
        /// </summary>
        public override byte CLASS_ID
        {
            get { return 0x03; }
        }
        #endregion

        /// <summary>
        /// The configuration in it's raw XML form
        /// </summary>
        protected virtual XmlNode m_raw
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
        /// Retrieve this config's name
        /// </summary>
        /// <returns>The config name as a string</returns>
        public virtual string getName() { return m_name; }

        public BMS_Config()
        {
            m_isLoaded = false;
            m_name = "";
            m_raw = null;
            m_settings = new Dictionary<string, BMS_Setting>();
            m_version = 0;
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
        /// <remarks>Assumes that the provided document points to the top level element as root</remarks>
        public virtual void load(XmlNode in_configDoc)
        {
            m_raw = in_configDoc;

            foreach (XmlAttribute it in in_configDoc.Attributes)
            {
                if (it.Name == "version")
                {
                    try
                    {
                        m_version = Int32.Parse(it.Value);
                    }
                    catch (Exception e)
                    {
                        throw new BadConfigFileException("Config file version does not have the proper format.", e);
                    }
                }
            }

            try
            {
                if (load_rec(in_configDoc, "") == null)
                {
                    throw new BadConfigFileException("No parsable elements present in config file.");
                }
            }
            catch (Exception e)
            {
                throw new BadConfigFileException("Could not load config from XML.", e);
            }

            m_isLoaded = true;
        }

        private BMS_Setting load_rec(XmlNode in_configNode, string in_parentName)
        {
            BMS_Setting curSetting = null;

            switch (in_configNode.NodeType)
            {
                case XmlNodeType.Element:
                    string qualName = (in_parentName == "") ? in_configNode.Name : in_parentName + "." + in_configNode.Name;
                    curSetting = new BMS_Setting(qualName);
                    curSetting.setValue(in_configNode.Value);
                    
                    //  Handle any attributes for this element
                    foreach (XmlAttribute at in in_configNode.Attributes)
                    {
                        curSetting.addAttr(new BMS_Attr(at.Name, at.Value));
                    }

                    //  Handle the children of this element
                    foreach (XmlNode it in in_configNode.ChildNodes)
                    {
                        if (it.NodeType == XmlNodeType.Text)
                        {
                            curSetting.setValue(it.Value);
                        }
                        else
                        {
                            BMS_Setting newChild = load_rec(it, qualName);

                            if (newChild != null)
                            {
                                curSetting.addChild(newChild);
                            }
                        }
                    }

                    m_settings.Add(curSetting.getName(), curSetting);
                    break;

                default:
                    break;
            }
            return curSetting;
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

