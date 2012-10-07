using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Artem.GoogleMap.WebSite;

public partial class examples_ListDataBound : Artem.GoogleMap.WebSite.UI.PageBase {

    #region Methods ///////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Handles the item data bound.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.Web.UI.WebControls.DataListItemEventArgs"/> instance containing the event data.</param>
    protected void HandleItemDataBound(object sender, DataListItemEventArgs e) {

        Artem.Web.UI.Controls.GoogleMap map = e.Item.FindControl("crtGooleMap") as Artem.Web.UI.Controls.GoogleMap;
        if (map != null) {
            GoogleMapData dataItem = (GoogleMapData)e.Item.DataItem;
            map.Latitude = dataItem.Latitude;
            map.Longitude = dataItem.Longitude;
            map.Zoom = dataItem.Zoom;
        }
    } 
    #endregion
}
