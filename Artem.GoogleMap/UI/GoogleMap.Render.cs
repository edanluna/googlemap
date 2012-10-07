using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using Artem.Google.Resources;

namespace Artem.Google.UI {

    /// <summary>
    /// The values for the GoogleMap RenderMode - Auto, Ajax, Web.
    /// On Auto render mode the control with switch automatically to Ajax render mode if ScriptManager 
    /// instance exists on the page, otherwise the old render mode will be used which is now known as Web.
    /// If the render mode is Ajax, the control will force usage of the ScriptControl on the page,
    /// otherwise will render error to the page as any ScriptControl.
    /// If the render mofe is Web, then the old render mode and JS inludes and references will be forced,
    /// even if the page is MS AJAX enabled (instance of ScriptControl exists, or referenced as script links).
    /// </summary>
    public enum MapRenderMode {
        /// <summary>
        /// 
        /// </summary>
        Auto,
        /// <summary>
        /// 
        /// </summary>
        Ajax,
        /// <summary>
        /// 
        /// </summary>
        Web
    }

    /// <summary>
    /// 
    /// </summary>
    partial class GoogleMap {

        #region Static Fields /////////////////////////////////////////////////////////////////////

        public static readonly string MapsApiUrl = "http://maps.google.com/maps/api/js?";

        #endregion

        #region Properties  ///////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Gets the client script.
        /// </summary>
        /// <value>The client script.</value>
        private ClientScriptManager ClientScript {
            get {
                if (_clientScript == null) {
                    Page page = this.Page;
                    if (page == null)
                        throw new InvalidOperationException(Errors.PageCannotBeNull);
                    _clientScript = page.ClientScript;
                }
                return _clientScript;
            }
        }
        ClientScriptManager _clientScript;

        /// <summary>
        /// Gets the script manager.
        /// </summary>
        /// <value>The script manager.</value>
        private ScriptManager ScriptManager {
            get {
                if (_scriptManager == null) {
                    Page page = this.Page;
                    if (page == null)
                        throw new InvalidOperationException(Errors.PageCannotBeNull);
                    _scriptManager = ScriptManager.GetCurrent(page);
                    if (this.RenderMode == MapRenderMode.Ajax && _scriptManager == null)
                        throw new InvalidOperationException(string.Format(Errors.RequiresScriptManager, this.ID));
                }
                return _scriptManager;
            }
        }
        ScriptManager _scriptManager;

        #endregion

        #region Methods ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e) {
            base.OnPreRender(e);

            if (!this.DesignMode) {
                ScriptManager.RegisterHiddenField(this, this.ClientStateID, this.SaveClientState());
                this.Page.RegisterRequiresPostBack(this);
                this.RenderGoogleReference();
                this.RenderScriptReferences();
            }
        }

        /// <summary>
        /// Renders the control to the specified HTML writer.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> object that receives the control content.</param>
        protected override void Render(HtmlTextWriter writer) {

            base.Render(writer);
            if (!this.DesignMode) this.RenderScriptDescriptors();
        }

        //public override void RenderBeginTag(HtmlTextWriter writer) {

        //    writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "");
        //    base.RenderBeginTag(writer);
        //}

        /// <summary>
        /// Renders the contents of the control to the specified writer. This method is used primarily by control developers.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter"/> that represents the output stream to render HTML content on the client.</param>
        protected override void RenderContents(HtmlTextWriter writer) {
            // KEEP THIS EMPTY
        }

        /// <summary>
        /// Renders the HTML closing tag of the control into the specified writer. This method is used primarily by control developers.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter"/> that represents the output stream to render HTML content on the client.</param>
        public override void RenderEndTag(HtmlTextWriter writer) {
            base.RenderEndTag(writer);

            // must be outside the map div otherwise Google API script will remove it
            if (_templateContainer != null)
                _templateContainer.RenderControl(writer);
            // street view pano
            //bool flag = this.IsStreetView &&
            //            this.StreetViewMode == StreetViewMode.Overlay &&
            //            string.IsNullOrEmpty(this.StreetViewPanoID);
            //if (flag) {
            //    writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + "_Pano");
            //    writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "500px");
            //    writer.AddStyleAttribute(HtmlTextWriterStyle.Height, "200px");
            //    writer.RenderBeginTag(HtmlTextWriterTag.Div);
            //    writer.RenderEndTag();
            //}
        }

