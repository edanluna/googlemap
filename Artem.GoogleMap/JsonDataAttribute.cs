using System;
using System.Collections.Generic;
using System.Text;

namespace Artem.Web.UI.Controls {

    /// <summary>
    /// 
    /// </summary>
    public enum JsonDataType {
        ReadWrite,
        ReadOnly
    }

    /// <summary>
    /// Marker attribute for the Json serialization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class JsonDataAttribute : Attribute {

        #region Properties  ///////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Gets or sets the type of the data.
        /// </summary>
        /// <value>The type of the data.</value>
        public JsonDataType DataType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="JsonDataAttribute"/> is encode.
        /// </summary>
        /// <value><c>true</c> if encode; otherwise, <c>false</c>.</value>
        public bool Encode { get; set; }

        #endregion

        #region Construct /////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonDataAttribute"/> class.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        /// <param name="encode">if set to <c>true</c> [encode].</param>
        public JsonDataAttribute(JsonDataType dataType, bool encode) {
            this.DataType = dataType;
            this.Encode = encode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonDataAttribute"/> class.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        public JsonDataAttribute(JsonDataType dataType)
            : this(dataType, false) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonDataAttribute"/> class.
        /// </summary>
        /// <param name="encode">if set to <c>true</c> [encode].</param>
        public JsonDataAttribute(bool encode)
            : this(JsonDataType.ReadWrite, encode) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonDataAttribute"/> class.
        /// </summary>
        public JsonDataAttribute()
            : this(JsonDataType.ReadWrite, false) {
        }
        #endregion
    }
}
