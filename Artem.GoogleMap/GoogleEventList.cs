using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.ComponentModel;

namespace Artem.Web.UI.Controls {

    /// <summary>
    /// 
    /// </summary>
    public class GoogleEventList : IStateManager, IJsonObject {

        #region Static Fields /////////////////////////////////////////////////////////////////////

        public const string AddressNotFound = "addressnotfound";
        public const string CancelLine = "cancelline";
        public const string Click = "click";
        public const string DoubleClick = "dblclick";
        public const string Drag = "drag";
        public const string DragEnd = "dragend";
        public const string DragStart = "dragstart";
        public const string EndLine = "endline";
        public const string GeoLocationLoaded = "geoload";
        public const string InfoWindowBeforeClose = "infowindowbeforeclose";
        public const string InfoWindowClose = "infowindowclose";
        public const string InfoWindowOpen = "infowindowopen";
        public const string LineUpdated = "lineupdated";
        public const string LocationLoaded = "locationloaded";
        public const string MapLoad = "load";
        public const string MapTypeAdd = "addmaptype";
        public const string MapTypeChanged = "maptypechanged";
        public const string MapTypeRemove = "removemaptype";
        public const string MouseDown = "mousedown";
        public const string MouseMove = "mousemove";
        public const string MouseOut = "mouseout";
        public const string MouseOver = "mouseover";
        public const string MouseUp = "mouseup";
        public const string Move = "move";
        public const string MoveEnd = "moveend";
        public const string MoveStart = "movestart";
        public const string OverlayAdd = "addoverlay";
        public const string OverlayRemove = "removeoverlay";
        public const string OverlaysClear = "clearoverlays";
        public const string Remove = "remove";
        public const string SingleRightClick = "singlerightclick";
        public const string VisibilityChanged = "visibilitychanged";
        public const string ZoomEnd = "zoomend";

        #endregion

        #region Fields  ///////////////////////////////////////////////////////////////////////////

        GoogleMap _map;
        GoogleEventType _eventType;
        EventHandlerList _events;
        Dictionary<string, string> _clientEvents;
        List<string> _serverEvents;

        #endregion

