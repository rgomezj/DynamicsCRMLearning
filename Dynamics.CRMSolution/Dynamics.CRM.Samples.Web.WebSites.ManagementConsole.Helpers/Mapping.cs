using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Pavliks.WAM.ManagementConsole.Domain;
using Pavliks.WAM.ManagementConsole.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pavliks.WAM.ManagementConsole.Helpers
{
    public class Mapping
    {
        public static string GetAttributeName(Entity entity, string attributeSearch)
        {
            foreach(KeyValuePair<string, object> attribute in entity.Attributes)
            {
                if(attribute.Key.ToUpper().Equals(attributeSearch.ToUpper()))
                {
                    return attribute.Key;
                }
            }
            return string.Empty;
        }
    }
}
