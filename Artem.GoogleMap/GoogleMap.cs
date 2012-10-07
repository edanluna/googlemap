using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Globalization;
using System.Security.Permissions;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Collections;

namespace Artem.Web.UI.Controls {

    /// <summary>
    /// 
    /// </summary>
    // CAS
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    //
    [ToolboxData("<{0}:GoogleMap runat=\"server\"> </{0}:GoogleMap>")]
    [Designer(typeof(GoogleMapDesigner))]
    //[DefaultBindingProperty("")]
    //[ComplexBindingProperties("")]
    public partial class GoogleMap : DataBoundControl, INamingContainer, IPostBackEventHandler {

        #region Static Fields /////////////////////////////////////////////////////////////////////

        public static Unit DefaultHeight = new Unit("480px");
        public static Unit DefaultWidth = new Unit("600px");
        public static int DefaultZoom = 4;

        #endregion

        #region Fields  ///////////////////////////////////////////////////////////////////////////

        IList<string> _actions;
        string _address;
        bool _allowBidirectionalLanguages;
        string _baseCountryCode;
        GoogleMapBehaviour _behaviour = GoogleMapBehaviour.DefaultSet;
        GoogleBounds _bounds;
        ClientScriptHelper _clientScriptProxy;
        string _dataAddressField;
        string _dataLatitudeField;
        string _dataLongitudeField;
        string _dataTextField;
        string _defaultAddress;
        GoogleMapView _defaultMapView;
        StateCollection<GoogleDirection> _directions;
        string _domainLanguage;
        bool _enableGoogleMapState = true;
        string _enterpriseKey;
        bool _insideUpdatePanel;
        bool _isSensor;
        bool _isStatic;
        bool _isStreetView;
        string _key;
        double _latitude;
        double _longitude;
        GoogleEventList _mapEvents;
        GoogleMarkerEvents _markerEvents;
        MarkerManagerOptions _markerManagerOptions;
        StateCollection<GoogleMarker> _markers;
        GoogleMarkerStyle _markerStyle;
        GooglePolygonEvents _polygonEvents;
        StateCollection<GooglePolygon> _polygons;
        GooglePolylineEvents _polylineEvents;
        StateCollection<GooglePolyline> _polylines;
        StreetViewMode _streetViewMode;
        string _streetViewPanoID;
        HtmlGenericControl _templateContainer;
        int _zoom = DefaultZoom;
        ZoomPanType _zoomPanType;

        #endregion

