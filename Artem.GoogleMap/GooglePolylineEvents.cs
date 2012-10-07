using System;
using System.Collections.Generic;
using System.Text;

namespace Artem.Web.UI.Controls {

    /// <summary>
    /// 
    /// </summary>
    public class GooglePolylineEvents : IJsonObject {

        #region Fields  ///////////////////////////////////////////////////////////////////////////

        GoogleEventList _events;
        GoogleMap _map;

        #endregion

        #region Properties ////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <value>The events.</value>
        public GoogleEventList Events {
            get {
                if (_events == null) {
                    _events = new GoogleEventList(_map, GoogleEventType.Polyline);
                }
                return _events;
            }
        }
        #endregion

        #region Client Events /////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Gets or sets the on client cancel line.
        /// </summary>
        /// <value>The on client cancel line.</value>
        public string OnClientCancelLine {
            get {
                return Events.GetClientHandler(GoogleEventList.CancelLine);
            }
            set {
                Events.AddClientHandler(GoogleEventList.CancelLine, value);
            }
        }

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
        /// Gets or sets the on client end line.
        /// </summary>
        /// <value>The on client end line.</value>
        public string OnClientEndLine {
            get {
                return Events.GetClientHandler(GoogleEventList.EndLine);
            }
            set {
                Events.AddClientHandler(GoogleEventList.EndLine, value);
            }
        }

        /// <summary>
        /// Gets or sets the on client line updated.
        /// </summary>
        /// <value>The on client line updated.</value>
        public string OnClientLineUpdated {
            get {
                return Events.GetClientHandler(GoogleEventList.LineUpdated);
            }
            set {
                Events.AddClientHandler(GoogleEventList.LineUpdated, value);
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
        /// Occurs when [cancel line].
        /// </summary>
        public event EventHandler<GoogleEventArgs> CancelLine {
            add {
                Events.AddServerHandler<GoogleEventArgs>(GoogleEventList.CancelLine, value);
            }
            remove {
                Events.RemoveServerHandler<GoogleEventArgs>(GoogleEventList.CancelLine, value);
            }
        }

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
        /// Occurs when [end line].
        /// </summary>
        public event EventHandler<GoogleEventArgs> EndLine {
            add {
                Events.AddServerHandler<GoogleEventArgs>(GoogleEventList.EndLine, value);
            }
            remove {
                Events.RemoveServerHandler<GoogleEventArgs>(GoogleEventList.EndLine, value);
            }
        }

        /// <summary>
        /// Occurs when [line updated].
        /// </summary>
        public event EventHandler<GoogleEventArgs> LineUpdated {
            add {
                Events.AddServerHandler<GoogleEventArgs>(GoogleEventList.LineUpdated, value);
            }
            remove {
                Events.RemoveServerHandler<GoogleEventArgs>(GoogleEventList.LineUpdated, value);
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
        public GooglePolylineEvents(GoogleMap map) {
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
