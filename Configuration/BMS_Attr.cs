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

        /// <summary>
        /// Default Constructor
        /// </summary>
        public BMS_Attr()
        {
            Name = "";
            Value = "";
        }

        /// <summary>
        /// Name initialized constructor
        /// </summary>
        /// <param name="in_name">The <seealso cref="BMS_Attr"/>'s name</param>
        public BMS_Attr(string in_name)
        {
            Name = in_name;
            Value = "";
        }

        /// <summary>
        /// Name and Value initialized constructor
        /// </summary>
        /// <param name="in_name">The name of the new <seealso cref="BMS_Attr"/>.</param>
        /// <param name="in_value">The value of this <seealso cref="BMS_Attr"/>.</param>
        public BMS_Attr(string in_name, string in_value)
        {
            Name = in_name;
            Value = in_value;
        }
    }
}

