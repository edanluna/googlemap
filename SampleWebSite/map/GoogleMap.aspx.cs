using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Artem.Web.UI.Controls;

public partial class map_GoogleMap : Artem.GoogleMap.WebSite.UI.PageBase {

    #region Methods /////////////////////////////////////////////////////////////////

    /// <summary>
    /// Handles the show click.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void HandleShowClick(object sender, EventArgs e) {

        string address = _txtAddress.Text;
        if (!string.IsNullOrEmpty(address)) {
            GoogleMap1.Address = address;
        }
    }
    #endregion
}
