using System;
using System.Collections.Generic;
using System.Text;

namespace Artem.Web.UI.Controls {

    /// <summary>
    /// 
    /// </summary>
    public class GoogleEventArgs : EventArgs {

        //#region Fields  ///////////////////////////////////////////////////////////////////////////

        //T _value;

        //#endregion

        //#region Properties  ///////////////////////////////////////////////////////////////////////

        ///// <summary>
        ///// Gets or sets the address.
        ///// </summary>
        ///// <value>The address.</value>
        //public T Value {
        //    get { return _value; }
        //    protected internal set { _value = value; }
        //}
        //#endregion

        //#region Construct /////////////////////////////////////////////////////////////////////////

        ///// <summary>
        ///// Initializes a new instance of the <see cref="AddressNotFoundEventArgs"/> class.
        ///// </summary>
        ///// <param name="address">The address.</param>
        //public GoogleEventArgs(T value) {
        //    _value = value;
        //}
        //#endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class GoogleAddressEventArgs : GoogleEventArgs {

        #region Fields  ///////////////////////////////////////////////////////////////////////////

        string _address;

        #endregion

        #region Properties  ///////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>The address.</value>
        public string Address {
            get { return _address; }
            protected internal set { _address = value; }
        }
        #endregion

        #region Construct /////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleAddressEventArgs"/> class.
        /// </summary>
        /// <param name="address">The address.</param>
        public GoogleAddressEventArgs(string address) {
            this.Address = address;
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class GoogleBoundsEventArgs : GoogleEventArgs {

        #region Fields  ///////////////////////////////////////////////////////////////////////////

        GoogleBounds _bounds;

        #endregion

        #region Properties  ///////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Gets or sets the bounds.
        /// </summary>
        /// <value>The bounds.</value>
        public GoogleBounds Bounds {
            get { return _bounds; }
            protected internal set { _bounds = value; }
        }
        #endregion

        #region Construct /////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleBoundsEventArgs"/> class.
        /// </summary>
        /// <param name="bounds">The bounds.</param>
        public GoogleBoundsEventArgs(GoogleBounds bounds) {
            this.Bounds = bounds;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleBoundsEventArgs"/> class.
        /// </summary>
        /// <param name="args">The args.</param>
        public GoogleBoundsEventArgs(string args) {
            throw new NotImplementedException();
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class GoogleLocationEventArgs : GoogleEventArgs {

        #region Fields  ///////////////////////////////////////////////////////////////////////////

        GoogleLocation _location;

        #endregion

        #region Properties  ///////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        public GoogleLocation Location {
            get { return _location; }
            protected internal set { _location = value; }
        }
        #endregion

        #region Construct /////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleLocationEventArgs"/> class.
        /// </summary>
        /// <param name="location">The location.</param>
        public GoogleLocationEventArgs(GoogleLocation location) {
            this.Location = location;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleLocationEventArgs"/> class.
        /// </summary>
        /// <param name="location">The location.</param>
        public GoogleLocationEventArgs(string location)
            : this(GoogleLocation.Parse(location)) {
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class GoogleVisibilityEventArgs : GoogleEventArgs {

        #region Fields  ///////////////////////////////////////////////////////////////////////////

        bool _visible;

        #endregion

        #region Properties  ///////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GoogleVisibilityEventArgs"/> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible {
            get { return _visible; }
            protected internal set { _visible = value; }
        }
        #endregion

        #region Construct /////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleVisibilityEventArgs"/> class.
        /// </summary>
        /// <param name="visible">if set to <c>true</c> [visible].</param>
        public GoogleVisibilityEventArgs(bool visible) {
            this.Visible = visible;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleVisibilityEventArgs"/> class.
        /// </summary>
        /// <param name="args">The args.</param>
        public GoogleVisibilityEventArgs(string args) {

            bool flag = false;
            if (bool.TryParse(args, out flag))
                this.Visible = flag;
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class GoogleZoomEventArgs : GoogleEventArgs {

        #region Fields  ///////////////////////////////////////////////////////////////////////////

        double _newLevel;
        double _oldLevel;

        #endregion

        #region Properties  ///////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Gets or sets the new level.
        /// </summary>
        /// <value>The new level.</value>
        public double NewLevel {
            get { return _newLevel; }
            protected internal set { _newLevel = value; }
        }

        /// <summary>
        /// Gets or sets the old level.
        /// </summary>
        /// <value>The old level.</value>
        public double OldLevel {
            get { return _oldLevel; }
            protected internal set { _oldLevel = value; }
        }
        #endregion

        #region Construct /////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleZoomEventArgs"/> class.
        /// </summary>
        /// <param name="newLevel">The new level.</param>
        /// <param name="oldLevel">The old level.</param>
        public GoogleZoomEventArgs(double newLevel, double oldLevel) {
            this.NewLevel = newLevel;
            this.OldLevel = oldLevel;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleZoomEventArgs"/> class.
        /// </summary>
        /// <param name="args">The args.</param>
        public GoogleZoomEventArgs(string args) {

            this.NewLevel = 0D;
            this.OldLevel = 0D;

            if (!string.IsNullOrEmpty(args)) {
                args = args.Trim('(', ')');
                string[] pair = args.Split(',');
                if (pair.Length >= 2) {
                    this.OldLevel = JsUtil.ToDouble(pair[0]);
                    this.NewLevel = JsUtil.ToDouble(pair[1]);
                }
            }
        }
        #endregion
    }
}
