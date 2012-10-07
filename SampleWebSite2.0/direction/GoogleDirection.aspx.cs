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

public partial class direction_GoogleDirection : Artem.GoogleMap.WebSite.UI.PageBase {

    #region Methods /////////////////////////////////////////////////////////////////

    protected void HandleShowDirectionsClick(object sender, EventArgs e) {

        string query = _txtQuery.Text;
        if (string.IsNullOrEmpty(query))
            query = "from: San Francisco, CA to: Mountain View, CA";
        string locale = _txtLocale.Text;
        GoogleMap1.Directions.Clear();
        GoogleMap1.Directions.Add(new GoogleDirection(query, "route", locale));
    }

    protected void HandleShowExtraData(object sender, EventArgs e) {

        if (GoogleMap1.Directions.Count > 0) {
            GoogleDirection dir = GoogleMap1.Directions[0];
            _ltrInfo.Text = string.Format("Bounds: {0}<br/>Distance: {1}<br/>Duration: {2}",
                dir.Bounds.ToString(), dir.Distance.Html, dir.Duration.Html);
        }
    }

    protected override void OnLoad(EventArgs e) {
        base.OnLoad(e);
        if (GoogleMap1.Directions.Count > 0)
            _ltrInfo.Text = GoogleMap1.Directions[0].Bounds.ToString();
    }
    #endregion
}
