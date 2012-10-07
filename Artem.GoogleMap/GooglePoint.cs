using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Web.UI;

namespace Artem.Web.UI.Controls {

    /// <summary>
    /// 
    /// </summary>
    public struct GooglePoint : IStateManager {

        #region Static Fields ///////////////////////////////////////////////////////////

        public static readonly GooglePoint Empty;

        public static readonly GooglePoint DefaultMarkerIconAnchor = new GooglePoint(8, 16);

        #endregion

        #region Static Methods //////////////////////////////////////////////////////////

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(GooglePoint a, GooglePoint b) {
            return ((a.X == b.X) && (a.Y == b.Y));
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(GooglePoint a, GooglePoint b) {
            return !(a == b);
        }

        /// <summary>
        /// Parses the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static GooglePoint Parse(string value) {
            string[] pair = value.Split(',');
            return new GooglePoint(JsUtil.ToInt(pair[0]), JsUtil.ToInt(pair[1]));
        }
        #endregion

        #region Fields  /////////////////////////////////////////////////////////////////

        [JsonData]
        public int X;
        [JsonData]
        public int Y;

        #endregion

        #region Construct  //////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="GooglePoint"/> struct.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public GooglePoint(int x, int y) {
            this.X = x;
            this.Y = y;
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

            if (!(obj is GooglePoint)) return false;
            GooglePoint point = (GooglePoint)obj;
            return ((point.X == this.X) && (point.Y == this.Y));
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
            return string.Format("{0},{1}", X.ToString(), Y.ToString());
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
                this.X = (int)state.First;
                this.Y = (int)state.Second;
            }
        }

        object IStateManager.SaveViewState() {
            return new Pair(this.X, this.Y);
        }

        void IStateManager.TrackViewState() {
            _tracking = true;
        }
        #endregion
    }
}
