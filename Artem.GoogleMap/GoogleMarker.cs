using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.ComponentModel;
using System.Drawing;
using System.Web.UI.WebControls;

namespace Artem.Web.UI.Controls {

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public partial class GoogleMarker : IStateManager {

        #region Fields  /////////////////////////////////////////////////////////////////

        IList<string> _actions;
        string _address;
        EventHandlerList _events;
        double _latitude;
        double _longitude;
        double? _maxZoom;
        double? _minZoom;
        OpenInfoBehaviour _openInfoBehaviour;
        string _text;
        // options
        GoogleMarkerBehaviour _behaviour = GoogleMarkerBehaviour.DefaultSet;
        string _title;
        // icon
        GooglePoint _iconAnchor = GooglePoint.DefaultMarkerIconAnchor;
        GoogleSize _iconSize = GoogleSize.DefaultMarkerIconSize;
        string _iconUrl;
        InfoWindowContent _infoContent;
        GooglePoint _infoWindowAnchor;
        ITemplate _infoWindowTemplate;
        string _shadowUrl;
        GoogleSize _shadowSize = GoogleSize.DefaultMarkerShadowSize;


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
        /// Gets or sets the address of the marker.
        /// </summary>
        /// <value>The address.</value>
        [JsonData]
        public string Address {
            get { return _address; }
            set { _address = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to 'Auto-pan' the map 
        /// as you drag the marker near the edge.
        /// </summary>
        /// <value><c>true</c> if [auto pan]; otherwise, <c>false</c>.</value>
        [JsonData]
        public bool AutoPan {
            get { return (_behaviour & GoogleMarkerBehaviour.AutoPan) != 0; }
            set {
                _behaviour = value
                    ? (_behaviour | GoogleMarkerBehaviour.AutoPan)
                    : (_behaviour & ~GoogleMarkerBehaviour.AutoPan);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GoogleMarker"/> is bouncy.
        /// Toggles whether or not the marker should bounce up and down after it finishes dragging.
        /// </summary>
        /// <value><c>true</c> if bouncy; otherwise, <c>false</c>.</value>
        [JsonData]
        public bool Bouncy {
            get { return (_behaviour & GoogleMarkerBehaviour.Bouncy) != 0; }
            set {
                _behaviour = value
                    ? (_behaviour | GoogleMarkerBehaviour.Bouncy)
                    : (_behaviour & ~GoogleMarkerBehaviour.Bouncy);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GoogleMarker"/> is clickable.
        /// Toggles whether or not the marker is clickable. 
        /// Markers that are not clickable or draggable are inert, consume less resources 
        /// and do not respond to any events. 
        /// The default value for this option is true, i.e. if the option is not specified, 
        /// the marker will be clickable.
        /// </summary>
        /// <value><c>true</c> if clickable; otherwise, <c>false</c>.</value>
        [JsonData]
        public bool Clickable {
            get { return (_behaviour & GoogleMarkerBehaviour.Clickable) != 0; }
            set {
                _behaviour = value
                    ? (_behaviour | GoogleMarkerBehaviour.Clickable)
                    : (_behaviour & ~GoogleMarkerBehaviour.Clickable);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GoogleMarker"/> is draggable.
        /// Toggles whether or not the marker will be draggable by users. Markers set up to be 
        /// dragged require more resources to set up than markers that are clickable. Any marker 
        /// that is draggable is also clickable, bouncy and auto-pan enabled by default. 
        /// The default value for this option is false.
        /// </summary>
        /// <value><c>true</c> if draggable; otherwise, <c>false</c>.</value>
        [JsonData]
        public bool Draggable {
            get { return (_behaviour & GoogleMarkerBehaviour.Draggable) != 0; }
            set {
                _behaviour = value
                    ? (_behaviour | GoogleMarkerBehaviour.Draggable)
                    : (_behaviour & ~GoogleMarkerBehaviour.Draggable);
            }
        }

        /// <summary>
        /// When dragging markers normally, the marker floats up and away from the cursor. 
        /// Setting this value to true keeps the marker underneath the cursor, 
        /// and moves the cross downwards instead. The default value for this option is false.
        /// </summary>
        /// <value><c>true</c> if [drag cross move]; otherwise, <c>false</c>.</value>
        [JsonData]
        public bool DragCrossMove {
            get { return (_behaviour & GoogleMarkerBehaviour.DragCrossMove) != 0; }
            set {
                _behaviour = value
                    ? (_behaviour | GoogleMarkerBehaviour.DragCrossMove)
                    : (_behaviour & ~GoogleMarkerBehaviour.DragCrossMove);
            }
        }

        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <value>The events.</value>
        protected internal EventHandlerList Events {
            get {
                if (_events == null)
                    _events = new EventHandlerList();
                return _events;
            }
        }

        /// <summary>
        /// Gets or sets the icon anchor. The pixel coordinate relative to the 
        /// top left corner of the icon image at which this icon is anchored to the map.
        /// </summary>
        /// <value>The icon anchor.</value>
        [JsonData]
        public GooglePoint IconAnchor {
            get { return _iconAnchor; }
            set { _iconAnchor = value; }
        }

        /// <summary>
        /// Gets or sets the size of the icon. The pixel size of the foreground image of the icon.
        /// </summary>
        /// <value>The size of the icon.</value>
        [JsonData]
        public GoogleSize IconSize {
            get { return _iconSize; }
            set { _iconSize = value; }
        }

        /// <summary>
        /// Gets or sets the foreground image URL of the icon.
        /// </summary>
        /// <value>The image URL.</value>
        [JsonData]
        public string IconUrl {
            get { return _iconUrl; }
            set { _iconUrl = value; }
        }

        /// <summary>
        /// Gets the content of the info.
        /// </summary>
        /// <value>The content of the info.</value>
        public InfoWindowContent InfoContent {
            get { return _infoContent ?? (_infoContent = new InfoWindowContent()); }
            protected internal set { _infoContent = value; }
        }

        /// <summary>
        /// Gets or sets the info window anchor.
        /// The pixel coordinate relative to the top left corner of the icon image at 
        /// which this icon is anchored to the map.
        /// </summary>
        /// <value>The info window anchor.</value>
        [JsonData]
        public GooglePoint InfoWindowAnchor {
            get { return _infoWindowAnchor; }
            set { _infoWindowAnchor = value; }
        }

        /// <summary>
        /// Gets or sets the controls' template content of the info window.
        /// </summary>
        /// <value>The content of the info window.</value>
        [Browsable(false)]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        [TemplateInstance(TemplateInstance.Single)]
        [TemplateContainer(typeof(GoogleMap))]
        public ITemplate InfoWindowTemplate {
            get { return _infoWindowTemplate; }
            set { _infoWindowTemplate = value; }
        }

        /// <summary>
        /// Gets or sets the latitude of the marker.
        /// </summary>
        /// <value>The latitude.</value>
        [JsonData]
        public double Latitude {
            get { return _latitude; }
            set { _latitude = value; }
        }

        /// <summary>
        /// Gets or sets the longitude of the marker.
        /// </summary>
        /// <value>The longitude.</value>
        [JsonData]
        public double Longitude {
            get { return _longitude; }
            set { _longitude = value; }
        }

        /// <summary>
        /// Gets or sets the max zoom.
        /// </summary>
        /// <value>The max zoom.</value>
        [JsonData]
        public double? MaxZoom {
            get { return _maxZoom; }
            set { _maxZoom = value; }
        }

        /// <summary>
        /// Gets or sets the min zoom.
        /// </summary>
        /// <value>The min zoom.</value>
        [JsonData]
        public double? MinZoom {
            get { return _minZoom; }
            set { _minZoom = value; }
        }

        /// <summary>
        /// Gets or sets the behaviour for opening the info window of the marker - 
        /// on which mouse event the info window of the marker to be opened.
        /// Available values are: Click, DoubleClick, MouseDown, MouseOut, MouseOver, MouseUp.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [show info on mouse over]; otherwise, <c>false</c>.
        /// </value>
        [JsonData]
        public OpenInfoBehaviour OpenInfoBehaviour {
            get { return _openInfoBehaviour; }
            set { _openInfoBehaviour = value; }
        }

        /// <summary>
        /// Gets or sets the pixel size of the shadow image, if custom image is used for icon.
        /// </summary>
        /// <value>The size of the shadow.</value>
        [JsonData]
        public GoogleSize ShadowSize {
            get { return _shadowSize; }
            set { _shadowSize = value; }
        }

        /// <summary>
        /// Gets or sets the shadow image URL of the icon, if custom image is used for icon.
        /// </summary>
        /// <value>The shadow URL.</value>
        [JsonData]
        public string ShadowUrl {
            get { return _shadowUrl; }
            set { _shadowUrl = value; }
        }

        /// <summary>
        /// Gets or sets the simple text content for the marker's info window.
        /// </summary>
        /// <value>The text.</value>
        [JsonData(JsonDataType.ReadOnly)]
        public string Text {
            get { return _text; }
            set { _text = value; }
        }

        /// <summary>
        /// Gets or sets the title of the marker. 
        /// This string will appear as tooltip on the marker, i.e. it will work just 
        /// as the title attribute on HTML elements.
        /// </summary>
        /// <value>The title.</value>
        [JsonData]
        public string Title {
            get { return _title; }
            set { _title = value; }
        }
        #endregion

        #region Construct  //////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleMarker"/> class.
        /// </summary>
        public GoogleMarker()
            : this(0D, 0D) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleMarker"/> class.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <param name="text">The text.</param>
        public GoogleMarker(double latitude, double longitude) {
            _latitude = latitude;
            _longitude = longitude;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleMarker"/> class.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="text">The text.</param>
        public GoogleMarker(string address) {
            _address = address;
        }
        #endregion

        #region Methods /////////////////////////////////////////////////////////////////

        /// <summary>
        /// Toes the json string.
        /// </summary>
        /// <returns></returns>
        public string ToJsonString() {
            return JsonSerializer<GoogleMarker>.Serialize(this);
        }

        #region - Actions -

        /// <summary>
        /// Closes the info window only if it belongs to this marker.
        /// </summary>
        public void CloseInfoWindow() {
            this.Actions.Add("{0}.Markers[{1}].closeInfoWindow();");
        }

        /// <summary>
        /// Hides the marker if it is currently visible. 
        /// Note that this function triggers the event GMarker.visibilitychanged 
        /// in case the marker is currently visible.
        /// </summary>
        public void Hide() {
            this.Actions.Add("{0}.Markers[{1}].hide();");
        }

        /// <summary>
        /// Opens the map info window over the icon of the marker. 
        /// The content of the info window is given as a DOM node.
        /// </summary>
        /// <param name="domnode">The domnode.</param>
        public void OpenInfoWindow(string domnode) {
            this.Actions.Add("{0}.Markers[{1}].openInfoWindow('" + domnode + "');");
        }

        /// <summary>
        /// Opens the map info window over the icon of the marker. 
        /// The content of the info window is given as a string that contains HTML text.
        /// </summary>
        /// <param name="content">The content.</param>
        public void OpenInfoWindowHtml(string content) {
            this.Actions.Add("{0}.Markers[{1}].openInfoWindowHtml('" + content + "');");
        }

        /// <summary>
        /// Shows the marker if it is currently hidden. 
        /// Note that this function triggers the event GMarker.visibilitychanged 
        /// in case the marker is currently hidden.
        /// </summary>
        public void Show() {
            this.Actions.Add("{0}.Markers[{1}].show();");
        }
        #endregion

        #region - ITemplate -

        //void ITemplate.InstantiateIn(Control container) {
        //    throw new NotImplementedException();
        //}

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
                _address = (string)state[0];
                _behaviour = (GoogleMarkerBehaviour)Enum.Parse(typeof(GoogleMarkerBehaviour), (string)state[1]);
                ((IStateManager)_iconAnchor).LoadViewState(state[2]);
                ((IStateManager)_iconSize).LoadViewState(state[3]);
                _iconUrl = (string)state[4];
                ((IStateManager)_infoWindowAnchor).LoadViewState(state[5]);
                _latitude = (double)state[6];
                _longitude = (double)state[7];
                _openInfoBehaviour = (OpenInfoBehaviour)Enum.Parse(typeof(OpenInfoBehaviour), (string)state[8]);
                ((IStateManager)_shadowSize).LoadViewState(state[9]);
                _shadowUrl = (string)state[10];
                _text = (string)state[11];
                _title = (string)state[12];
                ((IStateManager)_infoContent).LoadViewState(state[13]);
            }
        }

        /// <summary>
        /// When implemented by a class, saves the changes to a server control's view state to an <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.Object"/> that contains the view state changes.
        /// </returns>
        object IStateManager.SaveViewState() {

            return new object[]{
                _address, 
                _behaviour,
                ((IStateManager)_iconAnchor).SaveViewState(), 
                ((IStateManager)_iconSize).SaveViewState(),
                _iconUrl,
                ((IStateManager)_infoWindowAnchor).SaveViewState(),
                _latitude,
                _longitude, 
                _openInfoBehaviour, 
                ((IStateManager)_shadowSize).SaveViewState(),
                _shadowUrl,
                _text,
                _title, 
                ((IStateManager)_infoContent).SaveViewState()
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