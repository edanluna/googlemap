using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Drawing;

namespace Artem.Web.UI.Controls {

    /// <summary>
    /// 
    /// </summary>
    public class GoogleCirclePolygon : GooglePolygon, IStateManager {

        #region Fields  /////////////////////////////////////////////////////////////////

        double _latitude;
        double _longitude;
        double _radius;

        #endregion

        #region Properties  /////////////////////////////////////////////////////////////

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        [JsonData]
        public double Latitude {
            get { return _latitude; }
            set {
                if (_latitude != value) {
                    _latitude = value;
                    BuildPoints();
                }
            }
        }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        [JsonData]
        public double Longitude {
            get { return _longitude; }
            set {
                if (_longitude != value) {
                    _longitude = value;
                    BuildPoints();
                }
            }
        }

        /// <summary>
        /// Gets or sets the radius.
        /// </summary>
        /// <value>The radius.</value>
        [JsonData]
        public double Radius {
            get { return _radius; }
            set {
                if (_radius != value) {
                    _radius = value;
                    BuildPoints();
                }
            }
        }
        #endregion

        #region Construct  //////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="GooglePoint"/> class.
        /// </summary>
        public GoogleCirclePolygon() {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GooglePoint"/> struct.
        /// </summary>
        /// <param name="lat">The lat.</param>
        /// <param name="lng">The LNG.</param>
        public GoogleCirclePolygon(double lat, double lng, int radius) {
            _latitude = lat;
            _longitude = lng;
            _radius = radius;
        }
        #endregion

        #region Methods /////////////////////////////////////////////////////////////////

        /// <summary>
        /// Builds the points.
        /// </summary>
        void BuildPoints() {

            bool canBuild = (Latitude !=0 && Longitude != 0 && Radius != 0);
            if (canBuild) {
                this.Points.Clear();
                double d2r = Math.PI / 180.0D; // degree to radian
                double r2d = 180.0D / Math.PI;
                double lat = ((double)Radius / 3963.0D) * r2d;
                double lng = lat / Math.Cos(Latitude * d2r);
                double theta, x, y;
                GoogleLocation firstPoint = null;
                for (int i = 0; i < 33; i++) {
                    theta = (double)Math.PI * ((double)i / 16.0D);
                    x = Latitude + (lat * Math.Sin(theta));
                    y = Longitude + (lng * Math.Cos(theta));
                    if (firstPoint != null) {
                        this.Points.Add(new GoogleLocation(x, y));
                    }
                    else {
                        this.Points.Add(firstPoint = new GoogleLocation(x, y));
                    }
                }
                this.Points.Add(firstPoint);
            }
        }

        /// <summary>
        /// Toes the json string.
        /// </summary>
        /// <returns></returns>
        public override string ToJsonString() {
            return JsonSerializer<GoogleCirclePolygon>.Serialize(this);
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

            object[] state = savedState as object[];
            if (state != null) {
                FillColor = (Color)state[0];
                FillOpacity = (float)state[1];
                IsClickable = (bool)state[2];
                Points.LoadViewState(state[3]);
                StrokeColor = (Color)state[4];
                StrokeOpacity = (float)state[5];
                StrokeWeight = (int)state[6];
                ((IStateManager)Bounds).LoadViewState(state[7]);
                Latitude = (double)state[8];
                Longitude = (double)state[9];
                Radius = (int)state[10];
            }
        }

        /// <summary>
        /// When implemented by a class, saves the changes to a server control's view state to an <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.Object"/> that contains the view state changes.
        /// </returns>
        object IStateManager.SaveViewState() {
            return new object[] { 
                FillColor, 
                FillOpacity, 
                IsClickable, 
                Points.SaveViewState(), 
                StrokeColor,
                StrokeOpacity,
                StrokeWeight,
                ((IStateManager)Bounds).SaveViewState(),
                Latitude, 
                Longitude, 
                Radius
            };
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
