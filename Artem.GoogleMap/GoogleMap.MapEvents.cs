using System;
using System.Collections.Generic;
using System.Text;

namespace Artem.Web.UI.Controls {

    /// <summary>
    /// 
    /// </summary>
    partial class GoogleMap {

        #region Client Events /////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Gets or sets the on client address not found.
        /// </summary>
        /// <value>The on client address not found.</value>
        public string OnClientAddressNotFound {
            get {
                return MapEvents.GetClientHandler(GoogleEventList.AddressNotFound);
            }
            set {
                MapEvents.AddClientHandler(GoogleEventList.AddressNotFound, value);
            }
        }

        /// <summary>
        /// Gets or sets the on client click.
        /// </summary>
        /// <value>The on client click.</value>
        public string OnClientClick {
            get {
                return MapEvents.GetClientHandler(GoogleEventList.Click);
            }
            set {
                MapEvents.AddClientHandler(GoogleEventList.Click, value);
            }
        }

        /// <summary>
        /// Gets or sets the on client double click.
        /// </summary>
        /// <value>The on client double click.</value>
        public string OnClientDoubleClick {
            get {
                return MapEvents.GetClientHandler(GoogleEventList.DoubleClick);
            }
            set {
                MapEvents.AddClientHandler(GoogleEventList.DoubleClick, value);
            }
        }

        /// <summary>
        /// Gets or sets the on client drag.
        /// </summary>
        /// <value>The on client drag.</value>
        public string OnClientDrag {
            get {
                return MapEvents.GetClientHandler(GoogleEventList.Drag);
            }
            set {
                MapEvents.AddClientHandler(GoogleEventList.Drag, value);
            }
        }

        /// <summary>
        /// Gets or sets the on client drag end.
        /// </summary>
        /// <value>The on client drag end.</value>
        public string OnClientDragEnd {
            get {
                return MapEvents.GetClientHandler(GoogleEventList.DragEnd);
            }
            set {
                MapEvents.AddClientHandler(GoogleEventList.DragEnd, value);
            }
        }

        /// <summary>
        /// Gets or sets the on client drag start.
        /// </summary>
        /// <value>The on client drag start.</value>
        public string OnClientDragStart {
            get {
                return MapEvents.GetClientHandler(GoogleEventList.DragStart);
            }
            set {
                MapEvents.AddClientHandler(GoogleEventList.DragStart, value);
            }
        }

        /// <summary>
        /// Gets or sets the on client geo location loaded.
        /// </summary>
        /// <value>The on client geo location loaded.</value>
        public string OnClientGeoLocationLoaded {
            get {
                return MapEvents.GetClientHandler(GoogleEventList.GeoLocationLoaded);
            }
            set {
                MapEvents.AddClientHandler(GoogleEventList.GeoLocationLoaded, value);
            }
        }

        /// <summary>
        /// Gets or sets the on client info window before close.
        /// </summary>
        /// <value>The on client info window before close.</value>
        public string OnClientInfoWindowBeforeClose {
            get {
                return MapEvents.GetClientHandler(GoogleEventList.InfoWindowBeforeClose);
            }
            set {
                MapEvents.AddClientHandler(GoogleEventList.InfoWindowBeforeClose, value);
            }
        }

        /// <summary>
        /// Gets or sets the on client info window close.
        /// </summary>
        /// <value>The on client info window close.</value>
        public string OnClientInfoWindowClose {
            get {
                return MapEvents.GetClientHandler(GoogleEventList.InfoWindowClose);
            }
            set {
                MapEvents.AddClientHandler(GoogleEventList.InfoWindowClose, value);
            }
        }

        /// <summary>
        /// Gets or sets the on client info window open.
        /// </summary>
        /// <value>The on client info window open.</value>
        public string OnClientInfoWindowOpen {
            get {
                return MapEvents.GetClientHandler(GoogleEventList.InfoWindowOpen);
            }
            set {
                MapEvents.AddClientHandler(GoogleEventList.InfoWindowOpen, value);
            }
        }

        /// <summary>
        /// Gets or sets the on client location loaded.
        /// </summary>
        /// <value>The on client location loaded.</value>
        public string OnClientLocationLoaded {
            get {
                return MapEvents.GetClientHandler(GoogleEventList.LocationLoaded);
            }
            set {
                MapEvents.AddClientHandler(GoogleEventList.LocationLoaded, value);
            }
        }

