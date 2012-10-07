using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace Artem.Web.UI.Controls {

    /// <summary>
    /// 
    /// </summary>
    public struct GoogleSize : IStateManager {

        #region Static Fields ///////////////////////////////////////////////////////////

        public static readonly GoogleSize Empty;

        public static readonly GoogleSize DefaultMarkerIconSize = new GoogleSize(16, 16);
        public static readonly GoogleSize DefaultMarkerShadowSize = new GoogleSize(16, 16);

        #endregion

        #region Static Methods //////////////////////////////////////////////////////////

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(GoogleSize a, GoogleSize b) {
            return ((a.Width == b.Width) && (a.Height == b.Height));
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(GoogleSize a, GoogleSize b) {
            return !(a == b);
        }

        /// <summary>
        /// Parses the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static GoogleSize Parse(string value) {
            string[] pair = value.Split(',');
            return new GoogleSize(JsUtil.ToInt(pair[0]), JsUtil.ToInt(pair[1]));
        }
        #endregion

        #region Fields  /////////////////////////////////////////////////////////////////

        [JsonData]
        public int Height;
        [JsonData]
        public int Width;

        #endregion

        #region Construct  //////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleSize"/> struct.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public GoogleSize(int width, int height) {
            this.Width = width;
            this.Height = height;
            _tracking = false;
        }
        #endregion

        #region Methods /////////////////////////////////////////////////////////////////

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        public override bool Equals(object obj) {

            if (!(obj is GoogleSize)) return false;
            GoogleSize size = (GoogleSize)obj;
            return ((size.Width == this.Width) && (size.Height == this.Height));
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode() {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> containing a fully qualified type name.
        /// </returns>
        public override string ToString() {
            return string.Format("{0},{1}", Width.ToString(), Height.ToString());
        }
        #endregion

        #region IStateManager Members ///////////////////////////////////////////////////

        bool _tracking;

        bool IStateManager.IsTrackingViewState {
            get { return _tracking; }
        }

        void IStateManager.LoadViewState(object savedState) {

            Pair state = savedState as Pair;
            if (state != null) {
                this.Width = (int)state.First;
                this.Height = (int)state.Second;
            }
        }

        object IStateManager.SaveViewState() {
            return new Pair(this.Width, this.Height);
        }

        void IStateManager.TrackViewState() {
            _tracking = true;
        }
        #endregion
    }
}
