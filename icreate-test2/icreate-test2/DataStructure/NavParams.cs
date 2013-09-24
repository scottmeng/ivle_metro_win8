using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.IO;

namespace icreate_test2.DataStructure
{
    [DataContract]
    class NavParams
    {
        [DataMember(Name = "moduleIndex")]
        public int moduleIndex { get; set; }
        
        [DataMember(Name = "announcementIndex")]
        public int announcementIndex { get; set; }

        public NavParams(int mi, int ai)
        {
            this.moduleIndex = mi;
            this.announcementIndex = ai;
        }
    }
}