        /// <summary>
        /// Gets or sets the on client load.
        /// </summary>
        /// <value>The on client load.</value>
        public string OnClientMapLoad {
            get {
                return MapEvents.GetClientHandler(GoogleEventList.MapLoad);
            }
            set {
                MapEvents.AddClientHandler(GoogleEventList.MapLoad, value);
            }
        }

        /// <summary>
        /// Gets or sets the on client map type add.
        /// </summary>
        /// <value>The on client map type add.</value>
        public string OnClientMapTypeAdd {
            get {
                return MapEvents.GetClientHandler(GoogleEventList.MapTypeAdd);
            }
            set {
                MapEvents.AddClientHandler(GoogleEventList.MapTypeAdd, value);
            }
        }

        /// <summary>
        /// Gets or sets the on client map type changed.
        /// </summary>
        /// <value>The on client map type changed.</value>
        public string OnClientMapTypeChanged {
            get {
                return MapEvents.GetClientHandler(GoogleEventList.MapTypeChanged);
            }
            set {
                MapEvents.AddClientHandler(GoogleEventList.MapTypeChanged, value);
            }
        }

        /// <summary>
        /// Gets or sets the on client map type remove.
        /// </summary>
        /// <value>The on client map type remove.</value>
        public string OnClientMapTypeRemove {
            get {
                return MapEvents.GetClientHandler(GoogleEventList.MapTypeRemove);
            }
            set {
                MapEvents.AddClientHandler(GoogleEventList.MapTypeRemove, value);
            }
        }

        /// <summary>
        /// Gets or sets the on client mouse move.
        /// </summary>
        /// <value>The on client mouse move.</value>
        public string OnClientMouseMove {
            get {
                return MapEvents.GetClientHandler(GoogleEventList.MouseMove);
            }
            set {
                MapEvents.AddClientHandler(GoogleEventList.MouseMove, value);
            }
        }

        /// <summary>
        /// Gets or sets the on client mouse over.
        /// </summary>
        /// <value>The on client mouse over.</value>
        public string OnClientMouseOver {
            get {
                return MapEvents.GetClientHandler(GoogleEventList.MouseOver);
            }
            set {
                MapEvents.AddClientHandler(GoogleEventList.MouseOver, value);
            }
        }

        /// <summary>
        /// Gets or sets the on client mouse out.
        /// </summary>
        /// <value>The on client mouse out.</value>
        public string OnClientMouseOut {
            get {
                return MapEvents.GetClientHandler(GoogleEventList.MouseOut);
            }
            set {
                MapEvents.AddClientHandler(GoogleEventList.MouseOut, value);
            }
        }

        /// <summary>
        /// Gets or sets the on client move.
        /// </summary>
        /// <value>The on client move.</value>
        public string OnClientMove {
            get {
                return MapEvents.GetClientHandler(GoogleEventList.Move);
            }
            set {
                MapEvents.AddClientHandler(GoogleEventList.Move, value);
            }
        }

        /// <summary>
        /// Gets or sets the on client move end.
        /// </summary>
        /// <value>The on client move end.</value>
        public string OnClientMoveEnd {
            get {
                return MapEvents.GetClientHandler(GoogleEventList.MoveEnd);
            }
            set {
                MapEvents.AddClientHandler(GoogleEventList.MoveEnd, value);
            }
        }

        /// <summary>
        /// Gets or sets the on client move start.
        /// </summary>
        /// <value>The on client move start.</value>
        public string OnClientMoveStart {
            get {
                return MapEvents.GetClientHandler(GoogleEventList.MoveStart);
            }
            set {
                MapEvents.AddClientHandler(GoogleEventList.MoveStart, value);
            }
        }

        /// <summary>
        /// Gets or sets the on client overlay add.
        /// </summary>
        /// <value>The on client overlay add.</value>
        public string OnClientOverlayAdd {
            get {
                return MapEvents.GetClientHandler(GoogleEventList.OverlayAdd);
            }
            set {
                MapEvents.AddClientHandler(GoogleEventList.OverlayAdd, value);
            }
        }

