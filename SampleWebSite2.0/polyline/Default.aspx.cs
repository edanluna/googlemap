using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Artem.Web.UI.Controls;

public partial class polyline_GooglePolyline : Artem.GoogleMap.WebSite.UI.PageBase {

    #region Methods /////////////////////////////////////////////////////////////////

    /// <summary>
    /// Handles the show polyline click.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void HandleShowPolylineClick(object sender, EventArgs e) {

        string value = this.Request["__points"];
        if (!string.IsNullOrEmpty(value)) {
            GooglePolyline line = new GooglePolyline();
            line.Color = Color.Blue;
            line.Weight = 2;
            string[] points = value.Split(';');
            foreach (string point in points) {
                line.Points.Add(GoogleLocation.Parse(point));
            }
            GoogleMap1.Polylines.Clear();
            GoogleMap1.Polylines.Add(line);
        }
    }

    protected void HandleShowExtraDataClick(object sender, EventArgs e) {

        if (GoogleMap1.Polylines.Count > 0) {
            GooglePolyline line = GoogleMap1.Polylines[0];
            _ltrInfo.Text = string.Format("Bounds: {0}", line.Bounds.ToString());
        }
    }
    #endregion
}
