using System.Collections.Generic;
using doob.Reflectensions;

namespace doob.Who.Ldap
{
    public class LdapObject: ExpandableObject
    {
        public string DistinguishedName { get; set; }

        public string[] ObjectClass { get; set; }


        public LdapObject(Dictionary<string, object> properties): base(properties)
        {
            
        }


      
    }
}
