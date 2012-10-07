using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Artem.Google.Web.Demo {

    public partial class DefaultPage : System.Web.UI.Page {

        protected void HandleTestClick(object sender, EventArgs e) {
            ltrDate.Text = DateTime.Now.ToString();
        }
    }
}