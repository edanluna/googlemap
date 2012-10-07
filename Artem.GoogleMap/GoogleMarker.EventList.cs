using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web.UI;

namespace Artem.Web.UI.Controls {
    public partial class GoogleMarker {

        /// <summary>
        /// 
        /// </summary>
        public class EventList : IStateManager, IJsonObject {

            #region Static Fields ///////////////////////////////////////////////////////////

            public const string Click = "click";
            public const string DoubleClick = "dblclick";
            public const string Drag = "drag";
            public const string DragEnd = "dragend";
            public const string DragStart = "dragstart";
            public const string GeoLocationLoaded = "geoload";
            public const string InfoWindowBeforeClose = "infowindowbeforeclose";
            public const string InfoWindowClose = "infowindowclose";
            public const string InfoWindowOpen = "infowindowopen";
            public const string MouseDown = "mousedown";
            public const string MouseOut = "mouseout";
            public const string MouseOver = "mouseover";
            public const string MouseUp = "mouseup";
            public const string Remove = "remove";
            public const string VisibilityChanged = "visibilitychanged";

            #endregion

            #region Fields  /////////////////////////////////////////////////////////////////

            Dictionary<string, string> _clientEvents;
            List<string> _serverEvents;

            #endregion

            #region Properties  /////////////////////////////////////////////////////////////

            /// <summary>
            /// Gets the client events.
            /// </summary>
            /// <value>The client events.</value>
            public Dictionary<string, string> ClientEvents {
                get {
                    if (_clientEvents == null)
                        _clientEvents = new Dictionary<string, string>();
                    return _clientEvents;
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
            public List<string> ServerEvents {
                get {
                    if (_serverEvents == null)
                        _serverEvents = new List<string>();
                    return _serverEvents;
                }
            }
            #endregion

            #region Methods /////////////////////////////////////////////////////////////////

            /// <summary>
            /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </returns>
            public string ToJsonString() {

                StringBuilder buff = new StringBuilder();
                buff.Append("{");
                if (!IsEmpty) {
                    // client events
                    if (HasClientEvents) {
                        buff.Append("\"ClientEvents\":[");
                        if (_clientEvents != null) {
                            foreach (KeyValuePair<string, string> pair in _clientEvents)
                                buff.Append("{")
                                    .AppendFormat("\"Key\":{0},\"Handler\":{1}", JsUtil.Encode(pair.Key), JsUtil.Encode(pair.Value))
                                    .Append("},");
                            buff.Length--;
                        }
                        buff.Append("]");
                    }
                    // server side events
                    if (HasServerEvents) {
                        if (HasClientEvents) buff.Append(",");
                        buff.Append("\"ServerEvents\":[");
                        string command;
                        foreach (string key in _serverEvents) {
                            command = string.Format("marker_event$INDEX${0}", key);
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

            #region IStateManager Members //////////////////////////////////////////////////

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
}
