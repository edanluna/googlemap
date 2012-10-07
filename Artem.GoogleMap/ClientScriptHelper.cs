using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Web.UI;

namespace Artem.Web.UI.Controls {

    /// <summary>
    /// 
    /// </summary>
    internal class ClientScriptHelper {

        #region Static Fields ///////////////////////////////////////////////////////////

        public static string AjaxAssemblyName = "System.Web.Extensions";

        static bool _IsAjaxAvailable;
        static Type _ScriptManagerType;
        static Type _UpdatePanelType;

        static MethodInfo _GetCurrent;
        static MethodInfo _RegisterArrayDeclaration;
        static MethodInfo _RegisterClientScriptBlock;
        static MethodInfo _RegisterClientScriptInclude;
        static MethodInfo _RegisterClientScriptResource;
        static MethodInfo _RegisterExpandoAttribute;
        static MethodInfo _RegisterHiddenField;
        static MethodInfo _RegisterOnSubmitStatement;
        static MethodInfo _RegisterStartupScript;

        #endregion

        #region Static Properties ///////////////////////////////////////////////////////

        /// <summary>
        /// Gets a value indicating whether this instance is ajax available.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is ajax available; otherwise, <c>false</c>.
        /// </value>
        public static bool IsAjaxAvailable {
            get { return _IsAjaxAvailable; }
        }
        #endregion

        #region Static Construct ////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes the <see cref="ClientScriptHelper"/> class.
        /// </summary>
        static ClientScriptHelper() {

            Assembly assembly = null;
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies()) {
                if (asm.FullName.StartsWith(AjaxAssemblyName)) {
                    assembly = asm;
                    break;
                }
            }
            if (assembly != null) {
                _ScriptManagerType = assembly.GetType("System.Web.UI.ScriptManager");
                _UpdatePanelType = assembly.GetType("System.Web.UI.UpdatePanel");
                if (_ScriptManagerType != null) {
                    _IsAjaxAvailable = true;
                    //
                    _GetCurrent = _ScriptManagerType.GetMethod(
                        "GetCurrent", new Type[] { typeof(Page) });
                    _RegisterArrayDeclaration = _ScriptManagerType.GetMethod(
                        "RegisterArrayDeclaration", new Type[] { typeof(Page), typeof(string), typeof(string) });
                    _RegisterClientScriptBlock = _ScriptManagerType.GetMethod(
                        "RegisterClientScriptBlock", new Type[] { typeof(Page), typeof(Type), typeof(string), typeof(string), typeof(bool) });
                    _RegisterClientScriptInclude = _ScriptManagerType.GetMethod(
                        "RegisterClientScriptInclude", new Type[] { typeof(Page), typeof(Type), typeof(string), typeof(string) });
                    _RegisterClientScriptResource = _ScriptManagerType.GetMethod(
                        "RegisterClientScriptResource", new Type[] { typeof(Page), typeof(Type), typeof(string) });
                    _RegisterExpandoAttribute = _ScriptManagerType.GetMethod(
                        "RegisterExpandoAttribute", new Type[] { typeof(Control), typeof(string), typeof(string), typeof(string), typeof(bool) });
                    _RegisterHiddenField = _ScriptManagerType.GetMethod(
                        "RegisterHiddenField", new Type[] { typeof(Page), typeof(string), typeof(string) });
                    _RegisterOnSubmitStatement = _ScriptManagerType.GetMethod(
                        "RegisterOnSubmitStatement", new Type[] { typeof(Page), typeof(Type), typeof(string), typeof(string) });
                    _RegisterStartupScript = _ScriptManagerType.GetMethod(
                        "RegisterStartupScript", new Type[] { typeof(Page), typeof(Type), typeof(string), typeof(string), typeof(bool) });
                }
            }
        }
        #endregion

        #region Static Methods //////////////////////////////////////////////////////////



