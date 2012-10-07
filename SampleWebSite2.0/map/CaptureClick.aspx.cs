using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Artem.Web.UI.Controls;

public partial class map_CaptureClick : Artem.GoogleMap.WebSite.UI.PageBase {

    #region Methods /////////////////////////////////////////////////////////////////

    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
    /// </summary>
    /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
    protected override void OnLoad(EventArgs e) {
        base.OnLoad(e);
        //
        if (IsPostBack) {
            string address = _txtAddress.Text;
            GoogleMap1.Address = address;
            GoogleMap1.Markers.Clear();
            GoogleMap1.Markers.Add(new GoogleMarker(address));
        }
    } 
    #endregion
}
