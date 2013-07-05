using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace icreate_test2.DataStructure
{
    [DataContract]
    class ExamInfoWrapper
    {
        [DataMember(Name = "Results")]
        public ExamInfo[] examInfos { get; set; }

        [DataMember(Name = "Comments")]
        public string comments { get; set; }

        public ExamInfoWrapper(ExamInfo[] infos, string myComments)
        {
            examInfos = infos;
            comments = myComments;
        }
    }
}
