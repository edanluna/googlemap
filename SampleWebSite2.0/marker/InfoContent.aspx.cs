using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Artem.Web.UI.Controls;

public partial class marker_InfoContent : System.Web.UI.Page {

    #region Methods ///////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
    /// </summary>
    /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
    protected override void OnLoad(EventArgs e) {
        base.OnLoad(e);

        //GoogleMap1.Markers[0].InfoContent.Controls.Add(new Calendar());

        GoogleMarker marker = new GoogleMarker();
        marker.Address = "Sofia Bulgaria";
        marker.InfoContent.Controls.Add(LoadControl("~/controls/TestInfoContent.ascx"));

        GoogleMap1.Markers.Add(marker);
    }
    #endregion
}
