using System;
using System.Collections.Generic;
using System.Text;

namespace Artem.Web.UI.Controls {

    /// <summary>
    /// 
    /// </summary>
    public partial class GoogleMarkerEvents : IJsonObject {

        #region Fields  ///////////////////////////////////////////////////////////////////////////

        GoogleEventList _events;
        GoogleMap _map;

        #endregion

        #region Properties  ///////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <value>The events.</value>
        public GoogleEventList Events {
            get {
                if (_events == null) {
                    _events = new GoogleEventList(_map, GoogleEventType.Marker);
                }
                return _events;
            }
        }
        #endregion

        #region Client Events /////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Gets or sets the on client click.
        /// </summary>
        /// <value>The on client click.</value>
        public string OnClientClick {
            get {
                return Events.GetClientHandler(GoogleEventList.Click);
            }
            set {
                Events.AddClientHandler(GoogleEventList.Click, value);
            }
        }

        /// <summary>
        /// Gets or sets the on client double click.
        /// </summary>
        /// <value>The on client double click.</value>
        public string OnClientDoubleClick {
            get {
                return Events.GetClientHandler(GoogleEventList.DoubleClick);
            }
            set {
                Events.AddClientHandler(GoogleEventList.DoubleClick, value);
            }
        }

        /// <summary>
        /// Gets or sets the on client drag.
        /// </summary>
        /// <value>The on client drag.</value>
        public string OnClientDrag {
            get {
                return Events.GetClientHandler(GoogleEventList.Drag);
            }
            set {
                Events.AddClientHandler(GoogleEventList.Drag, value);
            }
        }

        /// <summary>
        /// Gets or sets the on client drag end.
        /// </summary>
        /// <value>The on client drag end.</value>
        public string OnClientDragEnd {
            get {
                return Events.GetClientHandler(GoogleEventList.DragEnd);
            }
            set {
                Events.AddClientHandler(GoogleEventList.DragEnd, value);
            }
        }

        /// <summary>
        /// Gets or sets the on client drag start.
        /// </summary>
        /// <value>The on client drag start.</value>
        public string OnClientDragStart {
            get {
                return Events.GetClientHandler(GoogleEventList.DragStart);
            }
            set {
                Events.AddClientHandler(GoogleEventList.DragStart, value);
            }
        }

        /// <summary>
        /// Gets or sets the on client geo location loaded.
        /// </summary>
        /// <value>The on client geo location loaded.</value>
        public string OnClientGeoLocationLoaded {
            get {
                return Events.GetClientHandler(GoogleEventList.GeoLocationLoaded);
            }
            set {
                Events.AddClientHandler(GoogleEventList.GeoLocationLoaded, value);
            }
        }

        /// <summary>
        /// Gets or sets the on client info window open.
        /// </summary>
        /// <value>The on client info window open.</value>
        public string OnClientInfoWindowOpen {
            get {
                return Events.GetClientHandler(GoogleEventList.InfoWindowOpen);
            }
            set {
                Events.AddClientHandler(GoogleEventList.InfoWindowOpen, value);
            }
        }

        /// <summary>
        /// Gets or sets the on client info window before close.
        /// </summary>
        /// <value>The on client info window before close.</value>
        public string OnClientInfoWindowBeforeClose {
            get {
                return Events.GetClientHandler(GoogleEventList.InfoWindowBeforeClose);
            }
            set {
                Events.AddClientHandler(GoogleEventList.InfoWindowBeforeClose, value);
            }
        }

        /// <summary>
        /// Gets or sets the on client info window close.
        /// </summary>
        /// <value>The on client info window close.</value>
        public string OnClientInfoWindowClose {
            get {
                return Events.GetClientHandler(GoogleEventList.InfoWindowClose);
            }
            set {
                Events.AddClientHandler(GoogleEventList.InfoWindowClose, value);
            }
        }

        /// <summary>
        /// Gets or sets the on client mouse down.
        /// </summary>
        /// <value>The on client mouse down.</value>
        public string OnClientMouseDown {
            get {
                return Events.GetClientHandler(GoogleEventList.MouseDown);
            }
            set {
                Events.AddClientHandler(GoogleEventList.MouseDown, value);
            }
        }

