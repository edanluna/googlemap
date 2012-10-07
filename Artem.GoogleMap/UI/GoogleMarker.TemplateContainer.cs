﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Artem.Google.UI {
    public partial class GoogleMarker {

        /// <summary>
        /// 
        /// </summary>
        public class TemplateContainer : WebControl, INamingContainer {

            #region Properties  /////////////////////////////////////////////////////////////

            /// <summary>
            /// Gets the server control identifier generated by ASP.NET.
            /// </summary>
            /// <value></value>
            /// <returns>
            /// The server control identifier generated by ASP.NET.
            /// </returns>
            public override string ClientID {
                get { return this.ID; }
            }

            /// <summary>
            /// Gets the <see cref="T:System.Web.UI.HtmlTextWriterTag"/> value that corresponds to this Web server control. This property is used primarily by control developers.
            /// </summary>
            /// <value></value>
            /// <returns>One of the <see cref="T:System.Web.UI.HtmlTextWriterTag"/> enumeration values.</returns>
            protected override HtmlTextWriterTag TagKey {
                get { return HtmlTextWriterTag.Div; }
            }

            /// <summary>
            /// Gets the unique, hierarchically qualified identifier for the server control.
            /// </summary>
            /// <value></value>
            /// <returns>
            /// The fully qualified identifier for the server control.
            /// </returns>
            public override string UniqueID {
                get { return this.ID; }
            }
            #endregion
        }
    }
}
