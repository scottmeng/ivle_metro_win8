using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace icreate_test2.DataStructure
{
    [DataContract]
    class Webcast
    {
        [DataMember(Name = "ID")]
        public String webcastId { get; set; }

        [DataMember(Name = "Title")]
        public String webcastTitle { get; set; }

        [DataMember(Name = "Creator")]
        public Member webcastCreator { get; set; }

        public Webcast(String id, String title, Member creator)
        {
            webcastId = id;
            webcastTitle = title;
            webcastCreator = creator;
        }
    }
}
