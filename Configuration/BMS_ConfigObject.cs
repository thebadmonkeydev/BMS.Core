using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core
{
    /// <summary>
    /// Base class for all configuration sub system objects
    /// </summary>
    public class BMS_ConfigObject : BMS_Object
    {
        /// <summary>
        /// Subsystem ID
        /// </summary>
        public override byte SUBSYS_ID
        {
            get { return 0x02; }
        }

        /// <summary>
        /// BMS_ConfigObject ID
        /// </summary>
        public override byte CLASS_ID
        {
            get { return 0x01; }
        }
    }
}
