using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.Design;
using System.ComponentModel;

namespace Artem.Web.UI.Controls {

    /// <summary>
    /// 
    /// </summary>
    public class GoogleMapDesigner : ControlDesigner {

        #region Properties  /////////////////////////////////////////////////////////////

        ///// <summary>
        ///// 
        ///// </summary>
        //public override TemplateGroupCollection TemplateGroups {
        //    get {
        //        TemplateGroupCollection collection = new TemplateGroupCollection();
        //        TemplateGroup group;
        //        TemplateDefinition template;
        //        GoogleMap control;

        //        control = (GoogleMap)Component;
        //        group = new TemplateGroup("Item");
        //        template = new TemplateDefinition(this, "Template", control, "Template", true);
        //        group.AddTemplateDefinition(template);
        //        collection.Add(group);
        //        return collection;
        //    }
        //}
        #endregion

        #region Methods /////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string GetDesignTimeHtml() {
            GoogleMap map = this.Component as GoogleMap;
            if (map != null)
                return string.Format("<span>Google Map points to {0}:{1} (latitude:longitude)</span>", map.Latitude, map.Longitude);
            else
                return "<span>Google Map</span>";
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="Component"></param>
        //public override void Initialize(IComponent Component) {
        //    base.Initialize(Component);
        //    SetViewFlags(ViewFlags.TemplateEditing, true);
        //}
        #endregion
    }
}