        /// <summary>
        /// Builds the google reference.
        /// </summary>
        /// <returns></returns>
        protected virtual void RenderGoogleReference() {

            var clientScript = this.Page.ClientScript;

            if (!clientScript.IsClientScriptIncludeRegistered("maps.google.com")) {
                StringBuilder buffer = new StringBuilder(MapsApiUrl);

                if (!string.IsNullOrEmpty(this.ApiVersion))
                    buffer.AppendFormat("v={0}&", this.ApiVersion);
                // sensor
                buffer.AppendFormat("sensor={0}", this.IsSensor.ToString().ToLower());
                // language
                if (!string.IsNullOrEmpty(this.Language))
                    buffer.AppendFormat("&language={0}", this.Language);
                // region
                if (!string.IsNullOrEmpty(this.Region))
                    buffer.AppendFormat("&region={0}", this.Region);

                //    buffer.Append("&allow_bidi=true");
                //if (!string.IsNullOrEmpty(this.EnterpriseKey))
                //    buffer.AppendFormat("&client={0}", this.EnterpriseKey);

                clientScript.RegisterClientScriptInclude("maps.google.com", buffer.ToString());
            }
        }

        /// <summary>
        /// Registers the script descriptors.
        /// </summary>
        private void RenderScriptDescriptors() {

            ScriptManager scriptManager = this.ScriptManager;
            if (scriptManager != null) {
                scriptManager.RegisterScriptDescriptors(this);
            }
            else {
                var csm = this.ClientScript;
                Type type = this.GetType();
                string key = "Artem.Google.Map:" + this.ID;
                bool appInitialized = Convert.ToBoolean(this.Context.Items["Sys.Application.initialize"]);

                string properties = this.Serializer.Serialize(new {
                    clientMapID = this.ID,
                    clientStateID = this.ClientStateID,
                    //mapEvents = this.MapEvents.ServerEvents.ToArray(),
                    name = this.UniqueID
                });

                Dictionary<string, string> eventEntries = new Dictionary<string, string>();
                // map events
                foreach (var name in this.MapEvents.Registry.Keys) {
                    foreach (var handler in this.MapEvents.Registry[name])
                        eventEntries.Add(name, handler);
                }
                // marker events
                foreach (var name in this.MarkerEvents.Registry.Keys) {
                    foreach (var handler in this.MarkerEvents.Registry[name])
                        eventEntries.Add("marker_" + name, handler);
                }
                // directions events
                foreach (var name in this.DirectionsEvents.Registry.Keys) {
                    foreach (var handler in this.DirectionsEvents.Registry[name])
                        eventEntries.Add("directions_" + name, handler);
                }
                // polygon events
                foreach (var name in this.PolygonEvents.Registry.Keys) {
                    foreach (var handler in this.PolygonEvents.Registry[name])
                        eventEntries.Add("polygon_" + name, handler);
                }
                // polyline events
                foreach (var name in this.PolylineEvents.Registry.Keys) {
                    foreach (var handler in this.PolylineEvents.Registry[name])
                        eventEntries.Add("polyline_" + name, handler);
                }
                string events = this.Serializer.Serialize(eventEntries);

                StringBuilder buffer = new StringBuilder();
                if (!appInitialized) {
                    buffer.AppendLine("Sys.Application.initialize();");
                    this.Context.Items["Sys.Application.initialize"] = true;
                }
                buffer
                    .AppendLine("Sys.Application.add_init(function() {")
                    .AppendFormat("\t$create(Artem.Google.Map, {0}, {1}, null, $get(\"{2}\"));", properties, events, this.ClientID)
                    .AppendLine()
                    .AppendLine("});");
                csm.RegisterStartupScript(type, key, buffer.ToString(), true);
                //csm.RegisterOnSubmitStatement(type, this.ClientStateID, "Artem.Google.Manager.save();");
            }
        }