        /// <summary>
        /// Gets or sets the on client mouse up.
        /// </summary>
        /// <value>The on client mouse up.</value>
        public string OnClientMouseUp {
            get {
                return Events.GetClientHandler(GoogleEventList.MouseUp);
            }
            set {
                Events.AddClientHandler(GoogleEventList.MouseUp, value);
            }
        }

        /// <summary>
        /// Gets or sets the on client mouse over.
        /// </summary>
        /// <value>The on client mouse over.</value>
        public string OnClientMouseOver {
            get {
                return Events.GetClientHandler(GoogleEventList.MouseOver);
            }
            set {
                Events.AddClientHandler(GoogleEventList.MouseOver, value);
            }
        }

        /// <summary>
        /// Gets or sets the on client mouse out.
        /// </summary>
        /// <value>The on client mouse out.</value>
        public string OnClientMouseOut {
            get {
                return Events.GetClientHandler(GoogleEventList.MouseOut);
            }
            set {
                Events.AddClientHandler(GoogleEventList.MouseOut, value);
            }
        }

        /// <summary>
        /// Gets or sets the on client remove.
        /// </summary>
        /// <value>The on client remove.</value>
        public string OnClientRemove {
            get {
                return Events.GetClientHandler(GoogleEventList.Remove);
            }
            set {
                Events.AddClientHandler(GoogleEventList.Remove, value);
            }
        }

        /// <summary>
        /// Gets or sets the on client visibility changed.
        /// </summary>
        /// <value>The on client visibility changed.</value>
        public string OnClientVisibilityChanged {
            get {
                return Events.GetClientHandler(GoogleEventList.VisibilityChanged);
            }
            set {
                Events.AddClientHandler(GoogleEventList.VisibilityChanged, value);
            }
        }
        #endregion

        #region Server Events /////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Occurs when [click].
        /// </summary>
        public event EventHandler<GoogleLocationEventArgs> Click {
            add {
                Events.AddServerHandler<GoogleLocationEventArgs>(GoogleEventList.Click, value);
            }
            remove {
                Events.RemoveServerHandler<GoogleLocationEventArgs>(GoogleEventList.Click, value);
            }
        }

        /// <summary>
        /// Occurs when [double click].
        /// </summary>
        public event EventHandler<GoogleLocationEventArgs> DoubleClick {
            add {
                Events.AddServerHandler<GoogleLocationEventArgs>(GoogleEventList.DoubleClick, value);
            }
            remove {
                Events.RemoveServerHandler<GoogleLocationEventArgs>(GoogleEventList.DoubleClick, value);
            }
        }

        /// <summary>
        /// Occurs when [drag].
        /// </summary>
        public event EventHandler<GoogleBoundsEventArgs> Drag {
            add {
                Events.AddServerHandler<GoogleBoundsEventArgs>(GoogleEventList.Drag, value);
            }
            remove {
                Events.RemoveServerHandler<GoogleBoundsEventArgs>(GoogleEventList.Drag, value);
            }
        }

        /// <summary>
        /// Occurs when [drag end].
        /// </summary>
        public event EventHandler<GoogleBoundsEventArgs> DragEnd {
            add {
                Events.AddServerHandler<GoogleBoundsEventArgs>(GoogleEventList.DragEnd, value);
            }
            remove {
                Events.RemoveServerHandler<GoogleBoundsEventArgs>(GoogleEventList.DragEnd, value);
            }
        }

        /// <summary>
        /// Occurs when [drag start].
        /// </summary>
        public event EventHandler<GoogleBoundsEventArgs> DragStart {
            add {
                Events.AddServerHandler<GoogleBoundsEventArgs>(GoogleEventList.DragStart, value);
            }
            remove {
                Events.RemoveServerHandler<GoogleBoundsEventArgs>(GoogleEventList.DragStart, value);
            }
        }

        /// <summary>
        /// Occurs when [geo location loaded].
        /// </summary>
        public event EventHandler<GoogleEventArgs> GeoLocationLoaded {
            add {
                Events.AddServerHandler<GoogleEventArgs>(GoogleEventList.GeoLocationLoaded, value);
            }
            remove {
                Events.RemoveServerHandler<GoogleEventArgs>(GoogleEventList.GeoLocationLoaded, value);
            }
        }

