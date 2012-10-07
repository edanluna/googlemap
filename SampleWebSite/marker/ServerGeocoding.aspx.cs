using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Artem.Web.UI.Controls;

public partial class marker_ServerGeocoding : System.Web.UI.Page {

    protected void HandleAny(object sender, GoogleEventArgs e) {

        GoogleMarker marker = sender as GoogleMarker;
        if (marker != null) {
            double lat = marker.Latitude;
            double lng = marker.Longitude;
            // TODO process the lat/lng of the marker
        }
    }

    protected override void OnLoad(EventArgs e) {
        base.OnLoad(e);

        if (!IsPostBack) {
            GoogleMarker marker = new GoogleMarker("sofia bulgaria");
            GoogleMap1.Markers.Add(marker);
        }
    }
}