        /// <summary>
        /// Registers the script references.
        /// </summary>
        private void RenderScriptReferences() {

            ScriptManager scriptManager = this.ScriptManager;
            if (scriptManager != null) {
                scriptManager.RegisterScriptControl(this);
            }
            else {
                var clientScript = this.Page.ClientScript;
                string name;
                Type type = this.GetType();

                foreach (var reference in this.GetScriptReferences()) {
                    name = reference.Name;
                    if (!clientScript.IsClientScriptIncludeRegistered(name)) {
                        clientScript.RegisterClientScriptResource(type, name);
                    }
                }
                //this.RenderScriptReferencesX();
            }
        }

        #region - Fetch Script References & Descriptors -

        /// <summary>
        /// Gets the script descriptors.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ScriptDescriptor> GetScriptDescriptors() {

            // map descriptor
            var descriptor = new ScriptControlDescriptor("Artem.Google.Map", this.ClientID);

            // properties
            descriptor.AddProperty("clientMapID", this.ID);
            descriptor.AddProperty("clientStateID", this.ClientStateID);
            //descriptor.AddProperty("mapEvents", this.MapEvents.ServerEvents.ToArray());
            descriptor.AddProperty("name", this.UniqueID);
            // map events
            foreach (var name in this.MapEvents.Registry.Keys) {
                foreach (var handler in this.MapEvents.Registry[name])
                    descriptor.AddEvent(name, handler);
            }
            // marker events
            foreach (var name in this.MarkerEvents.Registry.Keys) {
                foreach (var handler in this.MarkerEvents.Registry[name])
                    descriptor.AddEvent("marker_" + name, handler);
            }
            // directions events
            foreach (var name in this.DirectionsEvents.Registry.Keys) {
                foreach (var handler in this.DirectionsEvents.Registry[name])
                    descriptor.AddEvent("directions_" + name, handler);
            }
            // polygon events
            foreach (var name in this.PolygonEvents.Registry.Keys) {
                foreach (var handler in this.PolygonEvents.Registry[name])
                    descriptor.AddEvent("polygon_" + name, handler);
            }
            // polyline events
            foreach (var name in this.PolylineEvents.Registry.Keys) {
                foreach (var handler in this.PolylineEvents.Registry[name])
                    descriptor.AddEvent("polyline_" + name, handler);
            }

            yield return descriptor;

        }

        /// <summary>
        /// Gets the script references.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ScriptReference> GetScriptReferences() {

            string assembly = this.GetType().Assembly.FullName;
#if DEBUG
            if (this.ScriptManager == null)
                yield return new ScriptReference("Artem.Google.Scripts.MicrosoftAjax.debug.js", assembly);

            yield return new ScriptReference("Artem.Google.Scripts.GoogleCommons.js", assembly);
            yield return new ScriptReference("Artem.Google.Scripts.GoogleMap.js", assembly);

            if (_markers.IsNotNullOrEmpty()) {
                yield return new ScriptReference("Artem.Google.Scripts.GoogleMarker.js", assembly);
                if (this.EnableMarkerManager)
                    yield return new ScriptReference("Artem.Google.Scripts.markermanager.js", assembly);
            }

            if (_polygons.IsNotNullOrEmpty()) 
                yield return new ScriptReference("Artem.Google.Scripts.GooglePolygon.js", assembly);

            if (_polylines.IsNotNullOrEmpty()) 
                yield return new ScriptReference("Artem.Google.Scripts.GooglePolyline.js", assembly);

            if (_directions.IsNotNullOrEmpty()) 
                yield return new ScriptReference("Artem.Google.Scripts.GoogleDirections.js", assembly);
