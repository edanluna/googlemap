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
    //[ParseChildren(false)]
    public partial class GoogleMarker : IStateManager {

        #region Fields  /////////////////////////////////////////////////////////////////

        IList<string> _actions;
        string _address;
        EventHandlerList _events;
        EventList _googleEvents;
        double _latitude;
        double _longitude;
        OpenInfoBehaviour _openInfoBehaviour;
        string _text;
        // options
        GoogleMarkerBehaviour _behaviour = GoogleMarkerBehaviour.DefaultSet;
        string _title;
        // icon
        GooglePoint _iconAnchor = new GooglePoint(8, 16);
        GoogleSize _iconSize = new GoogleSize(16, 16);
        string _imageUrl;
        GooglePoint _infoWindowAnchor;
        string _shadowUrl;
        GoogleSize _shadowSize = new GoogleSize(16, 16);

        ITemplate _infoWindowContent;

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
        /// Gets the events.
        /// </summary>
        /// <value>The events.</value>
        //[JsonData]
        public EventList GoogleEvents {
            get {
                if (_googleEvents == null) {
                    _googleEvents = new EventList();
                    // TODO get map here
                    //if (( as IStateManager).IsTrackingViewState)
                    //    (_googleEvents as IStateManager).TrackViewState();
                }
                return _googleEvents;
            }
            //set { _googleEvents = value; }
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
        public string ImageUrl {
            get { return _imageUrl; }
            set { _imageUrl = value; }
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
        public ITemplate InfoWindowContent {
            get { return _infoWindowContent; }
            set { _infoWindowContent = value; }
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
        [JsonData]
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

        #region - Client Events -

        /// <summary>
        /// Gets or sets the on client click.
        /// </summary>
        /// <value>The on client click.</value>
        public string OnClientClick {
            get {
                return GoogleEvents.ClientEvents.ContainsKey(EventList.Click) ?
                    GoogleEvents.ClientEvents[EventList.Click] : null;
            }
            set { GoogleEvents.ClientEvents[EventList.Click] = value; }
        }

        /// <summary>
        /// Gets or sets the on client double click.
        /// </summary>
        /// <value>The on client double click.</value>
        public string OnClientDoubleClick {
            get {
                return GoogleEvents.ClientEvents.ContainsKey(EventList.DoubleClick)
                    ? GoogleEvents.ClientEvents[EventList.DoubleClick] : null;
            }
            set { GoogleEvents.ClientEvents[EventList.DoubleClick] = value; }
        }

        /// <summary>
        /// Gets or sets the on client drag.
        /// </summary>
        /// <value>The on client drag.</value>
        public string OnClientDrag {
            get {
                return GoogleEvents.ClientEvents.ContainsKey(EventList.Drag)
                    ? GoogleEvents.ClientEvents[EventList.Drag] : null;
            }
            set { GoogleEvents.ClientEvents[EventList.Drag] = value; }
        }

        /// <summary>
        /// Gets or sets the on client drag end.
        /// </summary>
        /// <value>The on client drag end.</value>
        public string OnClientDragEnd {
            get {
                return GoogleEvents.ClientEvents.ContainsKey(EventList.DragEnd)
                    ? GoogleEvents.ClientEvents[EventList.DragEnd] : null;
            }
            set { GoogleEvents.ClientEvents[EventList.DragEnd] = value; }
        }

        /// <summary>
        /// Gets or sets the on client drag start.
        /// </summary>
        /// <value>The on client drag start.</value>
        public string OnClientDragStart {
            get {
                return GoogleEvents.ClientEvents.ContainsKey(EventList.DragStart)
                    ? GoogleEvents.ClientEvents[EventList.DragStart] : null;
            }
            set { GoogleEvents.ClientEvents[EventList.DragStart] = value; }
        }

        /// <summary>
        /// Gets or sets the on client geo location loaded.
        /// </summary>
        /// <value>The on client geo location loaded.</value>
        public string OnClientGeoLocationLoaded {
            get {
                return GoogleEvents.ClientEvents.ContainsKey(EventList.GeoLocationLoaded)
                    ? GoogleEvents.ClientEvents[EventList.GeoLocationLoaded] : null;
            }
            set { GoogleEvents.ClientEvents[EventList.GeoLocationLoaded] = value; }
        }

        /// <summary>
        /// Gets or sets the on client info window open.
        /// </summary>
        /// <value>The on client info window open.</value>
        public string OnClientInfoWindowOpen {
            get {
                return GoogleEvents.ClientEvents.ContainsKey(EventList.InfoWindowOpen)
                    ? GoogleEvents.ClientEvents[EventList.InfoWindowOpen] : null;
            }
            set { GoogleEvents.ClientEvents[EventList.InfoWindowOpen] = value; }
        }

        /// <summary>
        /// Gets or sets the on client info window before close.
        /// </summary>
        /// <value>The on client info window before close.</value>
        public string OnClientInfoWindowBeforeClose {
            get {
                return GoogleEvents.ClientEvents.ContainsKey(EventList.InfoWindowBeforeClose)
                    ? GoogleEvents.ClientEvents[EventList.InfoWindowBeforeClose] : null;
            }
            set { GoogleEvents.ClientEvents[EventList.InfoWindowBeforeClose] = value; }
        }

        /// <summary>
        /// Gets or sets the on client info window close.
        /// </summary>
        /// <value>The on client info window close.</value>
        public string OnClientInfoWindowClose {
            get {
                return GoogleEvents.ClientEvents.ContainsKey(EventList.InfoWindowClose)
                    ? GoogleEvents.ClientEvents[EventList.InfoWindowClose] : null;
            }
            set { GoogleEvents.ClientEvents[EventList.InfoWindowClose] = value; }
        }

        /// <summary>
        /// Gets or sets the on client mouse down.
        /// </summary>
        /// <value>The on client mouse down.</value>
        public string OnClientMouseDown {
            get {
                return GoogleEvents.ClientEvents.ContainsKey(EventList.MouseDown)
                    ? GoogleEvents.ClientEvents[EventList.MouseDown] : null;
            }
            set { GoogleEvents.ClientEvents[EventList.MouseDown] = value; }
        }

        /// <summary>
        /// Gets or sets the on client mouse up.
        /// </summary>
        /// <value>The on client mouse up.</value>
        public string OnClientMouseUp {
            get {
                return GoogleEvents.ClientEvents.ContainsKey(EventList.MouseUp)
                    ? GoogleEvents.ClientEvents[EventList.MouseUp] : null;
            }
            set { GoogleEvents.ClientEvents[EventList.MouseUp] = value; }
        }

        /// <summary>
        /// Gets or sets the on client mouse over.
        /// </summary>
        /// <value>The on client mouse over.</value>
        public string OnClientMouseOver {
            get {
                return GoogleEvents.ClientEvents.ContainsKey(EventList.MouseOver)
                    ? GoogleEvents.ClientEvents[EventList.MouseOver] : null;
            }
            set { GoogleEvents.ClientEvents[EventList.MouseOver] = value; }
        }

        /// <summary>
        /// Gets or sets the on client mouse out.
        /// </summary>
        /// <value>The on client mouse out.</value>
        public string OnClientMouseOut {
            get {
                return GoogleEvents.ClientEvents.ContainsKey(EventList.MouseOut)
                    ? GoogleEvents.ClientEvents[EventList.MouseOut] : null;
            }
            set { GoogleEvents.ClientEvents[EventList.MouseOut] = value; }
        }

        /// <summary>
        /// Gets or sets the on client remove.
        /// </summary>
        /// <value>The on client remove.</value>
        public string OnClientRemove {
            get {
                return GoogleEvents.ClientEvents.ContainsKey(EventList.Remove)
                    ? GoogleEvents.ClientEvents[EventList.Remove] : null;
            }
            set { GoogleEvents.ClientEvents[EventList.Remove] = value; }
        }

        /// <summary>
        /// Gets or sets the on client visibility changed.
        /// </summary>
        /// <value>The on client visibility changed.</value>
        public string OnClientVisibilityChanged {
            get {
                return GoogleEvents.ClientEvents.ContainsKey(EventList.VisibilityChanged)
                    ? GoogleEvents.ClientEvents[EventList.VisibilityChanged] : null;
            }
            set { GoogleEvents.ClientEvents[EventList.VisibilityChanged] = value; }
        }
        #endregion
        #endregion

        #region Events //////////////////////////////////////////////////////////////////

        /// <summary>
        /// Occurs when [click].
        /// </summary>
        public event EventHandler Click {
            add {
                Events.AddHandler(EventList.Click, value);
                GoogleEvents.ServerEvents.Add(EventList.Click);
            }
            remove {
                Events.RemoveHandler(EventList.Click, value);
                GoogleEvents.ServerEvents.Remove(EventList.Click);
            }
        }

        /// <summary>
        /// Occurs when [double click].
        /// </summary>
        public event EventHandler DoubleClick {
            add {
                Events.AddHandler(EventList.DoubleClick, value);
                GoogleEvents.ServerEvents.Add(EventList.DoubleClick);
            }
            remove {
                Events.RemoveHandler(EventList.DoubleClick, value);
                GoogleEvents.ServerEvents.Remove(EventList.DoubleClick);
            }
        }

        /// <summary>
        /// Occurs when [drag].
        /// </summary>
        public event EventHandler Drag {
            add {
                Events.AddHandler(EventList.Drag, value);
                GoogleEvents.ServerEvents.Add(EventList.Drag);
            }
            remove {
                Events.RemoveHandler(EventList.Drag, value);
                GoogleEvents.ServerEvents.Remove(EventList.Drag);
            }
        }

        /// <summary>
        /// Occurs when [drag end].
        /// </summary>
        public event EventHandler DragEnd {
            add {
                Events.AddHandler(EventList.DragEnd, value);
                GoogleEvents.ServerEvents.Add(EventList.DragEnd);
            }
            remove {
                Events.RemoveHandler(EventList.DragEnd, value);
                GoogleEvents.ServerEvents.Remove(EventList.DragEnd);
            }
        }

        /// <summary>
        /// Occurs when [drag start].
        /// </summary>
        public event EventHandler DragStart {
            add {
                Events.AddHandler(EventList.DragStart, value);
                GoogleEvents.ServerEvents.Add(EventList.DragStart);
            }
            remove {
                Events.RemoveHandler(EventList.DragStart, value);
                GoogleEvents.ServerEvents.Remove(EventList.DragStart);
            }
        }

        /// <summary>
        /// Occurs when [geo location loaded].
        /// </summary>
        public event EventHandler GeoLocationLoaded {
            add {
                Events.AddHandler(EventList.GeoLocationLoaded, value);
                GoogleEvents.ServerEvents.Add(EventList.GeoLocationLoaded);
            }
            remove {
                Events.RemoveHandler(EventList.GeoLocationLoaded, value);
                GoogleEvents.ServerEvents.Remove(EventList.GeoLocationLoaded);
            }
        }

        /// <summary>
        /// Occurs when [info window before close].
        /// </summary>
        public event EventHandler InfoWindowBeforeClose {
            add {
                Events.AddHandler(EventList.InfoWindowBeforeClose, value);
                GoogleEvents.ServerEvents.Add(EventList.InfoWindowBeforeClose);
            }
            remove {
                Events.RemoveHandler(EventList.InfoWindowBeforeClose, value);
                GoogleEvents.ServerEvents.Remove(EventList.InfoWindowBeforeClose);
            }
        }

        /// <summary>
        /// Occurs when [info window close].
        /// </summary>
        public event EventHandler InfoWindowClose {
            add {
                Events.AddHandler(EventList.InfoWindowClose, value);
                GoogleEvents.ServerEvents.Add(EventList.InfoWindowClose);
            }
            remove {
                Events.RemoveHandler(EventList.InfoWindowClose, value);
                GoogleEvents.ServerEvents.Remove(EventList.InfoWindowClose);
            }
        }

        /// <summary>
        /// Occurs when [info window open].
        /// </summary>
        public event EventHandler InfoWindowOpen {
            add {
                Events.AddHandler(EventList.InfoWindowOpen, value);
                GoogleEvents.ServerEvents.Add(EventList.InfoWindowOpen);
            }
            remove {
                Events.RemoveHandler(EventList.InfoWindowOpen, value);
                GoogleEvents.ServerEvents.Remove(EventList.InfoWindowOpen);
            }
        }

        /// <summary>
        /// Occurs when [mouse down].
        /// </summary>
        public event EventHandler MouseDown {
            add {
                Events.AddHandler(EventList.MouseDown, value);
                GoogleEvents.ServerEvents.Add(EventList.MouseDown);
            }
            remove {
                Events.RemoveHandler(EventList.MouseDown, value);
                GoogleEvents.ServerEvents.Remove(EventList.MouseDown);
            }
        }

        /// <summary>
        /// Occurs when [mouse out].
        /// </summary>
        public event EventHandler MouseOut {
            add {
                Events.AddHandler(EventList.MouseOut, value);
                GoogleEvents.ServerEvents.Add(EventList.MouseOut);
            }
            remove {
                Events.RemoveHandler(EventList.MouseOut, value);
                GoogleEvents.ServerEvents.Remove(EventList.MouseOut);
            }
        }

        /// <summary>
        /// Occurs when [mouse over].
        /// </summary>
        public event EventHandler MouseOver {
            add {
                Events.AddHandler(EventList.MouseOver, value);
                GoogleEvents.ServerEvents.Add(EventList.MouseOver);
            }
            remove {
                Events.RemoveHandler(EventList.MouseOver, value);
                GoogleEvents.ServerEvents.Remove(EventList.MouseOver);
            }
        }

        /// <summary>
        /// Occurs when [mouse up].
        /// </summary>
        public event EventHandler MouseUp {
            add {
                Events.AddHandler(EventList.MouseUp, value);
                GoogleEvents.ServerEvents.Add(EventList.MouseUp);
            }
            remove {
                Events.RemoveHandler(EventList.MouseUp, value);
                GoogleEvents.ServerEvents.Remove(EventList.MouseUp);
            }
        }

        /// <summary>
        /// Occurs when [remove].
        /// </summary>
        public event EventHandler Remove {
            add {
                Events.AddHandler(EventList.Remove, value);
                GoogleEvents.ServerEvents.Add(EventList.Remove);
            }
            remove {
                Events.RemoveHandler(EventList.Remove, value);
                GoogleEvents.ServerEvents.Remove(EventList.Remove);
            }
        }

        /// <summary>
        /// Occurs when [visibility changed].
        /// </summary>
        public event EventHandler VisibilityChanged {
            add {
                Events.AddHandler(EventList.VisibilityChanged, value);
                GoogleEvents.ServerEvents.Add(EventList.VisibilityChanged);
            }
            remove {
                Events.RemoveHandler(EventList.VisibilityChanged, value);
                GoogleEvents.ServerEvents.Remove(EventList.VisibilityChanged);
            }
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

        #region - Event Handling -

        /// <summary>
        /// Raises the post back event.
        /// </summary>
        /// <param name="arg">The arg.</param>
        protected internal virtual void FireEvent(string key) {

            Delegate handler = null;
            switch (key) {
                case EventList.Click:
                    handler = Events[EventList.Click];
                    break;
                case EventList.DoubleClick:
                    handler = Events[EventList.DoubleClick];
                    break;
                case EventList.Drag:
                    handler = Events[EventList.Drag];
                    break;
                case EventList.DragEnd:
                    handler = Events[EventList.DragEnd];
                    break;
                case EventList.DragStart:
                    handler = Events[EventList.DragStart];
                    break;
                case EventList.GeoLocationLoaded:
                    handler = Events[EventList.GeoLocationLoaded];
                    break;
                case EventList.InfoWindowBeforeClose:
                    handler = Events[EventList.InfoWindowBeforeClose];
                    break;
                case EventList.InfoWindowClose:
                    handler = Events[EventList.InfoWindowClose];
                    break;
                case EventList.InfoWindowOpen:
                    handler = Events[EventList.InfoWindowOpen];
                    break;
                case EventList.MouseDown:
                    handler = Events[EventList.MouseDown];
                    break;
                case EventList.MouseOut:
                    handler = Events[EventList.MouseOut];
                    break;
                case EventList.MouseOver:
                    handler = Events[EventList.MouseOver];
                    break;
                case EventList.MouseUp:
                    handler = Events[EventList.MouseUp];
                    break;
                case EventList.Remove:
                    handler = Events[EventList.Remove];
                    break;
                case EventList.VisibilityChanged:
                    handler = Events[EventList.VisibilityChanged];
                    break;
            }
            if (handler != null) handler.DynamicInvoke(this, EventArgs.Empty);
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
                _imageUrl = (string)state[4];
                ((IStateManager)_infoWindowAnchor).LoadViewState(state[5]);
                _latitude = (double)state[6];
                _longitude = (double)state[7];
                _openInfoBehaviour = (OpenInfoBehaviour)Enum.Parse(typeof(OpenInfoBehaviour), (string)state[8]);
                ((IStateManager)_shadowSize).LoadViewState(state[9]);
                _shadowUrl = (string)state[10];
                _title = (string)state[11];
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
                _iconAnchor, 
                _iconSize,
                _imageUrl,
                _infoWindowAnchor,
                _latitude,
                _longitude, 
                _openInfoBehaviour, 
                _shadowSize,
                _shadowUrl,
                _title
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