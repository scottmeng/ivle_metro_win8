using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace icreate_test2.DataStructure
{
    [DataContract]
    class ThreadWrapper
    {
        [DataMember(Name = "Results")]
        public Thread[] threads { get; set; }

        [DataMember(Name = "Comments")]
        public string comments { get; set; }

        public ThreadWrapper(Thread[] ts, String cs)
        {
            threads = ts;
            comments = cs;
        }
    }
}