        #region Properties  ///////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Gets the client events.
        /// </summary>
        /// <value>The client events.</value>
        [JsonData]
        public Dictionary<string, string> ClientEvents {
            get {
                if (_clientEvents == null)
                    _clientEvents = new Dictionary<string, string>();
                return _clientEvents;
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
        /// Gets a value indicating whether this instance has client events.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has client events; otherwise, <c>false</c>.
        /// </value>
        public bool HasClientEvents {
            get { return _clientEvents != null && _clientEvents.Count > 0; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has server events.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has server events; otherwise, <c>false</c>.
        /// </value>
        public bool HasServerEvents {
            get { return _serverEvents != null && _serverEvents.Count > 0; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is empty.
        /// </summary>
        /// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
        public bool IsEmpty {
            get {
                return (_clientEvents == null || _clientEvents.Count == 0) &&
                    (_serverEvents == null || _serverEvents.Count == 0);
            }
        }

        /// <summary>
        /// Gets the server events.
        /// </summary>
        /// <value>The server events.</value>
        [JsonData]
        public List<string> ServerEvents {
            get {
                if (_serverEvents == null)
                    _serverEvents = new List<string>();
                return _serverEvents;
            }
        }
        #endregion

        #region Construct  ////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="EventList"/> class.
        /// </summary>
        /// <param name="map">The map.</param>
        public GoogleEventList(GoogleMap map, GoogleEventType type) {
            _map = map;
            _eventType = type;
        }
        #endregion

        #region Methods ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Adds the client handler.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="handler">The handler.</param>
        public void AddClientHandler(string key, string handler) {
            this.ClientEvents[key] = handler;
        }

        /// <summary>
        /// Gets the client handler.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string GetClientHandler(string key) {
            return this.ClientEvents.ContainsKey(key) ? this.ClientEvents[key] : null;
        }

        /// <summary>
        /// Adds the server handler.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="handler">The handler.</param>
        public void AddServerHandler(string key, EventHandler handler) {
            this.Events.AddHandler(key, handler);
            this.ServerEvents.Add(key);
        }

        /// <summary>
        /// Adds the server handler.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="handler">The handler.</param>
        public void AddServerHandler<T>(string key, EventHandler<T> handler) where T : EventArgs {
            this.Events.AddHandler(key, handler);
            this.ServerEvents.Add(key);
        }

        /// <summary>
        /// Removes the server handler.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="handler">The handler.</param>
        public void RemoveServerHandler(string key, EventHandler handler) {
            this.Events.RemoveHandler(key, handler);
            this.ServerEvents.Remove(key);
        }

        /// <summary>
        /// Removes the server handler.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="handler">The handler.</param>
        public void RemoveServerHandler<T>(string key, EventHandler<T> handler) where T : EventArgs {
            this.Events.RemoveHandler(key, handler);
            this.ServerEvents.Remove(key);
        }

        /// <summary>
        /// Fires the event.
        /// </summary>
        /// <param name="key">The key.</param>
        protected internal virtual void RaiseEvent(object sender, string key, string args) {

            Delegate handler = null;
            EventArgs e = GoogleEventArgs.Empty;
            switch (key) {
                case GoogleEventList.AddressNotFound:
                    handler = Events[GoogleEventList.AddressNotFound];
                    e = new GoogleAddressEventArgs(args);
                    break;
                case GoogleEventList.CancelLine:
                    handler = Events[GoogleEventList.CancelLine];
                    break;
                case GoogleEventList.Click:
                    handler = Events[GoogleEventList.Click];
                    e = new GoogleLocationEventArgs(args);
                    break;
                case GoogleEventList.DoubleClick:
                    handler = Events[GoogleEventList.DoubleClick];
                    e = new GoogleLocationEventArgs(args);
                    break;
                case GoogleEventList.Drag:
                    handler = Events[GoogleEventList.Drag];
                    e = new GoogleBoundsEventArgs(_map.Bounds);
                    break;
                case GoogleEventList.DragEnd:
                    handler = Events[GoogleEventList.DragEnd];
                    e = new GoogleBoundsEventArgs(_map.Bounds);
                    break;
                case GoogleEventList.DragStart:
                    handler = Events[GoogleEventList.DragStart];
                    e = new GoogleBoundsEventArgs(_map.Bounds);
                    break;
                case GoogleEventList.EndLine:
                    handler = Events[GoogleEventList.EndLine];
                    break;
                case GoogleEventList.GeoLocationLoaded:
                    handler = Events[GoogleEventList.GeoLocationLoaded];
                    break;
                case GoogleEventList.InfoWindowBeforeClose:
                    handler = Events[GoogleEventList.InfoWindowBeforeClose];
                    break;
                case GoogleEventList.InfoWindowClose:
                    handler = Events[GoogleEventList.InfoWindowClose];
                    break;
                case GoogleEventList.InfoWindowOpen:
                    handler = Events[GoogleEventList.InfoWindowOpen];
                    break;
                case GoogleEventList.LineUpdated:
                    handler = Events[GoogleEventList.LineUpdated];
                    break;
                case GoogleEventList.LocationLoaded:
                    handler = Events[GoogleEventList.LocationLoaded];
                    e = new GoogleAddressEventArgs(args);
                    break;
                case GoogleEventList.MapLoad:
                    handler = Events[GoogleEventList.MapLoad];
                    break;
                case GoogleEventList.MapTypeAdd:
                    handler = Events[GoogleEventList.MapTypeAdd];
                    break;
                case GoogleEventList.MapTypeChanged:
                    handler = Events[GoogleEventList.MapTypeChanged];
                    break;
                case GoogleEventList.MapTypeRemove:
                    handler = Events[GoogleEventList.MapTypeRemove];
                    break;
                case GoogleEventList.MouseDown:
                    handler = Events[GoogleEventList.MouseDown];
                    e = new GoogleLocationEventArgs(args);
                    break;
                case GoogleEventList.MouseMove:
                    handler = Events[GoogleEventList.MouseMove];
                    e = new GoogleLocationEventArgs(args);
                    break;
                case GoogleEventList.MouseOut:
                    handler = Events[GoogleEventList.MouseOut];
                    e = new GoogleLocationEventArgs(args);
                    break;
                case GoogleEventList.MouseOver:
                    handler = Events[GoogleEventList.MouseOver];
                    e = new GoogleLocationEventArgs(args);
                    break;
                case GoogleEventList.MouseUp:
                    handler = Events[GoogleEventList.MouseUp];
                    e = new GoogleLocationEventArgs(args);
                    break;
                case GoogleEventList.Move:
                    handler = Events[GoogleEventList.Move];
                    e = new GoogleEventArgs();
                    break;
                case GoogleEventList.MoveEnd:
                    handler = Events[GoogleEventList.MoveEnd];
                    e = new GoogleEventArgs();
                    break;
                case GoogleEventList.MoveStart:
                    handler = Events[GoogleEventList.MoveStart];
                    e = new GoogleEventArgs();
                    break;
                case GoogleEventList.OverlayAdd:
                    handler = Events[GoogleEventList.OverlayAdd];
                    break;
                case GoogleEventList.OverlayRemove:
                    handler = Events[GoogleEventList.OverlayRemove];
                    break;
                case GoogleEventList.OverlaysClear:
                    handler = Events[GoogleEventList.OverlaysClear];
                    break;
                case GoogleEventList.Remove:
                    handler = Events[GoogleEventList.Remove];
                    break;
                case GoogleEventList.SingleRightClick:
                    handler = Events[GoogleEventList.SingleRightClick];
                    e = new GoogleLocationEventArgs(args);
                    break;
                case GoogleEventList.ZoomEnd:
                    handler = Events[GoogleEventList.ZoomEnd];
                    e = new GoogleZoomEventArgs(args);
                    break;
                case GoogleEventList.VisibilityChanged:
                    handler = Events[GoogleEventList.VisibilityChanged];
                    e = new GoogleVisibilityEventArgs(args);
                    break;
            }

            if (handler != null) handler.DynamicInvoke(sender, e);
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public string ToJsonString() {

            string prefix = "map_event$";
            switch (_eventType) {
                case GoogleEventType.Marker:
                    prefix = "marker_event:INDEX$";
                    break;
                case GoogleEventType.Polygon:
                    prefix = "polygon_event:INDEX$";
                    break;
                case GoogleEventType.Polyline:
                    prefix = "polyline_event:INDEX$";
                    break;
            }
            StringBuilder buff = new StringBuilder();
            buff.Append("{");
            if (!IsEmpty) {
                // client events
                if (HasClientEvents) {
                    buff.Append("\"ClientEvents\":[");
                    foreach (KeyValuePair<string, string> pair in _clientEvents)
                        buff.Append("{")
                            .AppendFormat("\"Key\":{0},\"Handler\":{1}", JsUtil.Encode(pair.Key), JsUtil.Encode(pair.Value))
                            .Append("},");
                    buff.Length--;
                    buff.Append("]");
                }
                // server side events
                if (HasServerEvents) {
                    if (HasClientEvents) buff.Append(",");
                    buff.Append("\"ServerEvents\":[");
                    string command;
                    foreach (string key in _serverEvents) {
                        command = _map.Page.ClientScript.GetPostBackEventReference(
                            _map, string.Format("{0}{1}$ARGS", prefix, key));
                        buff.Append("{")
                            .AppendFormat("\"Key\":{0},\"Handler\":{1}", JsUtil.Encode(key), JsUtil.Encode(command))
                            .Append("},");
                    }
                    buff.Length--;
                    buff.Append("]");
                }
            }
            buff.Append("}");
            return buff.ToString();
        }
        #endregion

        #region IStateManager Members /////////////////////////////////////////////////////////////

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
        /// When implemented by a class, loads the server control's previously saved view state to the control.
        /// </summary>
        /// <param name="state">An <see cref="T:System.Object"/> that contains the saved view state values for the control.</param>
        void IStateManager.LoadViewState(object savedState) {

            Pair state = savedState as Pair;
            if (state != null) {
                string value = state.First as string;
                if (!string.IsNullOrEmpty(value)) {
                    string[] pair;
                    foreach (string item in value.Split(';')) {
                        pair = item.Split(':');
                        ClientEvents[pair[0]] = pair[1];
                    }
                }
                value = state.Second as string;
                if (!string.IsNullOrEmpty(value)) {
                    ServerEvents.AddRange(value.Split(';'));
                }
            }
        }

        /// <summary>
        /// When implemented by a class, saves the changes to a server control's view state to an <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.Object"/> that contains the view state changes.
        /// </returns>
        object IStateManager.SaveViewState() {

            string first = null;
            string second = null;
            if (_clientEvents != null) {
                List<string> list = new List<string>();
                foreach (string key in _clientEvents.Keys) {
                    list.Add(string.Format("{0}:{1}", key, _clientEvents[key]));
                }
                first = string.Join(";", list.ToArray());
            }
            if (_serverEvents != null)
                second = string.Join(";", _serverEvents.ToArray());
            return new Pair(first, second);
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