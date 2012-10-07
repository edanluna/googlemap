using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace Artem.Web.UI.Controls {

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StateCollection<T> : List<T>, IStateManager where T : IStateManager, new(){

        #region Fields  /////////////////////////////////////////////////////////////////

        bool _tracking;

        #endregion

        #region Properties  /////////////////////////////////////////////////////////////

        /// <summary>
        /// When implemented by a class, gets a value indicating whether a server control is tracking its view state changes.
        /// </summary>
        /// <value></value>
        /// <returns>true if a server control is tracking its view state changes; otherwise, false.</returns>
        public bool IsTrackingViewState {
            get { return _tracking; }
        }
        #endregion

        #region Methods /////////////////////////////////////////////////////////////////

        /// <summary>
        /// Loads the state of the view.
        /// </summary>
        /// <param name="savedState">State of the saved.</param>
        public void LoadViewState(object savedState) {

            object[] state = savedState as object[];
            if (state != null) {
                T item;
                bool exists;
                for (int i = 0; i < state.Length; i++) {
                    item = (exists =( i < this.Count)) ? this[i] : new T();
                    item.LoadViewState(state[i]);
                    if (this.IsTrackingViewState)
                        item.TrackViewState();
                    if(!exists) Add(item);
                }
            }
        }

        /// <summary>
        /// When implemented by a class, saves the changes to a server control's view state to an <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.Object"/> that contains the view state changes.
        /// </returns>
        public object SaveViewState() {

            if (this.Count > 0) {
                int count = this.Count;
                object[] state = new object[count];
                for (int i = 0; i < count; i++) {
                    state[i] = this[i].SaveViewState();
                }
                return state;
            }
            else
                return null;
        }

        /// <summary>
        /// When implemented by a class, instructs the server control to track changes to its view state.
        /// </summary>
        public void TrackViewState() {
            _tracking = true;
        }
        #endregion
    }
}
