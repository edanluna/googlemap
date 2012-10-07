using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;
using System.Security.Permissions;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;

namespace Artem.Web.UI.Controls {

    /// <summary>
    /// 
    /// </summary>
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ParseChildren(true, "Points")]
    [ToolboxData("<{0}:GooglePolyline runat=\"server\"> </{0}:GooglePolyline>")]
    public class GooglePolyline : IStateManager {

        #region Fields  /////////////////////////////////////////////////////////////////

        IList<string> _actions;
        GoogleBounds _bounds;
        Color _color;
        bool _isClickable = true;
        bool _isGeodesic = false;
        float _opacity = .5F;
        StateCollection<GoogleLocation> _points;
        int _weight = 5;

        #endregion

        #region Properties  /////////////////////////////////////////////////////////////

        /// <summary>
        /// Gets or sets the actions.
        /// </summary>
        /// <value>The actions.</value>
        protected internal IList<string> Actions {
            get {
                if (_actions == null)
                    _actions = new List<string>();
                return _actions; 
            }
        }

        /// <summary>
        /// Gets or sets the bounds of the polyline. 
        /// Have in mind it requires an additional post back after polyline is loaded.
        /// </summary>
        /// <value>The bounds.</value>
        public GoogleBounds Bounds {
            get { return _bounds; }
            protected internal set { _bounds = value; }
        }

        /// <summary>
        /// Gets or sets a value for color of the polyline. 
        /// The color is given as a string that contains the color in hexadecimal numeric HTML style, i.e. #RRGGBB.
        /// </summary>
        /// <value>The color.</value>
        [JsonData]
        public Color Color {
            get { return _color; }
            set { _color = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this polyline is clickable.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is clickable; otherwise, <c>false</c>.
        /// </value>
        [JsonData]
        public bool IsClickable {
            get { return _isClickable; }
            set { _isClickable = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this polyline is geodesic.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is geodesic; otherwise, <c>false</c>.
        /// </value>
        [JsonData]
        public bool IsGeodesic {
            get { return _isGeodesic; }
            set { _isGeodesic = value; }
        }

        /// <summary>
        /// Gets or sets the opacity of polyline.
        /// The opacity is given as a number between 0 and 1. The line will be antialiased and semitransparent.
        /// </summary>
        /// <value>The opacity.</value>
        [JsonData]
        public float Opacity {
            get { return _opacity; }
            set { _opacity = value; }
        }

        /// <summary>
        /// Gets or sets the points of polyline.
        /// </summary>
        /// <value>The points.</value>
        [Browsable(true)]
        [Category("Google")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        [JsonData]
        public StateCollection<GoogleLocation> Points {
            get {
                if (_points == null) {
                    _points = new StateCollection<GoogleLocation>();
                    if ((this as IStateManager).IsTrackingViewState)
                        _points.TrackViewState();
                }
                return _points;
            }
            protected internal set {
                _points = value;
                if ((_points != null) && ((this as IStateManager).IsTrackingViewState)) {
                    _points.TrackViewState();
                }
            }
        }

        /// <summary>
        /// Gets or sets the weight of polyline.
        /// The weight is the width of the line in pixels.
        /// </summary>
        /// <value>The weight.</value>
        [JsonData]
        public int Weight {
            get { return _weight; }
            set { _weight = value; }
        }
        #endregion

        #region Methods /////////////////////////////////////////////////////////////////

        /// <summary>
        /// Toes the json string.
        /// </summary>
        /// <returns></returns>
        public string ToJsonString() {
            return JsonSerializer<GooglePolyline>.Serialize(this);
        }

        #region - Actions -

        /// <summary>
        /// Fires the client-side hide() of the google polyline.
        /// </summary>
        public void Hide() {
            this.Actions.Add("{0}.Polylines[{1}].hide();");
        }

        /// <summary>
        /// Fires the client-side show() of the google polyline.
        /// </summary>
        public void Show() {
            this.Actions.Add("{0}.Polylines[{1}].show();");
        }
        #endregion
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
                _color = (Color)state[0];
                _isClickable = (bool)state[1];
                _isGeodesic = (bool)state[2];
                _opacity = (float)state[3];
                _points.LoadViewState(state[4]);
                _weight = (int)state[5];
                ((IStateManager)_bounds).LoadViewState(state[6]);
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
                _color, _isClickable, _isGeodesic, _opacity, 
                _points.SaveViewState(), _weight, 
                ((IStateManager)_bounds).SaveViewState()};
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