#else
            if (this.ScriptManager == null)
                yield return new ScriptReference("Artem.Google.Scripts.MicrosoftAjax.js", assembly);

            yield return new ScriptReference("Artem.Google.Scripts.GoogleCommons.min.js", assembly);
            yield return new ScriptReference("Artem.Google.Scripts.GoogleMap.min.js", assembly);

            if (_markers.IsNotNullOrEmpty()) {
                yield return new ScriptReference("Artem.Google.Scripts.GoogleMarker.min.js", assembly);
                if (this.EnableMarkerManager)
                    yield return new ScriptReference("Artem.Google.Scripts.markermanager.js", assembly);
            }

            if (_polygons.IsNotNullOrEmpty()) 
                yield return new ScriptReference("Artem.Google.Scripts.GooglePolygon.min.js", assembly);

            if (_polylines.IsNotNullOrEmpty()) 
                yield return new ScriptReference("Artem.Google.Scripts.GooglePolyline.min.js", assembly);

            if (_directions.IsNotNullOrEmpty()) 
                yield return new ScriptReference("Artem.Google.Scripts.GoogleDirections.min.js", assembly);
#endif
        }
        #endregion

        #region - OBSOLETE -

        //        /// <summary>
        //        /// Renders the script references.
        //        /// </summary>
        //        protected virtual void RenderScriptReferencesX() {

        //            var csm = this.ClientScript;
        //            Type type = this.GetType();
        //#if DEBUG
        //            ScriptReference sr;
        //            //sr.

        //            csm.RegisterClientScriptResource(type, "Artem.Google.Scripts.MicrosoftAjax.debug.js");
        //            csm.RegisterClientScriptResource(type, "Artem.Google.Scripts.ArtemGoogle.js");
        //            if (this.EnableMarkerManager)
        //                csm.RegisterClientScriptResource(type, "Artem.Google.Scripts.markermanager.js");
        //#else
        //                csm.RegisterClientScriptResource(type, "Artem.Google.Scripts.MicrosoftAjax.js");
        //                csm.RegisterClientScriptResource(type, "Artem.Google.Scripts.ArtemGoogle.min.js");
        //                if (this.EnableMarkerManager)
        //                    csm.RegisterClientScriptResource(type, "Artem.Google.Scripts.markermanager_packed.js");
        //#endif
        //        }

        ///// <summary>
        ///// Renders the init script.
        ///// </summary>
        ///// <returns></returns>
        //protected virtual string RenderMapScript() {

        //    string id = this.ID;
        //    //string action;
        //    StringBuilder script = new StringBuilder();
        //    // construct
        //    script
        //        .AppendFormat("var {0} = new Artem.Google.Map({1});", id, this.SaveClientState())
        //        .AppendLine();
        //    //// map's actions
        //    //for (int i = 0; i < this.Actions.Count; i++) {
        //    //    action = this.Actions[i];
        //    //    script
        //    //        .AppendFormat("{0}.addAction({1});", id, JsUtil.Encode(action))
        //    //        .AppendLine();
        //    //}
        //    //// markers
        //    //string config;
        //    //GoogleMarker marker;
        //    //for (int i = 0; i < this.Markers.Count; i++) {
        //    //    marker = this.Markers[i];
        //    //    if (_markerStyle != null) _markerStyle.ApplyOnMarker(marker);
        //    //    config = marker.ToJsonString();
        //    //    config = Regex.Replace(config, @"(""IconUrl"":"")([^""]*)""",
        //    //        delegate(Match m) {
        //    //            return string.Format("{0}{1}\"", m.Groups[1].Value, ResolveUrl(m.Groups[2].Value));
        //    //        },
        //    //        JsUtil.DefaultRegexOptions
        //    //    );
        //    //    config = Regex.Replace(config, @"(""ShadowUrl"":"")([^""]*)""",
        //    //        delegate(Match m) {
        //    //            return string.Format("{0}{1}\"", m.Groups[1].Value, ResolveUrl(m.Groups[2].Value));
        //    //        },
        //    //        JsUtil.DefaultRegexOptions
        //    //    );
        //    //    script
        //    //        .AppendFormat("{0}.addMarker({1});", id, config)
        //    //        .AppendLine();
        //    //    // info window content template
        //    //    if (marker.InfoContent.Controls.Count > 0) {
        //    //        marker.InfoContent.ID = string.Format("Marker{0}Content", i.ToString());
        //    //        _templateContainer.Controls.Add(marker.InfoContent);
        //    //        script.AppendFormat("{0}.Markers[{1}].OpenWindowContent = '{2}';", id, i, marker.InfoContent.ClientID);
        //    //    }
        //    //    else if (marker.InfoWindowTemplate != null) {
        //    //        GoogleMarker.TemplateContainer container = new GoogleMarker.TemplateContainer();
        //    //        container.ID = string.Format("Marker{0}Content", i.ToString());
        //    //        marker.InfoWindowTemplate.InstantiateIn(container);
        //    //        _templateContainer.Controls.Add(container);
        //    //        script.AppendFormat("{0}.Markers[{1}].OpenWindowContent = '{2}';", id, i, container.ClientID);
        //    //    }
        //    //    // marker's actions
        //    //    for (int j = 0; j < marker.Actions.Count; j++) {// string action in this.Markers[i].Actions) {
        //    //        action = marker.Actions[j];
        //    //        action = string.Format(action, id, i);
        //    //        script
        //    //            .AppendFormat("{0}.addAction({1});", id, JsUtil.Encode(action))
        //    //            .AppendLine();
        //    //    }
        //    //}
        //    //// directions
        //    //GoogleDirection dir;
        //    //for (int i = 0; i < this.Directions.Count; i++) {
        //    //    dir = this.Directions[i];
        //    //    script
        //    //        .AppendFormat("{0}.addDirection({1});", id, dir.ToJsonString())
        //    //        .AppendLine();
        //    //    // direction's actions
        //    //    for (int j = 0; j < dir.Actions.Count; j++) {
        //    //        action = dir.Actions[j];
        //    //        action = string.Format(action, id, i);
        //    //        script
        //    //            .AppendFormat("{0}.addAction({1});", id, JsUtil.Encode(action))
        //    //            .AppendLine();
        //    //    }
        //    //}
        //    //// polylines
        //    //GooglePolyline line;
        //    //for (int i = 0; i < this.Polylines.Count; i++) {
        //    //    line = this.Polylines[i];
        //    //    script
        //    //        .AppendFormat("{0}.addPolyline({1});", id, line.ToJsonString())
        //    //        .AppendLine();
        //    //    // polyline's actions
        //    //    for (int j = 0; j < line.Actions.Count; j++) {
        //    //        action = line.Actions[j];
        //    //        action = string.Format(action, id, i);
        //    //        script
        //    //            .AppendFormat("{0}.addAction({1});", id, JsUtil.Encode(action))
        //    //            .AppendLine();
        //    //    }
        //    //}
        //    //// polygons
        //    //GooglePolygon pg;
        //    //for (int i = 0; i < this.Polygons.Count; i++) {
        //    //    pg = this.Polygons[i];
        //    //    script
        //    //        .AppendFormat("{0}.addPolygon({1});", id, pg.ToJsonString())
        //    //        .AppendLine();
        //    //    // polygon's actions
        //    //    for (int j = 0; j < pg.Actions.Count; j++) {
        //    //        action = pg.Actions[j];
        //    //        action = string.Format(action, id, i);
        //    //        script
        //    //            .AppendFormat("{0}.addAction({1});", id, JsUtil.Encode(action))
        //    //            .AppendLine();
        //    //    }
        //    //}
        //    //// load
        //    //script.AppendFormat("{0}.load();", id).AppendLine();
        //    return script.ToString();
        //}
        #endregion
        #endregion
    }
}
