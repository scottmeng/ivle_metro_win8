using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace icreate_test2.DataStructure
{
    [DataContract]
    class ForumWrapper
    {
        [DataMember(Name = "Results")]
        public List<Forum> forums { get; set; }

        [DataMember(Name = "Comments")]
        public string comments { get; set; }

        public ForumWrapper(List<Forum> fs, String cs)
        {
            forums = fs;
            comments = cs;
        }
    }
}