        #region Properties ////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Gets the actions recorder.
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
        /// Gets or sets the address.
        /// </summary>
        /// <value>The address.</value>
        [Category("Google")]
        //[Bindable(true, BindingDirection.OneWay)]
        [JsonData]
        public string Address {
            get { return _address; }
            set {
                if (_address != value) {
                    _address = value;
                    _latitude = _longitude = 0;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [allow bidirectional languages].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [allow bidirectional languages]; otherwise, <c>false</c>.
        /// </value>
        [Category("Google")]
        public bool AllowBidirectionalLanguages {
            get { return _allowBidirectionalLanguages; }
            set { _allowBidirectionalLanguages = value; }
        }

        /// <summary>
        /// Gets or sets the base country code.
        /// </summary>
        /// <value>The base country code.</value>
        [Category("Google")]
        [JsonData]
        public string BaseCountryCode {
            get { return _baseCountryCode; }
            set { _baseCountryCode = value; }
        }

        /// <summary>
        /// Gets or sets the bound.
        /// </summary>
        /// <value>The bound.</value>
        [Category("Google")]
        public GoogleBounds Bounds {
            get { return _bounds; }
            protected internal set { _bounds = value; }
        }

        /// <summary>
        /// Gets the server control identifier generated by ASP.NET.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The server control identifier generated by ASP.NET.
        /// </returns>
        [JsonData]
        public override string ClientID {
            get { return base.ClientID; }
        }

        /// <summary>
        /// Gets the server control identifier generated by ASP.NET.
        /// </summary>
        /// <value></value>
        /// <returns>The server control identifier generated by ASP.NET.</returns>
        [JsonData]
        public string ClientMapID {
            get { return this.ID; }
        }

        /// <summary>
        /// Gets or sets the client script proxy.
        /// </summary>
        /// <value>The client script proxy.</value>
        internal ClientScriptHelper ClientScriptProxy {
            get { return _clientScriptProxy; }
        }

        /// <summary>
        /// Gets the client state ID.
        /// </summary>
        /// <value>The client state ID.</value>
        protected internal string ClientStateID {
            get {
                return this.ClientID + "_State";
            }
        }

        /// <summary>
        /// Gets or sets the data address field.
        /// </summary>
        /// <value>The data address field.</value>
        [Category("Google")]
        public string DataAddressField {
            get { return _dataAddressField; }
            set { _dataAddressField = value; }
        }

        /// <summary>
        /// Gets or sets the data latitude field.
        /// </summary>
        /// <value>The data latitude field.</value>
        [Category("Google")]
        public string DataLatitudeField {
            get { return _dataLatitudeField; }
            set { _dataLatitudeField = value; }
        }

        /// <summary>
        /// Gets or sets the data longitude field.
        /// </summary>
        /// <value>The data longitude field.</value>
        [Category("Google")]
        public string DataLongitudeField {
            get { return _dataLongitudeField; }
            set { _dataLongitudeField = value; }
        }

        /// <summary>
        /// Gets or sets the data text field.
        /// </summary>
        /// <value>The data text field.</value>
        [Category("Google")]
        public string DataTextField {
            get { return _dataTextField; }
            set { _dataTextField = value; }
        }

        /// <summary>
        /// Gets or sets the default address.
        /// </summary>
        /// <value>The default address.</value>
        [Category("Google")]
        [JsonData]
        public string DefaultAddress {
            get { return _defaultAddress; }
            set { _defaultAddress = value; }
        }

        /// <summary>
        /// Gets or sets the default map view.
        /// </summary>
        /// <value>The default map view.</value>
        [Category("Google")]
        [JsonData]
        public GoogleMapView DefaultMapView {
            get { return _defaultMapView; }
            set { _defaultMapView = value; }
        }

        /// <summary>
        /// Gets the directions.
        /// </summary>
        /// <value>The directions.</value>
        [Browsable(true)]
        [Category("Google")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [JsonData]
        public StateCollection<GoogleDirection> Directions {
            get {
                if (_directions == null) {
                    _directions = new StateCollection<GoogleDirection>();
                    if (this.IsTrackingViewState) _directions.TrackViewState();
                }
                return _directions;
            }
            protected internal set { _directions = value; }
        }

        /// <summary>
        /// Gets or sets the domain language.
        /// </summary>
        /// <value>The domain language.</value>
        [Category("Google")]
        public string DomainLanguage {
            get { return _domainLanguage; }
            set { _domainLanguage = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable continuous zoom].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [enable continuous zoom]; otherwise, <c>false</c>.
        /// </value>
        [Category("Google")]
        [JsonData]
        public bool EnableContinuousZoom {
            get { return (_behaviour & GoogleMapBehaviour.ContinuousZoom) != 0; }
            set {
                _behaviour = value
                    ? (_behaviour | GoogleMapBehaviour.ContinuousZoom)
                    : (_behaviour & ~GoogleMapBehaviour.ContinuousZoom);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [double click zoom].
        /// </summary>
        /// <value><c>true</c> if [double click zoom]; otherwise, <c>false</c>.</value>
        [Category("Google")]
        [JsonData]
        public bool EnableDoubleClickZoom {
            get { return (_behaviour & GoogleMapBehaviour.DoubleClickZoom) != 0; }
            set {
                _behaviour = value
                    ? (_behaviour | GoogleMapBehaviour.DoubleClickZoom)
                    : (_behaviour & ~GoogleMapBehaviour.DoubleClickZoom);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable dragging].
        /// </summary>
        /// <value><c>true</c> if [enable dragging]; otherwise, <c>false</c>.</value>
        [Category("Google")]
        [JsonData]
        public bool EnableDragging {
            get { return (_behaviour & GoogleMapBehaviour.Dragging) != 0; }
            set {
                _behaviour = value
                    ? (_behaviour | GoogleMapBehaviour.Dragging)
                    : (_behaviour & ~GoogleMapBehaviour.Dragging);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable google bar].
        /// </summary>
        /// <value><c>true</c> if [enable google bar]; otherwise, <c>false</c>.</value>
        [Category("Google")]
        [JsonData]
        public bool EnableGoogleBar {
            get { return (_behaviour & GoogleMapBehaviour.GoogleBar) != 0; }
            set {
                _behaviour = value
                    ? (_behaviour | GoogleMapBehaviour.GoogleBar)
                    : (_behaviour & ~GoogleMapBehaviour.GoogleBar);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable google map state].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [enable google map state]; otherwise, <c>false</c>.
        /// </value>
        [Category("Google")]
        public bool EnableGoogleMapState {
            get { return _enableGoogleMapState; }
            set { _enableGoogleMapState = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable info window].
        /// </summary>
        /// <value><c>true</c> if [enable info window]; otherwise, <c>false</c>.</value>
        [Category("Google")]
        [JsonData]
        public bool EnableInfoWindow {
            get { return (_behaviour & GoogleMapBehaviour.InfoWindow) != 0; }
            set {
                _behaviour = value
                    ? (_behaviour | GoogleMapBehaviour.InfoWindow)
                    : (_behaviour & ~GoogleMapBehaviour.InfoWindow);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable marker manager].
        /// </summary>
        /// <value><c>true</c> if [enable marker manager]; otherwise, <c>false</c>.</value>
        [Category("Google")]
        [JsonData]
        public bool EnableMarkerManager {
            get { return (_behaviour & GoogleMapBehaviour.MarkerManager) != 0; }
            set {
                _behaviour = value
                    ? (_behaviour | GoogleMapBehaviour.MarkerManager)
                    : (_behaviour & ~GoogleMapBehaviour.MarkerManager);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable reverse geocoding].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [enable reverse geocoding]; otherwise, <c>false</c>.
        /// </value>
        [Category("Google")]
        [JsonData]
        public bool EnableReverseGeocoding {
            get { return (_behaviour & GoogleMapBehaviour.ReverseGeocoding) != 0; }
            set {
                _behaviour = value
                    ? (_behaviour | GoogleMapBehaviour.ReverseGeocoding)
                    : (_behaviour & ~GoogleMapBehaviour.ReverseGeocoding);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable scroll wheel zoom].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [enable scroll wheel zoom]; otherwise, <c>false</c>.
        /// </value>
        [Category("Google")]
        [JsonData]
        public bool EnableScrollWheelZoom {
            get { return (_behaviour & GoogleMapBehaviour.ScrollWheelZoom) != 0; }
            set {
                _behaviour = value
                    ? (_behaviour | GoogleMapBehaviour.ScrollWheelZoom)
                    : (_behaviour & ~GoogleMapBehaviour.ScrollWheelZoom);
            }
        }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        [Category("Google")]
        [DefaultValue("")]
        [Description("The Enterprise key obtained from google premier maps.")]
        [JsonData]
        public string EnterpriseKey {
            get {
                return (_enterpriseKey != null)
                    ? _enterpriseKey : WebConfigurationManager.AppSettings["GoogleMapEnterpriseKey"];
            }
            set { _enterpriseKey = value; }
        }


        /// <summary>
        /// Gets or sets the height of the Web server control.
        /// </summary>
        /// <value></value>
        /// <returns>A <see cref="T:System.Web.UI.WebControls.Unit"/> that represents the height of the control. The default is <see cref="F:System.Web.UI.WebControls.Unit.Empty"/>.</returns>
        /// <exception cref="T:System.ArgumentException">The height was set to a negative value.</exception>
        [JsonData]
        public override Unit Height {
            get { return (base.Height != Unit.Empty) ? base.Height : DefaultHeight; }
            set { base.Height = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [inside update panel].
        /// </summary>
        /// <value><c>true</c> if [inside update panel]; otherwise, <c>false</c>.</value>
        [Category("Google")]
        [Obsolete("This control property is no longer used!")]
        public bool InsideUpdatePanel {
            get { return _insideUpdatePanel; }
            set { _insideUpdatePanel = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is sensor.
        /// </summary>
        /// <value><c>true</c> if this instance is sensor; otherwise, <c>false</c>.</value>
        [Category("Google")]
        public bool IsSensor {
            get { return _isSensor; }
            set { _isSensor = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is street view.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is street view; otherwise, <c>false</c>.
        /// </value>
        [Category("Google")]
        [JsonData]
        public bool IsStreetView {
            get { return _isStreetView; }
            set { _isStreetView = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is static.
        /// </summary>
        /// <value><c>true</c> if this instance is static; otherwise, <c>false</c>.</value>
        [Category("Google")]
        [JsonData]
        public bool IsStatic {
            get { return _isStatic; }
            set { _isStatic = value; }
        }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        [Category("Google")]
        [DefaultValue("")]
        [Description("The user key obtained from google map api.")]
        [JsonData]
        public string Key {
            get {
                return (_key != null)
                    ? _key : WebConfigurationManager.AppSettings["GoogleMapKey"];
            }
            set { _key = value; }
        }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        [Category("Google")]
        //[Bindable(true, BindingDirection.OneWay)]
        [Description("")]
        [JsonData]
        public double Latitude {
            get { return _latitude; }
            set { _latitude = value; }
        }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        [Category("Google")]
        //[Bindable(true, BindingDirection.OneWay)]
        [Description("")]
        [JsonData]
        public double Longitude {
            get { return _longitude; }
            set { _longitude = value; }
        }

        /// <summary>
        /// Gets a list of event handler delegates for the control. This property is read-only.
        /// </summary>
        /// <value></value>
        /// <returns>The list of event handler delegates.</returns>
        public GoogleEventList MapEvents {
            get {
                if (_mapEvents == null) {
                    _mapEvents = new GoogleEventList(this, GoogleEventType.Map);
                }
                return _mapEvents;
            }
        }

        [Browsable(true)]
        [Category("Google")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public GoogleMarkerEvents MarkerEvents {
            get {
                if (_markerEvents == null)
                    _markerEvents = new GoogleMarkerEvents(this);
                return _markerEvents;
            }
            protected internal set {
                _markerEvents = value;
            }
        }

        /// <summary>
        /// Gets or sets the marker manager options.
        /// </summary>
        /// <value>The marker manager options.</value>
        [Browsable(true)]
        [Category("Google")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [JsonData]
        public MarkerManagerOptions MarkerManagerOptions {
            get {
                if (_markerManagerOptions == null)
                    _markerManagerOptions = new MarkerManagerOptions();
                return _markerManagerOptions;
            }
            protected internal set {
                _markerManagerOptions = value;
            }
        }

        /// <summary>
        /// Gets the markers.
        /// </summary>
        /// <value>The markers.</value>
        [Browsable(true)]
        [Category("Google")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public StateCollection<GoogleMarker> Markers {
            get {
                if (_markers == null) {
                    _markers = new StateCollection<GoogleMarker>();
                    if (this.IsTrackingViewState) _markers.TrackViewState();
                }
                return _markers;
            }
            protected internal set {
                _markers = value;
                if ((_markers != null) && (this.IsTrackingViewState)) {
                    _markers.TrackViewState();
                }
            }
        }

        /// <summary>
        /// Gets or sets the marker style.
        /// </summary>
        /// <value>The marker style.</value>
        [Browsable(true)]
        [Category("Google")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public GoogleMarkerStyle MarkerStyle {
            get {
                return _markerStyle;
            }
            set {
                if (_markerStyle != value) {
                    _markerStyle = value;
                    if (_markerStyle != null && this.IsTrackingViewState) {
                        ((IStateManager)_markerStyle).TrackViewState();
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the polygon events.
        /// </summary>
        /// <value>The polygon events.</value>
        [Browsable(true)]
        [Category("Google")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public GooglePolygonEvents PolygonEvents {
            get {
                if (_polygonEvents == null)
                    _polygonEvents = new GooglePolygonEvents(this);
                return _polygonEvents;
            }
            protected internal set {
                _polygonEvents = value;
            }
        }

        /// <summary>
        /// Gets or sets the polygons.
        /// </summary>
        /// <value>The polygons.</value>
        [Browsable(true)]
        [Category("Google")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public StateCollection<GooglePolygon> Polygons {
            get {
                if (_polygons == null) {
                    _polygons = new StateCollection<GooglePolygon>();
                    if (this.IsTrackingViewState) _polygons.TrackViewState();
                }
                return _polygons;
            }
            protected internal set {
                _polygons = value;
                if ((_polygons != null) && (this.IsTrackingViewState)) {
                    _polygons.TrackViewState();
                }
            }
        }

        /// <summary>
        /// Gets or sets the polyline events.
        /// </summary>
        /// <value>The polyline events.</value>
        [Browsable(true)]
        [Category("Google")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public GooglePolylineEvents PolylineEvents {
            get {
                if (_polylineEvents == null)
                    _polylineEvents = new GooglePolylineEvents(this);
                return _polylineEvents;
            }
            protected internal set {
                _polylineEvents = value;
            }
        }

        /// <summary>
        /// Gets or sets the polylines.
        /// </summary>
        /// <value>The polylines.</value>
        [Browsable(true)]
        [Category("Google")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public StateCollection<GooglePolyline> Polylines {
            get {
                if (_polylines == null) {
                    _polylines = new StateCollection<GooglePolyline>();
                    if (this.IsTrackingViewState) _polylines.TrackViewState();
                }
                return _polylines;
            }
            protected internal set {
                _polylines = value;
                if ((_polylines != null) && (this.IsTrackingViewState)) {
                    _polylines.TrackViewState();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show map type control].
        /// </summary>
        /// <value><c>true</c> if [show map type control]; otherwise, <c>false</c>.</value>
        [Category("Google")]
        [JsonData]
        public bool ShowMapTypeControl {
            get { return (_behaviour & GoogleMapBehaviour.MapTypeControl) != 0; }
            set {
                _behaviour = value
                    ? (_behaviour | GoogleMapBehaviour.MapTypeControl)
                    : (_behaviour & ~GoogleMapBehaviour.MapTypeControl);
            }
        }

        [Category("Google")]
        [JsonData]
        public bool ShowScaleControl {
            get { return (_behaviour & GoogleMapBehaviour.ScaleControl) != 0; }
            set {
                _behaviour = value
                    ? (_behaviour | GoogleMapBehaviour.ScaleControl)
                    : (_behaviour & ~GoogleMapBehaviour.ScaleControl);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show traffic].
        /// </summary>
        /// <value><c>true</c> if [show traffic]; otherwise, <c>false</c>.</value>
        [Category("Google")]
        [JsonData]
        public bool ShowTraffic {
            get { return (_behaviour & GoogleMapBehaviour.Traffic) != 0; }
            set {
                _behaviour = value
                    ? (_behaviour | GoogleMapBehaviour.Traffic)
                    : (_behaviour & ~GoogleMapBehaviour.Traffic);
            }
        }

        /// <summary>
        /// Gets or sets the street view mode.
        /// </summary>
        /// <value>The street view mode.</value>
        [Category("Google")]
        [JsonData]
        public StreetViewMode StreetViewMode {
            get { return _streetViewMode; }
            set { _streetViewMode = value; }
        }

        /// <summary>
        /// Gets or sets the street view pano.
        /// </summary>
        /// <value>The street view pano.</value>
        [Category("Google")]
        [JsonData]
        public string StreetViewPanoID {
            get { return _streetViewPanoID; }
            set { _streetViewPanoID = value; }
        }

        /// <summary>
        /// Gets the <see cref="T:System.Web.UI.HtmlTextWriterTag"/> value that corresponds to this Web server control. This property is used primarily by control developers.
        /// </summary>
        /// <value></value>
        /// <returns>One of the <see cref="T:System.Web.UI.HtmlTextWriterTag"/> enumeration values.</returns>
        protected override HtmlTextWriterTag TagKey {
            get { return HtmlTextWriterTag.Div; }
        }

        /// <summary>
        /// Gets or sets the width of the Web server control.
        /// </summary>
        /// <value></value>
        /// <returns>A <see cref="T:System.Web.UI.WebControls.Unit"/> that represents the width of the control. The default is <see cref="F:System.Web.UI.WebControls.Unit.Empty"/>.</returns>
        /// <exception cref="T:System.ArgumentException">The width of the Web server control was set to a negative value. </exception>
        [JsonData]
        public override Unit Width {
            get { return (base.Width != Unit.Empty) ? base.Width : DefaultWidth; }
            set { base.Width = value; }
        }

        /// <summary>
        /// Gets or sets the zoom.
        /// </summary>
        /// <value>The zoom.</value>
        [Category("Google")]
        [Bindable(true, BindingDirection.OneWay)]
        [JsonData]
        public int Zoom {
            get { return _zoom; }
            set { _zoom = value; }
        }

        /// <summary>
        /// Gets or sets the type of the zoom pan.
        /// </summary>
        /// <value>The type of the zoom pan.</value>
        [Category("Google")]
        [JsonData]
        public ZoomPanType ZoomPanType {
            get { return _zoomPanType; }
            set { _zoomPanType = value; }
        }
        #endregion

        #region Construct  ////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleMap"/> class.
        /// </summary>
        public GoogleMap() {
            _clientScriptProxy = new ClientScriptHelper(this);
        }
        #endregion

        #region Methods ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls() {
            base.CreateChildControls();

            _templateContainer = new HtmlGenericControl("div");
            _templateContainer.ID = "Templates";
            _templateContainer.Style[HtmlTextWriterStyle.Display] = "none";
            this.Controls.Add(_templateContainer);

            this.ClearChildViewState();
        }

        /// <summary>
        /// Loads the state of the google map.
        /// </summary>
        /// <param name="state">The state.</param>
        protected virtual void LoadGoogleMapState(string state) {

            if (!string.IsNullOrEmpty(state) && !JsUtil.IsUndefined(state)) {
                JsonSerializer<GoogleMap>.Deserialize(state, this);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e) {
            base.OnInit(e);

            // initialize from saved state
            if (this.EnableGoogleMapState && !this.DesignMode)
                LoadGoogleMapState(this.Page.Request[this.ClientStateID]);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e) {
            base.OnPreRender(e);

            //
            StringBuilder apiUrl = new StringBuilder("http://maps.google.com/maps?file=api&v=2.x");
            apiUrl.AppendFormat("&key={0}", this.Key);
            if (!string.IsNullOrEmpty(this.EnterpriseKey))
                apiUrl.AppendFormat("&client={0}", this.EnterpriseKey);
            if (this.IsSensor)
                apiUrl.Append("&sensor=true");
            if (!string.IsNullOrEmpty(this.DomainLanguage))
                apiUrl.AppendFormat("&hl={0}", this.DomainLanguage);
            if (this.AllowBidirectionalLanguages)
                apiUrl.Append("&allow_bidi=true");
            ClientScriptProxy.RegisterClientScriptInclude("maps.google.com", apiUrl.ToString());

#if DEBUG
            ClientScriptProxy.RegisterClientScriptResource("Artem.Web.UI.Controls.Scripts.GoogleMap-4.1.js");
            if (this.EnableMarkerManager)
                ClientScriptProxy.RegisterClientScriptResource("Artem.Web.UI.Controls.Scripts.markermanager.js");
#else
            ClientScriptProxy.RegisterClientScriptResource("Artem.Web.UI.Controls.Scripts.GoogleMap-4.1.min.js");
             if (this.EnableMarkerManager)
                ClientScriptProxy.RegisterClientScriptResource("Artem.Web.UI.Controls.Scripts.markermanager_packed.js");
#endif

            string key = "Artem.Web.GoogleMap:" + this.ClientMapID;
            ClientScriptProxy.RegisterStartupScript(key, RenderMapScript(), true);

            Page.ClientScript.RegisterOnSubmitStatement(GetType(), this.ClientStateID, "Artem.Web.GoogleManager.save();");
        }

        /// <summary>
        /// Renders the contents of the control to the specified writer. This method is used primarily by control developers.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter"/> that represents the output stream to render HTML content on the client.</param>
        protected override void RenderContents(HtmlTextWriter writer) {
            // KEEP THIS EMPTY
        }

        /// <summary>
        /// Renders the HTML closing tag of the control into the specified writer. This method is used primarily by control developers.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter"/> that represents the output stream to render HTML content on the client.</param>
        public override void RenderEndTag(HtmlTextWriter writer) {
            base.RenderEndTag(writer);

            // must be outside the map div otherwise Google API script will remove it
            if (_templateContainer != null)
                _templateContainer.RenderControl(writer);
            // street view pano
            bool flag = this.IsStreetView &&
                        this.StreetViewMode == StreetViewMode.Overlay &&
                        string.IsNullOrEmpty(this.StreetViewPanoID);
            if (flag) {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + "_Pano");
                writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "500px");
                writer.AddStyleAttribute(HtmlTextWriterStyle.Height, "200px");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.RenderEndTag();
            }
            // state bag
            string stateBagId = this.ClientStateID;
            writer.AddAttribute(HtmlTextWriterAttribute.Id, stateBagId);
            writer.AddAttribute(HtmlTextWriterAttribute.Name, stateBagId);
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
        }

        /// <summary>
        /// Toes the json.
        /// </summary>
        /// <returns></returns>
        protected string ToJsonString() {

            string json = JsonSerializer<GoogleMap>.Serialize(this);
            string width = string.Format("\"{0}\"", this.Width.ToString());
            string height = string.Format("\"{0}\"", this.Height.ToString());
            json = Regex.Replace(json, @"(?<=""Width"":)(\{[^\}]*\})", width, JsUtil.DefaultRegexOptions);
            json = Regex.Replace(json, @"(?<=""Height"":)(\{[^\}]*\})", height, JsUtil.DefaultRegexOptions);
            return json;
        }

        #region - Map Script -

        /// <summary>
        /// Renders the init script.
        /// </summary>
        /// <returns></returns>
        protected virtual string RenderMapScript() {

            string id = this.ClientMapID;
            string action;
            StringBuilder script = new StringBuilder();
            // construct
            script
                .AppendFormat("var {0} = new Artem.Web.GoogleMap({1});", id, this.ToJsonString())
                .AppendLine();
            // map's actions
            for (int i = 0; i < this.Actions.Count; i++) {
                action = this.Actions[i];
                script
                    .AppendFormat("{0}.addAction({1});", id, JsUtil.Encode(action))
                    .AppendLine();
            }
            // markers
            string config;
            GoogleMarker marker;
            for (int i = 0; i < this.Markers.Count; i++) {
                marker = this.Markers[i];
                if (_markerStyle != null) _markerStyle.ApplyOnMarker(marker);
                config = marker.ToJsonString();
                config = Regex.Replace(config, @"(""IconUrl"":"")([^""]*)""",
                    delegate(Match m) {
                        return string.Format("{0}{1}\"", m.Groups[1].Value, ResolveUrl(m.Groups[2].Value));
                    },
                    JsUtil.DefaultRegexOptions
                );
                config = Regex.Replace(config, @"(""ShadowUrl"":"")([^""]*)""",
                    delegate(Match m) {
                        return string.Format("{0}{1}\"", m.Groups[1].Value, ResolveUrl(m.Groups[2].Value));
                    },
                    JsUtil.DefaultRegexOptions
                );
                script
                    .AppendFormat("{0}.addMarker({1});", id, config)
                    .AppendLine();
                // info window content template
                if (marker.InfoContent.Controls.Count > 0) {
                    marker.InfoContent.ID = string.Format("Marker{0}Content", i.ToString());
                    _templateContainer.Controls.Add(marker.InfoContent);
                    script.AppendFormat("{0}.Markers[{1}].OpenWindowContent = '{2}';", id, i, marker.InfoContent.ClientID);
                }
                else if (marker.InfoWindowTemplate != null) {
                    GoogleMarker.TemplateContainer container = new GoogleMarker.TemplateContainer();
                    container.ID = string.Format("Marker{0}Content", i.ToString());
                    marker.InfoWindowTemplate.InstantiateIn(container);
                    _templateContainer.Controls.Add(container);
                    script.AppendFormat("{0}.Markers[{1}].OpenWindowContent = '{2}';", id, i, container.ClientID);
                }
                // marker's actions
                for (int j = 0; j < marker.Actions.Count; j++) {// string action in this.Markers[i].Actions) {
                    action = marker.Actions[j];
                    action = string.Format(action, id, i);
                    script
                        .AppendFormat("{0}.addAction({1});", id, JsUtil.Encode(action))
                        .AppendLine();
                }
            }
            // directions
            GoogleDirection dir;
            for (int i = 0; i < this.Directions.Count; i++) {
                dir = this.Directions[i];
                script
                    .AppendFormat("{0}.addDirection({1});", id, dir.ToJsonString())
                    .AppendLine();
                // direction's actions
                for (int j = 0; j < dir.Actions.Count; j++) {
                    action = dir.Actions[j];
                    action = string.Format(action, id, i);
                    script
                        .AppendFormat("{0}.addAction({1});", id, JsUtil.Encode(action))
                        .AppendLine();
                }
            }
            // polylines
            GooglePolyline line;
            for (int i = 0; i < this.Polylines.Count; i++) {
                line = this.Polylines[i];
                script
                    .AppendFormat("{0}.addPolyline({1});", id, line.ToJsonString())
                    .AppendLine();
                // polyline's actions
                for (int j = 0; j < line.Actions.Count; j++) {
                    action = line.Actions[j];
                    action = string.Format(action, id, i);
                    script
                        .AppendFormat("{0}.addAction({1});", id, JsUtil.Encode(action))
                        .AppendLine();
                }
            }
            // polygons
            GooglePolygon pg;
            for (int i = 0; i < this.Polygons.Count; i++) {
                pg = this.Polygons[i];
                script
                    .AppendFormat("{0}.addPolygon({1});", id, pg.ToJsonString())
                    .AppendLine();
                // polygon's actions
                for (int j = 0; j < pg.Actions.Count; j++) {
                    action = pg.Actions[j];
                    action = string.Format(action, id, i);
                    script
                        .AppendFormat("{0}.addAction({1});", id, JsUtil.Encode(action))
                        .AppendLine();
                }
            }
            // load
            script.AppendFormat("{0}.load();", id).AppendLine();
            return script.ToString();
        }
        #endregion

        #region - Data Bind -

        /// <summary>
        /// When overridden in a derived class, binds data from the data source to the control.
        /// </summary>
        /// <param name="data">The <see cref="T:System.Collections.IEnumerable"/> list of data returned from a <see cref="M:System.Web.UI.WebControls.DataBoundControl.PerformSelect"/> method call.</param>
        protected override void PerformDataBinding(IEnumerable data) {
            base.PerformDataBinding(data);

            if (data != null) {
                bool hasAddressDataField = !string.IsNullOrEmpty(DataAddressField);
                bool hasLatitudeDataField = !string.IsNullOrEmpty(DataLatitudeField);
                bool hasLongitudeDataField = !string.IsNullOrEmpty(DataLongitudeField);
                bool hasTextDataField = !string.IsNullOrEmpty(DataTextField);
                GoogleMarker marker;
                foreach (object dataItem in data) {
                    marker = new GoogleMarker();
                    if (hasAddressDataField)
                        marker.Address = DataBinder.Eval(dataItem, DataAddressField, "");
                    if (hasLatitudeDataField)
                        marker.Latitude = JsUtil.ToDouble(DataBinder.Eval(dataItem, DataLatitudeField, ""));
                    if (hasLongitudeDataField)
                        marker.Longitude = JsUtil.ToDouble(DataBinder.Eval(dataItem, DataLongitudeField, ""));
                    if (hasTextDataField)
                        marker.Text = DataBinder.Eval(dataItem, DataTextField, "");
                    this.Markers.Add(marker);
                }
            }
        }

        /// <summary>
        /// Retrieves data from the associated data source.
        /// </summary>
        protected override void PerformSelect() {

            if (IsBoundUsingDataSourceID)
                this.OnDataBinding(EventArgs.Empty);

            DataSourceView view = GetData();
            view.Select(
                CreateDataSourceSelectArguments(),
                this.OnDataSourceViewSelectCallback);
            // The PerformDataBinding method has completed.
            RequiresDataBinding = false;
            MarkAsDataBound();
            // Raise the DataBound event.
            OnDataBound(EventArgs.Empty);
        }

        /// <summary>
        /// Called when [data source view select callback].
        /// </summary>
        /// <param name="retrievedData">The retrieved data.</param>
        void OnDataSourceViewSelectCallback(IEnumerable retrievedData) {

            if (IsBoundUsingDataSourceID)
                OnDataBinding(EventArgs.Empty);
            PerformDataBinding(retrievedData);
        }
        #endregion

        #region - Actions -

        /// <summary>
        /// Opens a simple info window at the given point. 
        /// Pans the map such that the opened info window is fully visible. 
        /// The content of the info window is given as a DOM node.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="node">The node.</param>
        /// <param name="options">The options.</param>
        public void OpenInfoWindow(GoogleLocation point, string node, string options) {

            this.Actions.Add(
                string.Format("{0}.openInfoWindow(new GLatLng({1}, {2}), {3} {4});",
                    this.ClientMapID,
                    JsUtil.Encode(this.Latitude),
                    JsUtil.Encode(this.Longitude),
                    JsUtil.Encode(node),
                    string.IsNullOrEmpty(options) ? string.Empty : "," + JsUtil.Encode(options)
                )
            );
        }

        /// <summary>
        /// Opens a simple info window at the given point. 
        /// Pans the map such that the opened info window is fully visible. 
        /// The content of the info window is given as HTML text.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="text">The text.</param>
        /// <param name="options">The options.</param>
        public void OpenInfoWindowHtml(GoogleLocation point, string text, string options) {

            this.Actions.Add(
                string.Format("{0}.openInfoWindowHtml(new GLatLng({1}, {2}), {3} {4});",
                    this.ClientMapID,
                    JsUtil.Encode(this.Latitude),
                    JsUtil.Encode(this.Longitude),
                    JsUtil.Encode(text),
                    string.IsNullOrEmpty(options) ? string.Empty : "," + JsUtil.Encode(options)
                )
            );
        }

        /// <summary>
        /// Changes the center point of the map to the given point. 
        /// If the point is already visible in the current map view, change the center in a smooth animation.
        /// </summary>
        /// <param name="center">The center.</param>
        public void PanTo(GoogleLocation center) {

            this.Actions.Add(
                string.Format("{0}.panTo(new GLatLng({1}, {2}));",
                    this.ClientMapID,
                    JsUtil.Encode(this.Latitude),
                    JsUtil.Encode(this.Longitude)
                )
            );
        }

        /// <summary>
        /// Starts a pan animation by the given distance in pixels.
        /// </summary>
        /// <param name="distance">The distance.</param>
        public void PanBy(int distance) {
            this.Actions.Add(
                string.Format("{0}.panBy({1});", this.ClientMapID, distance));
        }

        /// <summary>
        /// Starts a pan animation by half the width of the map in the indicated directions. 
        /// +1 is right and down, -1 is left and up, respectively.
        /// </summary>
        /// <param name="dx">The dx.</param>
        /// <param name="dy">The dy.</param>
        public void PanDirection(int dx, int dy) {
            this.Actions.Add(
                string.Format("{0}.panDirection({1}, {2});", this.ClientMapID, dx, dy));
        }

        /// <summary>
        /// Increments zoom level by one.
        /// </summary>
        public void ZoomIn() {
            this.Actions.Add(string.Format("{0}.zoomIn();", this.ClientMapID));
        }

        /// <summary>
        /// Decrements zoom level by one.
        /// </summary>
        public void ZoomOut() {
            this.Actions.Add(string.Format("{0}.zoomOut();", this.ClientMapID));
        }
        #endregion

        #region - IPostBackEventHandler -

        /// <summary>
        /// When implemented by a class, enables a server control to process an event raised when a form is posted to the server.
        /// </summary>
        /// <param name="eventArgument">A <see cref="T:System.String"/> that represents an optional event argument to be passed to the event handler.</param>
        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument) {

            if (!string.IsNullOrEmpty(eventArgument)) {
                string[] parts = eventArgument.Split('$');
                if (parts.Length > 0) {
                    string[] typePair = parts[0].Split(':');
                    string type = typePair[0];
                    int index = 0;
                    if (typePair.Length >= 2) {
                        int.TryParse(typePair[1], out index);
                    }
                    string key = (parts.Length >= 2) ? parts[1] : null;
                    string args = (parts.Length >= 3) ? parts[2] : null;

                    switch (type) {
                        case "map_event":
                            MapEvents.RaiseEvent(this, key, args);
                            break;
                        case "marker_event":
                            MarkerEvents.RaiseEvent(this.Markers[index], key, args);
                            break;
                        case "polygon_event":
                            PolygonEvents.RaiseEvent(this.Polygons[index], key, args);
                            break;
                        case "polyline_event":
                            PolylineEvents.RaiseEvent(this.Polylines[index], key, args);
                            break;
                    }
                }
            }
        }
        #endregion
        #endregion
    }
}