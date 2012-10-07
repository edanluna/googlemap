using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Artem.Web.UI.Controls;

public partial class map_ClientUpdate : Artem.GoogleMap.WebSite.UI.PageBase {

    #region Static Methods //////////////////////////////////////////////////////////

    #endregion

    #region Methods /////////////////////////////////////////////////////////////////

    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
    /// </summary>
    /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
    protected override void OnLoad(EventArgs e) {
        base.OnLoad(e);
        //
        int counter = 0;
        int.TryParse(Session["__Counter"] as string, out counter);
        Session["__Counter"] = (++counter).ToString();
        _ltrCounter.Text = string.Format("Counter: {0}", counter.ToString());
    }

    protected override void OnPreRender(EventArgs e) {
        base.OnPreRender(e);
        //
        if (this.IsPostBack) {
            Random rnd = new Random();
            double lat, lng;
            StringBuilder buff = new StringBuilder();
            buff.AppendFormat("{0}.Markers = null;{0}.clearOverlays();", GoogleMap1.ClientMapID);
            for (int i = 0; i < 5; i++) {
                lat = (rnd.NextDouble() * 25) + 30;
                lng = (rnd.NextDouble() * 15) + 15;
                buff.AppendFormat("{0}.addMarker(", GoogleMap1.ClientMapID)
                    .Append("{")
                    .AppendFormat("Latitude:{0},Longitude:{1}", JsUtil.Encode(lat), JsUtil.Encode(lng))
                    .Append("});");
            }
            buff.AppendFormat("{0}.render();", GoogleMap1.ClientMapID);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "__Test",
                "Sys.Application.add_load(function() {" + buff.ToString() + ";});", true);
        }
    }
    #endregion
}
