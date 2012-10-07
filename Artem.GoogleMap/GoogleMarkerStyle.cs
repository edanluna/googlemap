using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Artem.Web.UI.Controls {

    /// <summary>
    /// 
    /// </summary>
    public class GoogleMarkerStyle : IStateManager {

        #region Fields  ///////////////////////////////////////////////////////////////////////////

        bool _autoPan = true;
        bool _bouncy;
        bool _clickable = true;
        bool _draggable;
        bool _dragCrossMove;
        GooglePoint _iconAnchor = GooglePoint.DefaultMarkerIconAnchor;
        GoogleSize _iconSize = GoogleSize.DefaultMarkerIconSize;
        string _iconUrl;
        GooglePoint _infoWindowAnchor;
        OpenInfoBehaviour _openInfoBehaviour;
        GoogleSize _shadowSize = GoogleSize.DefaultMarkerShadowSize;
        string _shadowUrl;
        string _text;
        string _title;

        #endregion

        #region Properties  ///////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Gets or sets a value indicating whether [auto pan].
        /// </summary>
        /// <value><c>true</c> if [auto pan]; otherwise, <c>false</c>.</value>
        public bool AutoPan {
            get { return _autoPan; }
            set { _autoPan = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GoogleMarkerStyle"/> is bouncy.
        /// </summary>
        /// <value><c>true</c> if bouncy; otherwise, <c>false</c>.</value>
        public bool Bouncy {
            get { return _bouncy; }
            set { _bouncy = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GoogleMarkerStyle"/> is clickable.
        /// </summary>
        /// <value><c>true</c> if clickable; otherwise, <c>false</c>.</value>
        public bool Clickable {
            get { return _clickable; }
            set { _clickable = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GoogleMarkerStyle"/> is draggable.
        /// </summary>
        /// <value><c>true</c> if draggable; otherwise, <c>false</c>.</value>
        public bool Draggable {
            get { return _draggable; }
            set { _draggable = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [drag cross move].
        /// </summary>
        /// <value><c>true</c> if [drag cross move]; otherwise, <c>false</c>.</value>
        public bool DragCrossMove {
            get { return _dragCrossMove; }
            set { _dragCrossMove = value; }
        }

        /// <summary>
        /// Gets or sets the icon anchor.
        /// </summary>
        /// <value>The icon anchor.</value>
        public GooglePoint IconAnchor {
            get { return _iconAnchor; }
            set { _iconAnchor = value; }
        }

        /// <summary>
        /// Gets or sets the size of the icon.
        /// </summary>
        /// <value>The size of the icon.</value>
        public GoogleSize IconSize {
            get { return _iconSize; }
            set { _iconSize = value; }
        }

        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        /// <value>The image URL.</value>
        public string IconUrl {
            get { return _iconUrl; }
            set { _iconUrl = value; }
        }

        /// <summary>
        /// Gets or sets the info window anchor.
        /// </summary>
        /// <value>The info window anchor.</value>
        public GooglePoint InfoWindowAnchor {
            get { return _infoWindowAnchor; }
            set { _infoWindowAnchor = value; }
        }

        /// <summary>
        /// Gets or sets the open info behaviour.
        /// </summary>
        /// <value>The open info behaviour.</value>
        public OpenInfoBehaviour OpenInfoBehaviour {
            get { return _openInfoBehaviour; }
            set { _openInfoBehaviour = value; }
        }

        /// <summary>
        /// Gets or sets the size of the shadow.
        /// </summary>
        /// <value>The size of the shadow.</value>
        public GoogleSize ShadowSize {
            get { return _shadowSize; }
            set { _shadowSize = value; }
        }

        /// <summary>
        /// Gets or sets the shadow URL.
        /// </summary>
        /// <value>The shadow URL.</value>
        public string ShadowUrl {
            get { return _shadowUrl; }
            set { _shadowUrl = value; }
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text {
            get { return _text; }
            set { _text = value; }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title {
            get { return _title; }
            set { _title = value; }
        }
        #endregion

        #region Methods ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Applies the on marker.
        /// </summary>
        /// <param name="marker">The marker.</param>
        public virtual void ApplyOnMarker(GoogleMarker marker) {

            marker.AutoPan = this.AutoPan;
            marker.Bouncy = this.Bouncy;
            marker.Clickable = this.Clickable;
            marker.Draggable = this.Draggable;
            marker.DragCrossMove = this.DragCrossMove;
            if (marker.IconAnchor == GooglePoint.DefaultMarkerIconAnchor)
                marker.IconAnchor = this.IconAnchor;
            if (marker.IconSize == GoogleSize.DefaultMarkerIconSize)
                marker.IconSize = this.IconSize;
            if (string.IsNullOrEmpty(marker.IconUrl))
                marker.IconUrl = this.IconUrl;
            marker.InfoWindowAnchor = this.InfoWindowAnchor;
            if (marker.OpenInfoBehaviour == OpenInfoBehaviour.Click)
                marker.OpenInfoBehaviour = this.OpenInfoBehaviour;
            if (marker.ShadowSize == GoogleSize.DefaultMarkerShadowSize)
                marker.ShadowSize = this.ShadowSize;
            if (string.IsNullOrEmpty(marker.ShadowUrl))
                marker.ShadowUrl = this.ShadowUrl;
            if (string.IsNullOrEmpty(marker.Text))
                marker.Text = this.Text;
            if (string.IsNullOrEmpty(marker.Title))
                marker.Title = this.Title;
        }
        #endregion

        #region IStateManager /////////////////////////////////////////////////////////////////////

        bool _tracking;

        /// <summary>
        /// When implemented by a class, gets a value indicating whether a server control is tracking its view state changes.
        /// </summary>
        /// <value></value>
        /// <returns>true if a server control is tracking its view state changes; otherwise, false.
        /// </returns>
        bool IStateManager.IsTrackingViewState {
            get { return _tracking; }
        }

        /// <summary>
        /// When implemented by a class, loads the server control's previously saved view state to the control.
        /// </summary>
        /// <param name="state">An <see cref="T:System.Object"/> that contains the saved view state values for the control.</param>
        void IStateManager.LoadViewState(object savedState) {

            object[] state = savedState as object[];
            if (state != null) {
                _autoPan = (bool)state[0];
                _bouncy = (bool)state[1];
                _clickable = (bool)state[2];
                _draggable = (bool)state[3];
                _dragCrossMove = (bool)state[4];
                _openInfoBehaviour = (OpenInfoBehaviour)(int)state[5];
                _iconUrl = (string)state[6];
                _shadowUrl = (string)state[7];
                _text = (string)state[8];
                _title = (string)state[9];
                ((IStateManager)_iconAnchor).LoadViewState(state[10]);
                ((IStateManager)_infoWindowAnchor).LoadViewState(state[11]);
                ((IStateManager)_iconSize).LoadViewState(state[12]);
                ((IStateManager)_shadowSize).LoadViewState(state[13]);
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
                _autoPan,
                _bouncy,
                _clickable,
                _draggable,
                _dragCrossMove,
                (int)_openInfoBehaviour, //5
                _iconUrl,
                _shadowUrl,
                _text,
                _title,//11
                ((IStateManager)_iconAnchor).SaveViewState(),//12
                ((IStateManager)_infoWindowAnchor).SaveViewState(),//13
                ((IStateManager)_iconSize).SaveViewState(),// 14
                ((IStateManager)_shadowSize).SaveViewState() //15
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