        /// <summary>
        /// Gets or sets the on client overlay remove.
        /// </summary>
        /// <value>The on client overlay remove.</value>
        public string OnClientOverlayRemove {
            get {
                return MapEvents.GetClientHandler(GoogleEventList.OverlayRemove);
            }
            set {
                MapEvents.AddClientHandler(GoogleEventList.OverlayRemove, value);
            }
        }

        /// <summary>
        /// Gets or sets the on client overlays clear.
        /// </summary>
        /// <value>The on client overlays clear.</value>
        public string OnClientOverlaysClear {
            get {
                return MapEvents.GetClientHandler(GoogleEventList.OverlaysClear);
            }
            set {
                MapEvents.AddClientHandler(GoogleEventList.OverlaysClear, value);
            }
        }

        /// <summary>
        /// Gets or sets the on client single right click.
        /// </summary>
        /// <value>The on client single right click.</value>
        public string OnClientSingleRightClick {
            get {
                return MapEvents.GetClientHandler(GoogleEventList.SingleRightClick);
            }
            set {
                MapEvents.AddClientHandler(GoogleEventList.SingleRightClick, value);
            }
        }

        /// <summary>
        /// Gets or sets the on client zoom end.
        /// </summary>
        /// <value>The on client zoom end.</value>
        public string OnClientZoomEnd {
            get {
                return MapEvents.GetClientHandler(GoogleEventList.ZoomEnd);
            }
            set {
                MapEvents.AddClientHandler(GoogleEventList.ZoomEnd, value);
            }
        }
        #endregion

        #region Events ////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Occurs when [address not found].
        /// </summary>
        public event EventHandler<GoogleAddressEventArgs> AddressNotFound {
            add {
                MapEvents.AddServerHandler<GoogleAddressEventArgs>(GoogleEventList.AddressNotFound, value);
            }
            remove {
                MapEvents.RemoveServerHandler<GoogleAddressEventArgs>(GoogleEventList.AddressNotFound, value);
            }
        }

        /// <summary>
        /// Occurs when [click].
        /// </summary>
        public event EventHandler<GoogleLocationEventArgs> Click {
            add {
                MapEvents.AddServerHandler<GoogleLocationEventArgs>(GoogleEventList.Click, value);
            }
            remove {
                MapEvents.RemoveServerHandler<GoogleLocationEventArgs>(GoogleEventList.Click, value);
            }
        }

        /// <summary>
        /// Occurs when [double click].
        /// </summary>
        public event EventHandler<GoogleLocationEventArgs> DoubleClick {
            add {
                MapEvents.AddServerHandler<GoogleLocationEventArgs>(GoogleEventList.DoubleClick, value);
            }
            remove {
                MapEvents.RemoveServerHandler<GoogleLocationEventArgs>(GoogleEventList.DoubleClick, value);
            }
        }

        /// <summary>
        /// Occurs when [drag].
        /// </summary>
        public event EventHandler<GoogleBoundsEventArgs> Drag {
            add {
                MapEvents.AddServerHandler<GoogleBoundsEventArgs>(GoogleEventList.Drag, value);
            }
            remove {
                MapEvents.RemoveServerHandler<GoogleBoundsEventArgs>(GoogleEventList.Drag, value);
            }
        }

        /// <summary>
        /// Occurs when [drag end].
        /// </summary>
        public event EventHandler<GoogleBoundsEventArgs> DragEnd {
            add {
                MapEvents.AddServerHandler<GoogleBoundsEventArgs>(GoogleEventList.DragEnd, value);
            }
            remove {
                MapEvents.RemoveServerHandler<GoogleBoundsEventArgs>(GoogleEventList.DragEnd, value);
            }
        }

        /// <summary>
        /// Occurs when [drag start].
        /// </summary>
        public event EventHandler<GoogleBoundsEventArgs> DragStart {
            add {
                MapEvents.AddServerHandler<GoogleBoundsEventArgs>(GoogleEventList.DragStart, value);
            }
            remove {
                MapEvents.RemoveServerHandler<GoogleBoundsEventArgs>(GoogleEventList.DragStart, value);
            }
        }

