using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace icreate_test2.DataStructure
{
    [DataContract]
    class WorkbinWrapper
    {
        [DataMember(Name = "Results")]
        public Workbin[] workbins { get; set; }

        [DataMember(Name = "Comments")]
        public string comments { get; set; }

        public WorkbinWrapper(Workbin[] ws, String cs)
        {
            workbins = ws;
            comments = cs;
        }
    }
}
