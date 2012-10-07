using System;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Artem.GoogleMap.WebSite.UI {

    /// <summary>
    /// 
    /// </summary>
    public class PageBase : System.Web.UI.Page {

        #region Static Fields ///////////////////////////////////////////////////////////

        static readonly string _WhitespacePattern = "\\s+";

        #endregion

        #region Fields  /////////////////////////////////////////////////////////////////

        string _description;
        string _keywords;

        #endregion

        #region Properties  /////////////////////////////////////////////////////////////

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public virtual string MetaDescription {
            get { return _description; }
            set {
                _description = Regex.Replace(
                    value, _WhitespacePattern, " ");
            }
        }

        /// <summary>
        /// Gets or sets the keywords.
        /// </summary>
        /// <value>The keywords.</value>
        public virtual string MetaKeywords {
            get { return _keywords; }
            set {
                _keywords = Regex.Replace(
                    value, _WhitespacePattern, " ");
            }
        }
        #endregion

        #region Methods /////////////////////////////////////////////////////////////////

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnInit(EventArgs e) {
            base.OnInit(e);
            ///
            CreateSeoHeaders();
        }

        #region - SEO Headers -

        /// <summary>
        /// Creates the seo headers.
        /// </summary>
        protected void CreateSeoHeaders() {
            ///
            /// Title meta
            ///
            HtmlHead header = Page.Header;
            HtmlMeta meta;
            string content = this.Title;
            if (!string.IsNullOrEmpty(content)) {
                meta = new HtmlMeta();
                meta.Name = "title";
                meta.Content = content;
                header.Controls.Add(meta);
            }
            ///
            /// Description
            /// 
            if (!string.IsNullOrEmpty(_description)) {
                //HtmlGenericControl comment = new HtmlGenericControl(null);
                //comment.InnerText = string.Format("!-- {0} --", _description);
                //header.Controls.Add(comment);
                ///
                meta = new HtmlMeta();
                meta.Name = "description";
                meta.Content = _description;
                header.Controls.Add(meta);
            }
            if (!string.IsNullOrEmpty(_keywords)) {
                ///
                /// keywords
                /// 
                meta = new HtmlMeta();
                meta.Name = "keywords";
                meta.Content = _keywords;
                header.Controls.Add(meta);
            }
        }
        #endregion
        #endregion
    }
}
