namespace BMS.Core
{
	/// <summary>
	/// Wraps a configuration attribute
	/// </summary>
    public class BMS_Attr : BMS_ConfigObject
    {
        #region Sub-System/Class ID
        /// <summary>
        /// BMS_Attr Class ID
        /// </summary>
        public override byte CLASS_ID
        {
            get { return 0x04; }
        }
        #endregion

        /// <summary>
        /// The attribute's name
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// The value of this attribute
        /// </summary>
        public string Value
        {
            get;
            set;
        }
    }
}

