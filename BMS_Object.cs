using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core
{
    /// <summary>
    /// Base class for all BMS enterprise classes
    /// </summary>
    public class BMS_Object
    {
        #region Subsystem/Class ID Declarations

        /// <summary>
        /// Root level Sub-System ID
        /// </summary>
        public virtual byte SUBSYS_ID
        {
            get { return 0x00; }
        }

        /// <summary>
        /// BMS_Object class ID
        /// </summary>
        public virtual byte CLASS_ID
        {
            get { return 0x01; }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Retrieves the string valued sub-system name based on the sub system ID
        /// </summary>
        /// <param name="in_subsysID"></param>
        /// <returns></returns>
        public static string getSubSysName(byte in_subsysID)
        {
            return m_subsysNameMap[in_subsysID];
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Thread safety object inherited throughout
        /// </summary>
        protected static object sync = new object();

        #endregion

        #region Private Properties

        /// <summary>
        /// Maps byte valued IDs to sub-system name string values
        /// </summary>
        private static readonly Dictionary<byte, string> m_subsysNameMap = new Dictionary<byte,string>()
        {
            {0x00, "Core"},
            {0x01, "Core.Logging"},
            {0x02, "Core.Configuration"},
            {0x03, "Core.Exceptions"},
            {0x04, "Core.Threading"},
            {0x05, "Core.Utility"}
        };

        #endregion
    }
}
