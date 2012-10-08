using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

[assembly: WebResource("Artem.Google.Scripts.GoogleMap.js", "text/javascript")]

namespace Artem.Google.UI
{
    public class TestMap : ScriptControl
    {
        protected override IEnumerable<ScriptDescriptor> GetScriptDescriptors()
        {
            yield return new ScriptControlDescriptor("Artem.Google.TestMap", this.ClientID);
        }

        protected override IEnumerable<ScriptReference> GetScriptReferences()
        {
            string assembly = this.GetType().Assembly.FullName;
            yield return new ScriptReference("Artem.Google.Scripts.GoogleMap.js", assembly);
        }
    }
}