        /// <summary>
        /// Occurs when [geo location loaded].
        /// </summary>
        public event EventHandler<GoogleEventArgs> GeoLocationLoaded {
            add {
                MapEvents.AddServerHandler<GoogleEventArgs>(GoogleEventList.GeoLocationLoaded, value);
            }
            remove {
                MapEvents.RemoveServerHandler<GoogleEventArgs>(GoogleEventList.GeoLocationLoaded, value);
            }
        }

        /// <summary>
        /// Occurs when [info window before close].
        /// </summary>
        public event EventHandler<GoogleEventArgs> InfoWindowBeforeClose {
            add {
                MapEvents.AddServerHandler<GoogleEventArgs>(GoogleEventList.InfoWindowBeforeClose, value);
            }
            remove {
                MapEvents.RemoveServerHandler<GoogleEventArgs>(GoogleEventList.InfoWindowBeforeClose, value);
            }
        }

        /// <summary>
        /// Occurs when [info window close].
        /// </summary>
        public event EventHandler<GoogleEventArgs> InfoWindowClose {
            add {
                MapEvents.AddServerHandler<GoogleEventArgs>(GoogleEventList.InfoWindowClose, value);
            }
            remove {
                MapEvents.RemoveServerHandler<GoogleEventArgs>(GoogleEventList.InfoWindowClose, value);
            }
        }

        /// <summary>
        /// Occurs when [info window open].
        /// </summary>
        public event EventHandler<GoogleEventArgs> InfoWindowOpen {
            add {
                MapEvents.AddServerHandler<GoogleEventArgs>(GoogleEventList.InfoWindowOpen, value);
            }
            remove {
                MapEvents.RemoveServerHandler<GoogleEventArgs>(GoogleEventList.InfoWindowOpen, value);
            }
        }

        /// <summary>
        /// Occurs when [location loaded].
        /// </summary>
        public event EventHandler<GoogleAddressEventArgs> LocationLoaded {
            add {
                MapEvents.AddServerHandler<GoogleAddressEventArgs>(GoogleEventList.LocationLoaded, value);
            }
            remove {
                MapEvents.RemoveServerHandler<GoogleAddressEventArgs>(GoogleEventList.LocationLoaded, value);
            }
        }

        /// <summary>
        /// Occurs when [map load].
        /// </summary>
        public event EventHandler<GoogleEventArgs> MapLoad {
            add {
                MapEvents.AddServerHandler<GoogleEventArgs>(GoogleEventList.MapLoad, value);
            }
            remove {
                MapEvents.RemoveServerHandler<GoogleEventArgs>(GoogleEventList.MapLoad, value);
            }
        }

        /// <summary>
        /// Occurs when [map type add].
        /// </summary>
        public event EventHandler<GoogleEventArgs> MapTypeAdd {
            add {
                MapEvents.AddServerHandler<GoogleEventArgs>(GoogleEventList.MapTypeAdd, value);
            }
            remove {
                MapEvents.RemoveServerHandler<GoogleEventArgs>(GoogleEventList.MapTypeAdd, value);
            }
        }

        /// <summary>
        /// Occurs when [map type changed].
        /// </summary>
        public event EventHandler<GoogleEventArgs> MapTypeChanged {
            add {
                MapEvents.AddServerHandler<GoogleEventArgs>(GoogleEventList.MapTypeChanged, value);
            }
            remove {
                MapEvents.RemoveServerHandler<GoogleEventArgs>(GoogleEventList.MapTypeChanged, value);
            }
        }

        /// <summary>
        /// Occurs when [map type remove].
        /// </summary>
        public event EventHandler<GoogleEventArgs> MapTypeRemove {
            add {
                MapEvents.AddServerHandler<GoogleEventArgs>(GoogleEventList.MapTypeRemove, value);
            }
            remove {
                MapEvents.RemoveServerHandler<GoogleEventArgs>(GoogleEventList.MapTypeRemove, value);
            }
        }

        /// <summary>
        /// Occurs when [mouse move].
        /// </summary>
        public event EventHandler<GoogleLocationEventArgs> MouseMove {
            add {
                MapEvents.AddServerHandler<GoogleLocationEventArgs>(GoogleEventList.MouseMove, value);
            }
            remove {
                MapEvents.RemoveServerHandler<GoogleLocationEventArgs>(GoogleEventList.MouseMove, value);
            }
        }

