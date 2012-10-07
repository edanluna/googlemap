using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Collections.Specialized;
using System.Drawing;
using System.Web;

namespace Artem.Web.UI.Controls {

    /// <summary>
    /// 
    /// </summary>
    public class JsonSerializer<T> {

        #region Static Methods //////////////////////////////////////////////////////////

        /// <summary>
        /// Serializes a value into a JSON compatible string.
        /// The serializer supports:
        /// &lt;&lt;ul&gt;&gt;
        /// &lt;&lt;li&gt;&gt; All simple types
        /// &lt;&lt;li&gt;&gt; POCO objects and Hierarchical POCO objects
        /// &lt;&lt;li&gt;&gt; Arrays
        /// &lt;&lt;li&gt;&gt; IList based collections
        /// &lt;&lt;li&gt;&gt; DataSet
        /// &lt;&lt;li&gt;&gt; DataTable
        /// &lt;&lt;li&gt;&gt; DataRow
        /// &lt;&lt;/ul&gt;&gt;
        /// <seealso>Class JSONSerializer</seealso>
        /// </summary>
        /// <param name="Value">
        /// The strongly typed value to parse
        /// </param>
        /// <returns>string</returns>
        public static string Serialize(T value) {
            StringBuilder sb = new StringBuilder();
            WriteObject(sb, value);
            return sb.ToString();
        }

        /// <summary>
        /// Takes a JSON string and attempts to create a .NET object from this 
        /// structure. An input type is required and any type that is serialized to 
        /// must support a parameterless constructor.
        /// 
        /// The de-serializer instantiates each object and runs through the properties
        /// 
        /// The deserializer supports
        /// &lt;&lt;ul&gt;&gt;
        /// &lt;&lt;li&gt;&gt; All simple types
        /// &lt;&lt;li&gt;&gt; Most POCO objects and Hierarchical POCO objects
        /// &lt;&lt;li&gt;&gt; Arrays and Object Arrays
        /// &lt;&lt;li&gt;&gt; IList based collections
        /// &lt;&lt;/ul&gt;&gt;
        /// 
        /// Note that the deserializer doesn't support DataSets/Tables/Rows like the 
        /// serializer as there's no type information available from the client to 
        /// create these objects on the fly.
        /// <seealso>Class JSONSerializer</seealso>
        /// </summary>
        /// <param name="JSONText">
        /// A string of JSON text passed from the client.
        /// </param>
        /// <param name="ValueType">
        /// The type of the object that is to be created from the JSON text.
        /// </param>
        /// <returns>The parsed object or null on failure. An exception is thrown if the type cannot be created</returns>
        public static T Deserialize(string jsonText) {
            return (T)ParseValueString(jsonText, typeof(T));
        }

        /// <summary>
        /// Deserializes the specified json text.
        /// </summary>
        /// <param name="jsonText">The json text.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static T Deserialize(string jsonText, T value) {
            StringReader reader = new StringReader(jsonText);
            return (T)ParseObject(reader, value, false);
        }
        #endregion

        #region Serialization Methods

        /// <summary>
        /// This serioalization code is based on Jason Diamonds JSON parsing
        /// routines part of MyAjax.NET (aka Anthem).
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="val"></param>
        static void WriteValue(StringBuilder sb, object val) {
            if (val == null || val == System.DBNull.Value) {
                sb.Append("null");
            }
            else if (val is string || val is Guid) {
                WriteString(sb, val.ToString());
            }
            else if (val is bool) {
                sb.Append(val.ToString().ToLower());
            }
            else if (val is double ||
                val is float ||
                val is long ||
                val is int ||
                val is short ||
                val is byte ||
                val is decimal) {
                sb.Append(Convert.ToString(val, JsUtil.NumberFormat));
            }
            else if (val.GetType().IsEnum) {
                sb.Append((int)val);
            }
            else if (val is DateTime) {
                sb.Append("new Date(\"");
                sb.Append(((DateTime)val).ToString("U") + " UTC");
                //sb.Append(((DateTime)val).ToString("MMMM, d yyyy HH:mm:ss", new CultureInfo("en-US", false).DateTimeFormat));
                sb.Append("\")");
            }
            else if (val is Color) {
                sb.Append(JsUtil.Encode((Color)val));
            }
            else if (val is IEnumerable) {
                WriteEnumerable(sb, val as IEnumerable);
            }
            else {
                WriteObject(sb, val);
            }
        }

