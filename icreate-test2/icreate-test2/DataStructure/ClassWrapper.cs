using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace icreate_test2.DataStructure
{
    [DataContract]
    class ClassWrapper
    {
        [DataMember(Name = "Results")]
        public Class[] classes { get; set; }

        [DataMember(Name = "Comments")]
        public string comments { get; set; }

        public ClassWrapper(Class[] myClasses, string myComments)
        {
            classes = myClasses;
            comments = myComments;
        }
    }
}
