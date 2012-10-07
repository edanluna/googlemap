using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace Artem.Web.UI.Controls {

    /// <summary>
    /// 
    /// </summary>
    public class GoogleLocation : IStateManager {

        #region Static Methods //////////////////////////////////////////////////////////

        /// <summary>
        /// Parses the specified pair.
        /// </summary>
        /// <param name="pair">The pair.</param>
        /// <returns></returns>
        public static GoogleLocation Parse(string point) {

            double lat = 0D;
            double lng = 0D;

            if (!string.IsNullOrEmpty(point)) {
                point = point.Trim('(', ')');
                string[] pair = point.Split(',');
                if (pair.Length >= 2) {
                    lat = JsUtil.ToDouble(pair[0]);
                    lng = JsUtil.ToDouble(pair[1]);
                }
            }

            return new GoogleLocation(lat, lng);
        }
        #endregion

        #region Fields  /////////////////////////////////////////////////////////////////

        private double _latitude;
        private double _longitude;

        #endregion

        #region Properties  /////////////////////////////////////////////////////////////

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        [JsonData]
        public double Latitude {
            get { return _latitude; }
            set { _latitude = value; }
        }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        [JsonData]
        public double Longitude {
            get { return _longitude; }
            set { _longitude = value; }
        }
        #endregion

        #region Construct  //////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="GooglePoint"/> class.
        /// </summary>
        public GoogleLocation() {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GooglePoint"/> struct.
        /// </summary>
        /// <param name="lat">The lat.</param>
        /// <param name="lng">The LNG.</param>
        public GoogleLocation(double lat, double lng) {
            _latitude = lat;
            _longitude = lng;
            _tracking = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GooglePoint"/> struct.
        /// </summary>
        /// <param name="source">The source.</param>
        public GoogleLocation(GoogleLocation source) {
            _latitude = source.Latitude;
            _longitude = source.Longitude;
            _tracking = source._tracking;
        }
        #endregion

        #region Methods /////////////////////////////////////////////////////////////////

        /// <summary>
        /// Toes the json.
        /// </summary>
        /// <returns></returns>
        public string ToJsonString() {
            return JsonSerializer<GoogleLocation>.Serialize(this);
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString() {
            return string.Format("{0},{1}",
                JsUtil.Encode(this.Latitude), JsUtil.Encode(this.Longitude));
        }
        #endregion

        #region IStateManager Members ///////////////////////////////////////////////////

        bool _tracking;

        /// <summary>
        /// When implemented by a class, gets a value indicating whether a server control is tracking its view state changes.
        /// </summary>
        /// <value></value>
        /// <returns>true if a server control is tracking its view state changes; otherwise, false.</returns>
        bool IStateManager.IsTrackingViewState {
            get { return _tracking; }
        }

        /// <summary>
        /// Loads the state of the view.
        /// </summary>
        /// <param name="savedState">State of the saved.</param>
        void IStateManager.LoadViewState(object savedState) {

            Pair state = savedState as Pair;
            if (state != null) {
                Latitude = (double)state.First;
                Longitude = (double)state.Second;
            }
        }

        /// <summary>
        /// When implemented by a class, saves the changes to a server control's view state to an <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.Object"/> that contains the view state changes.
        /// </returns>
        object IStateManager.SaveViewState() {
            return new Pair(Latitude, Longitude);
        }

        /// <summary>
        /// When implemented by a class, instructs the server control to track changes to its view state.
        /// </summary>
        void IStateManager.TrackViewState() {
            _tracking = true;
        }
        #endregion
    }
}
