using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core
{
    /// <summary>
    /// Base class for all logging sub system classes
    /// </summary>
    public class BMS_LoggingObject : BMS_Object
    {
        /// <summary>
        /// Logging sub system ID
        /// </summary>
        public override byte SUBSYS_ID
        {
            get { return 0x01; }
        }

        /// <summary>
        /// Log base class ID
        /// </summary>
        public override byte CLASS_ID
        {
            get { return 0x01; }
        }
    }
}
