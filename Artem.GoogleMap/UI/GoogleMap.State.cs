using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Artem.Google.UI {

    /// <summary>
    /// 
    /// </summary>
    partial class GoogleMap : IPostBackDataHandler {

        #region Fields  ///////////////////////////////////////////////////////////////////////////

        JavaScriptSerializer _serializer;

        #endregion

        #region Properties  ///////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Gets the client state ID.
        /// </summary>
        /// <value>The client state ID.</value>
        protected internal string ClientStateID {
            get {
                return this.ClientID + "_ClientState";
            }
        }

        /// <summary>
        /// Gets the serializer.
        /// </summary>
        /// <value>The serializer.</value>
        protected internal JavaScriptSerializer Serializer {
            get {
                if (_serializer == null) {
                    _serializer = JsUtil.CreateSerializer();
                }
                return _serializer;
            }
        }
        #endregion

        #region Methods ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Loads the state of the google map.
        /// </summary>
        /// <param name="state">The state.</param>
        protected virtual void LoadClientState(string state) {

            var proxy = this.Serializer.Deserialize<ClientState>(state);
            proxy.Save(this);

            //List<GoogleDirection> dirs = new List<GoogleDirection>();
            //dirs.Add(new GoogleDirection { Locale = "Test", Bounds = new GoogleBounds { NorthEast = new GoogleLocation { Latitude = 1, Longitude = 2 } } });
            //string buffer = this.Serializer.Serialize(dirs);

            //List<GoogleDirection> result = this.Serializer.Deserialize<List<GoogleDirection>>(buffer);
        }

        /// <summary>
        /// Saves the state of the client.
        /// </summary>
        protected virtual string SaveClientState() {

            ClientState state = new ClientState(this);
            return this.Serializer.Serialize(state);
        }

        #region IPostBackDataHandler Members

        /// <summary>
        /// When implemented by a class, processes postback data for an ASP.NET server control.
        /// </summary>
        /// <param name="postDataKey">The key identifier for the control.</param>
        /// <param name="postCollection">The collection of all incoming name values.</param>
        /// <returns>
        /// true if the server control's state changes as a result of the postback; otherwise, false.
        /// </returns>
        bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection) {

            string state = postCollection[this.ClientStateID];
            if (!string.IsNullOrEmpty(state)) {
                this.LoadClientState(state);
            }
            return false;
        }

        /// <summary>
        /// When implemented by a class, signals the server control to notify the ASP.NET application that the state of the control has changed.
        /// </summary>
        void IPostBackDataHandler.RaisePostDataChangedEvent() {
        }
        #endregion
        #endregion

        #region Nested Types //////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// </summary>
        [DataContract]
        public class ClientState {

            #region Properties  ///////////////////////////////////////////////////////////////////////

            public string Address { get; set; }
            public LatLng Center { get; set; }
            public string Language { get; set; }
            public string Region { get; set; }
            public int Zoom { get; set; }

            public GoogleBounds Bounds { get; set; }
            public string DefaultAddress { get; set; }
            public bool DisableClear { get; set; }
            public bool DisableDefaultUI { get; set; }
            public bool DisableDoubleClickZoom { get; set; }
            public bool DisableKeyboardShortcuts { get; set; }
            public bool Draggable { get; set; }
            public string DraggableCursor { get; set; }
            public string DraggingCursor { get; set; }
            public bool EnableReverseGeocoding { get; set; }
            public bool EnableScrollWheelZoom { get; set; }
            public MapType MapType { get; set; }
            public MapTypeControlOptions MapTypeControlOptions { get; set; }
            public NavigationControlOptions NavigationControlOptions { get; set; }
            public ScaleControlOptions ScaleControlOptions { get; set; }
            public bool ShowMapTypeControl { get; set; }
            public bool ShowNavigationControl { get; set; }
            public bool ShowScaleControl { get; set; }
            public bool ShowStreetViewControl { get; set; }
            public StreetViewPanorama StreetView { get; set; }

            public List<GoogleMarker> Markers { get; set; }
            public List<GooglePolygon> Polygons { get; set; }
            public List<GooglePolyline> Polylines { get; set; }





            public List<GoogleDirections> Directions { get; set; }
            public bool EnableContinuousZoom { get; set; }
            
            public bool EnableGoogleBar { get; set; }
            public bool EnableInfoWindow { get; set; }
            public bool EnableMarkerManager { get; set; }
            public bool EnablePinchToZoom { get; set; }
            public string EnterpriseKey { get; set; }
            public double Height { get; set; }
            public bool IsStatic { get; set; }
            public MarkerManagerOptions MarkerManagerOptions { get; set; }
            //public GooglePolygonEvents PolygonEvents { get; set; }
            //public GooglePolylineEvents PolylineEvents { get; set; }
            
            public bool ShowTraffic { get; set; }
            public double Width { get; set; }

            #endregion

            #region Construct /////////////////////////////////////////////////////////////////////////

            /// <summary>
            /// Initializes a new instance of the <see cref="ClientState"/> class.
            /// </summary>
            /// <param name="map">The map.</param>
            public ClientState(GoogleMap map) {
                this.Load(map);
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ClientState"/> class.
            /// </summary>
            /// <param name="map">The map.</param>
            public ClientState() {
            }
            #endregion

            #region Methods ///////////////////////////////////////////////////////////////////////////

            public void Load(GoogleMap map) {
                this.Address = map.Address;
                this.Center = map.Center;
                this.Language = map.Language;
                this.Region = map.Region;
                this.Zoom = map.Zoom;

                this.Bounds = map.Bounds;
                this.DefaultAddress = map.DefaultAddress;
                this.DisableClear = map.DisableClear;
                this.DisableDefaultUI = map.DisableDefaultUI;
                this.DisableDoubleClickZoom = map.DisableDoubleClickZoom;
                this.DisableKeyboardShortcuts = map.DisableKeyboardShortcuts;
                this.Draggable = map.Draggable;
                this.DraggableCursor = map.DraggableCursor;
                this.DraggingCursor = map.DraggingCursor;
                this.EnableReverseGeocoding = map.EnableReverseGeocoding;
                this.EnableScrollWheelZoom = map.EnableScrollWheelZoom;
                this.MapType = map.MapType;
                this.MapTypeControlOptions = map.MapTypeControlOptions;
                this.NavigationControlOptions = map.NavigationControlOptions;
                this.ScaleControlOptions = map.ScaleControlOptions;
                this.ShowScaleControl = map.ShowScaleControl;
                this.ShowMapTypeControl = map.ShowMapTypeControl;
                this.ShowNavigationControl = map.ShowNavigationControl;
                this.ShowStreetViewControl = map.ShowStreetViewControl;
                this.StreetView = map.StreetView;

                this.Markers = map.Markers;
                this.Polygons = map.Polygons;
                this.Polylines = map.Polylines;




                
                
                
                this.Directions = map.Directions;
                this.EnableContinuousZoom = map.EnableContinuousZoom;
                this.EnableGoogleBar = map.EnableGoogleBar;
                this.EnableInfoWindow = map.EnableInfoWindow;
                this.EnableMarkerManager = map.EnableMarkerManager;
                this.EnablePinchToZoom = map.EnablePinchToZoom;
                this.EnableScrollWheelZoom = map.EnableScrollWheelZoom;
                this.EnterpriseKey = map.EnterpriseKey;
                this.Height = map.Height.Value;
                this.IsStatic = map.IsStatic;
                //this.Latitude = map.Latitude;
                //this.Longitude = map.Longitude;
                this.MarkerManagerOptions = map.MarkerManagerOptions;
                //this.PolygonEvents = map.PolygonEvents;
                //this.PolylineEvents = map.PolylineEvents;
                this.ShowTraffic = map.ShowTraffic;
                this.Width = map.Width.Value;
            }

            public void Save(GoogleMap map) {
                map.Address = this.Address;
                map.Center = this.Center;
                map.Language = this.Language;
                map.Region = this.Region;
                map.Zoom = this.Zoom;

                map.Bounds = this.Bounds;
                map.DefaultAddress = this.DefaultAddress;
                map.DisableClear = this.DisableClear;
                map.DisableDefaultUI = this.DisableDefaultUI;
                map.DisableDoubleClickZoom = this.DisableDoubleClickZoom;
                map.DisableKeyboardShortcuts = this.DisableKeyboardShortcuts;
                map.Draggable = this.Draggable;
                map.DraggableCursor = this.DraggableCursor;
                map.DraggingCursor = this.DraggingCursor;
                map.EnableReverseGeocoding = this.EnableReverseGeocoding;
                map.EnableScrollWheelZoom = this.EnableScrollWheelZoom;
                map.MapType = this.MapType;
                map.MapTypeControlOptions = this.MapTypeControlOptions;
                map.NavigationControlOptions = this.NavigationControlOptions;
                map.ScaleControlOptions = this.ScaleControlOptions;
                map.ShowMapTypeControl = this.ShowMapTypeControl;
                map.ShowNavigationControl = this.ShowNavigationControl;
                map.ShowScaleControl = this.ShowScaleControl;
                map.ShowStreetViewControl = this.ShowStreetViewControl;
                map.StreetView = this.StreetView;

                map.Markers = this.Markers;
                map.Polygons = this.Polygons;
                map.Polylines = this.Polylines;







                map.Directions = this.Directions;
                map.EnableContinuousZoom = this.EnableContinuousZoom;
                
                map.EnableGoogleBar = this.EnableGoogleBar;
                map.EnableInfoWindow = this.EnableInfoWindow;
                map.EnableMarkerManager = this.EnableMarkerManager;
                map.EnablePinchToZoom = this.EnablePinchToZoom;
                map.EnableScrollWheelZoom = this.EnableScrollWheelZoom;
                map.EnterpriseKey = this.EnterpriseKey;
                map.Height = new Unit(this.Height, UnitType.Pixel);
                map.IsStatic = this.IsStatic;
                map.MarkerManagerOptions = this.MarkerManagerOptions;
                //map.PolygonEvents = this.PolygonEvents;
                //map.PolylineEvents = this.PolylineEvents;
                map.ShowScaleControl = this.ShowScaleControl;
                map.ShowTraffic = this.ShowTraffic;
                map.Width = new Unit(this.Width, UnitType.Pixel);
            }
            #endregion
        }
        #endregion
    }
}
