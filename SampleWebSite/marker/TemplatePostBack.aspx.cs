using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class marker_TemplatePostBack : System.Web.UI.Page {

    #region Methods ///////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Handles the click.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void HandleClick(object sender, EventArgs e) {
        txtTimestamp.Text = DateTime.Now.ToString();
    }
    #endregion
}