        /// <summary>
        /// Determines whether [is ajax enabled] [the specified page].
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns>
        /// 	<c>true</c> if [is ajax enabled] [the specified page]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAjaxEnabled(Page page) {
            return GetCurrentScripManager(page) != null;
        }

        /// <summary>
        /// Determines whether [is ajax enabled] [the specified page].
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns>
        /// 	<c>true</c> if [is ajax enabled] [the specified page]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAjaxPostBack(Page page) {
            return (page.Request.Headers["x-microsoftajax"] != null);
        }

        /// <summary>
        /// Determines whether [is inside update panel] [the specified control].
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns>
        /// 	<c>true</c> if [is inside update panel] [the specified control]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInsideUpdatePanel(Control control) {

            Control parent = control;
            while ((parent = parent.Parent) != null) {
                if (parent.GetType() == _UpdatePanelType) return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the current scrip manager.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        public static object GetCurrentScripManager(Page page) {

            if (_GetCurrent != null)
                return _GetCurrent.Invoke(null, new object[] { page });
            return null;
        }

        /// <summary>
        /// Registers the array declaration.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public static void RegisterArrayDeclaration(Page page, string name, string value) {

            if (IsAjaxEnabled(page))
                _RegisterArrayDeclaration.Invoke(null, new object[] { page, name, value });
            else
                page.ClientScript.RegisterArrayDeclaration(name, value);
        }

        /// <summary>
        /// Registers the client script block.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="type">The type.</param>
        /// <param name="key">The key.</param>
        /// <param name="script">The script.</param>
        /// <param name="addScriptTags">if set to <c>true</c> [add script tags].</param>
        public static void RegisterClientScriptBlock(
            Page page, string key, string script, bool addScriptTags) {

            Type type = typeof(ClientScriptHelper);
            if (IsAjaxEnabled(page))
                _RegisterClientScriptBlock.Invoke(null, new object[] { page, type, key, script, addScriptTags });
            else if (!page.ClientScript.IsClientScriptBlockRegistered(type, key))
                page.ClientScript.RegisterClientScriptBlock(type, key, script, addScriptTags);
        }

        /// <summary>
        /// Registers the client script include.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="type">The type.</param>
        /// <param name="key">The key.</param>
        /// <param name="url">The URL.</param>
        public static void RegisterClientScriptInclude(Page page, string key, string url) {

            Type type = typeof(ClientScriptHelper);
            if (IsAjaxEnabled(page))
                _RegisterClientScriptInclude.Invoke(null, new object[] { page, type, key, url });
            else if (!page.ClientScript.IsClientScriptIncludeRegistered(type, key))
                page.ClientScript.RegisterClientScriptInclude(type, key, url);
        }

        /// <summary>
        /// Registers the client script resource.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="type">The type.</param>
        /// <param name="resourceName">Name of the resource.</param>
        public static void RegisterClientScriptResource(Page page, string resourceName) {

            Type type = typeof(ClientScriptHelper);
            if (IsAjaxEnabled(page))
                _RegisterClientScriptResource.Invoke(null, new object[] { page, type, resourceName });
            else
                page.ClientScript.RegisterClientScriptResource(type, resourceName);
        }

        /// <summary>
        /// Registers the expando attribute.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="controlId">The control id.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="attributeValue">The attribute value.</param>
        /// <param name="encode">if set to <c>true</c> [encode].</param>
        public static void RegisterExpandoAttribute(Control control, string controlId, string attributeName, string attributeValue, bool encode) {

            if (IsAjaxEnabled(control.Page))
                _RegisterExpandoAttribute.Invoke(null, new object[] { control, controlId, attributeName, attributeValue, encode });
            else
                control.Page.ClientScript.RegisterExpandoAttribute(controlId, attributeName, attributeValue, encode);
        }

        /// <summary>
        /// Registers the hidden field.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public static void RegisterHiddenField(Page page, string name, string value) {

            if (IsAjaxEnabled(page))
                _RegisterHiddenField.Invoke(null, new object[] { page, name, value });
            else
                page.ClientScript.RegisterHiddenField(name, value);
        }

        /// <summary>
        /// Registers the on submit statement.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="type">The type.</param>
        /// <param name="key">The key.</param>
        /// <param name="script">The script.</param>
        public static void RegisterOnSubmitStatement(Page page, string key, string script) {

            Type type = typeof(ClientScriptHelper);
            if (IsAjaxEnabled(page))
                _RegisterOnSubmitStatement.Invoke(null, new object[] { page, type, key, script });
            else if (!page.ClientScript.IsOnSubmitStatementRegistered(type, key))
                page.ClientScript.RegisterOnSubmitStatement(type, key, script);
        }

        /// <summary>
        /// Registers the startup script.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="type">The type.</param>
        /// <param name="key">The key.</param>
        /// <param name="script">The script.</param>
        /// <param name="addScriptTags">if set to <c>true</c> [add script tags].</param>
        public static void RegisterStartupScript(Page page, string key, string script, bool addScriptTags) {

            Type type = typeof(ClientScriptHelper);
            if (IsAjaxEnabled(page))
                _RegisterStartupScript.Invoke(null, new object[] { page, type, key, script, addScriptTags });
            else if (!page.ClientScript.IsStartupScriptRegistered(type, key))
                page.ClientScript.RegisterStartupScript(type, key, script, addScriptTags);
        }
        #endregion

        #region Fields  /////////////////////////////////////////////////////////////////

        Control _control;

        #endregion

        #region Construct  //////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientScriptHelper"/> class.
        /// </summary>
        /// <param name="control">The control.</param>
        public ClientScriptHelper(Control control) {
            if (control == null)
                throw new ArgumentNullException("control");
            _control = control;
        }
        #endregion

        #region Methods /////////////////////////////////////////////////////////////////

        /// <summary>
        /// Determines whether [is ajax enabled].
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [is ajax enabled]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAjaxEnabled() {
            return IsAjaxEnabled(_control.Page);
        }

        /// <summary>
        /// Determines whether [is ajax post back] [the specified page].
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns>
        /// 	<c>true</c> if [is ajax post back] [the specified page]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAjaxPostBack() {
            return IsAjaxPostBack(_control.Page);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is inside update panel.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is inside update panel; otherwise, <c>false</c>.
        /// </value>
        public bool IsInsideUpdatePanel() {
            return IsInsideUpdatePanel(_control);
        }

        /// <summary>
        /// Gets the current scrip manager.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        public object GetCurrentScripManager() {
            return GetCurrentScripManager(_control.Page);
        }

        /// <summary>
        /// Registers the array declaration.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public void RegisterArrayDeclaration(string name, string value) {
            RegisterArrayDeclaration(_control.Page, name, value);
        }

        /// <summary>
        /// Registers the client script block.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="script">The script.</param>
        /// <param name="addScriptTags">if set to <c>true</c> [add script tags].</param>
        public void RegisterClientScriptBlock(string key, string script, bool addScriptTags) {
            RegisterClientScriptBlock(_control.Page, key, script, addScriptTags);
        }

        /// <summary>
        /// Registers the client script include.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="url">The URL.</param>
        public void RegisterClientScriptInclude(string key, string url) {
            RegisterClientScriptInclude(_control.Page, key, url);
        }

        /// <summary>
        /// Registers the client script resource.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        public void RegisterClientScriptResource(string resourceName) {
            RegisterClientScriptResource(_control.Page, resourceName);
        }

        /// <summary>
        /// Registers the expando attribute.
        /// </summary>
        /// <param name="controlId">The control id.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="attributeValue">The attribute value.</param>
        /// <param name="encode">if set to <c>true</c> [encode].</param>
        public void RegisterExpandoAttribute(string controlId, string attributeName, string attributeValue, bool encode) {
            RegisterExpandoAttribute(_control, controlId, attributeName, attributeValue, encode);
        }

        /// <summary>
        /// Registers the hidden field.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public void RegisterHiddenField(string name, string value) {
            RegisterHiddenField(_control.Page, name, value);
        }

        /// <summary>
        /// Registers the on submit statement.
        /// </summary>      
        /// <param name="key">The key.</param>
        /// <param name="script">The script.</param>
        public void RegisterOnSubmitStatement(string key, string script) {
            RegisterOnSubmitStatement(_control.Page, key, script);
        }

        /// <summary>
        /// Registers the startup script.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="script">The script.</param>
        /// <param name="addScriptTags">if set to <c>true</c> [add script tags].</param>
        public void RegisterStartupScript(string key, string script, bool addScriptTags) {
            RegisterStartupScript(_control.Page, key, script, addScriptTags);
        }
        #endregion
    }
}