        static void WriteString(StringBuilder sb, string s) {
            sb.Append("\"");
            foreach (char c in s) {
                switch (c) {
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    default:
                        int i = (int)c;
                        if (i < 32 || i > 127) {
                            sb.AppendFormat("\\u{0:X04}", i);
                        }
                        else {
                            sb.Append(c);
                        }
                        break;
                }
            }
            sb.Append("\"");
        }

        static void WriteEnumerable(StringBuilder sb, IEnumerable e) {
            bool hasItems = false;
            sb.Append("[");

            foreach (object val in e) {
                WriteValue(sb, val);
                sb.Append(",");
                hasItems = true;
            }
            // Remove the trailing comma.
            if (hasItems) {
                --sb.Length;
            }
            sb.Append("]");
        }

        static void WriteObject(StringBuilder sb, object o) {
            MemberInfo[] members = o.GetType().GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField | BindingFlags.GetProperty);
            sb.Append("{");
            bool hasMembers = false;
            bool isMarked;
            bool mustEncode;
            JsonDataAttribute[] attrs;
            foreach (MemberInfo member in members) {
                attrs = member.GetCustomAttributes(typeof(JsonDataAttribute), true) as JsonDataAttribute[];
                if (isMarked = (attrs != null && attrs.Length > 0))
                    mustEncode = attrs[0].Encode;
                else
                    mustEncode = false;
                object val = null;
                bool hasValue = GetMemberValue(member, o, out val);
                //if ((member.MemberType & MemberTypes.Field) == MemberTypes.Field) {
                //    FieldInfo field = (FieldInfo)member;
                //    val = field.GetValue(o);
                //    hasValue = true;
                //}
                //else if ((member.MemberType & MemberTypes.Property) == MemberTypes.Property) {
                //    PropertyInfo property = (PropertyInfo)member;
                //    if (property.CanRead && property.GetIndexParameters().Length == 0) {
                //        val = property.GetValue(o, null);
                //        hasValue = true;
                //    }
                //}
                if (hasValue) {
                    if (isMarked) {
                        sb.Append("\"");
                        sb.Append(member.Name);
                        sb.Append("\":");
                        if (mustEncode && val is string)
                            WriteValue(sb, HttpUtility.HtmlEncode(val as string));
                        else
                            WriteValue(sb, val);
                        sb.Append(",");
                        hasMembers = true;
                    }
                    else if (val is IJsonObject) {
                        sb.Append("\"");
                        sb.Append(member.Name);
                        sb.Append("\":");
                        sb.Append(((IJsonObject)val).ToJsonString());
                        sb.Append(",");
                        hasMembers = true;
                    }
                }
            }
            if (hasMembers) {
                --sb.Length;
            }
            sb.Append("}");
        }

        #endregion

        #region Deserialization methods

        /// <summary>
        /// High level parsing method that takes a JSON string and tries to
        /// convert it to the appropriate type. 
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="ValueType"></param>
        /// <returns></returns>
        static object ParseValueString(string value, Type type) {

            if (value != null && !JsUtil.IsUndefined( value )) {
                if (type == typeof(string)) {
                    if (value == "null") return null;
                    //
                    value = value.Replace(@"\r", "\r");
                    value = value.Replace(@"\n", "\n");
                    value = value.Replace(@"\\", "\\");
                    value = value.Replace(@"\""", "\"");
                    value = value.Replace(@"\t", "\t");
                    value = value.Replace(@"\b", "\b");
                    value = value.Replace(@"\f", "\f");
                    // *** Strip off leading and ending quotes
                    return value.Trim('"');
                }
                else if (type == typeof(int)) {
                    return JsUtil.ToInt(value);
                }
                else if (type == typeof(float) || type == typeof(Single)) {
                    return JsUtil.ToSingle(value);
                }
                else if (type == typeof(double)) {
                    return JsUtil.ToDouble(value);
                }
                else if (type == typeof(long)) {
                    return JsUtil.ToLong(value);
                }
                else if (type == typeof(bool)) {
                    return JsUtil.ToBoolean(value);
                }
                else if (type == typeof(short)) {
                    return JsUtil.ToShort(value);
                }
                else if (type == typeof(byte)) {
                    return JsUtil.ToByte(value);
                }
                else if (type == typeof(decimal)) {
                    return JsUtil.ToDecimal(value);
                }
                else if (type == typeof(DateTime)) {
                    if (value == null) return DateTime.MinValue;
                    try {
                        string[] dateParts = value.Split(',');
                        return new DateTime(int.Parse(dateParts[0]),
                                            int.Parse(dateParts[1]) + 1,
                                            int.Parse(dateParts[2]),
                                            int.Parse(dateParts[3]),
                                            int.Parse(dateParts[4]),
                                            int.Parse(dateParts[5]),
                                            DateTimeKind.Utc);
                    }
                    catch {
                        return DateTime.MinValue;
                    }
                }
                else if (type == typeof(Color)) {
                    return JsUtil.ToColor(value);
                }
                else if (type.IsArray || type.GetInterface("IList") != null) {
                    StringReader reader = new StringReader(value);
                    return ParseArray(reader, type);
                }
                else if (type.IsClass) {
                    StringReader reader = new StringReader(value);
                    return ParseObject(reader, type, false);
                }
                else if (type.IsEnum) {
                    return Enum.Parse(type, value);
                }
            }
            return null;
        }

        /// <summary>
        /// Parsing routine specific to parsing an object. Note the recursive flag which 
        /// allows skipping over prefix information.
        /// </summary>
        /// <param name="Reader"></param>
        /// <param name="ValueType"></param>
        /// <param name="RecursiveCall"></param>
        /// <returns></returns>
        static object ParseObject(StringReader reader, Type type, bool recursiveCall) {
            return ParseObject(reader, Activator.CreateInstance(type), recursiveCall);
        }

        /// <summary>
        /// Parses the object.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="resultObject">The result object.</param>
        /// <param name="recursiveCall">if set to <c>true</c> [recursive call].</param>
        /// <returns></returns>
        static object ParseObject(StringReader reader, object obj, bool recursiveCall) {

            int val = 0;
            char lastChar = '|';
            char nextChar = '{';
            string currentProperty = "";
            ParseStates state = ParseStates.None;
            StringBuilder sb = new StringBuilder();
            NameValueCollection properties = new NameValueCollection();
            //
            if (!recursiveCall) {
                reader.Read();
                if (val == -1) return null;
                nextChar = (char)val;
            }
            // parse loop
            while (true) {
                lastChar = nextChar;
                val = reader.Read();
                if (val == -1) break;
                nextChar = (char)val;
                // find property
                if (state == ParseStates.None) {
                    switch (nextChar) {
                        case '}':
                            return obj;
                        case '"':
                            state = ParseStates.InPropertyName;
                            currentProperty = "";
                            break;
                    }
                    continue;
                }
                //if (state == ParseStates.None) continue;
                //
                if (state == ParseStates.InPropertyName && nextChar == '"') {
                    state = ParseStates.InPropertyValueTransition;
                }
                else if (state == ParseStates.InPropertyName) {
                    currentProperty += nextChar;
                }
                else if (state == ParseStates.InPropertyValueTransition && nextChar == ':') {
                    continue;
                }
                else if (state == ParseStates.InPropertyValueTransition && nextChar == '"') {
                    state = ParseStates.InStringValue;
                }
                else if (state == ParseStates.InPropertyValueTransition && nextChar == '[') {
                    MemberInfo[] mi = obj.GetType().GetMember(currentProperty,
                        BindingFlags.Instance | BindingFlags.Instance | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.Public);
                    if (mi == null || mi.Length < 1)
                        AssignProperty(obj, currentProperty, null);
                    else {
                        ParseMemberArray(reader, mi[0], obj);
                        //Type objectType = null;
                        //if (mi[0].MemberType == MemberTypes.Field)
                        //    objectType = ((FieldInfo)mi[0]).FieldType;
                        //else
                        //    objectType = ((PropertyInfo)mi[0]).PropertyType;
                        ////
                        //object result = ParseArray(reader, objectType);
                        ////
                        //if (mi[0].MemberType == MemberTypes.Field)
                        //    ((FieldInfo)mi[0]).SetValue(obj, result);
                        //else
                        //    ((PropertyInfo)mi[0]).SetValue(obj, result, null);
                        //
                        nextChar = ']';
                    }
                    state = ParseStates.None;
                }
                // *** Nested Object - recursively read characters
                else if (state == ParseStates.InPropertyValueTransition && nextChar == '{') {
                    state = ParseStates.InObject;
                    MemberInfo[] mi = obj.GetType().GetMember(currentProperty,
                        BindingFlags.Instance | BindingFlags.Instance | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.Public);
                    if (mi == null || mi.Length < 1)
                        AssignProperty(obj, currentProperty, null);
                    else {
                        Type objectType = null;
                        if (mi[0].MemberType == MemberTypes.Field)
                            objectType = ((FieldInfo)mi[0]).FieldType;
                        else
                            objectType = ((PropertyInfo)mi[0]).PropertyType;
                        //
                        object result = ParseObject(reader, objectType, true);
                        //
                        if (mi[0].MemberType == MemberTypes.Field)
                            ((FieldInfo)mi[0]).SetValue(obj, result);
                        else
                            ((PropertyInfo)mi[0]).SetValue(obj, result, null);
                        //
                        nextChar = '}';
                    }
                    state = ParseStates.None;
                }
                else if (state == ParseStates.InPropertyValueTransition) {
                    state = ParseStates.InValue;
                    sb.Append(nextChar);
                }
                else if (state == ParseStates.InStringValue) {
                    if (nextChar == '"' && lastChar != '\'') {
                        state = ParseStates.None;
                        AssignProperty(obj, currentProperty, sb.ToString());
                        sb.Length = 0;
                    }
                    else
                        sb.Append(nextChar);
                }
                else if (state == ParseStates.InValue && nextChar == '}') {
                    AssignProperty(obj, currentProperty, sb.ToString());
                    sb.Length = 0;
                    state = ParseStates.EndOfObject;
                    return obj;
                }
                else if (state == ParseStates.InValue && nextChar == ',') {
                    if (reader.Peek() != '"')
                        // *** check if hte next character is a property delimiter
                        // *** if not it's a , in the value like in dates or decimals
                        sb.Append(nextChar);
                    else {
                        AssignProperty(obj, currentProperty, sb.ToString());
                        sb.Length = 0;
                        state = ParseStates.None;
                    }
                }
                else if (state == ParseStates.InValue) {
                    sb.Append(nextChar);
                }
                else if (state == ParseStates.EndOfObject || nextChar == '}') {
                    return obj;
                }
            }
            return obj;
        }

        /// <summary>
        /// Parses a array subtype 
        /// </summary>
        /// <param name="Reader"></param>
        /// <param name="ArrayType"></param>
        /// <returns></returns>
        static object ParseArray(StringReader reader, Type arrayType) {
            // *** Retrieve the type of child elements - Can be Array, Collection etc.
            Type elementType = GetArrayType(arrayType);

            // *** Start by parsing each of the items
            //ArrayList items = new ArrayList();
            IList items = Activator.CreateInstance(arrayType) as IList;

            char nextChar = '[';
            char lastChar = '|';
            ParseStates state = ParseStates.None;

            while (true) {
                lastChar = nextChar;
                int val = reader.Read();
                if (val == -1) break;
                nextChar = (char)val;
                // *** Adding an object
                if ((state == ParseStates.None || state == ParseStates.InPropertyValueTransition) && nextChar == '{') {
                    object parsedObject = ParseObject(reader, elementType, true);
                    items.Add(parsedObject);
                    state = ParseStates.InPropertyValueTransition;
                }
                else if ((state == ParseStates.InPropertyValueTransition || state == ParseStates.None) && nextChar == ']') {
                    if (arrayType.IsArray) {
                        Array elementArray = Activator.CreateInstance(arrayType, items.Count) as Array;
                        for (int i = 0; i < items.Count; i++) {
                            object item = Activator.CreateInstance(elementType);
                            elementArray.SetValue(items[i], i);
                        }
                        return elementArray;
                    }
                    else if (arrayType.GetInterface("IList") != null) {
                        //return col;
                        break;
                    }
                }
            }
            return items;
        }

        /// <summary>
        /// Parses the array.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="array">The array.</param>
        static object ParseArray(StringReader reader, IList list) {

            Type arrayType = list.GetType();
            Type elementType = GetArrayType(list.GetType());
            char nextChar = '[';
            char lastChar = '|';
            ParseStates state = ParseStates.None;
            object parsedObject;

            int i = 0;
            while (true) {
                lastChar = nextChar;
                int val = reader.Read();
                if (val == -1) break;
                nextChar = (char)val;
                // *** Setting an object
                if ((state == ParseStates.None || state == ParseStates.InPropertyValueTransition) && nextChar == '{') {
                    if (i < list.Count) {
                        parsedObject = ParseObject(reader, list[i++], true);
                    }
                    else {
                        list.Add(ParseObject(reader, elementType, true));
                        i++;
                    }
                    state = ParseStates.InPropertyValueTransition;
                }
                else if ((state == ParseStates.InPropertyValueTransition || state == ParseStates.None) && nextChar == ']') {
                    break;
                }
            }
            return list;
        }

        /// <summary>
        /// Returns the type of item type of the array/collection
        /// </summary>
        /// <param name="ArrayType"></param>
        /// <returns></returns>
        static Type GetArrayType(Type arrayType) {

            if (arrayType.IsArray)
                return arrayType.GetElementType();

            if (arrayType == typeof(DataTable))
                return typeof(DataRow);

            if (arrayType.GetInterface("IList") != null) {
                MethodInfo method = arrayType.GetMethod("Add");
                ParameterInfo parameter = method.GetParameters()[0];
                Type resultType = parameter.ParameterType;
                return resultType;
            }

            return null;
        }

        /// <summary>
        /// Assigns the property.
        /// </summary>
        /// <param name="resultObject">The result object.</param>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        static void AssignProperty(object resultObject, string property, string value) {
            MemberInfo[] mi = resultObject.GetType().GetMember(property,
                                                    BindingFlags.Instance |
                                                    BindingFlags.GetField | BindingFlags.GetProperty |
                                                    BindingFlags.IgnoreCase | BindingFlags.Public);
            if (mi == null || mi.Length < 1)
                return;

            object objValue = ParseValueString(value, GetMemberType(mi[0]));
            SetMemberValue(mi[0], resultObject, objValue);

            //if (mi[0].MemberType == MemberTypes.Field) {
            //    FieldInfo fInfo = mi[0] as FieldInfo;
            //    objValue = ParseValueString(value, fInfo.FieldType);
            //    if (objValue != null)
            //        fInfo.SetValue(resultObject, objValue);
            //}
            //else {
            //    PropertyInfo pInfo = mi[0] as PropertyInfo;
            //    objValue = ParseValueString(value, pInfo.PropertyType);
            //    if (objValue != null)
            //        pInfo.SetValue(resultObject, objValue, null);
            //}
        }

        #region - Members Parse -

        /// <summary>
        /// Parses the member array.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="member">The member.</param>
        /// <param name="obj">The obj.</param>
        static void ParseMemberArray(StringReader reader, MemberInfo member, object obj) {

            object value = null;
            if (GetMemberValue(member, obj, out value) && value != null && value is IList) {
                ParseArray(reader, value as IList);
            }
            else {
                Type objectType = GetMemberType(member);
                object result = ParseArray(reader, objectType);
                SetMemberValue(member, obj, result);
            }
        }

        static void ParseMemberObject() {
        }
        #endregion

        #region - Members Utility -

        /// <summary>
        /// Gets the type of the member.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <returns></returns>
        static Type GetMemberType(MemberInfo member) {

            if ((member.MemberType & MemberTypes.Field) == MemberTypes.Field) {
                return ((FieldInfo)member).FieldType;
            }
            else {
                return ((PropertyInfo)member).PropertyType;
            }
        }

        /// <summary>
        /// Gets the member value.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="obj">The obj.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        static bool GetMemberValue(MemberInfo member, object obj, out object value) {

            if ((member.MemberType & MemberTypes.Field) == MemberTypes.Field) {
                value = ((FieldInfo)member).GetValue(obj);
                return true;
            }
            else if ((member.MemberType & MemberTypes.Property) == MemberTypes.Property) {
                PropertyInfo property = (PropertyInfo)member;
                if (property.CanRead && property.GetIndexParameters().Length == 0) {
                    value = property.GetValue(obj, null);
                    return true;
                }
            }

            value = null;
            return false;
        }

        /// <summary>
        /// Sets the member value.
        /// </summary>
        /// <param name="member">The member.</param>
        static void SetMemberValue(MemberInfo member, object obj, object value) {

            if (value != null) {
                if ((member.MemberType & MemberTypes.Field) == MemberTypes.Field) {
                    ((FieldInfo)member).SetValue(obj, value);
                }
                else if ((member.MemberType & MemberTypes.Property) == MemberTypes.Property) {
                    ((PropertyInfo)member).SetValue(obj, value, null);
                }
            }
        }
        #endregion
        #endregion

        #region Nested Types ////////////////////////////////////////////////////////////

        private enum ParseStates {
            None,
            InPropertyName,
            InPropertyValueTransition,
            InStringValue,
            InValue,
            InDate,
            InObject,
            EndOfObject
        }
        #endregion
    }
}