        /// <summary>
        /// Occurs when [info window before close].
        /// </summary>
        public event EventHandler<GoogleEventArgs> InfoWindowBeforeClose {
            add {
                Events.AddServerHandler<GoogleEventArgs>(GoogleEventList.InfoWindowBeforeClose, value);
            }
            remove {
                Events.RemoveServerHandler<GoogleEventArgs>(GoogleEventList.InfoWindowBeforeClose, value);
            }
        }

        /// <summary>
        /// Occurs when [info window close].
        /// </summary>
        public event EventHandler<GoogleEventArgs> InfoWindowClose {
            add {
                Events.AddServerHandler<GoogleEventArgs>(GoogleEventList.InfoWindowClose, value);
            }
            remove {
                Events.RemoveServerHandler<GoogleEventArgs>(GoogleEventList.InfoWindowClose, value);
            }
        }

        /// <summary>
        /// Occurs when [info window open].
        /// </summary>
        public event EventHandler<GoogleEventArgs> InfoWindowOpen {
            add {
                Events.AddServerHandler<GoogleEventArgs>(GoogleEventList.InfoWindowOpen, value);
            }
            remove {
                Events.RemoveServerHandler<GoogleEventArgs>(GoogleEventList.InfoWindowOpen, value);
            }
        }

        /// <summary>
        /// Occurs when [mouse down].
        /// </summary>
        public event EventHandler<GoogleLocationEventArgs> MouseDown {
            add {
                Events.AddServerHandler<GoogleLocationEventArgs>(GoogleEventList.MouseDown, value);
            }
            remove {
                Events.RemoveServerHandler<GoogleLocationEventArgs>(GoogleEventList.MouseDown, value);
            }
        }

        /// <summary>
        /// Occurs when [mouse out].
        /// </summary>
        public event EventHandler<GoogleLocationEventArgs> MouseOut {
            add {
                Events.AddServerHandler<GoogleLocationEventArgs>(GoogleEventList.MouseOut, value);
            }
            remove {
                Events.RemoveServerHandler<GoogleLocationEventArgs>(GoogleEventList.MouseOut, value);
            }
        }

        /// <summary>
        /// Occurs when [mouse over].
        /// </summary>
        public event EventHandler<GoogleLocationEventArgs> MouseOver {
            add {
                Events.AddServerHandler<GoogleLocationEventArgs>(GoogleEventList.MouseOver, value);
            }
            remove {
                Events.RemoveServerHandler<GoogleLocationEventArgs>(GoogleEventList.MouseOver, value);
            }
        }

        /// <summary>
        /// Occurs when [mouse up].
        /// </summary>
        public event EventHandler<GoogleLocationEventArgs> MouseUp {
            add {
                Events.AddServerHandler<GoogleLocationEventArgs>(GoogleEventList.MouseUp, value);
            }
            remove {
                Events.RemoveServerHandler<GoogleLocationEventArgs>(GoogleEventList.MouseUp, value);
            }
        }

        /// <summary>
        /// Occurs when [remove].
        /// </summary>
        public event EventHandler<GoogleEventArgs> Remove {
            add {
                Events.AddServerHandler<GoogleEventArgs>(GoogleEventList.Remove, value);
            }
            remove {
                Events.RemoveServerHandler<GoogleEventArgs>(GoogleEventList.Remove, value);
            }
        }

        /// <summary>
        /// Occurs when [visibility changed].
        /// </summary>
        public event EventHandler<GoogleVisibilityEventArgs> VisibilityChanged {
            add {
                Events.AddServerHandler<GoogleVisibilityEventArgs>(GoogleEventList.VisibilityChanged, value);
            }
            remove {
                Events.RemoveServerHandler<GoogleVisibilityEventArgs>(GoogleEventList.VisibilityChanged, value);
            }
        }
        #endregion

        #region Construct /////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleMarkerEvents"/> class.
        /// </summary>
        /// <param name="map">The map.</param>
        public GoogleMarkerEvents(GoogleMap map) {
            _map = map;
        }
        #endregion

        #region Methods ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Raises the event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="key">The key.</param>
        /// <param name="args">The args.</param>
        public void RaiseEvent(object sender, string key, string args) {
            Events.RaiseEvent(sender, key, args);
        }

        /// <summary>
        /// Toes the json string.
        /// </summary>
        /// <returns></returns>
        public string ToJsonString() {
            return this.Events.ToJsonString();
        } 
        #endregion
    }
}
