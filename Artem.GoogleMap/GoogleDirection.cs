using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace Artem.Web.UI.Controls {

    /// <summary>
    /// 
    /// </summary>
    public class GoogleDirection : IStateManager {

        #region Static Methods //////////////////////////////////////////////////////////

        /// <summary>
        /// Froms the json string.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns></returns>
        public static GoogleDirection FromJsonString(string jsonText) {
            return JsonSerializer<GoogleDirection>.Deserialize(jsonText);
        }
        #endregion

        #region Fields  /////////////////////////////////////////////////////////////////

        IList<string> _actions;
        GoogleBounds _bounds;
        GoogleDistance _distance;
        GoogleDuration _duration;
        string _locale;
        string _query;
        bool _preserveViewport;
        string _routePanelId;

        #endregion

        #region Properties  /////////////////////////////////////////////////////////////

        /// <summary>
        /// Gets or sets the actions of the polygon.
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
        /// Gets or sets the bounds of this <see cref="GoogleDirection"/>.
        /// It is used to get the bounding box for the result of this directions query.
        /// Have in mind it requires an additional post back after driving location are loaded.
        /// </summary>
        /// <value>The bounds.</value>
        public GoogleBounds Bounds {
            get { return _bounds; }
            protected internal set { _bounds = value; }
        }

        /// <summary>
        /// Gets or sets the distance of this <see cref="GoogleDirection"/>.
        /// </summary>
        /// <value>The distance.</value>
        public GoogleDistance Distance {
            get { return _distance; }
            protected internal set { _distance = value; }
        }

        /// <summary>
        /// Gets or sets the duration of this <see cref="GoogleDirection"/>.
        /// </summary>
        /// <value>The duration.</value>
        public GoogleDuration Duration {
            get { return _duration; }
            protected internal set { _duration = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GoogleDirection"/> is localized.
        /// The locale to use for the directions result. For example, "en_US", "fr", "fr_CA", etc.
        /// </summary>
        /// <value><c>true</c> if localized; otherwise, <c>false</c>.</value>
        [JsonData]
        public string Locale {
            get { return _locale; }
            set { _locale = value; }
        }

        /// <summary>
        /// Gets or sets the query of <see cref="GoogleDirection"/>.
        /// The query parameter is a string containing any valid directions query, e.g. 
        /// "from: Seattle to: San Francisco" or "from: Toronto to: Ottawa to: New York". 
        /// </summary>
        /// <value>The query.</value>
        [JsonData]
        public string Query {
            get { return _query; }
            set { _query = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GoogleDirection"/> should alter the viewport.
        /// When this option is set to true, the viewport is left alone for this request.
        /// </summary>
        /// <value><c>true</c> to preserve; otherwise, <c>false</c>.</value>
        [JsonData]
        public bool PreserveViewport {
            get { return _preserveViewport; }
            set { _preserveViewport = value; }
        }

        /// <summary>
        /// Gets or sets the route panel id.
        /// </summary>
        /// <value>The route panel id.</value>
        [JsonData]
        public string RoutePanelId {
            get { return _routePanelId; }
            set { _routePanelId = value; }
        }
        #endregion

        #region Construct  //////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleDirection"/> class.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="routePanelId">The route panel id.</param>
        /// <param name="locale">The locale.</param>
        /// <param name="preserveViewport">if set to <c>true</c> [preserve viewport].</param>
        public GoogleDirection(string query, string routePanelId, string locale, bool preserveViewport) {
            _query = query;
            _routePanelId = routePanelId;
            _locale = (locale != null) ? locale : "en_US";
            _preserveViewport = preserveViewport;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleDirection"/> class.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="routePanelId">The route panel id.</param>
        /// <param name="locale">The locale.</param>
        public GoogleDirection(string query, string routePanelId, string locale)
            : this(query, routePanelId, locale, false) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleDirection"/> class.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="routePanelId">The route panel id.</param>
        public GoogleDirection(string query, string routePanelId)
            : this(query, routePanelId, null, false) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleDirection"/> class.
        /// </summary>
        /// <param name="query">The query.</param>
        public GoogleDirection(string query)
            : this(query, null, null, false) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleDirection"/> class.
        /// </summary>
        public GoogleDirection()
            : this(null, null, null, false) {
        }
        #endregion

        #region Methods /////////////////////////////////////////////////////////////////

        /// <summary>
        /// Toes the json string.
        /// </summary>
        /// <returns></returns>
        public string ToJsonString() {
            return JsonSerializer<GoogleDirection>.Serialize(this);
        }

        #region - Actions -


        #endregion
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
        /// Loads the state of the view.
        /// </summary>
        /// <param name="savedState">State of the saved.</param>
        void IStateManager.LoadViewState(object savedState) {

            Pair state = savedState as Pair;
            if (state != null) {
                object[] firstNode = state.First as object[];
                if (firstNode != null) {
                    _query = firstNode[0] as string;
                    _routePanelId = firstNode[1] as string;
                    _locale = firstNode[2] as string;
                    _preserveViewport = (firstNode[3] as string == "true") ? true : false;
                }

                Triplet secondNode = state.Second as Triplet;
                if (secondNode != null) {
                    ((IStateManager)_bounds).LoadViewState(secondNode.First);
                    ((IStateManager)_distance).LoadViewState(secondNode.Second);
                    ((IStateManager)_duration).LoadViewState(secondNode.Third);
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
            return new Pair(
                new object[] { _query, _routePanelId, _locale, _preserveViewport },
                new Triplet(
                    ((IStateManager)_bounds).SaveViewState(),
                    ((IStateManager)_distance).SaveViewState(),
                    ((IStateManager)_duration).SaveViewState()));
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
