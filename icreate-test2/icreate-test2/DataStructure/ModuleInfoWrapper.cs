using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace icreate_test2.DataStructure
{
    [DataContract]
    class ModuleInfoWrapper
    {
        [DataMember(Name = "Results")]
        public List<Module> modules { get; set; }

        [DataMember(Name = "Comments")]
        public string comments { get; set; }

        public ModuleInfoWrapper(List<Module> ms, String cs)
        {
            modules = ms;
            comments = cs;
        }
    }
}
