using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace icreate_test2.DataStructure
{
    [DataContract]
    class WebcastWrapper
    {
        [DataMember(Name = "Results")]
        public Webcast[] webcasts { get; set; }

        [DataMember(Name = "Comments")]
        public string comments { get; set; }

        public WebcastWrapper(Webcast[] ws, String cs)
        {
            webcasts = ws;
            comments = cs;
        }
    }
}