        /// <summary>
        /// Occurs when [mouse out].
        /// </summary>
        public event EventHandler<GoogleLocationEventArgs> MouseOut {
            add {
                MapEvents.AddServerHandler<GoogleLocationEventArgs>(GoogleEventList.MouseOut, value);
            }
            remove {
                MapEvents.RemoveServerHandler<GoogleLocationEventArgs>(GoogleEventList.MouseOut, value);
            }
        }

        /// <summary>
        /// Occurs when [mouse over].
        /// </summary>
        public event EventHandler<GoogleLocationEventArgs> MouseOver {
            add {
                MapEvents.AddServerHandler<GoogleLocationEventArgs>(GoogleEventList.MouseOver, value);
            }
            remove {
                MapEvents.RemoveServerHandler<GoogleLocationEventArgs>(GoogleEventList.MouseOver, value);
            }
        }

        /// <summary>
        /// Occurs when [move].
        /// </summary>
        public event EventHandler<GoogleEventArgs> Move {
            add {
                MapEvents.AddServerHandler<GoogleEventArgs>(GoogleEventList.Move, value);
            }
            remove {
                MapEvents.RemoveServerHandler<GoogleEventArgs>(GoogleEventList.Move, value);
            }
        }

        /// <summary>
        /// Occurs when [move end].
        /// </summary>
        public event EventHandler<GoogleEventArgs> MoveEnd {
            add {
                MapEvents.AddServerHandler<GoogleEventArgs>(GoogleEventList.MoveEnd, value);
            }
            remove {
                MapEvents.RemoveServerHandler<GoogleEventArgs>(GoogleEventList.MoveEnd, value);
            }
        }

        /// <summary>
        /// Occurs when [move start].
        /// </summary>
        public event EventHandler<GoogleEventArgs> MoveStart {
            add {
                MapEvents.AddServerHandler<GoogleEventArgs>(GoogleEventList.MoveStart, value);
            }
            remove {
                MapEvents.RemoveServerHandler<GoogleEventArgs>(GoogleEventList.MoveStart, value);
            }
        }

        /// <summary>
        /// Occurs when [overlay add].
        /// </summary>
        public event EventHandler<GoogleEventArgs> OverlayAdd {
            add {
                MapEvents.AddServerHandler<GoogleEventArgs>(GoogleEventList.OverlayAdd, value);
            }
            remove {
                MapEvents.RemoveServerHandler<GoogleEventArgs>(GoogleEventList.OverlayAdd, value);
            }
        }

        /// <summary>
        /// Occurs when [overlay remove].
        /// </summary>
        public event EventHandler<GoogleEventArgs> OverlayRemove {
            add {
                MapEvents.AddServerHandler<GoogleEventArgs>(GoogleEventList.OverlayRemove, value);
            }
            remove {
                MapEvents.RemoveServerHandler<GoogleEventArgs>(GoogleEventList.OverlayRemove, value);
            }
        }

        /// <summary>
        /// Occurs when [overlays clear].
        /// </summary>
        public event EventHandler<GoogleEventArgs> OverlaysClear {
            add {
                MapEvents.AddServerHandler<GoogleEventArgs>(GoogleEventList.OverlaysClear, value);
            }
            remove {
                MapEvents.RemoveServerHandler<GoogleEventArgs>(GoogleEventList.OverlaysClear, value);
            }
        }

        /// <summary>
        /// Occurs when [single right click].
        /// </summary>
        public event EventHandler<GoogleLocationEventArgs> SingleRightClick {
            add {
                MapEvents.AddServerHandler<GoogleLocationEventArgs>(GoogleEventList.SingleRightClick, value);
            }
            remove {
                MapEvents.RemoveServerHandler<GoogleLocationEventArgs>(GoogleEventList.SingleRightClick, value);
            }
        }

        /// <summary>
        /// Occurs when [zoom end].
        /// </summary>
        public event EventHandler<GoogleZoomEventArgs> ZoomEnd {
            add {
                MapEvents.AddServerHandler<GoogleZoomEventArgs>(GoogleEventList.ZoomEnd, value);
            }
            remove {
                MapEvents.RemoveServerHandler<GoogleZoomEventArgs>(GoogleEventList.ZoomEnd, value);
            }
        }
        #endregion
    }
}
