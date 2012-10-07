using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Security.Permissions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Drawing;

namespace Artem.Web.UI.Controls {

    /// <summary>
    /// 
    /// </summary>
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ParseChildren(true, "Points")]
    [ToolboxData("<{0}:GooglePolygon runat=\"server\"> </{0}:GooglePolygon>")]
    public class GooglePolygon : IStateManager {

        #region Fields  /////////////////////////////////////////////////////////////////

        IList<string> _actions;
        GoogleBounds _bounds;
        bool _enableDrawing;
        bool _enableEditing;
        Color _fillColor;
        float _fillOpacity = .5F;
        bool _isClickable = true;
        StateCollection<GoogleLocation> _points;
        Color _strokeColor;
        float _strokeOpacity = .5F;
        int _strokeWeight = 5;

        #endregion

        #region Properties  /////////////////////////////////////////////////////////////

        /// <summary>
        /// Gets or sets the actions of the polygon.
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
        /// Gets or sets the bounds of the polygon. 
        /// Have in mind it requires an additional post back after polygon is loaded.
        /// </summary>
        /// <value>The bounds.</value>
        public GoogleBounds Bounds {
            get { return _bounds; }
            protected internal set { _bounds = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonData]
        public bool EnableDrawing {
            get { return _enableDrawing; }
            set { _enableDrawing = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonData]
        public bool EnableEditing {
            get { return _enableEditing; }
            set { _enableEditing = value; }
        }

        /// <summary>
        /// Gets or sets the color of the fill.
        /// </summary>
        /// <value>The color of the fill.</value>
        [JsonData]
        public Color FillColor {
            get { return _fillColor; }
            set { _fillColor = value; }
        }

        /// <summary>
        /// Gets or sets the fill opacity.
        /// </summary>
        /// <value>The fill opacity.</value>
        [JsonData]
        public float FillOpacity {
            get { return _fillOpacity; }
            set { _fillOpacity = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is clickable.
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
        /// Gets or sets the points.
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
        /// Gets or sets the color of the stroke.
        /// </summary>
        /// <value>The color of the stroke.</value>
        [JsonData]
        public Color StrokeColor {
            get { return _strokeColor; }
            set { _strokeColor = value; }
        }

        /// <summary>
        /// Gets or sets the stroke opacity.
        /// </summary>
        /// <value>The stroke opacity.</value>
        [JsonData]
        public float StrokeOpacity {
            get { return _strokeOpacity; }
            set { _strokeOpacity = value; }
        }

        /// <summary>
        /// Gets or sets the stroke weight.
        /// </summary>
        /// <value>The stroke weight.</value>
        [JsonData]
        public int StrokeWeight {
            get { return _strokeWeight; }
            set { _strokeWeight = value; }
        }
        #endregion

        #region Methods /////////////////////////////////////////////////////////////////

        /// <summary>
        /// Toes the json string.
        /// </summary>
        /// <returns></returns>
        public virtual string ToJsonString() {
            return JsonSerializer<GooglePolygon>.Serialize(this);
        }

        #region - Actions -

        /// <summary>
        /// Fires the client-side hide() of the google polygon.
        /// </summary>
        public void Hide() {
            this.Actions.Add("{0}.Polygons[{1}].hide();");
        }

        /// <summary>
        /// Fires the client-side show() of the google polygon.
        /// </summary>
        public void Show() {
            this.Actions.Add("{0}.Polygons[{1}].show();");
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
                FillColor = (Color)state[0];
                FillOpacity = (float)state[1];
                IsClickable = (bool)state[2];
                Points.LoadViewState(state[3]);
                StrokeColor = (Color)state[4];
                StrokeOpacity = (float)state[5];
                StrokeWeight = (int)state[6];
                ((IStateManager)Bounds).LoadViewState(state[7]);
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
                ((IStateManager)Bounds).SaveViewState()
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